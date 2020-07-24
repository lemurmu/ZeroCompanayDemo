#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#ifdef LD_FIXFQ_API
#else
#define LD_FIXFQ_API extern "C" _declspec(dllimport)

//´´½¨¾ä±ú
LD_FIXFQ_API int _stdcall Add(int a, int b);

#endif