i=imread("morph.png");
A = im2bw(i);
B = regionprops(A, 'EulerNumber');
%Any single object with no holes will have an Euler characteristic of 1, any single object with 1 hole will have an Euler characteristic of 0, two holes -> -1 etc. So you can segment out all the objects with EC < 1.
holeIndices = find( [B.EulerNumber] < 1 ); % returns 1* size of array so i take the 2nd dimension 
cnt=size(holeIndices,2);
cnt

