@background @login @safaricomtopup
Feature: SafaricomTopup

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

	Given the application in in test mode

Scenario: Successful Safaricom Topup
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Mobile Topup button
	Then the Mobile Topup Select Operator Page is displayed
	
	Given I tap on the Safaricom button
	Then the Mobile Topup Select Product Page is displayed

	Given I tap on the Custom button
	Then the Mobile Topup Topup Details Page is displayed
	
	When I enter the following topup details
	| CustomerMobileNumber | TopupAmount |
	| 123456789            | 10			 |
	And I tap on Perform Topup
	
	Then The Topup Successful Screen will be displayed

@PRTest 
Scenario: Successful Safaricom Topup with Email Address Captured
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Mobile Topup button
	Then the Mobile Topup Select Operator Page is displayed
	
	Given I tap on the Safaricom button
	Then the Mobile Topup Select Product Page is displayed

	Given I tap on the Custom button
	Then the Mobile Topup Topup Details Page is displayed
	
	When I enter the following topup details
	| CustomerMobileNumber | TopupAmount | CustomerEmailAddress                |
	| 123456789            | 10          | testcustomer@customer.co.uk |

	And I tap on Perform Topup
	
	Then The Topup Successful Screen will be displayed

Scenario: Failed Safaricom Topup
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Mobile Topup button
	Then the Mobile Topup Select Operator Page is displayed
	
	Given I tap on the Safaricom button
	Then the Mobile Topup Select Product Page is displayed
	
	Given I tap on the Custom button
	Then the Mobile Topup Topup Details Page is displayed

	When I enter the following topup details
	| CustomerMobileNumber | TopupAmount |
	| 123456789            | 1000        |
	And I tap on Perform Topup
	
	Then The Topup Failed Screen will be displayed

Scenario: Failed Validation Topup - No Amount
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Mobile Topup button
	Then the Mobile Topup Select Operator Page is displayed
	
	Given I tap on the Safaricom button
	Then the Mobile Topup Select Product Page is displayed
	
	Given I tap on the Custom button
	Then the Mobile Topup Topup Details Page is displayed
		
	When I enter the following topup details
	| CustomerMobileNumber | TopupAmount |
	| 123456789            |             |
	And I tap on Perform Topup
	
	Then The Topup Validation Error will be displayed

Scenario: Failed Validation Topup - No Mobile Number
	Given I am on the Login Screen
	
	When I enter 'merchantuser@testmerchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	And I tap on Login
	
	Then the Merchant Home Page is displayed

	And the available balance is shown as 2000.00

	Given I tap on the Transactions button
	Then the Transactions Page is displayed
	
	Given I tap on the Mobile Topup button
	Then the Mobile Topup Select Operator Page is displayed
	
	Given I tap on the Safaricom button
	Then the Mobile Topup Select Product Page is displayed
	
	Given I tap on the Custom button
	Then the Mobile Topup Topup Details Page is displayed

	When I enter the following topup details
	| CustomerMobileNumber | TopupAmount |
	|                      | 100000      |
	And I tap on Perform Topup
	
	Then The Topup Validation Error will be displayed	