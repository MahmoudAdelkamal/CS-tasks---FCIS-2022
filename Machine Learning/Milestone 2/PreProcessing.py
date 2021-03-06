from sklearn.preprocessing import LabelEncoder
import numpy as np
import pandas as pd


def Feature_Encoder(X, cols):
    for c in cols:
        lbl = LabelEncoder()
        lbl.fit(list(X[c].values))
        X[c] = lbl.transform(list(X[c].values))
    return X


def featureScaling(X, a, b):
    X = np.array(X)
    Normalized_X = np.zeros((X.shape[0], X.shape[1]))
    for i in range(X.shape[1]):
        Normalized_X[:, i] = ((X[:, i] - min(X[:, i])) / (max(X[:, i]) - min(X[:, i]))) * (b - a) + a
    return Normalized_X


def Preprocessing():
    data = pd.read_csv('House_Data_Classification.csv')
    Encoding_cols = (
        'Street', 'LotShape', 'Utilities', 'MSZoning', 'LotConfig', 'LandSlope', 'Neighborhood', 'BldgType',
        'HouseStyle',
        'RoofStyle', 'RoofMatl', 'Exterior1st', 'Exterior2nd', 'MasVnrType', 'ExterQual', 'ExterCond', 'Foundation',
        'BsmtQual',
        'BsmtCond', 'BsmtExposure', 'BsmtFinType1', 'BsmtFinType2', 'Heating', 'HeatingQC', 'CentralAir', 'Electrical',
        'KitchenQual', 'Functional', 'FireplaceQu', 'GarageType', 'GarageFinish', 'GarageQual', 'GarageCond',
        'PavedDrive',
        'PoolQC', 'Fence', 'MiscFeature', 'SaleType', 'SaleCondition', 'f1', 'f2', 'f3')

    Y = data.iloc[:, 77]

    for i in range(len(Y)):
        if Y[i] == 'cheap':
            Y.loc[i] = 0
        elif Y[i] == 'moderate':
            Y.loc[i] = 1
        else:
            Y.loc[i] = 2
    Y = Y.astype('int')

    new = data["MiscFeature2"].str.replace(',', '')
    new = new.str.replace('\'', '')
    new = new.str.replace(':', '')
    new = new.str.replace('{', '')
    new = new.str.replace('}', '')
    new = new.str.split(' ')
    data["f1"] = 0
    data["f2"] = 0
    data["f3"] = 0
    data.drop("MiscFeature2", 1, inplace=True)

    for i in range(data.shape[0]):
        for j in range(0, 5, 2):
            data[new[i][j]][i] = new[i][j + 1]

    Features = data.iloc[:, 1:80]  # Features

    Features = Feature_Encoder(Features, Encoding_cols)
    Features["PriceRate"] = Y
    for feature in Features:
        Features[feature].fillna(Features[feature].mean(), inplace=True)

    corr = Features.corr()
    Features = Features[corr.index[abs(corr["PriceRate"]) > 0.5]]

    Features.drop("PriceRate", 1, inplace=True)

    Features = featureScaling(Features, 0, 1)

    return Features, Y

