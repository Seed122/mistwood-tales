#include "MoveableEntity.h"
#include "Map.h"
#include "World.h"



MoveableEntity::MoveableEntity()
{
	
}


MoveableEntity::~MoveableEntity()
{
}


int MoveableEntity::Move(Direction direction)
{
	Map* map = World::Instance().CurrentMap;
	int newX, newY;
	switch (direction)
	{
	case Left:
		newX = X - 1;
		newY = Y;
		break;
	case Right:
		newX = X + 1;
		newY = Y;
		break;
	case Up:
		newX = X;
		newY = Y - 1;
		break;
	case Down:
		newX = X;
		newY = Y + 1;
		break;
	default:
		break;
	}
	if (!map->CheckIfCoordsAreAvailable(newX, newY))
		return -1;
	X = newX;
	Y = newY;
	FaceDirection = direction;
}
