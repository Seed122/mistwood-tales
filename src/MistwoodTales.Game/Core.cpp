#include <ctype.h>
#include <stdlib.h>
#include <thread>
#include <curses.h>
#include "Camera.h"
#include "Globals.h"

using namespace std;

void dispose() {
	delete Globals::Cam;
	endwin();
}


volatile bool Globals::isShuttingDown = false;
Camera * Globals::Cam = nullptr;

void initConsole() {

 	WINDOW * mainWnd = initscr();
	mouse_set(ALL_MOUSE_EVENTS);
	PDC_save_key_modifiers(TRUE);
	PDC_return_key_modifiers(TRUE);
	int _rows = 45;
	int _cols = 150;
	resize_term(_rows, _cols);
	mvwaddstr(mainWnd, 1, 1, "Loading...");
	Globals::Cam = new Camera(_cols, _rows, mainWnd);
	curs_set(0);
	raw();            // Accept raw keyboard input, so you don't have to
					  // strike Enter after every keypress.

	////nodelay(stdscr, false);
	noecho();
	keypad(mainWnd, TRUE);
}

#ifndef WIN32
int main() {
#else
int WinMain() {
#endif
	initConsole();
	while (!Globals::isShuttingDown) {
		napms(1000);
	}
	dispose();
	return 0;
}
