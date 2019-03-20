from api.firestore import database
from flask_restful import Resource, reqparse
from flask import request
from api.responses import Response as res
import json
import pandas as pd
from ml.ml import ML

class Training(Resource):

    def post(self, username):

        user_ref = database.document(u'users/'+username)
        user = user_ref.get()

        if user.exists:
            sessions_ref = database.collection(u'users/'+username+'/sessions')
            sessions = sessions_ref.get()

            session_objs = []

            for session in sessions:
                session_obj = session.to_dict()
                session_objs.append(session_obj)

            sessions_number = len(session_obj["ppg"]["heartRate"])*len(session_objs)
            df = pd.DataFrame(columns=['anxiety','fatigue','productivity','stress', 'heartRate', 'interbeatInterval', 'spO2', 'scl'])

            for s in session_objs:
                df_temp = pd.DataFrame(columns=['anxiety','fatigue','productivity','stress', 'heartRate', 'interbeatInterval', 'spO2', 'scl'])
                for i in list(s["ppg"]["heartRate"].keys()):
                    df_temp.loc[i] = pd.Series(
                        {'anxiety': s['self_reported']['anxiety'],
                        'fatigue':s['self_reported']['fatigue'],
                        'productivity':s['self_reported']['productivity'],
                        'stress':s['self_reported']['stress'],
                        'heartRate': s['ppg']['heartRate'][i],
                        'interbeatInterval':s['ppg']['interbeatInterval'][i],
                        'spO2':s['ppg']['spO2'][i],
                        'scl': s['gsr']['scl'][i]
                        }
                    )
                df = df.append(df_temp)
            print(df)
            ML.train(username, df)

            
            return None, 200
        else:
            return res.NOT_FOUND(username)
