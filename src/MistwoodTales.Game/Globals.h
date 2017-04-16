#pragma once
#include "Camera.h"

class Globals
{
public:
	static Camera * Cam;
	static volatile bool isShuttingDown;
private:
};