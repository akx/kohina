@echo off
echo Recreating C and CS
python make_blend.py
echo Building DLL
set LIBSRC=src/cblend_blend.c src/cblend_util.c
gcc -s -Isrc -mfpmath=sse -msse -msse2 -O3 -Wall -o cblend.dll -Wl,--out-implib,libcblenddll.a -shared -std=c99 %LIBSRC%
echo Building test app
gcc -Isrc -Wall -g -o cblend_test.exe -O3 -std=c99 test.c libcblenddll.a
echo Done
