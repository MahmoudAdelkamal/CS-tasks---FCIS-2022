img1 = imread("morph.png");
WhitePixels = 0;
[h , w , ~] = size(img1); 
for i = 1:h
  for j = 1:w
    if (img1(i,j,1) == 255) && (img1(i,j,2) == 255) && (img1(i,j,3) == 255) 
        WhitePixels++;
    endif
  endfor
endfor
rtn = WhitePixels / (h*w)
