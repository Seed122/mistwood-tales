#include <signal.h>
#include <ctype.h>
#include <stdlib.h>
#include <sys/types.h>
#include <time.h>
#include <thread>
#include "Map.h"
#include "World.h"
#include "Player.h"
#include <curses.h>
#include "Camera.h"

using namespace std;
//using namespace World;



volatile bool ApplicationExiting = false;

#define RENDERDELAY 80
#define INPUTDELAY 100


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
		erase();
		Camera::Instance().Render();
		napms(RENDERDELAY);
		refresh();
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
	WINDOW * wnd = initscr();
	Camera::Instance().Window = wnd;

	raw();            // Accept raw keyboard input, so you don't have to
					  // strike Enter after every keypress.

	noecho();         // Don't echo user input back to the screen.
	start_color();
	//short r = floor(52 * 3.921)/10;
	//short g = floor(168 * 3.921) / 10;
	//short b = floor(83 * 3.921) / 10;
	//short colorId = 15;
	////init_color(colorId, r, g, b);
	////init_pair(1, COLOR_WHITE, COLOR_BLACK);
	//

	////init_pair(2, colorId, colorId);
	////init_pair(3, COLOR_BLACK, COLOR_GREEN);
	////nodelay(stdscr, false);
	//noecho();
	keypad(wnd, TRUE);
	//
	//bool exit = false;
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
