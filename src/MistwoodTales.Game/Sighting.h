#pragma once
#include "term.h"
#include <string>

class Sighting
{
public:
	Sighting(attr_t color, std::string description);
	~Sighting();
	attr_t Color;
	std::string Description;
};

