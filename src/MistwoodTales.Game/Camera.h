#pragma once
#include <curses.h>
#include "panel.h"
#include <string>
#include <vector>
#include "Sighting.h"
#include "Point.h"
#include <thread>
#include <list>
using namespace std;

class Camera
{
public:
	int Width;
	int Height;
	void Render();
	static attr_t GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb);
	Point AbsoluteToRelativeCoords(Point absoluteCoords);
	Point RelativeToAbsoluteCoords(Point relativeCoords);
	Camera(int width, int height, WINDOW * window);
	~Camera();
	void InputThreadFunction();
	void RenderScreenThreadFunction();
	void processSymbol(int symbol);
	void AddSighting(Sighting sighting);
private:
	int _screenCenterX;
	int _screenCenterY;
	int _camX1;
	int _camX2;
	int _camY1;
	int _camY2;
	WINDOW * Window;
	PANEL* PlayerPanel;
	PANEL* SightingsBorderedPanel;
	PANEL* SightingsPanel;
	void RenderContent();
	void RenderPlayerPanel();
	void RenderPlayer();
	void RenderNPCs();
	void RenderLandscape();
	void RenderSightingsPanel();
	void SetColor(attr_t) const;
	list<Sighting> Sightings;
	thread * _screenThread;
	thread * _inThread;
	// запрещаем копирование
	Camera(Camera const&) = delete;
	Camera& operator= (Camera const&) = delete;
};

