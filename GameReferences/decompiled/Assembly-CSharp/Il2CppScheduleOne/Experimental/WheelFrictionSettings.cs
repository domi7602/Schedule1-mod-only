using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Experimental;

[System.Serializable]
public class WheelFrictionSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ExtremumSlip;

	private static readonly System.IntPtr NativeFieldInfoPtr_ExtremumValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_AsymptoteSlip;

	private static readonly System.IntPtr NativeFieldInfoPtr_AsymptoteValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_Stiffness;

	private static readonly System.IntPtr NativeMethodInfoPtr_Blend_Public_WheelFrictionSettings_WheelFrictionSettings_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float ExtremumSlip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExtremumSlip);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExtremumSlip)) = num;
		}
	}

	public unsafe float ExtremumValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExtremumValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExtremumValue)) = num;
		}
	}

	public unsafe float AsymptoteSlip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AsymptoteSlip);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AsymptoteSlip)) = num;
		}
	}

	public unsafe float AsymptoteValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AsymptoteValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AsymptoteValue)) = num;
		}
	}

	public unsafe float Stiffness
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Stiffness);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Stiffness)) = num;
		}
	}

	static WheelFrictionSettings()
	{
		Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Experimental", "WheelFrictionSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr);
		NativeFieldInfoPtr_ExtremumSlip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, "ExtremumSlip");
		NativeFieldInfoPtr_ExtremumValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, "ExtremumValue");
		NativeFieldInfoPtr_AsymptoteSlip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, "AsymptoteSlip");
		NativeFieldInfoPtr_AsymptoteValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, "AsymptoteValue");
		NativeFieldInfoPtr_Stiffness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, "Stiffness");
		NativeMethodInfoPtr_Blend_Public_WheelFrictionSettings_WheelFrictionSettings_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, 100685970);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr, 100685971);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 295034, RefRangeEnd = 295036, XrefRangeStart = 295021, XrefRangeEnd = 295034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WheelFrictionSettings Blend(WheelFrictionSettings other, float t)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Blend_Public_WheelFrictionSettings_WheelFrictionSettings_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<WheelFrictionSettings>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WheelFrictionSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WheelFrictionSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WheelFrictionSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
