#pragma once
#include "Entity.h"

class NPC : public Entity
{
public:
	NPC();
	~NPC();
	std::wstring Description;
};

