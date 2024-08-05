# Online Library System

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Screenshots](#screenshots)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The Online Library System is a web application designed to manage a digital library. It allows users to browse, borrow, and purchase books. The system also includes an admin panel for managing users and book catalogs.

## Features

- **User Authentication**: Registration, login, and OTP verification.
- **Book Catalog Management**: Detailed information about books including title, author, ISBN, published date, category, description, price, and cover image.
- **Borrowing and Purchasing System**: Users can borrow books for a specified period or purchase them.
- **User Roles**: Admin and member roles with role-specific functionalities.
- **APIs**: Check user book status and access PDF content.

## Technologies Used

- **ASP.NET MVC**
- **Entity Framework (EF)**
- **LINQ**
- **SQL Server**
- **ASP.NET Core Identity**
- **HTML/CSS**
- **JavaScript**

## Installation

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/online-library-system.git
    cd online-library-system
    ```

2. **Set up the database**:
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.
   - Run the migrations to create the database schema:
     ```sh
     dotnet ef database update
     ```

3. **Run the application**:
    ```sh
    dotnet run
    ```

4. **Seed the database** (optional):
   - You can add initial data to your database by modifying the `SeedData` method in `DbInitializer.cs`.

## Usage

1. **User Registration**: New users can register by providing a username, email, and password.
2. **OTP Verification**: Upon registration, users will receive an OTP via email for verification.
3. **Login**: Verified users can log in with their credentials.
4. **Browse Books**: Users can browse the book catalog, search, and filter books.
5. **Borrow/Purchase Books**: Users can borrow books for a specific period or purchase them.
6. **Admin Panel**: Admin users can manage users and book catalogs.

## Project Structure

- **Controllers**:
  - `UserController`: Manages user actions such as registration, login, profile management, and book details.
  - `BookController`: Handles book-related actions, including borrowing, purchasing, and displaying book details.
  - `AdminController`: Manages administrative tasks such as user management and book catalog management.

- **Models**:
  - `User`: Includes properties for username, email, password, roles, and balance.
  - `Book`: Comprehensive model with properties for title, author, ISBN, published date, category, description, price, cover image, and PDF file path.
  - `OTP`: Model for handling OTP verification.
  - `Purchase`: Manages purchase transactions.
  - `Borrow`: Manages borrowing records.

- **Views**:
  - `Book Details View`: Displays book details and allows borrowing or purchasing.
  - `Access Denied View`: Informs users of permission issues.
  - `Login View`: User-friendly login interface with validation.
  - `Registration View`: Interface for new user registration with validation.
  - `OTP View`: Guides users through the OTP verification process.

## API Endpoints

- **Check Book Status**:
  - `GET /api/books/status/{userId}/{bookId}`
  - Returns whether the user has borrowed or purchased the book.

- **Read Book**:
  - `GET /api/books/read/{userId}/{bookId}`
  - Returns the PDF content of the book if the user has access.

## Screenshots

_Add screenshots  here._

## Contributing
i will add CONTRIBUTING.md soon
`Contributions are welcome! Please read the [contributing guidelines](CONTRIBUTING.md) for more information.`

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
