import ml.model
from flask import Flask
from flask_restful import Api
from api.resources.stress import Stress

class FlaskApp:
    
    def __init__(self):
        self.app = Flask(__name__)
        self.api = Api(self.app)
        self.api.add_resource(Stress, '/stress')
        print("flask app ready!")


fa = FlaskApp()
app = fa.app
api = fa.api