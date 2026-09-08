using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public static class CGResourceHandler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_resourceLoadersCache;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterResourceLoader_Public_Static_Void_String_ICGResourceLoader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateResource_Public_Static_Component_CGModule_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DestroyResource_Public_Static_Void_CGModule_String_Component_String_Boolean_0;

	public unsafe static Dictionary<string, ICGResourceLoader> resourceLoadersCache
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_resourceLoadersCache, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, ICGResourceLoader>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_resourceLoadersCache, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	static CGResourceHandler()
	{
		Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGResourceHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr);
		NativeFieldInfoPtr_resourceLoadersCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr, "resourceLoadersCache");
		NativeMethodInfoPtr_RegisterResourceLoader_Public_Static_Void_String_ICGResourceLoader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr, 100666111);
		NativeMethodInfoPtr_CreateResource_Public_Static_Component_CGModule_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr, 100666112);
		NativeMethodInfoPtr_DestroyResource_Public_Static_Void_CGModule_String_Component_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGResourceHandler>.NativeClassPtr, 100666113);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 50324, RefRangeEnd = 50328, XrefRangeStart = 50294, XrefRangeEnd = 50324, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RegisterResourceLoader(string resourceName, ICGResourceLoader loader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)loader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterResourceLoader_Public_Static_Void_String_ICGResourceLoader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 50354, RefRangeEnd = 50355, XrefRangeStart = 50328, XrefRangeEnd = 50354, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Component CreateResource(CGModule module, string resName, string context)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(resName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateResource_Public_Static_Component_CGModule_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Component>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 50379, RefRangeEnd = 50381, XrefRangeStart = 50355, XrefRangeEnd = 50379, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DestroyResource(CGModule module, string resName, Component obj, string context, bool kill)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(resName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &kill;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DestroyResource_Public_Static_Void_CGModule_String_Component_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGResourceHandler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
