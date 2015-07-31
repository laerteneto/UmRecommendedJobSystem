function distance = manhattanDistance(vector1, vector2)

vector_size = size(vector1, 2);
vector_sum = 0;
for i=1:vector_size
   vector_sum = vector_sum + abs(vector1(i) - vector2(i));
end

%this is the similarity, not the distance
distance = 1 -( vector_sum / vector_size );