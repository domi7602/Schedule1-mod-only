using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppVLB;

[OriginalName("Assembly-CSharp.dll", "VLB", "ShadowUpdateRate")]
[Flags]
public enum ShadowUpdateRate
{
	Never = 1,
	OnEnable = 2,
	OnBeamMove = 4,
	EveryXFrames = 8,
	OnBeamMoveAndEveryXFrames = OnBeamMove | EveryXFrames
}
