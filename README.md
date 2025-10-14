# 🚀 FoundationAuth API: A Secure Starting Point

Hey there! Welcome to the **FoundationAuth** project — your new go-to for building **user authentication, authorization, and email verification systems** in **ASP.NET Core**.  
We've crafted this API with a solid **N-Tier architecture** to give you a strong, secure, and flexible foundation for all your future applications.

---

## ✨ What's Under the Hood?

We've packed this project with some of the best industry standards to ensure it's robust and ready for anything.

### 🧩 Architecture
We keep things clean with a strict **4-layer separation**:
- **Domain**
- **DataAccess**
- **Business**
- **API**

It's all about keeping concerns separate and your code easy to manage.

### 🔐 Authentication
We use **JWT (JSON Web Token)** for secure, stateless sessions.  
No more dealing with clunky session management!

### 🛡️ Security
Your users' data is safe with us.  
We use **BCrypt** to hash passwords, so credentials are **never stored in plain text**.

### 🎯 Authorization
Need to protect admin-only content?  
Our **Role-Based Access Control (RBAC)** lets you lock down endpoints with ease.

### 💾 Data Management
We're all in on **EF Core Code-First with MSSQL (LocalDB)**.  
You define your models in code, and EF Core handles the rest.

### 🧠 Data Integrity
We use **AutoMapper** to ensure data transfer between layers is clean and decoupled.  
Think of it as a smart middleman that keeps everything organized.

### 🔑 Configuration Security
Sensitive info like keys, connection strings, and mail credentials are safely stored using **User Secrets** or in `appsettings.json` (local only).

---

## 📧 Email Verification System

We’ve integrated a **mail verification feature** to enhance account security.

When a user registers, a **verification code** (or clickable link) is automatically sent to their registered email address.  
By clicking the link, users can verify their accounts and activate their profiles.

### 🧬 How It Works

1. A user registers via `/api/Auth/register`.
2. The API sends a unique verification link to the user's email.
3. The user clicks the link → account becomes **verified**.
4. Only verified users can log in or access protected endpoints.

### ⚙️ Email Configuration

To enable email functionality, make sure the following section exists in your `appsettings.json`:

```json
{
  "EmailSettings": {
    "SenderEmail": "your_email@gmail.com",
    "SenderPassword": "your_app_password",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "EnableSsl": true
  }
}
```

> ⚠️ **Important:**  
> - Use an **App Password** instead of your real Gmail password.  
> - To create one, enable 2-Step Verification in your Google Account and generate an App Password.  
> - Never push this file to GitHub! Use **User Secrets** or environment variables in production.

---

## 🛠️ Let's Get It Running

Ready to spin it up? Just follow these simple steps.

### Step 1: Grab the Essentials
Make sure you have the **.NET 9 SDK** and **SQL Server LocalDB** installed.  
Once you're in the project root, run this command to get all the NuGet packages:

```bash
dotnet restore
```

---

### Step 2: Lock Down Your Secrets
Security is our top priority. We'll use **User Secrets** to keep your sensitive data out of source control.  
Just navigate to your **Api** project folder and run:

```bash
# 1. Initialize the secret manager
dotnet user-secrets init

# 2. Set your JWT Secret Key
dotnet user-secrets set "JwtSettings:SecretKey" "This-should-be-a-very-long-and-secret-key-at-least-16-characters-long"

# 3. Set email credentials
dotnet user-secrets set "EmailSettings:SenderEmail" "your_email@gmail.com"
dotnet user-secrets set "EmailSettings:SenderPassword" "your_app_password"
```

---

### Step 3: Bring the Database to Life
This part is a piece of cake. From your project's root directory, run these commands to apply our code-first schema to your database.

```bash
# 1. Create the migration files (if they don't exist yet)
dotnet ef migrations add FinalSetup --project DataAccess --startup-project Api

# 2. Apply the migration and create the database
dotnet ef database update --project DataAccess --startup-project Api
```

💡 **Pro Tip:**  
Ran into a connection error? Just double-check that the `Server=` value in your `appsettings.json` matches the name of your **LocalDB instance**.

---

### Step 4: Time to Launch!
Everything's set. Go ahead and fire up the API!

```bash
dotnet run --project Api
```

Your browser will automatically open the **Swagger UI**, where you can explore every endpoint.

---

## 🗺️ Your API Endpoints

Our API is fully documented and ready for you to play with!

| Method | Endpoint              | What it does                              | Access         |
|--------|------------------------|-------------------------------------------|----------------|
| POST   | `/api/Auth/register`   | Creates a new user account (sends email verification). | 🔓 Public       |
| POST   | `/api/Auth/login`      | Authenticates a verified user and returns a token. | 🔓 Public       |
| GET    | `/api/Auth/verify`     | Verifies a user's email via code or link. | 🔓 Public       |
| GET    | `/api/Auth/profile`    | Fetches the current user’s profile.       | 🔒 Authenticated |
| PUT    | `/api/Auth/profile`    | Updates a user’s profile details.         | 🔒 Authenticated |
| GET    | `/api/Auth/admin-test` | Secret endpoint — only for Admins!        | 🔒 Admin Role   |

---

## 🧪 Quick Test: Email Verification

1. Register a new user with `/api/Auth/register`.  
2. Check your email inbox for the **verification link**.  
3. Click the link — your account becomes verified.  
4. Now log in using `/api/Auth/login`.

✅ **Result:** You should receive a JWT token only after verification.

---

## 💡 What's Next?

Our foundation is strong, but there's always room for growth! Here are a few ideas for future enhancements:

- 🔁 **Refresh Tokens:** Keep users logged in without re-entering passwords.  
- 🔒 **Two-Factor Authentication:** Add an extra layer of security.  
- 📊 **Centralized Logging:** Set up a robust system to monitor all activities.

---

## 📄 License

This project is open-source and available under the **MIT License**.

---

### 👨‍💻 Developed with ❤️ by **Furkan Emre SAYGIN**
