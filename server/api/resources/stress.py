from api.firestore import database
from flask_restful import Resource
from flask import request

ref = database.collection(u'stress')

class Stress(Resource):

    def get(self):
        docs = ref.get()
        results = {}
        results_list = []
        for doc in docs:
            results_list.append(doc.to_dict())
        results['get'] = results_list
        print('deleted')
        return results

    def post(self):
        data = request.get_json()
        ref.add(data)
        return {'post': data}