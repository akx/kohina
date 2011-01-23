#include "cblend_common.h"

#include <stdlib.h>
#include <stdint.h>

#define MT_INCLUDE_MODE
#include "mt19937ar.c"


EXPORT void Seed(uint32_t v) {
	init_genrand(v);
}

EXPORT void Noise(uint8_t *buf, uint32_t lenBytes, uint32_t aMask, uint32_t oMask) {
	uint32_t *pbuf = (uint32_t*) buf;
	uint32_t *pend = pbuf + (lenBytes >> 2);
	uint32_t iOMask = ~oMask;
	uint32_t aoMask = aMask & oMask;
	while(pbuf < pend) {
		uint32_t ov = *pbuf;
		*pbuf++ = (genrand_int32() & aoMask) | (ov & iOMask);
	}
}

EXPORT void Clear(uint8_t *buf, uint32_t lenBytes, uint32_t color, uint32_t oMask) {
	uint32_t *pbuf = (uint32_t*) buf;
	uint32_t *pend = pbuf + (lenBytes >> 2);
	uint32_t iOMask = ~oMask;
	while(pbuf < pend) {
		uint32_t nv = color;
		uint32_t ov = *pbuf;
		*pbuf++ = (nv & oMask) | (ov & iOMask);
	}
}

EXPORT void CRemap(uint8_t *buf, uint32_t lenBytes, uint32_t mask) {
	uint32_t *pbuf = (uint32_t*) buf;
	uint32_t *pend = pbuf + (lenBytes >> 2);
	while(pbuf < pend) {
		uint32_t v = *pbuf;
		*pbuf++ =
			(((v >> ((mask >> 0) & 0xFF)) & 0xFF) << 0) |
			(((v >> ((mask >> 8) & 0xFF)) & 0xFF) << 8) |
			(((v >> ((mask >> 16) & 0xFF)) & 0xFF) << 16) |
			(((v >> ((mask >> 24) & 0xFF)) & 0xFF) << 24);
	}
}

/*
 Adapted from http://en.literateprograms.org/RGB_to_HSV_color_space_conversion_(C)
*/

#define MIN3(x,y,z)  ((y) <= (z) ? ((x) <= (y) ? (x) : (y)) : ((x) <= (z) ? (x) : (z)))
#define MAX3(x,y,z)  ((y) >= (z) ? ((x) >= (y) ? (x) : (y)) : ((x) >= (z) ? (x) : (z)))

static void rgb_to_hsv(uint8_t r, uint8_t g, uint8_t b, uint8_t *hue, uint8_t *sat, uint8_t *val) {
	uint8_t rgb_min, rgb_max, rgb_delta;
	rgb_max = MAX3(r, g, b);
	if(rgb_max == 0) {
		*hue = 0;
		*sat = 0;
		*val = 0;
		return;
	}
	rgb_min = MIN3(r, g, b);
	*val = rgb_max;
	rgb_delta = rgb_max - rgb_min;

	*sat = (uint8_t)(255 * rgb_delta / *val);
	if (*sat == 0) { // Monochromatic
		*hue = 0;
		return;
	}

	/* Compute hue */
	if (rgb_max == r)		*hue = 43 * (g - b) / rgb_delta;
	else if (rgb_max == g)	*hue = 85 + 43 * (b - r) / rgb_delta;
	else					*hue = 171 + 43 * (r - g) / rgb_delta;
}



static void hsv_to_rgb(uint8_t ih, uint8_t is, uint8_t iv, uint8_t *r, uint8_t *g, uint8_t *b)
{
	float f, x, y, z, h, s, v;
	int i;

	if (is == 0) {
		*r = *g = *b = iv;
		return;
	}
	
	v = iv;
	s = is / 255.0f;
	h = ih / 42.0f;
	i = (int)h;
	f = h - i;
	x = v * s;
	y = x * f;
	v += 0.5f;
	z = v - x;

	switch (i) {
		case 6:
		case 0:	*r = v;	*g = z + y;	*b = z;	break;
		case 1:	*r = v - y;	*g = v;	*b = z; break;
		case 2:	*r = z;	*g = v;	*b = z + y;	break;
		case 3:	*r = z;	*g = v - y;	*b = v;	break;
		case 4:	*r = z + y;	*g = z;	*b = v;	break;
		case 5:	*r = v;	*g = z;	*b = v - y;	break;
	}
}



EXPORT void ConvertColorSpace(uint8_t *buf, uint32_t lenBytes, uint8_t mode) {
	uint8_t *end = buf + lenBytes;
	while(buf < end) {
		if(mode == 0) {
			rgb_to_hsv(buf[0], buf[1], buf[2], &buf[0], &buf[1], &buf[2]);
		} else if(mode == 1) {
			hsv_to_rgb(buf[0], buf[1], buf[2], &buf[0], &buf[1], &buf[2]);
		}
		buf += 4;
	}
}

