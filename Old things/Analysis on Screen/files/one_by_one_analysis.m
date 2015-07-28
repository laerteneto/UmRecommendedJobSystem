function [result] = one_by_one_analysis(job_list, X, my_ratings, my_predictions, index_predictions, num_top_jobs)

index_rated_jobs = Rated_jobs_Generator(my_ratings, size(job_list,1));
num_rated_jobs = size(index_rated_jobs,1);
result = zeros(num_top_jobs, num_rated_jobs);

fprintf('\nONE-BY-ONE ANALYSIS\n\n');

for i = 1:num_top_jobs
    for k = 1: num_rated_jobs
        j = index_predictions(i);
        result(i,k) = manhattanDistance(X(index_rated_jobs(k),:), X(j,:));
        if result(i,k) >= .7
            fprintf('Predicting rating %.1f for job %s - Related with the job %s that has a rating %d - the similarity is %.2f %%\n',  ...
                my_predictions(j), job_list{j}, job_list{index_rated_jobs(k)}, my_ratings(index_rated_jobs(k)), result(i,k) * 100);
        end  
    end
end