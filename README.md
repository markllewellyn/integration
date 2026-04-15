# Multi-Tenant Commerce Integration Platform (Production System Excerpts)

## Overview

This repository contains selected code excerpts, models, and supporting documentation from a production multi-tenant commerce integration platform built in C# using Azure Functions.

The system was developed as part of a third-party enterprise integration solution responsible for ingesting, transforming, and processing commerce-related data across multiple tenant environments.

The platform handles multiple commerce domains including:
- Orders
- Order items
- Shipping and fulfilment data
- Returns
- Cancellations
- Product and item-level data

This repository does not represent a full application. Instead, it provides a curated set of architectural components and implementation patterns used within a broader production system.

---

## System Context

The full production system supports:

- Multi-tenant orchestration across dynamically configured tenants
- External API integration for commerce data (orders, returns, cancellations, and product data)
- Stateful incremental processing per tenant (time-window based execution)
- Secure configuration management using Azure Key Vault
- Hybrid persistence across Cosmos DB, blob storage, and relational databases
- Resilient execution patterns including retry, throttling, and exponential backoff
- Tenant-driven configuration controlling execution behaviour and external endpoints

---

## Architecture Overview

The system follows a layered, cloud-native architecture:

- Azure Functions (Orchestration Layer)
- Tenant Configuration Layer
- API Integration Layer
- Processing & Transformation Layer
- Persistence Layer (Database, Cosmos DB, Blob Storage)

---

## Repository Structure

documents/
- ArchitectureDiagram.png
- Project.bmp
- Project.png
- ok.cs

models/
- OrderItem.cs
- ShippingAddress.cs
- TenantConfiguration.cs

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

### Multi-Tenant Orchestration
Azure Function-based orchestration engine that:
- Retrieves active tenants from configuration sources
- Calculates incremental processing windows per tenant
- Executes tenant-specific commerce data ingestion
- Persists processing state for incremental sync

---

### Tenant Configuration Model
A central configuration model enabling dynamic behaviour per tenant:
- API endpoints per tenant
- Execution timing and retry limits
- Feature flags and processing controls
- State tracking for incremental processing

---

### API Integration Layer
Responsible for:
- External commerce API communication
- Authentication token management
- Rate limiting and throttling control
- Retry and backoff strategies

---

### Secure Configuration Management
Uses Azure Key Vault for:
- Secure secret retrieval
- Environment-aware configuration
- Local development fallback support

---

### Data Persistence Layer
Supports multiple storage strategies:
- Relational database persistence
- Cosmos DB for configuration/state
- Blob storage for tenant configuration files

---

## Models

### TenantConfiguration
Defines tenant-specific behaviour, including:
- API endpoints
- processing state (e.g. OrdersCreatedAfter)
- execution timeouts
- feature flags

### OrderItem
Represents order line items from commerce systems

### ShippingAddress
Represents shipping data per order

---

## Engineering Highlights

- Multi-tenant distributed architecture
- Azure Functions orchestration model
- Configuration-driven execution design
- Secure secret management via Key Vault
- Robust API retry and throttling strategy
- Separation of concerns across integration layers
- Multi-domain commerce processing (orders, returns, cancellations)
- Hybrid persistence strategy (Database, Cosmos DB, Blob Storage)
- Production-grade logging and resilience patterns

---

## Architecture Diagram

See:
documents/ArchitectureDiagram.png

---

## Confidentiality Statement

This repository contains sanitised excerpts of a production system built for a third-party enterprise environment.

All sensitive identifiers, proprietary logic, and production configuration details have been removed or abstracted.

This project is intended to demonstrate:
- engineering approach
- architecture design
- implementation quality

rather than reproduce the full production system.

---

## About

Senior Software Engineer specialising in:
- C# / .NET backend systems
- Azure Functions and cloud integration
- Multi-tenant distributed processing systems
- Enterprise API integration platforms
- Configuration-driven architectures
- Hybrid persistence patterns
