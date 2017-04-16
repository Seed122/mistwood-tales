//#include "stdafx.h"
#include "Map.h"
#include <string>
#include <fstream>
#include <sstream>
#include <curses.h>
#include <codecvt>
#include "Camera.h"
using namespace std;


Map::Map(string file)
{
	Load(file);
}


Map::~Map()
{
	delete Data;
}

#pragma optimize( "g", on )   

std::wstringstream readFile(const char* filename)
{
	std::wifstream wif(filename);
	
    wif.imbue(std::locale(wif.getloc(), new std::codecvt_utf8<wchar_t, 0x10ffff, std::consume_header>));
	std::wstringstream wss;
	wss << wif.rdbuf();
	return wss;
}

int Map::Load(string file) {
	wifstream infile(file.c_str(), std::ios::binary);
	if (!infile.is_open())
	{
		//cout << "There was an error opening the file.\n";
		return -1;
	}
// apply BOM-sensitive UTF-16 facet
    infile.imbue(std::locale(infile.getloc(), new std::codecvt_utf8<wchar_t, 0x10ffff, std::consume_header>));

	 std::wstring s;
	 infile >> s;

	 wstringstream inputStream = readFile(file.c_str());
	 wstring input;

	std::getline(inputStream, input, L',');
	if (65279 == input[0]) {
		// byte-order mark (BOM) symbol
		input = input.substr(1, input.length() - 1);
	}
	Width = stoi(input);

	std::getline(inputStream, input, L'\n');
	Height = stoi(input);
	long arraySize = Width*Height;
	Data = new MapItem[arraySize];

	for (long i = 0; i < arraySize; i++) {
		std::getline(inputStream, input, L','); // wtf?
		std::getline(inputStream, input, L',');
		int fr = stoi(input);
		std::getline(inputStream, input, L',');
		int fg = stoi(input);
		std::getline(inputStream, input, L',');
		int fb = stoi(input);
		std::getline(inputStream, input, L','); // f-alpha
		std::getline(inputStream, input, L',');
		int br = stoi(input);
		std::getline(inputStream, input, L',');
		int bg = stoi(input);
		std::getline(inputStream, input, L',');
		int bb = stoi(input);
		std::getline(inputStream, input, L','); // b-alpha
		Data[i].Color = Camera::GetOutputRgbColor(fr, fg, fb, br, bg, bb);
		std::getline(inputStream, input, L',');
		Data[i].Symbol = input[0];
		std::getline(inputStream, input, L'\n');
		Data[i].CanWalk = stoi(input);
	}
}

bool Map::CheckIfCoordsAreAvailable(int x, int y)
{
	if (x < 0
		|| y < 0
		|| x >= Width
		|| y >= Height)
		return false;
	long index = Width*y + x;
	return Data[index].CanWalk;
}


MapItem Map::GetItem(int x, int y) 
{
	if (x < 0
		|| y < 0
		|| x >= Width
		|| y >= Height)
		return MapItem();
	long index = Width*y + x;
	return Data[index];
}

MapItem Map::GetItem(Point point)
{
	return GetItem(point.X, point.Y);
}
