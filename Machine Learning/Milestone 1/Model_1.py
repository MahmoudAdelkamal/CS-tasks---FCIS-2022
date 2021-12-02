import numpy as np
import pandas as pd
from Pre_Processing import *
from sklearn.model_selection import cross_val_score
from sklearn import linear_model
from sklearn import metrics
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
from sklearn.linear_model import LinearRegression

Features, Sales = Pre_processing()
X_train, X_test, y_train, y_test = train_test_split(Features, Sales, test_size=0.30, shuffle=False)

multiple_linear_regression = LinearRegression()

multiple_linear_regression.fit(X_train, y_train)

y_pred = multiple_linear_regression.predict(X_test)

err = metrics.mean_squared_error(y_test, y_pred)
Accuracy = metrics.r2_score(y_test, y_pred)
print('Model 1 Test Mean Square Error:', err)
print('Accuracy : ', Accuracy * 100, '%')
