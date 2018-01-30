#include "StdAfx.h"
#include "CLR_Values.h"
#include "XParserHeader.h"
namespace ParserTools {
	CLR_Values::CLR_Values(String^ Name,CValues* Values)
	{
		if (Values==NULL)
		{
			throw;
		}
		mValues=Values;
		mName=Name;
		mVarInfo=NULL;
		mLock =gcnew ReaderWriterLock();
		OwnerIsMe=false;
		mUpdating =false;
	}

	void CLR_Values:: Insert(int Index, IEnumerable<double>^ value)
	{
	}

	CLR_Values::CLR_Values(String^ Name,bool Single)
	{
		mName=Name;
		if (Single)
		{
			mVarInfo =new CVarInfo();
			mVarInfo->VarType =::Single;
			mVarInfo->Value =0;
			mVarInfo->Values =NULL;
			mVarInfo->Link =NULL;
			mVarInfo->LinkFlag =false;
			mVarInfo->RunVar1 =NULL;
			mVarInfo->RunVar2 =NULL;
			mValues=NULL;
		}
		else
		{
			mVarInfo =NULL;
			mValues=new CValues();

		}
		OwnerIsMe=true;
		mLock =gcnew ReaderWriterLock();
	}
	CLR_Values::CLR_Values(String^ Name,LPVarInfo Values)
	{
		mName=Name;
		mVarInfo=Values;
		if (Values->VarType !=::Single)
		{
			mValues=Values->Values;

		}else
			mValues=NULL;
		OwnerIsMe=false;
		
		mLock =gcnew ReaderWriterLock();
	}

	void CLR_Values::SetCount(int Count)
	{
		mLock->AcquireWriterLock(-1);
		try
		{
			if (mVarInfo==NULL)
			{
				mValues->Clear();
				for (int i=0;i<Count;i++)
					mValues->Add(ParserHelper::Invalidation);
			}
		}finally
		{
			mLock->ReleaseWriterLock();
		}
	}
	void CLR_Values::Add(double Value)
	{
		mLock->AcquireWriterLock(-1);
		try
		{
			if (mVarInfo!=NULL)
			{
				if (mVarInfo->VarType == ::Single)
				{
					mVarInfo->Value=Value;
				}
			}
			else
			{
				mValues->Add(Value);
			}
		}finally
		{
			mLock->ReleaseWriterLock();
		}
	}

	int CLR_Values::GetMaxMin(int Start,int Count,double %Max,double %Min)
	{
		if (mUpdating)
		{
			Max=0;
			Min=0;
			return 0;
		}
		mLock->AcquireReaderLock(-1);
		try
		{
			if (Start<0)
			{
				Start=0;
			}
			if (mVarInfo !=NULL)
			{
				if (mVarInfo->VarType == ::Single)
				{
					Max=mVarInfo->Value;
					Min=mVarInfo->Value;
					return 1;
				}
			}
			Max=ParserHelper::Invalidation;
			Min=ParserHelper::Invalidation;
			if (Count<=0) 
			{
				return 0;
			}
			int maxcount=mValues->GetCount();
			if (maxcount<=0) return 0;
			int p=maxcount-Start-1;
			if (p<0) return 0;
			double tMax=MININT32;
			double tMin=MAXINT32;
			
			int c=0;
			while (c<Count)
			{
				double v=(*mValues)[p];
				if (v!=ParserHelper::Invalidation)
				{
					if (v>tMax) tMax=v;
					if (v<tMin) tMin=v;
				}
				p--;
				c++;
				if (p<0) break;
			}
			if (tMax!=MININT32)
				Max=tMax;
			if (tMin!=MAXINT32)
				Min=tMin;
			return c;
		}
		finally
		{
			mLock->ReleaseReaderLock();
		}
	}
	/*void CLR_Values::GetMaxMin(int Start,int %Count,double %Max,double %Min)
	{
		if (Start < 0)
		{
            Count += Start;
            Start = 0;
        }
		if (mVarInfo!=NULL)
		{
			if (mVarInfo->VarType == ::Single)
			{
				Max=mVarInfo->Value;
				Min=mVarInfo->Value;
				return;
			}
		}
		double tMax=MININT32;
		double tMin=MAXINT32;
		int c=Start;
		int maxcount=mValues->GetCount();
		if (c>=maxcount)
		{
			Max=0;
			Min=0;
			Count=0;
			return ;
			//throw gcnew RankException();
		}
		CValues* values=mValues;
		int p=0;
		while (true)
		{
			double v=(*values)[c];
			if (v!=MAXDWORD)
			{
				if (v>tMax) tMax=v;
				if (v<tMin) tMin=v;
			}
			p++;
			c++;
			if ((p>=Count) || (c>=maxcount)) break;
		}
		Count=p;
		if (tMax!=MININT32)
			Max=tMax;
		if (tMin!=MAXINT32)
			Min=tMin;
	}
*/
	int CLR_Values ::GetCount()
	{
		mLock->AcquireReaderLock(-1);
		try
		{
			if (mVarInfo!=NULL)
				return GetVarCount(mVarInfo);
			else
				return mValues->GetCount();
		}
		finally
		{
			mLock->ReleaseReaderLock();
		}
	}
	void CLR_Values::BeginUpdate()
	{
		mLock->AcquireWriterLock(-1);
		try
		{
			mUpdating=true;
		}finally
		{
			mLock->ReleaseWriterLock();
		}
	}
	void CLR_Values::EndUpdate()
	{
		mLock->AcquireWriterLock(-1);
		try
		{
			mUpdating=false;
		}finally
		{
			mLock->ReleaseWriterLock();
		}
	}
	void CLR_Values ::Clear()
	{
		mLock->AcquireWriterLock(-1);
		try
		{
			if (mVarInfo!=NULL)
			{
				if (mVarInfo->VarType == ::Single)
					mVarInfo->Value=0;
				else
					mVarInfo->Values-> Clear();
			}
			else
			mValues->Clear();
		}finally
		{
			mLock->ReleaseWriterLock();
		}
	}
	CLR_Values::!CLR_Values()
	{
		if (OwnerIsMe)
		{
			if (mVarInfo!=NULL)
			{
				delete mVarInfo;
			}
			else
			{
				delete mValues;
			}
		}
	}
	CLR_Values ::~CLR_Values(void)
	{
		this->!CLR_Values();
		
	}
}
