import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

from Model1 import *
from Model2 import *
from Model3 import *
from Model4 import *
from Model5 import *


names = ['Linear SVM', 'Gaussian SVM', 'Polynomial SVM', 'Decision Tree', 'Adaboost']
models = [Linear_SVM(), Gaussian_SVM(), Polynomial_SVM(), Decision_Tree(), AdaBoost()]

def addlabels(x,y):
    for i in range(len(x)):
        plt.text(i,y[i],y[i])

def AccuracyGraph():
    values = [models[0][0], models[1][0], models[2][0], models[3][0], models[4][0]]

    plt.bar(names, values)
    addlabels(names,values)
    plt.title('Accuracy')
    plt.show()

def TrainingTimeGraph():
    values = [models[0][1], models[1][1], models[2][1], models[3][1], models[4][1]]
    plt.bar(names, values)
    addlabels(names, values)
    plt.title('Training Time')
    plt.show()

def TestingTimeGraph():
    values = [models[0][2], models[1][2], models[2][2], models[3][2], models[4][2]]
    plt.bar(names, values)
    addlabels(names, values)
    plt.title('Testing Time')
    plt.show()

AccuracyGraph()
TrainingTimeGraph()
TestingTimeGraph()