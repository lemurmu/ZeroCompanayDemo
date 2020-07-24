#include "ttt.h"
using namespace std;


int ttt::add(int a, int b) {
	return a + b;
}

//带参构造函数
ttt::ttt(double x, double y) {
	this->x = x;
	this->y = y;
}
//无参构造函数
ttt::ttt() {
	x = 45;
	y = 90;
}
// 析构函数
ttt:: ~ttt() {
	x = 0;
	y = 0;
}



