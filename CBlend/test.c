#include "src/cblend_blend.h"
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <time.h>

void Noise(uint8_t *buf, uint32_t lenBytes, uint32_t aMask, uint32_t oMask);
void Seed(uint32_t);
void ConvertColorSpace(uint8_t *buf, uint32_t lenBytes, uint8_t mode);

float ent(uint8_t *buf, uint32_t lenBytes) {
	uint8_t *end = buf + lenBytes;
	uint32_t b[256] = {0};
	while(buf<end) b[*buf++]++;
	double entf = 0;
	for (int i = 0; i < 256; i++) {
		if(b[i]) {
			double r = ((double)b[i] / (double)lenBytes);
			entf += -r * log2(r);
        }
    }
	return entf;
}

int main() {
	uint8_t *buf1, *buf2;
	const uint32_t w = 320, h = 240;
	const uint32_t nBytes = w * h * 4;
	buf1 = malloc(nBytes);
	buf2 = malloc(nBytes);
	Seed(time(NULL));
	for(int i = 0; i <= B__LAST; i++) {
		Noise(buf1, nBytes, 0xFFFFFFFF, 0xFFFFFFFF);
		Noise(buf2, nBytes, 0xFFFFFFFF, 0xFFFFFFFF);
		//printf("Testing %d...\n", i);
		Blend(i, buf1, buf2, nBytes, .7);
		ConvertColorSpace(buf1, nBytes, 0);
		Noise(buf2, nBytes, 0xFFFFFFFF, 0xFFFFFFFF);
		BlendV(i, buf1, buf2, nBytes, .6);
		ConvertColorSpace(buf1, nBytes, 1);
	}
	
	//printf("Testing noise...\n");
	Noise(buf1, nBytes, 0xFFFFFFFF, 0xFFFFFFFF);

}