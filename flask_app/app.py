from flask import Flask, request
from flask_restful import Api
from hello_world import HelloWorld
from multiply import Multiply

app = Flask(__name__)
api = Api(app)


api.add_resource(HelloWorld, '/')
api.add_resource(Multiply, '/multi/<int:num>')


if __name__ == '__main__':
    app.run(debug=True)