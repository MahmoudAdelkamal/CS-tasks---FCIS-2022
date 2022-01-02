import numpy as np
import time
import pandas as pd
from Pre_Processing import *
from sklearn.model_selection import cross_val_score
from sklearn import linear_model
from sklearn import metrics
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
from sklearn.linear_model import LinearRegression
import pickle
'''
Features, Sales = Pre_processing('House_Data.csv')
X_train, X_test, y_train, y_test = train_test_split(Features, Sales, test_size=0.30, shuffle=True, random_state=15)

multiple_linear_regression = LinearRegression()
# to calculate training time
start=time.time()
multiple_linear_regression.fit(X_train, y_train)
stop=time.time()
y_pred = multiple_linear_regression.predict(X_test)

#save model
filename = 'finalized_model.sav'
pickle.dump(multiple_linear_regression, open(filename, 'wb'))

'''
# load the model from disk
filename = 'finalized_model.sav'
loaded_model = pickle.load(open(filename, 'rb'))
X_test, Y_test = Pre_processing('House_Data.csv')
result = loaded_model.score(X_test, Y_test)
print('Accuracy : ', result*100)
'''
err = metrics.mean_squared_error(y_test, y_pred)
Accuracy = metrics.r2_score(y_test, y_pred)
print('Model 1 Test Mean Square Error:', err)
print('Accuracy : ', Accuracy * 100, '%')
print(f"Training time: {stop - start}s")
'''