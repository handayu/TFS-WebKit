#pragma once
#include "XParserHeader.h"
typedef void (*Param1Call)(int Postion ,LPVarInfo Param1,LPVarInfo Return);
typedef void (*Param2Call)(int Postion ,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Result);
typedef void (*Param3Call)(int Postion ,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Param3,LPVarInfo Result);
typedef void (*Param4Call)(int Postion ,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Param3,LPVarInfo Param4,LPVarInfo Result);
void  XP_ABS(int Postion, LPVarInfo Param, LPVarInfo Result);
void  XP_SUM(int Postion, LPVarInfo Param ,LPVarInfo Count,LPVarInfo Result);
void  XP_MA(int Postion, LPVarInfo Param ,LPVarInfo Count,LPVarInfo Result);
void  XP_EMA(int Postion,LPVarInfo Param ,LPVarInfo Count,LPVarInfo Result);
void  XP_HHV(int Postion,LPVarInfo Param,LPVarInfo Count,LPVarInfo Result);
void  XP_LLV(int Postion,LPVarInfo Param,LPVarInfo Count,LPVarInfo Result);
void  XP_SMA(int Postion,LPVarInfo Param,LPVarInfo N,LPVarInfo M,LPVarInfo Result);
void  XP_REF(int Postion,LPVarInfo Param ,LPVarInfo Count,LPVarInfo Result);
void  XP_STD(int Postion,LPVarInfo Param,LPVarInfo Count,LPVarInfo Result);
void  XP_MAX(int Postion,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Result);
void  XP_MIN(int Postion,LPVarInfo Param1,LPVarInfo Param2,LPVarInfo Result);
