from flask import request
from flask_restful import Resource


class HelloWorld(Resource):
    def get(self):
        return {'get': 'Hello World'}
    
    def post(self):
        data = request.get_json()
        return {'posted': data}, 201