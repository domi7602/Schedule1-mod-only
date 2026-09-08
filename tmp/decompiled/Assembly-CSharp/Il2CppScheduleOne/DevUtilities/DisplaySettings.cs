using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct DisplaySettings
{
	[OriginalName("Assembly-CSharp.dll", "", "EDisplayMode")]
	public enum EDisplayMode
	{
		Windowed,
		FullscreenWindow,
		ExclusiveFullscreen
	}

	[ObfuscatedName("ScheduleOne.DevUtilities.DisplaySettings+<>c__DisplayClass10_0")]
	public sealed class __c__DisplayClass10_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_resolutions;

		private static readonly System.IntPtr NativeFieldInfoPtr_i;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetResolutions_b__0_Internal_Boolean_Resolution_0;

		public unsafe Il2CppStructArray<Resolution> resolutions
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resolutions);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Resolution>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resolutions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe int i
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_i);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_i)) = num;
			}
		}

		public unsafe Il2CppSystem.Predicate<Resolution> __9__0
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Predicate<Resolution>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)predicate));
			}
		}

		static __c__DisplayClass10_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "<>c__DisplayClass10_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr);
			NativeFieldInfoPtr_resolutions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "resolutions");
			NativeFieldInfoPtr_i = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "i");
			NativeFieldInfoPtr___9__0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "<>9__0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, 100675249);
			NativeMethodInfoPtr__GetResolutions_b__0_Internal_Boolean_Resolution_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, 100675250);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass10_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 197116, XrefRangeEnd = 197118, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetResolutions_b__0(Resolution x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetResolutions_b__0_Internal_Boolean_Resolution_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass10_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_ResolutionIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_DisplayMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_VSync;

	private static readonly System.IntPtr NativeFieldInfoPtr_TargetFPS;

	private static readonly System.IntPtr NativeFieldInfoPtr_UIScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_CameraBobbing;

	private static readonly System.IntPtr NativeFieldInfoPtr_ActiveDisplayIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_UnitType;

	private static readonly System.IntPtr NativeFieldInfoPtr_PauseOnFocusLost;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetResolutions_Public_Static_List_1_Resolution_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDenominatorSafe_Private_Static_UInt32_RefreshRate_0;

	[FieldOffset(0)]
	public int ResolutionIndex;

	[FieldOffset(4)]
	public EDisplayMode DisplayMode;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.U1)]
	public bool VSync;

	[FieldOffset(12)]
	public int TargetFPS;

	[FieldOffset(16)]
	public float UIScale;

	[FieldOffset(20)]
	public float CameraBobbing;

	[FieldOffset(24)]
	public int ActiveDisplayIndex;

	[FieldOffset(28)]
	public Settings.EUnitType UnitType;

	[FieldOffset(32)]
	[MarshalAs(UnmanagedType.U1)]
	public bool PauseOnFocusLost;

	static DisplaySettings()
	{
		Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "DisplaySettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr);
		NativeFieldInfoPtr_ResolutionIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "ResolutionIndex");
		NativeFieldInfoPtr_DisplayMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "DisplayMode");
		NativeFieldInfoPtr_VSync = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "VSync");
		NativeFieldInfoPtr_TargetFPS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "TargetFPS");
		NativeFieldInfoPtr_UIScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "UIScale");
		NativeFieldInfoPtr_CameraBobbing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "CameraBobbing");
		NativeFieldInfoPtr_ActiveDisplayIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "ActiveDisplayIndex");
		NativeFieldInfoPtr_UnitType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "UnitType");
		NativeFieldInfoPtr_PauseOnFocusLost = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, "PauseOnFocusLost");
		NativeMethodInfoPtr_GetResolutions_Public_Static_List_1_Resolution_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, 100675247);
		NativeMethodInfoPtr_GetDenominatorSafe_Private_Static_UInt32_RefreshRate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, 100675248);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 197149, RefRangeEnd = 197151, XrefRangeStart = 197118, XrefRangeEnd = 197149, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<Resolution> GetResolutions()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetResolutions_Public_Static_List_1_Resolution_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Resolution>>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe static uint GetDenominatorSafe(RefreshRate refreshRate)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&refreshRate);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDenominatorSafe_Private_Static_UInt32_RefreshRate_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<DisplaySettings>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
