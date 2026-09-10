using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppEPOOutline;

public static class RendererFilteringUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_filteredOutlinables;

	private static readonly System.IntPtr NativeMethodInfoPtr_Filter_Public_Static_Void_Camera_OutlineParameters_0;

	public unsafe static List<Outlinable> filteredOutlinables
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_filteredOutlinables, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Outlinable>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_filteredOutlinables, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static RendererFilteringUtility()
	{
		Il2CppClassPointerStore<RendererFilteringUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "RendererFilteringUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RendererFilteringUtility>.NativeClassPtr);
		NativeFieldInfoPtr_filteredOutlinables = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RendererFilteringUtility>.NativeClassPtr, "filteredOutlinables");
		NativeMethodInfoPtr_Filter_Public_Static_Void_Camera_OutlineParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RendererFilteringUtility>.NativeClassPtr, 100667445);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 64258, RefRangeEnd = 64260, XrefRangeStart = 64220, XrefRangeEnd = 64258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Filter(Camera camera, OutlineParameters parameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)camera);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Filter_Public_Static_Void_Camera_OutlineParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RendererFilteringUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
