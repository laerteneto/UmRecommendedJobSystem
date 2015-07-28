function [result] = average_analysis(job_list, average_rating, X, my_predictions, index_predictions, num_top_jobs)

result = zeros(num_top_jobs, 1);

fprintf('\nAVERAGE ANALYSIS\n\n');

for i = 1:num_top_jobs
    j = index_predictions(i);
    result(i) = manhattanDistance(average_rating, X(j,:));
    fprintf('Predicting rating %.1f for job %s with an accuracy of %.2f %%\n', my_predictions(j), ...
            job_list{j}, result(i) * 100);
end

fprintf('\n');