using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class DryingRackConfigurationData : RenamableConfigurationData
{
	private static readonly IntPtr NativeFieldInfoPtr_TargetQuality;

	private static readonly IntPtr NativeFieldInfoPtr_Destination;

	private static readonly IntPtr NativeFieldInfoPtr_StartThreshold;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_QualityFieldData_ObjectFieldData_NumberFieldData_0;

	public unsafe QualityFieldData TargetQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetQuality);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<QualityFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TargetQuality)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)qualityFieldData));
		}
	}

	public unsafe ObjectFieldData Destination
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Destination);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Destination)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectFieldData));
		}
	}

	public unsafe NumberFieldData StartThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartThreshold);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NumberFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StartThreshold)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)numberFieldData));
		}
	}

	static DryingRackConfigurationData()
	{
		Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DryingRackConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_TargetQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr, "TargetQuality");
		NativeFieldInfoPtr_Destination = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr, "Destination");
		NativeFieldInfoPtr_StartThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr, "StartThreshold");
		NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_QualityFieldData_ObjectFieldData_NumberFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr, 100669384);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134371, RefRangeEnd = 134373, XrefRangeStart = 134371, XrefRangeEnd = 134373, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DryingRackConfigurationData(StringFieldData name, QualityFieldData targetquality, ObjectFieldData destination, NumberFieldData startThreshold)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DryingRackConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)targetquality);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)startThreshold);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_QualityFieldData_ObjectFieldData_NumberFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DryingRackConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
