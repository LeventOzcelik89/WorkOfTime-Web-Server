ČI
jD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime.PublishLater\Program.cs
	namespace		 	
Infoline		
 
.		 

WorkOfTime		 
.		 
PublishLater		 *
{

 
class 	
Program
 
{ 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
Console 
. 
ForegroundColor #
=$ %
ConsoleColor& 2
.2 3
Green3 8
;8 9
Console 
. 
	WriteLine 
( 
$str (
)( )
;) *
var 
	remoteCon 
=  
ConfigurationManager 0
.0 1
AppSettings1 <
[< =
$str= O
]O P
;P Q
var 

connection 
= 
new  
Infoline! )
.) *
	Framework* 3
.3 4
Helper4 :
.: ;
CryptographyHelper; M
(M N
)N O
.O P
DecryptP W
(W X
	remoteConX a
)a b
;b c
var 
executes 
= 
TenantConfig '
.' (

GetTenants( 2
(2 3
)3 4
;4 5
var 
refreshView 
= 
$str  
;  !
using 
( 
var 
_db 
= 
new  
InfolineDatabase! 1
(1 2

connection2 <
.< =
Replace= D
(D E
$strE Y
,Y Z
$strZ f
)f g
,g h
DatabaseTypei u
.u v
Mssqlv {
){ |
)| }
{ 
var 
views 
= 
_db 
.  
ExecuteReader  -
<- .

sysobjects. 8
>8 9
(9 :
$str: s
)s t
.t u
ToListu {
({ |
)| }
;} ~
refreshView 
= 
string $
.$ %
Join% )
() *
$str* -
,- .
views/ 4
.4 5
Select5 ;
(; <
x< =
=>> @
$strA X
+Y Z
x[ \
.\ ]
name] a
+b c
$strd h
)h i
)i j
;j k
} 
foreach 
( 
var 
execute  
in! #
executes$ ,
), -
{ 
if 
( 
! 
execute 
. 

TenantCode '
.' (
HasValue( 0
)0 1
{ 
continue   
;   
}!! 
try## 
{$$ 
using%% 
(%% 
var%% 
_db%% "
=%%# $
new%%% (
InfolineDatabase%%) 9
(%%9 :

connection%%: D
.%%D E
Replace%%E L
(%%L M
$str%%M a
,%%a b
$str%%c o
+%%p q
(%%r s
execute%%s z
.%%z {

TenantCode	%%{ 
==
%% 
$num
%% 
?
%% 
$str
%% 
:
%% 
execute
%% 
.
%% 

TenantCode
%% §
.
%%§ Ø
ToString
%%Ø °
(
%%° ±
)
%%± ²
)
%%² ³
)
%%³ “
,
%%“ µ
DatabaseType
%%¶ Ā
.
%%Ā Ć
Mssql
%%Ć Č
)
%%Č É
)
%%É Ź
{&& 
var'' 
res'' 
=''  !
_db''" %
.''% &
ExecuteNonQuery''& 5
(''5 6
refreshView''6 A
)''A B
;''B C
if(( 
((( 
res(( 
.((  
result((  &
)((& '
{)) 
Console** #
.**# $
ForegroundColor**$ 3
=**4 5
ConsoleColor**6 B
.**B C
Green**C H
;**H I
}++ 
else,, 
{-- 
Console.. #
...# $
ForegroundColor..$ 3
=..4 5
ConsoleColor..6 B
...B C
Red..C F
;..F G
}// 
Console00 
.00  
	WriteLine00  )
(00) *
res00* -
.00- .
result00. 4
?005 6
execute007 >
.00> ?

TenantCode00? I
+00J K
$str00L d
:00e f
execute00g n
.00n o

TenantCode00o y
+00z {
$str	00| 
)
00 
;
00 
}22 
}33 
catch55 
(55 
System55 
.55 
	Exception55 '
)55( )
{66 
Console77 
.77 
ForegroundColor77 +
=77, -
ConsoleColor77. :
.77: ;
Red77; >
;77> ?
Console88 
.88 
	WriteLine88 %
(88% &
execute88& -
.88- .

TenantCode88. 8
+889 :
$str88; T
)88T U
;88U V
}99 
try;; 
{<< 
using== 
(== 
var== 
client== %
===& '
new==( +
	WebClient==, 5
(==5 6
)==6 7
)==7 8
{>> 
var?? 
uri?? 
=??  !
execute??" )
.??) *
	WebDomain??* 3
.??3 4
Split??4 9
(??9 :
$char??: =
)??= >
.??> ?
FirstOrDefault??? M
(??M N
)??N O
+??P Q
$str??R d
;??d e
string@@ 
JsonData@@ '
=@@( )
client@@* 0
.@@0 1
DownloadString@@1 ?
(@@? @
new@@@ C
Uri@@D G
(@@G H
uri@@H K
)@@K L
)@@L M
;@@M N
ConsoleAA 
.AA  
ForegroundColorAA  /
=AA0 1
ConsoleColorAA2 >
.AA> ?
GreenAA? D
;AAD E
ConsoleBB 
.BB  
	WriteLineBB  )
(BB) *
executeBB* 1
.BB1 2

TenantCodeBB2 <
+BB= >
$strBB? R
)BBR S
;BBS T
}CC 
}EE 
catchFF 
(FF 
	ExceptionFF  
)FF  !
{GG 
ConsoleHH 
.HH 
ForegroundColorHH +
=HH, -
ConsoleColorHH. :
.HH: ;
RedHH; >
;HH> ?
ConsoleII 
.II 
	WriteLineII %
(II% &
executeII& -
.II- .

TenantCodeII. 8
+II9 :
$strII; O
)IIO P
;IIP Q
}JJ 
}LL 
ConsoleNN 
.NN 
ForegroundColorNN #
=NN$ %
ConsoleColorNN& 2
.NN2 3
BlueNN3 7
;NN7 8
ConsoleOO 
.OO 
	WriteLineOO 
(OO 
$strOO &
)OO& '
;OO' (
ConsolePP 
.PP 
ReadLinePP 
(PP 
)PP 
;PP 
}RR 	
publicSS 
classSS 

sysobjectsSS 
{TT 	
publicUU 
stringUU 
nameUU 
{UU  
getUU! $
;UU$ %
setUU& )
;UU) *
}UU+ ,
publicVV 
stringVV 
	object_idVV #
{VV$ %
getVV& )
;VV) *
setVV+ .
;VV. /
}VV0 1
publicWW 
stringWW 
principal_idWW &
{WW' (
getWW) ,
;WW, -
setWW. 1
;WW1 2
}WW3 4
publicXX 
stringXX 
	schema_idXX #
{XX$ %
getXX& )
;XX) *
setXX+ .
;XX. /
}XX0 1
publicYY 
stringYY 
parent_object_idYY *
{YY+ ,
getYY- 0
;YY0 1
setYY2 5
;YY5 6
}YY7 8
publicZZ 
stringZZ 
typeZZ 
{ZZ  
getZZ! $
;ZZ$ %
setZZ& )
;ZZ) *
}ZZ+ ,
public[[ 
string[[ 
	type_desc[[ #
{[[$ %
get[[& )
;[[) *
set[[+ .
;[[. /
}[[0 1
public\\ 
string\\ 
create_date\\ %
{\\& '
get\\( +
;\\+ ,
set\\- 0
;\\0 1
}\\2 3
public]] 
string]] 
modify_date]] %
{]]& '
get]]( +
;]]+ ,
set]]- 0
;]]0 1
}]]2 3
public^^ 
string^^ 
is_ms_shipped^^ '
{^^( )
get^^* -
;^^- .
set^^/ 2
;^^2 3
}^^4 5
public__ 
string__ 
is_published__ &
{__' (
get__) ,
;__, -
set__. 1
;__1 2
}__3 4
public`` 
string`` 
is_schema_published`` -
{``. /
get``0 3
;``3 4
set``5 8
;``8 9
}``: ;
}bb 	
}cc 
}dd „
zD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime.PublishLater\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str ;
); <
]< =
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str =
)= >
]> ?
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *