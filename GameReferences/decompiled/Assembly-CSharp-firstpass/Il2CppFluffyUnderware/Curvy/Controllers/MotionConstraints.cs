using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

[OriginalName("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "MotionConstraints")]
[Flags]
public enum MotionConstraints
{
	None = 0,
	FreezePositionX = 1,
	FreezePositionY = 2,
	FreezePositionZ = 4,
	FreezeRotationX = 8,
	FreezeRotationY = 0x10,
	FreezeRotationZ = 0x20
}
