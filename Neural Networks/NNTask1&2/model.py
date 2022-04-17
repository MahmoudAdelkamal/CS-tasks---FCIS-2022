import numpy as np

class model :

    def __init__(self):
        pass

    def net_value(self,X,weight,bias) :
        return (bias*weight[0] + X[0]*weight[1] + X[1]*weight[2])

    def predict(self,res):
        if res > 0.0 :
            return 1
        else :
            return -1
    def train(self,feature1,feature2,class_label,learning_rate,no_of_epochs,bias):
        error = 1
        epoch = 0
        weight = [np.random.rand(1)[0],np.random.rand(1)[0],np.random.rand(1)[0]]
        while epoch < no_of_epochs :
            for i in range(0,len(feature1)):
                X = [feature1[i],feature2[i]]
                t = class_label[i]
                v = self.net_value(X,weight,bias)
                y = self.predict(v)
                if t==class_label[0] :
                    t = 1
                else :
                    t = -1
                error = t - y
                weight[0] = weight[0] + learning_rate*error
                weight[1] = weight[1] + learning_rate*error*X[0]
                weight[2] = weight[2] + learning_rate*error*X[1]
            epoch+=1
            w1 = weight[1]
            w2 = weight[2]
            b = weight[0] * bias
        return (w1,w2,b)

    def net_inputTest(self, X, weight, bias):
        return (bias + weight[0] * X[0] + weight[1] * X[1])

    def perceptron_test(self, featureX, featureY, w, bias):
        x = [featureX, featureY]
        v = self.net_inputTest(x, w, bias)
        return self.predict(v)

