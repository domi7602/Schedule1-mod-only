using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Experimental;

public class WheelOverrideData : ScriptableObject
{
	[OriginalName("Assembly-CSharp.dll", "", "OverrideCategory")]
	[Flags]
	public enum OverrideCategory
	{
		None = 0,
		Wheels = 1
	}

	[OriginalName("Assembly-CSharp.dll", "", "WheelOverrides")]
	[Flags]
	public enum WheelOverrides
	{
		None = 0,
		ForwardFriction = 1,
		SidewaysFriction = 2
	}

	[OriginalName("Assembly-CSharp.dll", "", "WheelFrictionOverrides")]
	[Flags]
	public enum WheelFrictionOverrides
	{
		None = 0,
		ExtremumSlip = 1,
		ExtremumValue = 2,
		AsymptoteSlip = 4,
		AsymptoteValue = 8,
		Stiffness = 0x10
	}

	private static readonly IntPtr NativeFieldInfoPtr_Settings;

	private static readonly IntPtr NativeFieldInfoPtr_Categories;

	private static readonly IntPtr NativeFieldInfoPtr_WheelFlags;

	private static readonly IntPtr NativeFieldInfoPtr_ForwardFrictionFlags;

	private static readonly IntPtr NativeFieldInfoPtr_SidewaysFrictionFlags;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe VehicleSettings Settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VehicleSettings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleSettings));
		}
	}

	public unsafe OverrideCategory Categories
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Categories);
			return *(OverrideCategory*)num;
		}
		set
		{
			*(OverrideCategory*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Categories)) = overrideCategory;
		}
	}

	public unsafe WheelOverrides WheelFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WheelFlags);
			return *(WheelOverrides*)num;
		}
		set
		{
			*(WheelOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WheelFlags)) = wheelOverrides;
		}
	}

	public unsafe WheelFrictionOverrides ForwardFrictionFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForwardFrictionFlags);
			return *(WheelFrictionOverrides*)num;
		}
		set
		{
			*(WheelFrictionOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForwardFrictionFlags)) = wheelFrictionOverrides;
		}
	}

	public unsafe WheelFrictionOverrides SidewaysFrictionFlags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SidewaysFrictionFlags);
			return *(WheelFrictionOverrides*)num;
		}
		set
		{
			*(WheelFrictionOverrides*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SidewaysFrictionFlags)) = wheelFrictionOverrides;
		}
	}

	static WheelOverrideData()
	{
		Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Experimental", "WheelOverrideData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr);
		NativeFieldInfoPtr_Settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, "Settings");
		NativeFieldInfoPtr_Categories = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, "Categories");
		NativeFieldInfoPtr_WheelFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, "WheelFlags");
		NativeFieldInfoPtr_ForwardFrictionFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, "ForwardFrictionFlags");
		NativeFieldInfoPtr_SidewaysFrictionFlags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, "SidewaysFrictionFlags");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr, 100685972);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WheelOverrideData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WheelOverrideData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WheelOverrideData(IntPtr pointer)
		: base(pointer)
	{
	}
}
