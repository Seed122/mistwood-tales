project(PDCurses C)
SET(PDCURSES_DIR ${CMAKE_CURRENT_SOURCE_DIR}/PDCurses  CACHE INTERNAL "source_list")
include_directories(${PDCURSES_DIR})

aux_source_directory(${PDCURSES_DIR}/pdcurses PDCURSES_CORE_FILES) 
 #set(PDC_WIDE ON CACHE BOOL)
if(WIN32)
    aux_source_directory(${PDCURSES_DIR}/win32a PDCURSES_PLATFORM_FILES) 
    add_library (PDCurses STATIC ${PDCURSES_PLATFORM_FILES} ${PDCURSES_CORE_FILES})
    add_executable(fireworks ${PDCURSES_DIR}/demos/firework.c)
    target_link_libraries (fireworks PDCurses)

else()
    add_executable(fireworks ${PDCURSES_DIR}/demos/firework.c)

 SET (CMAKE_C_FLAGS "-O2 -DHAVE_CONFIG_H -DXCURSES -DDYNAMIC -I /usr/include/X11 -I /usr/include")
     aux_source_directory(${PDCURSES_DIR}/x11 PDCURSES_PLATFORM_FILES) 
    set(CMAKE_POSITION_INDEPENDENT_CODE ON)
    add_library (PDCurses STATIC ${PDCURSES_PLATFORM_FILES} ${PDCURSES_CORE_FILES})
    target_compile_definitions(PDCurses PUBLIC PDC_WIDE=1)

    link_directories(/usr/lib/x86_64-linux-gnu)
    
    target_link_libraries (PDCurses Xaw7 X11 Xmu Xt SM ICE Xext Xpm)
    target_link_libraries (fireworks PDCurses)
endif()




