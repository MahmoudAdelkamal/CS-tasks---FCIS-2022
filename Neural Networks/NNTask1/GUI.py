from tkinter import *
from tkinter import ttk, StringVar
from Utils import *
from model import model
import numpy as np

class GUI:

    def __init__(self):

        self.root = Tk()
        self.root.geometry("800x500")
        self.root.title('Task1 - Perceptron Algorithm')
        self.root.option_add("*Font","Helvetica  13 bold ")
        self.padY = (10, 10)
        self.padX = 20
        self.features = ["X1 & X2", "X1 & X3", "X1 & X4", "X2 & X3", "X2 & X4", "X3 & X4"]
        self.classes = ["C1 & C2", "C2 & C3", "C1 & C3"]
        self.chClasses = StringVar(self.root)
        self.chClasses.set(self.classes[0])
        self.chFeatures = StringVar(self.root)
        self.chFeatures.set(self.features[0])
        self.learnRate = StringVar(self.root)
        self.epochsNo = StringVar(self.root)
        self.bias = IntVar(self.root)
        self.bias.set(0)
        self.checked = IntVar(self.root)
        self.X1_training = []
        self.X2_training = []
        self.X1_testing = []
        self.X2_testing = []
        self.training_classLabel = []
        self.testing_classLabel = []
        self.c1 = 0
        self.c2 = 0
        self.w1 = 0
        self.w2 = 0
        self.b = 0
        self.initComp()
        self.root.mainloop()
    def initComp(self):

        firstFeatureLbl = Label(self.root, text="Select Two Features:").grid(row=1, column=0, padx=self.padX, pady=self.padY, sticky=W)
        self.ffCBox = ttk.Combobox(self.root, values=self.features, textvariable=self.chFeatures).grid(row=1, column=1, padx=self.padX, pady=self.padY)
        firstClassLbl = Label(self.root, text="Select Two Classes:").grid(row=2, column=0, padx=self.padX, pady=self.padY, sticky=W)
        self.fcCBox = ttk.Combobox(self.root, textvariable=self.chClasses, values=self.classes).grid(row=2, column=1, padx=self.padX, pady=self.padY)
        lRateLbl = Label(self.root, text="Enter a learning rate:").grid(row=3, column=0, padx=self.padX, pady=self.padY, sticky=W)
        self.lRateEntry = Entry(self.root,textvariable=self.learnRate).grid(row=3, column=1, padx=self.padX , pady=self.padY)
        epochLbl = Label(self.root, text="Enter number of Epochs:").grid(row=4, column=0, padx=self.padX, pady=self.padY, sticky=W)
        self.epochEntry = Entry(self.root, textvariable=self.epochsNo).grid(row=4, column=1, padx=self.padX, pady=self.padY)
        biasCheckBox = Checkbutton(self.root, text="Bias ",variable=self.bias).grid(row=5, column=0,padx=self.padX ,pady=self.padY, sticky=E)
        calButton = Button(self.root, text="All Combinations", bg="light Gray", command=lambda: self.possibleCombinations()).grid(row=5, column=1,padx=self.padX ,pady=self.padY, sticky=E)
        lrnButton = Button(self.root, text="Perceptron Algorithm", bg="light Gray", command=lambda: self.train()).grid(row=6, column=1, padx=self.padX, pady=self.padY, sticky=E)
        discriminativeButton = Button(self.root, text="Discriminative Features", bg="light Gray", command=lambda: self.plotFeatures()).grid(row=7,column=1,padx=self.padX,pady=self.padY,sticky=E)
        tstButton = Button(self.root, text="Testing Phase", bg="light Gray", command=lambda: self.test()).grid(row=8, column=1, padx=self.padX, pady=self.padY, sticky=E)

    def possibleCombinations(self):
        read_data = Utils()
        read_data.draw_iris_dataset()

    def plotFeatures(self):
        featureX, featureY,plt = self.initilizeData()
        if self.w1 !=0 and self.w2 != 0:
            class_ = [int(self.chClasses.get()[1]), int(self.chClasses.get()[6])]
            feature_ = [int(self.chFeatures.get()[1]), int(self.chFeatures.get()[6])]
            plt.discriminative_line(featureX, featureY, class_, feature_, self.w1, self.w2, self.b)

    def initilizeData(self):
        chosenFeatures = self.chFeatures.get()
        feature1 = int(chosenFeatures[1])
        feature2 = int(chosenFeatures[6])
        rd = Utils()
        rd.readData()
        featureX = self.returnFeature(feature1,rd)
        featureY = self.returnFeature(feature2,rd)
        return (featureX, featureY,rd)

    def returnFeature(self,index,rd):
        feature = []
        if index == 1:
            feature = rd.IrisX1
        elif index == 2:
            feature = rd.IrisX2
        elif index == 3:
            feature = rd.IrisX3
        else:
            feature = rd.IrisX4
        return feature

    def manageTrainingFeatures(self):
        # initilize X1 and X2
        featureX, featureY,rd_ = self.initilizeData()
        choosenClasses = self.chClasses.get()
        class1 = int(choosenClasses[1])
        class2 = int(choosenClasses[6])
        self.training_classLabel = []
        self.X1_training = []
        self.X2_training = []

        if class1 == 1:
            self.X1_training = featureX[0:30]
            self.X2_training = featureY[0:30]
            self.training_classLabel = [1 for i in range(0, 30)]
            if class2 == 2:
                self.X1_training.extend(featureX[50:80])
                self.X2_training.extend(featureY[50:80])
                self.training_classLabel.extend([2 for i in range(0, 30)])
            else:
                self.X1_training.extend(featureX[100:130])
                self.X2_training.extend(featureY[100:130])
                self.training_classLabel.extend([3 for i in range(0, 30)])
        elif class1 == 2:
            self.X1_training = featureX[50:80]
            self.X2_training = featureY[50:80]
            self.training_classLabel = [2 for i in range(0, 30)]
            self.X1_training.extend(featureX[100:130])
            self.X2_training.extend(featureY[100:130])
            self.training_classLabel.extend([3 for i in range(0, 30)])
    def manageTestingFeatures(self):
        featureX, featureY,rd_ = self.initilizeData()
        choosenClasses = self.chClasses.get()
        class1 = int(choosenClasses[1])
        class2 = int(choosenClasses[6])
        self.c1 = class1
        self.c2 = class2
        ####
        if class1 == 1:
            self.X1_testing = featureX[30:50]
            self.X2_testing = featureY[30:50]
            self.testing_classLabel = [1 for i in range(0, 20)]
            if class2 == 2:
                self.X1_testing.extend(featureX[80:100])
                self.X2_testing.extend(featureY[80:100])
                self.testing_classLabel.extend([2 for i in range(0, 20)])
            else:
                self.X1_testing.extend(featureX[130:150])
                self.X2_testing.extend(featureY[130:150])
                self.testing_classLabel.extend([3 for i in range(0, 20)])
        elif class1 == 2:
            self.X1_testing = featureX[80:100]
            self.X2_testing = featureY[80:100]
            self.testing_classLabel = [2 for i in range(0, 20)]
            self.X1_testing.extend(featureX[130:150])
            self.X2_testing.extend(featureY[130:150])
            self.testing_classLabel.extend([3 for i in range(0, 20)])

    def train(self):
        perceptron = model()
        bias = self.bias.get()
        learning_rate = float(self.learnRate.get())
        num_of_epochs = int(self.epochsNo.get())
        self.manageTrainingFeatures()
        w1,w2,b = perceptron.train(self.X1_training,self.X2_training,self.training_classLabel,learning_rate,num_of_epochs,bias)
        self.w1 = w1
        self.w2 = w2
        self.b = b
    def test(self):
        perceptron = model()
        self.manageTestingFeatures()
        w = [self.w1,self.w2]
        Accuracy = 0
        conf = np.zeros([2,2], dtype='int32')
        print('-------------------------------------------------------------------------')
        for i in range(0,len(self.X1_testing)):
            y = perceptron.perceptron_test(self.X1_testing[i],self.X2_testing[i],w,self.b)
            if (y == 1 and self.c1 == self.testing_classLabel[i]):
                conf[0, 0] = conf[0, 0] + 1
                Accuracy += 1
            elif (y == -1 and self.testing_classLabel[i] == self.c2):
                conf[1,1] = conf[1,1] + 1
                Accuracy += 1
            elif (y == 1 and self.testing_classLabel[i] != self.c1):
                conf[0, 1] = conf[0, 1] + 1
            elif (y == -1 and self.testing_classLabel[i] != self.c2):
                conf[1, 0] = conf[1, 0] + 1
            print('X1: ' + str(self.X1_testing[i]), 'X2: ' + str(self.X2_testing[i]))
            print('Y: ' + str(y))
        #print('classes: ' + str(self.testing_classLabel))
        print('confusion matrix: ' + str(conf))
        print('Accuracy: ' + str((Accuracy/40)*100) +'%')