function RestoredImage = RemoveMoire2( i )
img=imread(i);
f2 = fft2(img);
f2 = fftshift(f2);
re = real(f2);
im = imag(f2);
mag = sqrt((re.^2)+(im.^2));
lo = log(mag);

%imshow(lo,[]);

f2(155:175 , 115: 140)=0;
f2(165:180 , 45:70)=0;

f2 = ifftshift(f2);
f2=ifft2(f2);
imshow(f2,[]);
RestoredImage=f2;

end