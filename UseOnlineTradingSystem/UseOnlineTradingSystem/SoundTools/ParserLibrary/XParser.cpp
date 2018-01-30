#include "StdAfx.h"
#include "XParser.h"
#include "afxcoll.h"
#include "stdlib.h"
#include "ParseTools.h"
#include <valarray>
#include "XParserFunc.h"
#define _DEBUG
void Split(CString separator,CString str,CStringList* List)
{
	int i=str.Find(separator);
	if (i>=0)
	{
		CString tmp= str.Left(i);
		List->AddTail(tmp);
		str.Delete(0,i+1);
		Split(separator,str,List);
	}
	else
		List->AddTail(str);
}
void cleanup(CString& s0)
{
    int i = s0.Find(' ');
    while (i >= 0)
    {
        s0.Delete(i, 1);
        i = s0.Find(' ');
    }
    i = s0.Find(L"\x10");
    while (i >= 0)
    {
        s0.Delete(i, 1);
        i = s0.Find(L"\x10");
    }
    i = s0.Find(L"\x13");
    while (i >= 0)
    {
        s0.Delete(i, 1);
        i = s0.Find(L"\x13");
    }
}

/// <summary>
/// ���ҵ�һ��������
/// </summary>
/// <param name="Count">��������λ��</param>
/// <param name="s0"></param>
/// <returns></returns>
int GetFirstOpp(int Count, CString s0, int Index)
{
    if (Count == 0) Count = s0.GetLength();
    int min = MAXINT;
	CString opps=sopps;
	
    for (int i = 0; i < opps.GetLength(); i++)
    {
        int ret = s0.Find(sopps[i], Index);
        //����ҵ�+-��
        if ((i < 2) && (ret >= 0))
        {
			if (ret == s0.GetLength() - 1) throw new CParserException(L"����һ�������Ĺ�ʽ");
			//����ҵ�+-��,�������м�
			if (ret==0)
			{
				if (opps.Find(s0[ret + 1]) >= 0) throw new  CParserException(L"����һ�������Ĺ�ʽ");
				ret=s0.Find(opps[i],ret+1);
			}
			else
			{
				 //����ҵ���������s0���м䣬��ô�˳��������������
				if (opps.Find(s0[ret - 1]) >= 0)
				{
					if (opps.Find(s0[ret + 1]) >= 0) throw new  CParserException(L"����һ�������Ĺ�ʽ");
					else
						continue;
				}
			}
			
        }

		if ((ret >= 0) && (ret < Count) && (ret < min))
			min = ret;
    }
    if (min != MAXINT)
    {
        if (min > Count) min = 0;
    }
    else
        min = -1;
    return min;
}
int GetFirstOpp(CString s0, int Index)
{
    return GetFirstOpp(s0.GetLength(), s0, Index);
}
int GetFirstOpp(int Count, CString s0)
{
    return GetFirstOpp(Count, s0, 0);
}

/// <summary>
/// ���Ҷ�Ӧ������
/// </summary>
/// <param name="i"></param>
/// <param name="s0"></param>
void matchbracket(PINT i, CString s0)
{
    int j = 1;
    int len = s0.GetLength();
	int x=*i;
    while (true)
    {
        x++;
        if (*i >= len) throw new CParserException(L"û���ҵ���Ӧ�ġ�)��");
        if (s0[x] == '(') j++;
        if (s0[x] == ')') j--;
        if (j == 0) break;
    }
	*i=x;
}

void GetCalcString(CString& s0, CPtrList* list, char Key)
{
	int p1 = s0.Find('(');
    CString func;
	CString opps=sopps;
    while (p1 >= 0)
    {
        int p2 = p1;
        matchbracket(&p2, s0); //����)��λ��
        if (p2 != p1)
        {
            CString tmp = s0.Mid(p1 + 1, p2 - p1 - 1); //���Ƴ�����
			GetCalcString(tmp, list, Key);  //�ݹ��г������Ԫ��
            if (p1 > 0)
            {
                int p = GetFirstOpp(p1, s0);  //ȡ�õ�һ��������

                //��ȡ��������
                if (p > 0)
                {
                    p = -1;
                    for (int i = p1 - 1; i >= 0; i--)
                    {
                        if ((opps.Find(s0[i]) >= 0) || (s0[i] == ','))
                        {
                            p = i + 1;
                            break;
                        }
                    }
                    if (p >= 0)
                    {
                        func = s0.Mid(p, p1 - p);
                    }
                }
                else
                {
					if (p1>=0)
					{
						func = s0.Mid(0,p1);

						int x = -1;
						
						for (int i = func.GetLength()-1; i >= 0; i--)
						{
							if (s0[i] == ',')
							{
								x = i;
								break;
							}
						}
						if (x>=0)
						{
							func = s0.Mid(x + 1, func.GetLength() - x-1);
						}
					}
                }

            }
			LPCallVarInfo info=new CCallVarInfo();
			
			CString val;
			val.Format(L"%c%08d", Key, list->GetCount());
			info->Name=val;
			/*if (!func.IsEmpty())  //�溯�����ü�����
			{
				info->Parse ='('+ProcYJX(tmp)+')';
			}
			else*/
			info->Parse = '('+tmp+')';
			info->Func=func;
			list->AddTail(info);
            s0 = s0.Mid(0, p1 - func.GetLength()) + val + s0.Mid(p2 + 1);
        }

        p1 = s0.Find('(');
    }
}

/// <summary>
/// �����������ȼ�
/// </summary>
/// <param name="str"></param>
/// <returns></returns>
void ProcYJX(CString& str)
{
    if (str[0] == '(')
    {
        int p = 0;
        matchbracket(&p, str);
        str.Delete(p, 1);
        str.Delete(0, 1);
    }
	CPtrList list;
    GetCalcString(str, &list, 'L');
    int i = 0;
    int lpos = 0;

    //���� * / ^ ���ȼ�
    while (i < str.GetLength())
    {
        switch (str[i])
        {
            case '*':
            case '/':
            case '^':
				{
					int p = GetFirstOpp(str, i + 1);
					int t = 0;
					if (p < 0)
						t = str.GetLength() - i - 1;
					else
						t = p - i - 1;
					CString val = str.Mid(i + 1, t);
					int x = val.Find(',');
					if (x >= 0)
					{
						val.Delete(x, val.GetLength());
					}
					
					str.Insert(lpos, L"(");   //Ϊ�����ȼ��ļ������ŷ���GetCalcString���·ֽ�
					str.Insert(i + val.GetLength() + 2, L")");
					i = i + 1 + val.GetLength(); 
				}
                break;
            case '+':
            case '-':
            case '>':
            case '<':
            case '@':
            case '#':
            case '~':
            case '=':
            case '|':
            case '&':
            case ',':
                lpos = i + 1;
                break;
        }
        i++;
    }

    GetCalcString(str, &list, 'L');
    lpos = 0;
    i = 0;

    //���� + -  ���ȼ�
    while (i < str.GetLength())
    {
        switch (str[i])
        {
            case '+':
            case '-':
				{
					if (i>0) //�����һ����+-����������
					{
					int p = GetFirstOpp(str, i + 1);
					int t = 0;
					if (p < 0)
						t = str.GetLength() - i - 1;
					else
						t = p - i - 1;
					CString val = str.Mid(i + 1, t);
					int x = val.Find(',');
					if (x >= 0)
					{
						val.Delete(x, val.GetLength());
					}
					
					str.Insert(lpos, L"(");   //Ϊ�����ȼ��ļ������ŷ���GetCalcString���·ֽ�
					str.Insert(i + val.GetLength() + 2, L")");
					i = i + 1 + val.GetLength(); 
					}
				}
                break;
            case '*':
            case '/':
            case '^':
            case '>':
            case '<':
            case '@':
            case '#':
            case '~':
            case '=':
            case '|':
            case '&':
            case ',':
                lpos = i + 1;
                break;
        }
        i++;
    }



    GetCalcString(str, &list, 'L');
    lpos = 0;
    i = 0;

    //���� > < = <> >= <=���ȼ�
    while (i < str.GetLength())
    {
        switch (str[i])
        {
            case '>':
            case '<':
            case '@':
            case '#':
            case '~':
            case '=':
				{
					int p = GetFirstOpp(str, i + 1);
					int t = 0;
					if (p < 0)
						t = str.GetLength() - i - 1;
					else
						t = p - i - 1;
					CString val = str.Mid(i + 1, t);
					int x = val.Find(',');
					if (x >= 0)
					{
						val.Delete(x, val.GetLength());
					}
					
					str.Insert(lpos, L"(");   //Ϊ�����ȼ��ļ������ŷ���GetCalcString���·ֽ�
					str.Insert(i + val.GetLength() + 2, L")");
					i = i + 1 + val.GetLength(); 
				}
                break;
            case '+':
            case '-':
            case '|':
            case '&':
            case '*':
            case '/':
            case '^':
            case ',':
                lpos = i + 1;
                break;
        }
        i++;
    }

    GetCalcString(str, &list, 'L');
    lpos = 0;
    i = 0;

    //���� & | �� ���ȼ�
    while (i < str.GetLength())
    {
        switch (str[i])
        {
            case '|':
            case '&':
				{
					int p = GetFirstOpp(str, i + 1);
					int t = 0;
					if (p < 0)
						t = str.GetLength() - i - 1;
					else
						t = p - i - 1;
					CString val = str.Mid(i + 1, t);
					int x = val.Find(',');
					if (x >= 0)
					{
						val.Delete(x, val.GetLength());
					}
					
					str.Insert(lpos, L"(");   //Ϊ�����ȼ��ļ������ŷ���GetCalcString���·ֽ�
					str.Insert(i + val.GetLength() + 2, L")");
					i = i + 1 + val.GetLength(); 
				}
                break;
            case '+':
            case '-':
            case '>':
            case '<':
            case '@':
            case '#':
            case '~':
            case '=':
            case '*':
            case '/':
            case '^':
            case ',':
                lpos = i + 1;
                break;
        }
        i++;
    }

	POSITION pos;
	for (pos = list.GetTailPosition(); pos != NULL;)
	{
		LPCallVarInfo info=(LPCallVarInfo)list.GetPrev(pos);
		str.Replace(info->Name,info->Parse);
	}
}

XParser::XParser(void)
{
	this->AddFunc(L"abs",XP_ABS,1);
	this->AddFunc(L"sum",XP_SUM,2);
	this->AddFunc(L"ma",XP_MA,2);
	this->AddFunc(L"ema",XP_EMA,2);
	this->AddFunc(L"sma",XP_SMA,3);
	this->AddFunc(L"hhv",XP_HHV,2);
	this->AddFunc(L"llv",XP_LLV,2);
	this->AddFunc(L"ref",XP_REF,2);
	this->AddFunc(L"std",XP_STD,2);
	this->AddFunc(L"max",XP_MAX,2);
	this->AddFunc(L"min",XP_MIN,2);

}

XParser::~XParser(void)
{
	Clear();
	for (POSITION pos=mFuncList.GetStartPosition();pos!=NULL;)
	{
		CString key;
		LPFuncInfo func;
		mFuncList.GetNextAssoc(pos,key,(PVOID&)func);
		delete func;
	}
	mFuncList.RemoveAll();
}

void XParser::AddFunc(CString Name,PVOID Proc,int ParamCount)
{
	LPFuncInfo func=new CFuncInfo();
	func->CallAddr =Proc;
	func->ParamCount =ParamCount;
	cleanup(Name);
	Name.MakeLower();
	func->Name =Name;
	mFuncList.SetAt(Name,func);
}
LPVarInfo XParser::GetVarPtr(CString Name)
{
	if (Name[0]==RunVarChr)
	{
		return (LPVarInfo)mRunParams[Name];
	}
	else
	{
		return (LPVarInfo)mParams[Name];
	}
}
//���ݲ�����������
double XParser::calculate(WCHAR opt, double v1,double v2)
{
	if ((v1>=MAXDWORD) || (v2>=MAXDWORD)) return MAXDWORD;
	////+-*/^><=@#~&|
	switch(opt)
	{
		case L'+':return v1+v2;
		case L'-':return v1-v2;
		case L'*': return v1*v2;
		case L'/':return v1/v2;
		case L'^':return exp(v2*log(v1));
		case L'>':return (double)((int)(v1*1000)>(int)(v2*1000)?1:0);
		case L'<':return (double)((int)(v1*1000)<(int)(v2*1000)?1:0);
		case L'=':return (double)((int)(v1*1000)==(int)(v2*1000)?1:0);
		case L'@':return (double)(v1>=v2?1:0);
		case L'#':return (double)(v1<=v2?1:0);
		case L'~':return (double)(v1!=v2?1:0);
		case L'&':return (double)((int)v1&(int)v2);
		case L'|':return (double)((int)v1|(int)v2);
	}
	return 00;
}
LPVarInfo XParser::evaluate(LPCallVarInfo CallInfo)
{
	CString s0=CallInfo->Parse;
	int p1=s0.Find(L'(');
	int p2=p1;
	//��������Ƿ�ƥ��
	if (p2>=0)
		matchbracket(&p2,s0);
	//�����һ������'��'��ô��ǰ�������ȥ�����������㣻	
	if (p1!=p2)
	{
		s0.Delete(p2,1);
		s0.Delete(p1,1);
	}

	if (CallInfo->Func.IsEmpty())
	{
		int p=GetFirstOpp(s0.GetLength(),s0,0);
		//��鹫ʽ
		if (p>0)
		{
			CString Val1=s0.Left(p);
			CString Val2=s0.Mid(p+1);
			if (Val1.IsEmpty() || (Val2.IsEmpty()))
			{
				throw new CParserException(L"����һ�������Ĺ�ʽ");
			}
			TRACE (L"Name:%s Val1:%s Val2:%s\n",CallInfo->Name,Val1,Val2);
			int x=GetFirstOpp(Val1,0);
			if (x>=0)
			{
				if ((Val1[x]!='+') && (Val1[x]!='-') && (Val1[x]!='!'))
				{
					throw new CParserException(L"����һ�������Ĺ�ʽ");
				}
			}
			x=GetFirstOpp(Val2,0);
			if (x>=0)
			{
				if ((Val2[x]!='+') && (Val2[x]!='-') && (Val2[x]!='!'))
				{
					throw new  CParserException(L"����һ�������Ĺ�ʽ");
				}
			}
			LPVarInfo pv1=NULL;
			LPVarInfo pv2=NULL;

			//����������
			bool IsConst=true;
			double d1=0;
			
			//���ȥ��������ֵ���Ǳ���
			if (!SToDouble(Val1,&d1))
			{	
					 //ת��ʧ�ܣ����ʱ���ڲ�������
					LPVarInfo info=GetVarPtr(Val1);
					if (info==NULL)
					{
						CString err;
						err.Format(L"(%s) %s ����ȷ�ı���",mVarName, Val1);
						throw new CParserException(err.GetString());
					}
					else
					{
						pv1=info ;
						
						IsConst =IsConst & (info->VarType ==Single);
						if (info->VarType ==Single)
						{
							d1=GetVar(info,0);
						}
					}
			}
			else
			{
				IsConst=IsConst & true;
			}
			double d2=0;
			if (!SToDouble(Val2,&d2))
			{	
					 //ת��ʧ�ܣ����ʱ���ڲ�������
					LPVarInfo info=GetVarPtr(Val2);
					if (info==NULL)
					{
						CString err;
						err.Format(L"(%s) %s ����ȷ�ı���",mVarName, Val2);
						throw new CParserException(err.GetString());
					}
					else
					{
						pv2=info ;
						IsConst =IsConst & (info->VarType ==Single);
						if (info->VarType ==Single)
						{
							d2=GetVar(info,0);
						}
					}
			}
			else
			{
				IsConst=IsConst & true;
			}
			
			if (IsConst)
			{
				//�����������ǳ������̽������ý��
				double val= calculate(s0[p],d1,d2);
				//���õ����н������
				LPVarInfo v= new CVarInfo();
				v->Value =val;
				v->VarType =Single;
				v->IsSys =false;
				v->LinkFlag =false;
				v->CallPtr =NULL;
				v->Values =NULL;
				return v;
			}
			else
			{
				//������һ��Ԫ����Ϊ�������ͣ���Ϊ�ù��������е����ݽ�������ָ��
				LPVarInfo v=new CVarInfo();
				v->Values =new CValues();
				v->Name =CallInfo->Name;
				v->VarType =RunVar;
				v->Func =NULL;
				v->CallPtr =NULL;
				v->Opt =s0[p];
				v->LinkFlag =false;
				
				if (pv1==NULL)
				{
					pv1=new CVarInfo();
					pv1->LinkFlag =false;
					pv1->VarType =Temp;
					pv1->Value =d1;
					pv1->RunVar1 =NULL;
					pv1->RunVar2 =NULL;
					pv1->Values =NULL;
					pv1->Link =NULL;

				}
				if (pv2==NULL)
				{
					pv2=new CVarInfo();
					pv2->VarType =Temp;
					pv2->LinkFlag =false;
					pv2->Link =NULL;
					pv2->Value =d2;
					pv2->Values =NULL;

				}
				v->RunVar1 =pv1;
				v->RunVar2 =pv2;
				return v;
			}
		}
		else
		{
			//û���ҵ������������м򵥸�ֵ
			int x=GetFirstOpp(s0,0);
			if (x>=0)
			{
				if ((s0[x]!='+') && (s0[x]!='-') && (s0[x]!='!'))
				{
					throw new CParserException(L"����һ�������Ĺ�ʽ");
				}
			}
			double d1=0;
			//���ȥ��������ֵ���Ǳ���
			if (!SToDouble(s0,&d1))
			{				
				LPVarInfo info=GetVarPtr(s0);
				if (info==NULL)
				{
					CString err;
					err.Format(L"(%s) %s ����ȷ�ı���",mVarName,s0);
					throw new CParserException(err);
				}
				else
				{
					//���õ����н������
					LPVarInfo v= new CVarInfo();
					v->LinkFlag =true;
					v->VarType =info->VarType;
					v->Link =info;
					v->IsSys =info->IsSys;
					return v;
				}
			}
			else
			{
				//���õ����н������
				LPVarInfo v= new CVarInfo();
				v->Name =CallInfo->Name;
				v->Value =d1;
				v->IsSys=false;
				v->VarType =Single;
				v->LinkFlag =false;
				v->Link =NULL;
				return v;
			}

		}

	}
	else
	{
		CStringList list;
		Split(L",",s0,&list);
		LPFuncInfo func=(LPFuncInfo)mFuncList[CallInfo->Func];
		if (func==NULL)
		{
			CString err;
			err.Format(L"%s δ֪�ĺ���",CallInfo->Func);
			throw new CParserException(err);
		}
		if (func->ParamCount !=list.GetCount())
		{
			CString err;
			err.Format(L"%s ���ò�������ȷ����鿴����˵��",CallInfo->Func);
			throw new CParserException(err);
		}
		LPVarInfo v=new CVarInfo();
		v->Name =CallInfo->Name;
		v->VarType =RunVar;
		v->Func =func;
		
		v->LinkFlag =false;
		v->CallPtr =new CPtrList();
		v->Values =new CValues;
		
		//���뵽�����б���
		bool IsConst=true;
		for (POSITION pos=list.GetHeadPosition();pos!=NULL;)
		{
			CString s0=list.GetNext(pos);
			int x=GetFirstOpp(s0,0);
			if (x>=0)
			{
				if ((s0[x]!='+') && (s0[x]!='-') && (s0[x]!='!'))
				{
					throw new CParserException(L"����һ�������Ĺ�ʽ");
				}
			}


			double d1;
			if (!SToDouble(s0,&d1))
			{
				LPVarInfo info=GetVarPtr(s0);
				if (info==NULL)
				{
					CString err;
					//err.Format(L"%s ����ȷ�ı���",Val1);
					throw new CParserException(s0+L" ����ȷ�ı���");
				}
				else
				{
					LPVarInfo ptr= new CVarInfo();
					ptr->VarType =info->VarType;
					ptr->IsSys =info->IsSys;
					ptr->Link =info;
					ptr->LinkFlag =true;
					v->CallPtr->AddTail(ptr); 
					IsConst =IsConst & (ptr->VarType ==Single);
				}
			}else
			{
				LPVarInfo ptr= new CVarInfo();
				ptr->Name =CallInfo->Name;
				ptr->Value =d1;
				ptr->IsSys =false;
				ptr->LinkFlag =false;
				ptr->VarType =Single;
				v->CallPtr->AddTail(ptr); 
				IsConst =IsConst & true;
			}
		}
		if (IsConst)
		{
			CallFunc(0,v);
			v->Value =(*v->Values)[0];
			
			v->VarType =Single;
			v->Func =NULL;
			delete v->Values;
			v->Values =NULL;
			delete v->CallPtr;
		}
		return v;
	}
	
}
void XParser::CallFunc(int Postion,LPVarInfo Ptr)
{
	LPVarInfo v1=Ptr;
	POSITION pos=Ptr->CallPtr->GetHeadPosition();
	LPVarInfo p1=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
	LPVarInfo p2;
	LPVarInfo p3;
	LPVarInfo p4;
	Param1Call call1;
	Param2Call call2;
	Param3Call call3;
	Param4Call call4;
	switch (Ptr->Func->ParamCount)
	{
		case 1:
			call1=(Param1Call)Ptr->Func->CallAddr;
			call1(Postion,p1,Ptr);
			break;
		case 2:
			call2=(Param2Call)Ptr->Func->CallAddr;
			p2=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			call2(Postion,p1,p2,Ptr);
			break;
		case 3:
			call3=(Param3Call)Ptr->Func->CallAddr;
			p2=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			p3=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			call3(Postion,p1,p2,p3,Ptr);
			break;
		case 4:
			call4=(Param4Call)Ptr->Func->CallAddr;
			p2=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			p3=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			p4=(LPVarInfo)Ptr->CallPtr->GetNext(pos);
			call4(Postion,p1,p2,p3,p4,Ptr);
			break;
	}

	/*_asm 
	{
		push v1
	}
	
	for (POSITION pos=Ptr->CallPtr->GetTailPosition();pos!=NULL;)
	{
		LPVarInfo v=(LPVarInfo)Ptr->CallPtr->GetPrev(pos);
		__asm
		{
			push v;
		}
	}
	PVOID proc=Ptr->Func->CallAddr;
	__asm
	{
		push Postion;
		call proc;
	}*/
}
void ClearRunValue(LPVarInfo Value)
{
	if (Value->LinkFlag)
	{
		ClearRunValue((LPVarInfo)Value->Link );
	}else
		if (Value->Values !=NULL)
			Value->Values->Clear();
}
void XParser::Reset()
{

	for (POSITION pos=mRunParams.GetStartPosition();pos!=NULL;)
	{
		CString Key;
		LPVarInfo info;
		mRunParams.GetNextAssoc(pos,Key,(void*&)info);
		if (info!=NULL)
		{
			if (info->VarType ==RunVar)
			{
				ClearRunValue(info);
				
				//if (info->VarType 
				//info->Values->Clear(); //�������н����
			}
			//ClearRunValue(info);
		}
	}
	mCalcPostion=0;
}
void XParser::Clear()
{
	//�������б���
	while (mParams.GetCount()>0)
	{
		POSITION pos=mParams.GetStartPosition();
		CString Key;
		LPVarInfo info;
		mParams.GetNextAssoc(pos,Key,(void*&)info);
		if (info!=NULL)
		{
			if ((info->VarType ==Array) && (!info->IsSys))
			{
				info->Values->Clear();
				delete info->Values;
			}
		}
		if (!info->IsSys)
			delete info;
		mParams.RemoveKey(Key);
	}
	mParams.RemoveAll();
	//�������б���

	for (POSITION pos=mRunParams.GetStartPosition();pos!=NULL;)
	{
		CString Key;
		LPVarInfo info;
		mRunParams.GetNextAssoc(pos,Key,(void*&)info);
		if (info!=NULL)
		{
			if ((info->VarType ==RunVar) && (!info->LinkFlag))
			{
				if (info->Func ==NULL)
				{
					LPVarInfo pv1=(LPVarInfo)info->RunVar1;
					LPVarInfo pv2=(LPVarInfo)info->RunVar2;
					if (pv1->VarType==Temp)
					{
						delete pv1;
					}
					if (pv2->VarType==Temp)
					{
						delete pv2;
					}
				}
				else
				{
					for (POSITION pos=info->CallPtr->GetHeadPosition();pos!=NULL;)
					{
						LPVarInfo  pv=(LPVarInfo)info->CallPtr->GetNext(pos);
						delete pv;
					}
					info->CallPtr->RemoveAll();
					delete info->CallPtr;
				}
				info->Values->Clear();
				delete info->Values ;
			}
			delete info;
		}
	}
	mRunParams.RemoveAll();
	
	mCalcPostion=0;  //����ǰ����λ����0;
	
}
//��̬���Ͳ���
CValues* XParser::AddParam(CString Name)
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
	v->Link =NULL;
	mParams.SetAt(Name,v);
	return v->Values;
}
void XParser::AddParam(CString Name,LPVarInfo Value)
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
	Value->IsSys =true;
	mParams.SetAt(Name,Value);
}
void XParser::AddParam(CString Name,double Value)
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
	v->LinkFlag =false;
	mParams.SetAt(Name,v);
}
//���㲢���أ�����ǵ�������Ļ��򷵻�PDOUBLE���ͣ�����Ƕ���򷵻���PDOUBLE��ɵ�CValues��ָ��
PVOID XParser::Calc(bool& IsSingle)
{
	Reset();
	LPVarInfo pv=(LPVarInfo)mRunParams[mRetValName];
	pv=CalcRunVal(pv);
	if (pv->VarType ==Single)
	{
		IsSingle=true;
		mCalcPostion=0;
		return &pv->Value;
	}
	else
	{
		IsSingle=false;
		mCalcPostion=pv->Values->GetCount()-1;
		return pv->Values;
	}
}

LPVarInfo XParser::CalcRunVal(LPVarInfo var)
{
	if (var->LinkFlag)
	{
		return CalcRunVal((LPVarInfo) var->Link);
	}else
		if (var->VarType ==RunVar)
		{
			int count=0;
			if (var->Func ==NULL)
			{
				LPVarInfo pv1=(LPVarInfo)var->RunVar1;
				LPVarInfo pv2=(LPVarInfo)var->RunVar2; //��ȡ2��ʽ�Ĳ���
				if ((pv1->VarType ==RunVar) || (pv2->LinkFlag)) 
				{
					pv1=CalcRunVal(pv1);
				}
				if ((pv2->VarType ==RunVar) || (pv2->LinkFlag))
				{
					pv2=CalcRunVal(pv2);
				}
				int c=GetVarCount(pv1);
				count=c>count?c:count;
				c=GetVarCount(pv2);
				count=c>count?c:count;
				if (count>0)
				{
					for (int i=mCalcPostion;i<count;i++)  //���ϴμ���λ��������
					{
						double v=calculate(var->Opt,GetVar(pv1,i),GetVar(pv2,i));
						SetVar(var,v,i);
					}
				}
				else
				{
					var->VarType =Single;
					var->Value =calculate(var->Opt,GetVar(pv1,0),GetVar(pv2,0));
				}
			}
			else
			{
				int c=0;
				for (POSITION pos=var->CallPtr->GetHeadPosition();pos!=NULL;)
				{
					LPVarInfo v=(LPVarInfo)var->CallPtr->GetNext(pos);
					CalcRunVal(v);
					int c=GetVarCount(v);
					count=c>count?c:count;
				}

				if (mCalcPostion==0)
				{
					for (int i=mCalcPostion;i<count;i++)
					{
						SetVar(var,MAXDWORD,i);
					}
				}
				
				for (int i=mCalcPostion;i<count;i++)
				{
					CallFunc(i,var);
				}
			}
			return var;
		}
		else
		if (var->VarType ==Single)
		{
			return var;
		}
		return var;
		
}
LPVarInfo GetLink(LPVarInfo V)
{
	if (V->LinkFlag)
	{
		return GetLink((LPVarInfo) V->Link);
	}else
	{
		return V;
	}
}
bool XParser::SetString(CString string)
{
	int p=string.Find(L";");
	mScrept=string;
	if (p>0)
	{
		string=string.Left(p);
		//throw new CParserException(L"û���ҵ�';'�Ž�β");
	}
	string.MakeLower();
	mCalcPostion=0;

	string.Replace(L" and ", L"&");
	string.Replace(L" or ", L"|");
    string.Replace(L">=", L"@");
    string.Replace(L"<=", L"#");
	string.Replace(L"<>", L"~");
    cleanup(string);
	if (string.Find(RunVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}

	if (string.Find(TempVarChr)>=0)
	{
		throw new CParserException(L"�������ư��������ַ�");
	}
	
	p=string.Find(L":");
	mIsHide=false;
	if (p>=0)
	{
		mVarName =string.Left(p);
		if (string.Find(L":=",p)>=0)
		{
			mIsHide =true;
			p++;
		}
		string.Delete(0,p+1);
	}
	mOutputType =0; //��׼����
	CString f=L",dashed";
	p=string.Find(f);
	if (p>=0)
	{
		mOutputType =1;//����
		string.Delete(p,f.GetLength());
	}

	f=L",volstick";
	p=string.Find(f);
	if (p>=0)
	{
		mOutputType =2;//�ɽ�����״ͼ
		string.Delete(p,f.GetLength());
	}

	f=L",colorstick";
	p=string.Find(f);
	if (p>=0)
	{
		mOutputType =3;//������ֱ��ͼ
		string.Delete(p,f.GetLength());
	}
	
	f=L",linestick";
	p=string.Find(f);
	if (p>=0)
	{
		mOutputType =4;//��ɫ��ֱ��ͼ
		string.Delete(p,f.GetLength());
	}


	f=L",pointdot";
	p=string.Find(f);
	if (p>=0)
	{
		mOutputType =5;//���
		string.Delete(p,f.GetLength());
	}
	mColor=L"";
	f=L",color(";
	p=string.Find(f);
	if (p>=0)
	{
		int p1=string.Find(L"(",p);
		int p2=p1;
		matchbracket(&p2,string);
		CString cs;
		if (p1!=p2)
		{
			mColor= string.Mid(p1+1,p2-p1-1);
			/*CStringList cl;
			COLORREF color;
			Split(L",",cs,&cl);
			if (cl.GetCount()!=3)
			{
				if (cl.GetCount()!=1)
				{
					throw new CParserException(L"�������ɫֵ"); 
				}else
				{
					mColor =color;
					if (!SToColor(cs,&color))
					{
						throw new CParserException(L"�������ɫֵ"); 
					}
				}
			}
			else
			{
				int r;
				int g;
				int b;

				if (!SToInt(cl.GetAt(cl.FindIndex(0)),&r))
				{
					throw new CParserException(L"�������ɫֵ"); 
				}
				if (!SToInt(cl.GetAt(cl.FindIndex(1)),&g))
				{
					throw new  CParserException(L"�������ɫֵ"); 
				}
				if (!SToInt(cl.GetAt(cl.FindIndex(2)),&b))
				{
					throw new  CParserException(L"�������ɫֵ"); 
				}
				color=RGB((byte)r,(byte)g,(byte)b);
			}

			mColor =color;*/

		}
		string.Delete(p,p2+1);
	}
	
    CPtrList list;

	GetCalcString(string, &list, RunVarChr);
	ProcYJX(string);

	POSITION pos;
	for (pos = list.GetTailPosition(); pos != NULL;)
	{
		LPCallVarInfo info=(LPCallVarInfo)list.GetPrev(pos);
		ProcYJX(info->Parse);
		string.Replace(info->Name,info->Func+'('+info->Parse+')');
	}

	while (list.GetCount()>0)
	{
		POSITION p=list.FindIndex(0);
		LPCallVarInfo info=(LPCallVarInfo)list.GetAt(p);
		delete info;
		list.RemoveAt(p);
	}
	GetCalcString(string,&list,RunVarChr);
#ifdef _DEBUG
	for (pos = list.GetHeadPosition(); pos != NULL;)
	{
		LPCallVarInfo info=(LPCallVarInfo)list.GetNext(pos);
		CString t;	
		t.Format(L"%s (%s) %s\n",info->Name,info->Func,info->Parse);
		TRACE(L"%s\n",t);
	}
#endif
	if (list.GetCount()==0)
	{
		LPCallVarInfo info=new CCallVarInfo();
		info->Parse =L"("+string+L")";
		CString val;
		val.Format(L"%c%08d", RunVarChr, 0);
		info->Name =val;
		list.AddHead(info);
	}
	
	for (pos=list.GetHeadPosition();pos!=NULL;)
	{
		LPCallVarInfo info=(LPCallVarInfo)list.GetNext(pos);
		///�����ֽ�Ͷ��ʽ�����ƴ���
		LPVarInfo var= evaluate(info);
		mRunParams.SetAt(info->Name,var);
	}
	LPCallVarInfo info=(LPCallVarInfo)list.GetAt(list.GetTailPosition());
	mRetValName=info->Name;
	LPVarInfo v=(LPVarInfo) mRunParams[mRetValName];
	mLastVar =GetLink(v);
	return (v->VarType ==Single);
}