# GoodSender.Model.EmailAccount

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Email** | **string** | Recipient email address. | 
**Name** | **string** | Optional display name for the recipient. Used in the To header on the consent email when present, and surfaced through the Dashboard. May be null when no name has been provided. | [optional] 
**Domain** | **string** | Domain part of the email address. | 
**ConsentStatus** | **string** | Status of the recipient&#39;s consent for receiving emails. &#39;pending&#39; &#x3D; awaiting consent email send, &#39;requested&#39; &#x3D; consent email dispatched, &#39;failed&#39; &#x3D; consent email delivery failed, &#39;granted&#39; &#x3D; recipient consented to receive emails, &#39;denied&#39; &#x3D; recipient declined to receive emails. | 
**EngagementStatus** | **string** | Status of the recipient&#39;s engagement with the emails. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

