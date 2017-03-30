#include "Player.h"
#include "NPC.h"
#include "vector"

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
	
private:
	void initNPCs();
	World();
	~World();
	// ���������� ����� ��������� �����������
	World(World const&) = delete;
	World& operator= (World const&) = delete;
};
