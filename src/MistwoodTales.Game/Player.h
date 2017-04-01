#pragma once
#include "Entity.h"
#include "MoveableEntity.h"

class Player : 	public MoveableEntity
{
public:
	Player();
	~Player();
	// Life
	int HP;
	int MaxHP;
	void TakeDamage(int damage);
};

