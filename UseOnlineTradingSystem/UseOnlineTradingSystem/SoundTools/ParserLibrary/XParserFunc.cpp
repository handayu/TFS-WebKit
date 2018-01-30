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
		//�������������㷵��
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
		//�������������㷵��
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
		//�������������㷵��
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
		//�������������㷵��
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
		//�������������㷵��
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
		//�������������㷵��
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}

	/*EMA(X,N),��X��N��ָ��ƽ���ƶ�ƽ�����㷨����Y=EMA(X,N)
		��Y=[2*X+(N-1)*Y']/(N+1),����Y'��ʾ��һ����Yֵ��
		���磺EMA(CLOSE,30)��ʾ��30��ָ��ƽ������*/
	int p=Postion-1;
	if (p<0) p=0;
	double lr=GetVar(Result,p);
	if (lr>=MAXDWORD) //�����һ������ֵ�Ǵ�������õ�ǰ����ĵ�һ��ֵ
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
		//�������������㷵��
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
		//�������������㷵��
		SetVar(Result,MAXDWORD,Postion);
		return ;
	}
	double m=GetVar(M,Postion);

	/*	��Y=[(X*M+Y1*(N-M)]/N,����Y'��ʾ��һ����Yֵ��*/
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