#include <signal.h>
#include <ctype.h>
#include <stdlib.h>
#include <thread>
#include "Map.h"
#include "World.h"
#include "Player.h"
#include <curses.h>
#include "Camera.h"
#include <panel.h>

using namespace std;
volatile bool ApplicationExiting = false;

#define RENDERDELAY 80
#define INPUTDELAY 100

PANEL * _mapPanel;

void processSymbol(int symbol) {
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
	case KEY_MOUSE:
		// check what is that
		request_mouse_pos();
		// Mouse_status.x, Mouse_status.y etc...
		break;
	case 'q':
		ApplicationExiting = true;
		break;
	default:
		break;
	}
}

std::thread * _screenThread;
void renderScreenThread() {
	while (!ApplicationExiting) {
		Camera::Instance().Render();
		napms(RENDERDELAY);
	}
}


std::thread * _inThread;
void inputThread() {

	while (!ApplicationExiting) {
		int symbol = getch();
		processSymbol(symbol);
		//napms(INPUTDELAY);
	}
}

void dispose() {
	_screenThread->join();
	delete _screenThread;
	_inThread->join();
	delete _inThread;
	endwin();
}

void initConsole() {
	WINDOW * mainWnd = initscr();
	mouse_set(ALL_MOUSE_EVENTS);
	PDC_save_key_modifiers(TRUE);
	PDC_return_key_modifiers(TRUE);
	int _rows = 40;
	int _cols = 150;
	resize_term(_rows, _cols);
	mvwaddstr(mainWnd, 1, 1, "Loading map...");
	Camera::Instance().Init(_cols, _rows, mainWnd);



	raw();            // Accept raw keyboard input, so you don't have to
					  // strike Enter after every keypress.

	////nodelay(stdscr, false);
	noecho();
	keypad(mainWnd, TRUE);
	_screenThread = new std::thread(renderScreenThread);
	_inThread = new std::thread(inputThread);
}

#ifdef _CONSOLE
int main() {
#else
int WinMain() {
#endif
	initConsole();
	while (!ApplicationExiting) {
		napms(RENDERDELAY);
	}
	dispose();
	return 0;
}
