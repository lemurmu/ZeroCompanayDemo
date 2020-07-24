// ConsoleApplication1.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//
#define    WIN32_LEAN_AND_MEAN

#include <iostream>
#include "ttt.h"
#include"fs.h"
#include"../Dll/pch.h"
#include "ConsoleApplication1.h"
using namespace std;

ttt t;
//extern void GetPath(string& path); //extern全局定义
//extern void load();
extern wchar_t* AnsiToUnicode(const char* szStr);
extern char* UnicodeToAnsi(const wchar_t* szStr);
typedef int(__stdcall* PUBadd)(int a, int b);

void GetPath(string& path)
{
	char szModuleFileName[255];		// 全路径名
	char drive[255];				// 盘符名称，比如说C盘啊，D盘啊
	char dir[255];					// 目录
	char fname[255];				// 进程名字
	char ext[255];					// 后缀，一般为exe或者是dll

	GetModuleFileNameA(NULL, szModuleFileName, 255); //获得当前进程的文件路径
	_splitpath_s(szModuleFileName, drive, dir, fname, ext);  //分割该路径，得到盘符，目录，文件名，后缀名
	path = drive;
	path += dir;
}
void load() {

	string dllpath;
	GetPath(dllpath);
	dllpath += "dlls\\LD.FIRSTDLL.dll";

	//load(dllpath.c_str());
	typedef int(*ptrSub)(int, int);
	/*
	在调用DLL函数之前，要定义函数指针，用来调用函数。
	可以看出，函数指针的类型与DLL中的要一致。
	*/
	string driverDir = dllpath;
	HMODULE hMod = LoadLibrary(driverDir.c_str());
	/*
		调用LoadLibrary函数加载DLL文件。加载成功，hMod指针不为空。
		这里也可以是一个地址加文件名
	*/
	if (hMod != NULL)
	{
		/*
		如果加载成功，则可通过GetProcAddress函数获取DLL中需要调用的函数的地址。
		获取成功，sum指针不为空。
		*/
		PUBadd add = (PUBadd)GetProcAddress(hMod, "Add");
		if (add != NULL)
		{
			std::cout << "10 + 6 = " << add(10, 6) << std::endl;
			/*获取地址成功后，通过sum调用函数功能。*/
		}
		FreeLibrary(hMod);
		/*在完成调用功能后，不在需要DLL支持，则可以通过FreeLibrary函数释放DLL。*/
	}
}




int main()
{

	int result = t.add(2, 39);
	cout << result << endl;
	cout << "Hello World!\n";
	// 声明简单的变量
	int    i;
	double d;

	// 声明引用变量
	int& r = i;
	double& s = d;

	i = 5;
	cout << "Value of i : " << i << endl;
	cout << "Value of i reference : " << r << endl;

	d = 11.7;
	cout << "Value of d : " << d << endl;
	cout << "Value of d reference : " << s << endl;


	/*指针：指针是一个变量，只不过这个变量存储的是一个地址，指向内存的一个存储单元；而引用跟原来
		的变量实质上是同一个东西，只不过是原变量的一个别名而已。如：*/

	int a = 1; int* p = &a;
	int c = 1; int& b = c;
	a = 56;
	c = 155;
	cout << p << endl;
	cout << *p << endl;
	cout << b << endl;
	cout << &b << endl;

	//test_fs();

	//sayHello();

	load();
	while (true)
	{
		Sleep(100);
	}
	return 0;
}



//将单字节char*转化为宽字节wchar_t*  
wchar_t* AnsiToUnicode(const char* szStr)
{
	int nLen = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, szStr, -1, NULL, 0);
	if (nLen == 0)
	{
		return NULL;
	}
	wchar_t* pResult = new wchar_t[nLen];
	MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, szStr, -1, pResult, nLen);
	return pResult;
}

//将宽字节wchar_t*转化为单字节char*  
char* UnicodeToAnsi(const wchar_t* szStr)
{
	int nLen = WideCharToMultiByte(CP_ACP, 0, szStr, -1, NULL, 0, NULL, NULL);
	if (nLen == 0)
	{
		return NULL;
	}
	char* pResult = new char[nLen];
	WideCharToMultiByte(CP_ACP, 0, szStr, -1, pResult, nLen, NULL, NULL);
	return pResult;
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门使用技巧: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
