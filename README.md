# Multi-Tenant Order Processing System (Production Excerpts)

## Overview

This repository contains **selected code excerpts and supporting documentation** from a production multi-tenant order processing system built in C# using Azure Functions.

The system was designed and implemented as part of a **third-party enterprise integration platform**, handling large-scale order ingestion, transformation, and persistence across multiple tenant environments.

This repository does not represent a full application. Instead, it highlights **key architectural components and engineering patterns** used within the broader solution.

---

## System Context

The full production system includes:

- Multi-tenant orchestration across configurable data sources
- External API integrations for order retrieval and enrichment
- Stateful incremental processing per tenant
- Resilient execution with retry and failure handling strategies
- Secure configuration management via Azure Key Vault
- Hybrid persistence across Cosmos DB, Blob Storage, and Oracle databases

---

## Architecture Overview

The system is structured into the following logical layers:

- **Azure Functions (Orchestration Layer)**  
  Timer-triggered functions coordinate tenant processing workflows.

- **Tenant Configuration Layer**  
  Determines tenant source (Cosmos DB, Blob Storage, or local development).

- **API Integration Layer**  
  Handles external system communication and authentication.

- **Processing & Transformation Layer**  
  Maps external order data into internal domain models.

- **Persistence Layer**  
  Writes processed data into downstream systems (Oracle / Cosmos DB).

---

## Included Code Snippets

The repository contains representative excerpts from key areas of the system:

/Snippets  
- 01-MultiTenantOrderProcessing.cs  
- 02-ApiExecutionLayer.cs  
- 03-KeyVaultService.cs  
- 04-CosmosTenantRepository.cs  
- 05-FunctionStartup.cs  

/Docs  
- ArchitectureOverview.md  
- ProcessingFlowDiagram.png  

---

## Engineering Highlights

This system demonstrates:

- Multi-tenant architecture design
- Cloud-native Azure Function orchestration
- Secure secret management using Azure Key Vault
- Resilient API communication patterns (retry, throttling, backoff)
- Separation of concerns across integration layers
- Hybrid persistence strategy across multiple data stores
- Production-grade logging and observability patterns

---

## Confidentiality Statement

This repository contains **sanitised and partial excerpts** of a production system built for a third-party enterprise environment.

All:
- client-specific identifiers  
- proprietary business logic  
- sensitive configuration values  
- and production data structures  

have been removed or abstracted.

The intent is to demonstrate **engineering approach, architecture, and implementation quality**, not to reproduce the full system.

---

## About

Senior Software Engineer specialising in:
- C# / .NET backend systems  
- Azure Functions & cloud integration  
- Multi-tenant distributed processing systems  
- Enterprise API and data pipeline architecture  
