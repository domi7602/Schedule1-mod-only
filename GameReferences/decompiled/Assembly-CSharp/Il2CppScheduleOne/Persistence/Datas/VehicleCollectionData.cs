using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class VehicleCollectionData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Vehicles;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_VehicleData_0;

	public unsafe Il2CppReferenceArray<VehicleData> Vehicles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Vehicles);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<VehicleData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Vehicles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static VehicleCollectionData()
	{
		Il2CppClassPointerStore<VehicleCollectionData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "VehicleCollectionData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehicleCollectionData>.NativeClassPtr);
		NativeFieldInfoPtr_Vehicles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleCollectionData>.NativeClassPtr, "Vehicles");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_VehicleData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleCollectionData>.NativeClassPtr, 100669482);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 134314, RefRangeEnd = 134323, XrefRangeStart = 134314, XrefRangeEnd = 134323, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleCollectionData(Il2CppReferenceArray<VehicleData> vehicles)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehicleCollectionData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicles);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_VehicleData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VehicleCollectionData(IntPtr pointer)
		: base(pointer)
	{
	}
}
