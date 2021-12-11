i=imread("morph.png");
A = im2bw(i);
props = regionprops(A, 'Area'); % Find areas of each blob and put into a structure.
allAreas = [props.Area];
cnt=0;
for curArea=allAreas
  tmp=uint32(sqrt(curArea));
  tmp=tmp*tmp;
  if(tmp==uint32(curArea))
  cnt=cnt+1;
  endif
endfor
cnt