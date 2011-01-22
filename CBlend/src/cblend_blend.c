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


#include <stdint.h>
#include "cblend_common.h"

static int Min(int a, int b) { return (a < b ? a : b); }
static int Max(int a, int b) { return (a > b ? a : b); }
static int Abs(int a) { return (a < 0 ? -a : a); }
#define Alpha(B,L,O)    ((uint8_t)(O * B + (1 - O) * L))
#define BLENDER_FUNC(name) static uint8_t CBlend_##name(uint8_t B, uint8_t L)

BLENDER_FUNC(Normal) {
	return L;
}

BLENDER_FUNC(Lighten) {
	return (L > B) ? L : B;
}

BLENDER_FUNC(Darken) {
	return (L > B) ? B : L;
}

BLENDER_FUNC(Multiply) {
	return (B * L) / 255;
}

BLENDER_FUNC(Average) {
	return (B + L) / 2;
}

BLENDER_FUNC(Add) {
	return Min(255, B + L);
}

BLENDER_FUNC(Subtract) {
	if(B + L < 255) return 0;
	return B + L - 255;
}

BLENDER_FUNC(Difference) {
	return Abs(B - L);
}

BLENDER_FUNC(Negation) {
	return 255 - Abs(255 - B - L);
}

BLENDER_FUNC(Screen) {
	return (255 - (((255 - B) * (255 - L)) >> 8));
}

BLENDER_FUNC(Exclusion) {
	return (B + L - 2 * B * L / 255);
}

BLENDER_FUNC(Overlay) {
	return (
		(L < 128) ?
		(2 * B * L / 255) :
		(255 - 2 * (255 - B) * (255 - L) / 255)
	);
}

BLENDER_FUNC(SoftLight) {
	return (
		(L < 128) ?
		(2*((B>>1)+64))*((float)L/255) :
		(255-(2*(255-((B>>1)+64))*(float)(255-L)/255))
	);
}

BLENDER_FUNC(HardLight) {
	return (CBlend_Overlay(L,B));
}

BLENDER_FUNC(ColorDodge) {
	return (
		(L == 255) ?
		L :
		Min(255, ((B << 8 ) / (255 - L)))
	);
}

BLENDER_FUNC(ColorBurn) {
	return ((uint8_t)((L == 0) ? L:Max(0, (255 - ((255 - B) << 8 ) / L))));
}

BLENDER_FUNC(LinearDodge) {
	return (CBlend_Add(B,L));
}

BLENDER_FUNC(LinearBurn) {
	return (CBlend_Subtract(B,L));
}

BLENDER_FUNC(LinearLight) {
	return ((uint8_t)(L < 128)?CBlend_LinearBurn(B,(2 * L)):CBlend_LinearDodge(B,(2 * (L - 128))));
}

BLENDER_FUNC(VividLight) {
	return ((uint8_t)(L < 128)?CBlend_ColorBurn(B,(2 * L)):CBlend_ColorDodge(B,(2 * (L - 128))));
}

BLENDER_FUNC(PinLight) {
	return ((uint8_t)(L < 128)?CBlend_Darken(B,(2 * L)):CBlend_Lighten(B,(2 * (L - 128))));
}

BLENDER_FUNC(HardMix) {
	return ((uint8_t)((CBlend_VividLight(B,L) < 128) ? 0:255));
}

BLENDER_FUNC(Reflect) {
	return ((uint8_t)((L == 255) ? L:Min(255, (B * B / (255 - L)))));
}

BLENDER_FUNC(Glow) {
	return (CBlend_Reflect(L,B));
}

BLENDER_FUNC(Phoenix) {
	return ((uint8_t)(Min(B,L) - Max(B,L) + 255));
}


static void BlendSolid(int mode, uint8_t * restrict buf1, uint8_t * restrict buf2, uint32_t lenBytes, float O) {
	uint8_t *end = buf1 + lenBytes;
	switch(mode) {
		case B_NORMAL:
			while(buf1 < end) {
				buf1[0] = CBlend_Normal(buf1[0], buf2[0]);
				buf1[1] = CBlend_Normal(buf1[1], buf2[1]);
				buf1[2] = CBlend_Normal(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LIGHTEN:
			while(buf1 < end) {
				buf1[0] = CBlend_Lighten(buf1[0], buf2[0]);
				buf1[1] = CBlend_Lighten(buf1[1], buf2[1]);
				buf1[2] = CBlend_Lighten(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DARKEN:
			while(buf1 < end) {
				buf1[0] = CBlend_Darken(buf1[0], buf2[0]);
				buf1[1] = CBlend_Darken(buf1[1], buf2[1]);
				buf1[2] = CBlend_Darken(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_MULTIPLY:
			while(buf1 < end) {
				buf1[0] = CBlend_Multiply(buf1[0], buf2[0]);
				buf1[1] = CBlend_Multiply(buf1[1], buf2[1]);
				buf1[2] = CBlend_Multiply(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_AVERAGE:
			while(buf1 < end) {
				buf1[0] = CBlend_Average(buf1[0], buf2[0]);
				buf1[1] = CBlend_Average(buf1[1], buf2[1]);
				buf1[2] = CBlend_Average(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_ADD:
			while(buf1 < end) {
				buf1[0] = CBlend_Add(buf1[0], buf2[0]);
				buf1[1] = CBlend_Add(buf1[1], buf2[1]);
				buf1[2] = CBlend_Add(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SUBTRACT:
			while(buf1 < end) {
				buf1[0] = CBlend_Subtract(buf1[0], buf2[0]);
				buf1[1] = CBlend_Subtract(buf1[1], buf2[1]);
				buf1[2] = CBlend_Subtract(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DIFFERENCE:
			while(buf1 < end) {
				buf1[0] = CBlend_Difference(buf1[0], buf2[0]);
				buf1[1] = CBlend_Difference(buf1[1], buf2[1]);
				buf1[2] = CBlend_Difference(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_NEGATION:
			while(buf1 < end) {
				buf1[0] = CBlend_Negation(buf1[0], buf2[0]);
				buf1[1] = CBlend_Negation(buf1[1], buf2[1]);
				buf1[2] = CBlend_Negation(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SCREEN:
			while(buf1 < end) {
				buf1[0] = CBlend_Screen(buf1[0], buf2[0]);
				buf1[1] = CBlend_Screen(buf1[1], buf2[1]);
				buf1[2] = CBlend_Screen(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_EXCLUSION:
			while(buf1 < end) {
				buf1[0] = CBlend_Exclusion(buf1[0], buf2[0]);
				buf1[1] = CBlend_Exclusion(buf1[1], buf2[1]);
				buf1[2] = CBlend_Exclusion(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_OVERLAY:
			while(buf1 < end) {
				buf1[0] = CBlend_Overlay(buf1[0], buf2[0]);
				buf1[1] = CBlend_Overlay(buf1[1], buf2[1]);
				buf1[2] = CBlend_Overlay(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SOFTLIGHT:
			while(buf1 < end) {
				buf1[0] = CBlend_SoftLight(buf1[0], buf2[0]);
				buf1[1] = CBlend_SoftLight(buf1[1], buf2[1]);
				buf1[2] = CBlend_SoftLight(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDLIGHT:
			while(buf1 < end) {
				buf1[0] = CBlend_HardLight(buf1[0], buf2[0]);
				buf1[1] = CBlend_HardLight(buf1[1], buf2[1]);
				buf1[2] = CBlend_HardLight(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORDODGE:
			while(buf1 < end) {
				buf1[0] = CBlend_ColorDodge(buf1[0], buf2[0]);
				buf1[1] = CBlend_ColorDodge(buf1[1], buf2[1]);
				buf1[2] = CBlend_ColorDodge(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORBURN:
			while(buf1 < end) {
				buf1[0] = CBlend_ColorBurn(buf1[0], buf2[0]);
				buf1[1] = CBlend_ColorBurn(buf1[1], buf2[1]);
				buf1[2] = CBlend_ColorBurn(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARDODGE:
			while(buf1 < end) {
				buf1[0] = CBlend_LinearDodge(buf1[0], buf2[0]);
				buf1[1] = CBlend_LinearDodge(buf1[1], buf2[1]);
				buf1[2] = CBlend_LinearDodge(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARBURN:
			while(buf1 < end) {
				buf1[0] = CBlend_LinearBurn(buf1[0], buf2[0]);
				buf1[1] = CBlend_LinearBurn(buf1[1], buf2[1]);
				buf1[2] = CBlend_LinearBurn(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARLIGHT:
			while(buf1 < end) {
				buf1[0] = CBlend_LinearLight(buf1[0], buf2[0]);
				buf1[1] = CBlend_LinearLight(buf1[1], buf2[1]);
				buf1[2] = CBlend_LinearLight(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_VIVIDLIGHT:
			while(buf1 < end) {
				buf1[0] = CBlend_VividLight(buf1[0], buf2[0]);
				buf1[1] = CBlend_VividLight(buf1[1], buf2[1]);
				buf1[2] = CBlend_VividLight(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PINLIGHT:
			while(buf1 < end) {
				buf1[0] = CBlend_PinLight(buf1[0], buf2[0]);
				buf1[1] = CBlend_PinLight(buf1[1], buf2[1]);
				buf1[2] = CBlend_PinLight(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDMIX:
			while(buf1 < end) {
				buf1[0] = CBlend_HardMix(buf1[0], buf2[0]);
				buf1[1] = CBlend_HardMix(buf1[1], buf2[1]);
				buf1[2] = CBlend_HardMix(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_REFLECT:
			while(buf1 < end) {
				buf1[0] = CBlend_Reflect(buf1[0], buf2[0]);
				buf1[1] = CBlend_Reflect(buf1[1], buf2[1]);
				buf1[2] = CBlend_Reflect(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_GLOW:
			while(buf1 < end) {
				buf1[0] = CBlend_Glow(buf1[0], buf2[0]);
				buf1[1] = CBlend_Glow(buf1[1], buf2[1]);
				buf1[2] = CBlend_Glow(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PHOENIX:
			while(buf1 < end) {
				buf1[0] = CBlend_Phoenix(buf1[0], buf2[0]);
				buf1[1] = CBlend_Phoenix(buf1[1], buf2[1]);
				buf1[2] = CBlend_Phoenix(buf1[2], buf2[2]);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
	}
}
static void BlendAlpha(int mode, uint8_t * restrict buf1, uint8_t * restrict buf2, uint32_t lenBytes, float O) {
	uint8_t *end = buf1 + lenBytes;
	switch(mode) {
		case B_NORMAL:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Normal(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Normal(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Normal(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LIGHTEN:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Lighten(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Lighten(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Lighten(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DARKEN:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Darken(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Darken(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Darken(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_MULTIPLY:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Multiply(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Multiply(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Multiply(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_AVERAGE:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Average(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Average(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Average(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_ADD:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Add(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Add(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Add(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SUBTRACT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Subtract(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Subtract(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Subtract(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DIFFERENCE:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Difference(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Difference(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Difference(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_NEGATION:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Negation(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Negation(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Negation(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SCREEN:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Screen(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Screen(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Screen(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_EXCLUSION:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Exclusion(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Exclusion(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Exclusion(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_OVERLAY:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Overlay(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Overlay(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Overlay(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SOFTLIGHT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_SoftLight(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_SoftLight(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_SoftLight(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDLIGHT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_HardLight(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_HardLight(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_HardLight(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORDODGE:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_ColorDodge(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_ColorDodge(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_ColorDodge(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORBURN:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_ColorBurn(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_ColorBurn(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_ColorBurn(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARDODGE:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_LinearDodge(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_LinearDodge(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_LinearDodge(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARBURN:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_LinearBurn(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_LinearBurn(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_LinearBurn(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARLIGHT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_LinearLight(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_LinearLight(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_LinearLight(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_VIVIDLIGHT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_VividLight(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_VividLight(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_VividLight(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PINLIGHT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_PinLight(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_PinLight(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_PinLight(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDMIX:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_HardMix(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_HardMix(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_HardMix(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_REFLECT:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Reflect(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Reflect(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Reflect(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_GLOW:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Glow(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Glow(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Glow(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PHOENIX:
			while(buf1 < end) {
				buf1[0] = Alpha(CBlend_Phoenix(buf1[0], buf2[0]), buf2[0], O);
				buf1[1] = Alpha(CBlend_Phoenix(buf1[1], buf2[1]), buf2[1], O);
				buf1[2] = Alpha(CBlend_Phoenix(buf1[2], buf2[2]), buf2[2], O);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
	}
}
static void BlendVAlpha(int mode, uint8_t * restrict buf1, uint8_t * restrict buf2, uint32_t lenBytes, float O) {
	uint8_t *end = buf1 + lenBytes;
	O *= 0.00392156862745098;
	switch(mode) {
		case B_NORMAL:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Normal(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Normal(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Normal(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LIGHTEN:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Lighten(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Lighten(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Lighten(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DARKEN:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Darken(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Darken(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Darken(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_MULTIPLY:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Multiply(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Multiply(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Multiply(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_AVERAGE:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Average(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Average(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Average(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_ADD:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Add(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Add(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Add(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SUBTRACT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Subtract(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Subtract(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Subtract(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_DIFFERENCE:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Difference(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Difference(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Difference(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_NEGATION:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Negation(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Negation(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Negation(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SCREEN:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Screen(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Screen(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Screen(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_EXCLUSION:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Exclusion(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Exclusion(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Exclusion(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_OVERLAY:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Overlay(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Overlay(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Overlay(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_SOFTLIGHT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_SoftLight(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_SoftLight(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_SoftLight(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDLIGHT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_HardLight(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_HardLight(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_HardLight(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORDODGE:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_ColorDodge(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_ColorDodge(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_ColorDodge(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_COLORBURN:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_ColorBurn(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_ColorBurn(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_ColorBurn(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARDODGE:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_LinearDodge(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_LinearDodge(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_LinearDodge(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARBURN:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_LinearBurn(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_LinearBurn(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_LinearBurn(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_LINEARLIGHT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_LinearLight(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_LinearLight(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_LinearLight(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_VIVIDLIGHT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_VividLight(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_VividLight(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_VividLight(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PINLIGHT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_PinLight(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_PinLight(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_PinLight(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_HARDMIX:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_HardMix(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_HardMix(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_HardMix(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_REFLECT:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Reflect(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Reflect(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Reflect(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_GLOW:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Glow(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Glow(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Glow(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
		case B_PHOENIX:
			while(buf1 < end) {
				float alpha = buf2[3] * O;
				buf1[0] = Alpha(CBlend_Phoenix(buf1[0], buf2[0]), buf2[0], alpha);
				buf1[1] = Alpha(CBlend_Phoenix(buf1[1], buf2[1]), buf2[1], alpha);
				buf1[2] = Alpha(CBlend_Phoenix(buf1[2], buf2[2]), buf2[2], alpha);
				buf1 += 4;
				buf2 += 4;
			}
		break;
		
	}
}

EXPORT void Blend(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O) {
	if(O <= 0) return;
	if(O >= 1) BlendSolid(mode, buf1, buf2, lenBytes, 1);
	else       BlendAlpha(mode, buf1, buf2, lenBytes, O);
}

EXPORT void BlendV(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O) {
	if(O <= 0) return;
	BlendVAlpha(mode, buf1, buf2, lenBytes, O);
}


