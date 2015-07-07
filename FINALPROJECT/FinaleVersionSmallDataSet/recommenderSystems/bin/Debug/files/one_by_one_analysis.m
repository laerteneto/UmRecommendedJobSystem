function [jobs1, prediction, jobs2, original_rating, similarity, amount] = one_by_one_analysis(job_list, X, my_ratings, my_predictions, index_predictions, num_top_jobs)

index_rated_jobs = Rated_jobs_Generator(my_ratings, size(job_list,1)); % get just the jobs that were rated 
num_rated_jobs = size(index_rated_jobs,1);
result = zeros(num_top_jobs, num_rated_jobs);

%ONE-BY-ONE ANALYSIS;
amount = 0;

for i = 1:num_top_jobs
    for k = 1: num_rated_jobs
        j = index_predictions(i);
        result(i,k) = manhattanDistance(X(index_rated_jobs(k),:), X(j,:)); % Using Manhattan Distance to compare the features
        if result(i,k) >= .7
            amount = amount + 1;   
        end  
    end
end

jobs1 = zeros(amount, 1);
prediction = zeros(amount, 1);
jobs2 = zeros(amount, 1);
original_rating = zeros(amount, 1);
similarity = zeros(amount, 1);

% getting the predictions that are greater or equal than 70%
h = 1;
for i = 1:num_top_jobs
    for k = 1: num_rated_jobs
        j = index_predictions(i);
        if result(i,k) >= .7
            jobs1(h,1) = j;
            prediction(h,1) = my_predictions(j);
            jobs2(h,1) = index_rated_jobs(k);
            original_rating(h, 1) = my_ratings(index_rated_jobs(k));
            similarity(h, 1) = result(i,k) * 100;
            h = h + 1;
        end  
    end
end
