from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request
from api.responses import Response as res

class Sessions(Resource):
    def __init__(self):
        self.arguments = [
            ('session_id', str, True),
            ('self_reported', dict, True),
            ('PPG', dict, True),
            ('GSR', dict, True),
        ]
        
        self.self_reported_arguments = [
            ('Anxiety', float, True, 'self_reported'),
            ('Stress', float, True, 'self_reported'),
            ('Fatigue', float, True, 'self_reported'),
            ('Productivity', float, True, 'self_reported'),
        ]

        self.PPG_arguments = [
            ('FirmwareRevision', str, True, 'PPG'),
            ('BodySensorLocation', str, True, 'PPG')
            #('HeartRate', ..., True),
            #('SPO2', ..., True)
        ]

        self.GSR_arguments = [
            ('FirmwareRevision', str, True, 'GSR'), 
            ('BodySensorLocation', str, True, 'GSR')
            #...,
            #...
        ]
    
    def get(self, username):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = database.collection(u'users/'+username+'/sessions')
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
    
            sessions_ref = user_ref.collection(u'sessions')

            outer_parser = reqparse.RequestParser(bundle_errors=True)
            for (n, t, b) in self.arguments:
                outer_parser.add_argument(n, type=t, required=b, help= "Wrong or missing entry")
            parsed_session = outer_parser.parse_args()
            
            for args in self.self_reported_arguments, self.PPG_arguments, self.GSR_arguments:
                inner_parser = reqparse.RequestParser(bundle_errors=True)
                for (n, t, b, l) in args:
                    inner_parser.add_argument(n, type=t, required=b, location=l, help= "Wrong or missing entry")
                nested_args = inner_parser.parse_args(req=parsed_session)
            
            session_obj = dict(parsed_session)

            session_id = session_obj['session_id']

            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if not session.exists:
                session_obj.pop('session_id')
                session_obj.pop('unparsed_arguments', None)
                session_ref.set(session_obj)
                return res.CREATED(username, session_obj)
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

            sessions_ref = user_ref.collection(u'sessions')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()

            if session.exists:

                outer_parser = reqparse.RequestParser(bundle_errors=True)
                for (n, t, b) in self.arguments:
                    outer_parser.add_argument(n, type=t, required=b, help= "Wrong or missing entry")
                parsed_session = outer_parser.parse_args()
                
                for args in self.self_reported_arguments, self.PPG_arguments, self.GSR_arguments:
                    inner_parser = reqparse.RequestParser(bundle_errors=True)
                    for (n, t, b, l) in args:
                        inner_parser.add_argument(n, type=t, required=b, location=l, help= "Wrong or missing entry")
                    nested_args = inner_parser.parse_args(req=parsed_session)
                
                session_obj = dict(outer_parser.parse_args())

                if session_id == session_obj['session_id']:

                    session_ref = sessions_ref.document(session_id)
                    session_obj.pop('session_id')
                    session_obj.pop('unparsed_arguments', None)
                    session_ref.set(session_obj)

                    return res.OK(username, session_id, update=session_obj)
                
                else:
                    return res.BAD_REQUEST(session_obj['session_id'], session_id)
                
            else:
                return res.NOT_FOUND(username, session_id)
        else:
            return res.NOT_FOUND(username)
