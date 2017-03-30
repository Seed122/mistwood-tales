#pragma once
#include <string>
#include "curses.h"
using namespace std;



enum Direction { Left, Right, Up, Down };

struct MapItem {
	//int X;
	//int Y;
	bool CanWalk;
	wchar_t Symbol;
	attr_t Color;
};

class Map
{
public:
	Map(string);
	~Map();
	int Width;
	int Height;
	int Load(string);
	MapItem* Data;
private:
	//splitString();
public:
	bool CheckIfCoordsAreAvailable(int x, int y);
	MapItem GetItem(int x, int y);
};

