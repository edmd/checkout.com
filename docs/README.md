## Payment Gateway API

# Context

A simple Payment Gateway to accept payments on behalf of merchants and process them onwards.

The solution consists of a API. An 'Application' calling the API. A collection of classes simulating 
the acquiring payment processors behind the API and the start of datastore used by this microservice. 
The API provides a reduced subset of possible endpoints that a Payment Gateway may provide.

 - Application layout:
	
	The Application is designed using a tiered model, the tiers are loosely coupled and provide clear 
	separation of concerns between the boundaries; from a top-down approach a tier only has access to the 
	tier immediately proceeding it.

	Simulator
        |______>Payment Gateway API
                         |______>Services
                                     |______>DataStore
									 
 - Security:
	
	The API is secured using authentication handled by an Identity Server by way of tokens. The tokens 
	grants access to the whole API. Authorisation granularity will be handled in future iterations via 
	configuring of Audiences, Scopes and Claims on the Controllers and Identity Server.


# Assumptions and Workings 

Uses simplified Entity Framework data access logic.

Elected to use an In-Memory Database, to simplify setup and running of the project. This avoids the 
need for migration scripts and Database scaffolding.

The API utilises the Mediator pattern for data interaction. For data transformation and mapping, the 
Models have been simplified by keeping the same property names to keep boilerplate to a minimum.

In a Production Environment, the Gateway would have agency over settlement. In the context of this 
Application, the Payment Gateway has not been configured to determine which Acquirer to use for a 
given corridor. We are simply determining the Payment Processor by the Card Details supplied.

# Excluded (due to time constraints) but desirable for future iterations

1. Improved logging formatting and richer information
2. Customised Application specific Exceptions bubbled up to the middleware
3. A Client library consuming the Payment Gateway Api interface
4. Further improved coverage via integration and unit tests.