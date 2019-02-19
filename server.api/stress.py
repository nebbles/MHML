from firestore import Firestore
from flask_restful import Resource
from flask import request


db = Firestore().db

class Stress(Resource):
    def get(self):
        stress_ref = db.collection(u'stress')
        docs = stress_ref.get()
        results = {}
        results_list = []
        for doc in docs:
            print(doc.id, doc.to_dict())
            results_list.append(doc.to_dict())
        results['get'] = results_list
        return results

    def post(self):
        data = request.get_json()
        db_ref = db.collection(u'stress')
        db_ref.add(data)
        return {'post': data}