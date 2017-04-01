#pragma once
#include "Player.h"
#include "NPC.h"
#include "vector"
#include "Sighting.h"

#pragma once
class World
{
public:
	static World& Instance()
	{
		// �������� ���������, ���� ��� ������� � ����������������
		static World w;
		return w;
	}
	Map * CurrentMap;
	Player * FirstPerson;
	vector<NPC> NPCs;
	Sighting GetSighting(const Point& point);

private:
	void initNPCs();
	void initPlayer();
	World();
	~World();
	// ���������� ����� ��������� �����������
	World(World const&) = delete;
	World& operator= (World const&) = delete;
};
