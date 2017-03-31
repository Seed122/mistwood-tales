#include <curses.h>
#pragma once
class Camera
{
public:
	int Width;
	int Height;
	static Camera& Instance()
	{
		// согласно стандарту, этот код ленивый и потокобезопасный
		static Camera w;
		return w;
	};
	void Render();
	WINDOW * Window;
	static attr_t GetOutputRgbColor(short fr, short fg, short fb, short br, short bg, short bb);
private:
	int _screenCenterX;
	int _screenCenterY;
	int _camX1;
	int _camX2;
	int _camY1;
	int _camY2;
	void RenderPlayer();
	void RenderNPCs();
	void RenderLandscape();
	Camera();
	~Camera();
	// необходимо также запретить копирование
	Camera(Camera const&) = delete;
	Camera& operator= (Camera const&) = delete;
};

