# Flask app development for server-side API

**Base URL**: _mhml.greenberg.io_
## Users Resource 
- **route**: _/api/users_

### **GET**: to retrieve a list of all usernames in the database
response: **200 OK**
```json
{
    "usernames": [
        ...,
        all usernames in db,
        ...,
    ]
}
```
**Note**: the returned list could be empty.

### **POST**: to add a new User Object to the database
request: JSON object with fields
- **Username** (str, **required**!!)
- **Name** (str, Optional)
- **Age** (int, Optional)
- **Gender**(bool, Optional)
- **Ethnicity** (str, Optional)
- **Location** (str, Optional)
- **Occupation** (str, Optional)

example:
```json
User_Object = 
{
    "Username": "caoanle13",
    "Name": "Cao An",
    "Age": 21,
    "Gender": 0,
    "Ethnicity": "Asian",
    "Location": "London", 
    "Occupation": "Student"
}
```
**Note**:
- For Gender: male -> 0, female -> 1
- Optional fields don't need to be included
- Types must be respected
- Keys are case sensitive

response: **201 CREATED**
```json
{
    "new user": User_Object
}
```

---


## User Resource 
- **route**: _/api/users/\<username>_

### **GET**: to retrieve a particular User Object from the database
response: **200 OK**
- User Object as JSON

example response for **GET** _/api/users/caoanle13_
```json
{
    "Username": "caoanle13",
    "Name": "Cao An",
    "Age": 21,
    "Gender": 0,
    "Ethnicity": "Asian",
    "Location": "London", 
    "Occupation": "Student"
}
```


### **DELETE**: to delete a particular User Object
response: **204 NO CONTENT**


---


## Sessions Resource 
- **route**: _/api/users/\<username>/self_reports_

### **GET**: to retrieve a list of all session IDs from a particular user in the database
response: **200 OK**
- List of session IDs under the key "session_ids"
```json
{
    "session_ids": [
        ...,
        all session_ids in from <username> in database,
        ...,
    ]
}
```
**Note**:
- the returned list could be empty
- A session ID field has type str (for now)

### **POST**: to add a new Session Object to a particular user in the database
request: JSON object with fields
- **session_id** (str, required)
- **Anxiety** (float, required)
- **Fatigue** (float, required)
- **Productivity**(float, required)
- **Stress** (float, required)

example of object: 
```json
{
    "session_id": "1",
    "Anxiety": 7.5,
    "Fatigue": 2,
    "Productivity": 0,
    "Stress": 10.0,
}
```
**Note**:
- The "session_id" entry does not form part of a Session Object (it is added to the database as the ID of the object)
- Types must be respected


response: **201 CREATED**
example response on **POST** with the above arguments at _/api/users/caoanle13/self_reports_
```json
{
    "user": "caoanle13",
    "new session": {
        "Anxiety": 7.5,
        "Fatigue": 2,
        "Productivity": 0,
        "Stress": 10.0,
    }
}
```


## Session Resource 
- **route**: _/api/users/\<username>/self_reports_/\<session_id>

### **GET**: to retrieve a particular session of a particular user in the database
response: **200 OK**
- Session Object as JSON
- Example response on **GET** _/api/users/caoanle13/self_reports_/1
```json
{
    "Anxiety": 7.5,
    "Fatigue": 2,
    "Productivity": 0,
    "Stress": 10.0,
}
```

### **POST**: to overwrite an existing Session Object of a particular user in the database
request: JSON object with fields
- **session_id** (str, required)
- **Anxiety** (float, required)
- **Fatigue** (float, required)
- **Productivity**(float, required)
- **Stress** (float, required)

example:
```json
{
    "session_id": "1",
    "Anxiety": 10.3,
    "Fatigue": 0,
    "Productivity": 5,
    "Stress": 10.0,
}
```
**Note**:
- The "session_id" argument and the \<session_id> from the URL must match


response: **200 OK**
- JSON object with keys: "user", "session_id", "updated".
- "updated" contains the new overwritten Session Object
example response on **POST** with the above arguments at _/api/users/caoanle13/self_reports_/1
```json
{
    "user": "caoanle13",
    "session_id": "1",
    "updated": {
        "Anxiety": 10.3,
        "Fatigue": 0,
        "Productivity": 5,
        "Stress": 10.0,
    }
}
```

