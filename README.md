# AuditLogProjectLatest
This project track audit log data

Overview Project

This service exposes User CRUD endpoints and writes detailed audit logs for every create/update/delete.
Stack: .NET 8 + EF Core (SQL Server) + DI. Code-first migrations keep your data safe.

Base URL (local dev): https://localhost:<port>
(open Swagger at /swagger to explore interactively)

Auth & Required Headers

This sample is unauthenticated by default. For audit purposes, you can pass:

X-User-Id (optional) – who performed the action (e.g., rylon.thomas).
If omitted, the service uses the authenticated User.Identity.Name (if configured), else "system".

X-Correlation-Id (optional) – a request trace ID (any GUID/string). Returned in logs to link requests across systems.

MY API 
1. POST	/api/user	Create a user (writes Created audit)
2. GET	/api/user/{id}	Get a user by id
3. PUT	/api/user/{id}	Update a user (writes Updated audit with only changed fields)
4. DELETE	/api/user/{id}	Delete a user (writes Deleted audit)
5. GET	/api/auditlog?entityName=User&entityId={id}&page=1&pageSize=50	List audit log entries

// Data of activty log
{
  "id": 12,
  "entityName": "User",
  "entityId": "1",
  "action": 2,               // 1=Created, 2=Updated, 3=Deleted
  "changesJson": {
    "Email": { "before": "ada@example.com", "after": "ada.l@example.com" }
  },
  "userId": "rylon.thomas",
  "occurredAt": "2025-09-02T15:20:10.123Z",
  "correlationId": "2222-..."
}


Status Codes

201 Created – user created

200 OK – GET success

204 No Content – update/delete success

400 Bad Request – invalid input

404 Not Found – user not found

500 Internal Server Error – unhandled error (see server logs)


Run locally 

Configure SQL Server in appsettings.json.
Create DB:
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update

