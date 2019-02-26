import ml.model
from flask import Flask
from flask_restful import Api
from api.resources.stress import Stress
from api.resources.users import Users, User

class FlaskApp:
    
    def __init__(self):
        self.app = Flask(__name__)
        self.api = Api(self.app)
        self.api.add_resource(Stress, '/stress')
        self.api.add_resource(Users, '/api/users')
        self.api.add_resource(User, '/api/users/<string:username>')
        print("flask app ready!")


fa = FlaskApp()
app = fa.app
api = fa.api