EventManagementApp
📖 Description
EventManagementApp is a simple master–detail CRUD application designed to manage events and their participants. It allows users to create, update, delete, and view events, while also handling related details such as attendees, schedules, and venues. This project demonstrates practical implementation of CRUD operations with a clean architecture, making it suitable for academic exams, portfolio projects, or as a starter template for real-world event management systems.

🚀 Features
Event Management (Master Table)

Add, edit, delete, and view events

Store event details (name, date, location, description, image, status)

Participant Management (Detail Table)

Register participants for specific events

Track participant details (name, email, phone, ticket type, payment status)

Link participants to events using foreign keys

CRUD Operations

Full Create, Read, Update, Delete functionality for both master and detail entities

Validation for required fields

Data Types Covered

Text (event name, participant name)

Number (ticket price, phone number)

Date (event date, registration date)

Boolean (active/inactive, payment status)

Image (event banner/logo)

Relational (event ↔ participants)

🛠️ Tech Stack
Frontend: .NET MAUI / WinForms / React (depending on implementation)

Backend: .NET Core / ADO.NET / SQLite / MySQL

Database: SQLite (lightweight, exam-friendly)

Language: C# (for MAUI/WinForms) or JavaScript/TypeScript (for React)

📂 Project Structure
Code
EventManagementApp/
│── Models/        # Event & Participant classes
│── Data/          # Database context & CRUD operations
│── UI/            # Forms/Pages for event & participant management
│── Assets/        # Images, icons
│── README.md      # Documentation
⚡ How to Run
Clone the repository:

bash
git clone https://github.com/tonmoyn4-netizen/EventManagementApp.git
Open the project in Visual Studio (for .NET MAUI/WinForms) or VS Code (for React).

Restore dependencies and build the project.

Run the application — you’ll see the Event Management Dashboard.

Start adding events and participants to test CRUD functionality.

🎯 Use Cases
Academic exam project (Master–Detail CRUD demonstration)

Portfolio showcase for job applications

Starter template for real-world event management systems
