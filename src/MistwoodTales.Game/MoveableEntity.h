#pragma once
#include "Map.h"
#include "Entity.h"
class MoveableEntity: public Entity
{
public:
	MoveableEntity();
	~MoveableEntity();
	virtual int Move(Direction direction);
	Direction FaceDirection;
};

