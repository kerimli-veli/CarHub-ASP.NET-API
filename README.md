# CarHub API

**CarHub API** is a RESTful ASP.NET Core Web API project designed for managing car listings and rental operations. It includes features like user authentication, role-based authorization, car listing CRUD operations, image upload, and reservation management. It serves as the backend of a car rental platform.

## 🔧 Technologies Used

- ASP.NET Core Web API (.NET 6+)
- Entity Framework Core
- MSSQL
- AutoMapper
- JWT Authentication
- Repository Pattern & Unit of Work
- FluentValidation
- Cloudinary (for image upload)
- Swagger / Swashbuckle (API documentation)

## 🚀 Features

### ✅ Authentication & Authorization
- JWT-based user authentication
- Role management (Admin and regular User)
- Register / Login endpoints

### 📦 Car Listings Management
- Admin can add, update, and delete car listings
- All users can browse and filter car listings
- Cloudinary integration for image uploading

### 👤 User Management
- View and update user profile
- Track listing and booking history

### 🛒 Reservation & Rental System
- Create a reservation for a car
- Date-based availability check
- Automatic rental duration calculation

## 🗂️ Project Structure
CarHub.API/
│
├── Controllers/         → API endpoints
├── DTOs/                → Data Transfer Objects
├── Entities/            → Database models
├── Interfaces/          → Abstraction layer (repositories, services)
├── Repositories/        → Data access layer
├── Services/            → Business logic layer
├── Helpers/             → Utilities (e.g. JWT token, photo services)
├── Middlewares/         → Exception handling, logging, etc.
└── Program.cs / Startup.cs → Application configuration


