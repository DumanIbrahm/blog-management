
Postly is a modern and user-friendly blog management system developed with **ASP.NET Core MVC**, **Entity Framework Core**, and **Bootstrap**. It allows users to create, edit, and manage their blog posts with support for categories, images, comments, and profile customization.

---

## 🚀 Features

- 🔐 User Authentication (Register / Login / Logout)
- 👤 User Profile Management (Update display name and profile photo)
- 📬 Comment System (with popup form)
- 🖼️ Image Upload for Blog Posts
- 📚 Category Filtering + Category Management
- 🔍 Live AJAX Search for Blogs
- 📄 Blog Pagination
- 💾 EF Core with Code-First & Migrations

---

## 🧪 Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server / SQLite (configurable)
- Identity for Authentication
- Bootstrap 5 & Icons
- JavaScript + Fetch API

✨ Features
In this blog web application, users can perform the following actions:

📝 Create Blog Posts: Write and publish blogs with a title, content, category, and optional image.
📋 Browse & Search Blogs: View all published blog posts and filter them by keyword using the search bar.
🧠 Filter by Category: Easily browse posts by selecting a specific category.
📄 View Blog Details: See the full content, image, category, and author details of a blog post.
💬 Add Comments: Logged-in users can write and post comments under blog posts.
✏️ Edit & Delete Blogs: Users can edit or delete only the blog posts they created.
👤 User Authentication: Register a new account or log in with an existing one.
⚙️ Update Profile: Change your display name and upload a profile picture from the profile page.
🔒 Change Password: Update your account password securely.
📚 Manage Categories: View, create, edit, or delete blog categories from the category management section.

🛡️ Admin Panel
Postly includes a fully-featured Admin Panel to help administrators manage the application efficiently.

🔑 Admin Access
To access the admin panel:
Register as a user.
Manually assign the Admin role to your account in the database (or via code/seed).
Log in and navigate to:
👉 /Admin/Index

🧭 Admin Dashboard Includes:
📊 Statistics Cards for:
Total Blogs
Total Users
Total Categories
Total Comments
📈 Weekly Blog Posts Chart: Displays the number of blog posts published in the last 7 days.
📊 Blog Distribution by Category: Bar chart of blogs grouped by category.

🔧 Admin Management Actions:
📝 View and delete all blog posts.
💬 View and delete all comments (including replies).
📁 View and delete categories (only if no blogs are linked).
👤 View and delete registered users.
⚠️ Admins can moderate content, manage user-generated data, and maintain the integrity of the platform.

## 🖼️ Example Screenshots
<img width="1470" alt="Ekran Resmi 2025-04-16 18 37 12" src="https://github.com/user-attachments/assets/c057565d-0ffa-4cd7-aa74-8cfdb38e472c" />
<img width="591" alt="Ekran Resmi 2025-04-16 18 37 54" src="https://github.com/user-attachments/assets/64764783-4554-44a9-abef-0cac773c5999" />
<img width="1470" alt="Ekran Resmi 2025-04-16 18 38 33" src="https://github.com/user-attachments/assets/eda0e2a0-9da2-4d91-97eb-798b46a1c2de" />
<img width="1470" alt="Ekran Resmi 2025-04-16 18 38 52" src="https://github.com/user-attachments/assets/a092a17c-f2d8-435f-bce1-1c5b3f9be18a" />
<img width="1470" alt="Ekran Resmi 2025-04-16 18 40 56" src="https://github.com/user-attachments/assets/e12c0e60-b3d0-4340-b1fa-2a919b040a04" />
<img width="1470" alt="Ekran Resmi 2025-04-16 18 41 05" src="https://github.com/user-attachments/assets/575cc519-cf3d-4299-b2b9-c61a114f6683" />

