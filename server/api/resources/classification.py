from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request
from api.responses import Response as res
import json
from ml.ml import ML
import numpy as np
import pandas as pd

class Classification(Resource):
    
    def get(self, username, session_id):
        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = user_ref.collection(u'sessions')
            session_ref = sessions_ref.document(session_id)
            session = session_ref.get()
            if session.exists:

                session_obj = session.to_dict()

                anxiety = session_obj['self_reported']['anxiety']
                fatigue = session_obj['self_reported']['fatigue']
                productivity = session_obj['self_reported']['productivity']
                stress = session_obj['self_reported']['stress']
                heart_rate = np.mean(list(session_obj['ppg']['heartRate'].values()))
                interbeat_iterval = np.mean(list(session_obj['ppg']['interbeatInterval'].values()))
                spO2 = np.mean(list(session_obj['ppg']['spO2'].values()))
                scl = np.mean(list(session_obj['gsr']['scl'].values()))

                df = pd.DataFrame(columns=['anxiety', 'fatigue', 'productivity', 'stress', 'heartRate', 'interbeatInterval', 'spO2', 'scl'])
                df.loc[session_id] = [anxiety, fatigue, productivity, stress, heart_rate, interbeat_iterval, spO2, scl]


                prediction = ML.predict(username , df)

                return {"user": username, "session_id": session_id, "prediction": prediction}, 200
            else:
                return res.NOT_FOUND(username, session_id)
        else:
            return res.NOT_FOUND(username)

