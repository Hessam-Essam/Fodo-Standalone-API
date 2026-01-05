# Fodo-Standalone-API




## Architecture Overview
FODO will be built using Clean Architecture principles and implemented as a .NET 10 microservices-based system, ensuring clear separation of concerns, high maintainability, and long-term scalability.

The architecture is organized into independent services, each responsible for a single business capability, enabling independent deployment, scaling, and evolution.

## Clean Architecture Layers

- Each microservice follows the Clean Architecture pattern:

### 1. Domain Layer

- Contains core business entities, value objects, and domain rules

- Completely independent of frameworks and infrastructure

- Examples: Role, User, Order, Item, InventoryTransaction

### 2. Application Layer

- Implements business use cases and workflows

- Defines service contracts, commands, queries, and validations

- Coordinates domain logic without knowing infrastructure details

### 3. Infrastructure Layer

- Handles technical concerns such as:

- Database access (SQL Server)

- External integrations

- Messaging and caching

- Implements repositories and data persistence

### 4. Presentation Layer

Exposes APIs consumed by:

- Web Portal

- POS Tablet

Uses RESTful endpoints with authentication and authorization enforcement
