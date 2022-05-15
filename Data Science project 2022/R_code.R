setwd("E:/Mahmoud Alaa/FCIS/4th Year SecondTerm/Data Science/Project/Data Science project 2022")


# Step 1

data <- read.csv("zeta.csv")
# Analyze summary of the data
print(summary(data))
# column names of the zeta table
print(colnames(data))
# The number of rows in the zeta table
print(nrow(data))
# checking if there are multiple rows in the zeta table
unique_data <- unique(data)
print(unique_data)
print(nrow(unique_data))

# Step 2

# load income data 
income_data <- read.delim2("zipincome.txt")
# change columns names
names(data)[1] <- "zipCode"
names(data)[6] <- "income"
print(colnames(data))
# Analyze summary of the data
print(summary(data))

# scatter plot
data_dataFrame <- data[, c("income", "zipCode")]
# This line is because big number shows like 0e+4
options(scipen=10000000)
plot(data_dataFrame$income,data_dataFrame$zipCode, main = "zipCode vs Income",
     xlab = "Income", ylab = "zipCode"
     )

# creating subset of data 
dataSubset <- subset(data,income<200000 & income > 7000)
print(summary(dataSubset))