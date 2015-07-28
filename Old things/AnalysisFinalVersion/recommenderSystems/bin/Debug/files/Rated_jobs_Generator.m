function new_rated_jobs = Rated_jobs_Generator(my_ratings, num_jobs)

%calculate the number of num_rated_jobs by how many ~= 0 there are in my_ratings
num_rated_jobs = 0;
for i = 1: num_jobs
    if my_ratings(i) ~= 0
        num_rated_jobs = num_rated_jobs + 1;
    end
end
%zeros to empty cells
new_rated_jobs = zeros(num_rated_jobs,1);
k = 1;
%letting the system know where there are no zeros
for i = 1: num_jobs
    if my_ratings(i) ~= 0
        new_rated_jobs(k) = i;
        k = k + 1;
    end
end