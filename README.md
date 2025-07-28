# ParkLite

A lightweight, self-contained Parking Account Management System for condominiums.  
Built from scratch using a custom C# UI framework with Raylib, designed to run anywhere.

## 🔧 Tech Stack

- **Backend:** ASP.NET Core API
- **Frontend:** Custom UI built with Raylib and .NET (no browser required)
- **Database:** SQLite
- **Testing:** xUnit (with unit tests for core domain logic)

## 🚀 Features

- Full CRUD for:
  - Parking Accounts
  - Contacts (Family Members)
  - Vehicles
- Inactive batch processor (CTE with 1-second delay per batch of 50)
- Fully self-hosted and embeddable
- Preloaded with 10+ dummy accounts
- Fast, responsive, and cross-platform

## 🧠 Why This Approach?

Most developers reach for standard browser-based UIs. I went in the opposite direction.

This is a **kiosk-grade native UI**, ideal for:
- Touchscreen terminals in condo lobbies
- Security desks
- Embedded hardware

## 🧪 Unit Tests

- Included for `AccountService` logic.
- Tests validate account creation, validation, and vehicle/contact relationships.

## 🗃️ Data Handling

- Inline SQL + stored procedures only.
- Efficient and readable command structure.
- Includes batch CTE example with delay handling.

## 📁 Run It

```bash
dotnet run --project src/ParkLite.API
dotnet run --project src/ParkLite.UI
