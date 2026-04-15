# Multi-Tenant Commerce Integration Platform (Production Excerpts)

This repository contains selected excerpts from a production integration system demonstrating architecture, patterns, and implementation approaches.  Sensitive and proprietary details have been removed.

## Overview

This repository contains selected code excerpts, models, and supporting components from a production multi-tenant commerce integration platform built in C# using Azure Functions.

The system was developed for a third-party enterprise integration scenario and is responsible for processing commerce-related data across multiple tenants, including:

- Orders
- Order items
- Shipping information
- Returns
- Cancellations
- Product-related data

This is not a full application export.  It is a selected set of representative components showing architecture, design patterns, and implementation approaches.

---

## System Summary

The platform is designed to support:

- Multi-tenant processing using configuration-driven execution
- External commerce API integration
- Incremental data processing per tenant
- Secure secret management using Azure Key Vault
- Resilient API communication (retry, throttling, backoff)
- Hybrid data persistence (relational database, Cosmos DB, blob storage)

---

## Architecture Overview

The system follows a cloud-native, layered approach:

- **Azure Functions** – orchestration and scheduled execution
- **Tenant Configuration Layer** – controls per-tenant behaviour and state
- **API Integration Layer** – external commerce system communication
- **Processing Layer** – transformation of API responses into domain models
- **Persistence Layer** – database / Cosmos DB / blob storage depending on configuration

---

## Repository Structure

documents/
- ArchitectureDiagram.png
- Project.png

models/
- TenantConfiguration.cs
- OrderItem.cs
- ShippingAddress.cs

snippets/
- 01-MultiTenantOrderProcessing.cs
- 02-TokenService.cs
- 03-KeyVaultService.cs
- 04-FlexibleBoolConverter.cs
- 06-ApiExecutionService.cs
- 07-CosmosDbService.cs
- 08-FunctionStartup.cs

README.md

---

## Key Components

### Multi-Tenant Processing
Azure Function-based orchestration that:
- Retrieves active tenants
- Calculates processing windows per tenant
- Executes API calls per tenant
- Maintains incremental sync state

---

### API Integration Layer
Handles:
- Authentication and token management
- External API communication
- Rate limiting and retry logic
- Resilient request execution

---

### Configuration Management
Supports:
- Azure Key Vault integration for secrets
- Environment-based configuration loading
- Local development fallback configuration

---

### Data Persistence
Depending on tenant configuration:
- Relational database storage for transactional data
- Cosmos DB for configuration/state
- Blob storage for tenant configuration files

---

## Models

### TenantConfiguration
Defines per-tenant behaviour including:
- API endpoints
- execution state (e.g. incremental sync timestamps)
- processing settings and limits

### OrderItem
Represents order line-level commerce data

### ShippingAddress
Represents order shipping details

---

## Notes

- All sensitive configuration values and proprietary business logic have been removed
- This repository contains representative excerpts only
- Intended to demonstrate architecture and implementation approach
