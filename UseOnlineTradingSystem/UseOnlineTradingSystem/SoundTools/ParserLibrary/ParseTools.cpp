#include "StdAfx.h"
#include "ParseTools.h"
BOOL IsNumeric(CString strData,BOOL bIsFloat) 
{ 
	int iDots = 0; 
	int iNegatives = 0; 
	strData.TrimRight(); 
	strData.TrimLeft(); 
	for(int i = 0;i < strData.GetLength();i++) 
	{ 
		if(strData.GetAt(i) == '-') 
		{ 
			iNegatives ++; 
			continue; 
		} 
		if(strData.GetAt(i) == '.') 
		{ 
			if(bIsFloat == FALSE) return FALSE; 

			iDots++; 
			if(iDots >= 2) return FALSE; 

			continue; 
		} 
		if((strData.GetAt(i) <'0')||(strData.GetAt(i)>'9')) 
			return FALSE; 
	} 
	if(iNegatives >= 2) 
		return FALSE; 
	return TRUE; 
}
bool SToDouble(CString lpText,double* Value)
{
	if (!IsNumeric(lpText,true))
	{
		return false;
	}
	else
	{
		*Value= _wtof(lpText);
		return true;
	}
}
bool SToInt(CString lpText,int* Value)
{
	if (!IsNumeric(lpText,false))
	{
		return false;
	}else
	{
		*Value= _wtoi(lpText);
		return true;
	}
}
bool SToInt64(CString lpText,__int64* Value)
{
	if (!IsNumeric(lpText,false))
	{
		return false;
	}else
	{
		*Value= _wtoi64(lpText);
		return true;
	}
}

