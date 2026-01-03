# GCSE Essay Marker

AI-powered essay marking application for GCSE English Literature, using Claude API with extended thinking for deep, insightful feedback.

## Features

- **AI-Powered Marking**: Uses Claude Sonnet 4.5 with extended thinking for thorough analysis
- **GCSE-Aligned**: Feedback based on official AQA marking criteria
- **Multiple Texts**: Support for Frankenstein, Macbeth, and Lord of the Flies
- **PDF Upload**: Drag-and-drop upload of scanned handwritten essays
- **Detailed Feedback**: Grade bands, criterion-by-criterion assessment, specific examples
- **Export Options**: Download feedback as PDF or Word document
- **Secure Auth**: Google OAuth with whitelist-based access control

## Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core 8 with SQL Server
- Google OAuth authentication
- HttpClient for Claude API integration

### Frontend
- Angular 17 with standalone components
- Tailwind CSS for styling
- jsPDF and docx.js for document export

## Quick Start

### Prerequisites
- .NET 8 SDK
- Node.js 20+
- Google OAuth credentials
- Anthropic API key

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/peter-hollis-orkastrate/gcse-marking.git
   cd gcse-marking
   ```

2. **Configure the API**

   Edit `api/GcseMarker.Api/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tcp:your-server.database.windows.net,1433;Initial Catalog=your-db;User ID=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
     },
     "Anthropic": {
       "ApiKey": "sk-ant-your-api-key"
     },
     "Google": {
       "ClientId": "your-client-id.apps.googleusercontent.com",
       "ClientSecret": "your-client-secret"
     },
     "FrontendUrl": "http://localhost:4200",
     "AllowedOrigins": ["http://localhost:4200"]
   }
   ```

3. **Run the API**
   ```bash
   cd api/GcseMarker.Api
   dotnet run
   ```

4. **Run the Frontend**
   ```bash
   cd frontend
   npm install
   npm start
   ```

5. **Access the app**
   - Frontend: http://localhost:4200
   - API: http://localhost:5000
   - Swagger: http://localhost:5000/swagger

## Configuration

### API (appsettings.json)

| Setting | Description |
|---------|-------------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string (ADO.NET format) |
| `Anthropic:ApiKey` | Claude API key from Anthropic |
| `Google:ClientId` | Google OAuth client ID |
| `Google:ClientSecret` | Google OAuth client secret |
| `FrontendUrl` | Frontend URL for OAuth redirect (e.g., `http://localhost:4200`) |
| `AllowedOrigins` | Array of CORS allowed origins for frontend |

### Frontend (environment.ts)

| Setting | Description |
|---------|-------------|
| `apiUrl` | API base URL (e.g., `http://localhost:5000/api`) |

### Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create OAuth 2.0 credentials
3. Add authorized JavaScript origins:
   - `http://localhost:5000` (dev)
   - `https://gcse-marker-api.azurewebsites.net` (prod)
4. Add authorized redirect URIs:
   - `http://localhost:5000/signin-google` (dev)
   - `https://gcse-marker-api.azurewebsites.net/signin-google` (prod)

### Adding Users

Users must be pre-registered in the database to access the application:

```sql
INSERT INTO Users (Email, Name, Enabled, IsAdmin)
VALUES ('teacher@school.edu', 'Teacher Name', 1, 0);
```

## Database

The application connects to an existing Azure SQL database. Tables required:

```sql
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [Name] NVARCHAR(255) NULL,
    [Enabled] BIT NOT NULL DEFAULT 1,
    [IsAdmin] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [LastLoginAt] DATETIME2 NULL
);

CREATE TABLE [dbo].[UsageLogs] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [SkillUsed] NVARCHAR(100) NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [FK_UsageLogs_Users] FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | Basic health check |
| GET | `/api/health/ready` | Readiness check with DB status |
| GET | `/api/skills` | List available marking skills |
| POST | `/api/mark` | Submit essay for marking |
| GET | `/api/auth/google` | Initiate Google OAuth |
| GET | `/api/auth/me` | Get current user info |
| POST | `/api/auth/logout` | End user session |

## Testing

**API Tests**
```bash
cd api
dotnet test
```

**Frontend Tests**
```bash
cd frontend
npm test
```

## Deployment

Both applications are deployed to Azure App Service (Windows) using GitHub Actions with Azure Deployment Center integration.

### Azure App Service Settings

Configure these in Azure Portal > App Service > Configuration > Application settings:

| Setting | Description |
|---------|-------------|
| `ConnectionStrings__DefaultConnection` | Azure SQL connection string (ADO.NET format - see example below) |
| `Anthropic__ApiKey` | Claude API key from Anthropic |
| `Google__ClientId` | Google OAuth client ID |
| `Google__ClientSecret` | Google OAuth client secret |
| `FrontendUrl` | Frontend URL for OAuth redirect (e.g., `https://gcse-marker-frontend.azurewebsites.net`) |
| `AllowedOrigins__0` | Frontend URL for CORS (e.g., `https://gcse-marker-frontend.azurewebsites.net`) |

**Note:** Double underscores (`__`) represent nested configuration in Azure. For arrays, use `__0`, `__1`, etc.

**Connection String Format (ADO.NET):**
```
Server=tcp:your-server.database.windows.net,1433;Initial Catalog=your-db;User ID=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### Deployment Triggers

- **API**: Deploys automatically when changes are pushed to `initial-build` branch in `api/` directory
- **Frontend**: Deploys automatically when changes are pushed to `initial-build` branch in `frontend/` directory

### Google OAuth Callback URL

Add to Google Cloud Console > APIs & Services > Credentials:
- **Authorized JavaScript origins:** `https://gcse-marker-api.azurewebsites.net`
- **Authorized redirect URIs:** `https://gcse-marker-api.azurewebsites.net/signin-google`

## Architecture

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   Angular SPA   │────▶│  ASP.NET Core   │────▶│   Claude API    │
│   (Frontend)    │     │      API        │     │ (Anthropic)     │
└─────────────────┘     └────────┬────────┘     └─────────────────┘
                                 │
                                 ▼
                        ┌─────────────────┐
                        │   Azure SQL     │
                        │   Database      │
                        └─────────────────┘
```
