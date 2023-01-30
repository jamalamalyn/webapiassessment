# Assessment - Create an API application in .Net Core

## Objective:
- To create a Web API that will be deployed to an Azure environment. The completed application does not need to run, but it must build without errrors.

---

## Story
- The application will expose CRUD operations as API that manage one entity.
- The entity is “Client” and it has the following properties.
  - Unique Id
  - Client Name
  - Contact email address
  - Date became customer
- Name of the properties and date types are up to you to decide.
- User authentication is out of scope.

---

## Requirements
- Use .Net 5 and C#
- Use Entity Framework 5 to access a database Note: please use this dummy connection string in your code Server=tcp:sqlserver.database.windows.net,1433;Initial Catalog=dummydb;Persist Security Info=False;User ID=sqluser;Password=sqlpassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
- Include swagger that displays your API design
- Consider the application will be deployed to Azure

--- 

## Optional
- Unit tests / Integration tests (framework is your choice)
  
---

## What next
- After you completed the exercise, please upload the whole source code, including a solution file to your GitHub repository and email the URI. If you do not have a private GitHub account and you do not wish to create one, please ZIP up the source code and send it by email.

---

## Data model
- Entity "Client" will have 4 fields
  - Unique Id
  - Client Name
  - Contact email address
  - Date became customer
 
```json
{
  "Id" : 1
  "Name" : "Brendan Lynch",
  "EmailAddress" : "test@test.com",
  "RegisteredDateTime" : "2023-01-30T08:31:44.882Z"
}
```
