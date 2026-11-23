#!/usr/bin/env python3
"""Validate the frozen WorldLook shader bundle against its build receipt.

Linux/macOS counterpart of Verify-ShaderBundle.ps1. Both scripts must use the
same shader input inventory, because the receipt records those hashes and is
generated once by the Unity editor build. This script itself stays outside the
inventory so the existing receipt keeps verifying.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import sys
from pathlib import Path

FIXED_INPUTS = (
    "unity/Assets/S1Mods/ReferenceWater.shader",
    "unity/Assets/S1Mods/ReferenceWater.shader.meta",
    "unity/Assets/Editor/BuildWorldLook.cs",
    "unity/Assets/Editor/BuildWorldLook.cs.meta",
    "unity/Packages/manifest.json",
    "unity/Packages/packages-lock.json",
    "unity/ProjectSettings/ProjectVersion.txt",
)

REQUIRED_PROPERTIES = (
    "_ShallowTint",
    "_DeepTint",
    "_Absorption",
    "_SurfaceOpacity",
    "_ReflectionStrength",
    "_WaveAmp",
    "_WaveScale",
    "_WaveSpeed",
    "_RippleStrength",
    "_WindActivity",
    "_SunStrength",
    "_SunSharpness",
    "_FoamColor",
    "_FoamDepth",
    "_FoamStrength",
    "_SkyColor",
    "_AmbientColor",
    "_DiagnosticMode",
)

EXPECTED_BUNDLE_ASSETS = (
    "assets/buildsettings/worldlookvariants.shadervariants",
    "assets/s1mods/referencewater.shader",
)


class VerificationError(Exception):
    """A receipt or bundle contract violation."""


def sha256_hex(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for block in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def require_file(path: Path) -> None:
    if not path.is_file():
        raise VerificationError(f"Required file is missing: {path}")


def shader_inputs(root: Path) -> list[dict[str, str]]:
    paths = [str(item) for item in FIXED_INPUTS]
    for folder, suffix in (("build", ".ps1"), ("unity/ProjectSettings", ".asset")):
        for entry in _list_inputs(root / folder, suffix):
            paths.append(entry.relative_to(root).as_posix())
    inputs: list[dict[str, str]] = []
    for relative in sorted(set(paths)):
        full = root / relative
        require_file(full)
        inputs.append({"path": relative, "sha256": sha256_hex(full)})
    return inputs


def as_int(value: object, label: str) -> int:
    if not isinstance(value, bool):
        if isinstance(value, int):
            return value
        if isinstance(value, float) and value.is_integer():
            return int(value)
        if isinstance(value, str) and value.strip().lstrip("+-").isdigit():
            return int(value)
    raise VerificationError(f"The shader receipt has an invalid {label} value.")


def _list_inputs(folder: Path, suffix: str) -> list[Path]:
    """Regular files with the given suffix, hidden names excluded like Get-ChildItem."""
    if not folder.is_dir():
        return []
    return sorted(
        entry
        for entry in folder.iterdir()
        if entry.is_file() and not entry.name.startswith(".") and entry.name.casefold().endswith(suffix)
    )


def assert_shader_bundle(root: Path, assets: Path) -> dict[str, object]:
    bundle_path = assets / "worldlook-shaders.bundle"
    receipt_path = assets / "shader-build.json"
    require_file(bundle_path)
    require_file(receipt_path)

    try:
        receipt = json.loads(receipt_path.read_text(encoding="utf-8-sig"))
    except (json.JSONDecodeError, UnicodeDecodeError) as error:
        raise VerificationError(f"The shader receipt is not valid JSON: {error}") from error
    if not isinstance(receipt, dict):
        raise VerificationError("The shader receipt must be a JSON object.")

    expected_contract = {
        "unityVersion": "2022.3.62f3",
        "platform": "StandaloneWindows64",
        "graphicsApi": "Direct3D11",
        "colorSpace": "Linear",
        "urpPackageVersion": "14.0.12",
        "corePackageVersion": "14.0.12",
    }
    contract_ok = as_int(receipt.get("schemaVersion"), "schemaVersion") == 1
    contract_ok = contract_ok and as_int(receipt.get("requestedWaterVariants"), "requestedWaterVariants") == 8
    contract_ok = contract_ok and as_int(receipt.get("loadedWaterVariants"), "loadedWaterVariants") == 8
    for key, expected in expected_contract.items():
        contract_ok = contract_ok and receipt.get(key) == expected
    if not contract_ok:
        raise VerificationError(
            "The shader receipt has an unsupported editor, pipeline, platform or variant contract. "
            "Rebuild with build/Build.ps1 -ShadersOnly."
        )

    bundle_hash = sha256_hex(bundle_path)
    bundle_bytes = bundle_path.stat().st_size
    receipt_bytes = as_int(receipt.get("bundleBytes"), "bundleBytes")
    if (receipt.get("bundleSha256") != bundle_hash or receipt_bytes != bundle_bytes or receipt_bytes < 32):
        raise VerificationError(
            "The shader bundle does not match its receipt (SHA-256/size). Rebuild with build/Build.ps1 -ShadersOnly."
        )

    bundle_assets = receipt.get("bundleAssets")
    if not isinstance(bundle_assets, list) or sorted(str(item) for item in bundle_assets) != sorted(
        EXPECTED_BUNDLE_ASSETS
    ):
        raise VerificationError("The bundle receipt must contain only ReferenceWater and its variant collection.")

    shaders = receipt.get("shaders")
    if not isinstance(shaders, list) or len(shaders) != 1:
        raise VerificationError("The bundle receipt has an invalid loaded-water shader contract.")
    shader = shaders[0]
    if (
        not isinstance(shader, dict)
        or shader.get("assetPath") != "Assets/S1Mods/ReferenceWater.shader"
        or shader.get("shaderName") != "S1Mods/ReferenceWater"
        or shader.get("editorSupported") is not True
        or as_int(shader.get("passCount"), "passCount") < 1
    ):
        raise VerificationError("The bundle receipt has an invalid loaded-water shader contract.")
    properties = shader.get("properties")
    if not isinstance(properties, list) or sorted(str(item) for item in properties) != sorted(REQUIRED_PROPERTIES):
        raise VerificationError("The loaded-water shader must expose all 18 required properties.")

    expected = shader_inputs(root)
    recorded = receipt.get("inputs")
    if not isinstance(recorded, list):
        raise VerificationError("Shader source inventory changed. Rebuild with build/Build.ps1 -ShadersOnly.")
    if len(recorded) != len(expected):
        raise VerificationError("Shader source inventory changed. Rebuild with build/Build.ps1 -ShadersOnly.")
    seen: set[str] = set()
    for entry in recorded:
        path = str(entry.get("path")) if isinstance(entry, dict) else ""
        if path in seen:
            raise VerificationError(f"Duplicate shader provenance entry: {path}")
        seen.add(path)
    by_path = {str(entry.get("path")): str(entry.get("sha256")) for entry in recorded if isinstance(entry, dict)}
    for input_file in expected:
        if by_path.get(input_file["path"]) != input_file["sha256"]:
            raise VerificationError(
                f"Shader input is missing or stale: {input_file['path']}. Rebuild with build/Build.ps1 -ShadersOnly."
            )
    return {"bundleSha256": bundle_hash, "receiptSha256": sha256_hex(receipt_path)}


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--project-root",
        default=str(Path(__file__).resolve().parents[1]),
        help="WorldLook project directory (default: parent of this build folder)",
    )
    parser.add_argument("--asset-directory", default=None, help="Directory containing the bundle and receipt")
    args = parser.parse_args(argv)

    try:
        root = Path(args.project_root).resolve()
        assets = Path(args.asset_directory).resolve() if args.asset_directory else root / "assets" / "embedded"
        verified = assert_shader_bundle(root, assets)
    except VerificationError as error:
        print(f"error: {error}", file=sys.stderr)
        return 1
    except OSError as error:
        print(f"error: {error}", file=sys.stderr)
        return 1
    print(
        "WorldLook shader bundle verified: "
        f"{verified['bundleSha256']} (8 requested/loaded variants, 18 properties)."
    )
    return 0


if __name__ == "__main__":
    sys.exit(main())
