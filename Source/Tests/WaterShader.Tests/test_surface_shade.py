"""Evaluate the actual scalar HLSL shade function, not a copy of its formula.

This small expression interpreter extracts the production function and executes its
statements with HLSL-compatible scalar/vector intrinsics and controlled derivatives.
It does not compile HLSL or test GPU derivatives; the Unity preview does that.
"""
import math
import pathlib
import re
import unittest

SHADER = pathlib.Path(__file__).resolve().parents[2] / 'Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader'


def clamp(x, lo, hi): return min(hi, max(lo, x))
def smoothstep(a, b, x):
    u = clamp((x-a)/(b-a), 0., 1.)
    return u*u*(3.-2.*u)
def dot(a, b): return sum(x*y for x, y in zip(a, b))


def load_program():
    source = SHADER.read_text(encoding='utf-8')
    match = re.search(r'float\s+WaterSurfaceShade\s*\(.*?\)\s*\{(.*?)\n\s*\}', source, re.S)
    if not match: raise ValueError('production WaterSurfaceShade function missing')
    body = re.sub(r'//[^\n]*', '', match.group(1))
    program = []
    for statement in body.split(';'):
        statement = statement.strip()
        if not statement: continue
        if statement.startswith('return '):
            program.append(('return', compile(statement[7:], '<shader return>', 'eval')))
        else:
            m = re.fullmatch(r'float\s+(\w+)\s*=\s*(.*)', statement, re.S)
            if not m: raise ValueError('Unsupported HLSL statement: ' + statement)
            program.append((m.group(1), compile(' '.join(m.group(2).split()), '<shader expression>', 'eval')))
    return program

PROGRAM = load_program()

def shade(x, z, t, footprint, amp=.15, ripple=.28, wind=.25, speed=1.3):
    env = dict(xz=(x,z), t=t, _WaveAmp=amp, _RippleStrength=ripple,
               _WindActivity=wind, _WaveSpeed=speed, float2=lambda x,y:(x,y),
               dot=dot, sqrt=math.sqrt, sin=math.sin, cos=math.cos,
               smoothstep=smoothstep, clamp=clamp, saturate=lambda x:clamp(x,0.,1.),
               ddx=lambda _: (footprint,0.), ddy=lambda _: (0.,0.))
    for name, expression in PROGRAM:
        value = eval(expression, {'__builtins__': {}}, env)
        if name == 'return': return value
        env[name] = value
    raise ValueError('missing return')

class SurfaceShadeTests(unittest.TestCase):
    def test_mean_does_not_drift_with_filtering(self):
        # Large regularly sampled patch in rotated coordinates, averaged across
        # several times. LOD footprint is constant per measurement.
        means = []
        for fp in (0., .25, .7, 2.):
            values = []
            for t in (0., 7.5, 31.):
                for i in range(160):
                    along = (i + .5) * 1.25
                    for j in range(24):
                        across = (j + .5) * 1.7
                        values.append(shade(.8*along-.6*across, .6*along+.8*across, t, fp))
            means.append(sum(values)/len(values))
        self.assertLess(max(abs(m-1.) for m in means), .015, str(means))
        self.assertLess(max(means)-min(means), .015, str(means))

    def test_bounds_visibility_and_full_filter(self):
        values = [shade(i*.071, j*.13, 7.5, 0., amp=.6, ripple=2., wind=1.)
                  for i in range(180) for j in range(20)]
        self.assertGreaterEqual(min(values), .4 - 1e-6)
        self.assertLessEqual(max(values), 1.6 + 1e-6)
        self.assertGreater(max(values)-min(values), .7)
        self.assertTrue(all(shade(i*.1, 1., 7.5, 3.) == 1. for i in range(80)))
        self.assertTrue(all(shade(i*.1, 1., 7.5, 0., amp=0.) == 1. for i in range(80)))

    def test_motion_wind_and_local_continuity(self):
        positions = [(i*.19, j*.23) for i in range(30) for j in range(12)]
        frame0 = [shade(x,z,10.,0.) for x,z in positions]
        frame1 = [shade(x,z,17.5,0.) for x,z in positions]
        self.assertGreater(sum(abs(a-b) for a,b in zip(frame0,frame1))/len(frame0), .05)
        for x,z in positions:
            reference = shade(x,z,10000.,.31,wind=.5)
            self.assertLess(abs(reference-shade(x+.0001,z,10000.,.31,wind=.5)), .001)
            self.assertLess(abs(reference-shade(x,z,10000.0001,.31,wind=.5)), .001)
            self.assertLess(abs(reference-shade(x,z,10000.,.3101,wind=.5)), .001)
            self.assertLess(abs(reference-shade(x,z,10000.,.31,wind=.5001)), .001)
        a = [abs(shade(x,z,10.,0.,wind=0.)-1.) for x,z in positions]
        b = [abs(shade(x,z,10.,0.,wind=1.)-1.) for x,z in positions]
        self.assertGreater(sum(b), sum(a)*1.15)

if __name__ == '__main__': unittest.main(verbosity=2)
