#pragma once
#include <string>

class Quest
{
public:
	int QuestId;
	std::wstring Name;
	std::wstring WhatToDo;
	
	int RequiresKillingMobs;
};
