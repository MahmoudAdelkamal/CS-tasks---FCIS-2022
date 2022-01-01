pkg load image;
img = imread('4.1.bmp');
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

#figure, imshow(res);


cc = bwconncomp(~res);

s = regionprops(cc,'BoundingBox');

Cimg = true(size(res));
for i = 1:cc.NumObjects
  if abs([s(i).BoundingBox(3)] - [s(i).BoundingBox(4)]) <= 2 
     Cimg(cc.PixelIdxList{i}) = false;
 
 endif

endfor
#figure, imshow(Cimg);
Cimg=~Cimg;


Afilled = imfill(Cimg,'holes'); % fill holes
L = bwlabel(Afilled); % label each connected component
holes = Afilled - Cimg; % get only holes
componentLabels = unique(nonzeros(L.*holes)); % get labels of components which have at least one hole
Cimg = Cimg.*L; % label original image
Cimg(~ismember(Cimg,componentLabels)) = 0; % delete all components which have no hole

#figure, imshow(~Cimg);

##if corners(1,1)-corners(2,1) > 0 && abs(corners(1,3)-corners(2,3))<=2
##		if abs(corners(1,1)-corners(2,1))<=2
##			final_img=res(corners(2,1):corners(1,3),corners(1,2):corners(3,4));
##      endif
##	
##endif

#figure, imshow(res);
for i = 0 : 360
tmp=Cimg;
tmp=imrotate(tmp,i);

ss = bwconncomp(tmp);
numPixels = cellfun(@numel,ss.PixelIdxList);
N=size(numPixels,2);

corners = zeros(0,4);

for j=1:N
#size(corners)

  [X,Y] = ind2sub(size(tmp),ss.PixelIdxList{j});
  
  mnx=min(X);
  mny=min(Y);
  
  mxx=max(X);
  mxy=max(Y);
  W=mxx-mnx;
  H=mxy-mny;
    
  FullObject=tmp(mnx:mxx,mny:mxy);
  
  ####
  #figure,imshow(FullObject);
  
  Afilled = imfill(FullObject,'holes'); % fill holes
  #figure, imshow(~Afilled);  
  
  
  #L = bwlabel(Afilled); % label each connected component
  holes = Afilled - FullObject; % get only holes
  
  
  Afilled = imfill(holes,'holes'); % fill holes
  
  
  #L = bwlabel(Afilled); % label each connected component
  Inner_Square = Afilled - holes; % get only holes
  
  s = regionprops(~Inner_Square,'BoundingBox');
  if W==0 || H==0 
    continue
  endif
  if Inner_Square(floor(H/2),floor(W/2)) != 0
    if abs([s(1).BoundingBox(3)] - [s(1).BoundingBox(4)]) <= 2 
      NewCorner = [mnx, mxx, mny, mxy];
      corners = [corners; NewCorner];
    endif
  endif
  
endfor
if size(corners)!=3
  continue
endif

answer = IsValidQrCode (corners);
#answer
if answer(1)==1
  tmpres=res;
  tmpres=imrotate(tmpres,i);
  if answer(2)>answer(3) || answer(4)>answer(5)
    continue
  endif
  final_img=tmpres(answer(2):answer(3),answer(4):answer(5));
  figure,imshow(final_img);
  #break;
endif


endfor







