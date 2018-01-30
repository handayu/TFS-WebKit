// Values.cpp : 实现文件
//

#include "stdafx.h"
#include "Values.h"
#include <math.h>


// CValues

CValues::CValues()
{
	mCount =0;
	mPage=NULL;
	mPageCount =0;
	mAllCount=0;
 	for (int i=0;i<MaxPageCount;i++)
	{
		mPages[i]=NULL;
	}
}

CValues::~CValues()
{
	Clear();
}

int CValues::Add(double Value)
{
	if ((mCount>=PageCount) || (mPage==NULL))
	{
		mPage=(double*)malloc(sizeof(double)*PageCount);
		mCount=0;
		mPages[mPageCount]=mPage;
		mPageCount++;
	}
	mPage[mCount]=Value;
	mCount++;
	mAllCount ++;
	return mAllCount-1;
}

void CValues::Clear()
{
	for (int i=0;i<mPageCount;i++)
	{
		LPDOUBLE p= (LPDOUBLE)mPages[i];
		
		free(p);
		mPages[i]=NULL;
	}
	mCount =0;
	mPage=NULL;
	mPageCount =0;
	mAllCount=0;
	mPageCount=0;
}

int CValues::GetCount() 
{
	return mAllCount;
}
double& CValues::operator [](int Index)
{
	int page=Index/PageCount;
	return mPages[page][Index%PageCount];
	//return *v;
}

// CValues 成员函数
