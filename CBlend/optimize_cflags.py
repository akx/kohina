import new, os
import xml.etree.ElementTree as etree
import hashlib
import time
import textwrap
from pygene.organism import Organism
from pygene.gene import IntGene
from pygene.population import Population

def read_builtin():
	sAr = """
	-mfpmath=sse @gol
-mno-fancy-math-387 @gol
-mno-fp-ret-in-387  @gol
-mrtd  -malign-double @gol
-mpreferred-stack-boundary=@var{num}
-mincoming-stack-boundary=@var{num}
-mcld -mcx16 -msahf -mmovbe -mcrc32 -mrecip @gol
-mmmx  -msse  -msse2 -msse3 -mssse3 -msse4.1 -msse4.2 -msse4 -mavx @gol
-maes -mpclmul -mfused-madd @gol
-msse4a -m3dnow -mpopcnt -mabm -mfma4 -mxop -mlwp @gol
-mthreads  -mno-align-stringops  -minline-all-stringops @gol
-minline-stringops-dynamically -mstringop-strategy=@var{alg} @gol
-mpush-args  -maccumulate-outgoing-args  -m128bit-long-double @gol
-m96bit-long-double  -mregparm=@var{num}  -msseregparm @gol
-mveclibabi=@var{type} -mpc32 -mpc64 -mpc80 -mstackrealign @gol
-momit-leaf-frame-pointer  -mno-red-zone -mno-tls-direct-seg-refs @gol
-mcmodel=@var{code-model} -mabi=@var{name} @gol
	"""
	sOpt="""
	-falign-functions[=@var{n}] -falign-jumps[=@var{n}] @gol
-falign-labels[=@var{n}] -falign-loops[=@var{n}] @gol
-fauto-inc-dec -fbranch-probabilities -fbranch-target-load-optimize @gol
-fbranch-target-load-optimize2 -fbtr-bb-exclusive -fcaller-saves @gol
-fcheck-data-deps -fconserve-stack -fcprop-registers -fcrossjumping @gol
-fcse-follow-jumps -fcse-skip-blocks -fcx-fortran-rules -fcx-limited-range @gol
-fdata-sections -fdce -fdce @gol
-fdelayed-branch -fdelete-null-pointer-checks -fdse -fdse @gol
-fearly-inlining -fipa-sra -fexpensive-optimizations -ffast-math @gol
-ffinite-math-only -ffloat-store -fexcess-precision=@var{style} @gol
-fforward-propagate -ffunction-sections @gol
-fgcse -fgcse-after-reload -fgcse-las -fgcse-lm @gol
-fgcse-sm -fif-conversion -fif-conversion2 -findirect-inlining @gol
-finline-functions -finline-functions-called-once -finline-limit=@var{n} @gol
-finline-small-functions -fipa-cp -fipa-cp-clone -fipa-matrix-reorg -fipa-pta @gol
-fipa-pure-const -fipa-reference -fipa-struct-reorg @gol
-fipa-type-escape -fira-algorithm=@var{algorithm} @gol
-fira-region=@var{region} -fira-coalesce @gol
-fira-loop-pressure -fno-ira-share-save-slots @gol
-fno-ira-share-spill-slots -fira-verbose=@var{n} @gol
-fivopts -fkeep-inline-functions -fkeep-static-consts @gol
-floop-block -floop-interchange -floop-strip-mine -fgraphite-identity @gol
-floop-parallelize-all @gol
-fmerge-all-constants -fmerge-constants -fmodulo-sched @gol
-fmodulo-sched-allow-regmoves -fmove-loop-invariants @gol
-fno-branch-count-reg @gol
-fno-defer-pop -fno-function-cse -fno-guess-branch-probability @gol
-fno-inline -fno-math-errno -fno-peephole -fno-peephole2 @gol
-fno-sched-interblock -fno-sched-spec -fno-signed-zeros @gol
-fno-toplevel-reorder -fno-trapping-math -fno-zero-initialized-in-bss @gol
-fomit-frame-pointer -foptimize-register-move -foptimize-sibling-calls @gol
-fpeel-loops -fpredictive-commoning -fprefetch-loop-arrays @gol
-freciprocal-math -fregmove -frename-registers -freorder-blocks @gol
-freorder-blocks-and-partition -freorder-functions @gol
-frerun-cse-after-loop -freschedule-modulo-scheduled-loops @gol
-frounding-math -fsched2-use-superblocks -fsched-pressure @gol
-fsched-spec-load -fsched-spec-load-dangerous @gol
-fsched-stalled-insns-dep[=@var{n}] -fsched-stalled-insns[=@var{n}] @gol
-fsched-group-heuristic -fsched-critical-path-heuristic @gol
-fsched-spec-insn-heuristic -fsched-rank-heuristic @gol
-fsched-last-insn-heuristic -fsched-dep-count-heuristic @gol
-fschedule-insns -fschedule-insns2 @gol
-fselective-scheduling -fselective-scheduling2 @gol
-fsel-sched-pipelining -fsel-sched-pipelining-outer-loops @gol
-fsignaling-nans -fsingle-precision-constant -fsplit-ivs-in-unroller @gol
-fsplit-wide-types @gol
-fstrict-aliasing -fstrict-overflow -fthread-jumps -ftracer @gol
-ftree-builtin-call-dce -ftree-ccp -ftree-ch -ftree-copy-prop @gol
-ftree-copyrename -ftree-dce @gol
-ftree-dominator-opts -ftree-dse -ftree-forwprop -ftree-fre -ftree-loop-im @gol
-ftree-phiprop -ftree-loop-distribution @gol
-ftree-loop-ivcanon -ftree-loop-linear -ftree-loop-optimize @gol
-ftree-parallelize-loops=@var{n} -ftree-pre -ftree-pta -ftree-reassoc @gol
-ftree-sink -ftree-sra -ftree-switch-conversion @gol
-ftree-ter -ftree-vect-loop-version -ftree-vectorize -ftree-vrp @gol
-funit-at-a-time -funroll-all-loops -funroll-loops @gol
-funswitch-loops @gol
-fvariable-expansion-in-unroller -fvect-cost-model -fvpt -fweb @gol
-fwhole-program @gol"""
	
	return [(v, '') for v in (sAr + " " + sOpt).split() if "@" not in v]

class CmdlineOrganismBase(Organism):
	baseCmd = "gcc -march=native -std=c99 FLAGS -o OUTPUT testbi.c"
	cache = {}
	
	def get_flags_str(self):
		flags = set()
		for geneName, gene in self.genome.iteritems():
			val = gene._opt[self[geneName] % len(gene._opt)]
			#print geneName, val, gene._opt
			if val:
				flags.add(val)
		flagStr = " ".join(sorted(flags))
		return flagStr
	
	def fitness(self):
		flagStr = self.get_flags_str()
		id = hashlib.md5(flagStr).digest().encode("base64").strip().replace("/", "_").replace("=", "-")
		outExe = "temp\\tmp_%s.exe" % id
		t = None
		if outExe in self.cache and os.path.isfile(outExe):
			t = self.cache[outExe]
		else:
			cmd = self.baseCmd.replace("FLAGS", flagStr).replace("OUTPUT", outExe).replace("INPUT", "ex.c")
			rv = os.system(cmd)
			if rv == 0 and os.path.isfile(outExe):
				t0 = time.clock()
				rv = os.system(outExe)
				if rv == 0:
					t = time.clock() - t0
				else:
					t = 99999
				print outExe, t
			else:
				print cmd
				t = 9999
			self.cache[outExe] = t
		
		return t
		

def make_organism():
	opts = read_builtin()
	cdict = {}
	for optId, opt in enumerate(opts):
		gene = new.classobj("O%dC" % optId, (IntGene, ), {"randMin": 0, "randMax": len(opt) - 1})
		gene._opt = opt
		cdict["O%d" % optId] = gene
	org = new.classobj("CmdlineOrganism", (CmdlineOrganismBase, ), {"genome": cdict})
	return org

class CLPopulation(Population):
	initPopulation = 20
	childCull = 5
	childCount = 20

org = make_organism()
pop = CLPopulation(species = org)

bestC = None

try:
	while 1:
		pop.gen()
		best = pop.organisms[0]
		fit = best.fitness()
		desc = pop.organisms[0].get_flags_str()
		print "\n".join(textwrap.wrap(pop.organisms[0].get_flags_str()))
		print "%f (delta: %s)" % (fit, (fit - bestC[0] if bestC else "?"))
		if not bestC or fit < bestC[0]:
			bestC = (fit, desc)
except KeyboardInterrupt:
	print bestC