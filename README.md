# Multi-Tenant Order Processing System (Production System Excerpts)

## Overview

This repository contains **selected code excerpts, models, and supporting documentation** from a production multi-tenant order processing system built in C# using Azure Functions.

The system was developed as part of a **third-party enterprise integration platform**, responsible for large-scale ingestion, transformation, and processing of order-related data across multiple tenant environments.

This repository does not represent a full application. Instead, it provides a curated set of **architectural components and implementation patterns** used within a broader production system.

---

## System Context

The full production system supports:

- Multi-tenant orchestration across dynamically configured tenants
- External API integration for order, shipping, and reporting data
- Stateful incremental processing per tenant (time-window based)
- Secure configuration management using Azure Key Vault
- Hybrid persistence across Cosmos DB, blob storage, and relational database systems
- Resilient execution patterns including retry, throttling, and backoff strategies

---

## Architecture Overview

The system follows a layered, cloud-native architecture:

- **Azure Functions (Orchestration Layer)**  
  Timer-triggered functions coordinate tenant-based processing workflows.

- **Tenant Configuration Layer**  
  A configuration-driven model that determines:
  - API endpoints
  - processing behaviour
  - execution limits
  - state tracking per tenant

- **API Integration Layer**  
  Handles external system communication, authentication, and request execution with resilience handling.

- **Processing & Transformation Layer**  
  Maps external API responses into internal domain models.

- **Persistence Layer**  
  Stores processed data into downstream systems including:
  relational databases, Cosmos DB, and blob storage depending on tenant configuration.

---

## Repository Structure
