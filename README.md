# Online Book Store: 
The Online Book Store is a .NET 8-based web application that allows users to browse, manage, and purchase books online. It provides a fully functional RESTful API with features such as book management, a shopping cart, and checkout functionality, as well as user authentication using JWT tokens. The project also includes integration with SQLite for data storage and OpenTelemetry for monitoring.

# Description: 
The purpose of this project is to demonstrate how to build a modern, cloud-ready web API using .NET 8. The Online Book Store allows users to:

Manage books (create, update, retrieve).
Add books to a shopping cart.
Checkout and calculate the total price of cart items.
Authenticate and authorize users with JWT-based authentication.
Monitor performance and trace API requests using OpenTelemetry with SQLite for in-memory telemetry data storage.

# Installation Instructions: 
Prerequisites:
.NET 8 SDK
SQLite (in-memory or local)
A development environment like Visual Studio or VS Code

Steps:
1. Clone the Repository:
git clone https://github.com/your-repo/online-book-store.git
cd online-book-store

2. Install Dependencies: Install necessary NuGet packages, including:
ASP.NET Core Identity
SQLite
OpenTelemetry

3. Database Setup: The project is pre-configured to use an SQLite in-memory database. You can modify the appsettings.json file to use a persistent SQLite database or another provider if needed.

4. Run the Application:
'dotnet run'
application will run at https://localhost:5001.

# Usage: 
Example Endpoints:
1. Books Management:
Add a book: POST /books
Get all books: GET /books

2. Shopping Cart:
Add a book to cart: POST /shoppingcart/add/{shoppingCartId}/{bookId}/{quantity}
Get cart details: GET /shoppingcart/{shoppingCartId}

3. Checkout:
Calculate total: GET /checkout/{shoppingCartId}/total

4. Auth:
Register: POST /Auth/register sample payload '{"username": "test", "firstName": "test","lastName": "test","email": "test@sample.com","password": "Pas@w0rd1"}'
Login: POST /Auth/login admin already seeded payload '{"username": "admin", "password": "Admin@123"}'
6. Trace

Authentication:
Users can register and login using the /auth/register and /auth/login endpoints. Once logged in, use the JWT token for subsequent authorized requests by including it in the request headers:
Authorization: Bearer <token>

Monitoring:
The project includes OpenTelemetry tracing for monitoring API performance. Data is stored in an SQLite database. You can view traces and metrics using the OpenTelemetry collector.

# License: 
This project is licensed under the MIT License.

# Contact Information: 
For questions, reach out at liam.lyu.ll@gmail.com.
