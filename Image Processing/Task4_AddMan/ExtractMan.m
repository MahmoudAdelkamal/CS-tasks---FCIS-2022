function [man] = ExtractMan(I)
[h, w, e] = size(I);
J = I;
for i = 1 : h
  for j = 1 : w
    R = I(i,j,1);
    G = I(i,j,2);
    B = I(i,j,3);
    if B >= 88 && (B-G > 40 || B-R > 30)
      I(i,j,1) = 0;
      I(i,j,2) = 255;
      I(i,j,3) = 0;
    elseif R > 180 && max(abs(R-G),max(abs(R-B),abs(G-B))) <= 19
      I(i,j,1) = 0;
      I(i,j,2) = 255;
      I(i,j,3) = 0;
      
    endif
  endfor
endfor
man = I;
imshow(man);
end