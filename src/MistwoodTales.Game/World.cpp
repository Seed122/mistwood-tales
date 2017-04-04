#include "Player.h"
#include "World.h"
#include "Map.h"
#include "NPC.h"
#include <string>
#include "Point.h"


World::World()
{
	CurrentMap = new Map("map.txt");
	initPlayer();
	initNPCs();
}

void World::initNPCs() {
	NPC npc;
	npc.Name = L"Гарольд";
	npc.Symbol = 'G';
	npc.Description = L"Гарольд, старейшина деревни";
	npc.X = 130;
	npc.Y = 40;

	NPCs.push_back(npc);
}

void World::initPlayer()
{
	FirstPerson = new Player();
	FirstPerson->MaxHP = 88;
	FirstPerson->HP = 70;
	FirstPerson->Name = L"Gatmeat";
}


World::~World()
{
	delete CurrentMap;
	delete FirstPerson;
}

Sighting World::GetSighting(const Point& point)
{


	Sighting res = Sighting();
	
	if (FirstPerson->X == point.X && FirstPerson->Y == point.Y) {
		res.Description = L"Это я, " + FirstPerson->Name;
		return res;
	}
	for (auto &npc : NPCs) {
		if (npc.X == point.X && npc.Y == point.Y) {
			res.Description = npc.Description;
			return res;
		}
	}
	auto item = CurrentMap->GetItem(point);
	switch (item.Symbol) {
	case L'≈':
		res.Description = L"Похоже, это вода";
		break;
	case L'/':
		res.Description = L"Это скала";
		break;
	case L'`':
		res.Description = L"Это песок";
		break;
	default:
		res.Description = L"Не знаю, что это";
		break;
	}
	return res;
}
