#include "StdAfx.h"
#include "CLR_ParserList.h"

namespace ParserTools {
	
	CLR_ParserList::CLR_ParserList(void)
	{
		mParserList=new ParserList();
		mParams =gcnew List<CLR_Values^>();
		mRets= gcnew List<CLR_RetInfo^>();
	}
	CLR_ParserList ::~CLR_ParserList(void)
	{
		delete mParserList;
	}
	void CLR_ParserList::AddParam(String^ Name,CLR_Values^ Values)
	{
		mMonitor->Enter(this);
		try
		{
			if (Values->mVarInfo !=NULL)
			{
				if (Values->mVarInfo->VarType ==::Single)
				{
					mParserList->AddParam(Name,Values->mVarInfo->Value);
				}
			}
			else
			mParserList->AddParam(Name,Values->mValues);
		}
		catch (CParserException * err)
		{
			throw gcnew Exception(gcnew String(err->ShowReason()));
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}
	CLR_Values^ CLR_ParserList::AddParam(String^ Name,double Value)
	{
		mMonitor->Enter(this);
		try
		{
			LPVarInfo cv=mParserList->AddParam(Name,Value);
			CLR_Values^ pv=gcnew CLR_Values(Name,cv);	
			mParams->Add(pv);
			return pv;
		}
		catch (CParserException * err)
		{
			throw gcnew Exception(gcnew String(err->ShowReason()));
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}	
	CLR_Values^ CLR_ParserList::AddParam(String^ Name)
	{
		mMonitor->Enter(this);
		try
		{
			CValues *cv=mParserList->AddParam(Name);
			CLR_Values^ pv=gcnew CLR_Values(Name,cv);	
			mParams->Add(pv);
			return  pv;
		}
		catch (CParserException * err)
		{
			throw gcnew Exception(gcnew String(err->ShowReason()));
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}

	List<CLR_RetInfo^>^ CLR_ParserList::GetRetValues()
	{
		mMonitor->Enter(this);
		try
		{
			return mRets;
		}
		finally
		{
			mMonitor->Exit(this);
		}
		//return gcnew LIST;
	}
	String^ CLR_ParserList::GetRetName(int Index)
	{
		mMonitor->Enter(this);
		try
		{
			return mRets[Index]->Name;
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}
	
	void CLR_ParserList::Reset()
	{
		mMonitor->Enter(this);
		try
		{
			mParserList->Reset();
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}
	void  CLR_ParserList::Calc()
	{
		mMonitor->Enter(this);
		try
		{
			mParserList->Calc();
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}
	void CLR_ParserList::Clear()
	{
		mMonitor->Enter(this);
		try
		{
			mParserList->Clear();
			for (int i=0;i<mParams->Count;i++)
			{
				CLR_Values^ cv= mParams[i];
				delete cv;
			}
			mRets->Clear();
			mParams->Clear();
		}
		finally
		{
			mMonitor->Exit(this);
		}

	}

	//设置计算脚本，并返回每个计算脚本的数组集合类
	List<CLR_RetInfo^>^ CLR_ParserList::SetParseText(String^ Text)
	{
		mMonitor->Enter(this);
		try
		{
			mRets->Clear();
			mParserList->SetString(Text);
			
			for (int i=0;i<mParserList->GetCount();i++)
			{
				if (mParserList->isHide(i))
					continue;

				LPVarInfo info= mParserList->GetRetVar(i);
				CString Name;
				CString color;
				mParserList->GetRetVarName(Name,i);
				String^ str=gcnew String(Name);
				CLR_RetInfo^ retinfo=gcnew CLR_RetInfo(gcnew CLR_Values(str,info));
				mParserList->GetRetColor(color,i);
				retinfo->color=ParserHelper::StringToColor(gcnew String(color));
				retinfo->LineType =mParserList->GetRetOutputType(i);
				retinfo->Name =gcnew String(str);
				mRets->Add(retinfo);
			}
			mScript=Text;
			return mRets;
		}
		catch (CParserException * err)
		{
			throw gcnew Exception(gcnew String(err->ShowReason()));
		}
		finally
		{
			mMonitor->Exit(this);
		}
	}

}