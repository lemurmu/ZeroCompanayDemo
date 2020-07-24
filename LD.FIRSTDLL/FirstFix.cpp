#include "pch.h"
#define LD_FIXFQ_API extern "C" _declspec(dllexport)
#include "FirstFix.h"


LD_FIXFQ_API int _stdcall Add(int a, int b)
{
    return a + b;
}
