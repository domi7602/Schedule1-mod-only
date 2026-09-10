using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppEPOOutline;

[OriginalName("Assembly-CSharp-firstpass.dll", "EPOOutline", "OutlinableDrawingMode")]
[Flags]
public enum OutlinableDrawingMode
{
	Normal = 1,
	ZOnly = 2,
	GenericMask = 4,
	Obstacle = 8,
	Mask = 0x10
}
