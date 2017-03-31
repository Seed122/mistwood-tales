#include "Camera.h"
#include <curses.h>
#include "World.h"
#include "Player.h"

#define EMPTY_SYMBOL ' '


// ctor
Camera::Camera()
{
	int _rows = 40;
	int _cols = 100;
	resize_term(_rows, _cols);
	///* Get the largest physical screen dimensions */
	//getmaxyx(Window, _rows, _cols);

	/* Resize so it fits */
	//resize_term(_rows - 1, _cols - 1);

	/* Get the screen dimensions that fit */
	//getmaxyx(Window, _rows, _cols);
	Width = COLS;
	Height = LINES;
	_screenCenterX = Width / 2;
	_screenCenterY = Height / 2;
}

void Camera::Render()
{

	Player* player = World::Instance().FirstPerson;
	Map* map = World::Instance().CurrentMap;
	 _camX1 = (player->X - _screenCenterX);
	if (_camX1 < 0) {
		_camX1 = 0;
	}
	 _camY1 = (player->Y - _screenCenterY);
	if (_camY1 < 0) {
		_camY1 = 0;
	}
	 _camX2 = _camX1 + Width;
	if (_camX2 >= map->Width) {
		_camX2 = map->Width;
		_camX1 = _camX2 - Width;
	}
	 _camY2 = _camY1 + Height;
	if (_camY2 >= map->Height) {
		_camY2 = map->Height;
		_camY1 = _camY2 - Height;
	}

	RenderLandscape();
	RenderNPCs();
	RenderPlayer();

	move(Height - 1, Width - 1);
}

// colors: 0-255
inline attr_t Camera::GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb) {
	const float coeff = 31.0 / 255.0;
	attr_t output_color = A_RGB(fr * coeff, fg * coeff, fb * coeff, br * coeff, bg * coeff, bb * coeff);
	return output_color;
}

void setColor(attr_t color) {
	
	attrset(color);
}


void Camera::RenderNPCs() {

	attr_t color = GetOutputRgbColor(95, 186, 125, 0, 0, 0);
	color |= A_UNDERLINE | A_BOLD;
	setColor(color);
	for (auto &npc : World::Instance().NPCs) {

		mvaddch(npc.Y - _camY1, npc.X - _camX1, npc.Symbol);
	}
}

void Camera::RenderPlayer() {
	//attron(COLOR_PAIR(3));
	attr_t color = GetOutputRgbColor(10, 109, 255, 230, 230, 230);
	setColor(color);
	Player* player = World::Instance().FirstPerson;
	wchar_t symbol;
	switch (player->FaceDirection)
	{
	case Right:
		symbol = L'→';
		break;
	case Left:
		symbol = L'←';
		break;
	case Up:
		symbol = L'↑';
		break;
	case Down:
		symbol = L'↓';
		break;

	default:
		break;
	}
	mvaddch(player->Y - _camY1, player->X - _camX1, symbol);
}



void Camera::RenderLandscape() {

	Player* player = World::Instance().FirstPerson;
	Map* map = World::Instance().CurrentMap;
	//for (int i = 0; i < Height; i++) {
	//	for (int j = 0; j < Width; j++)
	//	{
	//		int mapX = (player->X - _screenCenterX) + j;
	//		int mapY = (player->Y - _screenCenterY) + i;
	//		if (mapX < 0 || mapY < 0 || mapX >= map->Width || mapY >= map->Height) {
	//		}
	//		else {
	//			MapItem item = map->GetItem(mapX, mapY);
	//			setColor(item.Color);
	//			mvaddch(i, j, item.Symbol);
	//		}
	//	}
	//}
	for (int i = 0; i < Height; i++) {
		for (int j = 0; j < Width; j++)
		{
			MapItem item = map->GetItem(j + _camX1, i + _camY1);
			setColor(item.Color);
			mvaddch(i, j, item.Symbol);
		}
	}
}

Camera::~Camera()
{
}
