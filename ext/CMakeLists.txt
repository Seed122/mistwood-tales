project(PDCurses)

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses)
message ("PDCurses dir: " ${CMAKE_CURRENT_SOURCE_DIR}/PDCurses)


file(GLOB_RECURSE PDCURSES_CORE_FILES PDCurses/pdcurses/*)
if(WIN32)
	file(GLOB_RECURSE PDCURSES_PLATFORM_FILES PDCurses/win32a/*)
else()
	file(GLOB_RECURSE PDCURSES_PLATFORM_FILES PDCurses/x11/*)
endif()
add_library (PDCurses STATIC  ${PDCURSES_CORE_FILES} ${PDCURSES_PLATFORM_FILES})


add_executable(fireworks ${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/demos/firework.c)
target_link_libraries (fireworks PDCurses)
