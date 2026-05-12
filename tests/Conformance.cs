using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

// State-aware conformance test for the .NET SDK against the real dev API.
// Mirrors tests/runners/node/conformance.ts (same scenario IDs).
public static class Conformance
{
    private static string RequireEnv(string key)
    {
        var v = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(v))
        {
            Console.Error.WriteLine($"FATAL: {key} is not set in .env.dev");
            Environment.Exit(2);
        }
        return v!;
    }

    private static string EnvOr(string key, string fallback)
    {
        var v = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(v) ? fallback : v!;
    }

    private static readonly List<(string id, string name, string status, string detail)> results = new();

    private static void Scenario(string id, string name, Func<(bool ok, string detail)> body)
    {
        try
        {
            var (ok, detail) = body();
            results.Add((id, name, ok ? "PASS" : "FAIL", detail));
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
            if (msg.Length > 160) msg = msg.Substring(0, 160);
            results.Add((id, name, "FAIL", $"unexpected: {ex.GetType().Name}: {msg}"));
        }
    }

    private static void Skip(string id, string name) =>
        results.Add((id, name, "SKIP", "destructive — set ALLOW_DESTRUCTIVE=1"));

    private static int HttpStatus(Exception ex)
    {
        // ApiException is the public exception type for restsharp template.
        var prop = ex.GetType().GetProperty("ErrorCode");
        if (prop != null && prop.GetValue(ex) is int code) return code;
        prop = ex.GetType().GetProperty("Status");
        if (prop != null && prop.GetValue(ex) is int s) return s;
        return 0;
    }

    private static string BodySnippet(Exception ex)
    {
        var prop = ex.GetType().GetProperty("ErrorContent");
        var body = prop?.GetValue(ex)?.ToString() ?? ex.Message;
        return body.Length > 160 ? body.Substring(0, 160) : body;
    }

    public static void Run()
    {
        var baseUrl = RequireEnv("BASE_URL");
        var apiKey = RequireEnv("GOODSENDER_API_KEY");
        var allowDestructive = Environment.GetEnvironmentVariable("ALLOW_DESTRUCTIVE") == "1";

        var verifiedDomain = RequireEnv("VERIFIED_SENDER_DOMAIN");
        var verifiedEmail = RequireEnv("VERIFIED_SENDER_EMAIL");
        var verifiedName = EnvOr("VERIFIED_SENDER_NAME", "GoodSender SDK Tests");
        var unverifiedDomain = RequireEnv("UNVERIFIED_SENDER_DOMAIN");
        var unverifiedEmail = RequireEnv("UNVERIFIED_SENDER_EMAIL");
        var granted1 = RequireEnv("RECIPIENT_GRANTED_1");
        var granted2 = RequireEnv("RECIPIENT_GRANTED_2");
        var denied1 = RequireEnv("RECIPIENT_DENIED_1");
        var denied2 = RequireEnv("RECIPIENT_DENIED_2");
        var templateId = RequireEnv("TEMPLATE_ID");

        var bytes = RandomNumberGenerator.GetBytes(3);
        var runTag = $"sdk-{DateTimeOffset.UtcNow.ToUnixTimeSeconds():x}-{Convert.ToHexString(bytes).ToLowerInvariant()}";
        var fresh1 = $"{runTag}-1@{verifiedDomain}";
        var fresh2 = $"{runTag}-2@{verifiedDomain}";

        var config = new Configuration { BasePath = baseUrl, AccessToken = apiKey };
        var emails = new EmailsApi(config);
        var domains = new DomainsApi(config);

        // ─── Read-only (R1–R6) ────────────────────────────────────

        Scenario("R1", "listDomains returns both fixtures with correct verification flags", () =>
        {
            var res = domains.ListDomains(limit: 100);
            // The Domain model's `domain` field is renamed to VarDomain to avoid
            // collision with the enclosing class name.
            var byName = res.Domains.ToDictionary(d => d.VarDomain, d => d);
            if (!byName.TryGetValue(verifiedDomain, out var v)) return (false, $"{verifiedDomain} not in listDomains response");
            if (!byName.TryGetValue(unverifiedDomain, out var u)) return (false, $"{unverifiedDomain} not in listDomains response");
            if (!v.Verification.Verified) return (false, $"{verifiedDomain} has verification.verified=false; should be true");
            if (u.Verification.Verified) return (false, $"{unverifiedDomain} has verification.verified=true; should be false");
            return (true, $"domains={res.Domains.Count}, verified=true, unverified=false");
        });

        Scenario("R2", "getEmailConsentStatus returns granted for approved recipient", () =>
        {
            var res = emails.GetEmailConsentStatus(granted1, domain: verifiedDomain);
            var entry = res.FirstOrDefault(e => e.Domain == verifiedDomain);
            if (entry == null) return (false, $"no entry for domain={verifiedDomain}");
            var cs = entry.ConsentStatus.ToString().ToLowerInvariant();
            if (cs != "granted") return (false, $"consentStatus={cs}, expected granted");
            return (true, $"consentStatus={cs}");
        });

        Scenario("R3", "getEmailConsentStatus returns denied for rejected recipient", () =>
        {
            var res = emails.GetEmailConsentStatus(denied1, domain: verifiedDomain);
            var entry = res.FirstOrDefault(e => e.Domain == verifiedDomain);
            if (entry == null) return (false, $"no entry for domain={verifiedDomain}");
            var cs = entry.ConsentStatus.ToString().ToLowerInvariant();
            if (cs != "denied") return (false, $"consentStatus={cs}, expected denied");
            return (true, $"consentStatus={cs}");
        });

        Scenario("R4", "getEmailConsentStatus returns 404 for unknown recipient", () =>
        {
            var probe = $"{runTag}-r4-probe@{verifiedDomain}";
            try
            {
                var res = emails.GetEmailConsentStatus(probe, domain: verifiedDomain);
                return (false, $"expected 404, got 200 with {res.Count} entries");
            }
            catch (Exception ex)
            {
                var code = HttpStatus(ex);
                if (code == 404) return (true, $"404 (probe={probe})");
                return (false, $"expected 404, got {code} {BodySnippet(ex)}");
            }
        });

        Scenario("R5", "listEmailConsents for verified domain includes all 4 fixtures", () =>
        {
            var collected = new List<string>();
            string? cursor = null;
            for (var p = 0; p < 20; p++)
            {
                var res = emails.ListEmailConsents(domain: verifiedDomain, limit: 100, cursor: cursor);
                if (res.Emails != null) collected.AddRange(res.Emails.Select(e => e.Email));
                cursor = res.NextCursor;
                if (string.IsNullOrEmpty(cursor)) break;
            }
            var missing = new[] { granted1, granted2, denied1, denied2 }.Where(e => !collected.Contains(e)).ToList();
            if (missing.Any()) return (false, $"missing from listEmailConsents: {string.Join(", ", missing)}");
            return (true, $"{collected.Count} entries scanned; all 4 fixtures present");
        });

        Scenario("R6", "listEmailConsents with consentStatus=granted filter excludes denied", () =>
        {
            var collected = new HashSet<string>();
            var statuses = new HashSet<string>();
            string? cursor = null;
            var pages = 0;
            for (var p = 0; p < 20; p++)
            {
                var res = emails.ListEmailConsents(domain: verifiedDomain, limit: 100, cursor: cursor, consentStatus: "granted");
                pages++;
                if (res.Emails != null)
                    foreach (var e in res.Emails) { collected.Add(e.Email); statuses.Add(e.ConsentStatus.ToString().ToLowerInvariant()); }
                cursor = res.NextCursor;
                if (string.IsNullOrEmpty(cursor)) break;
            }
            var nonGranted = statuses.Where(s => s != "granted").OrderBy(s => s).ToList();
            if (nonGranted.Any()) return (false, $"filter leaked non-granted statuses: {string.Join(",", nonGranted)}");
            var missing = new[] { granted1, granted2 }.Where(e => !collected.Contains(e)).ToList();
            if (missing.Any())
            {
                var sample = collected.Take(5).ToList();
                var sampleStr = sample.Count == 0 ? "(none)" : string.Join(", ", sample);
                return (false, $"filter returned {collected.Count} entries across {pages} page(s); missing={string.Join(",", missing)}; sample=[{sampleStr}]");
            }
            if (collected.Contains(denied1) || collected.Contains(denied2)) return (false, "denied fixtures leaked into granted filter");
            return (true, $"{collected.Count} granted entries; denied fixtures absent");
        });

        // ─── Destructive (D1–D6, E1–E5) ────────────────────────────

        if (allowDestructive)
        {
            Scenario("D1", "sendEmail to 2 granted recipients delivers both", () =>
            {
                var req = new SendEmailRequest(new List<SendEmail> { new SendEmail(
                    from: new Address(email: verifiedEmail, name: verifiedName),
                    to: new List<Address> { new Address(email: granted1), new Address(email: granted2) },
                    subject: $"SDK conformance D1 {runTag}",
                    textContent: "D1 — granted recipients") });
                var res = emails.SendEmail(req);
                if (res.Sent == 2 && res.Declined == 0) return (true, $"sent={res.Sent} declined={res.Declined}");
                return (false, $"sent={res.Sent} declined={res.Declined}, expected 2/0");
            });

            Scenario("D2", "sendEmail to 2 denied recipients declines both", () =>
            {
                var req = new SendEmailRequest(new List<SendEmail> { new SendEmail(
                    from: new Address(email: verifiedEmail, name: verifiedName),
                    to: new List<Address> { new Address(email: denied1), new Address(email: denied2) },
                    subject: $"SDK conformance D2 {runTag}",
                    textContent: "D2") });
                var res = emails.SendEmail(req);
                if (res.Sent == 0 && res.Declined == 2) return (true, $"sent={res.Sent} declined={res.Declined}");
                return (false, $"sent={res.Sent} declined={res.Declined}, expected 0/2");
            });

            Scenario("D3", "sendEmail granted+denied mix splits correctly", () =>
            {
                var req = new SendEmailRequest(new List<SendEmail> { new SendEmail(
                    from: new Address(email: verifiedEmail, name: verifiedName),
                    to: new List<Address> { new Address(email: granted1), new Address(email: denied1) },
                    subject: $"SDK conformance D3 {runTag}",
                    textContent: "D3") });
                var res = emails.SendEmail(req);
                if (res.Sent == 1 && res.Declined == 1) return (true, $"sent={res.Sent} declined={res.Declined}");
                return (false, $"sent={res.Sent} declined={res.Declined}, expected 1/1");
            });

            Scenario("D4", "sendTemplateEmail to granted recipient returns status=sent", () =>
            {
                var req = new TemplateEmailRequest(
                    from: new Address(email: verifiedEmail, name: verifiedName),
                    to: new Address(email: granted1),
                    subject: $"SDK conformance D4 {runTag}",
                    template: new TemplateEmailRequestTemplate(templateId: templateId, variables: new Dictionary<string, string>()));
                var res = emails.SendTemplateEmail(req);
                var s = res.Status.ToString().ToLowerInvariant();
                return s == "sent" ? (true, $"status={s}") : (false, $"status={s}, expected sent");
            });

            Scenario("D5", "sendTemplateEmail to denied recipient returns status=declined", () =>
            {
                var req = new TemplateEmailRequest(
                    from: new Address(email: verifiedEmail, name: verifiedName),
                    to: new Address(email: denied1),
                    subject: $"SDK conformance D5 {runTag}",
                    template: new TemplateEmailRequestTemplate(templateId: templateId, variables: new Dictionary<string, string>()));
                var res = emails.SendTemplateEmail(req);
                var s = res.Status.ToString().ToLowerInvariant();
                return s == "declined" ? (true, $"status={s}") : (false, $"status={s}, expected declined");
            });

            Scenario("D6", "requestEmailConsent registers 2 fresh addresses", () =>
            {
                var req = new ConsentEmailRequest(
                    domain: verifiedDomain,
                    emails: new List<ConsentEmailEntry>
                    {
                        new ConsentEmailEntry(new ConsentEmailRecipient(email: fresh1, name: "Fresh 1")),
                        new ConsentEmailEntry(new ConsentEmailRecipient(email: fresh2, name: "Fresh 2")),
                    });
                var res = emails.RequestEmailConsent(req);
                var n = res.Emails?.Count ?? 0;
                if (n != 2) return (false, $"expected 2 entries, got {n}");
                var statuses = res.Emails!.Select(e => e.ConsentStatus.ToString().ToLowerInvariant()).OrderBy(s => s).ToList();
                return (true, $"2 fresh addresses; statuses=[{string.Join(",", statuses)}] {fresh1} {fresh2}");
            });

            Scenario("E1", "sendEmail from unverified domain is rejected", () =>
            {
                try
                {
                    var req = new SendEmailRequest(new List<SendEmail> { new SendEmail(
                        from: new Address(email: unverifiedEmail),
                        to: new List<Address> { new Address(email: granted1) },
                        subject: $"SDK conformance E1 {runTag}",
                        textContent: "should be rejected") });
                    var res = emails.SendEmail(req);
                    return (false, $"expected 4xx, got 200 sent={res.Sent}");
                }
                catch (Exception ex)
                {
                    var code = HttpStatus(ex);
                    if (code >= 400 && code < 500) return (true, $"{code} {BodySnippet(ex)}");
                    return (false, $"expected 4xx, got {code} {BodySnippet(ex)}");
                }
            });

            Scenario("E2", "sendTemplateEmail from unverified domain is rejected", () =>
            {
                try
                {
                    var req = new TemplateEmailRequest(
                        from: new Address(email: unverifiedEmail),
                        to: new Address(email: granted1),
                        subject: $"SDK conformance E2 {runTag}",
                        template: new TemplateEmailRequestTemplate(templateId: templateId, variables: new Dictionary<string, string>()));
                    var res = emails.SendTemplateEmail(req);
                    return (false, $"expected 4xx, got 200 status={res.Status}");
                }
                catch (Exception ex)
                {
                    var code = HttpStatus(ex);
                    if (code >= 400 && code < 500) return (true, $"{code} {BodySnippet(ex)}");
                    return (false, $"expected 4xx, got {code} {BodySnippet(ex)}");
                }
            });

            Scenario("E3", "sendTemplateEmail with bogus template_id returns 404", () =>
            {
                var bad = $"{runTag}-does-not-exist";
                try
                {
                    var req = new TemplateEmailRequest(
                        from: new Address(email: verifiedEmail),
                        to: new Address(email: granted1),
                        subject: $"SDK conformance E3 {runTag}",
                        template: new TemplateEmailRequestTemplate(templateId: bad, variables: new Dictionary<string, string>()));
                    var res = emails.SendTemplateEmail(req);
                    return (false, $"expected 404, got 200 status={res.Status}");
                }
                catch (Exception ex)
                {
                    var code = HttpStatus(ex);
                    if (code == 404) return (true, $"404 {BodySnippet(ex)}");
                    return (false, $"expected 404, got {code} {BodySnippet(ex)}");
                }
            });

            Scenario("E4", "requestEmailConsent for unverified domain is rejected", () =>
            {
                var fresh = $"{runTag}-e4-target@example.com";
                try
                {
                    var req = new ConsentEmailRequest(domain: unverifiedDomain,
                        emails: new List<ConsentEmailEntry> { new ConsentEmailEntry(fresh) });
                    var res = emails.RequestEmailConsent(req);
                    return (false, $"expected 4xx, got 200 emails={res.Emails?.Count ?? 0}");
                }
                catch (Exception ex)
                {
                    var code = HttpStatus(ex);
                    if (code >= 400 && code < 500) return (true, $"{code} {BodySnippet(ex)}");
                    return (false, $"expected 4xx, got {code} {BodySnippet(ex)}");
                }
            });

            Scenario("E5", "listEmailConsents for non-existent domain", () =>
            {
                var bogus = $"not-a-real-domain-{runTag}.invalid";
                try
                {
                    var res = emails.ListEmailConsents(domain: bogus, limit: 1);
                    return (true, $"200 emails={res.Emails?.Count ?? 0} (no error path for unknown domain)");
                }
                catch (Exception ex)
                {
                    var code = HttpStatus(ex);
                    if (code >= 400 && code < 500) return (true, $"{code} {BodySnippet(ex)}");
                    return (false, $"unexpected: {code} {BodySnippet(ex)}");
                }
            });
        }
        else
        {
            var pending = new (string id, string name)[]
            {
                ("D1", "sendEmail to 2 granted recipients"),
                ("D2", "sendEmail to 2 denied recipients"),
                ("D3", "sendEmail granted+denied mix"),
                ("D4", "sendTemplateEmail to granted"),
                ("D5", "sendTemplateEmail to denied"),
                ("D6", "requestEmailConsent for 2 fresh addresses"),
                ("E1", "sendEmail from unverified domain rejected"),
                ("E2", "sendTemplateEmail from unverified domain rejected"),
                ("E3", "sendTemplateEmail with bogus template_id"),
                ("E4", "requestEmailConsent for unverified domain rejected"),
                ("E5", "listEmailConsents for non-existent domain"),
            };
            foreach (var (id, name) in pending) Skip(id, name);
        }

        foreach (var (id, name, status, detail) in results)
        {
            var n = name.Length > 58 ? name.Substring(0, 58) : name;
            Console.WriteLine($"{status,-4}  dotnet  {id}  {n,-58}  {detail}");
        }
        var passed = results.Count(r => r.status == "PASS");
        var failed = results.Count(r => r.status == "FAIL");
        var skipped = results.Count(r => r.status == "SKIP");
        Console.WriteLine();
        Console.WriteLine($"{passed} passed, {failed} failed, {skipped} skipped");
        if (allowDestructive)
        {
            Console.WriteLine();
            Console.WriteLine("Destructive run created consent records for cleanup:");
            Console.WriteLine($"  {fresh1}");
            Console.WriteLine($"  {fresh2}");
        }
        Environment.Exit(failed > 0 ? 1 : 0);
    }
}
