#
include_directories(${PDCURSES_DIR})
aux_source_directory(${CMAKE_CURRENT_SOURCE_DIR} GAME_FILES) 
add_executable(mistwood ${GAME_FILES})

find_package (Threads)
target_link_libraries (mistwood ${CMAKE_THREAD_LIBS_INIT})
target_compile_definitions(mistwood PUBLIC PDC_WIDE=1)
target_link_libraries (mistwood PDCurses)