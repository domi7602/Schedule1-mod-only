using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppLiquidVolumeFX;

public static class DetailExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_allowsRefraction_Public_Static_Boolean_DETAIL_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_usesFlask_Public_Static_Boolean_DETAIL_0;

	static DetailExtensions()
	{
		Il2CppClassPointerStore<DetailExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "LiquidVolumeFX", "DetailExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DetailExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_allowsRefraction_Public_Static_Boolean_DETAIL_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DetailExtensions>.NativeClassPtr, 100664511);
		NativeMethodInfoPtr_usesFlask_Public_Static_Boolean_DETAIL_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DetailExtensions>.NativeClassPtr, 100664512);
	}

	[CallerCount(0)]
	public unsafe static bool allowsRefraction(this DETAIL detail)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&detail);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_allowsRefraction_Public_Static_Boolean_DETAIL_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool usesFlask(this DETAIL detail)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&detail);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_usesFlask_Public_Static_Boolean_DETAIL_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DetailExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
