#pragma once
class ttt
{

private:
	double x;
	double y;

public:
	int add(int a, int b);
	//带参构造函数
	ttt(double x, double y);

	//无参构造函数
	ttt();
	//虚 析构函数
	virtual ~ttt();

	// 纯虚函数
//	virtual double getVolume() = 0;//C++ 接口是使用抽象类来实现的，抽象类与数据抽象互不混淆，数据抽象是一个把实现细节与相关的数据分离开的概念。
	//如果类中至少有一个函数被声明为纯虚函数，则这个类就是抽象类。纯虚函数是通过在声明中使用 "= 0" 来指定的，不能被实例化

};

