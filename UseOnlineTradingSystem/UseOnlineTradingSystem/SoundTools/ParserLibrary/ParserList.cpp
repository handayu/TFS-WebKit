#include "StdAfx.h"
#include "ParserList.h"
#include "XParser.h"

void ParserList::Clear()
{
	for (POSITION pos=mParserList.GetHeadPosition ();pos!=NULL;)
	{
		XParser * par =(XParser *) mParserList.GetNext(pos);
		delete par;
	}
	mParserList.RemoveAll();
	for (POSITION pos=mParams.GetStartPosition ();pos!=NULL;)
	{			
		CString vn;
		LPVarInfo vl;
		mParams.GetNextAssoc(pos,vn,(void*&)vl);
		if (vl->OwnerIsMe)
		{
			if (vl->VarType ==Array)
			{
				delete vl->Values;
			}
		}
		delete vl;
	}
	mParams.RemoveAll();

}
void ParserList::Reset()
{
	for (POSITION pos=mParserList.GetHeadPosition ();pos!=NULL;)
	{
		XParser * par =(XParser *) mParserList.GetNext(pos);
		par->Reset();
	}
}
int ParserList::GetCount()
{
	return (int)mParserList.GetCount();
}
const LPVarInfo ParserList::GetRetVar(int Index)
{
	XParser* xp=(XParser*) mParserList.GetAt(mParserList.FindIndex(Index));
	return xp->mLastVar;
}
void ParserList::GetRetColor(CString& Color,int Index)
{
	XParser* xp=(XParser*) mParserList.GetAt(mParserList.FindIndex(Index));
	Color=xp->mColor;
}
const int ParserList::GetRetOutputType(int Index)
{
	XParser* xp=(XParser*) mParserList.GetAt(mParserList.FindIndex(Index));
	return xp->mOutputType;
}
void ParserList::GetRetVarName(CString& Name,int Index)
{
	XParser* xp=(XParser*) mParserList.GetAt(mParserList.FindIndex(Index));
	Name =xp->mVarName;
}
bool ParserList::isHide(int Index)
{
	XParser* xp=(XParser*) mParserList.GetAt(mParserList.FindIndex(Index));
	return xp->mIsHide;
}
int ParserList::SetString(CString str)
{
	if (mParserList.GetCount())
		throw new CParserException(L"���ȵ���������");
	if (str.Find(L"&")>0)
		throw new CParserException(L"����һЩ���ܽ������������");
	CStringList list;
	Split(L"\n",str,&list);
	LPVarInfo Last=NULL;
	CString varname;
	CMapStringToPtr RetList;
	for (POSITION pos=list.GetHeadPosition ();pos!=NULL;)
	{
		CString s=list.GetNext(pos);
		if (!s.Trim().IsEmpty()) 
		{
			XParser * par=new XParser();
			for (POSITION vp=mParams.GetStartPosition();vp!=NULL;)
			{
				CString vn;
				LPVarInfo vl;
				mParams.GetNextAssoc(vp,vn,(void*&)vl);
				par->AddParam(vn,vl);
			}
			if (Last!=NULL)
			{
				RetList.SetAt(varname,Last);

				//par->AddParam(varname,Last); //��ǰһ�μ�������Ϊ����������һ�й�ʽ��
			}
			for (POSITION pp=RetList.GetStartPosition();pp!=NULL;)
			{
				CString vn;
				LPVarInfo vl;
				RetList.GetNextAssoc(pp,vn,(void*&)vl);
				par->AddParam(vn,vl);
			}
			par->SetString(s);
			if (par->mVarName.IsEmpty())
			{
				throw new CParserException(L"��ʽû���κ����");
			}
			mParserList.AddTail(par); 
			varname=par->mVarName;
			Last=par->mLastVar; 
		}
	}
	return (int)mParserList.GetCount();
}

void ParserList::AddParam(CString Name,CValues* Values)
{
	cleanup(Name);
	if (Name.Find(RunVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	if (Name.Find(TempVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}
	Name.MakeLower();
	if (mParams[Name]!=NULL)
	{
		throw new CParserException(L"�ظ��Ĳ���");
	}
	LPVarInfo v= new CVarInfo();
	v->VarType =Array;
	v->LinkFlag =false;
	v->Name =Name;
	v->IsSys =false;
 	v->Values =Values;
	v->OwnerIsMe=false;
	mParams.SetAt(Name,v);
}
//��̬���Ͳ���
CValues* ParserList::AddParam(CString Name)
{
	cleanup(Name);
	if (Name.Find(RunVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	if (Name.Find(TempVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	Name.MakeLower();
	//UINT hash= mParams.HashKey(Name);
//	if (mParams(Name)>=0)
	if (mParams[Name]!=NULL)
	{
		throw new CParserException(L"�ظ��Ĳ���");
	}
	LPVarInfo v= new CVarInfo();
	v->VarType =Array;
	v->Name =Name;
	v->IsSys =false;
 	v->Values =new CValues();
	v->LinkFlag =false;
	mParams.SetAt(Name,v);
	v->OwnerIsMe=true;
	return v->Values;
}
LPVarInfo ParserList::AddParam(CString Name,double Value)
{
	cleanup(Name);
	if (Name.Find(RunVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	if (Name.Find(TempVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	Name.MakeLower();
	//UINT hash= mParams.HashKey(Name);
//	if (mParams(Name)>=0)
	if (mParams[Name]!=NULL)
	{
		throw new CParserException(L"�ظ��Ĳ���");
	}
	LPVarInfo v= new CVarInfo();
	v->Name =Name;
	v->VarType =Single;
	v->Value =Value;
	v->IsSys =false;
	v->OwnerIsMe=true;
	v->LinkFlag=false;
	mParams.SetAt(Name,v);
	return v;
}
void ParserList::Calc()
{
	for (POSITION pos=mParserList.GetHeadPosition();pos!=NULL;)
	{
		XParser* par=(XParser*) mParserList.GetNext(pos);
		bool IsSingle=false;
		par->Calc(IsSingle);
	}
	
}
ParserList::ParserList(void)
{
}
ParserList::~ParserList(void)
{
	Clear();
}
