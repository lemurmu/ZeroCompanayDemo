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

};

