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
  - Real-time updates (ready for SignalR integration)
- 👤 **User Profile**
  - Editable name, password, and profile image
  - Images stored securely in `wwwroot/profile-images`
- 📧 **Email Notifications**
  - Send transactional emails on new movie creation or updates (via Brevo API)
- ⚡ **Redis Caching**
  - Improved performance for movies and profiles
- 🎨 **Modern UI**
  - Built with Angular 17 + Bootstrap + ngx-charts for analytics dashboard

---

## 🛠️ Tech Stack

### 🔹 Backend (API)
- **.NET 8 Web API**
- **Entity Framework Core**
- **AutoMapper**
- **Redis (StackExchange.Redis)**
- **Serilog**
- **Brevo Email SDK**
- **JWT Authentication**

### 🔹 Frontend (Client)
- **Angular 18**
- **RxJS / Signals**
- **ngx-toastr** for notifications
- **ngx-spinner** for loading UI
- **ngx-charts** for dashboard analytics
- **Bootstrap 5** for styling

---

## 🧱 Architecture

