from flask import Flask
from flask_restful import Api
from hello_world import HelloWorld
from multiply import Multiply
from stress import Stress

app = Flask(__name__)
api = Api(app)


api.add_resource(HelloWorld, '/')
api.add_resource(Multiply, '/multi/<int:num>')
api.add_resource(Stress, '/stress')


if __name__ == '__main__':
    app.run(host='localhost', port=5000)