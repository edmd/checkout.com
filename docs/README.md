## Payment Gateway API

# Context

A simple Payment Gateway to accept payments on behalf of merchants and process them onwards.

The solution consists of a API. A simulator 'Application' calling the API. A collection of classes simulating 
the acquiring payment processors behind the API and the start of a datastore used by this microservice. 
The API provides a reduced subset of possible endpoints that a Payment Gateway may provide.

 - Application layout:
	
	The Application is designed using a tiered model, the tiers are loosely coupled and provide clear 
	separation of concerns between the boundaries; from a top-down approach a tier only has access to the 
	tier immediately proceeding it.

	Simulator --> Payment Gateway API --> Services --> Repository --> DataStore
									 
 - Security:
	
	The API is secured using authentication handled by an Identity Server by way of tokens. The tokens 
	grants access to the whole API. Authorisation granularity will be handled in future iterations via 
	configuring of Audiences, Scopes and Claims on the API Controllers and Identity Server.

 - Error Handling:
	
	Error handling is reserved at the presentation and middleware layer. Lower tiers do not handle logging
	There should be a very clear need for additional information required for triage to break this rule, 
	even so this can be mitigated. i.e. the other layers simply bubble up exceptions that are 
	handled in the top tier.

 - Logging and PII:
	
	Presentation level View Models have PII properties masked, because only these models are logged, with 
	this simple approach we avoid logging PII provided this pattern is maintained going forward.
	
 - Testing:
	
	The Solution has sample unit and integration tests for each of the respective tiers. These would need 
	to be exanded on to bring coverage up to a respectable level. Where testing would not yield a  
	benefit to time invested; these solution entities can be excluded by being decorated with the 
	[ExcludeFromCodeCoverage] attribute.
	 
	The appropriate TestRunner nugets have been included in the Solution, so if you want to view the Test 
	Results, open your chosen test Explorer and run the tests after compilation.

 - Deployment and Running:
	
	The Application has been built using .NET 6.0. This will need to be downloaded and installed on the 
	machine running the application.
	
	The simplist way to run the application is to run the Solution in Debug mode in Visual Studio or equivalent, 
	this will download the nuget dependencies and run the projects in the correct order. 

	Alternatively, open 3 terminal windows and run each of the 3 Applications (Identity, API and App) from
	the seperate windows using the following example commands (depending on where you downloaded the App to):
	
	1.	MSBuild.exe "C:\sln\PaymentGateway.sln" /t:Rebuild /p:Configuration=Release /p:Platform="Any CPU"
	2.	cd "C:\sln\src\IdentityServer"
	    C:\sln\src\IdentityServer>dotnet run Identity
	3.	cd "C:\sln\src\PaymentGateway.API"
	    C:\sln\src\PaymentGateway.API>dotnet run PaymentGateway.API
	4.	cd "C:\sln\src\PaymentGateway.Simulator"					
	    C:\sln\src\PaymentGateway.API>dotnet run PaymentGateway.Simulator
	
- Production:
    
	In a Production environment, this Application should be containerised and deployed to an automated ScaleSet, 
	for example: Kubernetes. This should be setup in a CI/CD pipeline. All Application configuration should be 
	self-contained and provisioned in AKV, KMS or an equivalent. Ideally Applicatoin configuratoin should be 
	held in memory on Application start up.

# Assumptions and Workings 

Uses simplified Entity Framework data access logic.

Elected to use an In-Memory Database, to simplify setup and running of the project. This avoids the 
need for migration scripts and Database scaffolding.

The API utilises the Mediator pattern for data interaction. For data transformation and mapping, the 
Models have been simplified by keeping the same property names to keep boilerplate to a minimum.

In a Production Environment, the Gateway would have agency over settlement. In the context of this 
Application, the Payment Gateway has not been configured to determine which Acquirer to use for a 
given corridor. We are simply determining the Payment Processor by the Card Details supplied.

i.e. Given a Card Number submitted to the Gateway, will determine which Acquirer to use:
	1. Card Numbers beginning with: 1111 - Simulated Acquirer offline
	2. Card Numbers beginning with: 1234 - Card fails Acquirer validation
	3. Any other Card Numbers: - Successful transaction creation

# Excluded (due to time constraints) but desirable for future iterations

1. Improved logging formatting and richer information, including Correlation Ids
2. Customised Application specific Exceptions bubbled up to the middleware
3. A Client library consuming the Payment Gateway Api interface
4. Further improved coverage via integration and unit tests.
5. The transaction entities in the Datastore need to be auditable, i.e. either contain audit timestamps
 	  or temporality should be applied to the tables storing transactional data:
	  https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables
6. The creation and application of Code First migration scripts; although this is not always possible and 
   a database first approach is the only choice in most brownfield cases
7. You can optionally search the project on TODO to view further items and explanation notes

# Result

![image](https://github.com/edmd/payment-gateway/assets/20398469/af6c5f2c-1669-47e9-b61e-5b6ab9c5ebf9)

![image](https://github.com/edmd/payment-gateway/assets/20398469/ca9d8cd4-e48e-47e0-8c9d-440d18b14b55)


