# 🚀 FoundationAuth - Strict N-Layer .NET Auth API

This project serves as a highly structured and scalable starting point for building robust user management functionality (Registration, Login). It strictly adheres to **Traditional N-Layer Architecture** principles, ensuring clear separation of concerns and maintainability.

## ✨ Key Features & Technologies

* **Architecture:** Strict N-Layer (API -> Business -> DataAccess -> Domain)
* **Approach:** **Code First** Development with Entity Framework Core
* **Platform:** .NET Core Web API
* **Database:** Entity Framework Core (EF Core) and MSSQL
* **Security:** Strong Password Hashing using **BCrypt.Net-Next**
* **Authentication:** **JWT (JSON Web Token)** for secure session management
* **Data Handling:** **DTOs** (Data Transfer Objects) are isolated within the Business layer.

## 📁 Project Structure and Dependencies

This architecture follows a strict, one-way dependency flow:

| Project | Layer | Responsibility | Key Principle |
| :--- | :--- | :--- | :--- |
| **Api** | Presentation | Handles HTTP requests and configures services. | **Depends ONLY on Business.** |
| **Business** | Business Logic | Contains core logic: hashing, JWT creation, and DTO handling. | **Depends ONLY on DataAccess.** |
| **DataAccess** | Data Access | Manages data persistence (EF Core Context, Migrations). | **Depends ONLY on Domain.** |
| **Domain** | Domain Model | Contains core entities (e.g., `User`). | **Has NO Dependencies.** |

## 🛠️ Setup and Installation

1.  **Prerequisites:** .NET SDK (6.0 or higher), SQL Server (or LocalDB), Git.
2.  **Clone the Repository:**
    ```bash
    git clone [REPO_URL]
    cd FoundationAuth
    ```
3.  **Restore Dependencies:**
    ```bash
    dotnet restore
    ```
4.  **Database Configuration:**
    * Set your MSSQL **Connection String** in `Api/appsettings.json`.
    * **Run Initial Migration** (Migrations will be created in the DataAccess project):
        ```bash
        # (Once migrations are created)
        dotnet ef database update --project DataAccess
        ```
5.  **Run the API:**
    ```bash
    dotnet run --project Api
    ```

---

## Git Commit'i

Bu güncellemeyi Git geçmişimize ekleyelim.

### Terminal Komutları (Git)

```bash
# README'yi stage et
git add README.md

# Commit
git commit -m "docs: Update README to reflect strict N-Layer architecture

- Clarified the strict one-way dependency flow (API -> Business -> DataAccess -> Domain).
- Updated the Project Structure table to detail dependency rules."
