img = imread('4.4.bmp');
bwimg = im2bw(img);
img = rgb2gray(img);
img = medfilt2(img);
[h,w] = size(img);


res = img;

for i = 1:(h/20)
  for j = 1:(w/20)

  x = i*20;
  y = j*20;
  
  mn = 255;
  mx = 0;
  
  ist = x-19;
  jst = y-19;
   
  for ii = ist:x
    for jj = jst:y
      
      ### MIN
      if img(ii,jj) < mn
        mn = img(ii,jj);
      endif
      
      ### MAX
      if img(ii,jj) > mx
        mx = img(ii,jj);
      endif

    endfor
  endfor
  
  if mx > mn+55
    th = mn + (mx - mn)/1.5;  
    
    for ii = ist:x
      for jj = jst:y
        if img(ii,jj) >= th
          res(ii,jj) = 255;
        else
          res(ii,jj) = 0;
        endif
        
      endfor
    endfor
  endif
  endfor
endfor



res = im2bw(res);

#figure, imshow(img);
#figure, imshow(bwimg);
figure, imshow(res);
L=bwlabel(res);
cc = bwconncomp(~res);
s = regionprops(cc,'BoundingBox');

Cimg = true(size(res));
for i = 1:cc.NumObjects
  if abs([s(i).BoundingBox(3)] - [s(i).BoundingBox(4)]) <= 2 
     Cimg(cc.PixelIdxList{i}) = false;
  endif

endfor
figure, imshow(Cimg);
Cimg=~Cimg;
Afilled = imfill(Cimg,'holes'); % fill holes
L = bwlabel(Afilled); % label each connected component
holes = Afilled - Cimg; % get only holes
componentLabels = unique(nonzeros(L.*holes)); % get labels of components which have at least one hole
Cimg = Cimg.*L; % label original image
Cimg(~ismember(Cimg,componentLabels)) = 0; % delete all components which have no hole
#Cimg(Cimg~=0)=1; % turn back from labels to binary - since you are later continuing with regionprops you maybe don't need this step.
#Cimg=~Cimg;
figure, imshow(Cimg);
ss = bwconncomp(Cimg);
numPixels = cellfun(@numel,ss.PixelIdxList);
N=size(numPixels,2);


for i=1:N

[X,Y] = ind2sub(size(Cimg),ss.PixelIdxList{i});

mnx=min(X);
mny=min(Y);

mxx=max(X);
mxy=max(Y);

H=mxx-mnx;
W=mxy-mny;
H
W
Z=~Cimg(mnx:mxx,mny:mxy);
figure,imshow(Z);
endfor



