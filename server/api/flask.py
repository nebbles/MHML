import ml.model
from flask import Flask
from flask_restful import Api
from api.resources.stress import Stress
from api.resources.users import Users, User
from api.resources.sessions import Sessions, Session

class FlaskApp:
    
    def __init__(self):
        self.app = Flask(__name__)
        self.app.url_map.strict_slashes = False
        self.api = Api(self.app)
        self.api.add_resource(Stress, '/stress')
        self.api.add_resource(Users, '/api/users')
        self.api.add_resource(User, '/api/users/<string:username>')
        self.api.add_resource(Sessions, '/api/users/<string:username>/self_reports')
        self.api.add_resource(Session, '/api/users/<string:username>/self_reports/<string:session_id>')
        print("flask app ready!")


fa = FlaskApp()
app = fa.app
api = fa.api