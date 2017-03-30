#include "Player.h"
#include "World.h"
#include "Map.h"
#include "NPC.h"
#include <string>


World::World()
{
	CurrentMap = new Map("map.txt");
	FirstPerson = new Player();
	initNPCs();
}

void World::initNPCs() {
	NPC npc;
	npc.Name = "Gheed";
	npc.Symbol = 'G';
	npc.X = 130;
	npc.Y = 40;

	NPCs.push_back(npc);
}


World::~World()
{
	delete CurrentMap;
	delete FirstPerson;
}
