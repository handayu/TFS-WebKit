#pragma once

// CValues ÃüÁîÄ¿±ê
#define PageCount  5000
#define MaxPageCount 100
typedef double *LPDOUBLE;

class CValues 
{
private:
	int mPageCount;
	LPDOUBLE mPages[MaxPageCount];
	double* mPage;
	int mCount;
	int mAllCount;
public:
	CValues();
	int GetCount();
	int Add(double Value);
	void Clear();
	double& operator[](int Index);
	virtual ~CValues();
};


