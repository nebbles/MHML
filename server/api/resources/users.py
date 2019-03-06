from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request
from api.responses import Response as res

users_ref = database.collection(u'users')

class Users(Resource):

    def __init__(self):
        self.arguments =[
            ('username', str, True),
            ('name', str, False),
            ('age', int, False),
            ('gender', bool, False),
            ('ethnicity', str, False),
            ('location', str, False),
            ('occupation', str, False)
        ] 
    
    def get(self):

        users = users_ref.get()
        usernames = []

        for user in users:
            username = user.to_dict()['username']
            usernames.append(username)

        return res.OK(usernames)
    

    def post(self):
        parser = reqparse.RequestParser(bundle_errors=True)
        for (n, t, b) in self.arguments:
            parser.add_argument(n, type=t, required=b, help="Wrong or missing entry")
        args = dict(parser.parse_args())

        username = args['username']
        user_ref = users_ref.document(username)
        user = user_ref.get()

        if not user.exists:
            data = {k: v for k, v in args.items() if v is not None}
            user_ref.set(data)
            return res.CREATED(data)

        else:
            return res.CONFLICT(username)

class User(Resource):

    def delete_collection(self, coll_ref, batch_size):
        docs = coll_ref.limit(10).get()
        deleted = 0

        for doc in docs:
            print(u'Deleting doc {} => {}'.format(doc.id, doc.to_dict()))
            doc.reference.delete()
            deleted = deleted + 1

        if deleted >= batch_size:
            return delete_collection(coll_ref, batch_size)

    def get(self, username):
        user_ref = users_ref.document(username)
        user = user_ref.get()
        if user.exists:
            return res.OK(user.to_dict())
        else:
            return res.NOT_FOUND(username)

    def delete(self, username):
        user_ref = users_ref.document(username)
        user = user_ref.get()
        if user.exists:
            user_sessions_ref = database.collection(u'users/'+username+'/sessions')
            user_ref.delete()
            self.delete_collection(user_sessions_ref, 10)
            return res.NO_CONTENT(username)
        else:
            return res.NOT_FOUND(username)




