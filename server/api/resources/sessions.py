from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request

#sessions_ref = database.collection(u'reports')

class Sessions(Resource):
    
    def get(self, username):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = database.collection(u'users/'+username+'/self_reports')
            sessions = sessions_ref.get()

            session_ids = []
            for session in sessions:
                session_ids.append(session.id)

            return {"session_ids": session_ids}, 200
        else:
            return {"error": "User '" + username + "' not found"}, 404

  
    def post(self, username):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
    
            sessions_ref = user_ref.collection(u'self_reports')

            parser = reqparse.RequestParser(bundle_errors=True)
            parser.add_argument('session_id', type=str, required=True, help="Wrong or missing entry")
            parser.add_argument('Anxiety',type=float, required=True, help="Wrong or missing entry")
            parser.add_argument('Stress',  type=float, required=True, help="Wrong or missing entry")
            parser.add_argument('Fatigue', type=float, required=True, help="Wrong or missing entry")
            parser.add_argument('Productivity', type=float, required=True, help="Wrong or missing entry")
            args = dict(parser.parse_args())

            session_id = args['session_id']

            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if not session.exists:
                data = {k: v for k, v in args.items() if k is not 'session_id'}
                session_ref.set(data)
                return {"user": username, "new session_id": data}, 201
            else:
                return {"error": "Session ID '" + session_id + "' for user '" + username + "' already exists"}, 409
        else:
            return {"error": "User '" + username + "' not found"}, 404



class Session(Resource):

    def get(self, username, session_id):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = user_ref.collection(u'self_reports')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()
            if session.exists:
                return session.to_dict(), 200
            else:
                return {"error": "Session ID '" + session_id + "' for user '" + username + "' not found"}, 404
        else:
            return {"error": "User '" + username + "' not found"}, 404



    def post(self, username, session_id):
        
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:

            sessions_ref = user_ref.collection(u'self_reports')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if session.exists:

                parser = reqparse.RequestParser(bundle_errors=True)
                parser.add_argument('session_id', type=str, required=True, help="Wrong or missing entry")
                parser.add_argument('Anxiety',type=float, required=True, help="Wrong or missing entry")
                parser.add_argument('Stress',  type=float, required=True, help="Wrong or missing entry")
                parser.add_argument('Fatigue', type=float, required=True, help="Wrong or missing entry")
                parser.add_argument('Productivity', type=float, required=True, help="Wrong or missing entry")
                args = dict(parser.parse_args())

                if session_id == args['session_id']:

                    session_ref = sessions_ref.document(session_id)
                    data = {k: v for k, v in args.items() if k is not 'session_id'}
                    session_ref.set(data)
                    return {"user": username, "session_id": session_id, "updated": data}, 200
                
                else:
                    return {"error": "Session ID argument '" + session.id + "' does not match with URL session ID '" + args['session_id'] + "'"}, 400
                
            else:
                return {"error": "Session ID '" + session_id + "' for user '" + username + "' not found"}, 404
        else:
            return {"error": "User '" + username + "' not found"}, 404

        

