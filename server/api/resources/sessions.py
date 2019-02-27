from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request
from api.responses import Response as res

class Sessions(Resource):
    def __init__(self):
        self.arguments = [('session_id', str), ('Anxiety', float), ('Stress', float), ('Fatigue', float), ('Productivity', float)]
    
    def get(self, username):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = database.collection(u'users/'+username+'/self_reports')
            sessions = sessions_ref.get()

            session_ids = []
            for session in sessions:
                session_ids.append(session.id)
            return res.OK(username, session_ids)
        else:
            return res.NOT_FOUND(username)

  
    def post(self, username):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
    
            sessions_ref = user_ref.collection(u'self_reports')

            parser = reqparse.RequestParser(bundle_errors=True)
            for (n, t) in self.arguments:
                parser.add_argument(n, type=t, required=True, help= "Wrong or missing entry")
            
            args = dict(parser.parse_args())

            session_id = args['session_id']

            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if not session.exists:
                data = {k: v for k, v in args.items() if k is not 'session_id'}
                session_ref.set(data)
                return res.CREATED(username, data)
            else:
                return res.CONFLICT(username, session_id)
        else:
            return res.NOT_FOUND(username)



class Session(Sessions):

    def get(self, username, session_id):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = user_ref.collection(u'self_reports')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()
            if session.exists:
                return res.OK(username, session.to_dict())
            else:
                return res.NOT_FOUND(username, session_id)
        else:
            return res.NOT_FOUND(username)



    def post(self, username, session_id):
        
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:

            sessions_ref = user_ref.collection(u'self_reports')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if session.exists:

                parser = reqparse.RequestParser(bundle_errors=True)
                for (n, t) in self.arguments:
                    parser.add_argument(n, type=t, required=True, help= "Wrong or missing entry")
                args = dict(parser.parse_args())

                if session_id == args['session_id']:

                    session_ref = sessions_ref.document(session_id)
                    data = {k: v for k, v in args.items() if k is not 'session_id'}
                    session_ref.set(data)
                    return res.OK(username, session_id, update=data)
                
                else:
                    return res.BAD_REQUEST(args['session_id'], session_id)
                
            else:
                return res.NOT_FOUND(username, session_id)
        else:
            return res.NOT_FOUND(username)

        

