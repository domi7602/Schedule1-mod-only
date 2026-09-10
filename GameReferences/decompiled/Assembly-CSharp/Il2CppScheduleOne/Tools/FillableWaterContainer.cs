using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

public class FillableWaterContainer : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MaxTapOpenValue;

	private static readonly IntPtr NativeFieldInfoPtr_Visuals;

	private static readonly IntPtr NativeFieldInfoPtr_FillSound;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float MaxTapOpenValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxTapOpenValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxTapOpenValue)) = num;
		}
	}

	public unsafe WaterContainerVisualizer Visuals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Visuals);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WaterContainerVisualizer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Visuals)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)waterContainerVisualizer));
		}
	}

	public unsafe AudioSourceController FillSound
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FillSound);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FillSound)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	static FillableWaterContainer()
	{
		Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "FillableWaterContainer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr);
		NativeFieldInfoPtr_MaxTapOpenValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr, "MaxTapOpenValue");
		NativeFieldInfoPtr_Visuals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr, "Visuals");
		NativeFieldInfoPtr_FillSound = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr, "FillSound");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr, 100677982);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 68082, RefRangeEnd = 68086, XrefRangeStart = 68082, XrefRangeEnd = 68086, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FillableWaterContainer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FillableWaterContainer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FillableWaterContainer(IntPtr pointer)
		: base(pointer)
	{
	}
}
