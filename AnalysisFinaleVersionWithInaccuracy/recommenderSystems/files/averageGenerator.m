function average = averageGenerator(X, my_ratings, num_features, num_jobs)

average = zeros(num_features, 1);
num_rated_jobs = 0;
for i = 1: num_jobs
    if my_ratings(i) ~= 0
        num_rated_jobs = num_rated_jobs + 1;
        for j = 1: num_features
            average(j) = average(j) + X(i,j);
        end
    end
end

for j = 1: num_features
     average(j) = average(j) / num_rated_jobs ;
end
