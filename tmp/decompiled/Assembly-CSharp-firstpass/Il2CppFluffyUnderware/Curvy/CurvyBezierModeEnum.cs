using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppFluffyUnderware.Curvy;

[OriginalName("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyBezierModeEnum")]
[Flags]
public enum CurvyBezierModeEnum
{
	None = 0,
	Direction = 1,
	Length = 2,
	Connections = 4,
	Combine = 8
}
