;#DropDB LlamachantFrameworkEasyTest

#Application LlamachantFramework.Win
#Application LlamachantFramework.Web
#IfDef LlamachantFramework.Win

*Action About...
 
*Action OK

#EndIf

#IfDef LlamachantFramework.Web
!Action Navigation(My Details)
#EndIf
