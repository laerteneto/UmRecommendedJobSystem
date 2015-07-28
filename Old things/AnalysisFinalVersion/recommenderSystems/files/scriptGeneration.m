function [jobs1, prediction, jobs2, original_rating, similarity, amount] = scriptGeneration(my_ratings, job_list ,Y, R, X, num_features)

% Returns:
%jobs 1 is the list of the top 10 jobs for each user ; prediction explaim
%itself; jobs2 are the other jobs that have a good rating compared with the 
%jobs1 with similarity greater or equal than 70%; original_rating are the
%original rating from some jobs given by the dataset; similarity is a percentage
%valeu to calculated how the features compareted in each job  are similar, 
%the amount is a security return to ensure that if a Job has no 10 top jobs with 
%70%, the system will calculate the averages just based in the quantity of
%top jobs returned


%Copy the matrix X to be used in one by one analysis without lose the
%original format, because the original X will be used and modified later
X2 = X;

%% Training collaborative filtering);
%  Add our own ratings to the data matrix
Y = [my_ratings Y];
R = [(my_ratings ~= 0) R];

%  Useful Values
num_users = size(Y, 2);
num_jobs = size(Y, 1);

% Set Initial Parameters (Theta, X)
Theta = randn(num_users, num_features);
% To be used for thetha meausurement
initial_parameters = [X(:); Theta(:)];

% Set options for fmincg
options = optimset('GradObj', 'on', 'MaxIter', 100);

% Set Regularization
lambda = 10;
theta = fmincg (@(t)(cofiCostFunc(t, Y, R, num_users, num_jobs, ...
                                num_features, lambda)), ...
                initial_parameters, options);

% Unfold the returned theta back into U and W
X = reshape(theta(1:num_jobs*num_features), num_jobs, num_features);
Theta = reshape(theta(num_jobs*num_features+1:end), ...
                num_users, num_features);

%% Recommender system learning completed at this point


% After training the model, we can now make recommendations by computing 
%the predictions matrix.
p = X * Theta';
my_predictions = p(:,1);

%if the new user has no ratings for any job, add the + Ymean
if nnz(my_ratings) == 0 %nnz == number of non-zeros
    my_predictions = my_predictions + Ymean;
end

% put the predictions in ix(index) in decrescent order
[r, ix] = sort(my_predictions, 'descend');

[jobs1, prediction, jobs2, original_rating, similarity, amount] = one_by_one_analysis(job_list, X2, my_ratings, my_predictions, ix, 10);


