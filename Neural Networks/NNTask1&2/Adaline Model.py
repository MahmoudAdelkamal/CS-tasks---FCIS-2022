import numpy as np

class Adaline_Model:

    def __init__(self):
        pass
    def train(self,feature1,feature2,class_label,learning_rate,threshold,bias):
        weights = np.random.rand(3)
        epoch = 0
        MSE = 1
        while MSE>threshold and epoch<10000:
            for i in range(len(feature1)):
                Xi = [feature1[i],feature2[i]]
                Ti = class_label[i]
                Yi = np.dot(weights.transpose(),Xi)
                err = Ti - Yi
                weights[0] = weights[0] + learning_rate * err
                weights[1] = weights[1] + learning_rate * err * Xi[0]
                weights[2] = weights[2] + learning_rate * err * Xi[1]
            for i in range(len(feature1)):
                Xi = [feature1[i],feature2[i]]
                Ti = class_label[i]
                Yi = np.dot(weights.transpose(),Xi)
                err = Ti - Yi
                MSE+=np.power(err,2)
            MSE/=2*len(feature1)
            epoch+=1
        return (weights[1],weights[2],weights[0]*bias)
        # لييه؟؟ Bias فكرني أسألك أنت هنا بتضرب في ال

    def predict(self,res):
        if res > 0.0 :
            return 1
        else :
            return -1

    def net_inputTest(self, X, weight, bias):
        return (bias + weight[0] * X[0] + weight[1] * X[1])

    def Adaline_test(self, featureX, featureY, w, bias):
        x = [featureX, featureY]
        v = self.net_inputTest(x, w, bias)
        return self.predict(v)