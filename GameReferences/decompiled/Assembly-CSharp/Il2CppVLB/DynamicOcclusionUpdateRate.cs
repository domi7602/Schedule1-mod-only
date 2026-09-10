using System;
using Il2CppInterop.Common.Attributes;

namespace Il2CppVLB;

[OriginalName("Assembly-CSharp.dll", "VLB", "DynamicOcclusionUpdateRate")]
[Flags]
public enum DynamicOcclusionUpdateRate
{
	Never = 1,
	OnEnable = 2,
	OnBeamMove = 4,
	EveryXFrames = 8,
	OnBeamMoveAndEveryXFrames = OnBeamMove | EveryXFrames
}
