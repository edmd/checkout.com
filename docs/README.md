# CheckOut.com Payment Gateway

# Context

Simple Payment Gateway to accept payments on behalf of merchants and process them onwards

PaymentGateway.Simulator

# Assumptions and Workings 

Uses simplified Entity Framework DAL

Elected to use In-Memory Database so as to avoid creating and applying Migration scripts

Demonstrates use of Command Design Pattern

# Excluded

1. Logging at the presentation layer.
2. Wrapping objects in validation responses.
3. A Client library consuming the Payment Api Gateway