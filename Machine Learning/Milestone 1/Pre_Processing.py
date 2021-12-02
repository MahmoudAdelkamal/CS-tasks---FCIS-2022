from sklearn.preprocessing import LabelEncoder
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns


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


def Pre_processing():
    data = pd.read_csv('House_Data.csv')
    Encoding_cols = (
        'Street', 'LotShape', 'Utilities', 'MSZoning', 'LotConfig', 'LandSlope', 'Neighborhood', 'BldgType',
        'HouseStyle',
        'RoofStyle', 'RoofMatl', 'Exterior1st', 'Exterior2nd', 'MasVnrType', 'ExterQual', 'ExterCond', 'Foundation',
        'BsmtQual',
        'BsmtCond', 'BsmtExposure', 'BsmtFinType1', 'BsmtFinType2', 'Heating', 'HeatingQC', 'CentralAir', 'Electrical',
        'KitchenQual', 'Functional', 'FireplaceQu', 'GarageType', 'GarageFinish', 'GarageQual', 'GarageCond',
        'PavedDrive',
        'PoolQC', 'Fence', 'MiscFeature', 'SaleType', 'SaleCondition')

    Features = data.iloc[:, 1:76]  # Features

    Sale = data['SalePrice']  # Label

    Features = Feature_Encoder(Features, Encoding_cols)

    for feature in Features:
        Features[feature].fillna(Features[feature].mean(), inplace=True)

    corr = data.corr()
    Top_Features = corr.index[abs(corr['SalePrice'] > 0.5)]

    # plt.subplots(figsize=(12, 8))
    # top_corr = data[Top_Features].corr()
    # sns.heatmap(top_corr, annot=True)
    # plt.show()

    Top_Features = Top_Features.delete(-1)
    Features = Features[Top_Features]

    Features = featureScaling(Features, 0, 1)
    Sale = np.expand_dims(Sale, axis=1)
    Sale = featureScaling(Sale, 0, 1)
    return Features, Sale
