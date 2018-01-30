#pragma once
#include "ParserList.h"
#include "CLR_Values.h"
#include "CLR_RetInfo.h"
using namespace System;
using namespace System::Text;
using namespace System::Collections::Generic;
namespace ParserTools {

	/*public ref class CLR_ParserException:Exception
	{
	public:
		CLR_ParserException(void);
	};*/
	public ref class CLR_ParserList
	{
	private:
		ParserList* mParserList;
		String^ mScript;
		List<CLR_Values^>^ mParams;
		List<CLR_RetInfo^>^ mRets;
		Monitor^ mMonitor;
	public:
		CLR_ParserList(void);
		virtual ~CLR_ParserList();
		void Clear();
		void Calc();
		void Reset();
		String^ GetRetName(int Index);
		CLR_Values^ AddParam(String^ Name);
		void AddParam(String^ Name,CLR_Values^ Values);
		CLR_Values^  AddParam(String^ Name,double Value);
		List<CLR_RetInfo^>^  SetParseText(String^ Text);
		List<CLR_RetInfo^>^ GetRetValues();
		property String^ Script
		{
			String^ get()
			{
				mMonitor->Enter(this);
				try
				{
					return mScript;
				}finally
				{
					mMonitor->Exit(this);
				}
			}
		}
		property int ParamCount
		{
			int get()
			{
				mMonitor->Enter(this);
				try
				{
					return mParams->Count;
				}finally
				{
					mMonitor->Exit(this);
				}
			}
		}
		property CLR_Values^ Params[int]
		{
			CLR_Values^ get(int Index)
			{	
				mMonitor->Enter(this);
				try
				{
					return mParams[Index];
				}finally
				{
					mMonitor->Exit(this);
				}
			}
		}
		property int RetCount
		{
			int get()
			{
				mMonitor->Enter(this);
				try
				{
					return mParserList->GetCount();
				}finally
				{
					mMonitor->Exit(this);
				}
			}
		}
		
	};
}
