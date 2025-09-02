# AuditLogProjectLatest
This project track audit log data

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
