# FoundationAuth API (.NET 9)

This project provides a **robust and foundational User Authentication and Authorization API**, built using a modern **Layered Architecture (N-Tier)** approach in **ASP.NET Core**.

It is designed to serve as a **secure, reusable core** for any larger application requiring authenticated user management.

---

## 🚀 Key Features

- **Architecture:** Strict separation across 4 Layers (**Domain**, **DataAccess**, **Business**, **API**) ensuring maintainability and scalability.  
- **Authentication:** Uses industry-standard **JWT (JSON Web Token)** for secure session management.  
- **Security:** Implements **BCrypt** for password hashing to ensure secure credential storage.  
- **Authorization:** Role-Based Access Control (**RBAC**) for granular endpoint protection (e.g., Admin, User).  
- **Data Management:** Built with **EF Core Code-First** approach supporting **MSSQL (LocalDB)**.  
- **Data Integrity:** **AutoMapper** is used for safe and decoupled data transfer between layers (Entity ↔ DTO).  
- **Configuration Security:** Sensitive values such as connection strings and JWT keys are managed securely using **User Secrets** (not stored in source control).  

---

## 🏗️ Architecture Layers

| Layer | Responsibility | Dependencies |
|-------|----------------|---------------|
| **API (Presentation)** | Handles HTTP requests, configures infrastructure (Dependency Injection, JWT, Swagger). | Business |
| **Business (Service)** | Implements core business logic (Register, Login, Token Generation). Uses DTOs. | DataAccess, Domain |
| **DataAccess (Data)** | Manages database operations (CRUD) and EF Core context. | Domain |
| **Domain (Core)** | Defines core entities (e.g., User) and enums. | None |

---

## 🛠️ Setup and Installation

This project requires the **.NET SDK (targeting .NET 9)** and a running instance of **SQL Server LocalDB**.

### Step 1: Install Dependencies

From the project root directory (where the `.sln` file is located), restore all NuGet packages:

```bash
dotnet restore
```

---

### Step 2: Configure Secure Secrets

User Secrets are used to prevent sensitive values such as the JWT secret key and database passwords from being committed to Git.

Navigate to the **Api** project directory and run the following commands:

```bash
# 1. Initialize the User Secrets feature
dotnet user-secrets init

# 2. Set the JWT Secret Key (MUST match the key name in appsettings.json)
dotnet user-secrets set "JwtSettings:SecretKey" "This-should-be-a-very-long-and-secret-key-at-least-16-characters-long"
```

---

### Step 3: Create the Database (Migration)

The database schema must be applied to your LocalDB instance. Run the following commands from the project root:

```bash
# 1. Create migration files (if not already created)
dotnet ef migrations add FinalSetup --project DataAccess --startup-project Api

# 2. Apply the schema and create the database in LocalDB
dotnet ef database update --project DataAccess --startup-project Api
```

> **Note:**  
> If you encounter a connection error, verify that the `Server=` value in your `appsettings.json` file matches your local SQL Server/LocalDB instance (e.g., `(localdb)\MSSQLLocalDB`).

---

### Step 4: Start the API

Once everything is configured, start the application. The **Swagger UI** should open automatically in your browser.

```bash
dotnet run --project Api
```

---

## 🔑 API Endpoints

Use the Swagger UI to explore and test the API functionality.

| Method | Endpoint | Description | Authorization Required |
|--------|-----------|-------------|--------------------------|
| **POST** | `/api/Auth/register` | Creates a new user account (default role: User). | ❌ No |
| **POST** | `/api/Auth/login` | Authenticates a user and returns a JWT token. | ❌ No |
| **GET** | `/api/Auth/profile` | Retrieves the authenticated user’s profile. | ✅ JWT Token |
| **PUT** | `/api/Auth/profile` | Updates username, email, or password. | ✅ JWT Token |
| **GET** | `/api/Auth/admin-test` | Restricted endpoint accessible only to Admin role. | ✅ JWT + Role Check |

---

## 🧪 Testing Authorization

1. Log in using `/api/Auth/login` and copy the generated **JWT token**.  
2. In Swagger, click the **Authorize** button and paste the token in this format:  

   ```
   Bearer [your_token]
   ```

3. Attempt to call the `/api/Auth/admin-test` endpoint.  
   - A regular user should receive **403 Forbidden**, confirming that role-based security is enforced correctly.

---

## 🧩 Technologies Used

- **.NET 9**
- **Entity Framework Core**
- **ASP.NET Core Web API**
- **AutoMapper**
- **BCrypt.Net**
- **JWT Authentication**
- **Swagger UI**

---

## 🧱 Future Enhancements

- Refresh token mechanism  
- Two-factor authentication (2FA)  
- Centralized exception handling and logging  
- Unit and integration testing coverage  

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).

---

**Developed with ❤️ using ASP.NET Core**
**Furkan Emre SAYGIN**