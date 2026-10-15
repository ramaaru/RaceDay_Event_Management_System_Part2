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
