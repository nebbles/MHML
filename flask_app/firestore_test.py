import firebase_admin
from firebase_admin import credentials
from firebase_admin import firestore
import os

# Use a service account
data = os.path.join(os.path.dirname(__file__), 'mhml_key.json')
cred = credentials.Certificate(data)
firebase_admin.initialize_app(cred)

db = firestore.client()

doc_ref = db.collection(u'members').document(u'c') # collection / document will be created if it doesn't exist
doc_ref.set({
    u'first': u'Cao An',
    u'last': u'Lê',
    u'role': u'Backend'
})

doc_ref = db.collection(u'members').document(u'l')
doc_ref.set({
    u'first': u'Leah',
    u'last': u'Pattison',
    u'role': u'App Design'
})

doc_ref = db.collection(u'members').document(u'b')
doc_ref.set({
    u'first': u'Ben',
    u'last': u'Greenberg',
    u'role': u'System Design'
})


doc_ref = db.collection(u'members').document(u'm')
doc_ref.set({
    u'first': u'Mohy',
    u'last': u'Aboualam',
    u'role': u'App Networks'
})

doc_ref = db.collection(u'members').document(u'f')
doc_ref.set({
    u'first': u'Felix',
    u'last': u'Crowther',
    u'role': u'Machine Learning'
})

doc_ref = db.collection(u'members').document(u's')
doc_ref.set({
    u'first': u'Scott',
    u'last': u'Bunting',
    u'role': u'Hardware'
})

doc_ref = db.collection(u'members').document(u'j')
doc_ref.set({
    u'first': u'Josephine',
    u'last': u'Latreille',
    u'role': u'Machine Learning'
})


members_ref = db.collection(u'members')
docs = members_ref.get()

for doc in docs:
    print(u'{} => {}'.format(doc.id, doc.to_dict()))
