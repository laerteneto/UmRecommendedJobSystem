function new_rated_jobs = Rated_jobs_Generator(my_ratings, num_jobs)

%calcular o num_rated_jobs atraves de quantos ~= 0 tem no my_ratings
num_rated_jobs = 0;
for i = 1: num_jobs
    if my_ratings(i) ~= 0
        num_rated_jobs = num_rated_jobs + 1;
    end
end

new_rated_jobs = zeros(num_rated_jobs,1);
k = 1;
for i = 1: num_jobs
    if my_ratings(i) ~= 0
        new_rated_jobs(k) = i;
        k = k + 1;
    end
end