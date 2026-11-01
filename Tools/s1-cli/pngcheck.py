"""Bounded, dependency-free decoding of Unity's RGB/RGBA 8-bit PNG evidence."""
import hashlib
from pathlib import Path
import struct
import zlib

MAX_BYTES = 64 * 1024 * 1024


def verify_png(result, game_dir, remaining):
    if (type(result.get('schema_version')) is not int or result['schema_version'] != 1
            or result.get('success') is not True or result.get('includes_ui') is not True
            or result.get('capture_source') != 'Unity.ScreenCapture/end-of-frame'):
        raise ValueError('Unsupported screenshot result schema/source')
    for key in ('width', 'height', 'bytes'):
        if type(result.get(key)) is not int or result[key] <= 0:
            raise ValueError('Invalid screenshot dimensions/size')
    width, height = result['width'], result['height']
    if max(width, height) > 8192 or width * height > 16777216 or result['bytes'] > MAX_BYTES:
        raise ValueError('Screenshot exceeds size budget')
    if not isinstance(result.get('path'), str):
        raise ValueError('Missing screenshot path')
    path = Path(result['path'])
    if not path.is_absolute() or str(path).startswith(('\\\\', '//')):
        raise ValueError('Screenshot path must be local and absolute')
    root = Path(game_dir).absolute() / 'UserData/S1MCP/Evidence/Screenshots'
    if path.parent.resolve() != root.resolve() or path.suffix.lower() != '.png':
        raise ValueError('Screenshot is outside the bridge evidence directory')
    for ancestor in (path, *path.parents):
        if ancestor.is_symlink() or (hasattr(ancestor, 'is_junction') and ancestor.is_junction()):
            raise ValueError('Screenshot reparse paths are refused')
    remaining()
    with path.open('rb') as stream:
        content = stream.read(MAX_BYTES + 1)
    if len(content) != result['bytes'] or not content.startswith(b'\x89PNG\r\n\x1a\n'):
        raise ValueError('PNG signature or reported byte count mismatch')
    offset, channels, ended = 8, None, False
    compressed = bytearray()
    seen_idat = False
    while offset < len(content):
        remaining()
        if offset + 12 > len(content):
            raise ValueError('Truncated PNG chunk')
        size = struct.unpack_from('>I', content, offset)[0]
        kind = content[offset + 4:offset + 8]
        end = offset + size + 12
        if end > len(content):
            raise ValueError('Truncated PNG data')
        value = content[offset + 8:end - 4]
        if zlib.crc32(kind + value) != struct.unpack_from('>I', content, end - 4)[0]:
            raise ValueError('PNG CRC mismatch')
        if offset == 8 and kind != b'IHDR':
            raise ValueError('PNG must start with IHDR')
        if kind == b'IHDR':
            if channels is not None or size != 13:
                raise ValueError('Invalid PNG header')
            w, h, depth, color, compression, filtering, interlace = struct.unpack('>IIBBBBB', value)
            if (w, h) != (width, height) or depth != 8 or color not in (2, 6) or (compression, filtering, interlace) != (0, 0, 0):
                raise ValueError('PNG dimensions or format differ from supported Unity RGB/RGBA8 output')
            channels = 3 if color == 2 else 4
        elif kind == b'IDAT':
            seen_idat = True
            compressed.extend(value)
        elif kind == b'IEND':
            if size or not seen_idat or end != len(content):
                raise ValueError('Invalid PNG ending')
            ended = True
        elif not kind[0] & 32 and kind != b'PLTE':
            raise ValueError('Unsupported critical PNG chunk')
        offset = end
    if not ended or channels is None:
        raise ValueError('Incomplete PNG')
    stride = width * channels
    expected = (stride + 1) * height
    decoder = zlib.decompressobj()
    raw = decoder.decompress(compressed, expected + 1)
    if len(raw) != expected or not decoder.eof or decoder.unused_data or decoder.unconsumed_tail:
        raise ValueError('PNG pixel stream is incomplete or excessive')
    previous = bytearray(stride)
    pixels = hashlib.sha256()
    for row in range(height):
        remaining()
        start = row * (stride + 1)
        mode = raw[start]
        current = bytearray(raw[start + 1:start + 1 + stride])
        if mode not in range(5):
            raise ValueError('Invalid PNG row filter')
        if mode:
            for i in range(stride):
                left = current[i - channels] if i >= channels else 0
                up = previous[i]
                upper_left = previous[i - channels] if i >= channels else 0
                if mode == 1:
                    predictor = left
                elif mode == 2:
                    predictor = up
                elif mode == 3:
                    predictor = (left + up) // 2
                else:
                    p = left + up - upper_left
                    distances = (abs(p - left), abs(p - up), abs(p - upper_left))
                    predictor = (left, up, upper_left)[distances.index(min(distances))]
                current[i] = (current[i] + predictor) & 255
        pixels.update(current)
        previous = current
    remaining()
    return dict(result, png_verified=True, sha256=hashlib.sha256(content).hexdigest(),
                decoded_pixels_sha256=pixels.hexdigest(), decoded_channels=channels)
