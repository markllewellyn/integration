# Multi-Tenant Order Processing Engine (Portfolio Project)

## Overview
This project demonstrates a simplified version of a multi-tenant order processing system built using C# and Azure Functions.

It simulates external API integration, data transformation, and data persistence using clean architecture principles.

All sensitive, proprietary, and production-specific logic has been removed or abstracted to ensure confidentiality.

---

## Architecture

The system follows a layered architecture:

- Azure Function (Timer Trigger) → Orchestration layer  
- Tenant Service → Retrieves active tenants  
- API Client → External system abstraction (mocked)  
- Transformation Layer → Maps external data to internal models  
- Repository Layer → Persists processed data  

See `ArchitectureDiagram.png` for a visual overview.

---

## Key Features

- Multi-tenant processing model  
- External API abstraction (mocked implementation)  
- Retry and error handling logic  
- Clean separation of concerns  
- Structured logging and maintainable architecture  

---

## Code Structure

/Snippets  
- 01-MultiTenantLoop.cs  
- 02-ApiClient.cs  
- 03-RetryLogic.cs  
- 04-Transformation.cs  

/docs  
- ArchitectureOverview.md  

---

## Note on Confidentiality

This project is a sanitised representation of production systems I have worked on.  
All proprietary logic, real data, and external system details have been removed or abstracted to ensure confidentiality.

---

## About

Senior Software Engineer with experience in C#, Azure Functions, and enterprise integration systems, specialising in multi-tenant architectures and backend processing pipelines.
