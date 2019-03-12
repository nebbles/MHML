import pandas as pd
import matplotlib.pyplot as plt

import numpy as np

from sklearn.model_selection import train_test_split
import statsmodels.formula.api as smf
import statsmodels.api as sm
import statsmodels.formula.api as smf
from sklearn.metrics import confusion_matrix
import seaborn as sns
import scipy
from sklearn import ensemble
from sklearn import tree
from sklearn.model_selection import cross_val_score
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import confusion_matrix, accuracy_score, precision_score, recall_score, f1_score
from sklearn import svm
from sklearn.model_selection import GridSearchCV
import sys
import pickle


class ML:
    "Class that holds all key function for the ML of the project"

    def predict(UserID, SessionData):
        # Loading most recent model for specific user
        try:
            pkl_path = "ml/models/" + UserID + ".pkl"
            with open(pkl_path, 'rb') as f:  
                Model = pickle.load(f)
        except:
            pkl_path = "ml/models/primitive.pkl"
             with open(pkl_path, 'rb') as f:
                 Model = pickle.load(f)
        # Predict stress level by averaging predicted stress level for all session data
        Predictions = []

        SessionData.drop('stress', axis=1, inplace=True)  # ensure collumn name matches here

        #for i in range(len(SessionData)):
            #Prediction = Model.predict(SessionData[:][i])
            #Predictions.append(Prediction)

        #AveragePrediction = mean(Predictions)

        prediction = Model.predict(SessionData)[0]

        return prediction


    def train(UserID, UserData):

        y = UserData['stress']  # isolate the 'stressed' column
        X = UserData.drop('stress', axis=1)  # dropping 'stress' coeficient


        # Feature scaling (if needed, and not done earlier by back-end)
        """
        from sklearn.preprocessing import StandardScaler
        sc_X = StandardScaler()
        X_train = sc_X.fit_transform(X_train)
        X_test = sc_X.transform(X_test)
        sc_y = StandardScaler()
        y_train = sc_y.fit_transform(y_train)
        """

        # Fit model to up to date user data
        from sklearn.ensemble import RandomForestRegressor
        Regressor = RandomForestRegressor(n_estimators = 10, random_state = 0)
        Regressor.fit(X, y)

        pkl_path = "ml/models/"+UserID+".pkl" 
        with open(pkl_path, 'wb') as f:  
            pickle.dump(Regressor, f)

