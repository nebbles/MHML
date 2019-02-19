from flask import request
from flask_restful import Resource


class Multiply(Resource):
    def get(self, num):
        return {'result': num*10}