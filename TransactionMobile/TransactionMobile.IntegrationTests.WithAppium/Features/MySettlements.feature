@background @login @settlements
Feature: MySettlements

Background: 
	
	Given I have created the following estates
	| EstateName    |
	| Test Estate 1 |

	Given I have created the following operators
	| EstateName    | OperatorName        | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
	| Test Estate 1 | Safaricom           | True                        | True                        |
	| Test Estate 1 | HealthCare Centre 1 | False                       | False                       |

	Given I create a contract with the following values
	| EstateName    | OperatorName        | ContractDescription          |
	| Test Estate 1 | Safaricom           | Safaricom Contract           |
	| Test Estate 1 | HealthCare Centre 1 | HealthCare Centre 1 Contract |

	When I create the following Products
	| EstateName    | OperatorName        | ContractDescription          | ProductName    | DisplayText | Value  |
	| Test Estate 1 | Safaricom           | Safaricom Contract           | 100 KES Topup  | 100 KES     | 100.00 |
	| Test Estate 1 | Safaricom           | Safaricom Contract           | Variable Topup | Custom      |        |
	| Test Estate 1 | HealthCare Centre 1 | HealthCare Centre 1 Contract | 10 KES Topup   | 10 KES      | 10.00  |

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

	Given the following transaction fees have been settled
	| MerchantName    | EstateName    | CalculatedValue | SettlementDate | OperatorIdentifier  | FeeDescription      |
	| Test Merchant 1 | Test Estate 1 | 0.25            | Today          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.25            | Today          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.25            | Today          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.80            | Today          | HealthCare Centre 1 | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.25            | Today          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.35            | Today          | HealthCare Centre 1 | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.55            | Yesterday          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.45            | Yesterday          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.25            | Yesterday          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.95            | Yesterday          | HealthCare Centre 1 | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.75            | Yesterday          | Safaricom           | Merchant Commission |
	| Test Merchant 1 | Test Estate 1 | 0.35            | Yesterday          | HealthCare Centre 1 | Merchant Commission |

	Given the application in in test mode

Scenario: My Settlements List

	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Reporting button
	Then the Reporting Page is displayed
	
	Given I tap on the My Settlements button

	Then the My Settlements List Page is displayed	
	And the following entries are displayed
	| SettlementDate | NumberFeesSettled | ValueOfFeesSettled | Order |
	| Today          | 6                 | 2.15               | 0     |
	| Yesterday      | 6                 | 3.30               | 1     |

#@PRTest
Scenario: My Settlements Analysis

	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Reporting button
	Then the Reporting Page is displayed
	
	Given I tap on the My Settlements button

	Then the My Settlements List Page is displayed	
	And the following entries are displayed
	| SettlementDate | NumberFeesSettled | ValueOfFeesSettled | 
	| Today          | 6                 | 2.15               | 
	| Yesterday      | 6                 | 3.30               | 

	When I select the settlement from 'Today'
	Then the Settlement Analysis Page is displayed