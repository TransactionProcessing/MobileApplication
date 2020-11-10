#!/bin/sh

mono TransactionMobile/packages/NUnit.ConsoleRunner.3.11.1/tools/nunit3-console.exe TransactionMobile/TransactionMobile.IntegrationTests/bin/Release/TransactionMobile.IntegrationTests.dll --where "cat == Android"