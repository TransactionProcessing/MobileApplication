@background @login @admin
Feature: Admin

Background: 
	
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Safaricom    | True                        | True                        |

	Given I create a contract with the following values
	| EstateName    | OperatorName | ContractDescription |
	| Test Estate 1 | Safaricom    | Safaricom Contract  |

	When I create the following Products
	| EstateName    | OperatorName | ContractDescription | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 1 | Safaricom    | Safaricom Contract  | Variable Topup | Custom      |        |

	When I add the following Transaction Fees
	| EstateName    | OperatorName    | ContractDescription | ProductName    | CalculationType | FeeDescription      | Value |
	| Test Estate 1 | Safaricom | Safaricom Contract | 100 KES Topup  | Fixed           | Merchant Commission | 2.00  |
	| Test Estate 1 | Safaricom | Safaricom Contract | 100 KES Topup  | Percentage      | Merchant Commission | 0.025 |
	| Test Estate 1 | Safaricom | Safaricom Contract | Variable Topup | Fixed           | Merchant Commission | 2.50  |

	Given I create the following merchants
	| MerchantName    | EstateName    | EmailAddress                     | Password | GivenName    | FamilyName |
	| Test Merchant 1 | Test Estate 1 | merchantuser@testmerchant1.co.uk | 123456   | TestMerchant | User1      |

	Given I make the following manual merchant deposits 
	| Reference | Amount  | DateTime  | MerchantName    | EstateName    |
	| Deposit1  | 1000.00 | Today     | Test Merchant 1 | Test Estate 1 |
	| Deposit2  | 1000.00 | Yesterday | Test Merchant 1 | Test Estate 1 |

@PRTest
Scenario: Successful Reconciliation
	Given I am on the Login Screen

	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed

	Given I tap on the Admin button
	Then the Admin Page is displayed

	Given I tap on the Reconciliation button
	Then the reconciliation success message toast will be displayed
