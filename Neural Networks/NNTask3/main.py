import numpy as np


class model:

    def __init__(self):
        pass

    def initialize_parameters(self, layer_dims):
        np.random.seed(3)
        parameters = {}
        layer_dims.insert(0, 4)  # Input layer in the network
        layer_dims.insert(len(layer_dims), 3)  # output layer in the network

        L = len(layer_dims)  # number of layers in the network

        for l in range(1, L):
            parameters['W' + str(l)] = np.random.randn(layer_dims[l], layer_dims[l - 1]) * 0.01
            parameters['b' + str(l)] = np.zeros((layer_dims[l], 1))

        return parameters

    def sigmoid(self, Z):
        A = 1 / (1 + np.exp(-Z))
        return A

    def hyperbolic_tangent_sigmoid(self, Z):
        A = (1 - np.exp(-Z)) / (1 + np.exp(-Z))
        return A

    def forward(self, A, W, b):

        Z = np.dot(W, A) + b
        # Z is the input of the activation function
        return Z

    def activation_forward(self, A_prev, W, b, activation):

        if activation == "sigmoid":
            Z = self.forward(A_prev, W, b)
            A = self.sigmoid(Z)

        elif activation == "Hyper":
            Z = self.forward(A_prev, W, b)
            A = self.Hyperbolic_Tangent_sigmoid(Z)

        cache = (A, W)  # cache is a tuple, stored for computing the backward pass

        return A, cache

    def model_forward(self, X, parameters, activation):
        caches = []
        A = X
        L = len(parameters) // 2
        for l in range(1, L + 1):
            A_prev = A
            A, cache = self.activation_forward(A_prev,
                                               parameters['W' + str[l]],
                                               parameters['b' + str[l]],
                                               activation)
            caches.append(cache)

        return A, caches

    def activation_backward(self, grad_next, cache):
        grad = grad_next * cache[1] * cache[0] * (1 - cache[0])
        return

    def model_backward(self, A, Y, caches):
        grads = {}
        L = len(caches)  # number of layers
        current_cache = caches[L - 1]
        grads['gradiant' + str(L - 1)] = (Y - A) * (A) * (1 - A)
        # Loop from l=L-2 to l=0
        for l in reversed(range(L - 1)):
            current_cache = caches[l]
            grads['gradiant' + str(l)] = self.activation_backward(grads['gradiant' + str(l + 1)], current_cache)

        return grads

    def update_parameters(self, parameters, grads, learning_rate, X, caches):
        # parameters = params.copy()
        L = len(parameters) // 2  # number of layers in the neural network
        parameters['W1'] = parameters['W1'] + learning_rate * grads['gradiant1'] * X

        for l in range(1, L):
            current_cache = caches[l-1]
            parameters['W' + str(l+1)] = parameters['W' + str(l+1)] + learning_rate * grads['gradiant' + str(l+1)] * \
                                       current_cache[0]
        return parameters

    def model(self,X,Y,number_of_layers,layers_dims,learning_rate,epochs):
        np.random.seed(1)
        costs=[]
        parameters=self.initialize_parameters(layers_dims)
        for i in range(epochs):
            #forward
            AL,caches=self.model_forward(X,parameters)

            #backward
            grads=self.model_backward(AL,Y,caches)

            parameters = self.update_parameters(parameters,grads,learning_rate,X,caches)

        return parameters