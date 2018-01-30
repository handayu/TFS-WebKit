#ifndef _XParser_
#define _XParser_
#include "Values.h"
typedef struct _CallVarInfo
{
	CString Name;
	CString Parse;
	CString Func;
}*LPCallVarInfo,CCallVarInfo;
enum EVarType {Single,Temp,RunVar,Array};
typedef struct _FuncInfo
{
	CString Name;
	int ParamCount;
	PVOID CallAddr;
}*LPFuncInfo,CFuncInfo;

typedef struct _VarInfo
{
	CString Name;
	EVarType VarType;
	PVOID Link;
	WCHAR Opt;
	PVOID RunVar1;
	PVOID RunVar2;
	LPFuncInfo Func;
	double Value;
	bool LinkFlag;
	CPtrList * CallPtr;
	CValues * Values;
	bool IsSys;
	bool OwnerIsMe;
}*LPVarInfo,CVarInfo;


/*inline double* GetVarAddr(LPVarInfo var,int index)
{
	if ((var->VarType ==Single) || (var->VarType ==Temp))
	{
		return &var->Value;
	}
	else
		if ((var->VarType==Array) || (var->VarType ==RunVar))
		{
			return  (*var->Values)[index]; 
		}
		else
			if (var->VarType==Link)
			{
				return GetVar((LPVarInfo)var->Link,index);
			}
			else
				return 0;
}*/
inline double GetVar(LPVarInfo var,int index)
{
	if (var->LinkFlag)
	{
		return GetVar((LPVarInfo)var->Link,index);
	}else
		if ((var->VarType ==Single) || (var->VarType ==Temp))
		{
			return var->Value;
		}
		else
			if ((var->VarType==Array) || (var->VarType ==RunVar))
			{
				return  (*var->Values)[index]; 
			}
			else
				return 0;
}
inline void SetVar(LPVarInfo var,double Value,int index)
{	
	if (var->LinkFlag)
	{
		SetVar((LPVarInfo)var->Link,Value,index);
	}else
		if ((var->VarType ==Single) || (var->VarType ==Temp))
		{
			var->Value=Value;
		}
		else
			if ((var->VarType==Array) || (var->VarType ==RunVar))
			{
				if (index>=var->Values->GetCount())
				{
					int c=index-var->Values->GetCount()+1;
					for (int i=0;i<c;i++)
						var->Values->Add(0);
				}
				(*var->Values)[index]=Value;
			}
}

inline int GetVarCount(LPVarInfo var)
{
	if (var->LinkFlag)
	{
		return GetVarCount((LPVarInfo)var->Link);
	}
	else
		if ((var->VarType ==Single) || (var->VarType ==Temp))
		{
			return 1;
		}
		else
			if ((var->VarType==Array) || (var->VarType ==RunVar))
			{
				return var->Values->GetCount();
			}
			else
				return 0;
}
#endif 