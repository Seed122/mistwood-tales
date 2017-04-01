#pragma once
#include <curses.h>
#include "panel.h"
#include <string>
#include <vector>
#include "Sighting.h"
#include "Point.h"
#include <thread>
using namespace std;

class Camera
{
public:
	int Width;
	int Height;
	PANEL* PlayerPanel;
	PANEL* SightingsPanel;
	void Render();
	WINDOW * Window;
	static attr_t GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb);
	Point AbsoluteToRelativeCoords(Point absoluteCoords);
	Point RelativeToAbsoluteCoords(Point relativeCoords);
	Camera(int width, int height, WINDOW * window);
	~Camera();
	void InputThreadFunction() const;
	void RenderScreenThreadFunction();
private:
	int _screenCenterX;
	int _screenCenterY;
	int _camX1;
	int _camX2;
	int _camY1;
	int _camY2;
	void RenderContent();
	void RenderPlayerPanel();
	void RenderPlayer();
	void RenderNPCs();
	void RenderLandscape();
	void RenderSightingsPanel();
	void SetColor(attr_t);
	vector<Sighting> Sightings;
	thread * _screenThread;
	thread * _inThread;
	// запрещаем копирование
	Camera(Camera const&) = delete;
	Camera& operator= (Camera const&) = delete;
};

