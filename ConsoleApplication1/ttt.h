#pragma once
class ttt
{

private:
	double x;
	double y;

public:
	int add(int a, int b);
	//���ι��캯��
	ttt(double x, double y);

	//�޲ι��캯��
	ttt();
	//�� ��������
	virtual ~ttt();

	// ���麯��
//	virtual double getVolume() = 0;//C++ �ӿ���ʹ�ó�������ʵ�ֵģ������������ݳ��󻥲����������ݳ�����һ����ʵ��ϸ������ص����ݷ��뿪�ĸ��
	//�������������һ������������Ϊ���麯�������������ǳ����ࡣ���麯����ͨ����������ʹ�� "= 0" ��ָ���ģ����ܱ�ʵ����

};

