�
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\APM_Activity.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
APM_Activity		 %
:		& '

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public"" 
short"" 
?"" 
generalType"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public&& 
	IGeometry&& 
location&& "
{&&# $
get&&% (
;&&( )
set&&* -
;&&- .
}&&. /
public** 
short** 
?** 
communicationType** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public.. 
short.. 
?.. 
notification.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}... /
}// 
}00 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\APM_ActivityAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
APM_ActivityAction		 +
:		, -

{

 
public 
Guid 
? 

activityId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\APM_ActivityRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		  
APM_ActivityRelation		 -
:		. /

{

 
public 
Guid 
? 

activityId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\APM_ActivityUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
APM_ActivityUser		 )
:		* +

{

 
public 
Guid 
? 

activityId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_Company.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
CMP_Company		 $
:		% &

{

 
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
mersisNo 
{  
get! $
;$ %
set& )
;) *
}* +
public"" 
string"" 
email"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""' (
public&& 
string&& 
phone&& 
{&& 
get&& !
;&&! "
set&&# &
;&&& '
}&&' (
public** 
string** 
fax** 
{** 
get** 
;**  
set**! $
;**$ %
}**% &
public.. 
short.. 
?.. 
taxType.. 
{.. 
get..  #
;..# $
set..% (
;..( )
}..) *
public22 
string22 
	taxOffice22 
{22  !
get22" %
;22% &
set22' *
;22* +
}22+ ,
public66 
string66 
	taxNumber66 
{66  !
get66" %
;66% &
set66' *
;66* +
}66+ ,
public:: 
	IGeometry:: 
location:: "
{::# $
get::% (
;::( )
set::* -
;::- .
}::. /
public>> 
string>> 
invoiceAddress>> $
{>>% &
get>>' *
;>>* +
set>>, /
;>>/ 0
}>>0 1
publicBB 
GuidBB 
?BB $
invoiceAddressLocationIdBB -
{BB. /
getBB0 3
;BB3 4
setBB5 8
;BB8 9
}BB9 :
publicFF 
stringFF 
openAddressFF !
{FF" #
getFF$ '
;FF' (
setFF) ,
;FF, -
}FF- .
publicJJ 
GuidJJ 
?JJ !
openAddressLocationIdJJ *
{JJ+ ,
getJJ- 0
;JJ0 1
setJJ2 5
;JJ5 6
}JJ6 7
publicNN 
stringNN 
commercialTitleNN %
{NN& '
getNN( +
;NN+ ,
setNN- 0
;NN0 1
}NN1 2
publicOO 
shortOO 
?OO 
isActiveOO 
{OO  
getOO! $
;OO$ %
setOO& )
;OO) *
}OO* +
publicPP 
stringPP 
descriptionPP !
{PP" #
getPP$ '
;PP' (
setPP) ,
;PP, -
}PP- .
}QQ 
}RR �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_CompanyCarKilometer.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
CMP_CompanyCarKilometer 0
:1 2

{ 
public 
DateTime 
? 
	entryDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
double		 
?		 
	kilometer		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
	IGeometry

 
location

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
string 
image 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
companyCarId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
Guid
?
commissionId
{
get
;
set
;
}
} 
} �(
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_CompanyCars.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_CompanyCars (
:) *

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
companyStorageId		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
string

 
name

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
plate 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
brand 
{ 
get !
;! "
set# &
;& '
}' (
public
string
model
{
get
;
set
;
}
public 
bool 
? 
isHire 
{ 
get !
;! "
set# &
;& '
}' (
public 
DateTime 
? 
contractStartDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
DateTime 
? 
contractEndDate (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string '
vehicleIdentificationNumber 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
string #
vehicleTransitionNumber -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
double 
? 
consumptionFuel &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 
insuranceStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
insuranceEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? %
trafficInsuranceStartDate 2
{3 4
get5 8
;8 9
set: =
;= >
}> ?
public 
DateTime 
? #
trafficInsuranceEndDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
string 
	tradeName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
engineNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
trafficReleaseDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
registrationDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
userName 
{  
get! $
;$ %
set& )
;) *
}* +
public   
int   
?   
fuelType   
{   
get   "
;  " #
set  $ '
;  ' (
}  ( )
public!! 
int!! 
?!! 
gearType!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!( )
public"" 
string"" 
policyNumber"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}"". /
public## 
Guid## 
?## 
responsiblePersonId## (
{##) *
get##+ .
;##. /
set##0 3
;##3 4
}##4 5
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_CompanyFileSelector.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
CMP_CompanyFileSelector 0
:1 2

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
fileGroupModule %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 

customerId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_CompanyRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_CompanyRelation ,
:- .

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 

customerId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_CompanyType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_CompanyType (
:) *

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
typesId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
}

 
} �2
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_Invoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_Invoice $
:% &

{ 
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
serialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
	rowNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

supplierId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public## 
Guid## 
?## 

customerId## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
public'' 
Guid'' 
?'' 

currencyId'' 
{''  !
get''" %
;''% &
set''' *
;''* +
}''+ ,
public++ 
DateTime++ 
?++ 
	issueDate++ "
{++# $
get++% (
;++( )
set++* -
;++- .
}++. /
public// 
DateTime// 
?// 

expiryDate// #
{//$ %
get//& )
;//) *
set//+ .
;//. /
}/// 0
public33 
short33 
?33 
stopaj33 
{33 
get33 "
;33" #
set33$ '
;33' (
}33( )
public77 
double77 
?77 
discount77 
{77  !
get77" %
;77% &
set77' *
;77* +
}77+ ,
public;; 
short;; 
?;; 
discountType;; "
{;;# $
get;;% (
;;;( )
set;;* -
;;;- .
};;. /
public?? 
double?? 
??? 
tevkifat?? 
{??  !
get??" %
;??% &
set??' *
;??* +
}??+ ,
publicCC 
DateTimeCC 
?CC 
sendingDateCC $
{CC% &
getCC' *
;CC* +
setCC, /
;CC/ 0
}CC0 1
publicGG 
shortGG 
?GG 
statusGG 
{GG 
getGG "
;GG" #
setGG$ '
;GG' (
}GG( )
publicKK 
stringKK 
tenderConditionsKK &
{KK' (
getKK) ,
;KK, -
setKK. 1
;KK1 2
}KK2 3
publicOO 
shortOO 
?OO 
paymentTypeOO !
{OO" #
getOO$ '
;OO' (
setOO) ,
;OO, -
}OO- .
publicSS 
shortSS 
?SS 
installmentCountSS &
{SS' (
getSS) ,
;SS, -
setSS. 1
;SS1 2
}SS2 3
publicWW 
doubleWW 
?WW 
rateExchangeWW #
{WW$ %
getWW& )
;WW) *
setWW+ .
;WW. /
}WW/ 0
public[[ 
string[[ 
customerTaxNumber[[ '
{[[( )
get[[* -
;[[- .
set[[/ 2
;[[2 3
}[[3 4
public__ 
string__ 
customerTaxOffice__ '
{__( )
get__* -
;__- .
set__/ 2
;__2 3
}__3 4
publiccc 
stringcc 
customerAdresscc $
{cc% &
getcc' *
;cc* +
setcc, /
;cc/ 0
}cc0 1
publicgg 
stringgg 
supplierTaxNumbergg '
{gg( )
getgg* -
;gg- .
setgg/ 2
;gg2 3
}gg3 4
publickk 
stringkk 
supplierTaxOfficekk '
{kk( )
getkk* -
;kk- .
setkk/ 2
;kk2 3
}kk3 4
publicoo 
stringoo 
supplierAdressoo $
{oo% &
getoo' *
;oo* +
setoo, /
;oo/ 0
}oo0 1
publicss 
stringss 

{ss$ %
getss& )
;ss) *
setss+ .
;ss. /
}ss/ 0
publicww 
stringww 

{ww$ %
getww& )
;ww) *
setww+ .
;ww. /
}ww/ 0
public{{ 
Guid{{ 
?{{ 
customerStorageId{{ &
{{{' (
get{{) ,
;{{, -
set{{. 1
;{{1 2
}{{2 3
public 
Guid 
? 
supplierStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
�� 
DateTime
�� 
?
�� 
validityDate
�� %
{
��& '
get
��( +
;
��+ ,
set
��- 0
;
��0 1
}
��1 2
public
�� 
double
�� 
?
�� 
discountPercent
�� &
{
��' (
get
��) ,
;
��, -
set
��. 1
;
��1 2
}
��2 3
public
�� 
Guid
�� 
?
�� '
invoiceDocumentTemplateId
�� .
{
��/ 0
get
��1 4
;
��4 5
set
��6 9
;
��9 :
}
��: ;
public
�� 
Guid
�� 
?
�� 
taskId
�� 
{
�� 
get
�� !
;
��! "
set
��# &
;
��& '
}
��' (
public
�� 
Guid
�� 
?
�� 
pid
�� 
{
�� 
get
�� 
;
�� 
set
��  #
;
��# $
}
��$ %
}
�� 
}�� �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_InvoiceAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_InvoiceAction *
:+ ,

{ 
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
transformInvoiceId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_InvoiceConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
CMP_InvoiceConfirmation 0
:1 2

{ 
public 
Guid 
? 
	advanceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_InvoiceDocumentTemplate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
CMP_InvoiceDocumentTemplate 4
:5 6

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
cover		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
tenderConditions &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
short 
? "
templateVisibleAllUser ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_InvoiceItem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CMP_InvoiceItem (
:) *

{ 
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
KDV 
{ 
get  
;  !
set" %
;% &
}& '
public## 
short## 
?## 
KDVType## 
{## 
get##  #
;### $
set##% (
;##( )
}##) *
public'' 
double'' 
?'' 
OIV'' 
{'' 
get''  
;''  !
set''" %
;''% &
}''& '
public(( 
short(( 
?(( 
OIVType(( 
{(( 
get((  #
;((# $
set((% (
;((( )
}(() *
public)) 
double)) 
?)) 
OTV)) 
{)) 
get))  
;))  !
set))" %
;))% &
}))& '
public** 
short** 
?** 
OTVType** 
{** 
get**  #
;**# $
set**% (
;**( )
}**) *
public.. 
double.. 
?.. 
discount.. 
{..  !
get.." %
;..% &
set..' *
;..* +
}..+ ,
public22 
short22 
?22 
discountType22 "
{22# $
get22% (
;22( )
set22* -
;22- .
}22. /
public66 
string66 
description66 !
{66" #
get66$ '
;66' (
set66) ,
;66, -
}66- .
public:: 
int:: 
?:: 
	itemOrder:: 
{:: 
get::  #
;::# $
set::% (
;::( )
}::) *
public;; 
double;; 
?;; 
alternativeQuantity;; *
{;;+ ,
get;;- 0
;;;0 1
set;;2 5
;;;5 6
};;6 7
public<< 
Guid<< 
?<< 
alternativeUnitId<< &
{<<' (
get<<) ,
;<<, -
set<<. 1
;<<1 2
}<<2 3
}== 
}>> �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_InvoiceTransform.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
CMP_InvoiceTransform -
:. /

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
invoiceIdTo		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_Sector.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 

CMP_Sector		 #
:		$ %

{

 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
sectorId 
{ 
get  #
;# $
set% (
;( )
}) *
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_Storage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
CMP_Storage		 $
:		% &

{

 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
address 
{ 
get  #
;# $
set% (
;( )
}) *
public"" 
Guid"" 
?"" 

locationId"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public&& 
Guid&& 
?&& 
supervisorId&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&- .
public'' 
string'' 
phone'' 
{'' 
get'' !
;''! "
set''# &
;''& '
}''' (
public(( 
string(( 
fax(( 
{(( 
get(( 
;((  
set((! $
;(($ %
}((% &
public)) 
Guid)) 
?)) 
pid)) 
{)) 
get)) 
;)) 
set))  #
;))# $
}))$ %
public** 
string** 
email** 
{** 
get** !
;**! "
set**# &
;**& '
}**' (
public++ 
string++ 
postCode++ 
{++  
get++! $
;++$ %
set++& )
;++) *
}++* +
public,, 
short,, 
?,, 
locationType,, "
{,,# $
get,,% (
;,,( )
set,,* -
;,,- .
},,. /
}-- 
}.. �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_StorageSection.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
CMP_StorageSection		 +
:		, -

{

 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	storageId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
} 
}   �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_TenderTransaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
CMP_TenderTransaction .
:/ 0

{ 
public 
Guid 
? 
tenderId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 

{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CMP_Types.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	CMP_Types "
:# $

{ 
public 
string 
typeName 
{  
get! $
;$ %
set& )
;) *
}* +
}		 
}

 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_Contact.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_Contact $
:% &

{ 
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
DateTime		 
?		 
ContactStartDate		 )
{		* +
get		, /
;		/ 0
set		1 4
;		4 5
}		5 6
public

 
DateTime

 
?

 
ContactEndDate

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
ContactType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
int
?

{
get
;
set
;
}
public 
Guid 
? 
PresentationStageId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_ContactUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_ContactUser (
:) *

{ 
public 
Guid 
? 
	ContactId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
UserId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
int

 
?

 
UserType

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_ManagerStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_ManagerStage )
:* +

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
Code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
Description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
IsSalesCompleted %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_MonthlyTarget.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_MonthlyTarget *
:+ ,

{ 
public 
DateTime 
? 
CPeriod  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
TargetGroupType #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
ProductGroupId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 
TargetPoint 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
BonusPercentage #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public## 
bool## 
?## 
IsFocus## 
{## 
get## "
;##" #
set##$ '
;##' (
}##( )
public'' 
bool'' 
?'' 
IsMultipleFocus'' $
{''% &
get''' *
;''* +
set'', /
;''/ 0
}''0 1
public++ 
Guid++ 
?++ 
RowId++ 
{++ 
get++  
;++  !
set++" %
;++% &
}++& '
public// 
Guid// 
?// 
GroupId// 
{// 
get// "
;//" #
set//$ '
;//' (
}//( )
}00 
}11 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_MonthlyTargetPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
CRM_MonthlyTargetPerson 0
:1 2

{ 
public 
DateTime 
? 
CPeriod  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
TargetUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
ProductGroupId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 
TargetPoint 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
BonusPercentage #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public## 
bool## 
?## 
IsFocus## 
{## 
get## "
;##" #
set##$ '
;##' (
}##( )
public'' 
bool'' 
?'' 
IsMultipleFocus'' $
{''% &
get''' *
;''* +
set'', /
;''/ 0
}''0 1
public++ 
Guid++ 
?++ 
RowId++ 
{++ 
get++  
;++  !
set++" %
;++% &
}++& '
public// 
Guid// 
?// 
GroupId// 
{// 
get// "
;//" #
set//$ '
;//' (
}//( )
}00 
}11 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_OpponentCompany.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_OpponentCompany ,
:- .

{ 
public 
string 
CompanyName !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_PerformanceMultiplier.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
CRM_PerformanceMultiplier 2
:3 4

{ 
public 
Guid 
? 
GroupId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
TargetGroupType #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
ProductGroupId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public   
int   
?   

{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public$$ 
int$$ 
?$$ 
Point$$ 
{$$ 
get$$ 
;$$  
set$$! $
;$$$ %
}$$% &
public(( 
bool(( 
?(( 
IsFocus(( 
{(( 
get(( "
;((" #
set(($ '
;((' (
}((( )
})) 
}** �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_Presentation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
CRM_Presentation )
:* +

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
Guid		 
?		 

{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
Guid

 
?

 
ChannelCompanyId

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
Guid 
? 
CustomerCompanyId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
PresentationStageId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
DateTime 
? 
CommitmentEndDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
double 
? !
OpponentInvoiceAmount ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
double 
? 
CompletionRate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
DateTime   
?   #
EstimatedCompletionDate   0
{  1 2
get  3 6
;  6 7
set  8 ;
;  ; <
}  < =
public$$ 
long$$ 
?$$ 
Budget$$ 
{$$ 
get$$ !
;$$! "
set$$# &
;$$& '
}$$' (
public(( 
int(( 
?(( 

{((" #
get(($ '
;((' (
set(() ,
;((, -
}((- .
public)) 
bool)) 
?)) 

hasContact)) 
{))  !
get))" %
;))% &
set))' *
;))* +
}))+ ,
public** 
short** 
?** 
PlaceofArrival** $
{**% &
get**' *
;*** +
set**, /
;**/ 0
}**0 1
}++ 
},, �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_PresentationAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
CRM_PresentationAction /
:0 1

{ 
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
presentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
	contactId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_PresentationInvoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
CRM_PresentationInvoice 0
:1 2

{ 
public 
Guid 
? 
presentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	invoiceId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
type

 
{

 
get

 
;

 
set

  #
;

# $
}

$ %
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_PresentationOpponentCompany.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class +
CRM_PresentationOpponentCompany 8
:9 :

{ 
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
OpponentCompanyId		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		2 3
}

 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\CRM_PresentationProducts.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
CRM_PresentationProducts 1
:2 3

{ 
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	ProductId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public
int
?
Amount
{
get
;
set
;
}
public 
bool 
? 
IsNew 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_Document.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
DOC_Document %
:& '

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
title		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
subject

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
responsibleUserId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
string

{
get
;
set
;
}
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentFlow.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
DOC_DocumentFlow )
:* +

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
organizationUnitId

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
short 
? 
order 
{ 
get !
;! "
set# &
;& '
}' (
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
DOC_DocumentRelation -
:. /

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
documentRelationId		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
}

 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentRevisionRequest.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
DOC_DocumentRevisionRequest 4
:5 6

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
revisionUserId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
revisionNumber

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
public 
string 
revisionContent %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentRevisionRequestConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 3
'DOC_DocumentRevisionRequestConfirmation @
:A B

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
revisionRequestId		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		2 3
public

 
Guid

 
?

 


 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public
string
description
{
get
;
set
;
}
public 
short 
? 
order 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentScope.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
DOC_DocumentScope *
:+ ,

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
organizationUnitId		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\DOC_DocumentVersion.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
DOC_DocumentVersion ,
:- .

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
string		 

{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
content

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
bool 
? 
isActive 
{ 
get  #
;# $
set% (
;( )
}) *
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_Task.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_Task		 !
:		" #

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	fixtureId 
{  
get! $
;$ %
set& )
;) *
}* +
public"" 
	IGeometry"" 
location"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}"". /
public## 
short## 
?## 
priority## 
{##  
get##! $
;##$ %
set##& )
;##) *
}##* +
public'' 
Guid'' 
?'' 

customerId'' 
{''  !
get''" %
;''% &
set''' *
;''* +
}''+ ,
public++ 
DateTime++ 
?++ 
dueDate++  
{++! "
get++# &
;++& '
set++( +
;+++ ,
}++, -
public,, 
Guid,, 
?,, 
customerStorageId,, &
{,,' (
get,,) ,
;,,, -
set,,. 1
;,,1 2
},,2 3
public-- 
DateTime-- 
?-- 

{--' (
get--) ,
;--, -
set--. 1
;--1 2
}--2 3
public.. 
DateTime.. 
?.. 
notificationDate.. )
{..* +
get.., /
;../ 0
set..1 4
;..4 5
}..5 6
public// 
Guid// 
?// 
companyCarId// !
{//" #
get//$ '
;//' (
set//) ,
;//, -
}//- .
public33 
Guid33 
?33 

taskPlanId33 
{33  !
get33" %
;33% &
set33' *
;33* +
}33+ ,
public77 
Guid77 
?77 
taskTemplateId77 #
{77$ %
get77& )
;77) *
set77+ .
;77. /
}77/ 0
public88 
short88 
?88 
	planLater88 
{88  !
get88" %
;88% &
set88' *
;88* +
}88+ ,
}99 
}:: �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskAuthority.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskAuthority *
:+ ,

{ 
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 

customerId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
Guid

 
?

 
userId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskFollowUpUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
FTM_TaskFollowUpUser -
:. /

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_TaskForm		 %
:		& '

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
json 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
isActive 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskFormRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		  
FTM_TaskFormRelation		 -
:		. /

{

 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
formId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
companyStorageId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskFormResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_TaskFormResult		 +
:		, -

{

 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
taskOperationId $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
formId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 

jsonResult  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskOperation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_TaskOperation		 *
:		+ ,

{

 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
status 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public"" 
Guid"" 
?"" 
	fixtureId"" 
{""  
get""! $
;""$ %
set""& )
;"") *
}""* +
public&& 
double&& 
?&& 
battery&& 
{&&  
get&&! $
;&&$ %
set&&& )
;&&) *
}&&* +
public'' 
short'' 
?'' 
subject'' 
{'' 
get''  #
;''# $
set''% (
;''( )
}'') *
public(( 
Guid(( 
?(( 
dataId(( 
{(( 
get(( !
;((! "
set((# &
;((& '
}((' (
})) 
}** �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskPlan %
:& '

{ 
public 
bool 
? 
enabled 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
frequencyStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
frequencyEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
int 
? 
	frequency 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
frequencyInterval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
int   
?   
taskCreationTime   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  0 1
public$$ 
string$$ 
times$$ 
{$$ 
get$$ !
;$$! "
set$$# &
;$$& '
}$$' (
public(( 
string(( 
weekDays(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
public,, 
string,, 
	monthDays,, 
{,,  !
get,," %
;,,% &
set,,' *
;,,* +
},,+ ,
public00 
Guid00 
?00 

templateId00 
{00  !
get00" %
;00% &
set00' *
;00* +
}00+ ,
}11 
}22 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskSubject.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskSubject (
:) *

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
Guid		 
?		 
pid		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskSubjectType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskSubjectType ,
:- .

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	subjectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskSubType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskSubType (
:) *

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
}		 
}

 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskTemplate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_TaskTemplate		 )
:		* +

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public   
Guid   
?   

customerId   
{    !
get  " %
;  % &
set  ' *
;  * +
}  + ,
public!! 
Guid!! 
?!! 
customerStorageId!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
Guid"" 
?"" 
companyCarId"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public## 
int## 
?## 
estimatedTaskMinute## '
{##( )
get##* -
;##- .
set##/ 2
;##2 3
}##3 4
public$$ 
string$$ 
description$$ !
{$$" #
get$$$ '
;$$' (
set$$) ,
;$$, -
}$$- .
public(( 
bool(( 
?(( 

{((# $
get((% (
;((( )
set((* -
;((- .
}((. /
public)) 
bool)) 
?)) 
sendMail)) 
{)) 
get))  #
;))# $
set))% (
;))( )
}))) *
}** 
}++ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskTemplateInventories.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
FTM_TaskTemplateInventories 4
:5 6

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
inventoryId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskTemplateSubjectType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
FTM_TaskTemplateSubjectType 4
:5 6

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	subjectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskTemplateUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		  
FTM_TaskTemplateUser		 -
:		. /

{

 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 

verifyCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskTemplateUserHelper.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
FTM_TaskTemplateUserHelper 3
:4 5

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
FTM_TaskUser		 %
:		& '

{

 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 

verifyCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FTM_TaskUserHelper.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
FTM_TaskUserHelper +
:, -

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\FVR_Favorites.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
Area		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 

Controller

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
Method 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
Count 
{ 
get 
;  
set! $
;$ %
}% &
public
bool
?
Status
{
get
;
set
;
}
public 
string 
Action 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_Issue.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	HDM_Issue		 "
:		# $

{

 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
expiryMinute  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
mainId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_IssueUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 

:		' (

{

 
public 
Guid 
? 
issueId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_Suggestion.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
HDM_Suggestion		 '
:		( )

{

 
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
issueId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
assignUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_Ticket.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 

HDM_Ticket		 #
:		$ %

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 

evaluation  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
channel 
{ 
get  #
;# $
set% (
;( )
}) *
public"" 
Guid"" 
?"" 
issueId"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}""( )
public&& 
Guid&& 
?&& 
suggestionId&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&- .
public** 
string** 
title** 
{** 
get** !
;**! "
set**# &
;**& '
}**' (
public.. 
string.. 
content.. 
{.. 
get..  #
;..# $
set..% (
;..( )
}..) *
public22 
Guid22 
?22 
requesterId22  
{22! "
get22# &
;22& '
set22( +
;22+ ,
}22, -
public66 
Guid66 
?66 
assignUserId66 !
{66" #
get66$ '
;66' (
set66) ,
;66, -
}66- .
public77 
DateTime77 
?77 
dueDate77  
{77! "
get77# &
;77& '
set77( +
;77+ ,
}77, -
public88 
string88 
evaluateDescription88 )
{88* +
get88, /
;88/ 0
set881 4
;884 5
}885 6
}99 
}:: �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_TicketAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
HDM_TicketAction		 )
:		* +

{

 
public 
Guid 
? 
ticketId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
ticketStatus "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_TicketMessage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
HDM_TicketMessage		 *
:		+ ,

{

 
public 
Guid 
? 
ticketId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
cc 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
bcc 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 
mailto 
{ 
get "
;" #
set$ '
;' (
}( )
}   
}!! �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HDM_TicketRequester.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
HDM_TicketRequester		 ,
:		- .

{

 
public 
string 
fullName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
company 
{ 
get  #
;# $
set% (
;( )
}) *
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_Keywords.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_Keywords $
:% &

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
}		 
}

 �%
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_Person.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	HR_Person "
:# $

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
SurName		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
IdentifyNumber

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
public 
string 
Phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}' (
public
Guid
?

LocationId
{
get
;
set
;
}
public 
DateTime 
? 
Birthday !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
ArrivalType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
ReferenceId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
	Education 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
MilitaryStatus "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
Language 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
LanguageType  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
RetirementDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
Adress 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

SchoolName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Branch 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
SalaryRangeMax "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string  
MilitaryExemptDetail *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
int 
? 
Gender 
{ 
get  
;  !
set" %
;% &
}& '
public   
DateTime   
?   
MilitaryDoneDate   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  5 6
public!! 
int!! 
?!! 

{!!" #
get!!$ '
;!!' (
set!!) ,
;!!, -
}!!- .
}"" 
}## �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_PersonKeywords.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_PersonKeywords *
:+ ,

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrKeywordsId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_PersonPosition.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_PersonPosition *
:+ ,

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrPositionId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_Plan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_Plan  
:! "

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
DateTime		 
?		 
PlanDate		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
int

 
?

 
Result

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
PdsEvulateFormId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
Guid
?
PdsEvulateFormResultId
{
get
;
set
;
}
public 
bool 
? 
mailSend 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
StaffNeedsId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
contactLink !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_PlanPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
Guid 
? 
HrPlanId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
UserId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_Position.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_Position $
:% &

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
}		 
}

 �#
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeeds.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
Guid 
? 
RequestApprovedId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
DateTime		 
?		 
RequestDate		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		0 1
public

 
Guid

 
?

 

LocationId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? 
ArrivalType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
	Education 
{ 
get  #
;# $
set% (
;( )
}) *
public
int
?
MilitaryStatus
{
get
;
set
;
}
public 
int 
? 
Language 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
LanguageType  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
ReferenceId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
?  
ReasonForStaffDemand (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
int 
? 
EmploymentStatus $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 
Gender 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
RetirementDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
NeedCode 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
SalaryRangeMax "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Comment 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
}   
}!! �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeedsKeywords.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
HR_StaffNeedsKeywords .
:/ 0

{ 
public 
Guid 
? 
HrKeywordsId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeedsPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_StaffNeedsPerson ,
:- .

{ 
public 
Guid 
? 
HrStaffNeedsId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 

HrPersonId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 
?

 
status

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public
int
?
SalaryRangeMax
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeedsPosition.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
HR_StaffNeedsPosition .
:/ 0

{ 
public 
Guid 
? 
HrPositionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeedsRequester.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
HR_StaffNeedsRequester /
:0 1

{ 
public 
Guid 
? 
StaffNeedId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
	RequestId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffNeedsStatus.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
HR_StaffNeedsStatus		 ,
:		- .

{

 
public 
Guid 
? 
staffNeedId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\HR_StaffStatus.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
HR_StaffStatus '
:( )

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
int

 
?

 
Status

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\InfolineTable.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

class 

{ 
public 

( 
) 
{ 	
id		 
=		 
Guid		 
.		 
NewGuid		 
(		 
)		 
;		  
}

 	
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}# $
public 
DateTime 
? 
created  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
DateTime 
? 
changed  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 
Guid 
? 
	changedby 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
Guid 
? 
	createdby 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
}   �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\Intranet1000.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
Intranet1000 %
:& '

{ 
public 
string 
COLUMN_NAME !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
IS_NULLABLE		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
	DATA_TYPE

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? $
CHARACTER_MAXIMUM_LENGTH ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
byte 
? 
NUMERIC_PRECISION &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
int
?

{
get
;
set
;
}
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CommisionConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
INV_CommisionConfirmation 2
:3 4

{ 
public 
Guid 
? 
commissionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
Guid 
? 
ruleStageId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
ruleId 
{ 
get !
;! "
set# &
;& '
}' (
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_Commissions.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
INV_Commissions (
:) *

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public
int
?
Information
{
get
;
set
;
}
public 
string 
InformationDetail '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
int 
? 
TravelInformation %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string #
TravelInformationDetail -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public## 
int## 
?## 

{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public$$ 
string$$ 
CommissionCode$$ $
{$$% &
get$$' *
;$$* +
set$$, /
;$$/ 0
}$$0 1
public%% 
string%% 

{%%$ %
get%%& )
;%%) *
set%%+ .
;%%. /
}%%/ 0
public&& 
string&& 
ToAdress&& 
{&&  
get&&! $
;&&$ %
set&&& )
;&&) *
}&&* +
public'' 
string'' 
CommissionSubject'' '
{''( )
get''* -
;''- .
set''/ 2
;''2 3
}''3 4
public(( 
double(( 
?(( 
	TotalDays((  
{((! "
get((# &
;((& '
set((( +
;((+ ,
}((, -
public)) 
double)) 
?)) 

TotalHours)) !
{))" #
get))$ '
;))' (
set))) ,
;)), -
}))- .
public** 
int** 
?** #
RequestForAccommodation** +
{**, -
get**. 1
;**1 2
set**3 6
;**6 7
}**7 8
public++ 
Guid++ 
?++ 
IdCompanyCar++ !
{++" #
get++$ '
;++' (
set++) ,
;++, -
}++- .
public,, 
string,, 
VehiclePlate,, "
{,,# $
get,,% (
;,,( )
set,,* -
;,,- .
},,. /
public-- 
double-- 
?-- 
VehicleKilometer-- '
{--( )
get--* -
;--- .
set--/ 2
;--2 3
}--3 4
}.. 
}// �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CommissionsInformation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
INV_CommissionsInformation 3
:4 5

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
Guid

 
?

 
to

 
{

 
get

 
;

 
set

 "
;

" #
}

# $
public 
Guid 
? 
from 
{ 
get 
;  
set! $
;$ %
}% &
public 
DateTime 
? 

{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
DateTime
?

returnDate
{
get
;
set
;
}
public 
string 
	hotelName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
hotelEntryDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
DateTime 
? 
hotelLeaveDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
rentalCarPlace #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
rentalCarStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
rentalCarEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
Guid 
? 
	shuttleTo 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
shuttleFrom  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
DateTime 
?  
shuttleDepartureDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
? 
shuttleReturnDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
note 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
	hotelNote 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
shuttleNote !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CommissionsPersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
INV_CommissionsPersons /
:0 1

{ 
public 
bool 
? 
IsOwner 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
Guid		 
?		 
IdCommisions		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
Guid

 
?

 
IdUser

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CommissionsProjects.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
INV_CommissionsProjects 0
:1 2

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	IdProject 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 

Percentile !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyDepartments.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
INV_CompanyDepartments /
:0 1

{ 
public 
Guid 
? 
PID 
{ 
get 
; 
set  #
;# $
}$ %
public		 
string		 
Name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
	ProjectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
INV_CompanyPerson		 *
:		+ ,

{

 
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
DateTime 
? 
JobStartDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? 

JobEndDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 

JobLeaving 
{  
get! $
;$ %
set& )
;) *
}* +
public"" 
string"" !
JobLeavingDescription"" +
{"", -
get"". 1
;""1 2
set""3 6
;""6 7
}""7 8
public&& 
string&& 
Title&& 
{&& 
get&& !
;&&! "
set&&# &
;&&& '
}&&' (
public'' 
int'' 
?'' 
Level'' 
{'' 
get'' 
;''  
set''! $
;''$ %
}''% &
}(( 
})) �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonAssessment.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
INV_CompanyPersonAssessment 4
:5 6

{ 
public 
string 
AssessmentCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public		 
int		 
?		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
DateTime

 
?

 
AssessmentDate

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
short 
? 
AssessmentType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public
DateTime
?
Manager2ApprovalDate
{
get
;
set
;
}
public 
string #
Manager1ApprovalComment -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string #
Manager2ApprovalComment -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
Manager2Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? 
IkApprovalDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
IKApprovalComment '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? "
GeneralManagerApproval +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string )
GeneralManagerApprovalComment 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public 
DateTime 
? &
GeneralManagerApprovalDate 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonAssessmentRating.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class -
!INV_CompanyPersonAssessmentRating :
:; <

{ 
public 
Guid 
? 
assessmentId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
question		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
answer

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonAvailability.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
INV_CompanyPersonAvailability 6
:7 8

{ 
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
Guid 
? 
	IdProject 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
rate 
{ 
get !
;! "
set# &
;& '
}' (
public
bool
?
isSalary
{
get
;
set
;
}
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonCalendar.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
INV_CompanyPersonCalendar 2
:3 4

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
bool 
? 
isSalary 
{ 
get  #
;# $
set% (
;( )
}) *
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonCalendarPersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class ,
 INV_CompanyPersonCalendarPersons 9
:: ;

{ 
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
IDPersonCalendar %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonDepartments.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 (
INV_CompanyPersonDepartments		 5
:		6 7

{

 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
DepartmentId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
IsBasePosition #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
Manager1 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager2 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager3 
{ 
get  #
;# $
set% (
;( )
}) *
public   
Guid   
?   
Manager4   
{   
get    #
;  # $
set  % (
;  ( )
}  ) *
public!! 
Guid!! 
?!! 
Manager5!! 
{!! 
get!!  #
;!!# $
set!!% (
;!!( )
}!!) *
public"" 
Guid"" 
?"" 
Manager6"" 
{"" 
get""  #
;""# $
set""% (
;""( )
}"") *
}## 
}$$ �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_CompanyPersonSalary.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
INV_CompanyPersonSalary 0
:1 2

{ 
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
Salary 
{ 
get  #
;# $
set% (
;( )
}) *
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_Permit.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

INV_Permit #
:$ %

{ 
public 
string 

PermitCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
PermitTypeId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public## 
string## 
AccessPhone## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public$$ 
string$$ 
ArriveAdress$$ "
{$$# $
get$$% (
;$$( )
set$$* -
;$$- .
}$$. /
public%% 
string%% 
Detail%% 
{%% 
get%% "
;%%" #
set%%$ '
;%%' (
}%%( )
public&& 
DateTime&& 
?&&  
Manager1ApprovalDate&& -
{&&. /
get&&0 3
;&&3 4
set&&5 8
;&&8 9
}&&9 :
public'' 
DateTime'' 
?''  
Manager2ApprovalDate'' -
{''. /
get''0 3
;''3 4
set''5 8
;''8 9
}''9 :
public(( 
Guid(( 
?(( 
Manager1Approval(( %
{((& '
get((( +
;((+ ,
set((- 0
;((0 1
}((1 2
public)) 
Guid)) 
?)) 
Manager2Approval)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))1 2
public** 
DateTime** 
?** 
IkApprovalDate** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public++ 
Guid++ 
?++ 

IkApproval++ 
{++  !
get++" %
;++% &
set++' *
;++* +
}+++ ,
public,, 
double,, 
?,, 
	TotalDays,,  
{,,! "
get,,# &
;,,& '
set,,( +
;,,+ ,
},,, -
public-- 
Guid-- 
?-- 
IdUser-- 
{-- 
get-- !
;--! "
set--# &
;--& '
}--' (
public.. 
double.. 
?.. 

TotalHours.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}..- .
public// 
string// 

{//$ %
get//& )
;//) *
set//+ .
;//. /
}/// 0
public00 
string00 

{00$ %
get00& )
;00) *
set00+ .
;00. /
}00/ 0
public11 
string11 

{11$ %
get11& )
;11) *
set11+ .
;11. /
}11/ 0
}22 
}33 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_PermitConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
INV_PermitConfirmation /
:0 1

{ 
public 
Guid 
? 
permitId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
Guid 
? 
ruleId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
ruleStageId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
int
?

hasApprove
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_PermitOffical.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
INV_PermitOffical *
:+ ,

{ 
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
EndDate
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\INV_PermitType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
INV_PermitType '
:( )

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public 
bool 
? 
IsPaidPermit !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
	CanHourly 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
bool 
? 
BeDocumented !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
RequestStaff !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
	ViewStaff 
{  
get! $
;$ %
set& )
;) *
}* +
} 
}   �.
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\IOT_CameraLog.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 
	modelName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
string		 
version		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
readtime

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
string 

macAddress  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
	ipAddress 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
string

subnetMask
{
get
;
set
;
}
public 
string 

dnsAddress  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
	videoType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	audioType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
videoStatusChannel1 )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
videoStatusChannel2 )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
videoStatusChannel3 )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
videoStatusChannel4 )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
alarmInputStatus1 '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
alarmInputStatus2 '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
alarmInputStatus3 '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
alarmInputStatus4 '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
alarmOutputStatus1 (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
alarmOutputStatus2 (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
alarmOutputStatus3 (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
alarmOutputStatus4 (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string "
motionDetectionStatus1 ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string "
motionDetectionStatus2 ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string "
motionDetectionStatus3 ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public   
string   "
motionDetectionStatus4   ,
{  - .
get  / 2
;  2 3
set  4 7
;  7 8
}  8 9
public!! 
string!! %
tamperingDetectionStatus1!! /
{!!0 1
get!!2 5
;!!5 6
set!!7 :
;!!: ;
}!!; <
public"" 
string"" %
tamperingDetectionStatus2"" /
{""0 1
get""2 5
;""5 6
set""7 :
;"": ;
}""; <
public## 
string## %
tamperingDetectionStatus3## /
{##0 1
get##2 5
;##5 6
set##7 :
;##: ;
}##; <
public$$ 
string$$ %
tamperingDetectionStatus4$$ /
{$$0 1
get$$2 5
;$$5 6
set$$7 :
;$$: ;
}$$; <
public%% 
string%% 
recordingStatus1%% &
{%%' (
get%%) ,
;%%, -
set%%. 1
;%%1 2
}%%2 3
public&& 
string&& 
recordingStatus2&& &
{&&' (
get&&) ,
;&&, -
set&&. 1
;&&1 2
}&&2 3
public'' 
string'' 
recordingStatus3'' &
{''' (
get'') ,
;'', -
set''. 1
;''1 2
}''2 3
public(( 
string(( 
recordingStatus4(( &
{((' (
get(() ,
;((, -
set((. 1
;((1 2
}((2 3
})) 
}** �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\MB_MobileDevice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
MB_MobileDevice		 (
:		) *

{

 
public 
string 
UniqID 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
AppID 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
AppVersionCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
AppVersionName $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Device 
{ 
get "
;" #
set$ '
;' (
}( )
public"" 
string"" 
Model"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""' (
public&& 
string&& 
	OSVersion&& 
{&&  !
get&&" %
;&&% &
set&&' *
;&&* +
}&&+ ,
public** 
string** 
Brand** 
{** 
get** !
;**! "
set**# &
;**& '
}**' (
public.. 
string.. 
Board.. 
{.. 
get.. !
;..! "
set..# &
;..& '
}..' (
public22 
string22 
Product22 
{22 
get22  #
;22# $
set22% (
;22( )
}22) *
public66 
string66 
Serial66 
{66 
get66 "
;66" #
set66$ '
;66' (
}66( )
public:: 
string:: 
SDK:: 
{:: 
get:: 
;::  
set::! $
;::$ %
}::% &
public>> 
string>> 
ScreenScale>> !
{>>" #
get>>$ '
;>>' (
set>>) ,
;>>, -
}>>- .
publicBB 
intBB 
?BB 

DeviceTypeBB 
{BB  
getBB! $
;BB$ %
setBB& )
;BB) *
}BB* +
}CC 
}DD �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\MB_MobileDeviceRequests.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 #
MB_MobileDeviceRequests		 0
:		1 2

{

 
public 
Guid 
? 
DeviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
TicketId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 
Browser 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 

TotalBytes 
{  
get! $
;$ %
set& )
;) *
}* +
public"" 
string"" 
PostedFiles"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public&& 
string&& 
	IPAddress&& 
{&&  !
get&&" %
;&&% &
set&&' *
;&&* +
}&&+ ,
}'' 
}(( �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\MB_Notification.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
MB_Notification		 (
:		) *

{

 
public 
Guid 
IdNotification "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
Status 
{ 
get 
;  
set! $
;$ %
}% &
public 
DateTime 

{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Message 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
	EventCode 
{ 
get  #
;# $
set% (
;( )
}) *
public"" 
Guid"" 
?"" 
	IdUserRef"" 
{""  
get""! $
;""$ %
set""& )
;"") *
}""* +
}## 
}$$ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_Account.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 

PA_Account		 #
:		$ %

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
bankId 
{ 
get !
;! "
set# &
;& '
}' (
public"" 
string"" 
iban"" 
{"" 
get""  
;""  !
set""" %
;""% &
}""& '
public&& 
Guid&& 
?&& 
dataId&& 
{&& 
get&& !
;&&! "
set&&# &
;&&& '
}&&' (
public** 
string** 
	dataTable** 
{**  !
get**" %
;**% &
set**' *
;*** +
}**+ ,
public.. 
bool.. 
?.. 
status.. 
{.. 
get.. !
;..! "
set..# &
;..& '
}..' (
public22 
bool22 
?22 
	isDefault22 
{22  
get22! $
;22$ %
set22& )
;22) *
}22* +
}33 
}44 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_Advance.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

PA_Advance #
:$ %

{ 
public 
DateTime 
? 
progressDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
double		 
?		 
amount		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
Guid

 
?

 

currencyId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public
short
?
	direction
{
get
;
set
;
}
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
rejectedDescription )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
tax 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
?  
paymentRequestedDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_AdvanceConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
PA_AdvanceConfirmation /
:0 1

{ 
public 
Guid 
? 
	advanceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_Ledger.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	PA_Ledger		 "
:		# $

{

 
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
accountRealtedId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
tax 
{ 
get  
;  !
set" %
;% &
}& '
public"" 
double"" 
?"" 
rateExchange"" #
{""$ %
get""& )
;"") *
set""+ .
;"". /
}""/ 0
public&& 
Guid&& 
?&& 

currencyId&& 
{&&  !
get&&" %
;&&% &
set&&' *
;&&* +
}&&+ ,
public** 
DateTime** 
?** 
date** 
{** 
get**  #
;**# $
set**% (
;**( )
}**) *
public.. 
short.. 
?.. 
type.. 
{.. 
get..  
;..  !
set.." %
;..% &
}..& '
public22 
Guid22 
?22 

{22# $
get22% (
;22( )
set22* -
;22- .
}22. /
public66 
string66 
description66 !
{66" #
get66$ '
;66' (
set66) ,
;66, -
}66- .
public77 
bool77 
?77 
isBalanceFixing77 $
{77% &
get77' *
;77* +
set77, /
;77/ 0
}770 1
public88 
double88 
?88 
	crossRate88  
{88! "
get88# &
;88& '
set88( +
;88+ ,
}88, -
public99 
Guid99 
?99 
	advanceId99 
{99  
get99! $
;99$ %
set99& )
;99) *
}99* +
}:: 
};; �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_Transaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PA_Transaction		 '
:		( )

{

 
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public"" 
Guid"" 
?"" 

currencyId"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public&& 
DateTime&& 
?&& 
date&& 
{&& 
get&&  #
;&&# $
set&&% (
;&&( )
}&&) *
public** 
string** 
description** !
{**" #
get**$ '
;**' (
set**) ,
;**, -
}**- .
public.. 
DateTime.. 
?.. 
progressDate.. %
{..& '
get..( +
;..+ ,
set..- 0
;..0 1
}..1 2
public// 
Guid// 
?// 
dataId// 
{// 
get// !
;//! "
set//# &
;//& '
}//' (
public00 
string00 
	dataTable00 
{00  !
get00" %
;00% &
set00' *
;00* +
}00+ ,
public11 
short11 
?11 
status11 
{11 
get11 "
;11" #
set11$ '
;11' (
}11( )
public22 
double22 
?22 
tax22 
{22 
get22  
;22  !
set22" %
;22% &
}22& '
}33 
}44 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PA_TransactionConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
PA_TransactionConfirmation 3
:4 5

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_EvaluateForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_EvaluateForm )
:* +

{ 
public 
int 
? 
formType 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
formName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
formCode

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_FormPermitTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_FormPermitTask +
:, -

{ 
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
endDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
bool

 
?

 
status

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
evaluateFormId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_FormPermitTaskUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
PDS_FormPermitTaskUser /
:0 1

{ 
public 
Guid 
? 
formPermitTaskId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
Guid		 
?		 
evaluatorId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
Guid

 
?

 
evaluatedUserId

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_FormResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_FormResult '
:( )

{ 
public 
string 
comment 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
int		 
?		 
status		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

  
formPermitTaskUserId

 )
{

* +
get

, /
;

/ 0
set

1 4
;

4 5
}

5 6
public 
Guid 
? 
evaluateFormId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
evaluatorId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
Guid
?
evaluatedUserId
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_Question.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_Question %
:& '

{ 
public 
string 
question 
{  
get! $
;$ %
set& )
;) *
}* +
}		 
}

 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_QuestionForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_QuestionForm )
:* +

{ 
public 
double 
? 
factor 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
int		 
?		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
	groupName

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? 

groupOrder 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

questionId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
Guid
?
evaluateFormId
{
get
;
set
;
}
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PDS_QuestionResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PDS_QuestionResult +
:, -

{ 
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
comment		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
Guid

 
?

 
questionFormId

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
Guid 
? 
formResultId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
score 
{ 
get 
;  
set! $
;$ %
}% &
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Brand.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	PRD_Brand		 "
:		# $

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Category.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_Category		 %
:		& '

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_CompanyBasedPrice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
PRD_CompanyBasedPrice .
:/ 0

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

categoryId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
sellingType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
companyType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public## 
short## 
?## 
productType## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public'' 
short'' 
?'' 
type'' 
{'' 
get''  
;''  !
set''" %
;''% &
}''& '
}(( 
})) �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_CompanyBasedPriceDetail.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
PRD_CompanyBasedPriceDetail 4
:5 6

{ 
public 
Guid 
companyBasedPriceId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
minCondition #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

monthCount 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public## 
DateTime## 
?## 
endDate##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##, -
public$$ 
double$$ 
?$$ 
price$$ 
{$$ 
get$$ "
;$$" #
set$$$ '
;$$' (
}$$( )
}%% 
}&& �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_DistributionPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
PRD_DistributionPlan -
:. /

{ 
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
outputId 
{ 
get  #
;# $
set% (
;( )
}) *
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_DistributionPlanRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
PRD_DistributionPlanRelation 5
:6 7

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
distributionPlanId		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_EntegrationAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
PRD_EntegrationAction .
:/ 0

{ 
public 
Guid 
? 
EntegrationFileId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
DistributorName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string  
CustomerOperatorCode *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string  
CustomerOperatorName *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public   
Guid   
?   
CustomerOperatorId   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  3 4
public!! 
string!! '
CustomerOperatorStorageCode!! 1
{!!2 3
get!!4 7
;!!7 8
set!!9 <
;!!< =
}!!= >
public%% 
Guid%% 
?%% %
CustomerOperatorStorageId%% .
{%%/ 0
get%%1 4
;%%4 5
set%%6 9
;%%9 :
}%%: ;
public&& 
string&& '
CustomerOperatorStorageCity&& 1
{&&2 3
get&&4 7
;&&7 8
set&&9 <
;&&< =
}&&= >
public'' 
string'' '
CustomerOperatorStorageTown'' 1
{''2 3
get''4 7
;''7 8
set''9 <
;''< =
}''= >
public++ 
string++ 

BranchName++  
{++! "
get++# &
;++& '
set++( +
;+++ ,
}++, -
public// 
string// 

BranchCode//  
{//! "
get//# &
;//& '
set//( +
;//+ ,
}//, -
public33 
string33 
	TaxNumber33 
{33  !
get33" %
;33% &
set33' *
;33* +
}33+ ,
public77 
string77 
ConsolidationCode77 '
{77( )
get77* -
;77- .
set77/ 2
;772 3
}773 4
public;; 
string;; 
ConsolidationName;; '
{;;( )
get;;* -
;;;- .
set;;/ 2
;;;2 3
};;3 4
public?? 
Guid?? 
??? 
	ProductId?? 
{??  
get??! $
;??$ %
set??& )
;??) *
}??* +
public@@ 
Guid@@ 
?@@ 
InventoryId@@  
{@@! "
get@@# &
;@@& '
set@@( +
;@@+ ,
}@@, -
publicDD 
stringDD 
ImeiDD 
{DD 
getDD  
;DD  !
setDD" %
;DD% &
}DD& '
publicHH 
stringHH 
SerialNoHH 
{HH  
getHH! $
;HH$ %
setHH& )
;HH) *
}HH* +
publicLL 
intLL 
?LL 
QuantityLL 
{LL 
getLL "
;LL" #
setLL$ '
;LL' (
}LL( )
}MM 
}NN �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_EntegrationFiles.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
PRD_EntegrationFiles -
:. /

{ 
public 
string 
FileName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
FileNameDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
FileTypeName "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
CreateDateInFtp (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
DistributorName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
DateTime   
?   
ProcessTime   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  0 1
public$$ 
bool$$ 
?$$ 
ResultFilesReading$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public(( 
string(( #
ResultFilesErrorMessage(( -
{((. /
get((0 3
;((3 4
set((5 8
;((8 9
}((9 :
})) 
}** �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_EntegrationImport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
PRD_EntegrationImport .
:/ 0

{ 
public 
string 
customerName "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
customerCode "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
productModel "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
distributorName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? '
distributorConfirmationDate 4
{5 6
get7 :
;: ;
set< ?
;? @
}@ A
public 
string 
imei 
{ 
get  
;  !
set" %
;% &
}& '
public## 
string## 
customerType## "
{### $
get##% (
;##( )
set##* -
;##- .
}##. /
public'' 
DateTime'' 
?'' 
contractStartDate'' *
{''+ ,
get''- 0
;''0 1
set''2 5
;''5 6
}''6 7
public++ 
string++ 
contractCode++ "
{++# $
get++% (
;++( )
set++* -
;++- .
}++. /
public// 
string// 
productGroup// "
{//# $
get//% (
;//( )
set//* -
;//- .
}//. /
public33 
string33 
sellingChannelType33 (
{33) *
get33+ .
;33. /
set330 3
;333 4
}334 5
public77 
string77 
distributorCode77 %
{77& '
get77( +
;77+ ,
set77- 0
;770 1
}771 2
public;; 
int;; 
?;; 
sellingQuantity;; #
{;;$ %
get;;& )
;;;) *
set;;+ .
;;;. /
};;/ 0
public?? 
int?? 
??? 
month?? 
{?? 
get?? 
;??  
set??! $
;??$ %
}??% &
publicCC 
intCC 
?CC 
yearCC 
{CC 
getCC 
;CC 
setCC  #
;CC# $
}CC$ %
}DD 
}EE �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_EntegrationStorage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
PRD_EntegrationStorage /
:0 1

{ 
public 
Guid 
? 
EntegrationFileId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
DistributorName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
DistStorageCode %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
DistStorageName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
DistStorageCity %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public!! 
string!! 
DistStorageTown!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public%% 
string%% 
ConsolidationCode%% '
{%%( )
get%%* -
;%%- .
set%%/ 2
;%%2 3
}%%3 4
public)) 
string)) 
ConsolidationName)) '
{))( )
get))* -
;))- .
set))/ 2
;))2 3
}))3 4
public-- 
Guid-- 
?-- 
	ProductId-- 
{--  
get--! $
;--$ %
set--& )
;--) *
}--* +
public11 
Guid11 
?11 
InventoryId11  
{11! "
get11# &
;11& '
set11( +
;11+ ,
}11, -
public55 
string55 
Imei55 
{55 
get55  
;55  !
set55" %
;55% &
}55& '
public99 
string99 
SerialNo99 
{99  
get99! $
;99$ %
set99& )
;99) *
}99* +
public== 
int== 
?== 
Quantity== 
{== 
get== "
;==" #
set==$ '
;==' (
}==( )
}>> 
}?? �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Inventory.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 

:		' (

{

 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 

serialcode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_InventoryAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_InventoryAction		 ,
:		- .

{

 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
transactionItemId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public"" 
string"" 
	dataTable"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public&& 
Guid&& 
?&& 

{&&# $
get&&% (
;&&( )
set&&* -
;&&- .
}&&. /
public** 
	IGeometry** 
location** "
{**# $
get**% (
;**( )
set*** -
;**- .
}**. /
}++ 
},, �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_InventoryTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_InventoryTask		 *
:		+ ,

{

 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
period 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public"" 
string"" 
description"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public## 
Guid## 
?## 
userId## 
{## 
get## !
;##! "
set### &
;##& '
}##' (
public$$ 
Guid$$ 
?$$ 
	companyId$$ 
{$$  
get$$! $
;$$$ %
set$$& )
;$$) *
}$$* +
}%% 
}&& �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Product.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_Product		 $
:		% &

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
barcode 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 

categoryId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public"" 
Guid"" 
?"" 
brandId"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}""( )
public&& 
Guid&& 
?&& 
unitId&& 
{&& 
get&& !
;&&! "
set&&# &
;&&& '
}&&' (
public** 
short** 
?** 
type** 
{** 
get**  
;**  !
set**" %
;**% &
}**& '
public.. 
short.. 
?.. 
status.. 
{.. 
get.. "
;.." #
set..$ '
;..' (
}..( )
public22 
short22 
?22 
barcodeType22 !
{22" #
get22$ '
;22' (
set22) ,
;22, -
}22- .
public33 
bool33 
?33 
isCriticalStock33 $
{33% &
get33' *
;33* +
set33, /
;33/ 0
}330 1
public44 
int44 
?44 

{44" #
get44$ '
;44' (
set44) ,
;44, -
}44- .
public88 
short88 
?88 
	stockType88 
{88  !
get88" %
;88% &
set88' *
;88* +
}88+ ,
}99 
}:: �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductBounty.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRD_ProductBounty *
:+ ,

{ 
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
	companyId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
	productId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 
year 
{ 
get 
; 
set  #
;# $
}$ %
public 
int 
? 
month 
{ 
get 
;  
set! $
;$ %
}% &
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductCompany.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductCompany		 +
:		, -

{

 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Production.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_Production		 '
:		( )

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
productionOrderDate ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
DateTime 
? #
scheduledProductionDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
Guid 
? 
productionCompanyId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 
productionStorageId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public   
Guid   
?   
centerCompanyId   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  0 1
public$$ 
Guid$$ 
?$$ 
centerStorageId$$ $
{$$% &
get$$' *
;$$* +
set$$, /
;$$/ 0
}$$0 1
public(( 
Guid(( 
?(( 
	productId(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
public,, 
double,, 
?,, 
quantity,, 
{,,  !
get,," %
;,,% &
set,,' *
;,,* +
},,+ ,
public00 
string00 
code00 
{00 
get00  
;00  !
set00" %
;00% &
}00& '
public44 
DateTime44 
?44 
lastProductionDate44 +
{44, -
get44. 1
;441 2
set443 6
;446 7
}447 8
public88 
string88 
schemaTitle88 !
{88" #
get88$ '
;88' (
set88) ,
;88, -
}88- .
}99 
}:: �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionOperation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 #
PRD_ProductionOperation		 0
:		1 2

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionProduct.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 !
PRD_ProductionProduct		 .
:		/ 0

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
serialCodes !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public"" 
double"" 
?"" 
amountSpent"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}"". /
public&& 
double&& 
?&& 
price&& 
{&& 
get&& "
;&&" #
set&&$ '
;&&' (
}&&( )
public** 
Guid** 
?** 

materialId** 
{**  !
get**" %
;**% &
set**' *
;*** +
}**+ ,
public.. 
short.. 
?.. 
type.. 
{.. 
get..  
;..  !
set.." %
;..% &
}..& '
public// 
short// 
?// 
transactionType// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
Guid00 
?00 

currencyId00 
{00  !
get00" %
;00% &
set00' *
;00* +
}00+ ,
public11 
Guid11 
?11 
unitId11 
{11 
get11 !
;11! "
set11# &
;11& '
}11' (
public22 
double22 
?22 

amountFire22 !
{22" #
get22$ '
;22' (
set22) ,
;22, -
}22- .
}33 
}44 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionProducts.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 "
PRD_ProductionProducts		 /
:		0 1

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
serialCodes !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
quantity 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public"" 
int"" 
?"" 
amountSpent"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public&& 
double&& 
?&& 
price&& 
{&& 
get&& "
;&&" #
set&&$ '
;&&' (
}&&( )
public** 
Guid** 
?** 

materialId** 
{**  !
get**" %
;**% &
set**' *
;*** +
}**+ ,
public.. 
short.. 
?.. 
type.. 
{.. 
get..  
;..  !
set.." %
;..% &
}..& '
}// 
}00 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionSchema.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		  
PRD_ProductionSchema		 -
:		. /

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionSchemaStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 %
PRD_ProductionSchemaStage		 2
:		3 4

{

 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 
stageNum 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
productionSchemaId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionSchemaSteps.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 %
PRD_ProductionSchemaSteps		 2
:		3 4

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 
stageNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductionStage		 ,
:		- .

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 
stageNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
productionSchemaId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductionUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductionUser		 +
:		, -

{

 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductMateriel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductMateriel		 ,
:		- .

{

 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

materialId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductPointSelling.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 #
PRD_ProductPointSelling		 0
:		1 2

{

 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductPrice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductPrice		 )
:		* +

{

 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_ProductTask		 (
:		) *

{

 
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
period 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
}   
}!! �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_ProductUnit.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRD_ProductUnit (
:) *

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
unitId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
double

 
?

 
quantity

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 
	isDefault 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_StockTaskPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRD_StockTaskPlan *
:+ ,

{ 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
taskId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
	storageId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public
string
inventoryIndexValue
{
get
;
set
;
}
public 
int 
? 
inventoryStampYear &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
newInventoryBrand '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string "
newInventoryIndexValue ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
Guid 
? 
newInventoryId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? !
newInventoryStampYear )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string !
inventorySerialNumber +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_TitanDeviceActivated.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
PRD_TitanDeviceActivated 1
:2 3

{ 
public 
string 
SerialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
IMEI1 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
IMEI2 
{ 
get !
;! "
set# &
;& '
}' (
public 
DateTime 
? 
CreatedOfTitan '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
DeviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	ProductId 
{  
get! $
;$ %
set& )
;) *
}* +
public## 
Guid## 
?## 
InventoryId##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##, -
public$$ 
string$$ 
TitanDeviceName$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% 

TitanModel%%  
{%%! "
get%%# &
;%%& '
set%%( +
;%%+ ,
}%%, -
public&& 
string&& 
TitanProduct&& "
{&&# $
get&&% (
;&&( )
set&&* -
;&&- .
}&&. /
}'' 
}(( �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_Transaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_Transaction		 (
:		) *

{

 
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public"" 
Guid"" 
?"" 
	invoiceId"" 
{""  
get""! $
;""$ %
set""& )
;"") *
}""* +
public&& 
Guid&& 
?&& 
orderId&& 
{&& 
get&& "
;&&" #
set&&$ '
;&&' (
}&&( )
public** 
Guid** 
?** 
outputId** 
{** 
get**  #
;**# $
set**% (
;**( )
}**) *
public.. 
string.. 
outputTable.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}..- .
public22 
Guid22 
?22 
outputCompanyId22 $
{22% &
get22' *
;22* +
set22, /
;22/ 0
}220 1
public66 
Guid66 
?66 
inputId66 
{66 
get66 "
;66" #
set66$ '
;66' (
}66( )
public:: 
string:: 

inputTable::  
{::! "
get::# &
;::& '
set::( +
;::+ ,
}::, -
public>> 
Guid>> 
?>> 
inputCompanyId>> #
{>>$ %
get>>& )
;>>) *
set>>+ .
;>>. /
}>>/ 0
publicBB 
DateTimeBB 
?BB 
	startDateBB "
{BB# $
getBB% (
;BB( )
setBB* -
;BB- .
}BB. /
publicFF 
DateTimeFF 
?FF 
endDateFF  
{FF! "
getFF# &
;FF& '
setFF( +
;FF+ ,
}FF, -
}GG 
}HH �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRD_TransactionItem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
PRD_TransactionItem		 ,
:		- .

{

 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
	unitPrice  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
serialCodes !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public   
Guid   
?   
alternativeUnitId   &
{  ' (
get  ) ,
;  , -
set  . 1
;  1 2
}  2 3
public!! 
double!! 
?!! 
alternativeQuantity!! *
{!!+ ,
get!!- 0
;!!0 1
set!!2 5
;!!5 6
}!!6 7
}"" 
}## �'
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_Project.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRJ_Project $
:% &

{ 
public 
string 
ProjectName !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
ProjectCode !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
ProjectScope "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
ProjectStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
ProjectEndDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
DateTime 
? 
ProjectConfirmDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public## 
bool## 
?## 
	IsConfirm## 
{##  
get##! $
;##$ %
set##& )
;##) *
}##* +
public'' 
bool'' 
?'' 
IsActive'' 
{'' 
get''  #
;''# $
set''% (
;''( )
}'') *
public++ 
int++ 
?++ 
ProjectType++ 
{++  !
get++" %
;++% &
set++' *
;++* +
}+++ ,
public,, 
Guid,, 
?,, 
	CompanyId,, 
{,,  
get,,! $
;,,$ %
set,,& )
;,,) *
},,* +
public-- 
Guid-- 
?-- 

{--# $
get--% (
;--( )
set--* -
;--- .
}--. /
public11 
string11 
TenderNo11 
{11  
get11! $
;11$ %
set11& )
;11) *
}11* +
public55 
string55 

TenderName55  
{55! "
get55# &
;55& '
set55( +
;55+ ,
}55, -
public99 
string99 
TenderWrite99 !
{99" #
get99$ '
;99' (
set99) ,
;99, -
}99- .
public:: 
Guid:: 
?:: 
IdProjectLinked:: $
{::% &
get::' *
;::* +
set::, /
;::/ 0
}::0 1
public;; 
string;; 
ProjectJiraKey;; $
{;;% &
get;;' *
;;;* +
set;;, /
;;;/ 0
};;0 1
public<< 
DateTime<< 
?<< 
WarrantyStartDate<< *
{<<+ ,
get<<- 0
;<<0 1
set<<2 5
;<<5 6
}<<6 7
public== 
DateTime== 
?== 
WarrantyEndDate== (
{==) *
get==+ .
;==. /
set==0 3
;==3 4
}==4 5
public>> 
string>>  
ProjectTechnicalType>> *
{>>+ ,
get>>- 0
;>>0 1
set>>2 5
;>>5 6
}>>6 7
publicBB 
intBB 
?BB 
	PriceTypeBB 
{BB 
getBB  #
;BB# $
setBB% (
;BB( )
}BB) *
publicFF 
doubleFF 
?FF 
ExchangeFF 
{FF  !
getFF" %
;FF% &
setFF' *
;FF* +
}FF+ ,
publicJJ 
DateTimeJJ 
?JJ !
ContractGuaranteeDateJJ .
{JJ/ 0
getJJ1 4
;JJ4 5
setJJ6 9
;JJ9 :
}JJ: ;
publicNN 
DateTimeNN 
?NN  
AdvanceGuaranteeDateNN -
{NN. /
getNN0 3
;NN3 4
setNN5 8
;NN8 9
}NN9 :
publicRR 
DateTimeRR 
?RR !
WarrantyGuaranteeDateRR .
{RR/ 0
getRR1 4
;RR4 5
setRR6 9
;RR9 :
}RR: ;
publicSS 
doubleSS 
?SS 
ContractAmountSS %
{SS& '
getSS( +
;SS+ ,
setSS- 0
;SS0 1
}SS1 2
publicTT 
doubleTT 
?TT 

{TT% &
getTT' *
;TT* +
setTT, /
;TT/ 0
}TT0 1
publicUU 
doubleUU 
?UU 
WarrantyAmountUU %
{UU& '
getUU( +
;UU+ ,
setUU- 0
;UU0 1
}UU1 2
}VV 
}WW �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectInvoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRJ_ProjectInvoice +
:, -

{ 
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectScopeLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
PRJ_ProjectScopeLevel .
:/ 0

{ 
public 
string 
level 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectScopeLevelItems.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
PRJ_ProjectScopeLevelItems 3
:4 5

{ 
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
scopeLevelId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
Guid

 
?

 

locationId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectServiceLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
PRJ_ProjectServiceLevel 0
:1 2

{ 
public 
Guid 
? 
scopeLevelId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
short		 
?		 

amercement		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
short

 
?

 
endTime

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
resolutionType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
bool
?
nextDay
{
get
;
set
;
}
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
taskType 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
typeLevelId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
resolutionTime $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
	startTime 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectSubjectLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
PRJ_ProjectSubjectLevel 0
:1 2

{ 
public 
string 
level 
{ 
get !
;! "
set# &
;& '
}' (
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
	projectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectSubjectLevelItems.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
PRJ_ProjectSubjectLevelItems 5
:6 7

{ 
public 
Guid 
? 
	subjectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
subjectLevelId

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectTimeline.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRJ_ProjectTimeline ,
:- .

{ 
public 
Guid 
	IdProject 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
Status 
{ 
get "
;" #
set$ '
;' (
}( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectTimelinePersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
PRJ_ProjectTimelinePersons 3
:4 5

{ 
public 
Guid 
? 

IdTimeline 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
	IdProject		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
IdUser

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ProjectTypeLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
PRJ_ProjectTypeLevel -
:. /

{ 
public 
string 
level 
{ 
get !
;! "
set# &
;& '
}' (
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
	projectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\PRJ_ScopeRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
PRJ_ScopeRelation *
:+ ,

{ 
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
corporateId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
Guid

 
?

 
	storageId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
endDate
{
get
;
set
;
}
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_AgileBoards.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SH_AgileBoards		 '
:		( )

{

 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public
string
description
{
get
;
set
;
}
public 
string 
json 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
lastUsedDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Competencies.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_Competencies (
:) *

{ 
public 
string 
CompetenciesName &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_CorrectiveActivity.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
SH_CorrectiveActivity .
:/ 0

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
workAccidentId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
Guid

 
?

 
	projectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
keyword 
{ 
get  #
;# $
set% (
;( )
}) *
public
DateTime
?
date
{
get
;
set
;
}
public 
Guid 
? 

templateId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_FilesRole.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_FilesRole %
:& '

{ 
public 
Guid 
? 
roleid 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
	fileGroup		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
string

 
	dataTable

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
bool 
? 
insert 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
delete 
{ 
get !
;! "
set# &
;& '
}' (
public
bool
?
preview
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Group.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_Group !
:" #

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_GroupUsers.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
groupId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_LinkedRoles.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SH_LinkedRoles		 '
:		( )

{

 
public 
Guid 
? 
RoleId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
InnerRoleId  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Pages.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_Pages !
:" #

{ 
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
Action		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
string

 
Area

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 

Controller  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Method 
{ 
get "
;" #
set$ '
;' (
}( )
public
bool
?

{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PagesRole.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_PagesRole %
:& '

{ 
public 
Guid 
? 
roleid 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
action		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PartialAssigment.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SH_PartialAssigment		 ,
:		- .

{

 
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
courseHours "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
schoolDepartment &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
lesson 
{ 
get "
;" #
set$ '
;' (
}( )
public"" 
double"" 
?"" 

hourlyWage"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public## 
string## 
	staffName## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonCertificate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
SH_PersonCertificate -
:. /

{ 
public 
Guid 
? 
CertificateTypeId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
int
?
CertificateTime
{
get
;
set
;
}
public 
DateTime 
? 
ExpirationDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonCertificateType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
SH_PersonCertificateType 1
:2 3

{ 
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
}		 
}

 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonCompetencies.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
SH_PersonCompetencies .
:/ 0

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
CompetenciesId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 
CompetenciesLevel %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �6
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonInformation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
SH_PersonInformation -
:. /

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Nationality !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Gender 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Military 
{ 
get "
;" #
set$ '
;' (
}( )
public 
DateTime 
? 
MilitaryDoneDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public## 
int## 
?##  
MilitaryDoneDuration## (
{##) *
get##+ .
;##. /
set##0 3
;##3 4
}##4 5
public'' 
string''  
MilitaryExemptDetail'' *
{''+ ,
get''- 0
;''0 1
set''2 5
;''5 6
}''6 7
public++ 
string++ #
MilitaryProbationDetail++ -
{++. /
get++0 3
;++3 4
set++5 8
;++8 9
}++9 :
public// 
DateTime// 
?// !
MilitaryProbationDate// .
{/// 0
get//1 4
;//4 5
set//6 9
;//9 :
}//: ;
public33 
string33 
IDSerialNumber33 $
{33% &
get33' *
;33* +
set33, /
;33/ 0
}330 1
public77 
string77 
IDMotherName77 "
{77# $
get77% (
;77( )
set77* -
;77- .
}77. /
public;; 
string;; 
IDFatherName;; "
{;;# $
get;;% (
;;;( )
set;;* -
;;;- .
};;. /
public?? 
Guid?? 
??? 
IDBornLocation?? #
{??$ %
get??& )
;??) *
set??+ .
;??. /
}??/ 0
publicCC 
intCC 
?CC 
IDBloodGroupCC  
{CC! "
getCC# &
;CC& '
setCC( +
;CC+ ,
}CC, -
publicGG 
stringGG 
IDPreviousSurnameGG '
{GG( )
getGG* -
;GG- .
setGG/ 2
;GG2 3
}GG3 4
publicKK 
GuidKK 
?KK 
IDCityKK 
{KK 
getKK !
;KK! "
setKK# &
;KK& '
}KK' (
publicOO 
GuidOO 
?OO 
IDTownOO 
{OO 
getOO !
;OO! "
setOO# &
;OO& '
}OO' (
publicSS 
stringSS 

IDDistrictSS  
{SS! "
getSS# &
;SS& '
setSS( +
;SS+ ,
}SS, -
publicWW 
stringWW 
	IDVillageWW 
{WW  !
getWW" %
;WW% &
setWW' *
;WW* +
}WW+ ,
public[[ 
string[[ 
IDDeliveryLocation[[ (
{[[) *
get[[+ .
;[[. /
set[[0 3
;[[3 4
}[[4 5
public__ 
string__ 
IDDeliveryDetail__ &
{__' (
get__) ,
;__, -
set__. 1
;__1 2
}__2 3
publiccc 
stringcc 
IDRecordNumbercc $
{cc% &
getcc' *
;cc* +
setcc, /
;cc/ 0
}cc0 1
publicgg 
DateTimegg 
?gg 
IDDeliveryDategg '
{gg( )
getgg* -
;gg- .
setgg/ 2
;gg2 3
}gg3 4
publickk 
stringkk 
IDVolumeNumberkk $
{kk% &
getkk' *
;kk* +
setkk, /
;kk/ 0
}kk0 1
publicoo 
stringoo 
IDFamilyNumberoo $
{oo% &
getoo' *
;oo* +
setoo, /
;oo/ 0
}oo0 1
publicss 
stringss 
IDRowNumberss !
{ss" #
getss$ '
;ss' (
setss) ,
;ss, -
}ss- .
publicww 
stringww 
PersonalMailww "
{ww# $
getww% (
;ww( )
setww* -
;ww- .
}ww. /
public{{ 
string{{ 
PersonalHomePhone{{ '
{{{( )
get{{* -
;{{- .
set{{/ 2
;{{2 3
}{{3 4
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
�� 
string
�� 
EmergencyPhone
�� $
{
��% &
get
��' *
;
��* +
set
��, /
;
��/ 0
}
��0 1
public
�� 
string
�� 

�� #
{
��$ %
get
��& )
;
��) *
set
��+ .
;
��. /
}
��/ 0
public
�� 
string
��  
EmergencyProximity
�� (
{
��) *
get
��+ .
;
��. /
set
��0 3
;
��3 4
}
��4 5
public
�� 
string
�� 
	Religious
�� 
{
��  !
get
��" %
;
��% &
set
��' *
;
��* +
}
��+ ,
public
�� 
string
��  
IDBornTownLocation
�� (
{
��) *
get
��+ .
;
��. /
set
��0 3
;
��3 4
}
��4 5
public
�� 
string
�� %
InsuranceIdentityNumber
�� -
{
��. /
get
��0 3
;
��3 4
set
��5 8
;
��8 9
}
��9 :
public
�� 
string
�� "
IdentificationNumber
�� *
{
��+ ,
get
��- 0
;
��0 1
set
��2 5
;
��5 6
}
��6 7
public
�� 
bool
�� 
?
�� 
hasAgi
�� 
{
�� 
get
�� !
;
��! "
set
��# &
;
��& '
}
��' (
}
�� 
}�� �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonLanguages.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_PersonLanguages +
:, -

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
	Languages 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
Reads 
{ 
get 
;  
set! $
;$ %
}% &
public 
int 
? 
Write 
{ 
get 
;  
set! $
;$ %
}% &
public 
int 
? 
Speak 
{ 
get 
;  
set! $
;$ %
}% &
public 
Guid 
? 
CertificateTypeId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
int   
?   
CertificateTime   #
{  $ %
get  & )
;  ) *
set  + .
;  . /
}  / 0
public!! 
DateTime!! 
?!! 
ExpirationDate!! '
{!!( )
get!!* -
;!!- .
set!!/ 2
;!!2 3
}!!3 4
public"" 
double"" 
?"" 
point"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}""( )
}## 
}$$ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonReferences.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_PersonReferences ,
:- .

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
ReferenceUserName '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
ReferencePosition '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
ReferencePhone $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string #
ReferenceWorkingCompany -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
}   
}!! �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonSchools.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_PersonSchools )
:* +

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
Guid

 
?

 
UserId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
SchoolId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
Level 
{ 
get 
;  
set! $
;$ %
}% &
public
string
Branch
{
get
;
set
;
}
public 
string 
area 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_PersonWorkExperience.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
SH_PersonWorkExperience 0
:1 2

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
CompanyName !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
JobStartDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? 

JobEndDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
WorkingPosition %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
JobDescription $
{% &
get' *
;* +
set, /
;/ 0
}0 1
}   
}!! �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Publications.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SH_Publications		 (
:		) *

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
keywords 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Report.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	SH_Report "
:# $

{ 
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public		 
int		 
?		 
type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
schema

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Role.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_Role  
:! "

{ 
public 
string 
rolname 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
roledescription		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public
short
?
roletype
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Schools.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

SH_Schools #
:$ %

{ 
public 
string 

SchoolName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
int		 
?		 
Type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_ShiftTracking.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_ShiftTracking )
:* +

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
shiftTrackingStatus )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 

qrCodeData 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	timestamp "
{# $
get% (
;( )
set* -
;- .
}. /
public## 
Guid## 
?## 
tableId## 
{## 
get## "
;##" #
set##$ '
;##' (
}##( )
public'' 
string'' 
	tableName'' 
{''  !
get''" %
;''% &
set''' *
;''* +
}''+ ,
public(( 
string(( 
qrCodeDataText(( $
{((% &
get((' *
;((* +
set((, /
;((/ 0
}((0 1
public,, 
Guid,, 
?,, !
shiftTrackingDeviceId,, *
{,,+ ,
get,,- 0
;,,0 1
set,,2 5
;,,5 6
},,6 7
public00 
int00 
?00 
passType00 
{00 
get00 "
;00" #
set00$ '
;00' (
}00( )
public44 
string44 
deviceUserId44 "
{44# $
get44% (
;44( )
set44* -
;44- .
}44. /
}55 
}66 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_ShiftTrackingDevice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
SH_ShiftTrackingDevice /
:0 1

{ 
public 
string 

DeviceName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

DeviceCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
DeviceBrand !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
DeviceModel !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
DeviceSerialNo $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
	IGeometry 
Location "
{# $
get% (
;( )
set* -
;- .
}. /
public## 
string## 
	IPAddress## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
public'' 
string'' 

{''$ %
get''& )
;'') *
set''+ .
;''. /
}''/ 0
public++ 
string++ 
Gateway++ 
{++ 
get++  #
;++# $
set++% (
;++( )
}++) *
public// 
int// 
?// 
Port// 
{// 
get// 
;// 
set//  #
;//# $
}//$ %
public33 
int33 
?33 

{33" #
get33$ '
;33' (
set33) ,
;33, -
}33- .
}44 
}55 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_ShiftTrackingDeviceUsers.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
SH_ShiftTrackingDeviceUsers 4
:5 6

{ 
public 
Guid 
? 
deviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
deviceUserId "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_Ticket.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	SH_Ticket "
:# $

{ 
public 
Guid 
? 
userid 
{ 
get !
;! "
set# &
;& '
}' (
public		 
DateTime		 
?		 

createtime		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
DateTime

 
?

 
endtime

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
IP 
{ 
get 
; 
set  #
;# $
}$ %
public 
Guid 
? 
DeviceId 
{ 
get  #
;# $
set% (
;( )
}) *
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_User.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SH_User		  
:		! "

{

 
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
	loginname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	firstname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public"" 
string"" 
lastname"" 
{""  
get""! $
;""$ %
set""& )
;"") *
}""* +
public&& 
DateTime&& 
?&& 
birthday&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&- .
public** 
string** 
password** 
{**  
get**! $
;**$ %
set**& )
;**) *
}*** +
public.. 
string.. 
title.. 
{.. 
get.. !
;..! "
set..# &
;..& '
}..' (
public22 
string22 
email22 
{22 
get22 !
;22! "
set22# &
;22& '
}22' (
public66 
string66 
phone66 
{66 
get66 !
;66! "
set66# &
;66& '
}66' (
public:: 
string:: 
	cellphone:: 
{::  !
get::" %
;::% &
set::' *
;::* +
}::+ ,
public>> 
string>> 
address>> 
{>> 
get>>  #
;>># $
set>>% (
;>>( )
}>>) *
publicBB 
GuidBB 
?BB 

locationIdBB 
{BB  !
getBB" %
;BB% &
setBB' *
;BB* +
}BB+ ,
publicFF 
stringFF 
companyCellPhoneFF &
{FF' (
getFF) ,
;FF, -
setFF. 1
;FF1 2
}FF2 3
publicJJ 
stringJJ  
companyCellPhoneCodeJJ *
{JJ+ ,
getJJ- 0
;JJ0 1
setJJ2 5
;JJ5 6
}JJ6 7
publicNN 
stringNN 
companyOfficePhoneNN (
{NN) *
getNN+ .
;NN. /
setNN0 3
;NN3 4
}NN4 5
publicRR 
stringRR "
companyOfficePhoneCodeRR ,
{RR- .
getRR/ 2
;RR2 3
setRR4 7
;RR7 8
}RR8 9
}SS 
}TT �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\sh_user2.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
sh_user2 !
:" #

{ 
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public		 
int		 
?		 
type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
	loginname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	firstname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
string
lastname
{
get
;
set
;
}
public 
DateTime 
? 
birthday !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
password 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	cellphone 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
address 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

locationId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
companyCellPhone &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string  
companyCellPhoneCode *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
companyOfficePhone (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string "
companyOfficePhoneCode ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_UserEmailAccount.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_UserEmailAccount ,
:- .

{ 
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
password		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
mailType

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
int 
? 
	isDefault 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
userid 
{ 
get !
;! "
set# &
;& '
}' (
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_UserEmailAccountShare.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
SH_UserEmailAccountShare 1
:2 3

{ 
public 
Guid 
? 

userIdFrom 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
userIdTo		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
bool

 
?

 
share

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_UserFireBaseToken.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
SH_UserFireBaseToken -
:. /

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
token		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_UserRole.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_UserRole $
:% &

{ 
public 
Guid 
? 
userid 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
roleid		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_WorkAccident.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SH_WorkAccident (
:) *

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
userId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
keyword 
{ 
get  #
;# $
set% (
;( )
}) *
public 
DateTime 
? 
accidentDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
Guid
?

templateId
{
get
;
set
;
}
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_WorkAccidentCalendar.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
SH_WorkAccidentCalendar 0
:1 2

{ 
public 
Guid 
? 
workAccidentId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 #
companyPersonCalendarId		 ,
{		- .
get		/ 2
;		2 3
set		4 7
;		7 8
}		8 9
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SH_WorkAccidentCertificate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
SH_WorkAccidentCertificate 3
:4 5

{ 
public 
Guid 
? 
workAccidentId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
personCertificateId		 (
{		) *
get		+ .
;		. /
set		0 3
;		3 4
}		4 5
}

 
} �&
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_Service.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SRV_Service		 $
:		% &

{

 
public 
Guid 
? 
PID 
{ 
get 
; 
set  #
;# $
}$ %
public   
int   
?   

{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public.. 
int.. 
?.. 
ServiceType.. 
{..  !
get.." %
;..% &
set..' *
;..* +
}..+ ,
public22 
string22 
ServiceCode22 !
{22" #
get22$ '
;22' (
set22) ,
;22, -
}22- .
public66 
Guid66 
?66 
ServiceCategory66 $
{66% &
get66' *
;66* +
set66, /
;66/ 0
}660 1
public:: 
Guid:: 
?:: 
ServiceFixture:: #
{::$ %
get::& )
;::) *
set::+ .
;::. /
}::/ 0
public>> 
Guid>> 
?>> 
ServiceStationId>> %
{>>& '
get>>( +
;>>+ ,
set>>- 0
;>>0 1
}>>1 2
publicBB 
GuidBB 
?BB "
ServiceDeclarationFirmBB +
{BB, -
getBB. 1
;BB1 2
setBB3 6
;BB6 7
}BB7 8
publicFF 
GuidFF 
?FF $
ServiceDeclarationPersonFF -
{FF. /
getFF0 3
;FF3 4
setFF5 8
;FF8 9
}FF9 :
publicJJ 
stringJJ 

{JJ$ %
getJJ& )
;JJ) *
setJJ+ .
;JJ. /
}JJ/ 0
publicNN 
stringNN 
VerificationCodeNN &
{NN' (
getNN) ,
;NN, -
setNN. 1
;NN1 2
}NN2 3
publicVV 
boolVV 
?VV #
VerificationCodeConfirmVV ,
{VV- .
getVV/ 2
;VV2 3
setVV4 7
;VV7 8
}VV8 9
publicZZ 
GuidZZ 
?ZZ )
VerificationCodeConfirmPersonZZ 2
{ZZ3 4
getZZ5 8
;ZZ8 9
setZZ: =
;ZZ= >
}ZZ> ?
public^^ 
	IGeometry^^ +
VerificationCodeConfirmLocation^^ 9
{^^: ;
get^^< ?
;^^? @
set^^A D
;^^D E
}^^E F
publicbb 
Guidbb 
?bb "
ServiceOperationPersonbb +
{bb, -
getbb. 1
;bb1 2
setbb3 6
;bb6 7
}bb7 8
publicff 
DateTimeff 
?ff  
ServiceOperationDateff -
{ff. /
getff0 3
;ff3 4
setff5 8
;ff8 9
}ff9 :
publicjj 
stringjj (
ServiceOperationResultDetailjj 2
{jj3 4
getjj5 8
;jj8 9
setjj: =
;jj= >
}jj> ?
publicnn 
	IGeometrynn $
ServiceOperationLocationnn 2
{nn3 4
getnn5 8
;nn8 9
setnn: =
;nn= >
}nn> ?
publicrr 
DateTimerr 
?rr '
ServiceOperationComfirmDaterr 4
{rr5 6
getrr7 :
;rr: ;
setrr< ?
;rr? @
}rr@ A
publicvv 
Guidvv 
?vv )
ServiceOperationComfirmPersonvv 2
{vv3 4
getvv5 8
;vv8 9
setvv: =
;vv= >
}vv> ?
publiczz 
stringzz )
ServiceOperationComfirmDetailzz 3
{zz4 5
getzz6 9
;zz9 :
setzz; >
;zz> ?
}zz? @
public
�� 
int
�� 
?
�� +
ServiceOperationComfirmResult
�� 1
{
��2 3
get
��4 7
;
��7 8
set
��9 <
;
��< =
}
��= >
public
�� 
	IGeometry
�� 
CurrentLocation
�� )
{
��* +
get
��, /
;
��/ 0
set
��1 4
;
��4 5
}
��5 6
public
�� 
int
�� 
?
�� #
CurrentLocationStatus
�� )
{
��* +
get
��, /
;
��/ 0
set
��1 4
;
��4 5
}
��5 6
public
�� 
bool
�� 
?
�� 

�� "
{
��# $
get
��% (
;
��( )
set
��* -
;
��- .
}
��. /
}
�� 
}�� �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_ServiceCalibration.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 "
SRV_ServiceCalibration		 /
:		0 1

{

 
public 
double 
?  
CurrentMeasuredValue +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
double 
? 
RealMeasuredValue (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 
	ServiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
StationMonitorId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
MonitorName !
{" #
get$ '
;' (
set) ,
;, -
}- .
public"" 
string"" 
Details"" 
{"" 
get""  #
;""# $
set""% (
;""( )
}"") *
public&& 
string&& 
Unit&& 
{&& 
get&&  
;&&  !
set&&" %
;&&% &
}&&& '
}'' 
}(( �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_ServiceFiles.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SRV_ServiceFiles		 )
:		* +

{

 
public 
Guid 
? 
	ServiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
	EventType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
FileName 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_ServiceHardwareChange.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 %
SRV_ServiceHardwareChange		 2
:		3 4

{

 
public 
Guid 
? 
	serviceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

servicePID 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

changeType 
{  
get! $
;$ %
set& )
;) *
}* +
public   
string   
description   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public$$ 
Guid$$ 
?$$ 
	fixtureId$$ 
{$$  
get$$! $
;$$$ %
set$$& )
;$$) *
}$$* +
public(( 
string(( 
FilePath(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
})) 
}** �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_ServiceMaintenance.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 "
SRV_ServiceMaintenance		 /
:		0 1

{

 
public 
Guid 
? 
maintenanceStepId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
	serviceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

servicePID 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
FormText 
{  
get! $
;$ %
set& )
;) *
}* +
public   
string   
FormJson   
{    
get  ! $
;  $ %
set  & )
;  ) *
}  * +
}!! 
}"" �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SRV_ServiceMaintenanceSteps.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 '
SRV_ServiceMaintenanceSteps		 4
:		5 6

{

 
public 
string 
FormText 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
FormJson 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_ChangedDevice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_ChangedDevice )
:* +

{ 
public 
Guid 
? 
oldInventoryId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
newInventoryId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public
Guid
?
	serviceId
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_Customer.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_Customer $
:% &

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
lastName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
phoneNumber !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
otherPhoneNumber &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
openLocationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public   
string   
Address   
{   
get    #
;  # $
set  % (
;  ( )
}  ) *
public!! 
string!! 
code!! 
{!! 
get!!  
;!!  !
set!!" %
;!!% &
}!!& '
public"" 
	IGeometry"" 
location"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}"". /
}## 
}$$ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_CustomerUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_CustomerUser (
:) *

{ 
public 
Guid 
? 
name 
{ 
get 
;  
set! $
;$ %
}% &
public		 
Guid		 
?		 
	serviceId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 

customerId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
} 
}

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_DeviceCameWith.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_DeviceCameWith *
:+ ,

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
amount 
{ 
get  
;  !
set" %
;% &
}& '
public 
bool 
? 
hasLost 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
	serviceId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_DeviceProblem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_DeviceProblem )
:* +

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	serviceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
bool 
? 
warranty 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_Problem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

SV_Problem #
:$ %

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_Service.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

SV_Service #
:$ %

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
Guid		 
?		 
inventoryId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
short

 
?

 
stage

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
short 
? 
deliveryType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string #
deliveryTypeDescription -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public
Guid
?
cargoId
{
get
;
set
;
}
public 
string 
cargoNo 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
customerType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string #
customerTypeDescription -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
	storageId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SV_ServiceOperation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SV_ServiceOperation ,
:- .

{ 
public 
Guid 
? 
	serviceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
CargoId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
CargoNo 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
customerType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
deliveryType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\sysdiagrams.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
sysdiagrams $
:% &

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
int		 
principal_id		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 

diagram_id

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
int 
? 
version 
{ 
get !
;! "
set# &
;& '
}' (
public 
byte 
[ 
] 

definition !
{" #
get$ '
;' (
set) ,
;, -
}- .
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_BlockCalendar.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SYS_BlockCalendar *
:+ ,

{ 
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_BlockMail.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_Email.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	SYS_Email		 "
:		# $

{

 
public 
string 
SendingMail !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
SendingTitle "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
SendingMessage $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
bool 
? 
SendingIsBodyHtml &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
Result 
{ 
get "
;" #
set$ '
;' (
}( )
public"" 
bool"" 
?"" 
Status"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""' (
}## 
}$$ �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_Enums.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	SYS_Enums		 "
:		# $

{

 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
	tableName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	fieldName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
enumKey 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	enumValue 
{  !
get" %
;% &
set' *
;* +
}+ ,
public"" 
string"" 
enumDescription"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
}## 
}$$ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_ExternalLinks.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
SYS_ExternalLinks		 *
:		+ ,

{

 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 
Label 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_Files.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	SYS_Files "
:# $

{ 
public 
string 
	DataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
DataId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
FilePath

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
string 
	FileGroup 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_Notification.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
SYS_Notification )
:* +

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
message 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
url 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 

paramaters  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\SYS_TableAdditionalProperty.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 '
SYS_TableAdditionalProperty		 4
:		5 6

{

 
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
propertyKey !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Airport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

UT_Airport #
:$ %

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Bank.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_Bank		  
:		! "

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Currency.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
UT_Currency $
:% &

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
symbol

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
subName 
{ 
get  #
;# $
set% (
;( )
}) *
public 
bool 
? 
	isLogging 
{  
get! $
;$ %
set& )
;) *
}* +
}
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Location.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_Location		 $
:		% &

{

 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
	IGeometry 
polygon !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
}   �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_LocationConfig.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_LocationConfig		 *
:		+ ,

{

 
public 
string 

configName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

shiftStart  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
shiftEnd 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
dataSendingCount $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
workDays 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public## 
DateTime## 
?## 
endDate##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##, -
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_LocationConfigUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 !
UT_LocationConfigUser		 .
:		/ 0

{

 
public 
Guid 
? 
locationConfigId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
isTrackingActive %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_LocationTracking.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_LocationTracking		 ,
:		- .

{

 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
deviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
	timeStamp 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Rules.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_Rules		 !
:		" #

{

 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
bool 
? 
	isDefault 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_RulesUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_RulesUser		 %
:		& '

{

 
public 
Guid 
? 
rulesId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_RulesUserStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
UT_RulesUserStage		 *
:		+ ,

{

 
public 
Guid 
? 
rulesId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
order 
{ 
get !
;! "
set# &
;& '
}' (
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
roleId 
{ 
get !
;! "
set# &
;& '
}' (
public   
double   
?   
limit   
{   
get   "
;  " #
set  $ '
;  ' (
}  ( )
}!! 
}"" �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Sector.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public		 

partial		 
class		 
	UT_Sector		 "
:		# $

{

 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Template.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
UT_Template $
:% &

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
template 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\UT_Unit.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
UT_Unit  
:! "

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
}		 
}

 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_Activity.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWAPM_Activity '
:( )

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
DateTime

 
?

 
	startDate

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
short
?
generalType
{
get
;
set
;
}
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
communicationType '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
short 
? 
notification "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
generalType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string #
communicationType_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 
notification_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
activityUserIds %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_ActivityAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWAPM_ActivityAction -
:. /

{ 
public 
Guid 
? 

activityId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
activityId_Title
{
get
;
set
;
}
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_ActivityOthers.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWAPM_ActivityOthers -
{ 
public 
int 
type 
{ 
get 
; 
set "
;" #
}# $
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
activityUserIds

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
DateTime 
? 
created  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
endDate
{
get
;
set
;
}
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_ActivityRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWAPM_ActivityRelation /
:0 1

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
DateTime

 
?

 
	startDate

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
short
?
generalType
{
get
;
set
;
}
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
communicationType '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
short 
? 
notification "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
generalType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string #
communicationType_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 
notification_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
activityUserIds %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
dataId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_ActivityRelationTables.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWAPM_ActivityRelationTables 5
{ 
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}" #
public		 
string		 
	dataTable		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
}
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWAPM_ActivityUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWAPM_ActivityUser +
:, -

{ 
public 
Guid 
? 

activityId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
}
} �/
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Company.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public		 
Guid		 
?		 
pid		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
mersisNo 
{  
get! $
;$ %
set& )
;) *
}* +
public
string
email
{
get
;
set
;
}
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
fax 
{ 
get 
;  
set! $
;$ %
}% &
public 
short 
? 
taxType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
	taxOffice 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	taxNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
invoiceAddress $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? $
invoiceAddressLocationId -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 
openAddress !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? !
openAddressLocationId *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
commercialTitle %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 
isActive 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
	pid_Title 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public   
string   
isActive_Title   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  0 1
public!! 
string!! *
invoiceAddressLocationId_Title!! 4
{!!5 6
get!!7 :
;!!: ;
set!!< ?
;!!? @
}!!@ A
public"" 
string"" 
	townTitle"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public## 
string## 
	cityTitle## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
public$$ 
string$$ '
openAddressLocationId_Title$$ 1
{$$2 3
get$$4 7
;$$7 8
set$$9 <
;$$< =
}$$= >
public%% 
string%% 
fullName%% 
{%%  
get%%! $
;%%$ %
set%%& )
;%%) *
}%%* +
public&& 
string&& 
logo&& 
{&& 
get&&  
;&&  !
set&&" %
;&&% &
}&&& '
public'' 
string'' 
Sectors'' 
{'' 
get''  #
;''# $
set''% (
;''( )
}'') *
public(( 
string(( 
CMPTypes_Title(( $
{((% &
get((' *
;((* +
set((, /
;((/ 0
}((0 1
public)) 
string)) 
customerIds)) !
{))" #
get))$ '
;))' (
set))) ,
;)), -
}))- .
public** 
string** 
searchField** !
{**" #
get**$ '
;**' (
set**) ,
;**, -
}**- .
}++ 
},, �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_CompanyCarKilometer.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWCMP_CompanyCarKilometer 2
:3 4

{ 
public 
DateTime 
? 
	entryDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
double		 
?		 
	kilometer		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
	IGeometry

 
location

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
string 
image 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
companyCarId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
Guid
?
commissionId
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
carPlate 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
carBrand 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
carModel 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
responsiblePersonId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �3
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_CompanyCars.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_CompanyCars *
:+ ,

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
companyStorageId		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
string

 
name

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
plate 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
brand 
{ 
get !
;! "
set# &
;& '
}' (
public
string
model
{
get
;
set
;
}
public 
bool 
? 
isHire 
{ 
get !
;! "
set# &
;& '
}' (
public 
DateTime 
? 
contractStartDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
DateTime 
? 
contractEndDate (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string '
vehicleIdentificationNumber 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
string #
vehicleTransitionNumber -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
double 
? 
consumptionFuel &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 
insuranceStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
insuranceEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? %
trafficInsuranceStartDate 2
{3 4
get5 8
;8 9
set: =
;= >
}> ?
public 
DateTime 
? #
trafficInsuranceEndDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
string 
	tradeName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
engineNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
trafficReleaseDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
registrationDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
userName 
{  
get! $
;$ %
set& )
;) *
}* +
public   
int   
?   
fuelType   
{   
get   "
;  " #
set  $ '
;  ' (
}  ( )
public!! 
int!! 
?!! 
gearType!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!( )
public"" 
string"" 
policyNumber"" "
{""# $
get""% (
;""( )
set""* -
;""- .
}"". /
public## 
Guid## 
?## 
responsiblePersonId## (
{##) *
get##+ .
;##. /
set##0 3
;##3 4
}##4 5
public$$ 
string$$ 
createdby_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% %
responsiblePersonId_Title%% /
{%%0 1
get%%2 5
;%%5 6
set%%7 :
;%%: ;
}%%; <
public&& 
string&& 
changedby_Title&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&1 2
public'' 
string'' 
companyId_Title'' %
{''& '
get''( +
;''+ ,
set''- 0
;''0 1
}''1 2
public(( 
string(( "
companyStorageId_Title(( ,
{((- .
get((/ 2
;((2 3
set((4 7
;((7 8
}((8 9
public)) 
double)) 
?)) 

{))% &
get))' *
;))* +
set)), /
;))/ 0
}))0 1
public** 
string** 
isHire_Title** "
{**# $
get**% (
;**( )
set*** -
;**- .
}**. /
public++ 
string++ 
fuelType_Title++ $
{++% &
get++' *
;++* +
set++, /
;++/ 0
}++0 1
},, 
}-- �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_CompanyFileSelector.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWCMP_CompanyFileSelector 2
:3 4

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
string		 
fileGroupModule		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
Guid

 

customerId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
CustomerId_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_CompanyType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_CompanyType *
:+ ,

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
typesId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

{
get
;
set
;
}
} 
} �E
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Document.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_Document '
:( )

{ 
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
serialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
	rowNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
Guid
?

supplierId
{
get
;
set
;
}
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	issueDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 

expiryDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
stopaj 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
discountType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
tevkifat 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
sendingDate $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
paymentType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
installmentCount &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
rateExchange #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
customerTaxNumber '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
customerTaxOffice '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
customerAdress $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
supplierTaxNumber '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public   
string   
supplierTaxOffice   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  3 4
public!! 
string!! 
supplierAdress!! $
{!!% &
get!!' *
;!!* +
set!!, /
;!!/ 0
}!!0 1
public"" 
string"" 

{""$ %
get""& )
;"") *
set""+ .
;"". /
}""/ 0
public## 
Guid## 
?## 
customerStorageId## &
{##' (
get##) ,
;##, -
set##. 1
;##1 2
}##2 3
public$$ 
Guid$$ 
?$$ 
supplierStorageId$$ &
{$$' (
get$$) ,
;$$, -
set$$. 1
;$$1 2
}$$2 3
public%% 
string%% 

type_Title%%  
{%%! "
get%%# &
;%%& '
set%%( +
;%%+ ,
}%%, -
public&& 
string&& 
discountType_Title&& (
{&&) *
get&&+ .
;&&. /
set&&0 3
;&&3 4
}&&4 5
public'' 
string'' 
paymentType_Title'' '
{''( )
get''* -
;''- .
set''/ 2
;''2 3
}''3 4
public(( 
string(( 
direction_Title(( %
{((& '
get((( +
;((+ ,
set((- 0
;((0 1
}((1 2
public)) 
string)) 
createdby_Title)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))1 2
public** 
string** 
changedby_Title** %
{**& '
get**( +
;**+ ,
set**- 0
;**0 1
}**1 2
public++ 
string++ 
Supplier_Title++ $
{++% &
get++' *
;++* +
set++, /
;++/ 0
}++0 1
public,, 
string,, 
Customer_Title,, $
{,,% &
get,,' *
;,,* +
set,,, /
;,,/ 0
},,0 1
public-- 
string-- !
CustomerStorage_Title-- +
{--, -
get--. 1
;--1 2
set--3 6
;--6 7
}--7 8
public.. 
string.. !
SupplierStorage_Title.. +
{.., -
get... 1
;..1 2
set..3 6
;..6 7
}..7 8
public// 
string// 
Currency_Title// $
{//% &
get//' *
;//* +
set//, /
;/// 0
}//0 1
public00 
string00 
Currency_Symbol00 %
{00& '
get00( +
;00+ ,
set00- 0
;000 1
}001 2
public11 
string11 

{11$ %
get11& )
;11) *
set11+ .
;11. /
}11/ 0
public22 
string22 
SerialNumber_Title22 (
{22) *
get22+ .
;22. /
set220 3
;223 4
}224 5
public33 
double33 
?33 
totalTax33 
{33  !
get33" %
;33% &
set33' *
;33* +
}33+ ,
public44 
double44 
?44 
totalSubAmount44 %
{44& '
get44( +
;44+ ,
set44- 0
;440 1
}441 2
public55 
double55 
?55 
totalAmount55 "
{55# $
get55% (
;55( )
set55* -
;55- .
}55. /
public66 
double66 
?66 
totalTaxAsTL66 #
{66$ %
get66& )
;66) *
set66+ .
;66. /
}66/ 0
public77 
double77 
?77 
totalSubAmountAsTL77 )
{77* +
get77, /
;77/ 0
set771 4
;774 5
}775 6
public88 
string88 
fullName88 
{88  
get88! $
;88$ %
set88& )
;88) *
}88* +
public99 
double99 
?99 
totalAmountAsTL99 &
{99' (
get99) ,
;99, -
set99. 1
;991 2
}992 3
}:: 
};; �H
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Invoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 
SerialNumber_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public		 
string		 
fullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
searchField

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
double 
? 
totalAmountAsTL &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
double
?
totalTax
{
get
;
set
;
}
public 
double 
? 
totalSubAmount %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
totalAmount "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
totalTaxAsTL #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
double 
? 
totalSubAmountAsTL )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
serialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
	rowNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

supplierId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	issueDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 

expiryDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
stopaj 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public   
short   
?   
discountType   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
double!! 
?!! 
discountPercent!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
double"" 
?"" 
tevkifat"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public## 
DateTime## 
?## 
sendingDate## $
{##% &
get##' *
;##* +
set##, /
;##/ 0
}##0 1
public$$ 
short$$ 
?$$ 
paymentType$$ !
{$$" #
get$$$ '
;$$' (
set$$) ,
;$$, -
}$$- .
public%% 
short%% 
?%% 
installmentCount%% &
{%%' (
get%%) ,
;%%, -
set%%. 1
;%%1 2
}%%2 3
public&& 
double&& 
?&& 
rateExchange&& #
{&&$ %
get&&& )
;&&) *
set&&+ .
;&&. /
}&&/ 0
public'' 
string'' 
customerTaxNumber'' '
{''( )
get''* -
;''- .
set''/ 2
;''2 3
}''3 4
public(( 
string(( 
customerTaxOffice(( '
{((( )
get((* -
;((- .
set((/ 2
;((2 3
}((3 4
public)) 
string)) 
customerAdress)) $
{))% &
get))' *
;))* +
set)), /
;))/ 0
}))0 1
public** 
string** 

{**$ %
get**& )
;**) *
set**+ .
;**. /
}**/ 0
public++ 
Guid++ 
?++ 
taskId++ 
{++ 
get++ !
;++! "
set++# &
;++& '
}++' (
public,, 
Guid,, 
?,, 
pid,, 
{,, 
get,, 
;,, 
set,,  #
;,,# $
},,$ %
public-- 
string-- 
supplierTaxNumber-- '
{--( )
get--* -
;--- .
set--/ 2
;--2 3
}--3 4
public.. 
string.. 
supplierTaxOffice.. '
{..( )
get..* -
;..- .
set../ 2
;..2 3
}..3 4
public// 
string// 
supplierAdress// $
{//% &
get//' *
;//* +
set//, /
;/// 0
}//0 1
public00 
string00 

{00$ %
get00& )
;00) *
set00+ .
;00. /
}00/ 0
public11 
string11 

type_Title11  
{11! "
get11# &
;11& '
set11( +
;11+ ,
}11, -
public22 
string22 
discountType_Title22 (
{22) *
get22+ .
;22. /
set220 3
;223 4
}224 5
public33 
string33 
status_Title33 "
{33# $
get33% (
;33( )
set33* -
;33- .
}33. /
public44 
string44 
paymentType_Title44 '
{44( )
get44* -
;44- .
set44/ 2
;442 3
}443 4
public55 
string55 
direction_Title55 %
{55& '
get55( +
;55+ ,
set55- 0
;550 1
}551 2
public66 
string66 
createdby_Title66 %
{66& '
get66( +
;66+ ,
set66- 0
;660 1
}661 2
public77 
string77 
changedby_Title77 %
{77& '
get77( +
;77+ ,
set77- 0
;770 1
}771 2
public88 
string88 
Supplier_Title88 $
{88% &
get88' *
;88* +
set88, /
;88/ 0
}880 1
public99 
string99 
Customer_Title99 $
{99% &
get99' *
;99* +
set99, /
;99/ 0
}990 1
public:: 
string:: 
Currency_Title:: $
{::% &
get::' *
;::* +
set::, /
;::/ 0
}::0 1
public;; 
string;; 
Currency_Symbol;; %
{;;& '
get;;( +
;;;+ ,
set;;- 0
;;;0 1
};;1 2
public<< 
string<< 

{<<$ %
get<<& )
;<<) *
set<<+ .
;<<. /
}<</ 0
}== 
}>> �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_InvoiceAction ,
:- .

{ 
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
	invoiceId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
Guid 
? 
transformInvoiceId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdBy_Photo %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWCMP_InvoiceConfirmation 2
:3 4

{ 
public 
Guid 
? 
	advanceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
ruleUserId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
ruleType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
confirmationUserIds )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string &
confirmationUserIds_Titles 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceDocumentTemplate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWCMP_InvoiceDocumentTemplate 6
:7 8

{ 
public 
short 
? "
templateVisibleAllUser ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string (
templateVisibleAllUser_Title 2
{3 4
get5 8
;8 9
set: =
;= >
}> ?
}
} �0
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceItem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_InvoiceItem *
:+ ,

{ 
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	productId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
double

 
?

 
quantity

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public
double
?
KDV
{
get
;
set
;
}
public 
short 
? 
KDVType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
OIV 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
OIVType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
OTV 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
OTVType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
discountType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
	itemOrder 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
alternativeQuantity *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
Guid 
? 
alternativeUnitId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

Unit_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Currency_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Currency_Symbol %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
short   
?   
	stockType   
{    !
get  " %
;  % &
set  ' *
;  * +
}  + ,
public!! 
short!! 
?!! 
invoiceType!! !
{!!" #
get!!$ '
;!!' (
set!!) ,
;!!, -
}!!- .
public"" 
short"" 
?"" 

{""$ %
get""& )
;"") *
set""+ .
;"". /
}""/ 0
public## 
string## 
invoiceType_Title## '
{##( )
get##* -
;##- .
set##/ 2
;##2 3
}##3 4
public$$ 
string$$ 
invoiceStatus_Title$$ )
{$$* +
get$$, /
;$$/ 0
set$$1 4
;$$4 5
}$$5 6
public%% 
double%% 
?%% 
totalSubAmount%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%1 2
public&& 
double&& 
?&& 
totalTax&& 
{&&  !
get&&" %
;&&% &
set&&' *
;&&* +
}&&+ ,
public'' 
double'' 
?'' 
	kdvAmount''  
{''! "
get''# &
;''& '
set''( +
;''+ ,
}'', -
public(( 
double(( 
?(( 
totalAmount(( "
{((# $
get((% (
;((( )
set((* -
;((- .
}((. /
public)) 
string)) 
createdBy_Photo)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))1 2
}** 
}++ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceItemReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWCMP_InvoiceItemReport 0
{ 
public 
DateTime 
? 
created  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
	invoiceId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
	productId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public
double
?
discount
{
get
;
set
;
}
public 
short 
? 
discountType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
	itemOrder 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
	stockType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 

stockCount !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

Unit_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Currency_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Currency_Symbol %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
totalSubAmount %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
totalTax 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 
totalAmount "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �!
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_InvoiceTransform.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWCMP_InvoiceTransform /
:0 1

{ 
public 
string  
SerialNumberTo_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public		 
string		 "
SerialNumberFrom_Title		 ,
{		- .
get		/ 2
;		2 3
set		4 7
;		7 8
}		8 9
public

 
Guid

 
?

 


 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
Guid 
? 
invoiceIdTo  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
short 
? 
typeFrom 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
typeFrom_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
typeTo 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
typeTo_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

supplierId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
Supplier_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
? 
issueDateTo $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
? 

{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
short 
? 
statusTo 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 

statusFrom  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
serialNumberTo $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
serialNumberFrom &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
rowNumberTo !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
}   �G
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Order.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_Order $
:% &

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
short		 
?		 
	direction		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
serialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public
string
	rowNumber
{
get
;
set
;
}
public 
Guid 
? 

supplierId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	issueDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 

expiryDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
sendingDate $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
stopaj 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
discountType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
discountPercent &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
tevkifat 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
paymentType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
rateExchange #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
customerStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
supplierStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
short 
? 
installmentCount &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
customerTaxNumber '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public   
string   
customerTaxOffice   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  3 4
public!! 
string!! 
customerAdress!! $
{!!% &
get!!' *
;!!* +
set!!, /
;!!/ 0
}!!0 1
public"" 
string"" 

{""$ %
get""& )
;"") *
set""+ .
;"". /
}""/ 0
public## 
string## 
supplierTaxNumber## '
{##( )
get##* -
;##- .
set##/ 2
;##2 3
}##3 4
public$$ 
string$$ 
supplierTaxOffice$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public%% 
string%% 
supplierAdress%% $
{%%% &
get%%' *
;%%* +
set%%, /
;%%/ 0
}%%0 1
public&& 
string&& 

{&&$ %
get&&& )
;&&) *
set&&+ .
;&&. /
}&&/ 0
public'' 
string'' 

type_Title''  
{''! "
get''# &
;''& '
set''( +
;''+ ,
}'', -
public(( 
string(( 
discountType_Title(( (
{(() *
get((+ .
;((. /
set((0 3
;((3 4
}((4 5
public)) 
string)) 
paymentType_Title)) '
{))( )
get))* -
;))- .
set))/ 2
;))2 3
}))3 4
public** 
string** 
direction_Title** %
{**& '
get**( +
;**+ ,
set**- 0
;**0 1
}**1 2
public++ 
string++ 
status_Title++ "
{++# $
get++% (
;++( )
set++* -
;++- .
}++. /
public,, 
string,, !
CustomerStorage_Title,, +
{,,, -
get,,. 1
;,,1 2
set,,3 6
;,,6 7
},,7 8
public-- 
string-- !
SupplierStorage_Title-- +
{--, -
get--. 1
;--1 2
set--3 6
;--6 7
}--7 8
public.. 
string.. 
createdby_Title.. %
{..& '
get..( +
;..+ ,
set..- 0
;..0 1
}..1 2
public// 
string// 
changedby_Title// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
string00 
Supplier_Title00 $
{00% &
get00' *
;00* +
set00, /
;00/ 0
}000 1
public11 
string11 
Customer_Title11 $
{11% &
get11' *
;11* +
set11, /
;11/ 0
}110 1
public22 
string22 
Currency_Title22 $
{22% &
get22' *
;22* +
set22, /
;22/ 0
}220 1
public33 
string33 
Currency_Symbol33 %
{33& '
get33( +
;33+ ,
set33- 0
;330 1
}331 2
public44 
string44 

{44$ %
get44& )
;44) *
set44+ .
;44. /
}44/ 0
public55 
double55 
?55 
totalTax55 
{55  !
get55" %
;55% &
set55' *
;55* +
}55+ ,
public66 
double66 
?66 
totalSubAmount66 %
{66& '
get66( +
;66+ ,
set66- 0
;660 1
}661 2
public77 
Guid77 
?77 
presentationId77 #
{77$ %
get77& )
;77) *
set77+ .
;77. /
}77/ 0
public88 
double88 
?88 
totalAmount88 "
{88# $
get88% (
;88( )
set88* -
;88- .
}88. /
public99 
double99 
?99 
totalTaxAsTL99 #
{99$ %
get99& )
;99) *
set99+ .
;99. /
}99/ 0
public:: 
double:: 
?:: 
totalSubAmountAsTL:: )
{::* +
get::, /
;::/ 0
set::1 4
;::4 5
}::5 6
public;; 
double;; 
?;; 
totalAmountAsTL;; &
{;;' (
get;;) ,
;;;, -
set;;. 1
;;;1 2
};;2 3
}<< 
}== �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Request.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
taskId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
	rowNumber
{
get
;
set
;
}
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	issueDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
sendingDate $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
customerAdress $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
customerStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string !
CustomerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
total 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Sector.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_Sector %
:& '

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
sectorId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
sectorId_Title
{
get
;
set
;
}
} 
} �$
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Storage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 
fullName 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
string		 
	pid_Title		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
bool

 
?

 
hasChildren

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Code $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
companyId_Image %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
locationType_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
locationId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
supervisorId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
bool 
? 
	myStorage 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
address 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

locationId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
supervisorId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
fax 
{ 
get 
;  
set! $
;$ %
}% &
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public   
string   
postCode   
{    
get  ! $
;  $ %
set  & )
;  ) *
}  * +
public!! 
short!! 
?!! 
locationType!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!. /
}"" 
}## �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_StorageSection.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWCMP_StorageSection -
:. /

{ 
public 
string 
productCount "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
pid		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
Guid

 
?

 
	companyId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
Guid 
? 
	storageId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public
string
name
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
	pid_Title 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
storageId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �I
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_Tender.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCMP_Tender %
:& '

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
double		 
?		 
totalAmountAsTL		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		2 3
public

 
short

 
?

 
	direction

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
serialNumber
{
get
;
set
;
}
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
	rowNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

supplierId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
	issueDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 

expiryDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
validityDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 
stopaj 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
discountType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
discountPercent &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
tevkifat 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
paymentType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
tenderConditions &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
rateExchange #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
installmentCount &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public   
string   
customerTaxNumber   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  3 4
public!! 
string!! 
customerTaxOffice!! '
{!!( )
get!!* -
;!!- .
set!!/ 2
;!!2 3
}!!3 4
public"" 
string"" 
customerAdress"" $
{""% &
get""' *
;""* +
set"", /
;""/ 0
}""0 1
public## 
string## 

{##$ %
get##& )
;##) *
set##+ .
;##. /
}##/ 0
public$$ 
string$$ 
supplierTaxNumber$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public%% 
string%% 
supplierTaxOffice%% '
{%%( )
get%%* -
;%%- .
set%%/ 2
;%%2 3
}%%3 4
public&& 
string&& 
supplierAdress&& $
{&&% &
get&&' *
;&&* +
set&&, /
;&&/ 0
}&&0 1
public'' 
Guid'' 
?'' %
invoiceDocumentTemplateId'' .
{''/ 0
get''1 4
;''4 5
set''6 9
;''9 :
}'': ;
public(( 
string(( 

{(($ %
get((& )
;(() *
set((+ .
;((. /
}((/ 0
public)) 
Guid)) 
?)) 
taskId)) 
{)) 
get)) !
;))! "
set))# &
;))& '
}))' (
public** 
string** 

type_Title**  
{**! "
get**# &
;**& '
set**( +
;**+ ,
}**, -
public++ 
string++ 
discountType_Title++ (
{++) *
get+++ .
;++. /
set++0 3
;++3 4
}++4 5
public,, 
string,, 
paymentType_Title,, '
{,,( )
get,,* -
;,,- .
set,,/ 2
;,,2 3
},,3 4
public-- 
string-- 
direction_Title-- %
{--& '
get--( +
;--+ ,
set--- 0
;--0 1
}--1 2
public.. 
string.. 
status_Title.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}... /
public// 
string// 
createdby_Title// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
string00 
changedby_Title00 %
{00& '
get00( +
;00+ ,
set00- 0
;000 1
}001 2
public11 
string11 
Supplier_Title11 $
{11% &
get11' *
;11* +
set11, /
;11/ 0
}110 1
public22 
string22 
Customer_Title22 $
{22% &
get22' *
;22* +
set22, /
;22/ 0
}220 1
public33 
string33 
Currency_Title33 $
{33% &
get33' *
;33* +
set33, /
;33/ 0
}330 1
public44 
string44 
Currency_Symbol44 %
{44& '
get44( +
;44+ ,
set44- 0
;440 1
}441 2
public55 
string55 

{55$ %
get55& )
;55) *
set55+ .
;55. /
}55/ 0
public66 
double66 
?66 
totalTax66 
{66  !
get66" %
;66% &
set66' *
;66* +
}66+ ,
public77 
double77 
?77 
totalSubAmount77 %
{77& '
get77( +
;77+ ,
set77- 0
;770 1
}771 2
public88 
Guid88 
?88 
presentationId88 #
{88$ %
get88& )
;88) *
set88+ .
;88. /
}88/ 0
public99 
double99 
?99 
totalAmount99 "
{99# $
get99% (
;99( )
set99* -
;99- .
}99. /
public:: 
double:: 
?:: 
totalTaxAsTL:: #
{::$ %
get::& )
;::) *
set::+ .
;::. /
}::/ 0
public;; 
double;; 
?;; 
totalSubAmountAsTL;; )
{;;* +
get;;, /
;;;/ 0
set;;1 4
;;;4 5
};;5 6
public<< 
string<< 

{<<$ %
get<<& )
;<<) *
set<<+ .
;<<. /
}<</ 0
}== 
}>> �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCMP_TenderTransaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWCMP_TenderTransaction 0
:1 2

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
tenderId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
Guid

 
?

 


 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public
string
code
{
get
;
set
;
}
public 
Guid 
? 
inputId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
outputId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
invoiceId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
outputId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string  
inputCompanyId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string !
outputCompanyId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
	rowNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
	requestId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} � 
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_Contact.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
PresentationId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
DateTime

 
?

 
ContactStartDate

 )
{

* +
get

, /
;

/ 0
set

1 4
;

4 5
}

5 6
public 
DateTime 
? 
ContactEndDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
int
?
ContactType
{
get
;
set
;
}
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
PresentationStageId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string %
PresentationStageId_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string %
PresentationStageId_Color /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string 
ContactType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
ContactStatus_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
int 
? 
TotalContactPerson &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
ContactTime_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
customerId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
Presentation_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 
CustomerCompanyId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
ChannelCompanyId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string #
CustomerCompanyId_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_ContactRelationTables.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWCRM_ContactRelationTables 4
{ 
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}" #
public		 
string		 
	dataTable		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
}
} �(
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_ContactUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCRM_ContactUser *
:+ ,

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
Guid

 
?

 
UserId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
int 
? 
UserType 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
Photo 
{ 
get !
;! "
set# &
;& '
}' (
public
string

User_Title
{
get
;
set
;
}
public 
string 
UserType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
ContactStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
ContactEndDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
ContactType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
PresentationStageId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string %
PresentationStageId_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string %
PresentationStageId_Color /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string 
ContactType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
ContactStatus_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
int 
? 
TotalContactPerson &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
ContactTime_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
customerId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public   
string   
Presentation_Title   (
{  ) *
get  + .
;  . /
set  0 3
;  3 4
}  4 5
public!! 
Guid!! 
?!! 
CustomerCompanyId!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
Guid"" 
?"" 
ChannelCompanyId"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
string## #
CustomerCompanyId_Title## -
{##. /
get##0 3
;##3 4
set##5 8
;##8 9
}##9 :
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_ManagerStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCRM_ManagerStage +
:, -

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
Code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
Description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
IsSalesCompleted %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTarget.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCRM_MonthlyTarget ,
:- .

{ 
public 
DateTime 
? 
CPeriod  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
int		 
?		 
TargetGroupType		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
Guid

 
?

 
ProductGroupId

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
int 
? 
TargetPoint 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public
int
?
BonusPercentage
{
get
;
set
;
}
public 
bool 
? 
IsFocus 
{ 
get "
;" #
set$ '
;' (
}( )
public 
bool 
? 
IsMultipleFocus $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
RowId 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
GroupId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ProductGroup_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTargetGrouped.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
VWCRM_MonthlyTargetGrouped 3
:4 5

{ 
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
string		 
changedby_Title		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
DateTime

 
?

 
CPeriod

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTargetGroupedPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 0
$VWCRM_MonthlyTargetGroupedPageReport =
{ 
public 
int 
TotalTarget 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
int		 
TotalProduct		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 
TotalProductFocused

 &
{

' (
get

) ,
;

, -
set

. 1
;

1 2
}

2 3
public 
int "
TotalProductNotFocused )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTargetGroupedPersonPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 6
*VWCRM_MonthlyTargetGroupedPersonPageReport C
{ 
public 
int 
TotalTarget 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
int		 
TotalProduct		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 
TotalProductFocused

 &
{

' (
get

) ,
;

, -
set

. 1
;

1 2
}

2 3
public 
int "
TotalProductNotFocused )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTargetPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWCRM_MonthlyTargetPerson 2
:3 4

{ 
public 
DateTime 
? 
CPeriod  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
TargetUserId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
Guid

 
?

 
ProductGroupId

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
int 
? 
TargetPoint 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public
int
?
BonusPercentage
{
get
;
set
;
}
public 
bool 
? 
IsFocus 
{ 
get "
;" #
set$ '
;' (
}( )
public 
bool 
? 
IsMultipleFocus $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
RowId 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
GroupId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ProductGroup_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
TargetUser_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_MonthlyTargetPersonGrouped.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class ,
 VWCRM_MonthlyTargetPersonGrouped 9
:: ;

{ 
public 
Guid 
? 
TargetUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
DateTime		 
?		 
CPeriod		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
TargetUser_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PerformanceMultiplier.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWCRM_PerformanceMultiplier 4
:5 6

{ 
public 
Guid 
? 
GroupId 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
int 
? 
TargetGroupType #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
ProductGroupId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
int
?

{
get
;
set
;
}
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Point 
{ 
get 
;  
set! $
;$ %
}% &
public 
bool 
? 
IsFocus 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string !
TargetGroupType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
ProductGroup_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PerformanceMultiplierGrouped.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class .
"VWCRM_PerformanceMultiplierGrouped ;
:< =

{ 
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
string		 
changedby_Title		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
DateTime

 
?

 
	StartDate

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PerformanceMultiplierGroupedPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 8
,VWCRM_PerformanceMultiplierGroupedPageReport E
{ 
public 
int 
TotalMultiplier "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
int		 
TotalProduct		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 
TotalProductFocused

 &
{

' (
get

) ,
;

, -
set

. 1
;

1 2
}

2 3
public 
int "
TotalProductNotFocused )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_Presentation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWCRM_Presentation +
:, -

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
Name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 


 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
Guid 
? 
ChannelCompanyId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
CustomerCompanyId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
Guid
?
PresentationStageId
{
get
;
set
;
}
public 
DateTime 
? 
CommitmentEndDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
double 
? !
OpponentInvoiceAmount ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
double 
? 
CompletionRate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? #
EstimatedCompletionDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
long 
? 
Budget 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 

hasContact 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
PlaceofArrival $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
SalesPerson_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
ChannelCompany_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string !
CustomerCompany_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
	cityTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	townTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string !
CustomerCompany_Phone +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
	IGeometry #
CustomerCompanyLocation 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public   
int   
?   
DaysSinceVisit   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
double!! 
?!! 
TotalPoints!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!. /
public"" 
string"" 
Stage_Color"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public## 
string## 
Stage_Title## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public$$ 
DateTime$$ 
?$$ (
PresentationStageChangedDate$$ 5
{$$6 7
get$$8 ;
;$$; <
set$$= @
;$$@ A
}$$A B
public%% 
int%% 
?%% 
TotalContact%%  
{%%! "
get%%# &
;%%& '
set%%( +
;%%+ ,
}%%, -
public&& 
string&& 
LastDescription&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&1 2
public'' 
string'' 

LastStatus''  
{''! "
get''# &
;''& '
set''( +
;''+ ,
}'', -
public(( 
string((  
PlaceofArrival_Title(( *
{((+ ,
get((- 0
;((0 1
set((2 5
;((5 6
}((6 7
public)) 
DateTime)) 
?)) 
Last_ContactDate)) )
{))* +
get)), /
;))/ 0
set))1 4
;))4 5
}))5 6
public** 
DateTime** 
?** 
LastActivityDate** )
{*** +
get**, /
;**/ 0
set**1 4
;**4 5
}**5 6
public++ 
string++ '
LastActivityCreatedBy_Title++ 1
{++2 3
get++4 7
;++7 8
set++9 <
;++< =
}++= >
public,, 
string,, 
Product_Titles,, $
{,,% &
get,,' *
;,,* +
set,,, /
;,,/ 0
},,0 1
public-- 
double-- 
?-- !
lastTenderTotalAmount-- ,
{--- .
get--/ 2
;--2 3
set--4 7
;--7 8
}--8 9
public.. 
string.. 
createdByPhoto.. $
{..% &
get..' *
;..* +
set.., /
;../ 0
}..0 1
public// 
string// 
salesPersonPhoto// &
{//' (
get//) ,
;//, -
set//. 1
;//1 2
}//2 3
}00 
}11 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PresentationAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWCRM_PresentationAction 1
:2 3

{ 
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
presentationId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
color 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
	contactId 
{  
get! $
;$ %
set& )
;) *
}* +
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PresentationInvoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWCRM_PresentationInvoice 2
:3 4

{ 
public 
Guid 
? 
presentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	invoiceId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
type

 
{

 
get

 
;

 
set

  #
;

# $
}

$ %
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
Presentation_Title
{
get
;
set
;
}
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PresentationOpponentCompany.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class -
!VWCRM_PresentationOpponentCompany :
:; <

{ 
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
OpponentCompanyId		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		2 3
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string !
OpponentCompany_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public
string
Presentation_Title
{
get
;
set
;
}
public 
string 
LastStage_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
LastStage_Color %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
CurrentPoint "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWCRM_PresentationProducts.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
VWCRM_PresentationProducts 3
:4 5

{ 
public 
Guid 
? 
PresentationId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	ProductId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
Amount

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
bool 
? 
IsNew 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
logo 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Presentation_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 

unit_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
sellingPrice #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
double 
? 
CurrentPoint #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWDOC_Document.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWDOC_Document '
:( )

{ 
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
subject		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
responsibleUserId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
string

{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string #
responsibleUserId_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWDOC_DocumentFlow.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWDOC_DocumentFlow +
:, -

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
organizationUnitId

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
short 
? 
order 
{ 
get !
;! "
set# &
;& '
}' (
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string $
organizationUnitId_Title .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWDOC_DocumentRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWDOC_DocumentRelation /
:0 1

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
documentRelationId		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string  
documentRelationCode *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public
string
documentRelationTitle
{
get
;
set
;
}
public 
string #
documentRelationSubject -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWDOC_DocumentRevisionRequest.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWDOC_DocumentRevisionRequest 6
:7 8

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
revisionUserId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
revisionNumber

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
public 
string 
revisionContent %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
documentCode "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
documentSubject %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 

lastStatus  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
confirmationUserIds )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string "
confirmationUserTitles ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string 
lastStatus_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWDOC_DocumentVersion.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWDOC_DocumentVersion .
:/ 0

{ 
public 
Guid 
? 

documentId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
string		 

{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
content

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
bool 
? 
isActive 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
documentCode "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_SubTypeTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_SubTypeTask *
:+ ,

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	subTypeId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
subTypeId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
}
} �o
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_Task.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

VWFTM_Task #
:$ %

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
string		 
searchField		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
int

 
	SLAStatus

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
scopeLevelId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
typeLevelId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
string
resolutionDate_Title
{
get
;
set
;
}
public 
string 
closingDate_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
scopeId_levelTitle (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string  
subjectId_LevelTitle *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
DateTime 
? 
penaltyStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
double 
? 
amercementTotal &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
SLAText 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 

amercement  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
resolutionTime $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
resolutionType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string ,
 customer_openAddressCityId_Title 6
{7 8
get9 <
;< =
set> A
;A B
}B C
public 
Guid 
? &
customer_openAddressCityId /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
taskPlanId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string !
customerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
assignableUserIds '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
assignableUserTitles *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public   
string   

{  $ %
get  & )
;  ) *
set  + .
;  . /
}  / 0
public!! 
string!! 
helperUserTitles!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
string"" !
taskSubjectType_Title"" +
{"", -
get"". 1
;""1 2
set""3 6
;""6 7
}""7 8
public## 
bool## 
?## 

isComplete## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
public$$ 
int$$ 
?$$ 
transactionCount$$ $
{$$% &
get$$' *
;$$* +
set$$, /
;$$/ 0
}$$0 1
public%% 
string%%  
operationDescription%% *
{%%+ ,
get%%- 0
;%%0 1
set%%2 5
;%%5 6
}%%6 7
public&& 
int&& 
?&& (
SahaOperasyonGorevFormSayisi&& 0
{&&1 2
get&&3 6
;&&6 7
set&&8 ;
;&&; <
}&&< =
public'' 
int'' 
?'' 
IslemSayisi'' 
{''  !
get''" %
;''% &
set''' *
;''* +
}''+ ,
public(( 
int(( 
?(( %
DoldurulanGorevFormSayisi(( -
{((. /
get((0 3
;((3 4
set((5 8
;((8 9
}((9 :
public)) 
int)) 
?)) ,
 MemnuniyetAnketiDoldurulmaSayisi)) 4
{))5 6
get))7 :
;)): ;
set))< ?
;))? @
}))@ A
public** 
Guid** 
?** 
memnuniyetAnketId** &
{**' (
get**) ,
;**, -
set**. 1
;**1 2
}**2 3
public++ 
int++ 
?++ 
DosyaSayisi++ 
{++  !
get++" %
;++% &
set++' *
;++* +
}+++ ,
public,, 
string,, 
workingHour,, !
{,," #
get,,$ '
;,,' (
set,,) ,
;,,, -
},,- .
public-- 
int-- 
?-- 
totalworkingHour-- $
{--% &
get--' *
;--* +
set--, /
;--/ 0
}--0 1
public.. 
string.. 
plate.. 
{.. 
get.. !
;..! "
set..# &
;..& '
}..' (
public// 
string// 
stopSubject_Titles// (
{//) *
get//+ .
;//. /
set//0 3
;//3 4
}//4 5
public00 
Guid00 
?00 
	projectId00 
{00  
get00! $
;00$ %
set00& )
;00) *
}00* +
public11 
string11 
assignUserPhoto11 %
{11& '
get11( +
;11+ ,
set11- 0
;110 1
}111 2
public22 
string22 
code22 
{22 
get22  
;22  !
set22" %
;22% &
}22& '
public33 
short33 
?33 
type33 
{33 
get33  
;33  !
set33" %
;33% &
}33& '
public44 
Guid44 
?44 
	companyId44 
{44  
get44! $
;44$ %
set44& )
;44) *
}44* +
public55 
bool55 
?55 

{55# $
get55% (
;55( )
set55* -
;55- .
}55. /
public66 
Guid66 
?66 
	fixtureId66 
{66  
get66! $
;66$ %
set66& )
;66) *
}66* +
public77 
	IGeometry77 
location77 "
{77# $
get77% (
;77( )
set77* -
;77- .
}77. /
public88 
short88 
?88 
priority88 
{88  
get88! $
;88$ %
set88& )
;88) *
}88* +
public99 
Guid99 
?99 

customerId99 
{99  !
get99" %
;99% &
set99' *
;99* +
}99+ ,
public:: 
DateTime:: 
?:: 
dueDate::  
{::! "
get::# &
;::& '
set::( +
;::+ ,
}::, -
public;; 
Guid;; 
?;; 
customerStorageId;; &
{;;' (
get;;) ,
;;;, -
set;;. 1
;;;1 2
};;2 3
public<< 
DateTime<< 
?<< 

{<<' (
get<<) ,
;<<, -
set<<. 1
;<<1 2
}<<2 3
public== 
DateTime== 
?== 
notificationDate== )
{==* +
get==, /
;==/ 0
set==1 4
;==4 5
}==5 6
public>> 
Guid>> 
?>> 
companyCarId>> !
{>>" #
get>>$ '
;>>' (
set>>) ,
;>>, -
}>>- .
public?? 
Guid?? 
??? 

taskPlanId?? 
{??  !
get??" %
;??% &
set??' *
;??* +
}??+ ,
public@@ 
Guid@@ 
?@@ 
taskTemplateId@@ #
{@@$ %
get@@& )
;@@) *
set@@+ .
;@@. /
}@@/ 0
publicAA 
shortAA 
?AA 
	planLaterAA 
{AA  !
getAA" %
;AA% &
setAA' *
;AA* +
}AA+ ,
publicBB 
stringBB 
company_CodeBB "
{BB# $
getBB% (
;BB( )
setBB* -
;BB- .
}BB. /
publicCC 
stringCC 

{CC$ %
getCC& )
;CC) *
setCC+ .
;CC. /
}CC/ 0
publicDD 
stringDD 
customer_TitleDD $
{DD% &
getDD' *
;DD* +
setDD, /
;DD/ 0
}DD0 1
publicEE 
GuidEE 
?EE *
customer_openAddressLocationIdEE 3
{EE4 5
getEE6 9
;EE9 :
setEE; >
;EE> ?
}EE? @
publicFF 
GuidFF 
?FF 1
%customerStorage_openAddressLocationIdFF :
{FF; <
getFF= @
;FF@ A
setFFB E
;FFE F
}FFF G
publicGG 
stringGG 
customer_OpenAdressGG )
{GG* +
getGG, /
;GG/ 0
setGG1 4
;GG4 5
}GG5 6
publicHH 
DateTimeHH 
?HH 
resolutionDateHH '
{HH( )
getHH* -
;HH- .
setHH/ 2
;HH2 3
}HH3 4
publicII 
DateTimeII 
?II 
rejectedDateII %
{II& '
getII( +
;II+ ,
setII- 0
;II0 1
}II1 2
publicJJ 
DateTimeJJ 
?JJ 
closingDateJJ $
{JJ% &
getJJ' *
;JJ* +
setJJ, /
;JJ/ 0
}JJ0 1
publicKK 
stringKK %
lastOperationStatus_TitleKK /
{KK0 1
getKK2 5
;KK5 6
setKK7 :
;KK: ;
}KK; <
publicLL 
intLL 
?LL 
lastOperationStatusLL '
{LL( )
getLL* -
;LL- .
setLL/ 2
;LL2 3
}LL3 4
publicMM 
	IGeometryMM !
lastOperationLocationMM /
{MM0 1
getMM2 5
;MM5 6
setMM7 :
;MM: ;
}MM; <
publicNN 
DateTimeNN 
?NN 
lastOperationDateNN *
{NN+ ,
getNN- 0
;NN0 1
setNN2 5
;NN5 6
}NN6 7
publicOO 
stringOO 
descriptionOO !
{OO" #
getOO$ '
;OO' (
setOO) ,
;OO, -
}OO- .
publicPP 
GuidPP 
?PP 
assignUserIdPP !
{PP" #
getPP$ '
;PP' (
setPP) ,
;PP, -
}PP- .
publicQQ 
stringQQ 
assignUser_TitleQQ &
{QQ' (
getQQ) ,
;QQ, -
setQQ. 1
;QQ1 2
}QQ2 3
publicRR 
boolRR 
?RR 

hasProblemRR 
{RR  !
getRR" %
;RR% &
setRR' *
;RR* +
}RR+ ,
publicSS 
shortSS 
?SS 
locationTypeSS "
{SS# $
getSS% (
;SS( )
setSS* -
;SS- .
}SS. /
publicTT 
stringTT 
createdby_TitleTT %
{TT& '
getTT( +
;TT+ ,
setTT- 0
;TT0 1
}TT1 2
publicUU 
stringUU 
changedby_TitleUU %
{UU& '
getUU( +
;UU+ ,
setUU- 0
;UU0 1
}UU1 2
publicVV 
stringVV 

type_TitleVV  
{VV! "
getVV# &
;VV& '
setVV( +
;VV+ ,
}VV, -
publicWW 
stringWW 
planLater_TitleWW %
{WW& '
getWW( +
;WW+ ,
setWW- 0
;WW0 1
}WW1 2
publicXX 
stringXX 
priority_TitleXX $
{XX% &
getXX' *
;XX* +
setXX, /
;XX/ 0
}XX0 1
}YY 
}ZZ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskAuthority.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskAuthority ,
:- .

{ 
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 

customerId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
Guid

 
?

 
userId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
userId_Title
{
get
;
set
;
}
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
customerId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskFollowUpUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWFTM_TaskFollowUpUser /
:0 1

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskForm '
:( )

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
json 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
isActive 
{  
get! $
;$ %
set& )
;) *
}* +
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
isActive_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 
	usedCount 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string "
companyStorageId_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string 
inventories_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskFormRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWFTM_TaskFormRelation /
:0 1

{ 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
	productId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
formId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
companyStorageId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
inventoryId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string  
companyStorage_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
formId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
formId_Type !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskFormResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWFTM_TaskFormResult -
:. /

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
taskOperationId		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		0 1
public

 
Guid

 
?

 
formId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 

jsonResult  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
	task_Name 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

form_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
	fixtureId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
customerStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �M
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskGrid.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskGrid '
:( )

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public
Guid
?
	fixtureId
{
get
;
set
;
}
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
dueDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
customerStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 

{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 
notificationDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
Guid 
? 
companyCarId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 

taskPlanId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
	planLater 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdTime !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
planStartDateTime '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
dueDateTime !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string !
customerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public   
string   

{  $ %
get  & )
;  ) *
set  + .
;  . /
}  / 0
public!! 
string!! 

{!!$ %
get!!& )
;!!) *
set!!+ .
;!!. /
}!!/ 0
public"" 
string"" 
taskType_Title"" $
{""% &
get""' *
;""* +
set"", /
;""/ 0
}""0 1
public## 
string## 
town## 
{## 
get##  
;##  !
set##" %
;##% &
}##& '
public$$ 
string$$ 
city$$ 
{$$ 
get$$  
;$$  !
set$$" %
;$$% &
}$$& '
public%% 
string%% 
description%% !
{%%" #
get%%$ '
;%%' (
set%%) ,
;%%, -
}%%- .
public&& 
string&& 
personelDescription&& )
{&&* +
get&&, /
;&&/ 0
set&&1 4
;&&4 5
}&&5 6
public'' 
string'' !
DurdurulmaDescription'' +
{'', -
get''. 1
;''1 2
set''3 6
;''6 7
}''7 8
public(( 
string(( 

{(($ %
get((& )
;(() *
set((+ .
;((. /
}((/ 0
public)) 
string)) 
helperUserTitles)) &
{))' (
get))) ,
;)), -
set)). 1
;))1 2
}))2 3
public** 
string** %
lastOperationStatus_Title** /
{**0 1
get**2 5
;**5 6
set**7 :
;**: ;
}**; <
public++ 
int++ 
?++ 
lastOperationStatus++ '
{++( )
get++* -
;++- .
set++/ 2
;++2 3
}++3 4
public,, 
int,, 
?,, ,
 MemnuniyetAnketiDoldurulmaSayisi,, 4
{,,5 6
get,,7 :
;,,: ;
set,,< ?
;,,? @
},,@ A
public-- 
string-- '
openAddressLocationId_Title-- 1
{--2 3
get--4 7
;--7 8
set--9 <
;--< =
}--= >
public.. 
Guid.. 
?.. 
assignUserId.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}..- .
public// 
DateTime// 
?// 
lastOperationDate// *
{//+ ,
get//- 0
;//0 1
set//2 5
;//5 6
}//6 7
public00 
int00 
?00 (
SahaOperasyonGorevFormSayisi00 0
{001 2
get003 6
;006 7
set008 ;
;00; <
}00< =
public11 
int11 
?11 %
DoldurulanGorevFormSayisi11 -
{11. /
get110 3
;113 4
set115 8
;118 9
}119 :
public22 
int22 
?22 
DosyaSayisi22 
{22  !
get22" %
;22% &
set22' *
;22* +
}22+ ,
public33 
DateTime33 
?33 
GorevAtamaTarihi33 )
{33* +
get33, /
;33/ 0
set331 4
;334 5
}335 6
public44 
DateTime44 
?44 !
GorevUstlenilmeTarihi44 .
{44/ 0
get441 4
;444 5
set446 9
;449 :
}44: ;
public55 
DateTime55 
?55  
GorevBaslangicTarihi55 -
{55. /
get550 3
;553 4
set555 8
;558 9
}559 :
public66 
DateTime66 
?66 
GorevBitisTarihi66 )
{66* +
get66, /
;66/ 0
set661 4
;664 5
}665 6
public77 
DateTime77 
?77 
DurdurulmaTarihi77 )
{77* +
get77, /
;77/ 0
set771 4
;774 5
}775 6
public88 
DateTime88 
?88 
DevamTarihi88 $
{88% &
get88' *
;88* +
set88, /
;88/ 0
}880 1
public99 
string99 
assignableUserIds99 '
{99( )
get99* -
;99- .
set99/ 2
;992 3
}993 4
public:: 
bool:: 
?:: 

isComplete:: 
{::  !
get::" %
;::% &
set::' *
;::* +
}::+ ,
public;; 
string;; 
assignUser_Title;; &
{;;' (
get;;) ,
;;;, -
set;;. 1
;;;1 2
};;2 3
public<< 
string<< 
	GecenSure<< 
{<<  !
get<<" %
;<<% &
set<<' *
;<<* +
}<<+ ,
public== 
string== 
	CevapSure== 
{==  !
get==" %
;==% &
set==' *
;==* +
}==+ ,
public>> 
string>> "
gorevBitisTarihiZamani>> ,
{>>- .
get>>/ 2
;>>2 3
set>>4 7
;>>7 8
}>>8 9
public?? 
string?? 
	groupName?? 
{??  !
get??" %
;??% &
set??' *
;??* +
}??+ ,
}@@ 
}AA �`
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskNew.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
string		 
searchField		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
int

 
	SLAStatus

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
scopeLevelId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
typeLevelId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
string
resolutionDate_Title
{
get
;
set
;
}
public 
string 
closingDate_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
DateTime 
? 
penaltyStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
double 
? 
amercementTotal &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
SLAText 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 

amercement  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
resolutionTime $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
resolutionType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string ,
 customer_openAddressCityId_Title 6
{7 8
get9 <
;< =
set> A
;A B
}B C
public 
Guid 
? &
customer_openAddressCityId /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string !
customerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
assignableUserIds '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
assignableUserTitles *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
helperUserTitles &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string !
taskSubjectType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
bool 
? 

isComplete 
{  !
get" %
;% &
set' *
;* +
}+ ,
public   
string    
operationDescription   *
{  + ,
get  - 0
;  0 1
set  2 5
;  5 6
}  6 7
public!! 
int!! 
?!! (
SahaOperasyonGorevFormSayisi!! 0
{!!1 2
get!!3 6
;!!6 7
set!!8 ;
;!!; <
}!!< =
public"" 
int"" 
?"" 
IslemSayisi"" 
{""  !
get""" %
;""% &
set""' *
;""* +
}""+ ,
public## 
int## 
?## %
DoldurulanGorevFormSayisi## -
{##. /
get##0 3
;##3 4
set##5 8
;##8 9
}##9 :
public$$ 
int$$ 
?$$ 
DosyaSayisi$$ 
{$$  !
get$$" %
;$$% &
set$$' *
;$$* +
}$$+ ,
public%% 
string%% 
workingHour%% !
{%%" #
get%%$ '
;%%' (
set%%) ,
;%%, -
}%%- .
public&& 
string&& 
plate&& 
{&& 
get&& !
;&&! "
set&&# &
;&&& '
}&&' (
public'' 
string'' 
stopSubject_Titles'' (
{'') *
get''+ .
;''. /
set''0 3
;''3 4
}''4 5
public(( 
Guid(( 
?(( 
	projectId(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
public)) 
string)) 
code)) 
{)) 
get))  
;))  !
set))" %
;))% &
}))& '
public** 
short** 
?** 
type** 
{** 
get**  
;**  !
set**" %
;**% &
}**& '
public++ 
Guid++ 
?++ 
	companyId++ 
{++  
get++! $
;++$ %
set++& )
;++) *
}++* +
public,, 
bool,, 
?,, 

{,,# $
get,,% (
;,,( )
set,,* -
;,,- .
},,. /
public-- 
Guid-- 
?-- 
	fixtureId-- 
{--  
get--! $
;--$ %
set--& )
;--) *
}--* +
public.. 
	IGeometry.. 
location.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}... /
public// 
short// 
?// 
priority// 
{//  
get//! $
;//$ %
set//& )
;//) *
}//* +
public00 
Guid00 
?00 

customerId00 
{00  !
get00" %
;00% &
set00' *
;00* +
}00+ ,
public11 
DateTime11 
?11 
dueDate11  
{11! "
get11# &
;11& '
set11( +
;11+ ,
}11, -
public22 
Guid22 
?22 
customerStorageId22 &
{22' (
get22) ,
;22, -
set22. 1
;221 2
}222 3
public33 
DateTime33 
?33 

{33' (
get33) ,
;33, -
set33. 1
;331 2
}332 3
public44 
DateTime44 
?44 
notificationDate44 )
{44* +
get44, /
;44/ 0
set441 4
;444 5
}445 6
public55 
Guid55 
?55 
companyCarId55 !
{55" #
get55$ '
;55' (
set55) ,
;55, -
}55- .
public66 
Guid66 
?66 

taskPlanId66 
{66  !
get66" %
;66% &
set66' *
;66* +
}66+ ,
public77 
Guid77 
?77 
taskTemplateId77 #
{77$ %
get77& )
;77) *
set77+ .
;77. /
}77/ 0
public88 
short88 
?88 
	planLater88 
{88  !
get88" %
;88% &
set88' *
;88* +
}88+ ,
public99 
string99 
company_Code99 "
{99# $
get99% (
;99( )
set99* -
;99- .
}99. /
public:: 
string:: 

{::$ %
get::& )
;::) *
set::+ .
;::. /
}::/ 0
public;; 
string;; 
customer_Title;; $
{;;% &
get;;' *
;;;* +
set;;, /
;;;/ 0
};;0 1
public<< 
Guid<< 
?<< *
customer_openAddressLocationId<< 3
{<<4 5
get<<6 9
;<<9 :
set<<; >
;<<> ?
}<<? @
public== 
Guid== 
?== 1
%customerStorage_openAddressLocationId== :
{==; <
get=== @
;==@ A
set==B E
;==E F
}==F G
public>> 
string>> 
customer_OpenAdress>> )
{>>* +
get>>, /
;>>/ 0
set>>1 4
;>>4 5
}>>5 6
public?? 
DateTime?? 
??? 
resolutionDate?? '
{??( )
get??* -
;??- .
set??/ 2
;??2 3
}??3 4
public@@ 
DateTime@@ 
?@@ 
rejectedDate@@ %
{@@& '
get@@( +
;@@+ ,
set@@- 0
;@@0 1
}@@1 2
publicAA 
DateTimeAA 
?AA 
closingDateAA $
{AA% &
getAA' *
;AA* +
setAA, /
;AA/ 0
}AA0 1
publicBB 
stringBB %
lastOperationStatus_TitleBB /
{BB0 1
getBB2 5
;BB5 6
setBB7 :
;BB: ;
}BB; <
publicCC 
intCC 
?CC 
lastOperationStatusCC '
{CC( )
getCC* -
;CC- .
setCC/ 2
;CC2 3
}CC3 4
publicDD 
	IGeometryDD !
lastOperationLocationDD /
{DD0 1
getDD2 5
;DD5 6
setDD7 :
;DD: ;
}DD; <
publicEE 
DateTimeEE 
?EE 
lastOperationDateEE *
{EE+ ,
getEE- 0
;EE0 1
setEE2 5
;EE5 6
}EE6 7
publicFF 
stringFF 
descriptionFF !
{FF" #
getFF$ '
;FF' (
setFF) ,
;FF, -
}FF- .
publicGG 
GuidGG 
?GG 
assignUserIdGG !
{GG" #
getGG$ '
;GG' (
setGG) ,
;GG, -
}GG- .
publicHH 
stringHH 
assignUser_TitleHH &
{HH' (
getHH) ,
;HH, -
setHH. 1
;HH1 2
}HH2 3
publicII 
boolII 
?II 

hasProblemII 
{II  !
getII" %
;II% &
setII' *
;II* +
}II+ ,
publicJJ 
stringJJ 
createdby_TitleJJ %
{JJ& '
getJJ( +
;JJ+ ,
setJJ- 0
;JJ0 1
}JJ1 2
publicKK 
stringKK 
changedby_TitleKK %
{KK& '
getKK( +
;KK+ ,
setKK- 0
;KK0 1
}KK1 2
publicLL 
stringLL 

type_TitleLL  
{LL! "
getLL# &
;LL& '
setLL( +
;LL+ ,
}LL, -
publicMM 
stringMM 
priority_TitleMM $
{MM% &
getMM' *
;MM* +
setMM, /
;MM/ 0
}MM0 1
}NN 
}OO �@
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskOld.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
string		 
searchField		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 


 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
string 
customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string !
customerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public
string

{
get
;
set
;
}
public 
Guid 
? 
assignUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
assignableUserIds '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
assignableUserTitles *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
helperUserTitles &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string !
taskSubjectType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
bool 
? 

isComplete 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
lastOperationStatus '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
	IGeometry !
lastOperationLocation /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
DateTime 
? 
lastOperationDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string  
operationDescription *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
int 
? (
SahaOperasyonGorevFormSayisi 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
int 
? 
IslemSayisi 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? %
DoldurulanGorevFormSayisi -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
int 
? 
DosyaSayisi 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
workingHour !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
plate 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
stopSubject_Titles (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public   
string   %
lastOperationStatus_Title   /
{  0 1
get  2 5
;  5 6
set  7 :
;  : ;
}  ; <
public!! 
string!! 
assignUser_Title!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
string"" 
code"" 
{"" 
get""  
;""  !
set""" %
;""% &
}""& '
public## 
short## 
?## 
type## 
{## 
get##  
;##  !
set##" %
;##% &
}##& '
public$$ 
Guid$$ 
?$$ 
	companyId$$ 
{$$  
get$$! $
;$$$ %
set$$& )
;$$) *
}$$* +
public%% 
bool%% 
?%% 

{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%. /
public&& 
Guid&& 
?&& 
	fixtureId&& 
{&&  
get&&! $
;&&$ %
set&&& )
;&&) *
}&&* +
public'' 
	IGeometry'' 
location'' "
{''# $
get''% (
;''( )
set''* -
;''- .
}''. /
public(( 
short(( 
?(( 
priority(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
public)) 
Guid)) 
?)) 

customerId)) 
{))  !
get))" %
;))% &
set))' *
;))* +
}))+ ,
public** 
DateTime** 
?** 
dueDate**  
{**! "
get**# &
;**& '
set**( +
;**+ ,
}**, -
public++ 
Guid++ 
?++ 
customerStorageId++ &
{++' (
get++) ,
;++, -
set++. 1
;++1 2
}++2 3
public,, 
DateTime,, 
?,, 

{,,' (
get,,) ,
;,,, -
set,,. 1
;,,1 2
},,2 3
public-- 
DateTime-- 
?-- 
notificationDate-- )
{--* +
get--, /
;--/ 0
set--1 4
;--4 5
}--5 6
public.. 
Guid.. 
?.. 
companyCarId.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}..- .
public// 
Guid// 
?// 

taskPlanId// 
{//  !
get//" %
;//% &
set//' *
;//* +
}//+ ,
public00 
Guid00 
?00 
taskTemplateId00 #
{00$ %
get00& )
;00) *
set00+ .
;00. /
}00/ 0
public11 
short11 
?11 
	planLater11 
{11  !
get11" %
;11% &
set11' *
;11* +
}11+ ,
public22 
string22 
createdby_Title22 %
{22& '
get22( +
;22+ ,
set22- 0
;220 1
}221 2
public33 
string33 
changedby_Title33 %
{33& '
get33( +
;33+ ,
set33- 0
;330 1
}331 2
public44 
string44 

type_Title44  
{44! "
get44# &
;44& '
set44( +
;44+ ,
}44, -
public55 
string55 
priority_Title55 $
{55% &
get55' *
;55* +
set55, /
;55/ 0
}550 1
}66 
}77 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskOperation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskOperation ,
:- .

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
int

 
?

 
status

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public
Guid
?
	fixtureId
{
get
;
set
;
}
public 
double 
? 
battery 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
subject 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

user_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
passingTime !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdPhoto "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
distance 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	task_Name 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
formResultId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
formId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 

jsonResult  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskPlan '
:( )

{ 
public 
bool 
? 
enabled 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
DateTime

 
?

 
frequencyStartDate

 +
{

, -
get

. 1
;

1 2
set

3 6
;

6 7
}

7 8
public 
DateTime 
? 
frequencyEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
int 
? 
	frequency 
{ 
get  #
;# $
set% (
;( )
}) *
public
int
?
frequencyInterval
{
get
;
set
;
}
public 
int 
? 
taskCreationTime $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
times 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
weekDays 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
	monthDays 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

templateId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 

task_Count 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
frequency_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string "
taskCreationTime_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string 
templateId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskSubject.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskSubject *
:+ ,

{ 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
int

 
?

 

generation

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskSubjectType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWFTM_TaskSubjectType .
:/ 0

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	subjectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
subjectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
Guid
?

customerId
{
get
;
set
;
}
} 
} �)
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskTemplate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskTemplate +
:, -

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public
short
?
priority
{
get
;
set
;
}
public 
Guid 
? 

customerId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
customerStorageId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? 
companyCarId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
estimatedTaskMinute '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
bool 
? 
sendMail 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
priority_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
customer_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string !
customerStorage_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
assignableUserIds '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
assignableUserTitles *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
helperUserTitles &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string !
taskSubjectType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public   
string   
plate   
{   
get   !
;  ! "
set  # &
;  & '
}  ' (
public!! 
int!! 
?!! 
createdTask_Count!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public"" 
int"" 
?"" 
usedTaskPlan_Count"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""2 3
public## 
int## 
?## 
inventory_Count## #
{##$ %
get##& )
;##) *
set##+ .
;##. /
}##/ 0
public$$ 
string$$ 
inventory_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
}%% 
}&& �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskTemplateInventories.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWFTM_TaskTemplateInventories 6
:7 8

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
inventoryId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

  
taskTemplateId_Title

 *
{

+ ,
get

- 0
;

0 1
set

2 5
;

5 6
}

6 7
public 
string 
inventoryId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
}

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskTemplateSubjectType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWFTM_TaskTemplateSubjectType 6
:7 8

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
	subjectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
subjectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskTemplateUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWFTM_TaskTemplateUser /
:0 1

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 

verifyCode

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string  
taskTemplateId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
photo 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskTemplateUserHelper.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWFTM_TaskTemplateUserHelper 5
:6 7

{ 
public 
Guid 
? 
taskTemplateId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string  
taskTemplateId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public
string
userId_Title
{
get
;
set
;
}
public 
string 
photo 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWFTM_TaskUser '
:( )

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 

verifyCode

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
taskId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
photo 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWFTM_TaskUserHelper.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWFTM_TaskUserHelper -
:. /

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
taskId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public
string
userId_Title
{
get
;
set
;
}
public 
string 
photo 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_Issue.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_Issue $
:% &

{ 
public 
Guid 
? 
pid 
{ 
get 
; 
set  #
;# $
}$ %
public		 
short		 
?		 
status		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
string

 
title

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
int 
? 
expiryMinute  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
mainId 
{ 
get !
;! "
set# &
;& '
}' (
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
	pid_Title 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_IssueUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_IssueUser (
:) *

{ 
public 
Guid 
? 
issueId 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
string
userId_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_Suggestion.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_Suggestion )
:* +

{ 
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
content		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
issueId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
assignUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
assignUserId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �)
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_Ticket.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_Ticket %
:& '

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
short

 
?

 

evaluation

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public
short
?
channel
{
get
;
set
;
}
public 
Guid 
? 
issueId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
suggestionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
requesterId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
assignUserId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
dueDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
evaluateDescription )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
suggestionId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
requesterId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
assignUserId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
priority_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
assignUser_Photo &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public   
string   
requester_Photo   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
createdby_Photo!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public"" 
int"" 
?"" 

replyCount"" 
{""  
get""! $
;""$ %
set""& )
;"") *
}""* +
public## 
int## 
?## 
forwardCount##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##, -
public$$ 
int$$ 
?$$ 
	noteCount$$ 
{$$ 
get$$  #
;$$# $
set$$% (
;$$( )
}$$) *
}%% 
}&& �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_TicketAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_TicketAction +
:, -

{ 
public 
Guid 
? 
ticketId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
short 
? 
ticketStatus "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
ticketId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Photo %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_TicketMessage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHDM_TicketMessage ,
:- .

{ 
public 
Guid 
? 
ticketId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
content

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
string 
cc 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
bcc 
{ 
get 
;  
set! $
;$ %
}% &
public
string
mailto
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ticketId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Photo %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHDM_TicketRequester.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWHDM_TicketRequester .
:/ 0

{ 
public 
string 
fullName 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
string		 
email		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
phone

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
company 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
} 
} �>
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_Person.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_Person $
:% &

{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
string		 
searchField		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
Name

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
SurName 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
IdentifyNumber $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
string
Phone
{
get
;
set
;
}
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 

LocationId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
Birthday !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
ArrivalType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
ReferenceId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
	Education 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
MilitaryStatus "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
Language 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
LanguageType  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
RetirementDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
Adress 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

SchoolName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Branch 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
SalaryRangeMax "
{# $
get% (
;( )
set* -
;- .
}. /
public   
string    
MilitaryExemptDetail   *
{  + ,
get  - 0
;  0 1
set  2 5
;  5 6
}  6 7
public!! 
int!! 
?!! 
Gender!! 
{!! 
get!!  
;!!  !
set!!" %
;!!% &
}!!& '
public"" 
DateTime"" 
?"" 
MilitaryDoneDate"" )
{""* +
get"", /
;""/ 0
set""1 4
;""4 5
}""5 6
public## 
int## 
?## 

{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public$$ 
string$$ 
createdby_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% 
changedby_Title%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%1 2
public&& 
string&& 
ReferenceId_Title&& '
{&&( )
get&&* -
;&&- .
set&&/ 2
;&&2 3
}&&3 4
public'' 
string'' 
LocationId_Title'' &
{''' (
get'') ,
;'', -
set''. 1
;''1 2
}''2 3
public(( 
string(( 
ArrivalType_Title(( '
{((( )
get((* -
;((- .
set((/ 2
;((2 3
}((3 4
public)) 
string)) 
Education_Title)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))1 2
public** 
string** 
DriverLicense_Title** )
{*** +
get**, /
;**/ 0
set**1 4
;**4 5
}**5 6
public++ 
string++  
MilitaryStatus_Title++ *
{+++ ,
get++- 0
;++0 1
set++2 5
;++5 6
}++6 7
public,, 
string,, 
Language_Title,, $
{,,% &
get,,' *
;,,* +
set,,, /
;,,/ 0
},,0 1
public-- 
string-- 
LanguageLevel_Title-- )
{--* +
get--, /
;--/ 0
set--1 4
;--4 5
}--5 6
public.. 
string.. 
Gender_Title.. "
{..# $
get..% (
;..( )
set..* -
;..- .
}... /
public// 
string// 
MaritalStatus_Title// )
{//* +
get//, /
;/// 0
set//1 4
;//4 5
}//5 6
public00 
int00 
?00 
Result00 
{00 
get00  
;00  !
set00" %
;00% &
}00& '
public11 
string11 
Result_Title11 "
{11# $
get11% (
;11( )
set11* -
;11- .
}11. /
public22 
string22 

{22$ %
get22& )
;22) *
set22+ .
;22. /
}22/ 0
public33 
string33 
HrPosition_Title33 &
{33' (
get33) ,
;33, -
set33. 1
;331 2
}332 3
public44 
string44 
HrKeywords_Title44 &
{44' (
get44) ,
;44, -
set44. 1
;441 2
}442 3
public55 
string55 
ProfilePhoto55 "
{55# $
get55% (
;55( )
set55* -
;55- .
}55. /
}66 
}77 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_PersonKeywords.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_PersonKeywords ,
:- .

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrKeywordsId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrPersonId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
string
HrKeywords_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_PersonPosition.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_PersonPosition ,
:- .

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrPositionId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrPersonId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
string
HrPositionId_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_Plan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	VWHR_Plan "
:# $

{ 
public 
string 
InterviewUser_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public		 
Guid		 
?		 

HrPersonId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
DateTime

 
?

 
PlanDate

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
int 
? 
Result 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
Guid
?
PdsEvulateFormId
{
get
;
set
;
}
public 
Guid 
? "
PdsEvulateFormResultId +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
bool 
? 
mailSend 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
StaffNeedsId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
contactLink !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrPersonId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
Result_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
FormName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
Evaluator_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
score 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_PlanPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_PlanPerson (
:) *

{ 
public 
Guid 
? 
HrPlanId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
UserId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
UserId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public
string

{
get
;
set
;
}
public 
string 
HRPerson_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
PlanDate !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Result 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Result_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �@
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeeds.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_StaffNeeds (
:) *

{ 
public 
Guid 
? 
RequestApprovedId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
DateTime		 
?		 
RequestDate		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		0 1
public

 
Guid

 
?

 

LocationId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? 
ArrivalType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
	Education 
{ 
get  #
;# $
set% (
;( )
}) *
public
int
?
MilitaryStatus
{
get
;
set
;
}
public 
int 
? 
Language 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
LanguageType  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
ReferenceId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
?  
ReasonForStaffDemand (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
int 
? 
EmploymentStatus $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 
Gender 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
RetirementDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
NeedCode 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
SalaryRangeMax "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Comment 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public   
string   
createdby_Title   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
changedby_Title!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public"" 
string"" 
LocationId_Title"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""2 3
public## 
string## #
RequestApprovedId_Title## -
{##. /
get##0 3
;##3 4
set##5 8
;##8 9
}##9 :
public$$ 
string$$ 
ReferenceId_Title$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public%% 
string%% 
priority_Title%% $
{%%% &
get%%' *
;%%* +
set%%, /
;%%/ 0
}%%0 1
public&& 
string&& 
ArrivalType_Title&& '
{&&( )
get&&* -
;&&- .
set&&/ 2
;&&2 3
}&&3 4
public'' 
string'' 
Education_Title'' %
{''& '
get''( +
;''+ ,
set''- 0
;''0 1
}''1 2
public(( 
string(( 
DriverLicense_Title(( )
{((* +
get((, /
;((/ 0
set((1 4
;((4 5
}((5 6
public)) 
string))  
MilitaryStatus_Title)) *
{))+ ,
get))- 0
;))0 1
set))2 5
;))5 6
}))6 7
public** 
string** 
Language_Title** $
{**% &
get**' *
;*** +
set**, /
;**/ 0
}**0 1
public++ 
string++ 
LanguageLevel_Title++ )
{++* +
get++, /
;++/ 0
set++1 4
;++4 5
}++5 6
public,, 
string,, &
ReasonForStaffDemand_Title,, 0
{,,1 2
get,,3 6
;,,6 7
set,,8 ;
;,,; <
},,< =
public-- 
string-- "
EmploymentStatus_Title-- ,
{--- .
get--/ 2
;--2 3
set--4 7
;--7 8
}--8 9
public.. 
string.. 
MaritalStatus_Title.. )
{..* +
get.., /
;../ 0
set..1 4
;..4 5
}..5 6
public// 
string// 
Gender_Title// "
{//# $
get//% (
;//( )
set//* -
;//- .
}//. /
public00 
string00 
Travelability_Title00 )
{00* +
get00, /
;00/ 0
set001 4
;004 5
}005 6
public11 
string11 
HrPosition_Title11 &
{11' (
get11) ,
;11, -
set11. 1
;111 2
}112 3
public22 
string22 
HrKeywords_Title22 &
{22' (
get22) ,
;22, -
set22. 1
;221 2
}222 3
public33 
int33 
?33 
TotalYonlendirme33 $
{33% &
get33' *
;33* +
set33, /
;33/ 0
}330 1
public44 
short44 
?44 

lastStatus44  
{44! "
get44# &
;44& '
set44( +
;44+ ,
}44, -
public55 
string55 
lastStatus_Title55 &
{55' (
get55) ,
;55, -
set55. 1
;551 2
}552 3
}66 
}77 �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeedsKeywords.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWHR_StaffNeedsKeywords 0
:1 2

{ 
public 
Guid 
? 
HrKeywordsId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrKeywords_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeedsPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWHR_StaffNeedsPerson .
:/ 0

{ 
public 
Guid 
? 
HrStaffNeedsId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public		 
Guid		 
?		 

HrPersonId		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
int

 
?

 
status

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public
int
?
SalaryRangeMax
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrPersonId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
NeedCode 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeedsPosition.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWHR_StaffNeedsPosition 0
:1 2

{ 
public 
Guid 
? 
HrPositionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
HrPositionId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
}
} �2
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeedsRequester.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWHR_StaffNeedsRequester 1
:2 3

{ 
public 
Guid 
? 
RequestApprovedId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
DateTime		 
?		 
RequestDate		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		0 1
public

 
Guid

 
?

 

LocationId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? 
ArrivalType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
	Education 
{ 
get  #
;# $
set% (
;( )
}) *
public
int
?
MilitaryStatus
{
get
;
set
;
}
public 
int 
? 
Language 
{ 
get "
;" #
set$ '
;' (
}( )
public 
int 
? 
LanguageType  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
ReferenceId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
?  
ReasonForStaffDemand (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
int 
? 
EmploymentStatus $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 
Gender 
{ 
get  
;  !
set" %
;% &
}& '
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
RetirementDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
NeedCode 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
SalaryRangeMin "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
SalaryRangeMax "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Comment 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
priority 
{  
get! $
;$ %
set& )
;) *
}* +
public   
short   
?   

lastStatus    
{  ! "
get  # &
;  & '
set  ( +
;  + ,
}  , -
public!! 
Guid!! 
?!! 
	RequestId!! 
{!!  
get!!! $
;!!$ %
set!!& )
;!!) *
}!!* +
public"" 
string"" 
LocationId_Title"" &
{""' (
get"") ,
;"", -
set"". 1
;""1 2
}""2 3
public## 
string## 
lastStatus_Title## &
{##' (
get##) ,
;##, -
set##. 1
;##1 2
}##2 3
public$$ 
string$$ &
ReasonForStaffDemand_Title$$ 0
{$$1 2
get$$3 6
;$$6 7
set$$8 ;
;$$; <
}$$< =
public%% 
string%% 
Education_Title%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%1 2
public&& 
string&& 
DriverLicense_Title&& )
{&&* +
get&&, /
;&&/ 0
set&&1 4
;&&4 5
}&&5 6
public'' 
string'' 
priority_Title'' $
{''% &
get''' *
;''* +
set'', /
;''/ 0
}''0 1
public(( 
int(( 
?(( 
TotalYonlendirme(( $
{((% &
get((' *
;((* +
set((, /
;((/ 0
}((0 1
public)) 
string)) 
HrPosition_Title)) &
{))' (
get))) ,
;)), -
set)). 1
;))1 2
}))2 3
public** 
string** 
HrKeywords_Title** &
{**' (
get**) ,
;**, -
set**. 1
;**1 2
}**2 3
}++ 
},, �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffNeedsStatus.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWHR_StaffNeedsStatus .
:/ 0

{ 
public 
Guid 
? 
staffNeedId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
staffNeedId_Title
{
get
;
set
;
}
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWHR_StaffStatus.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWHR_StaffStatus )
:* +

{ 
public 
Guid 
? 

HrPersonId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
HrStaffNeedsId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
int

 
?

 
Status

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
HrPersonId_Title
{
get
;
set
;
}
public 
string  
HrStaffNeedsId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
} 
} �)
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_Commissions.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_Commissions *
:+ ,

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
int

 
?

 
Information

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 
InformationDetail '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
int 
? 
TravelInformation %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
TravelInformationDetail
{
get
;
set
;
}
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
CommissionCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
ToAdress 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
CommissionSubject '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
	TotalDays  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 

TotalHours !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? #
RequestForAccommodation +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
Guid 
? 
IdCompanyCar !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
VehiclePlate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
VehicleKilometer '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
createdPhoto "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Information_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public   
string   #
TravelInformation_Title   -
{  . /
get  0 3
;  3 4
set  5 8
;  8 9
}  9 :
public!! 
string!! 
ApproveStatus_Title!! )
{!!* +
get!!, /
;!!/ 0
set!!1 4
;!!4 5
}!!5 6
public"" 
string"" "
Manager1Approval_Title"" ,
{""- .
get""/ 2
;""2 3
set""4 7
;""7 8
}""8 9
public## 
string## 
	Car_Title## 
{##  !
get##" %
;##% &
set##' *
;##* +
}##+ ,
public$$ 
string$$ 
TotalHoursText$$ $
{$$% &
get$$' *
;$$* +
set$$, /
;$$/ 0
}$$0 1
}%% 
}&& �:
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CommissionsInformation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWINV_CommissionsInformation 5
:6 7

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
Guid

 
?

 
to

 
{

 
get

 
;

 
set

 "
;

" #
}

# $
public 
Guid 
? 
from 
{ 
get 
;  
set! $
;$ %
}% &
public 
DateTime 
? 

{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
DateTime
?

returnDate
{
get
;
set
;
}
public 
string 
	hotelName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
hotelEntryDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
DateTime 
? 
hotelLeaveDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
rentalCarPlace #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
rentalCarStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
rentalCarEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
Guid 
? 
	shuttleTo 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
shuttleFrom  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
DateTime 
?  
shuttleDepartureDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
? 
shuttleReturnDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
note 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
	hotelNote 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
shuttleNote !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Information_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public   
string   #
TravelInformation_Title   -
{  . /
get  0 3
;  3 4
set  5 8
;  8 9
}  9 :
public!! 
string!! 
ApproveStatus_Title!! )
{!!* +
get!!, /
;!!/ 0
set!!1 4
;!!4 5
}!!5 6
public"" 
int"" 
?"" 
travelInformation"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
int## 
?## #
requestForAccommodation## +
{##, -
get##. 1
;##1 2
set##3 6
;##6 7
}##7 8
public$$ 
string$$ 

from_Title$$  
{$$! "
get$$# &
;$$& '
set$$( +
;$$+ ,
}$$, -
public%% 
string%% 
to_Title%% 
{%%  
get%%! $
;%%$ %
set%%& )
;%%) *
}%%* +
public&& 
string&& 
shuttleFrom_Title&& '
{&&( )
get&&* -
;&&- .
set&&/ 2
;&&2 3
}&&3 4
public'' 
string'' 
shuttleTo_Title'' %
{''& '
get''( +
;''+ ,
set''- 0
;''0 1
}''1 2
public(( 
string((  
rentalCarPlace_Title(( *
{((+ ,
get((- 0
;((0 1
set((2 5
;((5 6
}((6 7
public)) 
string)) 
hotelLocation_Title)) )
{))* +
get)), /
;))/ 0
set))1 4
;))4 5
}))5 6
public** 
string** 
airportFrom_Title** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public++ 
string++ 
airportTo_Title++ %
{++& '
get++( +
;+++ ,
set++- 0
;++0 1
}++1 2
public,, 
string,, 
flightInfoFiles,, %
{,,& '
get,,( +
;,,+ ,
set,,- 0
;,,0 1
},,1 2
public-- 
string-- 
hotelInfoFiles-- $
{--% &
get--' *
;--* +
set--, /
;--/ 0
}--0 1
public.. 
string.. 
rentalCarInfoFiles.. (
{..) *
get..+ .
;... /
set..0 3
;..3 4
}..4 5
public// 
string// 
shuttleInfoFiles// &
{//' (
get//) ,
;//, -
set//. 1
;//1 2
}//2 3
public00 
string00 
busInfoFiles00 "
{00# $
get00% (
;00( )
set00* -
;00- .
}00. /
public11 
string11 
companyCarFiles11 %
{11& '
get11( +
;11+ ,
set11- 0
;110 1
}111 2
}22 
}33 �1
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CommissionsPersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWINV_CommissionsPersons 1
:2 3

{ 
public 
Guid  
CommissionsPersonsId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public		 
string		 
Files		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
searchField

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
int
?
Information
{
get
;
set
;
}
public 
string 
InformationDetail '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
int 
? 
TravelInformation %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string #
TravelInformationDetail -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
CommissionCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
ToAdress 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
CommissionSubject '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
	TotalDays  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 

TotalHours !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? #
RequestForAccommodation +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
Guid 
? 
IdCompanyCar !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
VehiclePlate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
VehicleKilometer '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
string   
changedby_Title   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
createdPhoto!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!. /
public"" 
string"" 
Information_Title"" '
{""( )
get""* -
;""- .
set""/ 2
;""2 3
}""3 4
public## 
string## #
TravelInformation_Title## -
{##. /
get##0 3
;##3 4
set##5 8
;##8 9
}##9 :
public$$ 
string$$ 
ApproveStatus_Title$$ )
{$$* +
get$$, /
;$$/ 0
set$$1 4
;$$4 5
}$$5 6
public%% 
string%% "
Manager1Approval_Title%% ,
{%%- .
get%%/ 2
;%%2 3
set%%4 7
;%%7 8
}%%8 9
public&& 
string&& 
	Car_Title&& 
{&&  !
get&&" %
;&&% &
set&&' *
;&&* +
}&&+ ,
public'' 
string'' 
TotalHoursText'' $
{''% &
get''' *
;''* +
set'', /
;''/ 0
}''0 1
public(( 
string(( 
Person_Title(( "
{((# $
get((% (
;((( )
set((* -
;((- .
}((. /
public)) 
bool)) 
?)) 
IsOwner)) 
{)) 
get)) "
;))" #
set))$ '
;))' (
}))( )
public** 
Guid** 
?** 
IdUser** 
{** 
get** !
;**! "
set**# &
;**& '
}**' (
}++ 
},, �(
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CommissionsProjects.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWINV_CommissionsProjects 2
:3 4

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
int

 
?

 
Information

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 
InformationDetail '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
int 
? 
TravelInformation %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
TravelInformationDetail
{
get
;
set
;
}
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
CommissionCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
ToAdress 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
CommissionSubject '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
	TotalDays  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 

TotalHours !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? #
RequestForAccommodation +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
Guid 
? 
IdCompanyCar !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
VehiclePlate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
double 
? 
VehicleKilometer '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
Information_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string #
TravelInformation_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	IdProject 
{  
get! $
;$ %
set& )
;) *
}* +
public   
double   
?   

Percentile   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public!! 
string!! 

{!!$ %
get!!& )
;!!) *
set!!+ .
;!!. /
}!!/ 0
public"" 
Guid"" 
?"" 
IdUser"" 
{"" 
get"" !
;""! "
set""# &
;""& '
}""' (
public## 
string## 
Person_Title## "
{### $
get##% (
;##( )
set##* -
;##- .
}##. /
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyCalendar.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWINV_CompanyCalendar .
{ 
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}" #
public		 
string		 
Katilimcilar		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
string

 
Description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
DateTime 
? 
created  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
Guid
?
	createdby
{
get
;
set
;
}
public 
DateTime 
? 
Start 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
End 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}' (
} 
} �0
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyCars.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_CompanyCars *
:+ ,

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
Plate		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
Brand

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
Model 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
IsHire 
{ 
get !
;! "
set# &
;& '
}' (
public
DateTime
?
ContractStartDate
{
get
;
set
;
}
public 
DateTime 
? 
ContractEndDate (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string '
VehicleIdentificationNumber 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
string #
VehicleTransitionNumber -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
ConsumptionFuel &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
DateTime 
? 
InsuranceStartDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
InsuranceEndDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? %
TrafficInsuranceStartDate 2
{3 4
get5 8
;8 9
set: =
;= >
}> ?
public 
DateTime 
? #
TrafficInsuranceEndDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
string 
	TradeName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
Color 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
EngineNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
TrafficReleaseDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
DateTime 
? 
RegistrationDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

LocationId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
UserName 
{  
get! $
;$ %
set& )
;) *
}* +
public   
DateTime   
?   
ExaminationDate   (
{  ) *
get  + .
;  . /
set  0 3
;  3 4
}  4 5
public!! 
int!! 
?!! 
FuelType!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!( )
public"" 
int"" 
?"" 
GearType"" 
{"" 
get"" "
;""" #
set""$ '
;""' (
}""( )
public## 
string## 
PolicyNumber## "
{### $
get##% (
;##( )
set##* -
;##- .
}##. /
public$$ 
string$$ 
KaskoPolicyNumber$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public%% 
string%% 
createdby_Title%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%1 2
public&& 
string&& 
changedby_Title&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&1 2
public'' 
string'' 

{''$ %
get''& )
;'') *
set''+ .
;''. /
}''/ 0
public(( 
string(( 
LocationId_Title(( &
{((' (
get(() ,
;((, -
set((. 1
;((1 2
}((2 3
public)) 
string)) 
UserIdTitle)) !
{))" #
get))$ '
;))' (
set))) ,
;)), -
}))- .
}** 
}++ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyDepartments.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWINV_CompanyDepartments 1
:2 3

{ 
public 
Guid 
? 
PID 
{ 
get 
; 
set  #
;# $
}$ %
public		 
string		 
Name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
	ProjectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
	PID_Title 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

Type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyDepartmentsPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class .
"VWINV_CompanyDepartmentsPageReport ;
{ 
public 
int 
ToplamDepartman "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
int		 
ToplamPozisyon		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

  
SonEklenenDepartment

 *
{

+ ,
get

- 0
;

0 1
set

2 5
;

5 6
}

6 7
public 
string $
DepartmanEnFazlaPozisyon .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPerson.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_CompanyPerson ,
:- .

{ 
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
IdUser		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
DateTime

 
?

 
JobStartDate

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
DateTime 
? 

JobEndDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 

JobLeaving 
{  
get! $
;$ %
set& )
;) *
}* +
public
string
JobLeavingDescription
{
get
;
set
;
}
public 
string 
Title 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
Level 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
JobLeaving_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
int 
? 
PersonWorkingCount &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �&
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonAssessment.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWINV_CompanyPersonAssessment 6
:7 8

{ 
public 
string 
AssessmentCode $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public		 
int		 
?		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
DateTime

 
?

 
AssessmentDate

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
short 
? 
AssessmentType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public
DateTime
?
Manager2ApprovalDate
{
get
;
set
;
}
public 
string #
Manager1ApprovalComment -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string #
Manager2ApprovalComment -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
Manager2Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? 
IkApprovalDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
IKApprovalComment '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? "
GeneralManagerApproval +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string )
GeneralManagerApprovalComment 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public 
DateTime 
? &
GeneralManagerApprovalDate 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ApproveStatus_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string "
Manager1Approval_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string "
Manager2Approval_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public   
string   (
GeneralManagerApproval_Title   2
{  3 4
get  5 8
;  8 9
set  : =
;  = >
}  > ?
public!! 
string!! 
IkApproval_Title!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
}"" 
}## �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonAvailability.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class +
VWINV_CompanyPersonAvailability 8
:9 :

{ 
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
Guid 
? 
	IdProject 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
rate 
{ 
get !
;! "
set# &
;& '
}' (
public
bool
?
isSalary
{
get
;
set
;
}
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonAvailabilityPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 5
)VWINV_CompanyPersonAvailabilityPageReport B
{ 
public 
int 
? !
ToplamMusaitlikSayisi )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public		 
string		 #
EnCokMusaitlikPersoneli		 -
{		. /
get		0 3
;		3 4
set		5 8
;		8 9
}		9 :
public

 
string

 !
EnCokMusaitlikProjesi

 +
{

, -
get

. 1
;

1 2
set

3 6
;

6 7
}

7 8
public 
string  
EnSonEklenenPersonel *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonCalendar.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWINV_CompanyPersonCalendar 4
:5 6

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
Title

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public
bool
?
isSalary
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

Type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
PersonCount 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonCalendarPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 1
%VWINV_CompanyPersonCalendarPageReport >
{ 
public 
int 
	TipDuyuru 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
int		 
TipNot		 
{		 
get		 
;		  
set		! $
;		$ %
}		% &
public

 
int

 
TipHatırlatma

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
int 
TipToplantı 
{  
get! $
;$ %
set& )
;) *
}* +
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonCalendarPersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class .
"VWINV_CompanyPersonCalendarPersons ;
{ 
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}" #
public		 
DateTime		 
?		 
created		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
Guid

 
?

 
	createdby

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
DateTime 
? 
Start 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
End 
{ 
get "
;" #
set$ '
;' (
}( )
public
string
Title
{
get
;
set
;
}
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Katilimcilar "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
} 
} �(
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonDepartments.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class *
VWINV_CompanyPersonDepartments 7
:8 9

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
Title

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
DepartmentId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
IsBasePosition #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
Guid
?
IdUser
{
get
;
set
;
}
public 
Guid 
? 
Manager1 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager2 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager3 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager4 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager5 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
Manager6 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Manager1_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Manager2_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Manager3_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Manager4_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Manager5_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Manager6_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
	ProjectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
Department_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public   
Guid   
?   
Department_PID   #
{  $ %
get  & )
;  ) *
set  + .
;  . /
}  / 0
public!! 
string!! 
	PID_Title!! 
{!!  !
get!!" %
;!!% &
set!!' *
;!!* +
}!!+ ,
public"" 
int"" 
?"" 
OrganizationType"" $
{""% &
get""' *
;""* +
set"", /
;""/ 0
}""0 1
public## 
int## 
?## 
DayArea## 
{## 
get## !
;##! "
set### &
;##& '
}##' (
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWINV_CompanyPersonPageReport 6
{ 
public 
string %
EnCokPersoneliOlanIsletme /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public		 
int		 
ToplamPersonel		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
SonPersonel

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
int 
? 
	PersonAge 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
PersonGender  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
int
?
AkilliKademe
{
get
;
set
;
}
public 
int 
? 
	AllKademe 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
PersonMilitary "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? 
PersonBlood 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
AnkaraPerson  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
IstanbulPerson "
{# $
get% (
;( )
set* -
;- .
}. /
public 
int 
? !
Son1YildaIstenAyrilan )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonSalary.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWINV_CompanyPersonSalary 2
:3 4

{ 
public 
int 
isActive 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
IdUser		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
DateTime

 
?

 
	StartDate

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
Salary 
{ 
get  #
;# $
set% (
;( )
}) *
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_CompanyPersonSalaryPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class /
#VWINV_CompanyPersonSalaryPageReport <
{ 
public 
int &
ToplamMaasliPersonelSayisi -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public		 
string		 !
EnYusekMaasliPersonel		 +
{		, -
get		. 1
;		1 2
set		3 6
;		6 7
}		7 8
public

 
double

 
EnYusekMaas

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 

MaasTarihi  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
}

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_Home.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

VWINV_Home #
{ 
public 
string 
CardKey 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
CardName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
	TotalText

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 
Note 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
NoteText 
{  
get! $
;$ %
set& )
;) *
}* +
public
int
?
Total
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_HomePageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWINV_HomePageReport -
{ 
public 
int 
DemirbasSayisi !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
int		 

{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
int

 
FaturaSayisi

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 

DepoSayisi 
{ 
get  #
;# $
set% (
;( )
}) *
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_Permit.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_Permit %
:& '

{ 
public 
string 

PermitCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
int		 
?		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 


 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
Guid 
? 
PermitTypeId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
EndDate
{
get
;
set
;
}
public 
string 
AccessPhone !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
ArriveAdress "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
Detail 
{ 
get "
;" #
set$ '
;' (
}( )
public 
DateTime 
?  
Manager1ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
?  
Manager2ApprovalDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 
Manager1Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
Manager2Approval %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? 
IkApprovalDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

IkApproval 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 
	TotalDays  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
IdUser 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 

TotalHours !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ApproveStatus_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public   
string   
PermitType_Title   &
{  ' (
get  ) ,
;  , -
set  . 1
;  1 2
}  2 3
public!! 
string!! 
Person_Title!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!. /
public"" 
string"" "
Manager1Approval_Title"" ,
{""- .
get""/ 2
;""2 3
set""4 7
;""7 8
}""8 9
public## 
string## "
Manager2Approval_Title## ,
{##- .
get##/ 2
;##2 3
set##4 7
;##7 8
}##8 9
public$$ 
string$$ 
IkApproval_Title$$ &
{$$' (
get$$) ,
;$$, -
set$$. 1
;$$1 2
}$$2 3
public%% 
string%% 
TotalHoursText%% $
{%%% &
get%%' *
;%%* +
set%%, /
;%%/ 0
}%%0 1
public&& 
string&& 
TcNo&& 
{&& 
get&&  
;&&  !
set&&" %
;&&% &
}&&& '
public'' 
string'' 

{''$ %
get''& )
;'') *
set''+ .
;''. /
}''/ 0
public(( 
string(( 
Files(( 
{(( 
get(( !
;((! "
set((# &
;((& '
}((' (
public)) 
string)) 
searchField)) !
{))" #
get))$ '
;))' (
set))) ,
;)), -
}))- .
}** 
}++ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitOffical.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_PermitOffical ,
:- .

{ 
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

Type_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitOfficialPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class *
VWINV_PermitOfficialPageReport 7
{ 
public 
int "
ToplamResmiİzinSayisi (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public		 
int		 
ToplamİzinSayisi		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
int

 !
ToplamİzinTipiSayisi

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
int 
? 
Gun 
{ 
get 
; 
set "
;" #
}# $
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWINV_PermitPageReport /
{ 
public 
double 
ToplamTalep !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
double		 
Gelecek		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
double

 
Gecmis

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
int 
IzinKullanan 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitSummary.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_PermitSummary ,
:- .

{ 
public 
double 
dayCountDeserved &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
int		 
PermitYearlyUsed		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
double

 
?

 
dayCountUsable

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public 
bool 
? 
IsPaidPermit !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
int
?

{
get
;
set
;
}
public 
bool 
? 
	CanHourly 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public 
bool 
? 
BeDocumented !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
RequestStaff !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
	ViewStaff 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
IsPaidPermit_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
CanHourly_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
BeDocumented_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
DepartmentId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
departmentId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
DateTime 
?  
employeeJobStartDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
? 
employeeDateOfBirth ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
bool 
? 

userStatus 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

Type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public   
string   
Status_Title   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
double!! 
?!! 
dayCountUsed!! #
{!!$ %
get!!& )
;!!) *
set!!+ .
;!!. /
}!!/ 0
public"" 
double"" 
?"" 
dayCountUsedPending"" *
{""+ ,
get""- 0
;""0 1
set""2 5
;""5 6
}""6 7
}## 
}$$ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWINV_PermitType )
:* +

{ 
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
bool		 
?		 
IsPaidPermit		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
int

 
?

 


 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
bool 
? 
	CanHourly 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
Descriptions "
{# $
get% (
;( )
set* -
;- .
}. /
public
bool
?
BeDocumented
{
get
;
set
;
}
public 
bool 
? 
RequestStaff !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
	ViewStaff 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
IsPaidPermit_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
CanHourly_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
BeDocumented_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWINV_PermitType_PageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWINV_PermitType_PageReport 4
{ 
public 
int  
ToplamIzinTipiSayisi '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public		 
string		 
EnCokVerisi		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWMB_MobileDevice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWMB_MobileDevice *
:+ ,

{ 
public 
string 
UniqID 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
AppID		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
AppVersionCode

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
public 
string 
AppVersionName $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
Device 
{ 
get "
;" #
set$ '
;' (
}( )
public
string
Model
{
get
;
set
;
}
public 
string 
	OSVersion 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
Brand 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Board 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
Product 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
Serial 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
SDK 
{ 
get 
;  
set! $
;$ %
}% &
public 
string 
ScreenScale !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 

DeviceType 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
CreatedBy_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ChangeBby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
DeviceType_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWMB_MobileDevicePageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWMB_MobileDevicePageReport 4
{ 
public 
int 
ToplamAndroidCihaz %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
int		 
ToplamIOSCihaz		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 )
EnCokKullanılanAndroidSurumu

 2
{

3 4
get

5 8
;

8 9
set

: =
;

= >
}

> ?
public 
string %
EnCokKullanılanIOSSurumu .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWMB_MobileDeviceRequest.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWMB_MobileDeviceRequest 1
:2 3

{ 
public 
Guid 
? 
DeviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
TicketId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
Url

 
{

 
get

 
;

  
set

! $
;

$ %
}

% &
public 
string 
Browser 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 

TotalBytes 
{  
get! $
;$ %
set& )
;) *
}* +
public
string
PostedFiles
{
get
;
set
;
}
public 
string 
	IPAddress 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
CreatedBy_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
ChangeBby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
MobileDevice_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �$
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_Account.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPA_Account %
:& '

{ 
public 
bool 
	myAccount 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
double		 
balance		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
double

 
	balanceTL

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
string
searchField
{
get
;
set
;
}
public 
string 
companyId_Image %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
Currency_Symbol %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
bankId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
iban 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
bool 
? 
	isDefault 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
currencyId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
bankId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public   
string   
status_Title   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
string!! 
dataId_Title!! "
{!!# $
get!!% (
;!!( )
set!!* -
;!!- .
}!!. /
}"" 
}## �?
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_Advance.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPA_Advance %
:& '

{ 
public 
int 
	hasReject 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
decimal		 
inputAmount		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
decimal

 
outputAmount

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
int 
balance 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
confirmationUser_Title
{
get
;
set
;
}
public 
string !
confirmationUserPhoto +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
Guid 
? 
accountById  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
accountData_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
DateTime 
? 
progressDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
rejectedDescription )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
tax 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
?  
paymentRequestedDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public   
string   
createdby_Title   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
statusInfoMessage!! '
{!!( )
get!!* -
;!!- .
set!!/ 2
;!!2 3
}!!3 4
public"" 
string"" 
changedby_Title"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
string## 
invoiceId_Title## %
{##& '
get##( +
;##+ ,
set##- 0
;##0 1
}##1 2
public$$ 
string$$ 
accountId_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% 
accountDataId_Title%% )
{%%* +
get%%, /
;%%/ 0
set%%1 4
;%%4 5
}%%5 6
public&& 
string&& 
direction_Title&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&1 2
public'' 
string'' 
status_Title'' "
{''# $
get''% (
;''( )
set''* -
;''- .
}''. /
public(( 
string(( 

type_Title((  
{((! "
get((# &
;((& '
set((( +
;((+ ,
}((, -
public)) 
string)) 
currencyId_Title)) &
{))' (
get))) ,
;)), -
set)). 1
;))1 2
}))2 3
public** 
string** 

{**$ %
get**& )
;**) *
set**+ .
;**. /
}**/ 0
public++ 
string++ 
ProfilePhoto++ "
{++# $
get++% (
;++( )
set++* -
;++- .
}++. /
public,, 
string,, 
accountDataTable,, &
{,,' (
get,,) ,
;,,, -
set,,. 1
;,,1 2
},,2 3
public-- 
Guid-- 
?-- 

{--# $
get--% (
;--( )
set--* -
;--- .
}--. /
public.. 
decimal.. 
?.. 
debt.. 
{.. 
get.. "
;.." #
set..$ '
;..' (
}..( )
public// 
string// 
Currency_Symbol// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
string00 
confirmationUserIds00 )
{00* +
get00, /
;00/ 0
set001 4
;004 5
}005 6
public11 
short11 
?11 
confirmationStatus11 (
{11) *
get11+ .
;11. /
set110 3
;113 4
}114 5
public22 
string22 
confirmUserIds22 $
{22% &
get22' *
;22* +
set22, /
;22/ 0
}220 1
public33 
string33 
confirmUser_Titles33 (
{33) *
get33+ .
;33. /
set330 3
;333 4
}334 5
public44 
string44 
rejectedUser_Titles44 )
{44* +
get44, /
;44/ 0
set441 4
;444 5
}445 6
public55 
string55 
rejectedUserIds55 %
{55& '
get55( +
;55+ ,
set55- 0
;550 1
}551 2
}66 
}77 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_AdvanceConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWPA_AdvanceConfirmation 1
:2 3

{ 
public 
Guid 
? 
	advanceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
ruleUserId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
ruleType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
confirmationUserIds )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string &
confirmationUserIds_Titles 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
} 
} �*
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_Ledger.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPA_Ledger $
:% &

{ 
public 
double 
totalAmount !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
decimal		 
inputAmount		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
decimal

 
outputAmount

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
double 
? 
balance 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public
Guid
?
accountRealtedId
{
get
;
set
;
}
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
tax 
{ 
get  
;  !
set" %
;% &
}& '
public 
double 
? 
rateExchange #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
isBalanceFixing $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
double 
? 
	crossRate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
	advanceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
accountId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string "
accountCompanyId_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string "
accountRealtedId_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string )
accountRelatedCompanyId_Title 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public   
string   
direction_Title   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
currencyId_Title!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
public"" 
string"" 

type_Title""  
{""! "
get""# &
;""& '
set""( +
;""+ ,
}"", -
public## 
string## 
transactionId_Title## )
{##* +
get##, /
;##/ 0
set##1 4
;##4 5
}##5 6
public$$ 
string$$ 

{$$$ %
get$$& )
;$$) *
set$$+ .
;$$. /
}$$/ 0
public%% 
string%% 
Currency_Symbol%% %
{%%& '
get%%( +
;%%+ ,
set%%- 0
;%%0 1
}%%1 2
}&& 
}'' �@
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_Transaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPA_Transaction )
:* +

{ 
public 
decimal 
inputAmount "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
decimal		 
outputAmount		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
int

 
	hasReject

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
int 
balance 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string

{
get
;
set
;
}
public 
string 
	ProjectId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
accountById  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
accountData_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	accountId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
	direction 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
progressDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
double 
? 
tax 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
string   
invoiceId_Title   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
string!! 
accountId_Title!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public"" 
string"" 
accountDataId_Title"" )
{""* +
get"", /
;""/ 0
set""1 4
;""4 5
}""5 6
public## 
string## 
dataId_Title## "
{### $
get##% (
;##( )
set##* -
;##- .
}##. /
public$$ 
string$$ 
direction_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% 
status_Title%% "
{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%. /
public&& 
string&& 

type_Title&&  
{&&! "
get&&# &
;&&& '
set&&( +
;&&+ ,
}&&, -
public'' 
string'' 
currencyId_Title'' &
{''' (
get'') ,
;'', -
set''. 1
;''1 2
}''2 3
public(( 
string(( 

{(($ %
get((& )
;(() *
set((+ .
;((. /
}((/ 0
public)) 
string)) 
ProfilePhoto)) "
{))# $
get))% (
;))( )
set))* -
;))- .
})). /
public** 
string** 
statusInfoMessage** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public++ 
string++ 
accountDataTable++ &
{++' (
get++) ,
;++, -
set++. 1
;++1 2
}++2 3
public,, 
Guid,, 
?,, 

{,,# $
get,,% (
;,,( )
set,,* -
;,,- .
},,. /
public-- 
decimal-- 
?-- 
debt-- 
{-- 
get-- "
;--" #
set--$ '
;--' (
}--( )
public.. 
string.. 
Currency_Symbol.. %
{..& '
get..( +
;..+ ,
set..- 0
;..0 1
}..1 2
public// 
string// 
confirmationUserIds// )
{//* +
get//, /
;/// 0
set//1 4
;//4 5
}//5 6
public00 
string00 "
confirmationUser_Title00 ,
{00- .
get00/ 2
;002 3
set004 7
;007 8
}008 9
public11 
string11 !
confirmationUserPhoto11 +
{11, -
get11. 1
;111 2
set113 6
;116 7
}117 8
public22 
short22 
?22 
confirmationStatus22 (
{22) *
get22+ .
;22. /
set220 3
;223 4
}224 5
public33 
string33 
confirmUserIds33 $
{33% &
get33' *
;33* +
set33, /
;33/ 0
}330 1
public44 
string44 
confirmUser_Titles44 (
{44) *
get44+ .
;44. /
set440 3
;443 4
}444 5
public55 
string55 
rejectedUser_Titles55 )
{55* +
get55, /
;55/ 0
set551 4
;554 5
}555 6
public66 
string66 
rejectedUserIds66 %
{66& '
get66( +
;66+ ,
set66- 0
;660 1
}661 2
}77 
}88 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPA_TransactionConfirmation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWPA_TransactionConfirmation 5
:6 7

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	ruleOrder 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
short
?
ruleType
{
get
;
set
;
}
public 
Guid 
? 

ruleUserId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

ruleRoleId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	ruleTitle 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Person_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
ruleUserId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
ruleType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
confirmationUserIds )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string &
confirmationUserIds_Titles 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_EvaluateForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPDS_EvaluateForm +
:, -

{ 
public 
int 
? 
formType 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
formName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
formCode

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
formType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 

formResult 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_FormPermitTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWPDS_FormPermitTask -
:. /

{ 
public 
string 
taskName 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
DateTime		 
?		 
	startDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
endDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
evaluateFormId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
formName_title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_FormPermitTaskUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWPDS_FormPermitTaskUser 1
:2 3

{ 
public 
int 
status 
{ 
get 
;  
set! $
;$ %
}% &
public		 
string		 
formName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
formCode

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 
formType 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
formType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
string
ProfilePhoto
{
get
;
set
;
}
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
DateTime 
? 
lastEvaluateDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
Guid 
? 
lastId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
formPermitTaskId %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
evaluatorId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
evaluatedUserId $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
evaluator_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
evaluatedUser_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
evaluateFormId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_FormResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPDS_FormResult )
:* +

{ 
public 
string 
comment 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
int		 
?		 
status		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

  
formPermitTaskUserId

 )
{

* +
get

, /
;

/ 0
set

1 4
;

4 5
}

5 6
public 
Guid 
? 
evaluateFormId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
evaluatorId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
Guid
?
evaluatedUserId
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
evaluator_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
evaluatedUser_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
formName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
formCode 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
formType 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
formType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
double 
? 
score 
{ 
get "
;" #
set$ '
;' (
}( )
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_Question.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPDS_Question '
:( )

{ 
public 
string 
question 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
string		 
createdby_Title		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
string

 
changedby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_QuestionForm.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPDS_QuestionForm +
:, -

{ 
public 
double 
? 
factor 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
int		 
?		 

{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
	groupName

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
int 
? 

groupOrder 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

questionId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
Guid
?
evaluateFormId
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
formName_title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
question 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPDS_QuestionResult.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWPDS_QuestionResult -
:. /

{ 
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
comment		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
Guid

 
?

 
questionFormId

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
Guid 
? 
formResultId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
score 
{ 
get 
;  
set! $
;$ %
}% &
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
double 
? 
factor 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
question 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
	groupName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 
percentScore  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Brand.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_Brand $
:% &

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Category.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_Category '
:( )

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
Guid		 
?		 
pid		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
fullname

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 

generation 
{  
get! $
;$ %
set& )
;) *
}* +
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_CompanyBasedPrice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRD_CompanyBasedPrice 0
:1 2

{ 
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	productId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 

categoryId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
sellingType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
short
?
companyType
{
get
;
set
;
}
public 
short 
? 
productType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
categoryId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
conditionType_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
sellingType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
companyType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
productType_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_CompanyBasedPriceDetail.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWPRD_CompanyBasedPriceDetail 6
:7 8

{ 
public 
Guid 
companyBasedPriceId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public		 
double		 
?		 
minCondition		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
double 
? 
discount 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
int 
? 

monthCount 
{  
get! $
;$ %
set& )
;) *
}* +
public
DateTime
?
	startDate
{
get
;
set
;
}
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

categoryId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
short 
? 
sellingType !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_DistributionPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWPRD_DistributionPlan /
:0 1

{ 
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
outputId 
{ 
get  #
;# $
set% (
;( )
}) *
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
outputId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string !
outputCompanyId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_DistributionPlanRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class *
VWPRD_DistributionPlanRelation 7
:8 9

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
distributionPlanId		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
code
{
get
;
set
;
}
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
outputId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
invoiceId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string !
outputCompanyId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string  
inputCompanyId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
} 
} �+
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_EntegrationAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRD_EntegrationAction 0
:1 2

{ 
public 
Guid 
? 
EntegrationFileId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
Guid		 
?		 

{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
string

 
DistributorName

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string  
CustomerOperatorCode *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public
string
CustomerOperatorName
{
get
;
set
;
}
public 
Guid 
? 
CustomerOperatorId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string '
CustomerOperatorStorageCode 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
Guid 
? %
CustomerOperatorStorageId .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public 
string '
CustomerOperatorStorageCity 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
string '
CustomerOperatorStorageTown 1
{2 3
get4 7
;7 8
set9 <
;< =
}= >
public 
string 

BranchName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

BranchCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
	TaxNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
ConsolidationCode '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
ConsolidationName '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
	ProductId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
InventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Imei 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
SerialNo 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
? 
Quantity 
{ 
get "
;" #
set$ '
;' (
}( )
public 
DateTime 
? 
Invoice_Address (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 

Invoice_No  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Company_Code "
{# $
get% (
;( )
set* -
;- .
}. /
public   
string   
CompanyName   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public!! 
string!! 
county!! 
{!! 
get!! "
;!!" #
set!!$ '
;!!' (
}!!( )
public"" 
string"" 
town"" 
{"" 
get""  
;""  !
set""" %
;""% &
}""& '
public## 
string## 

DeviceCode##  
{##! "
get### &
;##& '
set##( +
;##+ ,
}##, -
public$$ 
string$$ 

DeviceName$$  
{$$! "
get$$# &
;$$& '
set$$( +
;$$+ ,
}$$, -
public%% 
DateTime%% 
?%% 
ActivationDate%% '
{%%( )
get%%* -
;%%- .
set%%/ 2
;%%2 3
}%%3 4
public&& 
int&& 
?&& 
Count&& 
{&& 
get&& 
;&&  
set&&! $
;&&$ %
}&&% &
}'' 
}(( �%
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_EntegrationImport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRD_EntegrationImport 0
:1 2

{ 
public 
string 
customerName "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
string		 
customerCode		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
string

 
productModel

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
string 
distributorName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
DateTime 
? '
distributorConfirmationDate 4
{5 6
get7 :
;: ;
set< ?
;? @
}@ A
public
string
imei
{
get
;
set
;
}
public 
string 
customerType "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
contractStartDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
contractCode "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
productGroup "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
sellingChannelType (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
distributorCode %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 
sellingQuantity #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 
month 
{ 
get 
;  
set! $
;$ %
}% &
public 
int 
? 
year 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 

company_Id 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
company_Name "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
inventory_Id !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
distributor_id #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
distributor_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 

product_Id 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public   
Guid   
?    
entegrationAction_id   )
{  * +
get  , /
;  / 0
set  1 4
;  4 5
}  5 6
public!! 
Guid!! 
?!! 
titanActivated_id!! &
{!!' (
get!!) ,
;!!, -
set!!. 1
;!!1 2
}!!2 3
}"" 
}## �.
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Inventory.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_Inventory (
:) *

{ 
public 
int 
isItSalable 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
string		 
fullName		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
fullNameProduct

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Image %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 

serialcode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
string !
firstActionType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string  
lastActionType_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string #
firstActionDataId_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string *
firstActionDataCompanyId_Title 4
{5 6
get7 :
;: ;
set< ?
;? @
}@ A
public 
string "
lastActionDataId_Title ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string )
lastActionDataCompanyId_Title 3
{4 5
get6 9
;9 :
set; >
;> ?
}? @
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
firstActionType %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 
lastActionType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
firstActionDataId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
Guid 
? $
firstActionDataCompanyId -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public   
Guid   
?   
lastActionId   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
public!! 
Guid!! 
?!! 
lastActionDataId!! %
{!!& '
get!!( +
;!!+ ,
set!!- 0
;!!0 1
}!!1 2
public"" 
Guid"" 
?"" #
lastActionDataCompanyId"" ,
{""- .
get""/ 2
;""2 3
set""4 7
;""7 8
}""8 9
public## 
Guid## 
?## 
firstTransactionId## '
{##( )
get##* -
;##- .
set##/ 2
;##2 3
}##3 4
public$$ 
Guid$$ 
?$$ 
lastTransactionId$$ &
{$$' (
get$$) ,
;$$, -
set$$. 1
;$$1 2
}$$2 3
public%% 
Guid%% 
?%% "
firstTransactionItemId%% +
{%%, -
get%%. 1
;%%1 2
set%%3 6
;%%6 7
}%%7 8
public&& 
Guid&& 
?&& !
lastTransactionItemId&& *
{&&+ ,
get&&- 0
;&&0 1
set&&2 5
;&&5 6
}&&6 7
public'' 
string'' #
lastActionCompanyTitles'' -
{''. /
get''0 3
;''3 4
set''5 8
;''8 9
}''9 :
}(( 
})) �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_InventoryAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWPRD_InventoryAction .
:/ 0

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
transactionItemId		 &
{		' (
get		) ,
;		, -
set		. 1
;		1 2
}		2 3
public

 
Guid

 
?

 
inventoryId

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public
string
	dataTable
{
get
;
set
;
}
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
transactionId_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
inventoryId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 

serialCode  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
dataCompanyId_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
dataId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_InventoryTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_InventoryTask ,
:- .

{ 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
DateTime		 
?		 
	startDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
endDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
double 
? 
period 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public
string
description
{
get
;
set
;
}
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
period_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
inventoryId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
lastTaskDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �<
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Product.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
double 
stockPerson !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
double		 
stockStorage		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
double

 

stockTotal

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
int 
criticalStockAlert %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
statusFlags !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
string
fullName
{
get
;
set
;
}
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
barcode 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 

categoryId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
brandId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
barcodeType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
bool 
? 
isCriticalStock $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
	stockType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
categoryId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public   
Guid   
?   

{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
string!! 
productUnitId_Title!! )
{!!* +
get!!, /
;!!/ 0
set!!1 4
;!!4 5
}!!5 6
public"" 
string"" 

type_Title""  
{""! "
get""# &
;""& '
set""( +
;""+ ,
}"", -
public## 
string## 
stockType_Title## %
{##& '
get##( +
;##+ ,
set##- 0
;##0 1
}##1 2
public$$ 
string$$ 
status_Title$$ "
{$$# $
get$$% (
;$$( )
set$$* -
;$$- .
}$$. /
public%% 
string%% !
isCriticalStock_Title%% +
{%%, -
get%%. 1
;%%1 2
set%%3 6
;%%6 7
}%%7 8
public&& 
double&& 
?&& 
currentSellingPoint&& *
{&&+ ,
get&&- 0
;&&0 1
set&&2 5
;&&5 6
}&&6 7
public'' 
string'' 
logo'' 
{'' 
get''  
;''  !
set''" %
;''% &
}''& '
public(( 
double(( 
?(( 
currentBuyingPrice(( )
{((* +
get((, /
;((/ 0
set((1 4
;((4 5
}((5 6
public)) 
Guid)) 
?)) #
currentBuyingCurrencyId)) ,
{))- .
get))/ 2
;))2 3
set))4 7
;))7 8
}))8 9
public** 
string** )
currentBuyingCurrencyId_Title** 3
{**4 5
get**6 9
;**9 :
set**; >
;**> ?
}**? @
public++ 
double++ 
?++ 
currentSellingPrice++ *
{+++ ,
get++- 0
;++0 1
set++2 5
;++5 6
}++6 7
public,, 
Guid,, 
?,, $
currentSellingCurrencyId,, -
{,,. /
get,,0 3
;,,3 4
set,,5 8
;,,8 9
},,9 :
public-- 
string-- *
currentSellingCurrencyId_Title-- 4
{--5 6
get--7 :
;--: ;
set--< ?
;--? @
}--@ A
public.. 
string.. &
currentSellingCurrencyCode.. 0
{..1 2
get..3 6
;..6 7
set..8 ;
;..; <
}..< =
public// 
double// 
?// 
currentServicePrice// *
{//+ ,
get//- 0
;//0 1
set//2 5
;//5 6
}//6 7
public00 
Guid00 
?00 $
currentServiceCurrencyId00 -
{00. /
get000 3
;003 4
set005 8
;008 9
}009 :
public11 
string11 *
currentServiceCurrencyId_Title11 4
{115 6
get117 :
;11: ;
set11< ?
;11? @
}11@ A
public22 
string22 &
currentServiceCurrencyCode22 0
{221 2
get223 6
;226 7
set228 ;
;22; <
}22< =
}33 
}44 �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductBounty.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_ProductBounty ,
:- .

{ 
public 
double 
? 
amount 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
Guid		 
?		 
	companyId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
	productId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
int 
? 
year 
{ 
get 
; 
set  #
;# $
}$ %
public 
int 
? 
month 
{ 
get 
;  
set! $
;$ %
}% &
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductCompany.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWPRD_ProductCompany -
:. /

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	companyId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
int

 
?

 
type

 
{

 
get

 
;

 
set

  #
;

# $
}

$ %
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
productId_Title
{
get
;
set
;
}
public 
string 
companyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
companyId_Image %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �*
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Production.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_Production )
:* +

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
DateTime

 
?

 
productionOrderDate

 ,
{

- .
get

/ 2
;

2 3
set

4 7
;

7 8
}

8 9
public 
DateTime 
? #
scheduledProductionDate 0
{1 2
get3 6
;6 7
set8 ;
;; <
}< =
public 
Guid 
? 
productionCompanyId (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public
Guid
?
productionStorageId
{
get
;
set
;
}
public 
Guid 
? 
centerCompanyId $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
centerStorageId $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
lastProductionDate +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
schemaTitle !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string %
productionCompanyId_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string %
productionStorageId_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string !
centerCompanyId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Code $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
productUnit_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string !
centerStorageId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
double 
? 
producedQuantity '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string  
assignableUserTitles *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public   
string   
assignableUserIds   '
{  ( )
get  * -
;  - .
set  / 2
;  2 3
}  3 4
public!! 
string!! %
lastProductionStage_Title!! /
{!!0 1
get!!2 5
;!!5 6
set!!7 :
;!!: ;
}!!; <
public"" 
string"" %
lastOperationStatus_Title"" /
{""0 1
get""2 5
;""5 6
set""7 :
;"": ;
}""; <
public## 
short## 
?## 
lastOperationStatus## )
{##* +
get##, /
;##/ 0
set##1 4
;##4 5
}##5 6
public$$ 
DateTime$$ 
?$$ 
lastOperationDate$$ *
{$$+ ,
get$$- 0
;$$0 1
set$$2 5
;$$5 6
}$$6 7
}%% 
}&& �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionOperation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWPRD_ProductionOperation 2
:3 4

{ 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
short		 
?		 
status		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public
string
	dataTable
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
dataId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} � 
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionProduct.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRD_ProductionProduct 0
:1 2

{ 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
	productId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
serialCodes

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public
double
?
amountSpent
{
get
;
set
;
}
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 

materialId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
transactionType %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 

amountFire !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
materialId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string $
materialId_TitleWithCode .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string !
transactionType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
currencyId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
	stockLeft  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionSchema.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWPRD_ProductionSchema /
:0 1

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
Guid

 
?

 
	productId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
productId_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionSchemaStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWPRD_ProductionSchemaStage 4
:5 6

{ 
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
int

 
?

 
stageNum

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
productionSchemaId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string $
productionSchemaId_Title .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionSchemaSteps.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class '
VWPRD_ProductionSchemaSteps 4
:5 6

{ 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
name

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
int 
? 
stageNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionStage.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWPRD_ProductionStage .
:/ 0

{ 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
code		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
name

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
int 
? 
stageNumber 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
productionSchemaId '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductionUser.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWPRD_ProductionUser -
:. /

{ 
public 
Guid 
? 
productionId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
Guid		 
?		 
userId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public
string
photo
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductMateriel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWPRD_ProductMateriel .
:/ 0

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
double		 
?		 
quantity		 
{		  !
get		" %
;		% &
set		' *
;		* +
}		+ ,
public

 
Guid

 
?

 

materialId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
materialId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 
price 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
currencySymbol $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
double 
? 

{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
double 
? 

stockTotal !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
stockCompanyIds %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductPointSelling.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWPRD_ProductPointSelling 2
:3 4

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
double		 
?		 
point		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
DateTime

 
?

 
	startDate

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductPrice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_ProductPrice +
:, -

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
double		 
?		 
price		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
short

 
?

 
type

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
Guid 
? 

currencyId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
currencyId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductTask.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_ProductTask *
:+ ,

{ 
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
endDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
double

 
?

 
period

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
Guid
?
	productId
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_ProductUnit.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_ProductUnit *
:+ ,

{ 
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
unitId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
double

 
?

 
quantity

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 
	isDefault 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
isDefault_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �1
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_StockAction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_StockAction *
:+ ,

{ 
public 
int 
	direction 
{ 
get "
;" #
set$ '
;' (
}( )
public		 
string		 
currency_Title		 $
{		% &
get		' *
;		* +
set		, /
;		/ 0
}		0 1
public

 
Guid

 
?

 


 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
double 
? 
	unitPrice  
{! "
get# &
;& '
set( +
;+ ,
}, -
public
string
serialCodes
{
get
;
set
;
}
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public 
double 
? 
quantity 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
alternativeUnitId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
alternativeQuantity *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
date 
{ 
get  #
;# $
set% (
;( )
}) *
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
stockId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

stockTable  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
stockCompanyId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
dataId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	dataTable 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
tritemUnitId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string  
stockCompanyId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public   
string   
dataId_Title   "
{  # $
get  % (
;  ( )
set  * -
;  - .
}  . /
public!! 
string!! 
dataCompanyId_Title!! )
{!!* +
get!!, /
;!!/ 0
set!!1 4
;!!4 5
}!!5 6
public"" 
string"" 
productId_Title"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
string## 
productName## !
{##" #
get##$ '
;##' (
set##) ,
;##, -
}##- .
public$$ 
string$$ 
productCode$$ !
{$$" #
get$$$ '
;$$' (
set$$) ,
;$$, -
}$$- .
public%% 
string%% 
status_Title%% "
{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%. /
public&& 
string&& 

type_Title&&  
{&&! "
get&&# &
;&&& '
set&&( +
;&&+ ,
}&&, -
public'' 
string'' 
unitId_Title'' "
{''# $
get''% (
;''( )
set''* -
;''- .
}''. /
public(( 
string(( #
alternativeUnitId_Title(( -
{((. /
get((0 3
;((3 4
set((5 8
;((8 9
}((9 :
public)) 
double)) 
?)) 

totalPrice)) !
{))" #
get))$ '
;))' (
set))) ,
;)), -
}))- .
public** 
double** 
?** 

{**% &
get**' *
;*** +
set**, /
;**/ 0
}**0 1
}++ 
},, �$
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_StockSummary.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_StockSummary +
{ 
public 
Guid 
? 
id 
{ 
get 
; 
set "
;" #
}# $
public		 
DateTime		 
?		 
created		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
double

 
?

 
quantity

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
stockId 
{ 
get "
;" #
set$ '
;' (
}( )
public
string

stockTable
{
get
;
set
;
}
public 
Guid 
? 
stockCompanyId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
Guid 
? 
tritemUnitId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
alternativeUnitId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
alternativeQuantity *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
productName !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
productType !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
productCode !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string  
stockCompanyId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
Guid 
? 
	productId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Image %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string #
alternativeUnitId_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
short 
? 
	stockType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
categoryId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
productPrice #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
double 
? 
servicePrice #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public   
string   
searchField   !
{  " #
get  $ '
;  ' (
set  ) ,
;  , -
}  - .
}!! 
}"" �!
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_StockTaskPlan.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_StockTaskPlan ,
:- .

{ 
public 
Guid 
? 
inventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
taskId		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
short

 
?

 
status

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
Guid 
? 
	storageId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public
string
inventoryIndexValue
{
get
;
set
;
}
public 
int 
? 
inventoryStampYear &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
newInventoryBrand '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string "
newInventoryIndexValue ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
Guid 
? 
newInventoryId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? !
newInventoryStampYear )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string !
inventorySerialNumber +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
storageId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
storageCode !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
storageAddress $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
	companyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
locationId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
serialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string %
lastOperationStatus_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
} 
}   �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_TitanDeviceActivated.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
VWPRD_TitanDeviceActivated 3
:4 5

{ 
public 
string 
SerialNumber "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
string		 
IMEI1		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
IMEI2

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
DateTime 
? 
CreatedOfTitan '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
DeviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public
Guid
?
	ProductId
{
get
;
set
;
}
public 
Guid 
? 
InventoryId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
TitanDeviceName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

TitanModel  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
TitanProduct "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
InvetoryId_Code %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �'
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_Transaction.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRD_Transaction *
:+ ,

{ 
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
DateTime		 
?		 
date		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
short 
? 
status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
short 
? 
type 
{ 
get  
;  !
set" %
;% &
}& '
public
string
description
{
get
;
set
;
}
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
orderId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
Guid 
? 
outputId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
outputTable !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
outputCompanyId $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? 
inputId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

inputTable  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
Guid 
? 
inputCompanyId #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
endDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
createdPhoto "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
invoiceId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public   
string   

{  $ %
get  & )
;  ) *
set  + .
;  . /
}  / 0
public!! 
string!! 
outputId_Title!! $
{!!% &
get!!' *
;!!* +
set!!, /
;!!/ 0
}!!0 1
public"" 
string""  
inputCompanyId_Title"" *
{""+ ,
get""- 0
;""0 1
set""2 5
;""5 6
}""6 7
public## 
string## !
outputCompanyId_Title## +
{##, -
get##. 1
;##1 2
set##3 6
;##6 7
}##7 8
}$$ 
}%% �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRD_TransactionItem.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWPRD_TransactionItem .
:/ 0

{ 
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public		 
Guid		 
?		 
	productId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
double

 
?

 
	unitPrice

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
serialCodes !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
unitId 
{ 
get !
;! "
set# &
;& '
}' (
public
double
?
quantity
{
get
;
set
;
}
public 
Guid 
? 
alternativeUnitId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
double 
? 
alternativeQuantity *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
transactionId_Title )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
string 
productId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
short 
? 
transactionType %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string  
transactionTypeTitle *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
unitId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string #
alternativeUnitId_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
outputId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string  
inputCompanyId_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string !
outputCompanyId_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
} 
} �;
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_Project.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
string 
ProjectName !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
string		 
ProjectCode		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
string

 
ProjectScope

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
DateTime 
? 
ProjectStartDate )
{* +
get, /
;/ 0
set1 4
;4 5
}5 6
public 
DateTime 
? 
ProjectEndDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public
DateTime
?
ProjectConfirmDate
{
get
;
set
;
}
public 
bool 
? 
	IsConfirm 
{  
get! $
;$ %
set& )
;) *
}* +
public 
bool 
? 
IsActive 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
ProjectType 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
Guid 
? 
	CompanyId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
TenderNo 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 

TenderName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
TenderWrite !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
Guid 
? 
IdProjectLinked $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
ProjectJiraKey $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
? 
WarrantyStartDate *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
DateTime 
? 
WarrantyEndDate (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string  
ProjectTechnicalType *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
int 
? 
	PriceType 
{ 
get  #
;# $
set% (
;( )
}) *
public 
double 
? 
Exchange 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
DateTime 
? !
ContractGuaranteeDate .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public 
DateTime 
?  
AdvanceGuaranteeDate -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
? !
WarrantyGuaranteeDate .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public   
double   
?   
ContractAmount   %
{  & '
get  ( +
;  + ,
set  - 0
;  0 1
}  1 2
public!! 
double!! 
?!! 

{!!% &
get!!' *
;!!* +
set!!, /
;!!/ 0
}!!0 1
public"" 
double"" 
?"" 
WarrantyAmount"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
string## 
createdby_Title## %
{##& '
get##( +
;##+ ,
set##- 0
;##0 1
}##1 2
public$$ 
string$$ 
changedby_Title$$ %
{$$& '
get$$( +
;$$+ ,
set$$- 0
;$$0 1
}$$1 2
public%% 
string%% 

{%%$ %
get%%& )
;%%) *
set%%+ .
;%%. /
}%%/ 0
public&& 
string&& 
Corporation_Title&& '
{&&( )
get&&* -
;&&- .
set&&/ 2
;&&2 3
}&&3 4
public'' 
string'' 
ProjectType_Title'' '
{''( )
get''* -
;''- .
set''/ 2
;''2 3
}''3 4
public(( 
double(( 
?(( 
DayArea(( 
{((  
get((! $
;(($ %
set((& )
;(() *
}((* +
public)) 
int)) 
?)) 
ProjectStarEndArea)) &
{))' (
get))) ,
;)), -
set)). 1
;))1 2
}))2 3
public** 
int** 
?** 
ProjectRemainingDay** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public++ 
string++ 
IsActive_Title++ $
{++% &
get++' *
;++* +
set++, /
;++/ 0
}++0 1
public,, 
string,, 
IsConfirm_Title,, %
{,,& '
get,,( +
;,,+ ,
set,,- 0
;,,0 1
},,1 2
public-- 
string-- !
IdProjectLinked_Title-- +
{--, -
get--. 1
;--1 2
set--3 6
;--6 7
}--7 8
public.. 
int.. 
?.. 
ProjectPersonCount.. &
{..' (
get..) ,
;.., -
set... 1
;..1 2
}..2 3
public// 
string// 
PriceType_Title// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
string00 
projectPersonIds00 &
{00' (
get00) ,
;00, -
set00. 1
;001 2
}002 3
public11 
string11 
searchField11 !
{11" #
get11$ '
;11' (
set11) ,
;11, -
}11- .
}22 
}33 �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectInvoice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWPRJ_ProjectInvoice -
:. /

{ 
public 
Guid 
? 
	invoiceId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRJ_ProjectPageReport 0
{ 
public 
int 
ToplamProje 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
int		 

{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
EnYakinBitisli

 $
{

% &
get

' *
;

* +
set

, /
;

/ 0
}

0 1
public 
string 
SonProje 
{  
get! $
;$ %
set& )
;) *
}* +
public 
int 
AktifTeknokent !
{" #
get$ '
;' (
set) ,
;, -
}- .
public
int
AktifTubitak
{
get
;
set
;
}
public 
int !
AktifTubitakTeknokent (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectScopeLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWPRJ_ProjectScopeLevel 0
:1 2

{ 
public 
string 
level 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	projectId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
locationIds_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectScopeLevelItems.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWPRJ_ProjectScopeLevelItems 5
:6 7

{ 
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
Guid		 
?		 
scopeLevelId		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
Guid

 
?

 

locationId

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
projectId_Title
{
get
;
set
;
}
public 
string 
scopeLevelId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
locationId_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectServiceLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWPRJ_ProjectServiceLevel 2
:3 4

{ 
public 
Guid 
? 
scopeLevelId !
{" #
get$ '
;' (
set) ,
;, -
}- .
public		 
short		 
?		 

amercement		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
short

 
?

 
endTime

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
bool 
? 

{# $
get% (
;( )
set* -
;- .
}. /
public 
short 
? 
resolutionType $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
bool
?
nextDay
{
get
;
set
;
}
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
short 
? 
taskType 
{  
get! $
;$ %
set& )
;) *
}* +
public 
Guid 
? 
typeLevelId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
scopeLevelId_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
Level_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string  
resolutionType_Title *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 

type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
short 
? 
resolutionTime $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
short 
? 
	startTime 
{  !
get" %
;% &
set' *
;* +
}+ ,
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectTimeline.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWPRJ_ProjectTimeline .
:/ 0

{ 
public 
Guid 
	IdProject 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 

lastStatus		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 

actionPlan

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
int 
? 
Type 
{ 
get 
; 
set  #
;# $
}$ %
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
EndDate
{
get
;
set
;
}
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
Description !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
short 
? 
Status 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

Type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
projectPersonIds &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
Status_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
projectCode !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectTimelinePageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class +
VWPRJ_ProjectTimelinePageReport 8
{ 
public 
int 

{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
int		 

{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
int

 


  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
string 
SonEklenenCizelge '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectTimelinePersons.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class (
VWPRJ_ProjectTimelinePersons 5
:6 7

{ 
public 
Guid 
? 

IdTimeline 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
	IdProject		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
IdUser

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

{
get
;
set
;
}
public 
string 
IdUser_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
IdTimeline_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ProjectTypeLevel.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWPRJ_ProjectTypeLevel /
:0 1

{ 
public 
string 
level 
{ 
get !
;! "
set# &
;& '
}' (
public		 
short		 
?		 
type		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

type_Title
{
get
;
set
;
}
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWPRJ_ScopeRelation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWPRJ_ScopeRelation ,
:- .

{ 
public 
Guid 
? 
corporateId  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
Guid		 
?		 
	storageId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
Guid

 
?

 
inventoryId

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
Guid 
? 
	projectId 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public
DateTime
?
endDate
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
corporateId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
string 
storageId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
inventoryId_Title '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_AgileBoards.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_AgileBoards )
:* +

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
name		 
{		 
get		  
;		  !
set		" %
;		% &
}		& '
public

 
string

 
description

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
json 
{ 
get  
;  !
set" %
;% &
}& '
public 
DateTime 
? 
lastUsedDate %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
createdby_Title
{
get
;
set
;
}
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_CorrectiveActivity.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWSH_CorrectiveActivity 0
:1 2

{ 
public 
Guid 
? 
taskId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
workAccidentId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
Guid

 
?

 
	projectId

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
keyword 
{ 
get  #
;# $
set% (
;( )
}) *
public
DateTime
?
date
{
get
;
set
;
}
public 
Guid 
? 

templateId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
content 
{ 
get  #
;# $
set% (
;( )
}) *
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
projectId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �M
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Employee.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 

:' (

{ 
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public		 
int		 
?		 
type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
code

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
	loginname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	firstname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
string
lastname
{
get
;
set
;
}
public 
DateTime 
? 
birthday !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
password 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	cellphone 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
address 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

locationId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
companyCellPhone &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string  
companyCellPhoneCode *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string 
companyOfficePhone (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string "
companyOfficePhoneCode ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public 
string 
fullName 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
nationality !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

City_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 

Town_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
BornLocation_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 

FatherName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public   
string   

MotherName    
{  ! "
get  # &
;  & '
set  ( +
;  + ,
}  , -
public!! 
string!!  
IdentificationNumber!! *
{!!+ ,
get!!- 0
;!!0 1
set!!2 5
;!!5 6
}!!6 7
public"" 
string"" 
EmergencyPerson"" %
{""& '
get""( +
;""+ ,
set""- 0
;""0 1
}""1 2
public## 
string## 
EmergencyPhone## $
{##% &
get##' *
;##* +
set##, /
;##/ 0
}##0 1
public$$ 
string$$ 
Gender_Title$$ "
{$$# $
get$$% (
;$$( )
set$$* -
;$$- .
}$$. /
public%% 
string%% 
MaritalStatus_Title%% )
{%%* +
get%%, /
;%%/ 0
set%%1 4
;%%4 5
}%%5 6
public&& 
string&& 
Military_Title&& $
{&&% &
get&&' *
;&&* +
set&&, /
;&&/ 0
}&&0 1
public'' 
DateTime'' 
?'' 
Military_DoneDate'' *
{''+ ,
get''- 0
;''0 1
set''2 5
;''5 6
}''6 7
public(( 
DateTime(( 
?(( 
Probation_Date(( '
{((( )
get((* -
;((- .
set((/ 2
;((2 3
}((3 4
public)) 
string)) 
ProbationDetail)) %
{))& '
get))( +
;))+ ,
set))- 0
;))0 1
}))1 2
public** 
string** 
IDBloodGroup_Title** (
{**) *
get**+ .
;**. /
set**0 3
;**3 4
}**4 5
public++ 
string++ 
personLanguages++ %
{++& '
get++( +
;+++ ,
set++- 0
;++0 1
}++1 2
public,, 
string,, 
personCertificates,, (
{,,) *
get,,+ .
;,,. /
set,,0 3
;,,3 4
},,4 5
public-- 
string-- 
personGroups-- "
{--# $
get--% (
;--( )
set--* -
;--- .
}--. /
public.. 
string.. 
companyName.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}..- .
public// 
string// 

{//$ %
get//& )
;//) *
set//+ .
;//. /
}/// 0
public00 
DateTime00 
?00 
jobStartDate00 %
{00& '
get00( +
;00+ ,
set00- 0
;000 1
}001 2
public11 
string11 

{11$ %
get11& )
;11) *
set11+ .
;11. /
}11/ 0
public22 
DateTime22 
?22 

jobEndDate22 #
{22$ %
get22& )
;22) *
set22+ .
;22. /
}22/ 0
public33 
string33 !
jobLeavingDescription33 +
{33, -
get33. 1
;331 2
set333 6
;336 7
}337 8
public44 
Guid44 
?44 
manager144 
{44 
get44  #
;44# $
set44% (
;44( )
}44) *
public55 
Guid55 
?55 
manager255 
{55 
get55  #
;55# $
set55% (
;55( )
}55) *
public66 
Guid66 
?66 
manager366 
{66 
get66  #
;66# $
set66% (
;66( )
}66) *
public77 
Guid77 
?77 
manager477 
{77 
get77  #
;77# $
set77% (
;77( )
}77) *
public88 
Guid88 
?88 
manager588 
{88 
get88  #
;88# $
set88% (
;88( )
}88) *
public99 
Guid99 
?99 
manager699 
{99 
get99  #
;99# $
set99% (
;99( )
}99) *
public:: 
double:: 
?:: 
salary:: 
{:: 
get::  #
;::# $
set::% (
;::( )
}::) *
public;; 
string;; 
workExperiences;; %
{;;& '
get;;( +
;;;+ ,
set;;- 0
;;;0 1
};;1 2
public<< 
string<< 
gratutedSchooles<< &
{<<' (
get<<) ,
;<<, -
set<<. 1
;<<1 2
}<<2 3
public== 
string== 
lastGratutedSchool== (
{==) *
get==+ .
;==. /
set==0 3
;==3 4
}==4 5
public>> 
DateTime>> 
?>> 
lastGratutedDate>> )
{>>* +
get>>, /
;>>/ 0
set>>1 4
;>>4 5
}>>5 6
public?? 
string?? 

{??$ %
get??& )
;??) *
set??+ .
;??. /
}??/ 0
public@@ 
string@@ 

highSchool@@  
{@@! "
get@@# &
;@@& '
set@@( +
;@@+ ,
}@@, -
publicAA 
stringAA 

universityAA  
{AA! "
getAA# &
;AA& '
setAA( +
;AA+ ,
}AA, -
}BB 
}CC �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_GroupUsers.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_GroupUsers (
:) *

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
groupId		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
userId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public
string

{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_LinkedRoles.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_LinkedRoles )
:* +

{ 
public 
Guid 
? 
RoleId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
InnerRoleId		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
RolName 
{ 
get  #
;# $
set% (
;( )
}) *
public
string
InnerRolName
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PagesRole.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_PagesRole '
:( )

{ 
public 
Guid 
? 
roleid 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
action		 
{		 
get		 "
;		" #
set		$ '
;		' (
}		( )
}

 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PartialAssigment.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWSH_PartialAssigment .
:/ 0

{ 
public 
DateTime 
? 
	startDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
endDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
double

 
?

 
courseHours

 "
{

# $
get

% (
;

( )
set

* -
;

- .
}

. /
public 
string 
schoolDepartment &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
lesson 
{ 
get "
;" #
set$ '
;' (
}( )
public
double
?

hourlyWage
{
get
;
set
;
}
public 
string 
	staffName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonCertificate.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWSH_PersonCertificate /
:0 1

{ 
public 
Guid 
? 
CertificateTypeId &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public		 
DateTime		 
?		 
	StartDate		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
DateTime

 
?

 
EndDate

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

, -
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
int
?
CertificateTime
{
get
;
set
;
}
public 
DateTime 
? 
ExpirationDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string !
CertificateType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonCertificateType.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
VWSH_PersonCertificateType 3
:4 5

{ 
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public		 
string		 
createdby_Title		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
string

 
changedby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonCompetencies.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class #
VWSH_PersonCompetencies 0
:1 2

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
CompetenciesId		 #
{		$ %
get		& )
;		) *
set		+ .
;		. /
}		/ 0
public

 
int

 
?

 
CompetenciesLevel

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

User_Title
{
get
;
set
;
}
public 
string 
Competencies_Title (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string #
CompetenciesLevel_Title -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonCompetenciesPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class -
!VWSH_PersonCompetenciesPageReport :
{ 
public 
int %
ToplamPersonelYeterliligi ,
{- .
get/ 2
;2 3
set4 7
;7 8
}8 9
public		 
string		 )
SonEklenenPersonelYeterliligi		 3
{		4 5
get		6 9
;		9 :
set		; >
;		> ?
}		? @
public

 
string

 0
$EnÇokYeterliliğiBulunanKullanıcı

 6
{

7 8
get

9 <
;

< =
set

> A
;

A B
}

B C
public 
DateTime 
SonKayitTarihi &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
} 
}
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonInformation.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWSH_PersonInformation /
:0 1

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
Nationality		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
int

 
?

 
Gender

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
Military 
{ 
get "
;" #
set$ '
;' (
}( )
public
DateTime
?
MilitaryDoneDate
{
get
;
set
;
}
public 
int 
?  
MilitaryDoneDuration (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string  
MilitaryExemptDetail *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
string #
MilitaryProbationDetail -
{. /
get0 3
;3 4
set5 8
;8 9
}9 :
public 
DateTime 
? !
MilitaryProbationDate .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public 
string 
IDSerialNumber $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
string 
IDMotherName "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
IDFatherName "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 
IDBornLocation #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
int 
? 
IDBloodGroup  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
IDPreviousSurname '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
Guid 
? 
IDCity 
{ 
get !
;! "
set# &
;& '
}' (
public 
Guid 
? 
IDTown 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 

IDDistrict  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
	IDVillage 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
IDDeliveryLocation (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
IDDeliveryDetail &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
IDRecordNumber $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
DateTime 
? 
IDDeliveryDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public   
string   
IDVolumeNumber   $
{  % &
get  ' *
;  * +
set  , /
;  / 0
}  0 1
public!! 
string!! 
IDFamilyNumber!! $
{!!% &
get!!' *
;!!* +
set!!, /
;!!/ 0
}!!0 1
public"" 
string"" 
IDRowNumber"" !
{""" #
get""$ '
;""' (
set"") ,
;"", -
}""- .
public## 
string## 
PersonalMail## "
{### $
get##% (
;##( )
set##* -
;##- .
}##. /
public$$ 
string$$ 
PersonalHomePhone$$ '
{$$( )
get$$* -
;$$- .
set$$/ 2
;$$2 3
}$$3 4
public%% 
string%% 

{%%$ %
get%%& )
;%%) *
set%%+ .
;%%. /
}%%/ 0
public&& 
string&& 
EmergencyPhone&& $
{&&% &
get&&' *
;&&* +
set&&, /
;&&/ 0
}&&0 1
public'' 
string'' 

{''$ %
get''& )
;'') *
set''+ .
;''. /
}''/ 0
public(( 
string(( 
EmergencyProximity(( (
{(() *
get((+ .
;((. /
set((0 3
;((3 4
}((4 5
public)) 
string)) 
	Religious)) 
{))  !
get))" %
;))% &
set))' *
;))* +
}))+ ,
public** 
string** 
IDBornTownLocation** (
{**) *
get**+ .
;**. /
set**0 3
;**3 4
}**4 5
public++ 
string++ #
InsuranceIdentityNumber++ -
{++. /
get++0 3
;++3 4
set++5 8
;++8 9
}++9 :
public,, 
string,,  
IdentificationNumber,, *
{,,+ ,
get,,- 0
;,,0 1
set,,2 5
;,,5 6
},,6 7
public-- 
bool-- 
?-- 
hasAgi-- 
{-- 
get-- !
;--! "
set--# &
;--& '
}--' (
public.. 
string.. 
createdby_Title.. %
{..& '
get..( +
;..+ ,
set..- 0
;..0 1
}..1 2
public// 
string// 
changedby_Title// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
string00 
IDCity_Title00 "
{00# $
get00% (
;00( )
set00* -
;00- .
}00. /
public11 
string11 
IDTown_Title11 "
{11# $
get11% (
;11( )
set11* -
;11- .
}11. /
public22 
string22  
IDBornLocation_Title22 *
{22+ ,
get22- 0
;220 1
set222 5
;225 6
}226 7
public33 
string33 
Gender_Title33 "
{33# $
get33% (
;33( )
set33* -
;33- .
}33. /
public44 
string44 
MaritalStatus_Title44 )
{44* +
get44, /
;44/ 0
set441 4
;444 5
}445 6
public55 
string55 
Military_Title55 $
{55% &
get55' *
;55* +
set55, /
;55/ 0
}550 1
public66 
string66 
IDBloodGroup_Title66 (
{66) *
get66+ .
;66. /
set660 3
;663 4
}664 5
public77 
DateTime77 
?77 

JobEndDate77 #
{77$ %
get77& )
;77) *
set77+ .
;77. /
}77/ 0
public88 
string88 

User_Title88  
{88! "
get88# &
;88& '
set88( +
;88+ ,
}88, -
}99 
}:: �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonLanguages.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class  
VWSH_PersonLanguages -
:. /

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
int		 
?		 
	Languages		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
int

 
?

 
Reads

 
{

 
get

 
;

  
set

! $
;

$ %
}

% &
public 
int 
? 
Write 
{ 
get 
;  
set! $
;$ %
}% &
public 
int 
? 
Speak 
{ 
get 
;  
set! $
;$ %
}% &
public
Guid
?
CertificateTypeId
{
get
;
set
;
}
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 
EndDate  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
CertificateName %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
int 
? 
CertificateTime #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
DateTime 
? 
ExpirationDate '
{( )
get* -
;- .
set/ 2
;2 3
}3 4
public 
double 
? 
point 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
Languages_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Reads_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Write_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
Speak_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string !
CertificateType_Title +
{, -
get. 1
;1 2
set3 6
;6 7
}7 8
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonReferences.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWSH_PersonReferences .
:/ 0

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
ReferenceUserName		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
public

 
string

 
ReferencePosition

 '
{

( )
get

* -
;

- .
set

/ 2
;

2 3
}

3 4
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
ReferencePhone $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
string
ReferenceWorkingCompany
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonSchools.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_PersonSchools +
:, -

{ 
public 
DateTime 
? 
	StartDate "
{# $
get% (
;( )
set* -
;- .
}. /
public		 
DateTime		 
?		 
EndDate		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
Guid

 
?

 
UserId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
Guid 
? 
SchoolId 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
Level 
{ 
get 
;  
set! $
;$ %
}% &
public
string
Branch
{
get
;
set
;
}
public 
string 
area 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
Level_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
School_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
DateTime 
? 

JobEndDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_PersonWorkExperience.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class %
VWSH_PersonWorkExperience 2
:3 4

{ 
public 
Guid 
? 
UserId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
CompanyName		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
DateTime

 
?

 
JobStartDate

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
DateTime 
? 

JobEndDate #
{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
WorkingPosition %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
JobDescription
{
get
;
set
;
}
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Publications.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_Publications *
:+ ,

{ 
public 
string 
name 
{ 
get  
;  !
set" %
;% &
}& '
public		 
string		 
description		 !
{		" #
get		$ '
;		' (
set		) ,
;		, -
}		- .
public

 
DateTime

 
?

 
date

 
{

 
get

  #
;

# $
set

% (
;

( )
}

) *
public 
string 
keywords 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
changedby_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Report.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_Report $
:% &

{ 
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public		 
int		 
?		 
type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
schema

 
{

 
get

 "
;

" #
set

$ '
;

' (
}

( )
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string

type_Title
{
get
;
set
;
}
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Role.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	VWSH_Role "
:# $

{ 
public 
string 
rolname 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
roledescription		 %
{		& '
get		( +
;		+ ,
set		- 0
;		0 1
}		1 2
public

 
short

 
?

 
roletype

 
{

  
get

! $
;

$ %
set

& )
;

) *
}

* +
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
roleType_Title
{
get
;
set
;
}
public 
string 
Users_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
} 
} �	
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Schools.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_Schools %
:& '

{ 
public 
string 

SchoolName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
int		 
?		 
Type		 
{		 
get		 
;		 
set		  #
;		# $
}		$ %
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 

Type_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
}
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_ShiftTracking.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_ShiftTracking +
:, -

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
Guid		 
?		 
	companyId		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		* +
public

 
short

 
?

 
shiftTrackingStatus

 )
{

* +
get

, /
;

/ 0
set

1 4
;

4 5
}

5 6
public 
	IGeometry 
location "
{# $
get% (
;( )
set* -
;- .
}. /
public 
Guid 
? 

qrCodeData 
{  !
get" %
;% &
set' *
;* +
}+ ,
public
DateTime
?
	timestamp
{
get
;
set
;
}
public 
Guid 
? 
tableId 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
	tableName 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
qrCodeDataText $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public 
Guid 
? !
shiftTrackingDeviceId *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
int 
? 
passType 
{ 
get "
;" #
set$ '
;' (
}( )
public 
string 
deviceUserId "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
UserId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
public 
string 
CompanyId_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
table_Title !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string %
ShiftTrackingStatus_Title /
{0 1
get2 5
;5 6
set7 :
;: ;
}; <
public 
string 
passType_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_ShiftTrackingDevice.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class $
VWSH_ShiftTrackingDevice 1
:2 3

{ 
public 
string 

DeviceName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public		 
string		 

DeviceCode		  
{		! "
get		# &
;		& '
set		( +
;		+ ,
}		, -
public

 
string

 
DeviceBrand

 !
{

" #
get

$ '
;

' (
set

) ,
;

, -
}

- .
public 
string 
DeviceModel !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
DeviceSerialNo $
{% &
get' *
;* +
set, /
;/ 0
}0 1
public
	IGeometry
Location
{
get
;
set
;
}
public 
string 
	IPAddress 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
string 
Gateway 
{ 
get  #
;# $
set% (
;( )
}) *
public 
int 
? 
Port 
{ 
get 
; 
set  #
;# $
}$ %
public 
int 
? 

{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_ShiftTrackingDeviceUsers.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class )
VWSH_ShiftTrackingDeviceUsers 6
:7 8

{ 
public 
Guid 
? 
deviceId 
{ 
get  #
;# $
set% (
;( )
}) *
public		 
string		 
deviceUserId		 "
{		# $
get		% (
;		( )
set		* -
;		- .
}		. /
public

 
Guid

 
?

 
userId

 
{

 
get

 !
;

! "
set

# &
;

& '
}

' (
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
userId_Title
{
get
;
set
;
}
public 
string 

DeviceName  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
DeviceSerialNo $
{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_Ticket.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
VWSH_Ticket $
{ 
public 
Guid 
id 
{ 
get 
; 
set !
;! "
}" #
public		 
Guid		 
?		 
userid		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
DateTime

 
?

 

createtime

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
DateTime 
? 
endtime  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
string 
IP 
{ 
get 
; 
set  #
;# $
}$ %
public
Guid
?
DeviceId
{
get
;
set
;
}
public 
string 

User_Title  
{! "
get# &
;& '
set( +
;+ ,
}, -
public 
int 
? 
TotalMinute 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
DeviceId_Title $
{% &
get' *
;* +
set, /
;/ 0
}0 1
} 
} �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_TicketPageReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class !
VWSH_TicketPageReport .
{ 
public 
string $
EnCokGirisYapanKullanici .
{/ 0
get1 4
;4 5
set6 9
;9 :
}: ;
public		 
string		 $
EnUzunSureDuranKullanici		 .
{		/ 0
get		1 4
;		4 5
set		6 9
;		9 :
}		: ;
}

 
} �V
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_User.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class 
	VWSH_User "
:# $

{ 
public 
string 
FullName 
{  
get! $
;$ %
set& )
;) *
}* +
public		 
int		  
PermitYearlyDeserved		 '
{		( )
get		* -
;		- .
set		/ 2
;		2 3
}		3 4
public

 
int

 
PermitYearlyUsed

 #
{

$ %
get

& )
;

) *
set

+ .
;

. /
}

/ 0
public 
double  
PermitExcuseDeserved *
{+ ,
get- 0
;0 1
set2 5
;5 6
}6 7
public 
double 
PermitExcuseUsed &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public
int
PermitYearlyUsable
{
get
;
set
;
}
public 
double 
PermitExcuseUsable (
{) *
get+ .
;. /
set0 3
;3 4
}4 5
public 
string 
searchField !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
int 
? 
PersonWorkingCount &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 

{$ %
get& )
;) *
set+ .
;. /
}/ 0
public 
bool 
? 
status 
{ 
get !
;! "
set# &
;& '
}' (
public 
int 
? 
type 
{ 
get 
; 
set  #
;# $
}$ %
public 
string 
code 
{ 
get  
;  !
set" %
;% &
}& '
public 
string 
	loginname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
	firstname 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
lastname 
{  
get! $
;$ %
set& )
;) *
}* +
public 
DateTime 
? 
birthday !
{" #
get$ '
;' (
set) ,
;, -
}- .
public 
string 
password 
{  
get! $
;$ %
set& )
;) *
}* +
public 
string 
title 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
email 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
phone 
{ 
get !
;! "
set# &
;& '
}' (
public 
string 
	cellphone 
{  !
get" %
;% &
set' *
;* +
}+ ,
public 
string 
address 
{ 
get  #
;# $
set% (
;( )
}) *
public 
Guid 
? 

locationId 
{  !
get" %
;% &
set' *
;* +
}+ ,
public   
string   
companyCellPhone   &
{  ' (
get  ) ,
;  , -
set  . 1
;  1 2
}  2 3
public!! 
string!!  
companyCellPhoneCode!! *
{!!+ ,
get!!- 0
;!!0 1
set!!2 5
;!!5 6
}!!6 7
public"" 
string"" 
companyOfficePhone"" (
{"") *
get""+ .
;"". /
set""0 3
;""3 4
}""4 5
public## 
string## "
companyOfficePhoneCode## ,
{##- .
get##/ 2
;##2 3
set##4 7
;##7 8
}##8 9
public$$ 
string$$  
IdentificationNumber$$ *
{$$+ ,
get$$- 0
;$$0 1
set$$2 5
;$$5 6
}$$6 7
public%% 
Guid%% 
?%% 
	CompanyId%% 
{%%  
get%%! $
;%%$ %
set%%& )
;%%) *
}%%* +
public&& 
string&& 
CompanyLogo&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&- .
public'' 
string'' 

{''$ %
get''& )
;'') *
set''+ .
;''. /
}''/ 0
public(( 
Guid(( 
?(( 
CompanyPersonId(( $
{((% &
get((' *
;((* +
set((, /
;((/ 0
}((0 1
public)) 
string)) 
CompanyPerson_Title)) )
{))* +
get)), /
;))/ 0
set))1 4
;))4 5
}))5 6
public** 
int** 
?** 
CompanyPerson_Level** '
{**( )
get*** -
;**- .
set**/ 2
;**2 3
}**3 4
public++ 
string++ !
JobLeavingDescription++ +
{++, -
get++. 1
;++1 2
set++3 6
;++6 7
}++7 8
public,, 
int,, 
?,, 

JobLeaving,, 
{,,  
get,,! $
;,,$ %
set,,& )
;,,) *
},,* +
public-- 
string-- 
JobLeaving_Title-- &
{--' (
get--) ,
;--, -
set--. 1
;--1 2
}--2 3
public.. 
DateTime.. 
?.. 

JobEndDate.. #
{..$ %
get..& )
;..) *
set..+ .
;... /
}../ 0
public// 
DateTime// 
?// 
JobStartDate// %
{//& '
get//( +
;//+ ,
set//- 0
;//0 1
}//1 2
public00 
bool00 
?00 
	IsWorking00 
{00  
get00! $
;00$ %
set00& )
;00) *
}00* +
public11 
Guid11 
?11 
Manager111 
{11 
get11  #
;11# $
set11% (
;11( )
}11) *
public22 
string22 
Manager1_Title22 $
{22% &
get22' *
;22* +
set22, /
;22/ 0
}220 1
public33 
Guid33 
?33 
Manager233 
{33 
get33  #
;33# $
set33% (
;33( )
}33) *
public44 
string44 
Manager2_Title44 $
{44% &
get44' *
;44* +
set44, /
;44/ 0
}440 1
public55 
Guid55 
?55 
Manager355 
{55 
get55  #
;55# $
set55% (
;55( )
}55) *
public66 
string66 
Manager3_Title66 $
{66% &
get66' *
;66* +
set66, /
;66/ 0
}660 1
public77 
Guid77 
?77 
Manager477 
{77 
get77  #
;77# $
set77% (
;77( )
}77) *
public88 
string88 
Manager4_Title88 $
{88% &
get88' *
;88* +
set88, /
;88/ 0
}880 1
public99 
Guid99 
?99 
Manager599 
{99 
get99  #
;99# $
set99% (
;99( )
}99) *
public:: 
string:: 
Manager5_Title:: $
{::% &
get::' *
;::* +
set::, /
;::/ 0
}::0 1
public;; 
Guid;; 
?;; 
Manager6;; 
{;; 
get;;  #
;;;# $
set;;% (
;;;( )
};;) *
public<< 
string<< 
Manager6_Title<< $
{<<% &
get<<' *
;<<* +
set<<, /
;<</ 0
}<<0 1
public== 
Guid== 
?== 
DepartmentId== !
{==" #
get==$ '
;==' (
set==) ,
;==, -
}==- .
public>> 
Guid>> 
?>> %
CompanyPersonDepartmentId>> .
{>>/ 0
get>>1 4
;>>4 5
set>>6 9
;>>9 :
}>>: ;
public?? 
string?? 
companyId_Code?? $
{??% &
get??' *
;??* +
set??, /
;??/ 0
}??0 1
public@@ 
string@@ 
Department_Title@@ &
{@@' (
get@@) ,
;@@, -
set@@. 1
;@@1 2
}@@2 3
publicAA 
stringAA 

Type_TitleAA  
{AA! "
getAA# &
;AA& '
setAA( +
;AA+ ,
}AA, -
publicBB 
stringBB 
Status_TitleBB "
{BB# $
getBB% (
;BB( )
setBB* -
;BB- .
}BB. /
publicCC 
stringCC 
locationId_TitleCC &
{CC' (
getCC) ,
;CC, -
setCC. 1
;CC1 2
}CC2 3
publicDD 
stringDD 
RoleIdsDD 
{DD 
getDD  #
;DD# $
setDD% (
;DD( )
}DD) *
publicEE 
stringEE 
ProfilePhotoEE "
{EE# $
getEE% (
;EE( )
setEE* -
;EE- .
}EE. /
publicFF 
stringFF 
SchoolLevel_TitleFF '
{FF( )
getFF* -
;FF- .
setFF/ 2
;FF2 3
}FF3 4
publicGG 
stringGG 
Gender_TitleGG "
{GG# $
getGG% (
;GG( )
setGG* -
;GG- .
}GG. /
publicHH 
stringHH 
IBANHH 
{HH 
getHH  
;HH  !
setHH" %
;HH% &
}HH& '
}II 
}JJ �
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_UserEmailAccountShare.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class &
VWSH_UserEmailAccountShare 3
:4 5

{ 
public 
Guid 
? 

userIdFrom 
{  !
get" %
;% &
set' *
;* +
}+ ,
public		 
Guid		 
?		 
userIdTo		 
{		 
get		  #
;		# $
set		% (
;		( )
}		) *
public

 
bool

 
?

 
share

 
{

 
get

  
;

  !
set

" %
;

% &
}

& '
public 
string 
createdby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public
string
userIdTo_Title
{
get
;
set
;
}
public 
string 
userIdFrom_Title &
{' (
get) ,
;, -
set. 1
;1 2
}2 3
public 
string 
userIdFromEmail %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
} 
} �

�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_UserFireBaseToken.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
BusinessData *
{ 
public 

partial 
class "
VWSH_UserFireBaseToken /
:0 1

{ 
public 
Guid 
? 
userId 
{ 
get !
;! "
set# &
;& '
}' (
public		 
string		 
token		 
{		 
get		 !
;		! "
set		# &
;		& '
}		' (
public

 
string

 
createdby_Title

 %
{

& '
get

( +
;

+ ,
set

- 0
;

0 1
}

1 2
public 
string 
changedby_Title %
{& '
get( +
;+ ,
set- 0
;0 1
}1 2
public 
string 
UserId_Title "
{# $
get% (
;( )
set* -
;- .
}. /
}
} �o
�D:\PROJELER\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server\Infoline.WorkOfTime\Infoline.WorkOfTime.BusinessData\DatabaseObjects\VWSH_UserReport.cs
	namespace 	
Infoline
 
. 

WorkOfTime 
. 
{ 

