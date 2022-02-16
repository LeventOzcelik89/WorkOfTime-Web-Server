ˆK
tD:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Applications\Infoline.WorkOfTime.LdapAgent\Program.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
	LdapAgent '
{ 
class 	
Program
 
{ 
static 
void 
Main 
( 
string 
[  
]  !
args" &
)& '
{ 	
var 
Url 
=  
ConfigurationManager *
.* +
AppSettings+ 6
.6 7
Get7 :
(: ;
$str; @
)@ A
;A B
var 
Username 
=  
ConfigurationManager /
./ 0
AppSettings0 ;
.; <
Get< ?
(? @
$str@ J
)J K
;K L
var 
Password 
=  
ConfigurationManager /
./ 0
AppSettings0 ;
.; <
Get< ?
(? @
$str@ J
)J K
;K L
var 
DistinguishedName !
=" # 
ConfigurationManager$ 8
.8 9
AppSettings9 D
.D E
GetE H
(H I
$strI \
)\ ]
;] ^
var 
Filter 
=  
ConfigurationManager -
.- .
AppSettings. 9
.9 :
Get: =
(= >
$str> F
)F G
;G H
var 
Type 
=  
ConfigurationManager +
.+ ,
AppSettings, 7
.7 8
Get8 ;
(; <
$str< B
)B C
;C D
if 
( 
Type 
== 
$str 
) 
{ 
PrincipalContext  
context! (
=) *
new+ .
PrincipalContext/ ?
(? @
ContextType@ K
.K L
DomainL R
,R S
UrlT W
,W X
nullY ]
,] ^
ContextOptions_ m
.m n
SecureSocketLayern 
|
€ 
ContextOptions
‚ 
.
 ‘
	Negotiate
‘ š
,
š ›
Username
œ ¤
,
¤ ¥
Password
¦ ®
)
® ¯
;
¯ °
try 
{ 
using   
(   
var   
searcher   '
=  ( )
new  * -
PrincipalSearcher  . ?
(  ? @
new  @ C
UserPrincipal  D Q
(  Q R
context  R Y
)  Y Z
)  Z [
)  [ \
{!! 
foreach## 
(##  !
var##! $
result##% +
in##, .
searcher##/ 7
.##7 8
FindAll##8 ?
(##? @
)##@ A
)##A B
{$$ 
var%% 
ccc%%  #
=%%$ %
result%%& ,
.%%, -
GetUnderlyingObject%%- @
(%%@ A
)%%A B
;%%B C
DirectoryEntry&& *
de&&+ -
=&&. /
result&&0 6
.&&6 7
GetUnderlyingObject&&7 J
(&&J K
)&&K L
as&&M O
DirectoryEntry&&P ^
;&&^ _
Console(( #
.((# $
	WriteLine(($ -
(((- .
JsonConvert((. 9
.((9 :
SerializeObject((: I
(((I J
de((J L
)((L M
)((M N
;((N O
Console)) #
.))# $
	WriteLine))$ -
())- .
$str)). 9
)))9 :
;)): ;
}** 
}++ 
},, 
catch-- 
(-- 
	Exception--  
ex--! #
)--# $
{.. 
Console// 
.// 
	WriteLine// %
(//% &
$str//& ;
+//< =
ex//> @
.//@ A
Message//A H
)//H I
;//I J
}00 
}11 
if55 
(55 
Type55 
==55 
$str55 
)55 
{66 
using77 
(77 
var77 
con77 
=77  
new77! $
LdapConnection77% 3
(773 4
Url774 7
)777 8
)778 9
{88 
con99 
.99 
AuthType99  
=99! "
AuthType99# +
.99+ ,
Basic99, 1
;991 2
con:: 
.:: 
SessionOptions:: &
.::& '
ProtocolVersion::' 6
=::7 8
$num::9 :
;::: ;
con;; 
.;; 
SessionOptions;; &
.;;& '
SecureSocketLayer;;' 8
=;;9 :
true;;; ?
;;;? @
try== 
{>> 
con?? 
.?? 
SessionOptions?? *
.??* +#
VerifyServerCertificate??+ B
=??C D
new??E H+
VerifyServerCertificateCallback??I h
(??h i
(??i j
conect??j p
,??p q
cer??r u
)??u v
=>??w y
true??z ~
)??~ 
;	?? €
con@@ 
.@@ 
Bind@@  
(@@  !
new@@! $
NetworkCredential@@% 6
(@@6 7
Username@@7 ?
,@@? @
Password@@A I
,@@I J
$str@@K W
)@@W X
)@@X Y
;@@Y Z
ConsoleDD 
.DD  
	WriteLineDD  )
(DD) *
$strDD* 5
)DD5 6
;DD6 7
}EE 
catchFF 
(FF 
	ExceptionFF $
exFF% '
)FF' (
{GG 
ConsoleHH 
.HH  
	WriteLineHH  )
(HH) *
$strHH* ?
+HH@ A
exHHB D
.HHD E
MessageHHE L
)HHL M
;HHM N
}II 
}JJ 
}KK 
ifOO 
(OO 
TypeOO 
==OO 
$strOO 
)OO 
{PP 
DirectoryEntryRR 
ldapConnectionRR -
=RR. /
newRR0 3
DirectoryEntryRR4 B
(RRB C
)RRC D
;RRD E
ldapConnectionSS 
.SS 
PathSS #
=SS$ %
$strSS& /
+SS0 1
UrlSS2 5
+SS6 7
DistinguishedNameSS8 I
;SSI J
ldapConnectionTT 
.TT 
UsernameTT '
=TT( )
UsernameTT* 2
;TT2 3
ldapConnectionUU 
.UU 
PasswordUU '
=UU( )
PasswordUU* 2
;UU2 3
DirectorySearcherVV !
searchVV" (
=VV) *
newVV+ .
DirectorySearcherVV/ @
(VV@ A
ldapConnectionVVA O
)VVO P
;VVP Q
ifWW 
(WW 
FilterWW 
!=WW 
nullWW "
)WW" #
{XX 
searchYY 
.YY 
FilterYY !
=YY" #
$strYY$ F
;YYF G
}ZZ "
SearchResultCollection[[ &
src[[' *
=[[+ ,
search[[- 3
.[[3 4
FindAll[[4 ;
([[; <
)[[< =
;[[= >
var\\ 
list\\ 
=\\ 
new\\ 
List\\ #
<\\# $

Dictionary\\$ .
<\\. /
string\\/ 5
,\\5 6
object\\7 =
>\\= >
>\\> ?
(\\? @
)\\@ A
;\\A B
foreach]] 
(]] 
SearchResult]] %
sr]]& (
in]]) +
src]], /
)]]/ 0
{^^ 
try__ 
{`` 
usingaa 
(aa 
DirectoryEntryaa -
entryaa. 3
=aa4 5
newaa6 9
DirectoryEntryaa: H
(aaH I
sraaI K
.aaK L
PathaaL P
)aaP Q
)aaQ R
{bb 
varcc 
usercc  $
=cc% &
newcc' *

Dictionarycc+ 5
<cc5 6
stringcc6 <
,cc< =
objectcc> D
>ccD E
(ccE F
)ccF G
;ccG H
ifdd 
(dd  
userdd  $
!=dd% '
nulldd( ,
&&dd- /
entrydd0 5
.dd5 6

Propertiesdd6 @
!=ddA C
nullddD H
)ddH I
{ee 
foreachff  '
(ff( )
stringff) /
propff0 4
inff5 7
entryff8 =
.ff= >

Propertiesff> H
.ffH I
PropertyNamesffI V
)ffV W
{gg  !
tryhh$ '
{ii$ %
userjj( ,
.jj, -
Addjj- 0
(jj0 1
propjj1 5
,jj5 6
entryjj7 <
.jj< =

Propertiesjj= G
[jjG H
propjjH L
]jjL M
[jjM N
$numjjN O
]jjO P
)jjP Q
;jjQ R
}kk$ %
catchll$ )
{ll* +
}ll, -
}mm  !
}nn 
listoo  
.oo  !
Addoo! $
(oo$ %
useroo% )
)oo) *
;oo* +
}pp 
}qq 
catchrr 
{rr 
}rr 
}ss 
Consolett 
.tt 
	WriteLinett !
(tt! "
JsonConverttt" -
.tt- .
SerializeObjecttt. =
(tt= >
listtt> B
)ttB C
)ttC D
;ttD E
}vv 
Consolexx 
.xx 
ReadLinexx 
(xx 
)xx 
;xx 
}yy 	
}zz 
}{{ °
„D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Applications\Infoline.WorkOfTime.LdapAgent\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str 8
)8 9
]9 :
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
$str :
): ;
]; <
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