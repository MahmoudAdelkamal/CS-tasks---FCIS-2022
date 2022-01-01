
function answer = IsValidQrCode (corners)
newMnx=0;
newMxx=0;
newMny=0;
newMxy=0;
retval=0;
C=2;
% try 1 with other points
if corners(1,1)-C > corners(2,1) && abs(corners(1,3)-corners(2,3))<=2
		if abs(corners(3,1)-corners(2,1))<=2 && corners(3,1)+C < corners(1,1)
			newMnx=corners(2,1);
      newMxx=corners(1,2);
      newMny=corners(1,3);
      newMxy=corners(3,4);
      retval=1;
		endif
endif   

if corners(1,1)-C > corners(3,1)&& abs(corners(1,3)-corners(3,3))<=2
	if abs(corners(3,1)-corners(2,1))<=2 && corners(2,1)+C < corners(1,1)
      newMnx=corners(3,1);
      newMxx=corners(1,2);
      newMny=corners(1,3);
      newMxy=corners(2,4);
      retval=1;
		endif
endif



% try 2 with other points
if corners(2,1)-C > corners(1,1) && abs(corners(2,3)-corners(1,3))<=2
		if abs(corners(3,1)-corners(1,1))<=2 && corners(3,1)+C < corners(2,1)
			newMnx=corners(1,1);
      newMxx=corners(2,2);
      newMny=corners(2,3);
      newMxy=corners(3,4);
      retval=1;
		endif
endif


if corners(2,1)-C > corners(3,1) && abs(corners(2,3)-corners(3,3))<=2
	if abs(corners(3,1)-corners(1,1))<=2 && corners(1,1)+C < corners(2,1)
			newMnx=corners(3,1);
      newMxx=corners(2,2);
      newMny=corners(2,3);
      newMxy=corners(1,4);
      retval=1;
		endif
endif

% try 3 with other points
if corners(3,1)-C > corners(1,1) && abs(corners(3,3)-corners(1,3))<=2
		if abs(corners(1,1)-corners(2,1))<=2 && corners(2,1)+C < corners(3,1)
			newMnx=corners(1,1);
      newMxx=corners(3,2);
      newMny=corners(3,3);
      newMxy=corners(2,4);
      retval=1;
		endif

endif

if corners(3,1)- C > corners(2,1) && abs(corners(3,3)-corners(2,3))<=2
	if abs(corners(1,1)-corners(2,1))<=2 && corners(1,1)+C < corners(3,1)
			newMnx=corners(2,1);
      newMxx=corners(3,2);
      newMny=corners(3,3);
      newMxy=corners(1,4);
      retval=1;
   endif
endif	

answer=[retval,newMnx,newMxx,newMny,newMxy];

endfunction
