// pch.cpp: 与预编译标头对应的源文件

#include "pch.h"
#include <iostream>
using namespace std;
// 当使用预编译的头时，需要使用此源文件，编译才能成功。

#define XML_NODE_KERNEL				"Kernel"
#define XML_NODE_DEVICES			"Devices"
#define XML_NODE_DEVICE				"Device"
#define XML_NODE_DRIVERS			"Drivers"
#define XML_NODE_DRIVER				"Driver"

#define XML_NODE_ATTRIBUTE_ID		"ID"
#define XML_NODE_ATTRIBUTE_NAME		"Name"
#define XML_NODE_ATTRIBUTE_STATIONTYPE "StationType"
#define XML_NODE_ATTRIBUTE_ASSEMBLY	"Assembly"
#define XML_NODE_ATTRIBUTE_ISLOAD	"IsLoad"
#define XML_NODE_ATTRIBUTE_MAIN		"Main"
#define XML_NODE_ATTRIBUTE_PORT		"Port"
#define INT_gg  3923.36366
const string STR_NAME = " hdhahdhahfhhds888f5a";

void sayHello()
{
	cout << XML_NODE_KERNEL << endl;
	cout << XML_NODE_ATTRIBUTE_STATIONTYPE << endl;
	cout << XML_NODE_ATTRIBUTE_PORT << endl;
	cout << XML_NODE_ATTRIBUTE_ISLOAD << endl;
	cout << XML_NODE_ATTRIBUTE_NAME << endl;
	cout << XML_NODE_DRIVERS << endl;
	cout << XML_NODE_ATTRIBUTE_MAIN << endl;
	cout << INT_gg << endl;
	cout << STR_NAME << endl;
	cout << "我是静态链接库!" << endl;
}
