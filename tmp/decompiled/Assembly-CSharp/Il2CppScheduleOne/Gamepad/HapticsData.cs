using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Gamepad;

public class HapticsData : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_Id;

	private static readonly IntPtr NativeFieldInfoPtr_HapticMode;

	private static readonly IntPtr NativeFieldInfoPtr_Duration;

	private static readonly IntPtr NativeFieldInfoPtr_EnterCurve;

	private static readonly IntPtr NativeFieldInfoPtr_ExitCurve;

	private static readonly IntPtr NativeFieldInfoPtr_LowFrequencySettings;

	private static readonly IntPtr NativeFieldInfoPtr_HighFrequencySettings;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string Id
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EHapticMode HapticMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HapticMode);
			return *(EHapticMode*)num;
		}
		set
		{
			*(EHapticMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HapticMode)) = eHapticMode;
		}
	}

	public unsafe Vector3 Duration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Duration);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Duration)) = vector;
		}
	}

	public unsafe AnimationCurve EnterCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EnterCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_EnterCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe AnimationCurve ExitCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExitCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExitCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe HapticSettings LowFrequencySettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LowFrequencySettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<HapticSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LowFrequencySettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hapticSettings));
		}
	}

	public unsafe HapticSettings HighFrequencySettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HighFrequencySettings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<HapticSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HighFrequencySettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hapticSettings));
		}
	}

	static HapticsData()
	{
		Il2CppClassPointerStore<HapticsData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "HapticsData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HapticsData>.NativeClassPtr);
		NativeFieldInfoPtr_Id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "Id");
		NativeFieldInfoPtr_HapticMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "HapticMode");
		NativeFieldInfoPtr_Duration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "Duration");
		NativeFieldInfoPtr_EnterCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "EnterCurve");
		NativeFieldInfoPtr_ExitCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "ExitCurve");
		NativeFieldInfoPtr_LowFrequencySettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "LowFrequencySettings");
		NativeFieldInfoPtr_HighFrequencySettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, "HighFrequencySettings");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsData>.NativeClassPtr, 100685922);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294618, XrefRangeEnd = 294623, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HapticsData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HapticsData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HapticsData(IntPtr pointer)
		: base(pointer)
	{
	}
}
