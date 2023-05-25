# MessageAPI
APIs for collecting user messages in my Portfolio and Future apps in ASP.NET and Microsoft SQL Server

# Usage

Clone the repo into a Visual Studio API project with Docker enabled, then in the command line:

- dotnet build
- dotnet run

Setting up a database:
<br>
- Installl the following packages to your project:

Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design
Install-Package Microsoft.EntityFrameworkCore.Tools

- Making database models from your database:

Scaffold-DbContext “Server={database servername}; Database={Database Name};Trusted_Connection=True;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

- Add The Following Connection String in your appsettings :

“ConnectionStrings”:{
 “DefaultConnection” : “Server={database servername}; Database={Database Name};Trusted_Connection=True;”
}

- Run the following code in your  package manager console :
- 
add-migration initial

- Run the following to update your database models run :

Scaffold-DbContext “Server={database servername}; Database={Database Name};Trusted_Connection=True;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

Command line templates for REST operations:

- POST:  Invoke-RestMethod -Uri http://localhost:5272/api/message/message -Method POST -ContentType "application/json" -Body '{ "id": 1, "name": "John Doe", "email": "johndoe@example.com", "content": "This is a test message." }'
- PUT:  Invoke-RestMethod -Uri http://localhost:5272/api/message/message -Method PUT -ContentType "application/json" -Body '{ "id": 123, "name": "Kevin", "email": "johndoe@example.com", "content": "Hi." }'
- GET:  Invoke-RestMethod -Uri 'http://localhost:5272/api/message/message/johndoe@example.com' -Method GET
- DELETE: Invoke-RestMethod -Uri 'http://localhost:5272/api/message/message/johndoe@example.com' -Method DELETE
****
