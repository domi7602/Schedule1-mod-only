using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Experimental;

[System.Serializable]
public class VehicleSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ForwardFriction;

	private static readonly System.IntPtr NativeFieldInfoPtr_SidewaysFriction;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_VehicleSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Blend_Public_VehicleSettings_VehicleSettings_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe WheelFrictionSettings ForwardFriction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForwardFriction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WheelFrictionSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ForwardFriction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelFrictionSettings));
		}
	}

	public unsafe WheelFrictionSettings SidewaysFriction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SidewaysFriction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WheelFrictionSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SidewaysFriction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheelFrictionSettings));
		}
	}

	static VehicleSettings()
	{
		Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Experimental", "VehicleSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr);
		NativeFieldInfoPtr_ForwardFriction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr, "ForwardFriction");
		NativeFieldInfoPtr_SidewaysFriction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr, "SidewaysFriction");
		NativeMethodInfoPtr_Clone_Public_VehicleSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr, 100685967);
		NativeMethodInfoPtr_Blend_Public_VehicleSettings_VehicleSettings_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr, 100685968);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr, 100685969);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 294997, RefRangeEnd = 294998, XrefRangeStart = 294984, XrefRangeEnd = 294997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleSettings Clone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_VehicleSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VehicleSettings>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 295006, RefRangeEnd = 295007, XrefRangeStart = 294998, XrefRangeEnd = 295006, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleSettings Blend(VehicleSettings other, float t)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Blend_Public_VehicleSettings_VehicleSettings_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VehicleSettings>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 295018, RefRangeEnd = 295021, XrefRangeStart = 295007, XrefRangeEnd = 295018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehicleSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VehicleSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
