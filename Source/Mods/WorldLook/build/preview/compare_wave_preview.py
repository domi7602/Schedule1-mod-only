"""Compare paired controlled PNG captures away from the center glint.

8-bit sRGB data, fixed rectangle masks; not a perceptual or in-game test.
Usage: python compare_wave_preview.py BASELINE_IMAGES CANDIDATE_IMAGES
"""
import json
import sys
from pathlib import Path
from PIL import Image, ImageChops

base, candidate = map(Path, sys.argv[1:3])
rects = [(40, 250, 350, 520), (610, 250, 920, 520)]

def compare(name):
    left = Image.open(base / name).convert('RGB')
    right = Image.open(candidate / name).convert('RGB')
    assert left.size == right.size == (960, 540)
    values = []
    signed = [0, 0, 0]
    count = 0
    for rect in rects:
        for a, b in zip(left.crop(rect).get_flattened_data(), right.crop(rect).get_flattened_data()):
            diff = [int(y)-int(x) for x, y in zip(a, b)]
            values.append(sum(map(abs, diff))/3)
            for ch in range(3): signed[ch] += diff[ch]
            count += 1
    values.sort()
    return {'pixels': count, 'mean_abs_rgb255': sum(values)/count,
            'p95_abs_rgb255': values[int(.95*(count-1))],
            'mean_signed_rgb255': [v/count for v in signed]}

names = ['near-t0.png','near-t7p5.png','near-t31.png','far-t0.png',
         'far-t7p5.png','far-t31.png','night-near-t0.png','night-near-t7p5.png',
         'night-far-t0.png','night-far-t7p5.png',
         'shallow-near-t0.png','deep-near-t0.png']
result = {n: compare(n) for n in names}
result['controls_identical'] = {n: ImageChops.difference(Image.open(base/n).convert('RGB'), Image.open(candidate/n).convert('RGB')).getbbox() is None for n in ['near-dry.png','far-dry.png','shallow-dry.png','deep-dry.png']}
print(json.dumps(result, indent=2))
