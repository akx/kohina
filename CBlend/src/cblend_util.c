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
