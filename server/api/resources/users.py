from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request

users_ref = database.collection(u'users')

class Users(Resource):
    
    def get(self):
        users = users_ref.get()
        usernames = []
        for user in users:
            username = user.to_dict()['Username']
            usernames.append(username)
        return {"usernames": usernames}, 200
    
    def post(self):
        parser = reqparse.RequestParser(bundle_errors=True)
        parser.add_argument('Username', required=True, help="Missing entry for Username field")
        parser.add_argument('Name', type=str, help="Invalid type for Name field")
        parser.add_argument('Age',  type=int, help="Invalid type for Age field")
        parser.add_argument('Gender', type=bool, help="Invalid type for Gender field")
        args = dict(parser.parse_args())

        username = args['Username']
        if not users_ref.document(username).get().exists:
            data = {k: v for k, v in args.items() if v is not None}
            users_ref.document(args['Username']).set(data)
            return {"new user": data}, 201
        else:
            return {"error": "CONFLICT, USER " + username + " ALREADY EXISTS"}, 409

class User(Resource):

    def get(self, username):
        user_ref = users_ref.document(username)
        user = user_ref.get()
        if user.exists:
            return user.to_dict(), 200
        else:
            return {"error": "NO USER FOUND FOR " + username}, 404

    def delete(self, username):
        user_ref = users_ref.document(username)
        user = user_ref.get()
        if user.exists:
            user_ref.delete()
            return "DELETED USER WITH USERNAME " + username, 204 
        else:
            return {"error":  "NO USER FOUND FOR " + username}, 404




