# GoodSender.Model.QuotaExceededError

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Code** | **string** | Machine-readable error code. | 
**Kind** | **string** | Whether the daily or monthly quota was exhausted. | 
**Message** | **string** | Human-readable error message. | 
**Limit** | **int** | Quota limit that was reached. | 
**Used** | **int** | Number of emails already used against the quota. | 
**ResetAt** | **DateTime** | Timestamp at which the quota window resets. | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

