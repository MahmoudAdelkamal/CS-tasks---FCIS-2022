setwd("D:/college material/4th year/2nd term/DataScience/Data Science project 2022")


# Step 1

data <- read.csv("zeta.csv")
print(summary(data))
# column names of the zeta table
print(colnames(data))
# the number of rows in the zeta table
print(nrow(data))
# checking if there are multiple rows in the zeta table
unique_data <- unique(data)
print(unique_data)
print(nrow(unique_data))



