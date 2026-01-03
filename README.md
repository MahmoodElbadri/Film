# 🎬 FilmHub — Movie Management System

FilmHub is a full-stack web application built with **.NET 8 Web API** and **Angular 17**, designed to manage movies, reviews, and user profiles with integrated caching, authentication, and email notifications.

---

## 🚀 Features

- 👥 **Authentication & Authorization**
  - JWT-based Auth (Login / Register)
  - Role-based access (Admin, User)
- 🎞️ **Movies Management**
  - Create, update, delete, and view movies
  - Pagination, search, and filtering
  - Image support for movie posters
- 🌟 **Reviews System**
  - Users can add ratings and comments for movies
- 👤 **User Profile**
  - Editable name, password, and profile image
- ❤️ **Watchlist**
  - Add or remove movies from personal watchlist
- 📧 **Email Notifications**
  - Send transactional emails on new movie creation or updates
- ⚡ **Redis Caching**
  - Improved performance for movies and profiles
- 🎨 **Modern UI**
  - Built with Angular 17 + Bootstrap for responsive design

---

## 🛠️ Tech Stack

### 🔹 Backend
- **.NET 8 Web API**
- **Entity Framework Core**
- **AutoMapper**
- **Redis (StackExchange.Redis)**
- **JWT Authentication**


---

## 🧱 Project Structure

```
FilmHub
├── Film.Api                → API Controllers & Services
├── Film.Application        → DTOs & Interfaces
├── Film.Domain             → Entities & Models
└──  Film.Infrastructure     → Data Access & External Services
```

---

## ⚙️ Setup Instructions

### Backend
```bash
cd Film.Api 
dotnet restore
dotnet ef database update
dotnet run
```

---

## 📊 Dashboard Features

- Movie statistics and analytics
- Genre distribution charts
- Recent movies overview

---

## 🚀 API Endpoints

- `https://localhost:7294/swagger` - API Documentation
- `http://localhost:4200` - Frontend Application

---

## 👨‍💻 About the Developer

**Badri** - Full Stack Developer
- Experience with Angular and .NET technologies
- Focused on collaborative development and code review
- Currently working on large-scale Angular and .NET projects

---

⭐ *A comprehensive movie management solution built with modern web technologies*
