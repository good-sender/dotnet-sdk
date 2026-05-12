# GoodSender.Model.ConsentEmailRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Domain** | **string** | Domain for the email addresses. | 
**RedirectUrl** | **string** | URL to which the user will be redirected after providing consent. {email} in the URL will be replaced with the recipient&#39;s email address. | [optional] 
**Emails** | [**List&lt;ConsentEmailEntry&gt;**](ConsentEmailEntry.md) | Recipients to request consent from. Each entry may be either a plain email string or a &#x60;{ email, name? }&#x60; object so callers can attach a display name to a specific recipient. Mixing the two forms in one request is allowed.  | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

