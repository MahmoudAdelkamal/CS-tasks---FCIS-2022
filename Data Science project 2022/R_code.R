setwd("~/Data Science project 2022")


# Step 1

data <- read.csv("zeta.csv")
#Analyze summary of the data
print(summary(data))
# column names of the zeta table
print(colnames(data))
# the number of rows in the zeta table
print(nrow(data))
# checking if there are multiple rows in the zeta table
unique_data <- unique(data)
print(unique_data)
print(nrow(unique_data))

# Step 2

#load income data 
income_data <- read.delim2("zipincome.txt")
#change columns names
names(data)[1] <- "zipCode"
names(data)[6] <- "income"
print(colnames(data))
#Analyze summary of the data
print(summary(data))

## Question d: scatter plot
data_dataFrame <- data[, c("income", "zipCode")]
#this line is because big number shows like 0e+4
options(scipen=10000000)
plot(data_dataFrame$income,data_dataFrame$zipCode, main = "zipCode vs Income",
     xlab = "Income", ylab = "zipCode"
     )

#creating subset of data 
dataSubset <- subset(data,income<200000 & income > 7000)
print(summary(dataSubset))

# Step 3
#create simple box plot
boxplot(income ~ zipCode,data=dataSubset,main="Average Household Income by ZipCode",xlab="ZipCodes",ylab="Income")
#create new box plot that it's y-axis uses a log scale
boxplot(income ~ zipCode,data=dataSubset,main="Average Household Income by ZipCode",xlab="ZipCodes",ylab="Income",log='y')

#============ Advanced Analytics/Methods (K-means)


# 1) Read csv file 

library(readr)
income_elec_stat <-read_csv('income_elec_state.csv')
colnames(income_elec_stat) <- c('state', 'mean household income', 'mean electricity usage')


# 2) Cluster data using k-means with K = 10

income_elec <- income_elec_stat[,-1] # dropping state column


k <- kmeans(income_elec, 10)

k

plot(income_elec, col = k$cluster) 
points(k$centers,col=1:10, pch = 4)


# 3) Determining a reasonable value of K using the elbow of the plot

wss <- numeric(12)

for (i in 1:12) wss[i] <- sum(kmeans(income_elec, centers = i)$withinss)

plot(1:12, wss, type = "b", xlab = "Number Of Clusters", ylab = "Within Groups Sum Of Squares")


k <- kmeans(income_elec, 3)

k

plot(income_elec, col = k$cluster)
points(k$centers,col=1:3, pch = 4)

# 4) Converting data into log10 scale

income_elec_log10 <- log10(income_elec)

#======== Cluster data using k-means with K = 10

k_log10 <- kmeans(income_elec_log10, 10)

k_log10


plot(income_elec_log10, col = k_log10$cluster)
points(k_log10$centers,col=1:10, pch = 4)

#======== cluster data using k-means with K = 3

k_log10 <- kmeans(income_elec_log10, 3)

k_log10


plot(income_elec_log10, col = k_log10$cluster)
points(k_log10$centers,col=1:3, pch = 4)

# 5) Re-evaluating choice of K

wss_log10 <- numeric(12)

for (i in 1:12) wss_log10[i] <- sum(kmeans(income_elec_log10, centers = i)$withinss)

plot(1:12, wss_log10, type = "b", xlab = "Number Of Clusters", ylab = "Within Groups Sum Of Squares")

k_log10 <- kmeans(income_elec_log10, 5)

k_log10


plot(income_elec_log10, col = k_log10$cluster)
points(k_log10$centers,col=1:5, pch = 4)

# 6) Removing outliers 

clean_income_elec_log10 <- subset(income_elec_log10, income_elec_log10$`mean electricity usage` > 2.825)

k_clean <- kmeans(clean_income_elec_log10, 5)

plot(clean_income_elec_log10, col = k_clean$cluster)
points(k_clean$centers,col=1:5, pch = 4)

#============= Re-evaluating choice of K

wss_clean_data <- numeric(12)

for (i in 1:12) wss_clean_data[i] <- sum(kmeans(clean_income_elec_log10, centers = i)$withinss)

plot(1:12, wss_clean_data, type = "b", xlab = "Number Of Clusters", ylab = "Within Groups Sum Of Squares")

k_clean <- kmeans(clean_income_elec_log10, 4)

k_clean


plot(clean_income_elec_log10, col = k_clean$cluster)
points(k_clean$centers,col=1:4, pch = 4)
