#include "Player.h"



Player::Player()
	: HP(0)
	, MaxHP(0)
{
	X = 120;
	Y = 20;
	Symbol = '%';
	FaceDirection = Right;
}


Player::~Player()
{
}


void Player::TakeDamage(int damage)
{
	HP -= damage;
	if (HP < 0) {
		HP = 0;
	}
}
