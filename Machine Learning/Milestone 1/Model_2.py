import pandas as pd
import time
from Pre_Processing import *
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import PolynomialFeatures
from sklearn.model_selection import cross_val_score
from sklearn import linear_model
from sklearn import metrics
import pickle


Features, Sales = Pre_processing('House_Data.csv')

# Split the data to training and testing sets
X_train, X_test, y_train, y_test = train_test_split(Features, Sales, test_size=0.30, shuffle=False)

Model2_poly_features = PolynomialFeatures(degree=2)
'''
X_train_poly_Model_2 = Model2_poly_features.fit_transform(X_train)
poly_Model2 = linear_model.LinearRegression()
scores = cross_val_score(poly_Model2, X_train_poly_Model_2, y_train, scoring='neg_mean_squared_error', cv=5)
model_1_score = abs(scores.mean())
#to calculate tarining time
start=time.time()
poly_Model2.fit(X_train_poly_Model_2, y_train)
stop=time.time()
# predicting on test data-set
prediction = poly_Model2.predict(Model2_poly_features.fit_transform(X_test))

# save the model to disk
filename = 'finalized_model2.sav'
pickle.dump(poly_Model2, open(filename, 'wb'))

'''

# load the model from disk
filename = 'finalized_model2.sav'
loaded_model = pickle.load(open(filename, 'rb'))
X_test, Y_test = Pre_processing('House_Data.csv')
result = loaded_model.score(Model2_poly_features.fit_transform(X_test), Y_test)
print('Accuracy : ', result*100)


'''
print('Model 2 Test Mean Square Error', metrics.mean_squared_error(y_test, prediction))
print('Accuracy : ', metrics.r2_score(y_test, prediction) * 100, "%")
print(f"Training time: {stop - start}s")
'''