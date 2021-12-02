import pandas as pd
from Pre_Processing import *
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import PolynomialFeatures
from sklearn.model_selection import cross_val_score
from sklearn import linear_model
from sklearn import metrics

Features, Sales = Pre_processing()

# Split the data to training and testing sets
X_train, X_test, y_train, y_test = train_test_split(Features, Sales, test_size=0.30, shuffle=False)

model_1_poly_features = PolynomialFeatures(degree=2)
# transforms the existing features to higher degree features.
X_train_poly_model_1 = model_1_poly_features.fit_transform(X_train)
# fit the transformed features to Linear Regression
poly_model1 = linear_model.LinearRegression()
scores = cross_val_score(poly_model1, X_train_poly_model_1, y_train, scoring='neg_mean_squared_error', cv=5)
model_1_score = abs(scores.mean())
poly_model1.fit(X_train_poly_model_1, y_train)

# predicting on test data-set
prediction = poly_model1.predict(model_1_poly_features.fit_transform(X_test))
Model_1_Mean_Square_Error = metrics.mean_squared_error(y_test, prediction)
print('Model 2 Test Mean Square Error', metrics.mean_squared_error(y_test, prediction))
print('Accuracy : ', metrics.r2_score(y_test, prediction) * 100, "%")
