class Response:
    
    def __init__(self):
        pass
    
    def NOT_FOUND(user, session=None):
        if session is None:
            return {"error": "User '" + user + "' not found"}, 404
        else:
            return {"error": "Session ID '" + session + "' for user '" + user + "' not found"}, 404
    
    def OK(user, session=None, update=False):
        if session is None:
            if isinstance(user, list):
                return {"usernames": user}, 200
            elif isinstance(user, dict):
                return user, 200
        else:
            if not update:
                if isinstance(session, list):
                    return {"session_ids": session}, 200
                elif isinstance(session, dict):
                    return session, 200
            else:
                return {"user": user, "session_id": session, "updated": update}, 200
                

    def CREATED(user, session=None):
        if session is None:
            return {"new user": user}, 201
        else:
            return {"user": user, "new session_id": session}, 201

    def CONFLICT(user, session=None):
        if session is None:
            return {"error": "User '" + user + "' already exists"}, 409
        else:
            return {"error": "Session ID '" + session + "' for user '" + user + "' already exists"}, 409
    
    def NO_CONTENT(obj):
        return {"deleted": obj}, 204

    def BAD_REQUEST(arg, url):
        return {"error": "Session ID argument '" + arg + "' does not match with URL session ID '" + url + "'"}, 400

if __name__ == "__main__":

    response = Response()
    
    print(response.NOT_FOUND('caoanle13'))
    print(response.NOT_FOUND('caoanle13', '2'))

    print(response.OK([1, 2, 3]))
    print(response.OK({"Username": "caoanle13", "age": 21}))
    print(response.OK({"Username": "caoanle13", "age": 21}, ['1', '2']))
    print(response.OK({"Username": "caoanle13", "age": 21}, {"Anxiety": 3, "Stress": 2}))
    print(response.OK({"Username": "caoanle13", "age": 21}, ['1', '2'], update=True))
    print(response.OK({"Username": "caoanle13", "age": 21}, {"Anxiety": 3, "Stress": 2}, update=True))
    
    print(response.CREATED("caoanle13"))
    print(response.CREATED("caoanle13", "1"))
    
    print(response.CONFLICT("caoanle13"))
    print(response.CONFLICT("caoanle13", "1"))
    
    print(response.NO_CONTENT({"ok": "not ok", "number": 5}))
    


