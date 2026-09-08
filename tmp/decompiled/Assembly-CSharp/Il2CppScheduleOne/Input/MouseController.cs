using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Input;

public static class MouseController : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__IsMouseVisible_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsMouseVisible_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_IsMouseVisible_Private_Static_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LockMouse_Public_Static_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FreeMouse_Public_Static_Void_Boolean_0;

	public unsafe static bool _IsMouseVisible_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__IsMouseVisible_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__IsMouseVisible_k__BackingField, (void*)(&flag));
		}
	}

	public unsafe static bool IsMouseVisible
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166344, XrefRangeEnd = 166348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsMouseVisible_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 166348, XrefRangeEnd = 166352, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsMouseVisible_Private_Static_set_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static MouseController()
	{
		Il2CppClassPointerStore<MouseController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Input", "MouseController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MouseController>.NativeClassPtr);
		NativeFieldInfoPtr__IsMouseVisible_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MouseController>.NativeClassPtr, "<IsMouseVisible>k__BackingField");
		NativeMethodInfoPtr_get_IsMouseVisible_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MouseController>.NativeClassPtr, 100672413);
		NativeMethodInfoPtr_set_IsMouseVisible_Private_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MouseController>.NativeClassPtr, 100672414);
		NativeMethodInfoPtr_LockMouse_Public_Static_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MouseController>.NativeClassPtr, 100672415);
		NativeMethodInfoPtr_FreeMouse_Public_Static_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MouseController>.NativeClassPtr, 100672416);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 166368, RefRangeEnd = 166371, XrefRangeStart = 166352, XrefRangeEnd = 166368, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LockMouse(bool showCrosshair = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&showCrosshair);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LockMouse_Public_Static_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 166391, RefRangeEnd = 166395, XrefRangeStart = 166371, XrefRangeEnd = 166391, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FreeMouse(bool hideCrosshair = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&hideCrosshair);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FreeMouse_Public_Static_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MouseController(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
