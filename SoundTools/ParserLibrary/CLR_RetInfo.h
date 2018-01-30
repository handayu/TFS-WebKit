#pragma once
#include "Values.h"
#include "XParserHeader.h"
#include "CLR_Values.h"
using namespace System;
using namespace System::Text;
using namespace System::Drawing;
using namespace System::Collections::Generic;
namespace ParserTools {
	
	public ref class CLR_RetInfo
	{
	private:
		CLR_Values^ mValues;
	public:
		CLR_RetInfo(CLR_Values^ Values);
		int LineType;
		System::Drawing::Color^ color;
		property CLR_Values^ Values
		{
			CLR_Values^ get()
			{
				return mValues;
			}
		}
		String^ Name;

	};
}
