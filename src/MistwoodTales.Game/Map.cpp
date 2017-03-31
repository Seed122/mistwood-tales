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

//
//int Map::Load(string file)
//{
//	ifstream infile;
//	infile.open(file, 1);
//	if (!infile.is_open())
//	{
//		//cout << "There was an error opening the file.\n";
//		return -1;
//	}
//	// todo:dfds
//
//
//	Height = std::count(std::istreambuf_iterator<char>(infile),
//		std::istreambuf_iterator<char>(), '\n');
//	infile.clear();
//	infile.seekg(0, ios::beg);
//	string fileString;
//	infile >> fileString;
//	Width = fileString.length();
//	infile.clear();
//	infile.seekg(0, ios::beg);
//	Walls = new char*[Height];
//	for (int i = 0; i < Height; i++) {
//		Walls[i] = new char[Width];
//		infile.getline(Walls[i], INT32_MAX, '\n');
//	}
//	return 0;
//}

std::wstringstream readFile(const char* filename)
{
	std::wifstream wif(filename);
	wif.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));
	std::wstringstream wss;
	wss << wif.rdbuf();
	return wss;
}

//
//wchar_t const* const digitTables[] =
//{
//	L"0123456789",
//	L"\u0660\u0661\u0662\u0663\u0664\u0665\u0666\u0667\u0668\u0669",
//	// ...
//};
////!     \return
////!         wch as a numeric digit, or -1 if it is not a digit
//int asNumeric(wchar_t wch)
//{
//	int result = -1;
//	for (wchar_t const* const* p = std::begin(digitTables);
//		p != std::end(digitTables) && result == -1;
//		++p) {
//		wchar_t const* q = std::find(*p, *p + 10, wch);
//		if (q != *p + 10) {
//			result = q - *p;
//		}
//		return result;
//	}
//}
//
int Map::Load(string file) {
	wifstream infile;
	//infile.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));
	infile.open(file, 1);
	if (!infile.is_open())
	{
		//cout << "There was an error opening the file.\n";
		return -1;
	}
	// todo:dfds
	wstringstream inputStream = readFile(file.c_str());
	wstring input;
	//inputStream.getline(&input, INT32_MAX, ',');
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
		//infile.getline(Walls[i], INT32_MAX, '\n');
	}
}

//
//int Map::Load(string file) {
//	ifstream infile;
//	//infile.imbue(std::locale(std::locale::empty(), new std::codecvt_utf8<wchar_t>));
//	infile.open(file, 1);
//	if (!infile.is_open())
//	{
//		//cout << "There was an error opening the file.\n";
//		return -1;
//	}
//	// todo:dfds
//	//wstringstream inputStream = readFile(file.c_str());
//	string input;
//	//inputStream.getline(&input, INT32_MAX, ',');
//	std::getline(infile, input, ',');
//	Width = stoi(input);
//
//	std::getline(infile, input, '\n');
//	Height = stoi(input);
//	long arraySize = Width*Height;
//	Data = new MapItem[arraySize];
//
//	for (long i = 0; i < arraySize; i++) {
//		std::getline(infile, input, ','); // wtf?
//		std::getline(infile, input, ',');
//		int fr = stoi(input);
//		std::getline(infile, input, ',');
//		int fg = stoi(input);
//		std::getline(infile, input, ',');
//		int fb = stoi(input);
//		std::getline(infile, input, ','); // f-alpha
//		std::getline(infile, input, ',');
//		int br = stoi(input);
//		std::getline(infile, input, ',');
//		int bg = stoi(input);
//		std::getline(infile, input, ',');
//		int bb = stoi(input);
//		std::getline(infile, input, ','); // b-alpha
//		Data[i].Color = Camera::GetOutputRgbColor(fr, fg, fb, br, bg, bb);
//		std::getline(infile, input, ',');
//		Data[i].Symbol = input[0];
//		std::getline(infile, input, '\n');
//
//		Data[i].CanWalk = stoi(input);
//		//infile.getline(Walls[i], INT32_MAX, '\n');
//	}
//	infile.close();
//}

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


MapItem Map::GetItem(int x, int y) {

	if (x < 0
		|| y < 0
		|| x >= Width
		|| y >= Height)
		throw (-1);
	long index = Width*y + x;
	return Data[index];
}