# GoodSender.Model.Attachment
Either inline_id or file_name is required. Content must be base64 encoded.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**FileName** | **string** | File name for the attachment | [optional] [default to ""]
**Content** | **byte[]** | Base64 encoded content | [optional] 
**ContentType** | **string** | MIME content type (required) | [default to ""]
**InlineId** | **string** | Inline attachment ID | [optional] [default to ""]

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

