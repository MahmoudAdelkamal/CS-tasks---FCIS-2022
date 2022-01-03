import numpy as np
import matplotlib.pyplot as plt
from sklearn import svm, datasets
import time
import pandas as pd
from PreProcessing import *
from sklearn.model_selection import cross_val_score
from sklearn import linear_model
from sklearn import metrics
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
from sklearn.linear_model import LinearRegression

def Polynomial_SVM():

    X, Y = Preprocessing()

    X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.20, shuffle=True, random_state= 7)

    C =  0.05  # SVM regularization parameter
    degree = 4

    start = time.time()
    poly_SVM = svm.SVC(kernel='poly', degree=degree, C=C).fit(X_train, Y_train)
    TrainingTime = time.time() - start

    start = time.time()
    prediction = poly_SVM.predict(X_test)
    TestingTime = time.time() - start

    accuracy = np.mean(prediction == Y_test)*100
    print('Polynomial SVM')
    print('Accuracy: ', accuracy)
    print('Training Time: ', TrainingTime)
    print('Testing Time: ', TestingTime)
    return accuracy, TrainingTime, TestingTime
Polynomial_SVM()