using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Graffiti;

public static class ESprayColorExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetColor_Public_Static_Color_ESprayColor_0;

	static ESprayColorExtensions()
	{
		Il2CppClassPointerStore<ESprayColorExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Graffiti", "ESprayColorExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ESprayColorExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetColor_Public_Static_Color_ESprayColor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ESprayColorExtensions>.NativeClassPtr, 100672733);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 169164, RefRangeEnd = 169165, XrefRangeStart = 169164, XrefRangeEnd = 169164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Color GetColor(this ESprayColor color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&color);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetColor_Public_Static_Color_ESprayColor_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ESprayColorExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
