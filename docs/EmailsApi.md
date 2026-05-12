# GoodSender.Api.EmailsApi

All URIs are relative to *https://api.goodsender.com*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetEmailConsentStatus**](EmailsApi.md#getemailconsentstatus) | **GET** /v1/emails/{email} | Get recipient consent status |
| [**ListEmailConsents**](EmailsApi.md#listemailconsents) | **GET** /v1/emails | List email consent statuses |
| [**RequestEmailConsent**](EmailsApi.md#requestemailconsent) | **POST** /v1/emails/consent | Request recipients&#39; consent to receive emails from your domain |
| [**SendEmail**](EmailsApi.md#sendemail) | **POST** /v1/emails/send | Send an email or a batch of emails |
| [**SendTemplateEmail**](EmailsApi.md#sendtemplateemail) | **POST** /v1/emails/template | Send a transactional email using a template |

<a id="getemailconsentstatus"></a>
# **GetEmailConsentStatus**
> List&lt;EmailAccount&gt; GetEmailConsentStatus (string email, string? domain = null)

Get recipient consent status

Retrieve the current consent status for an email address. Optionally filter by sender domain.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class GetEmailConsentStatusExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new EmailsApi(config);
            var email = user@example.com;  // string | Email address to look up.
            var domain = example.com;  // string? | Optional sender domain to filter consent records by. When omitted, returns consent across all domains. (optional) 

            try
            {
                // Get recipient consent status
                List<EmailAccount> result = apiInstance.GetEmailConsentStatus(email, domain);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EmailsApi.GetEmailConsentStatus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEmailConsentStatusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get recipient consent status
    ApiResponse<List<EmailAccount>> response = apiInstance.GetEmailConsentStatusWithHttpInfo(email, domain);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EmailsApi.GetEmailConsentStatusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **email** | **string** | Email address to look up. |  |
| **domain** | **string?** | Optional sender domain to filter consent records by. When omitted, returns consent across all domains. | [optional]  |

### Return type

[**List&lt;EmailAccount&gt;**](EmailAccount.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Recipient consent status across all domains |  -  |
| **400** | Invalid email address |  -  |
| **404** | Email address was not found |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="listemailconsents"></a>
# **ListEmailConsents**
> EmailListResponse ListEmailConsents (string domain, int? limit = null, string? cursor = null, string? consentStatus = null, string? engagementStatus = null)

List email consent statuses

Retrieve a paginated list of email consent statuses for a domain.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class ListEmailConsentsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new EmailsApi(config);
            var domain = example.com;  // string | Sender domain to filter consent records by.
            var limit = 50;  // int? | Maximum number of records to return. (optional)  (default to 50)
            var cursor = "cursor_example";  // string? | Cursor for pagination. (optional) 
            var consentStatus = "pending";  // string? | Status of the recipient's consent for receiving emails. 'pending' = awaiting consent email send, 'requested' = consent email dispatched, 'failed' = consent email delivery failed, 'granted' = recipient consented to receive emails, 'denied' = recipient declined to receive emails. (optional) 
            var engagementStatus = "new";  // string? | Status of the recipient's engagement with the emails. (optional) 

            try
            {
                // List email consent statuses
                EmailListResponse result = apiInstance.ListEmailConsents(domain, limit, cursor, consentStatus, engagementStatus);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EmailsApi.ListEmailConsents: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ListEmailConsentsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // List email consent statuses
    ApiResponse<EmailListResponse> response = apiInstance.ListEmailConsentsWithHttpInfo(domain, limit, cursor, consentStatus, engagementStatus);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EmailsApi.ListEmailConsentsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **domain** | **string** | Sender domain to filter consent records by. |  |
| **limit** | **int?** | Maximum number of records to return. | [optional] [default to 50] |
| **cursor** | **string?** | Cursor for pagination. | [optional]  |
| **consentStatus** | **string?** | Status of the recipient&#39;s consent for receiving emails. &#39;pending&#39; &#x3D; awaiting consent email send, &#39;requested&#39; &#x3D; consent email dispatched, &#39;failed&#39; &#x3D; consent email delivery failed, &#39;granted&#39; &#x3D; recipient consented to receive emails, &#39;denied&#39; &#x3D; recipient declined to receive emails. | [optional]  |
| **engagementStatus** | **string?** | Status of the recipient&#39;s engagement with the emails. | [optional]  |

### Return type

[**EmailListResponse**](EmailListResponse.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A paginated list of email consent statuses |  -  |
| **400** | Invalid request parameters |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="requestemailconsent"></a>
# **RequestEmailConsent**
> ConsentEmailResult RequestEmailConsent (ConsentEmailRequest consentEmailRequest)

Request recipients' consent to receive emails from your domain

Send a consent message to each address so recipients can approve or reject future emails from your domain. Include the email addresses in the request body to start the consent flow. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class RequestEmailConsentExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new EmailsApi(config);
            var consentEmailRequest = new ConsentEmailRequest(); // ConsentEmailRequest | 

            try
            {
                // Request recipients' consent to receive emails from your domain
                ConsentEmailResult result = apiInstance.RequestEmailConsent(consentEmailRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EmailsApi.RequestEmailConsent: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RequestEmailConsentWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Request recipients' consent to receive emails from your domain
    ApiResponse<ConsentEmailResult> response = apiInstance.RequestEmailConsentWithHttpInfo(consentEmailRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EmailsApi.RequestEmailConsentWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **consentEmailRequest** | [**ConsentEmailRequest**](ConsentEmailRequest.md) |  |  |

### Return type

[**ConsentEmailResult**](ConsentEmailResult.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Recipient consent status for each address (found or created) |  -  |
| **400** | Invalid request |  -  |
| **429** | Too many consents are awaiting processing for this workspace. The internal release queue will drain pending entries automatically; retry later.  |  -  |
| **500** | Internal server error. The upfront quota reservation (if any) is refunded and the request is safe to retry — &#x60;getOrCreateEmailsInDb&#x60; is idempotent.  |  -  |
| **502** | Bad gateway - upstream email service unavailable |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="sendemail"></a>
# **SendEmail**
> SendEmailResponse SendEmail (SendEmailRequest sendEmailRequest)

Send an email or a batch of emails

Send one or more emails. Emails can be sent only to recipients who have opted in to receive communications from your domain. The response indicates how many emails were sent versus not sent, based on each recipient's consent state. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class SendEmailExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new EmailsApi(config);
            var sendEmailRequest = new SendEmailRequest(); // SendEmailRequest | List of emails to send

            try
            {
                // Send an email or a batch of emails
                SendEmailResponse result = apiInstance.SendEmail(sendEmailRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EmailsApi.SendEmail: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SendEmailWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Send an email or a batch of emails
    ApiResponse<SendEmailResponse> response = apiInstance.SendEmailWithHttpInfo(sendEmailRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EmailsApi.SendEmailWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **sendEmailRequest** | [**SendEmailRequest**](SendEmailRequest.md) | List of emails to send |  |

### Return type

[**SendEmailResponse**](SendEmailResponse.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Email(s) accepted for sending |  -  |
| **400** | Bad request - validation error |  -  |
| **401** | Unauthorized - invalid or missing API key |  -  |
| **413** | Payload too large |  -  |
| **429** | Quota exceeded. |  * Retry-After - Seconds until the quota resets. <br>  |
| **500** | Internal server error |  -  |
| **502** | Bad gateway - upstream email service unavailable |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="sendtemplateemail"></a>
# **SendTemplateEmail**
> TemplateEmailResponse SendTemplateEmail (TemplateEmailRequest templateEmailRequest)

Send a transactional email using a template

Send a transactional email using a predefined template for common use cases like OTP codes, order confirmations, and new device login alerts. If the recipient has \"denied\" consent, the response returns `{\"status\": \"declined\"}` and the email is not sent. Unknown recipients are auto-registered with \"pending\" consent. The template endpoint does not change the recipient's consent. Each email includes an approve/reject footer allowing the recipient to manage future communications. Provide the template ID and any variables to fill in the placeholders. All variables are optional and will be replaced with an empty string if omitted. URL-type variables must point to the same domain as the sender's email address. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class SendTemplateEmailExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new EmailsApi(config);
            var templateEmailRequest = new TemplateEmailRequest(); // TemplateEmailRequest | Template email to send

            try
            {
                // Send a transactional email using a template
                TemplateEmailResponse result = apiInstance.SendTemplateEmail(templateEmailRequest);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EmailsApi.SendTemplateEmail: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SendTemplateEmailWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Send a transactional email using a template
    ApiResponse<TemplateEmailResponse> response = apiInstance.SendTemplateEmailWithHttpInfo(templateEmailRequest);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EmailsApi.SendTemplateEmailWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **templateEmailRequest** | [**TemplateEmailRequest**](TemplateEmailRequest.md) | Template email to send |  |

### Return type

[**TemplateEmailResponse**](TemplateEmailResponse.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Whether the templated email was sent |  -  |
| **400** | Bad request - invalid variables or request body |  -  |
| **401** | Unauthorized - invalid or missing API key |  -  |
| **404** | Template not found |  -  |
| **413** | Payload too large |  -  |
| **429** | Quota exceeded. |  * Retry-After - Seconds until the quota resets. <br>  |
| **500** | Internal server error |  -  |
| **502** | Bad gateway - upstream email service unavailable |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

