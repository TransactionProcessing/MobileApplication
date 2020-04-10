@login @base @shared
Feature: Login

#Background: 
#	
#	Given the following security roles exist
#	| RoleName |
#	| Merchant   |
#
#	Given the following api resources exist
#	| ResourceName            | DisplayName                    | Secret  | Scopes                  | UserClaims                 |
#	| estateManagement        | Estate Managememt REST         | Secret1 | estateManagement        | MerchantId, EstateId, role |
#	| transactionProcessor    | Transaction Processor REST     | Secret1 | transactionProcessor    |                            |
#	| transactionProcessorAcl | Transaction Processor ACL REST | Secret1 | transactionProcessorAcl | MerchantId, EstateId, role |
#
#	Given the following clients exist
#	| ClientId       | ClientName      | Secret  | AllowedScopes                                                 | AllowedGrantTypes  |
#	| serviceClient  | Service Client  | Secret1 | estateManagement,transactionProcessor,transactionProcessorAcl | client_credentials |
#	| merchantClient | Merchant Client | Secret1 | transactionProcessorAcl                                       | password           |
#
#	Given I have a token to access the estate management and transaction processor acl resources
#	| ClientId      | 
#	| serviceClient | 
#
#	Given I have created the following estates
#	| EstateName    |
#	| Test Estate 1 |
#
#	Given I have created the following operators
#	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
#	| Test Estate 1 | Safaricom    | True                        | True                        |
#
#	Given I create the following merchants
#	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
#	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |
#
#	Given I have created the following security users
#	| EmailAddress                  | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
#	| merchantuser@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |
#
#	Given I have assigned the following  operator to the merchants
#	| OperatorName | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
#	| Safaricom    | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |
#	   
#@PRTest
#Scenario: Login as Merchant
#	Given I am on the Login Screen
#	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
#	And I enter '123456' as the Password
#	And I tap on Login
#	Then the Merchant Home Page is displayed
