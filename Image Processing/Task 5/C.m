img1 = imread("morph.png");
imgcomp = imcomplement(img1);
[l,k] = bwlabel(im2bw(imgcomp));
ans = k-1;
ans
