using System;
using System.Collections.Generic;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

// 6-method mock-mode smoke test (against Prism). Moved from Program.cs;
// Program.cs is now a thin dispatcher between Smoke and Conformance.
public static class Smoke
{
    public static void Run()
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost:4010";
        var apiKey = Environment.GetEnvironmentVariable("GOODSENDER_API_KEY") ?? "test-key";

        var config = new Configuration { BasePath = baseUrl, AccessToken = apiKey };
        var emails = new EmailsApi(config);
        var domains = new DomainsApi(config);

        var results = new List<(string method, bool ok, string detail)>();

        void RunOne(string method, Func<string> body)
        {
            try { results.Add((method, true, body())); }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Length > 200) msg = msg.Substring(0, 200);
                results.Add((method, false, $"{ex.GetType().Name}: {msg}"));
            }
        }

        RunOne("sendEmail", () =>
        {
            var req = new SendEmailRequest(new List<SendEmail>
            {
                new SendEmail(
                    from: new Address(email: "sender@example.com"),
                    to: new List<Address> { new Address(email: "recipient@example.com") },
                    subject: "Hello",
                    textContent: "Body")
            });
            var res = emails.SendEmail(req);
            return $"sent={res.Sent} declined={res.Declined}";
        });

        RunOne("sendTemplateEmail", () =>
        {
            var req = new TemplateEmailRequest(
                from: new Address(email: "sender@example.com"),
                to: new Address(email: "recipient@example.com"),
                subject: "OTP",
                template: new TemplateEmailRequestTemplate(
                    templateId: "otp_code",
                    variables: new Dictionary<string, string> { ["code"] = "123456" }));
            var res = emails.SendTemplateEmail(req);
            return $"status={res.Status}";
        });

        RunOne("requestEmailConsent", () =>
        {
            var req = new ConsentEmailRequest(
                domain: "example.com",
                emails: new List<ConsentEmailEntry> { new ConsentEmailEntry("smoke-dotnet@example.com") });
            var res = emails.RequestEmailConsent(req);
            return $"emails={res.Emails?.Count ?? 0}";
        });

        RunOne("getEmailConsentStatus", () =>
        {
            var res = emails.GetEmailConsentStatus("user@example.com", domain: "example.com");
            return $"entries={res.Count}";
        });

        RunOne("listEmailConsents", () =>
        {
            var res = emails.ListEmailConsents(domain: "example.com", limit: 50);
            return $"emails={res.Emails?.Count ?? 0}";
        });

        RunOne("listDomains", () =>
        {
            var res = domains.ListDomains(limit: 50);
            return $"domains={res.Domains.Count}";
        });

        foreach (var (method, ok, detail) in results)
            Console.WriteLine($"{(ok ? "PASS" : "FAIL"),-4}  dotnet  {method,-22}  {detail}");

        var failed = 0;
        foreach (var (_, ok, _) in results) if (!ok) failed++;
        var passed = results.Count - failed;
        Console.WriteLine();
        Console.WriteLine($"{passed} passed, {failed} failed");
        Environment.Exit(failed > 0 ? 1 : 0);
    }
}
