# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MovieDB is a full-stack application for tracking movies, theater plays, and concerts. It consists of:
- **MovieDB.Api/** - ASP.NET Core 9 Web API with SQLite database
- **MovieDB.Client/** - Vue 3 + TypeScript PWA frontend
- **MovieDB.Tests/** - xUnit integration and unit tests

## Development Commands

### Backend (API)
```bash
# From MovieDB.Api/ directory
dotnet run                    # Start development server
dotnet ef database update     # Apply database migrations
dotnet test                   # Run tests (from root)
```

### Frontend (Client)
```bash
# From MovieDB.Client/ directory
npm run dev                   # Start development server (Vite)
npm run build                 # Build for production
npm run serve                 # Preview production build
```

## Architecture Overview

### Backend Architecture
- **Minimal APIs pattern** with endpoints organized in separate files
- **JWT authentication** with refresh tokens
- **Repository/Service pattern** with dependency injection
- **Entity Framework Core** with SQLite and automatic migrations
- **User-scoped data isolation** - all entities belong to authenticated users

### Frontend Architecture
- **Vue 3 Composition API** with `<script setup>` syntax
- **Hybrid state management**: Vuex for client state, TanStack Query for server state
- **Route guards** for authentication protection
- **PWA-ready** with service worker and manifest
- **Tailwind CSS 4** for styling

### Key Patterns
- TypeScript interfaces for type safety across frontend/backend boundaries
- Soft delete pattern with `DeletedAt` fields
- Environment-based configuration with dotenv support
- Clean separation between pages (route components) and reusable components

## Project Structure

### API Structure
- `App/Endpoints/` - Minimal API endpoint definitions
- `App/Services/` - Business logic layer
- `App/Models/` - Entity classes
- `App/Http/Requests|Responses/` - DTOs for API communication
- `Bootstrap/Startup.cs` - Service configuration and middleware setup

### Client Structure
- `src/pages/` - Route components organized by feature
- `src/components/` - Reusable UI components
- `src/api/` - API service layer with type-safe methods
- `src/types/` - TypeScript interfaces matching API DTOs
- `src/services/` - Router, store, and utility services

## Environment Setup

### Required Configuration
- **API**: Set JWT secret and SMTP credentials in `appsettings.Development.json` or environment variables
- **Client**: Copy `.env.example` to `.env.local` and configure API endpoint

### Prerequisites
- .NET 9 SDK for backend
- Node.js/npm for frontend
- Entity Framework CLI tools: `dotnet tool install --global dotnet-ef`