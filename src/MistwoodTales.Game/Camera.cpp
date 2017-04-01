#include "Camera.h"
#include <curses.h>
#include "World.h"
#include "Player.h"
#include "panel.h"

#define EMPTY_SYMBOL ' '


// ctor
Camera::Camera()
{
}



PANEL *mkpanel(int rows, int cols, int tly, int tlx)
{
	WINDOW *win = newwin(rows, cols, tly, tlx);
	PANEL *pan = (PANEL *)0;

	if (win)
	{
		pan = new_panel(win);

		if (!pan)
			delwin(win);
	}

	return pan;
}

void Camera::Init(int width, int height, WINDOW * window) {
	curs_set(0);
	Width = width;
	Height = height;
	auto camPanel = mkpanel(LINES, COLS, 0, 0);
	Window = camPanel -> win;
	_screenCenterX = Width / 2;
	_screenCenterY = Height / 2;
	int ppwidth = 40;
	int ppheight = 4;
	PlayerPanel = mkpanel(ppheight, ppwidth, 0, 0);
	box(PlayerPanel->win, 0, 0);
	auto player = World::Instance().FirstPerson;
	mvwaddstr(PlayerPanel->win, 0, 3, player->Name.c_str());
	show_panel(PlayerPanel);
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
	RenderPlayerPanel();
	


	update_panels();
	doupdate();
}

// colors: 0-255
attr_t Camera::GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb) {
	const float coeff = 31.0 / 255.0;
	attr_t output_color = A_RGB(fr * coeff, fg * coeff, fb * coeff, br * coeff, bg * coeff, bb * coeff);
	return output_color;
}

void Camera::SetColor(attr_t color) {
	wattrset(Window, color);
}


void Camera::RenderNPCs() {

	attr_t color = GetOutputRgbColor(95, 186, 125, 0, 0, 0);
	color |= A_UNDERLINE | A_BOLD;
	SetColor(color);
	for (auto &npc : World::Instance().NPCs) {
		mvwaddch(Window, npc.Y - _camY1, npc.X - _camX1, npc.Symbol);
	}
}

void Camera::RenderPlayerPanel()
{
	auto player = World::Instance().FirstPerson;
	mvwprintw(PlayerPanel->win, 1, 1, "HP: %d/%d", player->HP, player->MaxHP);
}

void Camera::RenderPlayer() {
	attr_t color = GetOutputRgbColor(10, 109, 255, 230, 230, 230);
	SetColor(color);
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
	mvwaddch(Window, player->Y - _camY1, player->X - _camX1, symbol);
}



void Camera::RenderLandscape() {

	Map* map = World::Instance().CurrentMap;
	for (int i = 0; i < Height; i++) {
		for (int j = 0; j < Width; j++)
		{
			// optimization for player panel
			// do not render map there
			if ((j >= PlayerPanel->wstartx)
				&& (j < PlayerPanel->wendx) 
				&& (i >= PlayerPanel->wstarty)
				&& (i < PlayerPanel->wendy)) {
				continue;;
			}
			MapItem item = map->GetItem(j + _camX1, i + _camY1);
			SetColor(item.Color);
			mvwaddch(Window, i, j, item.Symbol);
		}
	}
}

Camera::~Camera()
{
}
