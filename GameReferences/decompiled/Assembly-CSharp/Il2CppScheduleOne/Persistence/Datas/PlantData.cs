using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class PlantData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_SeedID;

	private static readonly IntPtr NativeFieldInfoPtr_GrowthProgress;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveBuds;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Single_Il2CppStructArray_1_Int32_0;

	public unsafe string SeedID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SeedID);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SeedID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float GrowthProgress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowthProgress);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowthProgress)) = num;
		}
	}

	public unsafe Il2CppStructArray<int> ActiveBuds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveBuds);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveBuds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static PlantData()
	{
		Il2CppClassPointerStore<PlantData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "PlantData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlantData>.NativeClassPtr);
		NativeFieldInfoPtr_SeedID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantData>.NativeClassPtr, "SeedID");
		NativeFieldInfoPtr_GrowthProgress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantData>.NativeClassPtr, "GrowthProgress");
		NativeFieldInfoPtr_ActiveBuds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantData>.NativeClassPtr, "ActiveBuds");
		NativeMethodInfoPtr__ctor_Public_Void_String_Single_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantData>.NativeClassPtr, 100669444);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135216, RefRangeEnd = 135217, XrefRangeStart = 135213, XrefRangeEnd = 135216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlantData(string seedID, float growthProgress, Il2CppStructArray<int> activeBuds)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlantData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(seedID);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &growthProgress;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeBuds);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Single_Il2CppStructArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlantData(IntPtr pointer)
		: base(pointer)
	{
	}
}
