function [out] = AddMan(Background,  man)
  out = Background;
  man = ExtractMan(man);
 [h, w, e] = size(man);
 for i = 1 :h
   for j = 1:w
     if man(i,j,2) != 255
       out(i,j,1) = man(i,j,1);
       out(i,j,2) = man(i,j,2);
       out(i,j,3) = man(i,j,3);
     endif
   endfor
 endfor
 imshow(out);
 end
