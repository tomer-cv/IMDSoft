# Hackathon1 Web API

A RESTful Web API built with ASP.NET Core 8.0, converted from a console application.

## Features

- RESTful API endpoints
- Swagger/OpenAPI documentation
- Full CRUD operations support

## Prerequisites

- .NET 8.0 SDK or later

## Running the Application

```bash
cd Hackathon1
dotnet run
```

The application will start on `http://localhost:5000` by default.

## API Endpoints

The application exposes the following endpoints through the `SampleController`:

### GET /api/sample
Returns a welcome message.

**Example:**
```bash
curl http://localhost:5000/api/sample
```

**Response:**
```json
{
  "message": "Welcome to Hackathon1 Web API"
}
```

### GET /api/sample/{id}
Retrieves an item by ID.

**Example:**
```bash
curl http://localhost:5000/api/sample/123
```

**Response:**
```json
{
  "id": 123,
  "message": "Retrieved item with ID: 123"
}
```

### POST /api/sample
Creates a new item.

**Example:**
```bash
curl -X POST http://localhost:5000/api/sample \
  -H "Content-Type: application/json" \
  -d '{"name":"test","value":123}'
```

**Response:**
```json
{
  "message": "Item created successfully",
  "data": {
    "name": "test",
    "value": 123
  }
}
```

### PUT /api/sample/{id}
Updates an existing item.

**Example:**
```bash
curl -X PUT http://localhost:5000/api/sample/123 \
  -H "Content-Type: application/json" \
  -d '{"name":"updated","value":999}'
```

**Response:**
```json
{
  "message": "Item 123 updated successfully",
  "data": {
    "name": "updated",
    "value": 999
  }
}
```

### DELETE /api/sample/{id}
Deletes an item by ID.

**Example:**
```bash
curl -X DELETE http://localhost:5000/api/sample/123
```

**Response:**
```json
{
  "message": "Item 123 deleted successfully"
}
```

## Swagger/OpenAPI Documentation

Interactive API documentation is available via Swagger UI at:
```
http://localhost:5000/swagger
```

The OpenAPI specification can be accessed at:
```
http://localhost:5000/swagger/v1/swagger.json
```

## Building the Project

```bash
cd Hackathon1
dotnet build
```

## Project Structure

- `Program.cs` - Application entry point and configuration
- `Controllers/` - API controllers
  - `SampleController.cs` - Sample REST API controller
- `appsettings.json` - Application configuration
- `appsettings.Development.json` - Development-specific configuration

## Adding New Endpoints

To add new endpoints, create a new controller class in the `Controllers/` directory:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace Hackathon1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YourController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Your response" });
    }
}
```

The controller will automatically be discovered and added to the API.
