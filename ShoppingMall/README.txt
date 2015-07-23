
Following is the details of Braintree Payment gateway Sandbox Credit/Visa card. please use this card number for the payment with MeeraGarments.
https://www.braintreepayments.com/docs/dotnet/reference/sandbox


Braintree Payment gateway credentials

Url:		https://sandbox.braintreegateway.com/login
UserName:	buntybubbly
Password:	bunty@1234567

Gmail Credentials

Gmail Url:	https://accounts.google.com/ServiceLogin
Email:		buntybunty2890@gmail.com
Password:	bunty@123456


BrainTree Payment Gateway Testing Visa/Credict Card numbers as follow

The sandbox environment only accepts test credit card numbers. The following card numbers may be used.

Visa
4111 1111 1111 1111
4005 5192 0000 0004
4009 3488 8888 1881
4012 0000 3333 0026
4012 0000 7777 7777
4012 8888 8888 1881
4217 6511 1111 1119
4500 6000 0000 0061

may failure of transaction due to followin reasons

Test Amounts(Sandbox Environment of Braintree) for Unsuccessful Transactions

When working with transactions, you can pass specific amounts to simulate different responses from the gateway.

Amounts between $0.01 - $1999.99 will simulate a successful authorization.
Amounts between $2000.00 - $2062.99 and $3000.00 - $3000.99 will decline with the coordinating Processor Response.
Amounts between $2063.00 - $2999.99 will simulate the generic decline message “Processor Declined” with a decline code equal to the amount.
Amounts $3001.00 and greater will also simulate a successful authorization.