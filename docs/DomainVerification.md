# GoodSender.Model.DomainVerification
Per-record verification state for the domain. `verified` is the overall flag; the individual `*_verified` fields indicate which DNS records still need attention when `verified` is `false`. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Verified** | **bool** | Overall verification status. True only when every required DNS record is in place. | 
**TrackingVerified** | **bool** | Whether the tracking subdomain CNAME is in place. | 
**ReturnPathVerified** | **bool** | Whether the return-path subdomain CNAME is in place. | 
**Dkim1Verified** | **bool** | Whether the first DKIM record is in place. | 
**Dkim2Verified** | **bool** | Whether the second DKIM record is in place. | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

