�}
aD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.DownloadFilesApp\Form1.cs
	namespace 	
Infoline
 
. 
DownloadFilesApp #
{ 
public 

partial 
class 
Form1 
:  
Form! %
{ 
public 
Form1 
( 
) 
{ 	
InitializeComponent 
(  
)  !
;! "
} 	
private 
void "
btnDownloadFiles_Click +
(+ ,
object, 2
sender3 9
,9 :
	EventArgs; D
eE F
)F G
{ 	
if 
( 

==  
null! %
)% &
{ 

MessageBox 
. 
Show 
(  
$str  6
)6 7
;7 8
return 
; 
} 
var 

tenantCode 
= 
Convert $
.$ %
ToInt32% ,
(, -

.: ;
Text; ?
)? @
;@ A
var   
tenant   
=   
TenantConfig   %
.  % &

GetTenants  & 0
(  0 1
)  1 2
.  2 3
Where  3 8
(  8 9
a  9 :
=>  ; =
a  > ?
.  ? @

TenantCode  @ J
==  K M

tenantCode  N X
)  X Y
.  Y Z
FirstOrDefault  Z h
(  h i
)  i j
;  j k
var"" 
db"" 
="" 
new"" 
WorkOfTimeDatabase"" +
(""+ ,
tenant"", 2
.""2 3
GetConnectionString""3 F
(""F G
)""G H
)""H I
;""I J
db## 
=## 
tenant## 
.## 
GetDatabase## #
(### $
)##$ %
;##% &
var%% 
files%% 
=%% 
db%% 
.%% 
GetSYS_Files%% '
(%%' (
)%%( )
;%%) *
foreach'' 
('' 
var'' 
item'' 
in''  
files''! &
)''& '
{(( 
var)) 

dataString)) 
=))  
$str))! #
;))# $
var** 
fileName** 
=** 
$str** !
;**! "
if,, 
(,, 
item,, 
.,, 
	DataTable,, "
==,,# %
$str,,& 3
),,3 4
{-- 
var.. 
data.. 
=.. 
db.. !
...! " 
GetVWCMP_CompanyById.." 6
(..6 7
item..7 ;
...; <
DataId..< B
...B C
Value..C H
)..H I
;..I J
if// 
(// 
data// 
==// 
null//  $
)//$ %
continue//& .
;//. /

dataString00 
=00  
data00! %
.00% &
code00& *
+00+ ,
$str00- 0
+001 2
data003 7
.007 8
name008 <
+00= >
$str00? B
+00C D
data00E I
.00I J

type_Title00J T
;00T U
fileName11 
=11 
$str11 (
;11( )
}22 
else33 
if33 
(33 
item33 
.33 
	DataTable33 '
==33( *
$str33+ 8
)338 9
{44 
var55 
data55 
=55 
db55 !
.55! " 
GetVWCRM_ContactById55" 6
(556 7
item557 ;
.55; <
DataId55< B
.55B C
Value55C H
)55H I
;55I J
if66 
(66 
data66 
==66 
null66  $
)66$ %
continue66& .
;66. /

dataString77 
=77  
data77! %
.77% &#
CustomerCompanyId_Title77& =
+77> ?
$str77@ C
+77D E
data77F J
.77J K
id77K M
;77M N
fileName88 
=88 
$str88 7
;887 8
}99 
else:: 
if:: 
(:: 
item:: 
.:: 
	DataTable:: '
==::( *
$str::+ =
)::= >
{;; 
var<< 
data<< 
=<< 
db<< !
.<<! "%
GetVWCRM_PresentationById<<" ;
(<<; <
item<<< @
.<<@ A
DataId<<A G
.<<G H
Value<<H M
)<<M N
;<<N O
if== 
(== 
data== 
==== 
null==  $
)==$ %
continue==& .
;==. /

dataString>> 
=>>  
data>>! %
.>>% &!
CustomerCompany_Title>>& ;
+>>< =
$str>>> A
+>>B C
data>>D H
.>>H I
id>>I K
;>>K L
fileName?? 
=?? 
$str?? 9
;??9 :
}@@ 
elseAA 
ifAA 
(AA 
itemAA 
.AA 
	DataTableAA '
==AA( *
$strAA+ C
)AAC D
{BB 
varCC 
dataCC 
=CC 
dbCC !
.CC! "+
GetVWINV_CommissionsPersonsByIdCC" A
(CCA B
itemCCB F
.CCF G
DataIdCCG M
.CCM N
ValueCCN S
)CCS T
;CCT U
ifDD 
(DD 
dataDD 
==DD 
nullDD  $
)DD$ %
continueDD& .
;DD. /

dataStringEE 
=EE  
dataEE! %
.EE% &
CommissionCodeEE& 4
+EE5 6
$strEE7 :
+EE; <
dataEE= A
.EEA B
Person_TitleEEB N
;EEN O
fileNameFF 
=FF 
$strFF 8
;FF8 9
}GG 
elseHH 
ifHH 
(HH 
itemHH 
.HH 
	DataTableHH '
==HH( *
$strHH+ H
)HHH I
{II 
varJJ 
dataJJ 
=JJ 
dbJJ !
.JJ! "0
$GetVWINV_CompanyPersonAssessmentByIdJJ" F
(JJF G
itemJJG K
.JJK L
DataIdJJL R
.JJR S
ValueJJS X
)JJX Y
;JJY Z
ifKK 
(KK 
dataKK 
==KK 
nullKK  $
)KK$ %
continueKK& .
;KK. /

dataStringLL 
=LL  
dataLL! %
.LL% &
AssessmentCodeLL& 4
+LL5 6
$strLL7 :
+LL; <
dataLL= A
.LLA B
AssessmentTypeLLB P
+LLQ R
$strLLS h
+LLi j
dataLLk o
.LLo p
Person_TitleLLp |
;LL| }
fileNameMM 
=MM 
$strMM 7
;MM7 8
}NN 
elseOO 
ifOO 
(OO 
itemOO 
.OO 
	DataTableOO '
==OO( *
$strOO+ 7
)OO7 8
{PP 
varQQ 
dataQQ 
=QQ 
dbQQ !
.QQ! "
GetVWINV_PermitByIdQQ" 5
(QQ5 6
itemQQ6 :
.QQ: ;
DataIdQQ; A
.QQA B
ValueQQB G
)QQG H
;QQH I
ifRR 
(RR 
dataRR 
==RR 
nullRR  $
)RR$ %
continueRR& .
;RR. /

dataStringSS 
=SS  
dataSS! %
.SS% &

PermitCodeSS& 0
+SS1 2
$strSS3 6
+SS7 8
dataSS9 =
.SS= >
Person_TitleSS> J
;SSJ K
fileNameTT 
=TT 
$strTT /
;TT/ 0
}UU 
elseVV 
ifVV 
(VV 
itemVV 
.VV 
	DataTableVV '
==VV( *
$strVV+ 8
)VV8 9
{WW 
varXX 
dataXX 
=XX 
dbXX !
.XX! " 
GetVWPRD_ProductByIdXX" 6
(XX6 7
itemXX7 ;
.XX; <
DataIdXX< B
.XXB C
ValueXXC H
)XXH I
;XXI J
ifYY 
(YY 
dataYY 
==YY 
nullYY  $
)YY$ %
continueYY& .
;YY. /

dataStringZZ 
=ZZ  
dataZZ! %
.ZZ% &
codeZZ& *
+ZZ+ ,
$strZZ- 0
+ZZ1 2
dataZZ3 7
.ZZ7 8
nameZZ8 <
;ZZ< =
fileName[[ 
=[[ 
$str[[ /
;[[/ 0
}\\ 
else]] 
if]] 
(]] 
item]] 
.]] 
	DataTable]] '
==]]( *
$str]]+ 8
)]]8 9
{^^ 
var__ 
data__ 
=__ 
db__ !
.__! " 
GetVWPRJ_ProjectById__" 6
(__6 7
item__7 ;
.__; <
DataId__< B
.__B C
Value__C H
)__H I
;__I J
if`` 
(`` 
data`` 
==`` 
null``  $
)``$ %
continue``& .
;``. /

dataStringaa 
=aa  
dataaa! %
.aa% &
ProjectCodeaa& 1
+aa2 3
$straa4 7
+aa8 9
dataaa: >
.aa> ?
ProjectNameaa? J
+aaK L
$straaM P
+aaQ R
dataaaS W
.aaW X
ProjectScopeaaX d
;aad e
fileNamebb 
=bb 
$strbb 0
;bb0 1
}cc 
elsedd 
ifdd 
(dd 
itemdd 
.dd 
	DataTabledd '
==dd( *
$strdd+ A
)ddA B
{ee 
varff 
dataff 
=ff 
dbff !
.ff! ")
GetVWSH_PersonCertificateByIdff" ?
(ff? @
itemff@ D
.ffD E
DataIdffE K
.ffK L
ValueffL Q
)ffQ R
;ffR S
ifgg 
(gg 
datagg 
==gg 
nullgg  $
)gg$ %
continuegg& .
;gg. /

dataStringhh 
=hh  
datahh! %
.hh% &

User_Titlehh& 0
+hh1 2
$strhh3 6
+hh7 8
datahh9 =
.hh= >!
CertificateType_Titlehh> S
+hhT U
$strhhV Y
+hhZ [
datahh\ `
.hh` a
CertificateNamehha p
;hhp q
fileNameii 
=ii 
$strii 7
;ii7 8
}jj 
elsekk 
ifkk 
(kk 
itemkk 
.kk 
	DataTablekk '
==kk( *
$strkk+ =
)kk= >
{ll 
varmm 
datamm 
=mm 
dbmm !
.mm! "%
GetVWSH_PersonSchoolsByIdmm" ;
(mm; <
itemmm< @
.mm@ A
DataIdmmA G
.mmG H
ValuemmH M
)mmM N
;mmN O
ifnn 
(nn 
datann 
==nn 
nullnn  $
)nn$ %
continuenn& .
;nn. /

dataStringoo 
=oo  
dataoo! %
.oo% &

User_Titleoo& 0
+oo1 2
$stroo3 6
+oo7 8
dataoo9 =
.oo= >
Level_Titleoo> I
+ooJ K
$strooL O
+ooP Q
dataooR V
.ooV W
School_TitleooW c
;ooc d
fileNamepp 
=pp 
$strpp 8
;pp8 9
}qq 
elserr 
ifrr 
(rr 
itemrr 
.rr 
	DataTablerr '
==rr( *
$strrr+ 4
)rr4 5
{ss 
vartt 
datatt 
=tt 
dbtt !
.tt! "
GetVWSH_UserByIdtt" 2
(tt2 3
itemtt3 7
.tt7 8
DataIdtt8 >
.tt> ?
Valuett? D
)ttD E
;ttE F
ifuu 
(uu 
datauu 
==uu 
nulluu  $
)uu$ %
continueuu& .
;uu. /

dataStringvv 
=vv  
datavv! %
.vv% &
codevv& *
+vv+ ,
$strvv- 0
+vv1 2
datavv3 7
.vv7 8
	loginnamevv8 A
+vvB C
$strvvD G
+vvH I
datavvJ N
.vvN O
FullNamevvO W
+vvX Y
$strvvZ ]
+vv^ _
datavv` d
.vvd e

+vvs t
$strvvu x
+vvy z
datavv{ 
.	vv �

Type_Title
vv� �
;
vv� �
fileNameww 
=ww 
$strww =
;ww= >
}xx 
elseyy 
continuezz 
;zz 
DownloadFile|| 
(|| 
tenant|| #
.||# $
	WebDomain||$ -
.||- .
Split||. 3
(||3 4
$char||4 7
)||7 8
[||8 9
$num||9 :
]||: ;
,||; <
item||= A
.||A B
FilePath||B J
,||J K
fileName||L T
,||T U

dataString||V `
,||` a
item||b f
.||f g

)||t u
;||u v
}}} 

MessageBox~~ 
.~~ 
Show~~ 
(~~ 
$str~~ #
)~~# $
;~~$ %
} 	
private
�� 
void
�� 
DownloadFile
�� !
(
��! "
string
��" (
baseUrl
��) 0
,
��0 1
string
��2 8
fileUrl
��9 @
,
��@ A
string
��B H
fileName
��I Q
,
��Q R
string
��S Y

dataString
��Z d
,
��d e
string
��f l
filetype
��m u
)
��u v
{
�� 	
try
�� 
{
�� 
var
�� 
destinationFile
�� #
=
��$ %
$str
��& /
+
��0 1
fileName
��2 :
;
��: ;
DestinationCheck
��  
(
��  !
destinationFile
��! 0
)
��0 1
;
��1 2
destinationFile
�� 
=
��  !
destinationFile
��" 1
+
��2 3
$str
��4 8
+
��9 :

dataString
��; E
+
��F G
$str
��H K
+
��L M
filetype
��N V
;
��V W
var
�� 
uri
�� 
=
�� 
baseUrl
�� !
+
��" #
$str
��$ '
+
��( )
fileUrl
��* 1
;
��1 2
using
�� 
(
�� 
	WebClient
��  
client
��! '
=
��( )
new
��* -
	WebClient
��. 7
(
��7 8
)
��8 9
)
��9 :
{
�� 
client
�� 
.
�� 
DownloadFile
�� '
(
��' (
new
��( +
Uri
��, /
(
��/ 0
uri
��0 3
)
��3 4
,
��4 5
destinationFile
��6 E
)
��E F
;
��F G
client
�� 
.
�� 
DownloadFileAsync
�� ,
(
��, -
new
��- 0
Uri
��1 4
(
��4 5
uri
��5 8
)
��8 9
,
��9 :
destinationFile
��; J
)
��J K
;
��K L
}
�� 
}
�� 
catch
�� 
(
�� 
	Exception
�� 
)
�� 
{
�� 
}
�� 
}
�� 	
private
�� 
static
�� 
void
�� 
DestinationCheck
�� ,
(
��, -
string
��- 3
destinationFiles
��4 D
)
��D E
{
�� 	
if
�� 
(
�� 
!
�� 
	Directory
�� 
.
�� 
Exists
�� !
(
��! "
destinationFiles
��" 2
)
��2 3
)
��3 4
	Directory
�� 
.
�� 
CreateDirectory
�� )
(
��) *
destinationFiles
��* :
)
��: ;
;
��; <
}
�� 	
}
�� 
}�� �
cD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.DownloadFilesApp\Program.cs
	namespace 	
Infoline
 
. 
DownloadFilesApp #
{ 
static		 

class		 
Program		 
{

 
[ 	
	STAThread	 
] 
static 
void 
Main 
( 
) 
{ 	
Application 
. 
EnableVisualStyles *
(* +
)+ ,
;, -
Application 
. -
!SetCompatibleTextRenderingDefault 9
(9 :
false: ?
)? @
;@ A
Application 
. 
Run 
( 
new 
Form1  %
(% &
)& '
)' (
;( )
} 	
} 
} �
sD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.DownloadFilesApp\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 


( 
$str 4
)4 5
]5 6
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
$str 6
)6 7
]7 8
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