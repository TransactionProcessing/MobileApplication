@login
Feature: Login

Background: 
	# Setup Estate
	# Setup Operators
	# Setup Merchant
	# Assign Operator To Merchant
	# Create Merchant User

Scenario: Login as Merchant
	Given I am on the Login Screen
	When I enter 'merchantuser@merchant1.co.uk' as the Email Address
	And I enter '123456' as the Password
	When I tap on Login
	Then the Merchant Home Page is displayed
