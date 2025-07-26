# WebAPI – Personnel Equipment & Leave Management System

## Overview

**WebAPI** is a modular, enterprise-grade RESTful API built with ASP.NET Core 6.0, designed for managing personnel equipment requests and leave applications within organizations. The solution features robust authentication, role-based authorization, and a clean, extensible architecture suitable for HR, IT, and administrative workflows.

## Architecture & Best Practices

- **Layered Architecture:** The solution is organized into Entities (data models), Repositories (data access), Services (business logic), Presentation (API controllers), and WebAPI (entry point & configuration).
- **ServiceManager & Lazy Loading:** All core services—including `IEquipmentService`, `ILeaveService`, and `IAuthenticationService`—are instantiated using .NET's `Lazy<T>` pattern within the `ServiceManager`. This ensures services are only created when needed, optimizing resource usage and supporting dependency injection best practices.
- **Repository Pattern:** Data access is abstracted via generic and specialized repositories, promoting testability and separation of concerns.
- **DTO & Entity Mapping:** AutoMapper is used for seamless, bi-directional mapping between entities and DTOs, ensuring clean API contracts and encapsulation.
- **Centralized Exception Handling & Logging:** NLog is integrated for robust logging, and custom middleware provides unified error handling across the API.
- **Swagger/OpenAPI:** Comprehensive API documentation and testing via Swagger UI.
- **Extensibility:** The architecture supports easy addition of new features, services, or data models with minimal impact on existing code.

## Authentication on GetAll Endpoints

The `GetAllEquipments` and `GetAllLeaves` endpoints are intentionally left open (no authentication required) to facilitate general listing and reporting scenarios (e.g., dashboards, public statistics). All other endpoints that access user-specific or sensitive data require JWT authentication and, where appropriate, role-based authorization.

## API Endpoints

### Authentication

| HTTP Method | Route                        | Description                | Auth Required |
|-------------|------------------------------|----------------------------|---------------|
| POST        | /api/authentication/register | Register a new user        | No            |
| POST        | /api/authentication/login    | Authenticate and get JWT   | No            |

### Equipment

| HTTP Method | Route                                    | Description                                 | Auth Required | Roles                |
|-------------|------------------------------------------|---------------------------------------------|---------------|----------------------|
| GET         | /api/equipment/GetAllEquipments          | Retrieve all equipment requests             | No            | -                    |
| GET         | /api/equipment/GetOneEquipment/{id}      | Retrieve a specific equipment request by ID | Yes           | Any authenticated    |
| POST        | /api/equipment/CreateOneEquipment        | Create a new equipment request              | Yes           | Any authenticated    |
| PUT         | /api/equipment/UpdateOneEquipment/{id}   | Update an equipment request by ID           | Yes           | Any authenticated    |
| DELETE      | /api/equipment/DeleteOneEquipment/{id}   | Delete an equipment request by ID           | Yes           | Any authenticated    |
| GET         | /api/equipment/MyRequests                | Retrieve current user's equipment requests  | Yes           | Employee, IT, Admin  |
| GET         | /api/equipment/Pending                   | Retrieve pending equipment requests         | Yes           | Admin, IT            |
| PUT         | /api/equipment/{id}/approve              | Approve an equipment request                | Yes           | Admin, IT            |
| PUT         | /api/equipment/{id}/reject               | Reject an equipment request                 | Yes           | Admin, IT            |

### Leave

| HTTP Method | Route                                    | Description                                 | Auth Required | Roles                |
|-------------|------------------------------------------|---------------------------------------------|---------------|----------------------|
| GET         | /api/leaves/GetAllLeaves                 | Retrieve all leave requests                 | No            | -                    |
| GET         | /api/leaves/GetOneLeave/{id}             | Retrieve a specific leave request by ID     | Yes           | Any authenticated    |
| POST        | /api/leaves/CreateOneLeave               | Create a new leave request                  | Yes           | Any authenticated    |
| PUT         | /api/leaves/UpdateOneLeave/{id}          | Update a leave request by ID                | Yes           | Any authenticated    |
| DELETE      | /api/leaves/DeleteOneLeave/{id}          | Delete a leave request by ID                | Yes           | Any authenticated    |
| GET         | /api/leaves/MyRequests?trackChanges={{bool}} | Retrieve current user's leave requests | Yes           | Any authenticated    |
| GET         | /api/leaves/Pending?trackChanges={{bool}}    | Retrieve pending leave requests         | Yes           | Admin                |
| PUT         | /api/leaves/{id}/approve                 | Approve a leave request                     | Yes           | Admin                |
| PUT         | /api/leaves/{id}/reject                  | Reject a leave request                      | Yes           | Admin                |

## User Roles & Permissions

| Role      | Capabilities                                                                                   |
|-----------|-----------------------------------------------------------------------------------------------|
| **Admin** | - Full access to all endpoints<br>- Approve/reject equipment and leave requests<br>- View all requests<br>- Manage users and roles |
| **IT**    | - Approve/reject equipment requests<br>- View all equipment requests<br>- Access own requests |
| **Employee** | - Submit, update, and delete own equipment and leave requests<br>- View status of own requests |

**Notes:**
- Only Admins can approve/reject leave requests.
- Both Admin and IT roles can approve/reject equipment requests.
- Employees can only manage their own requests and cannot approve/reject others' requests.

## Main Entities

- **User:** Identity-based user with first/last name, roles, and authentication credentials.
- **EquipmentItem:** Represents an equipment type (e.g., laptop, phone).
- **EquipmentRequest:** Tracks user requests for equipment, approval status, and related metadata. Includes navigation properties for User, Approver, and EquipmentItem.
- **LeaveRequest:** Tracks user leave applications, type, status, and period. Includes navigation properties for User and LeaveType.
- **LeaveType:** Defines types of leave (e.g., annual, sick).

## DTO & Mapping

- All entities have corresponding DTOs, and AutoMapper is used for robust, bi-directional mapping.
- The `MappingProfile` class defines all mappings, ensuring clean separation between API contracts and internal models.

## Configuration

- **Database:** Connection string is set in `WebAPI/appsettings.json` (default: SQLite file `webapi.db`).
- **JWT:** JWT settings are configurable in `WebAPI/appsettings.json` under `JwtSettings`.
- **Swagger:** API documentation is available at [https://localhost:7012/swagger](https://localhost:7012/swagger).

## Running the Project

1. Ensure .NET 6.0 SDK is installed.
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Apply migrations:
   ```bash
   dotnet ef database update --project WebAPI/WebAPI.csproj
   ```
4. Run the API:
   ```bash
   dotnet run --project WebAPI/WebAPI.csproj
   ```
5. Access Swagger UI in your browser.

## Extensibility & Best Practices

- **Lazy Loading of Services:** All core services, including `IEquipmentService`, are lazy-loaded via the `ServiceManager` using .NET's `Lazy<T>`. This ensures efficient resource usage and supports scalable, testable code.
- **Separation of Concerns:** The solution strictly separates data access, business logic, and presentation layers, making it easy to maintain and extend.
- **Open/Closed Principle:** Adding new features or entities requires minimal changes to existing code, thanks to the use of interfaces, dependency injection, and mapping profiles.
- **Comprehensive Error Handling:** Centralized exception middleware and structured logging ensure reliability and maintainability.

## License

This project is intended for educational and internal use. For production or commercial deployment, please review and update the license as appropriate. 