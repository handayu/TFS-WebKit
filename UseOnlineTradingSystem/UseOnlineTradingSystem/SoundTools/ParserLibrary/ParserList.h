#pragma once
#include "XParser.h"
class ParserList
{
private:
	CPtrList mParserList;
	CMapStringToPtr mParams;
	
public:
	ParserList(void);
	void Calc();
	~ParserList(void);
	LPVarInfo AddParam(CString Name,double Value);
	CValues* AddParam(CString Name);
	void AddParam(CString Name,CValues* Values);
	int SetString(CString str);
	int GetCount();
	void Clear();
	void Reset();
	const LPVarInfo GetRetVar(int Index);
	bool isHide(int Index);
	void GetRetVarName(CString& Name,int Index);
	void GetRetColor(CString& Color,int Index);
	const int GetRetOutputType(int Index);
};
