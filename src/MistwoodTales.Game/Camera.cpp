#include "Camera.h"
#include <curses.h>
#include "World.h"
#include "Player.h"
#include "panel.h"
#include <thread>
#include "Globals.h"
#include <sstream>


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


#define RENDERDELAY 80
//#define INPUTDELAY 100

void Camera::RenderScreenThreadFunction() {
	while (!Globals::isShuttingDown) {
		Render();
		napms(RENDERDELAY);
	}
}



void Camera::processSymbol(int symbol) {
	if (KEY_MOUSE == symbol) {
		// check what is that at the point
		request_mouse_pos();
		if (-1 == Mouse_status.x || -1 == Mouse_status.y) {
			return;
		}
		auto coords = RelativeToAbsoluteCoords(Point(Mouse_status.x, Mouse_status.y));
		auto sighting = World::Instance().GetSighting(coords);
		AddSighting(sighting);
		// Mouse_status.x, Mouse_status.y etc...
		return;
	}
	Player * player = World::Instance().FirstPerson;
	switch (symbol)
	{
	case KEY_UP:
		player->Move(Direction::Up);
		break;
	case KEY_DOWN:
		player->Move(Direction::Down);
		break;
	case KEY_LEFT:
		player->Move(Direction::Left);
		break;
	case KEY_RIGHT:
		player->Move(Direction::Right);
		break;
	case 'q':
		Globals::isShuttingDown = true;
		break;
	default:
		break;
	}
}

void Camera::InputThreadFunction()
{
	while (!Globals::isShuttingDown) {
		int symbol = getch();
		processSymbol(symbol);
		//napms(INPUTDELAY);
	}
}


Camera::Camera(int width, int height, WINDOW * window): _camX1(0), _camX2(0), _camY1(0), _camY2(0)
{
	Width = width;
	Height = height;
	auto camPanel = mkpanel(LINES, COLS, 0, 0);
	Window = camPanel->win;
	_screenCenterX = Width / 2;
	_screenCenterY = Height / 2;

	// create player panel
	int ppwidth = 40;
	int ppheight = 4;
	PlayerPanel = mkpanel(ppheight, ppwidth, 0, 0);
	box(PlayerPanel->win, 0, 0);
	auto player = World::Instance().FirstPerson;
	mvwaddwstr(PlayerPanel->win, 0, 3, player->Name.c_str());
	show_panel(PlayerPanel);

	// create sightings panel
	int spwidth = 60;
	int spheight = 3;
	SightingsBorderedPanel = mkpanel(spheight, spwidth, LINES - spheight, COLS - spwidth);
	for (int i = 1; i < spheight; i++) {
		mvwaddch(SightingsBorderedPanel->win, i, 0, ACS_VLINE);
	}
	for (int i = 1; i < spwidth; i++) {
		mvwaddch(SightingsBorderedPanel->win, 0, i, ACS_HLINE);
	}
	mvwaddch(SightingsBorderedPanel->win, 0, 0, ACS_ULCORNER);
	//box(SightingsBorderedPanel->win, 0, 0);
	SightingsPanel = mkpanel(spheight - 1, spwidth - 1, LINES - spheight + 1, COLS - spwidth + 1);
	_screenThread = new thread(&Camera::RenderScreenThreadFunction, this);
	_inThread = new thread(&Camera::InputThreadFunction, this);
}

Point Camera::AbsoluteToRelativeCoords(Point absoluteCoords)
{
	Point res;
	res.X = absoluteCoords.X - _camX1;
	res.Y = absoluteCoords.Y - _camY1;
	return res;
}

Point Camera::RelativeToAbsoluteCoords(Point relativeCoords)
{
	Point res;
	res.X = relativeCoords.X + _camX1;
	res.Y = relativeCoords.Y + _camY1;
	return res;
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
	
	RenderContent();

	update_panels();
	doupdate();
}

// colors: 0-255
attr_t Camera::GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb) {
	const float coeff = 31.0 / 255.0;
	attr_t output_color = A_RGB(fr * coeff, fg * coeff, fb * coeff, br * coeff, bg * coeff, bb * coeff);
	return output_color;
}

void Camera::SetColor(attr_t color) const
{
	wattrset(Window, color);
}

int maxSightingsCount = 1;

void Camera::AddSighting(Sighting sighting)
{
	if (Sightings.size() == maxSightingsCount) {
		Sightings.pop_front();
	}
	Sightings.push_back(sighting);
}


void Camera::RenderNPCs() {

	attr_t color = GetOutputRgbColor(0, 255, 0, 0, 0, 0);
	color |= A_UNDERLINE | A_BOLD;
	SetColor(color);
	for (auto &npc : World::Instance().NPCs) {
		mvwaddch(Window, npc.Y - _camY1, npc.X - _camX1, npc.Symbol);
	}
}

void Camera::RenderContent()
{
	RenderLandscape();
	RenderNPCs();
	RenderPlayer();
	RenderPlayerPanel();
	RenderSightingsPanel();
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

void Camera::RenderSightingsPanel()
{
	auto win = SightingsPanel->win;
	int sightingsCount = Sightings.size() < maxSightingsCount 
		? Sightings.size() 
		: maxSightingsCount;
	wclear(win);
	for (auto &element : Sightings) {
		SetColor(GetOutputRgbColor(255, 0, 120, 0, 0, 0));
		waddwstr(win, element.Description.c_str());
		waddch(win,'\n');
	}
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
			auto coords = RelativeToAbsoluteCoords(Point(j,i));
			MapItem item = map->GetItem(coords.X, coords.Y);
			SetColor(item.Color);
			mvwaddch(Window, i, j, item.Symbol);
		}
	}
}


Camera::~Camera()
{
	_screenThread->join();
	delete _screenThread;
	_inThread->join();
	delete _inThread;
	delete PlayerPanel;
	delete SightingsPanel;
	delete SightingsBorderedPanel;
}
