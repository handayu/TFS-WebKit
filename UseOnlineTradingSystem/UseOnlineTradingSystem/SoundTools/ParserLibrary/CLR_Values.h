#pragma once
#include "Values.h"
#include "XParserHeader.h"
using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
using namespace System::Threading;
namespace ParserTools {
	public ref class CLR_Values : public ICLR_Values  
	{
	private:
		bool OwnerIsMe;
		String^ mName;
		Object^ mTag;
		bool mUpdating;
		ReaderWriterLock^ mLock;
		int GetCount();
	internal:
		CValues* mValues;
		LPVarInfo mVarInfo;
	public:
		property String^ Name
		{
			virtual String^ get()
			{
				return mName;
			}
		};
		property Object^ Tag
		{
			virtual Object^ get()
			{
				return mTag;
			}
			virtual void set(Object^ value)
			{
				mTag=value;
			}
		}
		property bool IsSingle
		{
			virtual bool get()
			{
				if (mVarInfo!=NULL)
				{
					if (mVarInfo->VarType == ::Single)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}
		
		property int Count
		{
			virtual int get()
			{
				if (mUpdating) return 0;
				return GetCount();
			}
		}
		
		property double default[int]
		{
			virtual double get(int Index)
			{
				if (mUpdating) return ParserHelper::Invalidation;
				mLock->AcquireReaderLock(-1);
				try
				{
					if (mUpdating) return ParserHelper::Invalidation;
					if (mVarInfo!=NULL)
					{
						if (mVarInfo->VarType == ::Single)
							return mVarInfo->Value;
						else
							return (*mValues)[Index];
					}
					else
						return (*mValues)[Index];
				}finally
				{
					mLock->ReleaseReaderLock();
				}
			}
			virtual void set(int Index, double value) 
			{
				mLock->AcquireWriterLock(-1);
				try
				{
					if (mVarInfo!=NULL)
					{
						if (mVarInfo->VarType == ::Single)
							mVarInfo->Value=value;
						else
							(*mValues)[Index]=value;
					}
					else
						(*mValues)[Index]=value;
				}
				finally
				{
					mLock->ReleaseWriterLock();
				}
			}
		};
		CLR_Values(String^ Name,CValues* Values);
		CLR_Values(String^ Name,LPVarInfo Values);
		CLR_Values(String^ Name,bool Single);
		virtual int GetMaxMin(int Start,int Count,double %Max, double %Min);
		virtual ~CLR_Values();
		!CLR_Values();
		//double& operator[](int Index);
		virtual void BeginUpdate();
		virtual void EndUpdate();
		virtual void SetCount(int Count);
		virtual void Clear();
		virtual void Add(double Value);
		virtual void Insert(int Index, IEnumerable<double>^ value);
	};
}
