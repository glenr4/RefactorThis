# Analysis of Original Code
- Written in .Net Core v2.2 which is not longer supported 

  -> Upgraded to latest stable version which is .Net v5.0 at the time of writing

- Uses SQlite as the database, this is not a production ready database and  needs to be upgraded to another relational database like SQL Server or Postgres

  -> Upgrading the database is beyond the scope of this coding exercise
  
  -> The database schema does not have primary keys or a one to many relationship between Products and ProductOptions, this would need to be added to the database design when upgrading the database

- The application is a Web API but includes a Razor package and useMvc() in the configuration, this is only required for serving HTML pages

- Everything is in a sinlge project, which does not allow for any code re-use 

- Mixes data access inside the Domain objects

- No tests

- No dependency injection

- Uses string substitution for creating database queries

  -> This is how SQL Injection vulnerabilities occurr, parameterised queries should always be used to avoid this

  -> Added EF Core to the project as it allows for easy, parameterised SQL generation and is widely used. The majority of the code will also be compatible for use with SQL Server, Postgres etc.

- Multiple classes defined in the Products.cs file

  -> Generally only one class should be defined within a file

- No logging

- No exception handling and generation of useful error messages

- No API documentation

- No authorisation/authentication





# My Approach
- Due to the issues listed above, I started from scratch with a new solution

- I have followed the [Clean Architecture](https://www.ssw.com.au/rules/rules-to-better-clean-architecture) model in that the Domain sits at the centre of the application structure and is wrapped by the Application code and then everything else interfaces with the application. This gives good
separation of concerns.

- The Domain is quite simple as there are no business rules to implement with the given requirements, which only asks for a RESTful service. So the only logic in the Domain is to validate the Domain object properties on construction. 

- I have used the Repository Pattern with Entity Framework Core to remove the dependency between other application layers, such as the API, from any particular library. This will allow for any other persistent storage to be used with minimal changes to the rest of the application.

  -> This also allows the application to change to CQRS in the future without major changes

- I have also used the Mediator pattern with the API, so the controller methods do not have a dependency on anything other than the Mediatr library. 

  -> This allows the application to be re-used with another type of API, without affecting the Web API project.

- The SQlite database does not have any referential integrity setup inside of it (but it should have) and so the relationship
between many ProductOptions to one Product has been checked for in the repository logic. Also uniqueness, which a Primary Key
would normally give.

- I have added logging to file for the purposes of this exercise, however in a production environment I would use Azure Application Insights or something similar


## Security
- Don't display detailed debug information in Production as this gives insight into the system that a hacker may be able to exploit

- Have added security headers to the responses which limit the attack surface as much as possible

- Only allow Swagger UI when running in development

  -> Swagger requires the Content Security Policy (CSP) to be opened up to allow inline scripts, which opens the API up to Cross Site Scripting (XSS) attacks

## Authentication/Authorisation
- Added authentication using OAuth v2 from Azure Active Directory
- Enabled in Production, to get an access token use: 

  `curl -X POST -H "Content-Type: application/x-www-form-urlencoded" -d 'client_id=a5b8d992-bf9f-4bc7-9a54-c68a54db1949&scope=api%3A%2F%2F3e7d25c4-8f0b-41dd-9885-9410984b4991/.default&client_secret=1aV7Q~V9TDe4KXc~KpbWqxy0bpae6DmtB2jdw&grant_type=client_credentials' 'https://login.microsoftonline.com/dee93494-4996-4929-ac33-83043d5f677a/oauth2/v2.0/token'`

  Then when sending a request use it as a bearer token in the authorization header.

# Recommendations Before Production Deployment
1. Add user authentication and authorisation
1. Upgrade the database to a production ready database such as SQL Server
1. Setup CI/CD 







# Test Coverage
To run the tests and generate a coverage report, run the following commands from a command prompt at the root folder level:

`dotnet test --collect:"Xplat Code Coverage"`

If not already installed: `dotnet tool install -g dotnet-reportgenerator-globaltool`

`reportgenerator -reports:"./**/coverage.cobertura.xml" -targetdir:"./TestCoverageReport" -reporttypes:"html"`

To view the report, open the `index.html` file in the `TestResults` folder.