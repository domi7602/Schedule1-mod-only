#!/usr/bin/env python3
"""Pin an explicitly reviewed list of source files; never discover files or package binaries."""

import argparse
from pathlib import Path
import sys

from package import PackagingError, checked_file, digest, encoded_json, load_entries, require, safe_name


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--root", type=Path, required=True)
    parser.add_argument("--file-list", type=Path, required=True, help="UTF-8 text: one explicitly selected project-relative file per line")
    parser.add_argument("--output", type=Path, required=True)
    parser.add_argument("--prefix", default="LightControl")
    args = parser.parse_args()
    try:
        root = args.root.resolve(strict=True)
        prefix = safe_name(args.prefix)
        require(not args.output.exists(), "Das Zielmanifest existiert bereits. Alte Belege werden nicht überschrieben.")
        names = [line.strip() for line in args.file_list.read_text(encoding="utf-8-sig").splitlines() if line.strip() and not line.lstrip().startswith("#")]
        require(len(names) == len(set(names)), "Die ausdrückliche Quelldateiliste enthält Duplikate.")
        files = []
        for name in names:
            source = checked_file(root, name)
            require(source != args.output.resolve(), "Ein Manifest darf seine eigene Prüfsumme nicht enthalten.")
            files.append({"source": name, "archive": prefix + "/" + safe_name(name), "role": "source", "sha256": digest(source.read_bytes())})
        manifest = {"schema_version": 1, "kind": "development", "files": files}
        load_entries(root, manifest, "development")
        args.output.parent.mkdir(parents=True, exist_ok=True)
        with args.output.open("xb") as output:
            output.write(encoded_json(manifest))
        print(f"Entwicklungsmanifest: {len(files)} ausdrücklich ausgewählte Textquellen.")
        return 0
    except (OSError, ValueError, PackagingError) as error:
        print(f"MANIFEST GESPERRT: {error}", file=sys.stderr)
        return 2


if __name__ == "__main__":
    sys.exit(main())
