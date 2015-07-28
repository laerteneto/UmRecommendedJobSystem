function [HI] = scriptGeneration(my_ratings, job_list ,Y, R, X, num_features)
HI = 0;

fprintf('\n\nNew user ratings:\n');

X2 = X;

for i = 1:length(my_ratings)
    if my_ratings(i) > 0 
        fprintf('Rated %d for %s\n', my_ratings(i), ...
                 job_list{i});
    end
end

fprintf('\n\nPress ENTER to continue:\n');
pause;


fprintf('\nTraining collaborative filtering...\n');

%  Add our own ratings to the data matrix
Y = [my_ratings Y];
R = [(my_ratings ~= 0) R];

%  Normalize Ratings
[Ynorm, Ymean] = normalizeRatings(Y, R);

%  Useful Values
num_users = size(Y, 2);
num_movies = size(Y, 1);

% Set Initial Parameters (Theta, X)
%X = randn(num_movies, num_features);
Theta = randn(num_users, num_features);

initial_parameters = [X(:); Theta(:)];

% Set options for fmincg
options = optimset('GradObj', 'on', 'MaxIter', 100);

% Set Regularization
lambda = 10;
theta = fmincg (@(t)(cofiCostFunc(t, Y, R, num_users, num_movies, ...
                                num_features, lambda)), ...
                initial_parameters, options);

% Unfold the returned theta back into U and W
X = reshape(theta(1:num_movies*num_features), num_movies, num_features);
Theta = reshape(theta(num_movies*num_features+1:end), ...
                num_users, num_features);

fprintf('Recommender system learning completed.\n');

fprintf('\nProgram paused. Press enter to continue.\n');
pause;


%% ================== Part 8: Recommendation for you ====================
%  After training the model, you can now make recommendations by computing
%  the predictions matrix.
%

p = X * Theta';
my_predictions = p(:,1);

%put an if here, if the new user has no ratings for any job, add the + Ymean
if nnz(my_ratings) == 0 %nnz == number of non-zeros
    my_predictions = my_predictions + Ymean;
end

[r, ix] = sort(my_predictions, 'descend');
%fprintf('\nTop recommendations for you:\n');
%for i=1:10
%    j = ix(i);
%    fprintf('Predicting rating %.1f for job %s\n', my_predictions(j), ...
%            job_list{j});
%end

%fprintf('\n\nOriginal ratings provided:\n');
%for i = 1:length(my_ratings)
%    if my_ratings(i) > 0 
%        fprintf('Rated %d for %s\n', my_ratings(i), ...
%                 job_list{i});
%    end
%end


% average_rating = averageGenerator(X2, my_ratings, num_features, size(job_list,1));
% average_analysis(job_list, average_rating, X2, my_predictions, ix, 10);

% fprintf('\nProgram paused. Press enter to continue.\n');
% pause;

one_by_one_analysis(job_list, X2, my_ratings, my_predictions, ix, 10);
pause;

