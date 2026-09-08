using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public static class ColliderExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsPointWithinCollider_Public_Static_Boolean_BoxCollider_Vector3_0;

	static ColliderExtensions()
	{
		Il2CppClassPointerStore<ColliderExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "ColliderExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ColliderExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_IsPointWithinCollider_Public_Static_Boolean_BoxCollider_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ColliderExtensions>.NativeClassPtr, 100674998);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 194014, RefRangeEnd = 194018, XrefRangeStart = 194008, XrefRangeEnd = 194014, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsPointWithinCollider(this BoxCollider collider, Vector3 point)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPointWithinCollider_Public_Static_Boolean_BoxCollider_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ColliderExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
