# Analysis of Original Code













# My Approach
- I have followed the [Clean Architecture](https://www.ssw.com.au/rules/rules-to-better-clean-architecture) approach in that the Domain sits at the centre of the application structure and is wrapped by the Application code and then everything else interfaces with the application. This gives good
separation of concerns.

- The Domain is quite simple as there are not any business rules to implement with the given requirements, which only 
asks for a RESTful service. So the only logic in the Domain is to validate the Domain object properties. 

- I have used the Repository Pattern with Entity Framework Core to remove the dependency between other application layers
from any particular library. This will allow for any other persistent storage to be used with minimal changes to the rest of the
application.

- The SQlite database does not have any referential integrity setup inside of it (but it should have) and so the relationship
between many ProductOptions to one Product has been checked for in the repository logic. Also uniqueness, which a Primary Key
would normally give.







# Recommendations Before Production Deployment
1. Add user authentication and authorisation
1. Upgrade the database to a production ready database such as SQL Server
1. Setup CI/CD with development and production environments







# Test Coverage
To run the tests and generate a coverage report, run the following commands from a command prompt at the root folder level:

`dotnet test --collect:"Xplat Code Coverage"`

`dotnet tool install -g dotnet-reportgenerator-globaltool`

`reportgenerator -reports:"./**/coverage.cobertura.xml" -targetdir:"./TestCoverageReport" -reporttypes:"html"`
