# GoodSender.Api.DomainsApi

All URIs are relative to *https://api.goodsender.com*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ListDomains**](DomainsApi.md#listdomains) | **GET** /v1/domains | List domains |

<a id="listdomains"></a>
# **ListDomains**
> DomainListResponse ListDomains (int? limit = null, string? cursor = null)

List domains

Retrieve a paginated list of sender domains for the workspace the API key belongs to. Each entry includes the domain's verification state so callers can detect when DNS records still need attention. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using GoodSender.Api;
using GoodSender.Client;
using GoodSender.Model;

namespace Example
{
    public class ListDomainsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://api.goodsender.com";
            // Configure Bearer token for authorization: bearerAuth
            config.AccessToken = "YOUR_BEARER_TOKEN";

            var apiInstance = new DomainsApi(config);
            var limit = 50;  // int? | Maximum number of records to return. (optional)  (default to 50)
            var cursor = "cursor_example";  // string? | Cursor for pagination, returned as `nextCursor` from a previous response. (optional) 

            try
            {
                // List domains
                DomainListResponse result = apiInstance.ListDomains(limit, cursor);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DomainsApi.ListDomains: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ListDomainsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // List domains
    ApiResponse<DomainListResponse> response = apiInstance.ListDomainsWithHttpInfo(limit, cursor);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DomainsApi.ListDomainsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **limit** | **int?** | Maximum number of records to return. | [optional] [default to 50] |
| **cursor** | **string?** | Cursor for pagination, returned as &#x60;nextCursor&#x60; from a previous response. | [optional]  |

### Return type

[**DomainListResponse**](DomainListResponse.md)

### Authorization

[bearerAuth](../README.md#bearerAuth)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A paginated list of domains for the workspace. |  -  |
| **400** | Invalid request parameters. |  -  |
| **401** | Unauthorized - invalid or missing API key. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

