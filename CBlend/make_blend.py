
blender_defs = u"""
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
"""


prelude = ur"""
#include <stdint.h>
#include "cblend_common.h"

static int Min(int a, int b) { return (a < b ? a : b); }
static int Max(int a, int b) { return (a > b ? a : b); }
static int Abs(int a) { return (a < 0 ? -a : a); }
#define Alpha(B,L,O)    ((uint8_t)(O * B + (1 - O) * L))
#define BLENDER_FUNC(name) static uint8_t CBlend_##name(uint8_t B, uint8_t L)
""" + blender_defs

import re
blenders = [m.group(1) for m in re.finditer("BLENDER_FUNC\(([^)]+)", blender_defs)]




def indent(code, nTabs):
	tabs = "\t" * nTabs
	return "\n".join("%s%s" % (tabs, line) for line in code.splitlines())

defs = []



for i, mode in enumerate(blenders):
	name = ("B_%s" % mode.upper())
	num = i + 1
	defs.append((name, num, mode))
	
	
def emit_c(stream, header_stream):
	for name, num, mode in defs:
		print >>stream, "#define %s %d" % (name, num)
		print >>header_stream, "#define %s %d" % (name, num)
	print >>header_stream, "#define B__LAST %d" % (num)
	print >>stream
	print >>stream, prelude
	print >>stream
	
	blender_code_alpha = u"""
while(buf1 < end) {
	buf1[0] = Alpha(CBlend_%(algo)s(buf1[0], buf2[0]), buf2[0], O);
	buf1[1] = Alpha(CBlend_%(algo)s(buf1[1], buf2[1]), buf2[1], O);
	buf1[2] = Alpha(CBlend_%(algo)s(buf1[2], buf2[2]), buf2[2], O);
	buf1 += 4;
	buf2 += 4;
}	
"""

	blender_code_valpha = u"""
while(buf1 < end) {
	float alpha = buf2[3] * O;
	buf1[0] = Alpha(CBlend_%(algo)s(buf1[0], buf2[0]), buf2[0], alpha);
	buf1[1] = Alpha(CBlend_%(algo)s(buf1[1], buf2[1]), buf2[1], alpha);
	buf1[2] = Alpha(CBlend_%(algo)s(buf1[2], buf2[2]), buf2[2], alpha);
	buf1 += 4;
	buf2 += 4;
}	
"""


	blender_code_solid = u"""
while(buf1 < end) {
	buf1[0] = CBlend_%(algo)s(buf1[0], buf2[0]);
	buf1[1] = CBlend_%(algo)s(buf1[1], buf2[1]);
	buf1[2] = CBlend_%(algo)s(buf1[2], buf2[2]);
	buf1 += 4;
	buf2 += 4;
}	
"""
	
	def emit_blend(fname, template, mul = None):
		template = template.strip()
		print >>stream, "static void %s(int mode, uint8_t * restrict buf1, uint8_t * restrict buf2, uint32_t lenBytes, float O) {" % fname
		print >>stream, "\tuint8_t *end = buf1 + lenBytes;"
		if mul is not None:
			print >> stream, "\tO *= %s;" % mul
		print >>stream, "\tswitch(mode) {"
		for name, num, mode in defs:
			print >>stream, "\t\tcase %s:" % name
			print >>stream, indent(template % {"algo": mode}, 3)
			print >>stream, "\t\tbreak;"
			print >>stream, "\t\t"
		print >>stream, "\t}"
		print >>stream, "}"
		
	emit_blend("BlendSolid", blender_code_solid)
	emit_blend("BlendAlpha", blender_code_alpha)
	emit_blend("BlendVAlpha", blender_code_valpha, "0.00392156862745098")
	
	entrypoint_code = u"""
EXPORT void Blend(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O) {
	if(O <= 0) return;
	if(O >= 1) BlendSolid(mode, buf1, buf2, lenBytes, 1);
	else       BlendAlpha(mode, buf1, buf2, lenBytes, O);
}

EXPORT void BlendV(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O) {
	if(O <= 0) return;
	BlendVAlpha(mode, buf1, buf2, lenBytes, O);
}

"""

	print >> stream, entrypoint_code
	print >> header_stream, "#include <stdint.h>"
	print >> header_stream, "void Blend(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O);"
	print >> header_stream, "void BlendV(int mode, uint8_t *buf1, uint8_t *buf2, uint32_t lenBytes, float O);"
	

def emit_cs(stream):
	print >>stream, "namespace CBlend {"
	print >>stream, "\tpublic enum BlendMode {"
	for name, num, mode in defs:
		print >>stream, "\t\t%s = %d," % (mode, num)
	print >>stream, "\t\t_Dummy"
	print >>stream, "\t};"
	print >>stream, "\tpublic class CBlend {"
	print >>stream, "\t\t[DllImport(\"cblend.dll\", EntryPoint = \"Blend\")]"
	print >>stream, "\t\tstatic extern void um_Blend(int mode, IntPtr buf1, IntPtr buf2, UInt32 lenBytes, float O);"
	print >>stream, "\t\tpublic static void Blend(BlendMode mode, IntPtr buf1, IntPtr buf2, UInt32 lenBytes, float O) {"
	print >>stream, "\t\t\tum_Blend((int)mode, buf1, buf2, lenBytes, O);"
	print >>stream, "\t\t}"
	print >>stream, "\t}"
	print >>stream, "}"
	
	

if __name__ == "__main__":
	import sys
	emit_c(
		file("src/cblend_blend.c", "wb"),
		file("src/cblend_blend.h", "wb")
	)