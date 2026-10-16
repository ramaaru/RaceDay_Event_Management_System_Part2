## Project overview

RaceDay is a web-based event management API designed for South African road-running, walking, and cycling events. It supports event organisers who create and manage events and participants who register, enrol in event categories, and view their enrolments and results.

This repository documents the planning and database design completed in **Part 1**, together with the ASP.NET Core Web API implementation and testing work completed and updated in **Part 2**.

## Part 1 — Analysis, planning, and database design

Part 1 established the system requirements, data model, and proposed API structure used to guide the implementation.

### Main planning deliverables

- **Entity Relationship Diagram (ERD):** defines the main entities and how they relate.
- **Database design and SQL script:** `docs/RaceDay_Database.sql` contains the SQL database schema.
- **API endpoint plan:** `docs/API_ENDPOINT_PLAN.md` lists the HTTP method, route, purpose, access role, request body, and expected response for the planned API endpoints.
- **Role and access planning:** distinguishes public operations from Participant and Organiser operations.

### Main data entities

The implementation's data model includes:

- `User` — account, contact, password-hash, and role information.
- `Event` — event details such as name, description, date, location, distance, and type.
- `Category` — event category information, including distance and type.
- `EventCategory` — associates an event with a category and its entry fee.
- `EventEnrolment` — records a participant's enrolment in an event category, enrolment date, race number, and status.
- `Result` — records a participant's event result, including position/time-related information.
- `Route` — route details for an event, including distance and elevation gain.
- `Weather` — weather information associated with an event.

Key relationships include organisers creating events; events being associated with categories through `EventCategory`; users enrolling in event categories through `EventEnrolment`; results being linked to enrolments; and route/weather records being linked to events.

Part 2 — API implementation
Technology stack
- C# and ASP.NET Core Web API targeting .NET 8
- Entity Framework Core (Code First)
- SQL Server for application persistence
- EF Core InMemory provider for isolated automated tests
- Session-based authentication
- Role checks for Organiser and Participant
- Swagger/OpenAPI for API exploration
- xUnit for automated tests
- GitHub Actions workflow for restore, build, and test automation
## Implemented functionality
- Account registration and login
- Password hashing and password verification through PasswordService
- Session values for the authenticated user's ID and role
- Logout and current-user/profile endpoints
- Profile viewing and updating
- Event browsing and organiser event management
- Category management and attaching categories to events
- Participant enrolment and enrolment status tracking
- Result management and result retrieval
- Event route management
- Event weather records
- Role-based checks to prevent participants from using organiser-only operations
## Authentication and authorisation behaviour
The API uses ASP.NET Core session state. A successful login stores values such as UserID and Role in the session. Protected controllers use the shared SessionControllerBase to check whether a user is logged in and whether the user's role is permitted.
# Expected access-control behaviour:
- An unauthenticated request to an organiser-only operation should receive 401 Unauthorized.
- An authenticated Participant attempting an Organiser-only operation should receive 403 Forbidden.
- An authenticated Organiser should be allowed to create and manage their own events.
- An authenticated Participant should be allowed to enrol in an available event category, subject to validation and duplicate-enrolment rules.
The session cookie must be retained and sent with subsequent requests. When running integration tests, each client should log in and then use the same cookie-aware client for protected requests.
# Database and Entity Framework Core updates
- RaceDayDbContext registers the application's entities using DbSet properties.
- Fluent configuration defines unique email addresses, required/max-length fields, decimal precision, and entity relationships.
- EventEnrolment uses EnrolmentID as its primary key.
- The RaceDay route model is aliased as RaceDayRoute where needed to avoid a naming conflict with Microsoft.AspNetCore.Routing.Route.
- SQL Server is configured for normal application use; the automated test factory replaces the context with an EF Core InMemory database so tests do not depend on a local SQL Server instance.

## Change summary
Part 1
- Documented system purpose, major entities, relationships, SQL database design, and proposed API endpoints.
- Defined Participant and Organiser access expectations.
Part 2 updates
- Documented the .NET 8 ASP.NET Core API, EF Core database context, session-based login, and role checks.
- Recorded the EventEnrolment.EnrolmentID primary-key configuration.
- Documented the RaceDayRoute alias used to resolve the Route type-name ambiguity.
- Added build/test commands, integration-test guidance, and GitHub Actions notes.

  ## YouTuBe LinK :  https://youtu.be/rEsaiOlUY8E?si=wjFgOHWhCzjnS7tu
