import time
from PreProcessing import *
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.ensemble import AdaBoostClassifier
from sklearn.tree import DecisionTreeClassifier

def AdaBoost():
    X, Y = Preprocessing()

    X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.20, shuffle=True, random_state= 7)

    bdt = AdaBoostClassifier(DecisionTreeClassifier(max_depth=4),
                             algorithm="SAMME",
                             n_estimators=155)
    scaler = StandardScaler()
    scaler.fit(X_train)

    X_train = scaler.transform(X_train)
    X_test = scaler.transform(X_test)



    start = time.time()
    bdt.fit(X_train,Y_train)
    TrainingTime = time.time() - start

    start = time.time()
    y_prediction = bdt.predict(X_test)
    TestingTime = time.time() - start


    accuracy=np.mean(y_prediction == Y_test)*100
    print('Adaboost')
    print('Accuracy: ', accuracy)
    print('Training Time: ', TrainingTime)
    print('Testing Time: ', TestingTime)
    return accuracy, TrainingTime, TestingTime
AdaBoost()