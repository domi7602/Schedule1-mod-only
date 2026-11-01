"""Generated PNG fixtures validate decoding, not actual Unity capture."""
import hashlib
import importlib.util
import os
from pathlib import Path
import struct
import tempfile
import unittest
import zlib

ROOT = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location('pngcheck', ROOT / 'pngcheck.py')
assert spec is not None and spec.loader is not None
pngcheck = importlib.util.module_from_spec(spec)
spec.loader.exec_module(pngcheck)


def chunk(kind, value):
    return struct.pack('>I', len(value)) + kind + value + struct.pack('>I', zlib.crc32(kind + value))


class PngTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory(dir=os.environ.get('TMPDIR'))
        self.addCleanup(self.temp.cleanup)
        self.game = Path(self.temp.name)
        self.path = self.game / 'UserData/S1MCP/Evidence/Screenshots/capture-fixture.png'
        self.path.parent.mkdir(parents=True)

    def make(self, raw=b'\0\xff\0\0\xff', compressed=None):
        encoded = zlib.compress(raw) if compressed is None else compressed
        content = b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>IIBBBBB', 1, 1, 8, 6, 0, 0, 0)) + chunk(b'IDAT', encoded) + chunk(b'IEND', b'')
        self.path.write_bytes(content)
        return dict(schema_version=1, success=True, includes_ui=True, capture_source='Unity.ScreenCapture/end-of-frame',
                    width=1, height=1, bytes=len(content), path=str(self.path.resolve()))

    def test_all_filters_are_decoded(self):
        # First pixel's left/up/upper-left are zero, for every PNG filter.
        for mode in range(5):
            with self.subTest(mode=mode):
                result = pngcheck.verify_png(self.make(bytes([mode, 255, 0, 0, 255])), self.game, lambda: 10)
                self.assertEqual(hashlib.sha256(bytes([255, 0, 0, 255])).hexdigest(), result['decoded_pixels_sha256'])

    def test_bad_row_filter(self):
        with self.assertRaises(ValueError):
            pngcheck.verify_png(self.make(bytes([5, 255, 0, 0, 255])), self.game, lambda: 10)

    def test_truncated_and_excessive_pixels(self):
        for raw in (b'\0', b'\0' * 100000):
            with self.subTest(size=len(raw)), self.assertRaises(ValueError):
                pngcheck.verify_png(self.make(raw), self.game, lambda: 10)

    def test_bad_zlib(self):
        with self.assertRaises(zlib.error):
            pngcheck.verify_png(self.make(compressed=b'bad'), self.game, lambda: 10)

    def test_path_escape_and_mismatched_metadata(self):
        original = self.make()
        for field, value in [('path', str(self.game / 'outside.png')), ('width', 2), ('bytes', 8), ('width', True), ('includes_ui', False)]:
            with self.subTest(field=field), self.assertRaises(ValueError):
                pngcheck.verify_png(dict(original, **{field: value}), self.game, lambda: 10)

    def test_deadline_is_checked(self):
        def expired():
            raise TimeoutError('test deadline')
        with self.assertRaises(TimeoutError):
            pngcheck.verify_png(self.make(), self.game, expired)


if __name__ == '__main__':
    unittest.main()
