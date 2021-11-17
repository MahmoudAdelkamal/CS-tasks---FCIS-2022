
function RestoredImage = RemoveMoire( i )
img=imread(i);
f = fft2(img);
f = fftshift(f);
re = real(f);
im = imag(f);
mag = sqrt((re.^2)+(im.^2));
lo = log(mag);

%imshow(lo,[]);

f(70:95 ,255:280)=0;
f(110:135 , 250:270)=0;
f(200:225 , 230: 255)=0;
f(245:270 , 220:255)=0;

f = ifftshift(f);
f=ifft2(f);


imshow(f,[]);
RestoredImage=f;

end