@login @voucherissue @base @shared
Feature: VoucherIssue

Background: 
	
	Given the following security roles exist
	| RoleName |
	| Merchant   |

	Given the following api resources exist
	| ResourceName            | DisplayName                    | Secret  | Scopes                  | UserClaims                 |
	| estateManagement        | Estate Managememt REST         | Secret1 | estateManagement        | MerchantId, EstateId, role |
	| transactionProcessor    | Transaction Processor REST     | Secret1 | transactionProcessor    |                            |
	| transactionProcessorACL | Transaction Processor ACL REST | Secret1 | transactionProcessorACL | MerchantId, EstateId, role |
	| voucherManagement    | Voucher Management REST     | Secret1 | voucherManagement    |                            |

	Given the following clients exist
	| ClientId       | ClientName      | Secret  | AllowedScopes                                                                   | AllowedGrantTypes  |
	| serviceClient  | Service Client  | Secret1 | estateManagement,transactionProcessor,transactionProcessorACL,voucherManagement | client_credentials |
	| merchantClient | Merchant Client | Secret1 | transactionProcessorACL,estateManagement                                        | password           |

	Given I have a token to access the estate management and transaction processor acl resources
	| ClientId      | 
	| serviceClient | 

	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Voucher    | True                        | True                        |

	Given I create a contract with the following values
	| EstateName    | OperatorName | ContractDescription |
	| Test Estate 1 | Voucher    | Healthcare Centre 1  |

	When I create the following Products
	| EstateName    | OperatorName | ContractDescription | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Voucher    | Healthcare Centre 1 | 10 KES Topup  | 10 KES     | 10.00 |

	Given I create the following merchants
	| MerchantName    | AddressLine1   | Town     | Region      | Country        | ContactName    | EmailAddress                 | EstateName    |
	| Test Merchant 1 | Address Line 1 | TestTown | Test Region | United Kingdom | Test Contact 1 | testcontact1@merchant1.co.uk | Test Estate 1 |

	Given I have created the following security users
	| EmailAddress                  | Password | GivenName    | FamilyName | EstateName    | MerchantName    |
	| merchantuser@testmerchant1.co.uk | 123456   | TestMerchant | User1      | Test Estate 1 | Test Merchant 1 |

	Given I have assigned the following  operator to the merchants
	| OperatorName | MerchantName    | MerchantNumber | TerminalNumber | EstateName    |
	| Voucher    | Test Merchant 1 | 00000001       | 10000001       | Test Estate 1 |

	Given I make the following manual merchant deposits 
	| Reference | Amount  | DateTime  | MerchantName    | EstateName    |
	| Deposit1  | 100.00 | Today     | Test Merchant 1 | Test Estate 1 |
	| Deposit2  | 100.00 | Yesterday | Test Merchant 1 | Test Estate 1 |

	Then the merchant balances are as follows
	| Balance | AvailableBalance | MerchantName    | EstateName    |
	| 200.00 | 200.00          | Test Merchant 1 | Test Estate 1 |

@PRTest
Scenario: Successful Voucher Issue
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 200.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Voucher button
	Then the Voucher Select Operator Page is displayed
	
	Given I tap on the HealthcareCentre1 button
	Then the Voucher Select Product Page is displayed

	Given I tap on the 10 KES button
	Then the Voucher Issue Page is displayed
	
	When I enter the following recipient details
	| RecipientMobile | RecipientEmail | CustomerEmailAddress |
	| 123456789       |                |                      |

	And I tap on Issue Voucher
	
	Then The Voucher Issue Successful Screen will be displayed
