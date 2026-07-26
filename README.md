# 📚 FindMyBook
Hosting Link:https://findmybook.onrender.com/
Frontend:https://github.com/niikita17/FindMyBook

A production-ready full-stack bookstore application built with **ASP.NET Core Web API** and **React.js**. The project demonstrates modern backend development practices including secure authentication, caching, API versioning, rate limiting, logging, and clean architecture.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- JWT Authentication
- Refresh Token Authentication
- HTTP-Only Cookie for Refresh Token
- Role-Based Authorization (Admin/User)

### 📖 Book Management
- Add, Update, Delete Books
- View Book Details
- Search Books
- Category-wise Filtering
- Sorting
- Pagination

### ⚡ Performance
- IMemoryCache
- Cache Invalidation
- Asynchronous Programming (async/await)

### 🛡 Security
- Global Exception Middleware
- FluentValidation
- Rate Limiting
- API Versioning
- CORS Configuration

### 📊 Logging & Monitoring
- Serilog Logging
- Audit Logging

### 🖼 Image Management
- Cloudinary Integration

### 📑 API Documentation
- Swagger / OpenAPI

---


# 🏗 Architecture

```
Client (React)

        │

REST API (ASP.NET Core)

        │

Controllers

        │

Services

        │

Repositories

        │

Entity Framework Core

        │

PostgreSQL (Neon)
```

---

# 📂 Project Structure

```
FindMyBook
│
├── MyBookBackend.API
├── MyBookBackend.Service
├── MyBookBackend.Repository
├── MyBookBackend.Domain
├── MyBookBackend.Common
└── FindMyBookFrontend
```

---

# ✨ Backend Features

- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- RESTful APIs
- SOLID Principles
- Global Exception Handling
- JWT Authentication
- Refresh Tokens
- Role-Based Authorization
- API Versioning
- IMemoryCache
- Rate Limiting
- Audit Logging
- FluentValidation
- Serilog Logging
- Pagination
- Filtering
- Sorting
- LINQ Query Optimization
- Swagger Documentation

---


## Frontend

```bash
cd FindMyBookFrontend

npm install

npm run dev
```

---

# 🔑 Environment Variables

Configure the following values in **appsettings.json**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_DATABASE_CONNECTION"
  },

  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE"
  },

  "Cloudinary": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  }
}
```

---

# 📚 Learning Outcomes

This project demonstrates practical implementation of:

- Clean Architecture
- Repository & Service Pattern
- Authentication & Authorization
- API Versioning
- Caching Strategies
- Rate Limiting
- Logging & Monitoring
- Exception Handling
- Entity Framework Core
- PostgreSQL
- REST API Design
- Secure Backend Development

---

# 👨‍💻 Author

**Nikita Mankape**

Backend Developer | ASP.NET Core | React | PostgreSQL | Azure Learner


