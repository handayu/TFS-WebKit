#pragma once
#include "XParserHeader.h"

void cleanup(CString& s0);
int GetFirstOpp(int Count, CString s0, int Index);
int GetFirstOpp(CString s0, int Index);
int GetFirstOpp(int Count, CString s0);
int GetOptYJX(char Opt);
void matchbracket(PINT i, CString s0);
void GetCalcString(CString& s0, CPtrList* list, char Key);
void ProcYJX(CString& str);
void Split(CString separator,CString str,CStringList* List);
void ClearRunValue(LPVarInfo Value);
class CParserException {
private:
	CString mMsg;
public:
   CParserException(CString Msg) {
	   mMsg=Msg;
   }
   ~CParserException() {};
   const WCHAR * ShowReason() const { 
	   return mMsg.GetString();

//      return mMsg;
   }
};
/// <summary>
/// ²Ù×÷·û³£Êý
/// </summary>
const LPTSTR sopps = L"+-*/^><=@#~&|";
const WCHAR RunVarChr=L';';
const WCHAR TempVarChr=L'~';
class XParser
{
private:
	UINT mCalcPostion;
	CString mRetValName;
	CPtrList mCallList;
	CMapStringToPtr mFuncList;
	CMapStringToPtr mParams;
	CMapStringToPtr mRunParams;
	CString mScrept;
	LPVarInfo CalcRunVal(LPVarInfo var);
	double calculate(WCHAR opt, double v1,double v2);
	LPVarInfo evaluate(LPCallVarInfo CallInfo);
	//LPVarInfo AllocSingleVar(CString Name,double Value,CMapStringToPtr* List);
	//LPVarInfo AllocVar(CString Name,CMapStringToPtr* List);
	void FreeVar(CString Name,CMapStringToPtr* List);
	LPVarInfo GetVarPtr(CString Name);
	void CallFunc(int Postion,LPVarInfo Ptr);
public:
	XParser(void);
	~XParser(void);
public :
	bool SetString(CString string);
	void AddParam(CString Name,double Value);
	void AddFunc(CString Name,PVOID Proc,int ParamCount);
	void AddParam(CString Name,LPVarInfo Value);
	CValues* AddParam(CString Name);
	void Clear();
	void Reset();
	PVOID Calc(bool& IsSingle);
	CString mVarName;
	bool mIsHide;
	int mOutputType;
	LPVarInfo mLastVar;
	CString mColor;
};
