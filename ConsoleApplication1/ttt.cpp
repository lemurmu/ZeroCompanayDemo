#include "ttt.h"
using namespace std;


int ttt::add(int a, int b) {
	return a + b;
}

//���ι��캯��
ttt::ttt(double x, double y) {
	this->x = x;
	this->y = y;
}
//�޲ι��캯��
ttt::ttt() {
	x = 45;
	y = 90;
}
// ��������
ttt:: ~ttt() {
	x = 0;
	y = 0;
}



