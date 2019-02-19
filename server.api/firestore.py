import firebase_admin
from firebase_admin import credentials
from firebase_admin import firestore
import os

class Firestore:

    def __init__(self):

        data = os.path.join(os.path.dirname(__file__), 'mhml_key.json')
        cred = credentials.Certificate(data)
        firebase_admin.initialize_app(cred)

        self.db = firestore.client()