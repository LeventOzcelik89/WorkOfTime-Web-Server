��
}D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Applications\Infoline.WorkOfTime.ApplicationUpdater\Program.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
ApplicationUpdater 0
{
public 

class 
Program 
{ 
public 
static 
DateTime 
LastRunTime *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 
static 
string 
ServicesURL (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
public 
static 
string 
RootPath %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 
static 
string 
RootBackupPath +
{, -
get. 1
;1 2
set3 6
;6 7
}8 9
public 
static 
string 
SiteUrl $
{% &
get' *
;* +
set, /
;/ 0
}1 2
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
Log 
. 
Success 
( 
$str ?
)? @
;@ A
ServicesURL 
= 
System  
.  !

.. / 
ConfigurationManager/ C
.C D
AppSettingsD O
[O P
$strP ]
]] ^
;^ _
RootPath 
= 
System 
. 

.+ , 
ConfigurationManager, @
.@ A
AppSettingsA L
[L M
$strM W
]W X
;X Y
RootBackupPath 
= 
System #
.# $

.1 2 
ConfigurationManager2 F
.F G
AppSettingsG R
[R S
$strS _
]_ `
;` a
SiteUrl 
= 
System 
. 

.* + 
ConfigurationManager+ ?
.? @
AppSettings@ K
[K L
$strL U
]U V
;V W
Log 
. 
Info 
( 
$str #
+$ %
ServicesURL& 1
)1 2
;2 3
Log 
. 
Info 
( 
$str .
+/ 0
RootPath1 9
)9 :
;: ;
Log   
.   
Info   
(   
$str   5
+  6 7
RootBackupPath  8 F
)  F G
;  G H
if"" 
("" 

("" 
)"" 
)""  
{## 
FileUpdater%% 
(%% 
)%% 
;%% 
UpdateVersionCode)) !
())! "
)))" #
;))# $
}++ 
Console,, 
.,, 
ReadLine,, 
(,, 
),, 
;,, 
}-- 	
public// 
static// 
void// 
UpdateVersionCode// ,
(//, -
)//- .
{00 	
try22 
{33 
var44 
fileDir44 
=44 
RootPath44 &
+44' (
$str44) ,
+44- .
$str44/ ;
;44; <
if55 
(55 
File55 
.55 
Exists55 
(55  
fileDir55  '
)55' (
)55( )
{66 
var77 
fileText77  
=77! "
File77# '
.77' (
ReadAllText77( 3
(773 4
fileDir774 ;
)77; <
;77< =
var99 
regx99 
=99 
new99 "
Regex99# (
(99( )
$str99) Y
)99Y Z
;99Z [
var:: 
rs:: 
=:: 
regx:: !
.::! "
Match::" '
(::' (
fileText::( 0
)::0 1
;::1 2
if<< 
(<< 
rs<< 
.<< 
Success<< "
)<<" #
{== 
var?? 
newValue?? $
=??% &
Guid??' +
.??+ ,
NewGuid??, 3
(??3 4
)??4 5
.??5 6
ToString??6 >
(??> ?
)??? @
.??@ A
	Substring??A J
(??J K
$num??K L
,??L M
$num??N O
)??O P
;??P Q
fileText@@  
=@@! "
fileText@@# +
.@@+ ,
Replace@@, 3
(@@3 4
rs@@4 6
.@@6 7
Value@@7 <
,@@< =
$str@@> a
+@@b c
newValue@@d l
+@@m n
$str@@o v
)@@v w
;@@w x
FileBB 
.BB 
WriteAllTextBB )
(BB) *
fileDirBB* 1
,BB1 2
fileTextBB3 ;
)BB; <
;BB< =
}DD 
LogFF 
.FF 
SuccessFF 
(FF  
$strFF  :
)FF: ;
;FF; <
}HH 
}II 
catchJJ 
(JJ 
	ExceptionJJ 
exJJ 
)JJ  
{KK 
LogLL 
.LL 
ErrorLL 
(LL 
$strLL 7
+LL8 9
exLL: <
.LL< =
MessageLL= D
)LLD E
;LLE F
}MM 
}OO 	
publicQQ 
staticQQ 
boolQQ 

(QQ( )
)QQ) *
{RR 	
usingTT 
(TT 
varTT 
clientTT 
=TT 
newTT  #
	WebClientTT$ -
(TT- .
)TT. /
)TT/ 0
{UU 
tryVV 
{WW 
varXX 
strXX 
=XX 
clientXX $
.XX$ %
DownloadStringXX% 3
(XX3 4
ServicesURLXX4 ?
+XX@ A
$strXXB T
)XXT U
;XXU V
ifYY 
(YY 
!YY 
stringYY 
.YY  

(YY- .
strYY. 1
)YY1 2
)YY2 3
{ZZ 
Log[[ 
.[[ 
Success[[ #
([[# $
ServicesURL[[$ /
+[[0 1
$str[[2 P
)[[P Q
;[[Q R
return\\ 
true\\ #
;\\# $
}]] 
else^^ 
{__ 
Log`` 
.`` 
Error`` !
(``! "
ServicesURL``" -
+``. /
$str``0 u
)``u v
;``v w
returnaa 
falseaa $
;aa$ %
}bb 
}cc 
catchdd 
(dd 
	Exceptiondd  
exdd! #
)dd# $
{ee 
Logff 
.ff 
Errorff 
(ff 
ServicesURLff )
+ff* +
$strff, y
+ffz {
exff| ~
)ff~ 
;	ff �
returngg 
falsegg  
;gg  !
}hh 
}ii 
}jj 	
publicll 
staticll 
voidll 
FileUpdaterll &
(ll& '
)ll' (
{mm 	
usingoo 
(oo 
varoo 
clientoo 
=oo 
newoo  #
	WebClientoo$ -
(oo- .
)oo. /
)oo/ 0
{pp 
clientqq 
.qq 
Encodingqq 
=qq  !
Encodingqq" *
.qq* +
UTF8qq+ /
;qq/ 0
varrr 

serverDaterr 
=rr  
DateTimerr! )
.rr) *
Nowrr* -
;rr- .
tryss 
{tt 

serverDateuu 
=uu  
Infolineuu! )
.uu) *
Helperuu* 0
.uu0 1
Jsonuu1 5
.uu5 6
Deserializeuu6 A
<uuA B
DateTimeuuB J
>uuJ K
(uuK L
clientuuL R
.uuR S
DownloadStringuuS a
(uua b
ServicesURLuub m
+uun o
$str	uup �
)
uu� �
)
uu� �
;
uu� �
}vv 
catchww 
{ww 
}ww 
ifzz 
(zz 
DateTimezz 
.zz 
Nowzz  
.zz  !
ToStringzz! )
(zz) *
$strzz* ;
)zz; <
!=zz= ?

serverDatezz@ J
.zzJ K
ToStringzzK S
(zzS T
$strzzT e
)zze f
)zzf g
{{{ 
Log|| 
.|| 
Info|| 
(|| 
$str|| H
)||H I
;||I J
var}} 
rs}} 
=}} 
WinTime}} $
.}}$ %
SetLocalTime}}% 1
(}}1 2

serverDate}}2 <
)}}< =
;}}= >
if~~ 
(~~ 
rs~~ 
)~~ 
{ 
Log
�� 
.
�� 
Success
�� #
(
��# $
$str
��$ T
)
��T U
;
��U V
}
�� 
else
�� 
{
�� 
Log
�� 
.
�� 
Error
�� !
(
��! "
$str
��" M
)
��M N
;
��N O
}
�� 
}
�� 
var
�� 

BackupPath
�� 
=
��  
Path
��! %
.
��% &
Combine
��& -
(
��- .
RootPath
��. 6
,
��6 7
$str
��8 ?
,
��? @
$str
��A J
)
��J K
;
��K L
if
�� 
(
�� 
!
�� 
	Directory
�� 
.
�� 
Exists
�� %
(
��% &

BackupPath
��& 0
)
��0 1
)
��1 2
{
�� 
	Directory
�� 
.
�� 
CreateDirectory
�� -
(
��- .

BackupPath
��. 8
)
��8 9
;
��9 :
}
�� 
var
�� "
UserTransactionsPath
�� (
=
��) *
Path
��+ /
.
��/ 0
Combine
��0 7
(
��7 8
RootPath
��8 @
,
��@ A
$str
��B I
,
��I J
$str
��K ]
)
��] ^
;
��^ _
if
�� 
(
�� 
!
�� 
	Directory
�� 
.
�� 
Exists
�� %
(
��% &"
UserTransactionsPath
��& :
)
��: ;
)
��; <
{
�� 
	Directory
�� 
.
�� 
CreateDirectory
�� -
(
��- ."
UserTransactionsPath
��. B
)
��B C
;
��C D
}
�� 
var
�� 

backupPath
�� 
=
��  
Path
��! %
.
��% &
Combine
��& -
(
��- .
RootBackupPath
��. <
,
��< =
DateTime
��> F
.
��F G
Now
��G J
.
��J K
ToString
��K S
(
��S T
$str
��T f
)
��f g
)
��g h
;
��h i
var
�� 
serverFiles
�� 
=
��  !
Infoline
��" *
.
��* +
Helper
��+ 1
.
��1 2
Json
��2 6
.
��6 7
Deserialize
��7 B
<
��B C

Dictionary
��C M
<
��M N
string
��N T
,
��T U
DateTime
��V ^
>
��^ _
>
��_ `
(
��` a
client
��a g
.
��g h
DownloadString
��h v
(
��v w
ServicesURL��w �
+��� �
$str��� �
)��� �
)��� �
;��� �
var
�� 
i
�� 
=
�� 
$num
�� 
;
�� 
foreach
�� 
(
�� 
var
�� 

serverfile
�� '
in
��( *
serverFiles
��+ 6
)
��6 7
{
�� 
var
�� 
filePath
��  
=
��! "
Path
��# '
.
��' (
Combine
��( /
(
��/ 0
RootPath
��0 8
,
��8 9

serverfile
��: D
.
��D E
Key
��E H
)
��H I
;
��I J
var
�� 
	extension
�� !
=
��" #
Path
��$ (
.
��( )
GetExtension
��) 5
(
��5 6
filePath
��6 >
)
��> ?
;
��? @
var
�� 
	directory
�� !
=
��" #
Path
��$ (
.
��( )
GetDirectoryName
��) 9
(
��9 :
filePath
��: B
)
��B C
;
��C D
var
�� 
fileName
��  
=
��! "
Path
��# '
.
��' (
GetFileName
��( 3
(
��3 4
filePath
��4 <
)
��< =
;
��= >
var
�� 
downloadURL
�� #
=
��$ %
ServicesURL
��& 1
+
��2 3
$str
��4 L
+
��M N

serverfile
��O Y
.
��Y Z
Key
��Z ]
;
��] ^
try
�� 
{
�� 
if
�� 
(
�� 
!
�� 
	Directory
�� &
.
��& '
Exists
��' -
(
��- .
	directory
��. 7
)
��7 8
)
��8 9
{
�� 
	Directory
�� %
.
��% &
CreateDirectory
��& 5
(
��5 6
	directory
��6 ?
)
��? @
;
��@ A
}
�� 
if
�� 
(
�� 
!
�� 
File
�� !
.
��! "
Exists
��" (
(
��( )
filePath
��) 1
)
��1 2
)
��2 3
{
�� 
client
�� "
.
��" #
DownloadFile
��# /
(
��/ 0
downloadURL
��0 ;
,
��; <
filePath
��= E
)
��E F
;
��F G
Log
�� 
.
��  
Success
��  '
(
��' (

serverfile
��( 2
.
��2 3
Key
��3 6
+
��7 8
$str
��9 M
)
��M N
;
��N O
i
�� 
++
�� 
;
��  
}
�� 
else
�� 
{
�� 
var
�� 
info
��  $
=
��% &
new
��' *
FileInfo
��+ 3
(
��3 4
filePath
��4 <
)
��< =
;
��= >
if
�� 
(
��  
info
��  $
.
��$ %

��% 2
<
��3 4

serverfile
��5 ?
.
��? @
Value
��@ E
&&
��F H
	extension
��I R
!=
��S U
$str
��V _
)
��_ `
{
�� 
var
��  #
fileBackupPath
��$ 2
=
��3 4
Path
��5 9
.
��9 :
Combine
��: A
(
��A B

backupPath
��B L
,
��L M

serverfile
��N X
.
��X Y
Key
��Y \
)
��\ ]
;
��] ^
var
��  #!
directoryBackupPath
��$ 7
=
��8 9
Path
��: >
.
��> ?
GetDirectoryName
��? O
(
��O P
fileBackupPath
��P ^
)
��^ _
;
��_ `
if
��  "
(
��# $
!
��$ %
	Directory
��% .
.
��. /
Exists
��/ 5
(
��5 6!
directoryBackupPath
��6 I
)
��I J
)
��J K
{
��  !
	Directory
��$ -
.
��- .
CreateDirectory
��. =
(
��= >!
directoryBackupPath
��> Q
)
��Q R
;
��R S
}
��  !
File
��  $
.
��$ %
Copy
��% )
(
��) *
filePath
��* 2
,
��2 3
fileBackupPath
��4 B
)
��B C
;
��C D
Log
��  #
.
��# $
Success
��$ +
(
��+ ,

serverfile
��, 6
.
��6 7
Key
��7 :
+
��; <
$str
��= \
)
��\ ]
;
��] ^
client
��  &
.
��& '
DownloadFile
��' 3
(
��3 4
downloadURL
��4 ?
,
��? @
filePath
��A I
)
��I J
;
��J K
Log
��  #
.
��# $
Success
��$ +
(
��+ ,

serverfile
��, 6
.
��6 7
Key
��7 :
+
��; <
$str
��= T
)
��T U
;
��U V
i
��  !
++
��! #
;
��# $
}
�� 
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� $
ex
��% '
)
��' (
{
�� 
Log
�� 
.
�� 
Error
�� !
(
��! "

serverfile
��" ,
.
��, -
Key
��- 0
+
��1 2
$str
��3 H
+
��I J
ex
��K M
.
��M N
Message
��N U
)
��U V
;
��V W
}
�� 
}
�� 
if
�� 
(
�� 
i
�� 
>
�� 
$num
�� 
)
�� 
{
�� 
Log
�� 
.
�� 
Warning
�� 
(
��  
$str
��  3
+
��4 5
i
��6 7
+
��8 9
$str
��: l
)
��l m
;
��m n
try
�� 
{
�� 
var
�� 

�� )
=
��* +
Infoline
��, 4
.
��4 5
Helper
��5 ;
.
��; <
Json
��< @
.
��@ A
Deserialize
��A L
<
��L M
Versions
��M U
>
��U V
(
��V W
client
��W ]
.
��] ^
DownloadString
��^ l
(
��l m
SiteUrl
��m t
+
��u v
$str��w �
)��� �
)��� �
;��� �
var
�� 
clientSaveVersion
�� -
=
��. /
client
��0 6
.
��6 7
DownloadString
��7 E
(
��E F
ServicesURL
��F Q
+
��R S
$str
��T v
+
��w x

.��� �
SiteUrl��� �
+��� �
$str��� �
+��� �

.��� �
Version��� �
+��� �
$str��� �
+��� �
RootBackupPath��� �
)��� �
;��� �
Log
�� 
.
�� 
Warning
�� #
(
��# $
$str
��$ I
)
��I J
;
��J K
}
�� 
catch
�� 
(
�� 
	Exception
�� $
ex
��% '
)
��' (
{
�� 
Log
�� 
.
�� 
Warning
�� #
(
��# $
ex
��$ &
.
��& '
Message
��' .
)
��. /
;
��/ 0
throw
�� 
;
�� 
}
�� 
}
�� 
else
�� 
{
�� 
Log
�� 
.
�� 
Warning
�� 
(
��  
$str
��  U
)
��U V
;
��V W
}
�� 
try
�� 
{
�� 
client
�� 
.
�� 
DownloadString
�� )
(
��) *
$str
��* L
)
��L M
;
��M N
}
�� 
catch
�� 
{
�� 
}
�� 
}
�� 
}
�� 	
}
�� 
public
�� 

class
�� 
Versions
�� 
{
�� 
public
�� 
string
�� 
Version
�� 
{
�� 
get
��  #
;
��# $
set
��% (
;
��( )
}
��* +
public
�� 
string
�� 
SiteUrl
�� 
{
�� 
get
��  #
;
��# $
set
��% (
;
��( )
}
��* +
}
�� 
public
�� 

static
�� 
class
�� 
WinTime
�� 
{
�� 
[
�� 	
StructLayout
��	 
(
�� 

LayoutKind
��  
.
��  !

Sequential
��! +
)
��+ ,
]
��, -
private
�� 
struct
�� 

SYSTEMTIME
�� !
{
�� 	
public
�� 
ushort
�� 
wYear
�� 
;
��  
public
�� 
ushort
�� 
wMonth
��  
;
��  !
public
�� 
ushort
�� 

wDayOfWeek
�� $
;
��$ %
public
�� 
ushort
�� 
wDay
�� 
;
�� 
public
�� 
ushort
�� 
wHour
�� 
;
��  
public
�� 
ushort
�� 
wMinute
�� !
;
��! "
public
�� 
ushort
�� 
wSecond
�� !
;
��! "
public
�� 
ushort
�� 

�� '
;
��' (
}
�� 	
[
�� 	
	DllImport
��	 
(
�� 
$str
�� !
,
��! "

EntryPoint
��# -
=
��. /
$str
��0 >
,
��> ?
SetLastError
��@ L
=
��M N
true
��O S
)
��S T
]
��T U
private
�� 
extern
�� 
static
�� 
bool
�� "
Win32SetLocalTime
��# 4
(
��4 5
ref
��5 8

SYSTEMTIME
��9 C
lpSystemTime
��D P
)
��P Q
;
��Q R
public
�� 
static
�� 
bool
�� 
SetLocalTime
�� '
(
��' (
DateTime
��( 0
sysTime
��1 8
)
��8 9
{
�� 	

SYSTEMTIME
�� 
systime
�� 
=
��  
new
��! $

SYSTEMTIME
��% /
(
��/ 0
)
��0 1
;
��1 2
systime
�� 
.
�� 
wYear
�� 
=
�� 
(
�� 
ushort
�� #
)
��# $
sysTime
��$ +
.
��+ ,
Year
��, 0
;
��0 1
systime
�� 
.
�� 
wMonth
�� 
=
�� 
(
�� 
ushort
�� $
)
��$ %
sysTime
��% ,
.
��, -
Month
��- 2
;
��2 3
systime
�� 
.
�� 

wDayOfWeek
�� 
=
��  
(
��! "
ushort
��" (
)
��( )
sysTime
��) 0
.
��0 1
	DayOfWeek
��1 :
;
��: ;
systime
�� 
.
�� 
wDay
�� 
=
�� 
(
�� 
ushort
�� "
)
��" #
sysTime
��# *
.
��* +
Day
��+ .
;
��. /
systime
�� 
.
�� 
wHour
�� 
=
�� 
(
�� 
ushort
�� #
)
��# $
sysTime
��$ +
.
��+ ,
Hour
��, 0
;
��0 1
systime
�� 
.
�� 
wMinute
�� 
=
�� 
(
�� 
ushort
�� %
)
��% &
sysTime
��& -
.
��- .
Minute
��. 4
;
��4 5
systime
�� 
.
�� 
wSecond
�� 
=
�� 
(
�� 
ushort
�� %
)
��% &
sysTime
��& -
.
��- .
Second
��. 4
;
��4 5
systime
�� 
.
�� 

�� !
=
��" #
(
��$ %
ushort
��% +
)
��+ ,
sysTime
��, 3
.
��3 4
Millisecond
��4 ?
;
��? @
return
�� 
Win32SetLocalTime
�� $
(
��$ %
ref
��% (
systime
��) 0
)
��0 1
;
��1 2
}
�� 	
}
�� 
}�� �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Applications\Infoline.WorkOfTime.ApplicationUpdater\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 


( 
$str A
)A B
]B C
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
$str C
)C D
]D E
[
assembly
:

AssemblyCopyright
(
$str
)
]
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