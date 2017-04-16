project(PDCurses C)

include_directories(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses)


#file(GLOB_RECURSE PDCURSES_CORE_FILES PDCurses/pdcurses/*)
aux_source_directory(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/pdcurses PDCURSES_CORE_FILES) 
if(WIN32)
	#file(GLOB_RECURSE PDCURSES_PLATFORM_FILES PDCurses/win32a/*)
    aux_source_directory(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/win32a PDCURSES_PLATFORM_FILES) 


    add_library (PDCurses STATIC ${PDCURSES_PLATFORM_FILES} ${PDCURSES_CORE_FILES})
    add_executable(fireworks ${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/demos/firework.c)
    target_link_libraries (fireworks PDCurses)

else()
    include_directories(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses)
    include_directories(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/x11)
    include_directories(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/pdcurses)

set(CMAKE_CXX_STANDARD 11)  # for c++11
find_package(X11 REQUIRED)
link_libraries(${X11_LIBRARIES})
include_directories(${X11_INCLUDE_DIR})
 SET (CMAKE_C_FLAGS "-O2 -fPIC -DHAVE_CONFIG_H -DXCURSES -DDYNAMIC -I /usr/include/X11 -I /usr/include ")
    # SET (CMAKE_CXX_FLAGS_RELEASE "-C -O2 -DHAVE_CONFIG_H -DXCURSES -DDYNAMIC")
    
	#file(GLOB_RECURSE PDCURSES_PLATFORM_FILES PDCurses/x11/*)
    aux_source_directory(${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/x11 PDCURSES_PLATFORM_FILES) 
        
    add_library (PDCurses STATIC ${PDCURSES_PLATFORM_FILES} ${PDCURSES_CORE_FILES})
    add_executable(fireworks ${CMAKE_CURRENT_SOURCE_DIR}/PDCurses/demos/firework.c)
    add_library(Xlib STATIC IMPORTED)
    link_directories(/usr/lib/x86_64-linux-gnu)
    target_link_libraries (fireworks libX11.a)
    target_link_libraries (fireworks PDCurses)
endif()




