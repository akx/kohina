#define B_NORMAL 1
#define B_LIGHTEN 2
#define B_DARKEN 3
#define B_MULTIPLY 4
#define B_AVERAGE 5
#define B_ADD 6
#define B_SUBTRACT 7
#define B_DIFFERENCE 8
#define B_NEGATION 9
#define B_SCREEN 10
#define B_EXCLUSION 11
#define B_OVERLAY 12
#define B_SOFTLIGHT 13
#define B_HARDLIGHT 14
#define B_COLORDODGE 15
#define B_COLORBURN 16
#define B_LINEARDODGE 17
#define B_LINEARBURN 18
#define B_LINEARLIGHT 19
#define B_VIVIDLIGHT 20
#define B_PINLIGHT 21
#define B_HARDMIX 22
#define B_REFLECT 23
#define B_GLOW 24
#define B_PHOENIX 25
#define B__LAST 25
#include <stdint.h>
void Blend(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O);
void BlendV(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O);
