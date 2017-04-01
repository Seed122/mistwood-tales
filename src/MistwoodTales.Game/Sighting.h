#pragma once
#include <string>

class Sighting
{
public:
	Sighting(std::wstring description);
	Sighting();
	~Sighting();
	std::wstring Description;
};

