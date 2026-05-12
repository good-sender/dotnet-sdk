# GoodSender.Model.SendEmail
Must provide valid sender and subject. At least one recipient (from 'to', 'cc', or 'bcc') is required. Either 'text_content', 'html_content', 'markdown_content', or 'template_id' is required. When 'markdown_content' is provided, 'text_content' and 'html_content' are ignored. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**From** | [**Address**](Address.md) | Sender address (required) | 
**To** | [**List&lt;Address&gt;**](Address.md) | To recipients. At least one recipient (to, cc, or bcc) is required. Maximum 1000 recipients per email. | 
**Subject** | **string** | The subject of the email (required) | [default to ""]
**TextContent** | **string** | Plain text content | [optional] 
**HtmlContent** | **string** | HTML content | [optional] 
**MarkdownContent** | **string** | Markdown content. When provided, text_content and html_content are ignored. The raw markdown is used as text_content and rendered to HTML for html_content.  | [optional] 
**TemplateId** | **string** | Template ID for templated emails | [optional] 
**TemplateData** | **Dictionary&lt;string, Object&gt;** | Data to populate template variables | [optional] 
**Attachments** | [**List&lt;Attachment&gt;**](Attachment.md) | Email attachments | [optional] 
**Headers** | **Dictionary&lt;string, string&gt;** | Custom email headers | [optional] 
**ReplyTo** | [**Address**](Address.md) | Reply-to address | [optional] 
**SendTime** | **long** | Unix timestamp for when to send the email. Must not be more than 72 hours in the future. If 0, sends immediately. | [optional] 
**WebhookData** | **Dictionary&lt;string, string&gt;** | Custom data to include in webhook events. Maximum 10 keys, key length 50 chars, value length 100 chars. | [optional] 
**Tag** | **string** | Custom tag for tracking. Maximum 100 characters. | [optional] 
**Tracking** | [**TrackingSettings**](TrackingSettings.md) | Email tracking settings | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

