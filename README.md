# GoodSender SDK for .NET

Official client library for the GoodSender email API. Package: `GoodSender`

## Installation

```bash
dotnet add package GoodSender
```

## Quick start

```csharp
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

var config = new Configuration
{
    BasePath = "https://api.goodsender.com",
    AccessToken = "YOUR_API_KEY",
};
var emails  = new EmailsApi(config);
var domains = new DomainsApi(config);

var req = new SendEmailRequest(new List<SendEmail>
{
    new SendEmail(
        from: new Address(email: "sender@example.com"),
        to: new List<Address> { new Address(email: "recipient@example.com") },
        subject: "Hello",
        textContent: "Body")
});
var res = emails.SendEmail(req);
Console.WriteLine($"sent={res.Sent} declined={res.Declined}");
```

## Examples

### Send via a template

```csharp
var req = new TemplateEmailRequest(
    from: new Address(email: "sender@example.com"),
    to: new Address(email: "recipient@example.com"),
    subject: "Your OTP",
    template: new TemplateEmailRequestTemplate(
        templateId: "otp_code",
        variables: new Dictionary<string, string> { ["code"] = "123456" }));
var res = emails.SendTemplateEmail(req);
Console.WriteLine($"status={res.Status}");
```

### List domains

```csharp
var res = domains.ListDomains(limit: 50);
Console.WriteLine($"domains={res.Domains.Count}");
```

### Check consent status

```csharp
var res = emails.GetEmailConsentStatus("user@example.com", domain: "example.com");
Console.WriteLine($"entries={res.Count}");

// List all consents for a domain
var list = emails.ListEmailConsents(domain: "example.com", limit: 50);
Console.WriteLine($"emails={list.Emails?.Count ?? 0}");
```

## Documentation

- API reference: <https://api.goodsender.com/docs>
- OpenAPI spec: `openapi/goodsender.yaml` in this repo
- Conformance tests: `tests/`

## Development

- Regenerate from spec: `scripts/regen.sh` (preserves `tests/`, `.github/`, and hand-curated files per `.regen-ignore`)
- Run conformance tests against local mock: `tests/run.sh mock`
- Run conformance against real dev API: `tests/run.sh dev` (requires `tests/.env.dev`)

## License

MIT — see [LICENSE](LICENSE).
