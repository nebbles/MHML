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


class ML:
    "Class that holds all key function for the ML of the project"

    def __init__(self, UserID=0, SessionID=0, FullUserData, Regressor):
        self.UserID = UserID
        self.SessionID = SessionID
        self.FullUserData = FullUserData
        self.Regressor = Regressor

    def Predict(UserID, SessionData):
        # Loading most recent model for specific user
        try:
            Model = pickle.load(open("model_" + UserID + ".pkl", "r"))
        except:
            Model = pickle.load(open("model_primitive.pkl", "r"))

        # Predict stress level by averaging predicted stress level for all session data
        Predictions = []

        SessionData.drop('Stressed')  # ensure collumn name matches here

        for i in len(SessionData)
            Prediction = Model.predict(SessionData[:][i])
            Predictions.append(Prediction)

        AveragePrediction = mean(Predictions)

        return AveragePrediction

    def Train(UserID, UserData):
        # Load data
        Data = UserData
        X = Data.drop('stressed', axis=1, inplace=True)  # dropping 'stress' coeficient
        y = Data['Stressed']  # isolate the 'stressed' collumn

        # Feature scaling (if needed, and not done earlier by back-end)
        """from sklearn.preprocessing import StandardScaler
        sc_X = StandardScaler()
        X_train = sc_X.fit_transform(X_train)
        X_test = sc_X.transform(X_test)
        sc_y = StandardScaler()
        y_train = sc_y.fit_transform(y_train)"""

        # Fit model to up to date user data
        from sklearn.ensemble import RandomForestRegressor
        # Regressor = RandomForestRegressor(n_estimators = 10, random_state = 0)
        # Regressor.fit(X, y)

        SaveModel(Regressor, UserID)

    def SaveModel(model, u):

        import pickle
        # serializing our model to a file called model.pkl
        pickle.dump(model, open("model_" + u + ".pkl", "wb"))
