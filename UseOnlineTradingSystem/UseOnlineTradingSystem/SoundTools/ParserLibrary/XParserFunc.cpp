#include "StdAfx.h"
#include "XParserFunc.h"
#include "math.h"



void XP_ABS(int Postion, LPVarInfo Param, LPVarInfo Result)
{
	double v=GetVar(Param,Postion);
	v =GetVar (Param,Postion);
	if (v<0)
		v=0-v;
	SetVar(Result,v,Postion);
}
void XP_STD(int Postion,LPVarInfo Param,LPVarInfo Count, LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double v=0;
	int p=0;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t>=MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return ;
		}
		v+=t;
		p++;
	}
	double m=v/p;
	v=0;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t>=MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return ;
		}
		v+=pow(t-m,2);
		
	}
	SetVar(Result,sqrt(v/(p-1)),Postion);
}
void XP_SUM(int Postion, LPVarInfo Param,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double v=0;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t==MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return;
		}
		v+=t;
	}
	SetVar(Result,v,Postion);
}

void XP_MA(int Postion, LPVarInfo Param,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double v=0;
	int p=0;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t>=MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return ;
		}
		v+=t;
		p++;
	}
	SetVar(Result,v/p,Postion);
}
void  XP_HHV(int Postion,LPVarInfo Param,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double v=MINLONG32;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t>=MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return ;
		}
		if (t>v)
			v=t;
	}
	if (v==MINLONG32)
		v=MAXDWORD;
	SetVar(Result,v,Postion);
}
void  XP_LLV(int Postion,LPVarInfo Param,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<=c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}

	double v=MAXLONG32;
	for (int i=Postion-c+1;i<=Postion;i++)
	{
		double t=GetVar(Param,i);
		if (t>=MAXDWORD)
		{
			SetVar(Result,MAXDWORD,Postion);
			return ;
		}
		if (t<v)
			v=t;
	}
	if (v==MAXLONG32)
		v=MAXDWORD;
	SetVar(Result,v,Postion);
}
void XP_EMA(int Postion, LPVarInfo Param,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if ((c<=0) || (Postion<=c-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}

	/*EMA(X,N),求X的N日指数平滑移动平均。算法：若Y=EMA(X,N)
		则Y=[2*X+(N-1)*Y']/(N+1),其中Y'表示上一周期Y值。
		例如：EMA(CLOSE,30)表示求30日指数平滑均价*/
	int p=Postion-1;
	if (p<0) p=0;
	double lr=GetVar(Result,p);
	if (lr>=MAXDWORD) //如果上一个返回值是错误的则用当前数组的第一个值
	{
		lr=GetVar(Param,Postion);
	}
	double xs=(double)2/(c+1);
	double t=GetVar(Param,Postion);
	double v= (t-lr)*xs+lr;
	//for (int i=Postion-c+1;i<=Postion;i++)
	//{
	//	double t=GetVar(Param,i);
	//	

	//	if (t>=MAXDWORD)
	//	{
	//		SetVar(Result,MAXDWORD,Postion);
	//		return ;
	//	}

	//}
	//double v=(2*GetVar(Param,Postion)+(c-1)*lr)/(c+1);
	SetVar(Result,v,Postion);
}
void  XP_MAX(int Postion,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Result)
{
	double v1=GetVar(Param1,Postion);
	double v2=GetVar(Param2,Postion);
	SetVar(Result,max(v1,v2),Postion);
}
void  XP_MIN(int Postion,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Result)
{
	double v1=GetVar(Param1,Postion);
	double v2=GetVar(Param2,Postion);
	SetVar(Result,min(v1,v2),Postion);
}
void  XP_REF(int Postion,LPVarInfo Param ,LPVarInfo Count,LPVarInfo Result)
{
	int c=(int)GetVar(Count,Postion);
	if (c<0)
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	int pos=Postion-c;
	if (pos<0)
		pos=0;
	double v=GetVar(Param,pos);
	SetVar(Result,v,Postion);
}
void XP_SMA(int Postion, LPVarInfo Param,LPVarInfo N,LPVarInfo M,LPVarInfo Result)
{
	int n=(int)GetVar(N,Postion);
	if ((n<=0) || (Postion<=n-1))
	{
		//计算条件不满足返回
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double m=GetVar(M,Postion);

	/*	则Y=[(X*M+Y1*(N-M)]/N,其中Y'表示上一周期Y值。*/
	int p=Postion-1;
	if (p<0) p=0;
	double lr=GetVar(Result,p);
	if (lr>=MAXDWORD)
	{
		lr=GetVar(Param,Postion);
	}
	double v=(m*GetVar(Param,Postion)+(n-m)*lr)/(n);
	SetVar(Result,v,Postion);
}