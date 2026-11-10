# RaceDay - Event Management System
## System Overview
RaceDay is a full-stack web-based event management system designed for the South African road running, walking, and cycling community.
## User Roles
- **Organiser**: Can create/edit/delete events, manage categories, capture participant results, and view enrolments.
- **Participant**: Can browse events, enter events by selecting a category, view their own enrolments, and track personal results.
## Part 2: API Setup and Run Instructions
This API is built using ASP.NET Core Web API in C#, connected to a SQL Server database using Entity Framework Core.
### How to run the API locally:
1. Ensure you have the .NET SDK and SQL Server installed.
2. Open a terminal and navigate to the API folder:
  `cd RaceDayApi`
3. Apply the database migrations (if not already applied):
  `dotnet ef database update`
4. Run the API:
  `dotnet run`
5. Open your browser and navigate to the Swagger UI link provided in the terminal (e.g., `https://localhost:xxxx/swagger`) to test the endpoints.
### Authentication & Security
- Passwords are hashed using **BCrypt** before being stored in the database.
- Session management is used to track the authenticated user's ID and Role.
- Role-based access control is enforced at the API level (e.g., only Organisers can create events).

## Part 3: Docker Setup
To run the MVC application inside a Docker container:
1. Ensure Docker Desktop is running.
2. Ensure the RaceDayApi is running locally on port 5000 (or update the environment variable below).
3. Build the image:
  `docker build -t raceday-mvc -f RaceDayMvc/Dockerfile .`
4. Run the container:
  `docker run -d -p 8080:8080 -e ApiSettings__BaseUrl="http://host.docker.internal:5000" --name raceday-container raceday-mvc`
5. Open your browser to: `http://localhost:8080`
## Video Presentation
[Watch the Part 3 Walkthrough Here]