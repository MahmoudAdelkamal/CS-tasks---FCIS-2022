import matplotlib.pyplot as plt
import numpy as np

class Utils:

    def __init__(self):
        self.IrisX1 = []
        self.IrisX2 = []
        self.IrisX3 = []
        self.IrisX4 = []
        self.IrisY = []

    def readData(self):
        with open('IrisData.txt') as irisTxt:
            irisTxt.readline()
            for x in irisTxt:
                X = x.split(',')
                self.IrisX1.append(float(X[0]))
                self.IrisX2.append(float(X[1]))
                self.IrisX3.append(float(X[2]))
                self.IrisX4.append(float(X[3]))
                self.IrisY.append(X[4])
        return self.IrisX1, self.IrisX2, self.IrisX3, self.IrisX4, self.IrisY

    def Draw_feature(self, x1, x2, f1,f2):
        plt.figure("Figure of Features")
        plt.scatter(x1[:50], x2[:50])
        plt.scatter(x1[50:100], x2[50:100])
        plt.scatter(x1[100:], x2[100:])
        plt.xlabel("ْْX"+f1)
        plt.ylabel("X"+f2)
        plt.show()

    def draw_iris_dataset(self):
        data = []
        data = self.readData()
        self.draw_6figure(data)

    def draw_6figure(self, data):
        for i in range(1, 4):
            self.Draw_feature(data[0], data[i],str(1), str(i+1))
        self.Draw_feature(data[1], data[2], str(2), str(3))
        self.Draw_feature(data[1], data[3], str(2), str(4))
        self.Draw_feature(data[2], data[3], str(3), str(4))

    def class_label(self, class_):
        if class_ == 1:
            return 'Iris Setosa'
        if class_ == 2:
            return 'Iris Versicolour'
        return 'Iris-virginica'

    def discriminative_line(self,X1, X2,class_, feature_, w1=0, w2=0, b=0):
        type_indx = [(class_[0]-1)*50, class_[0]*50, (class_[1]-1)*50, class_[1]*50]
        lineX = [int(np.min(X2) - 2), int(np.max(X1) + 1)]
        lineY = [-(b + w1 * xi) / w2 for xi in lineX]

        fig = plt.figure('Discrimination Line')
        plt.scatter(X1[type_indx[0]:type_indx[1]], X2[type_indx[0]:type_indx[1]], label=self.class_label(class_[0]))
        plt.scatter(X1[type_indx[2]:type_indx[3]], X2[type_indx[2]:type_indx[3]], label=self.class_label(class_[1]))

        plt.plot(lineX, lineY)

        plt.title('Plotting of Features')
        plt.xlabel('Feature' + str(feature_[0]))
        plt.ylabel('Feature' + str(feature_[1]))
        plt.show()