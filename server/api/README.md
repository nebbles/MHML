<h2 align="center"><br>API Specification</h2>

<br>

**Base URL**: ___mhml.greenberg.io___

**Note**: This specification refers heavily to objects defined in the [Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md). 

## Users Collection :

### GET /api/users

**Required**: `None`

| Response code | Response body |
|--|--|
| 200 Ok| `{'usernames': [array of usernames]}`
- This request retrieves a complete list of usernames stored in the database.
- **Note**: the array could be empty.

---

### POST /api/users

**Required**: `'username'` key of the `User` object.


| Response code| Response body |
|:----|:----|
| **201** Created | `{'new user': User object}`
| **409** Conflict| `{"error": "User <username> already exists"}`

This request adds a new `User` object to the database.


## User Resource

### GET /api/users/\<username>

**Required**: `None`

| Response code | Response body |
|--|--|
| **200** Ok| `User object`
| **404** Not Found| `{"error": "User <username> not found"}`
- This request retrieves a single `User` object registered with username `<username>`.
- If no such user exists, a 404 error is thrown.

---

### DELETE /api/users/\<username>

**Required**: `username` key of the `User` object.

| Response code | Response body |
|--|--|
| **204** No Content| `None`
| **404** Not Found| `{"error": "User <username> not found"}`
- This request deletes the `User` object registered with username `<username>` from the database.
- If no such user exists, a 404 error is thrown.



## Sessions Collection :

### GET /api/users/\<username>/sessions

**Required**: `None`

| Response code | Response body |
|--|--|
| 200 Ok| `{"session_ids": [array of session ids]}`
| **404** Not Found| `{"error": "User <username> not found"}`

- This request retrieves a complete list of session ids stored in the database for a particular user.
- **Note**: the array could be empty.
- If no such user exists, a 404 error is thrown.

---

### POST /api/users/\<username>/sessions

**Required**: the following keys of the `Session` object are required:

- `"session_id"`
- `"firmwareRevision"`
- `"selfReported"`
- `"ppg"`
- `"gsr"`

Each of the keys' value must be of the correct type according to the [Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md). 

| Response code | Response body |
|--|--|
| **201** Created | `{"user": User Object,"new session": Session Object}`
| **409** Conflict| `{"error": "Session ID <session_id> for user <username> already exists"}`
| **404** Not Found| `{"error": "User <username> not found"}`
|**400** Bad Request| `{"message": {<some_objec_key>: "Wrong or missing entry"}`

- This request adds a new `Session` object to the database (linked to some user) through `<username>`.
- If a `Session` object in the database has the same `<session_id>`  as the one in request body, a 409 error is thrown.
- If no user registered with `<username>` is found, a 404 error is thrown.
- If the request body does not comply with the `Session` object as defined in [Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md), a 400 error is thrown.



## Session Resource

### GET /api/users/\<username>/sessions/\<session_id>

**Required**: `None`

| Response code | Response body |
|--|--|
| **200** Ok| `Session object`
| **404** Not Found| `{"error": "User <username> not found"}`
| | `{"error": "Session ID <session_id> for user <username> not found"}`
- This request retrieves a single `Session` object registered with id `<session_id>`, linked to a particular user.
- If no such user exists, a 404 error is thrown.
- Similarly, if the user exists but has no `Session` object under `<session_id>`, a 404 error is thrown.

---

### POST /api/users/\<username>/sessions/\<session_id>
**Required**: the following keys of the `Session` object are required:
- `"session_id"`
- `"firmwareRevision"`
- `"selfReported"`
- `"ppg"`
- `"gsr"`
- `"session_id"` key of a `Session` object.

Each of the keys’ value must be of the correct type according to the [Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md).
Note that the request body parameter `"session_id"` and the URL parameter `<session_id>` must match.

| Response code | Response body |
|--|--|
| **200** Ok| `{"user": User Object, "session_id": <session_id>, "updated": new Session Object}`
| **404** Not Found| `{"error": "User <username> not found"}`
| | `{"error": "Session ID <session_id> for user <username> not found"}`
|**400** Bad Request| `{"message": {<some_objec_key>: "Wrong or missing entry"}`
| | `{"error": "Session ID argument "session_id" does not match with URL session ID <session_id>"}`

- This request updates the `Sesssion` object with id `<session_id>` belonging to user with username `<username>`.
-  If no user registered with `<username>` is found, a 404 error is thrown.
- Similarly, if the user exists but has no `Session` object under `<session_id>`, a 404 error is thrown.


---

### DELETE /api/users/\<username>/sessions/\<session_id>

**Required**: `None`

| Response code | Response body |
|--|--|
| **204** No Content| `None`
| **404** Not Found| `{"error": "User <username> not found"}`
| | `{"error": "Session ID <session_id> for user <username> not found"}`
- This request deletes the `Session` object registered with id `<session_id>` linked to the user with username `<username>` from the database.
- If no such user exists, a 404 error is thrown.
- Similarly, if the user exists but has no `Session` object under `<session_id>`, a 404 error is thrown.
- If the request body does not comply with the `Session` object as defined in [Data Specification](https://github.com/nebbles/MHML/blob/develop/docs/Data_Specification.md), a 400 error is thrown.
