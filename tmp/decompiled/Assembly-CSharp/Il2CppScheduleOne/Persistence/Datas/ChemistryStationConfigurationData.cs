using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class ChemistryStationConfigurationData : RenamableConfigurationData
{
	private static readonly IntPtr NativeFieldInfoPtr_Recipe;

	private static readonly IntPtr NativeFieldInfoPtr_Destination;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_StationRecipeFieldData_ObjectFieldData_0;

	public unsafe StationRecipeFieldData Recipe
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Recipe);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StationRecipeFieldData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Recipe)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stationRecipeFieldData));
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

	static ChemistryStationConfigurationData()
	{
		Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ChemistryStationConfigurationData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr);
		NativeFieldInfoPtr_Recipe = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr, "Recipe");
		NativeFieldInfoPtr_Destination = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr, "Destination");
		NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_StationRecipeFieldData_ObjectFieldData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr, 100669382);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 134792, RefRangeEnd = 134797, XrefRangeStart = 134792, XrefRangeEnd = 134797, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ChemistryStationConfigurationData(StringFieldData name, StationRecipeFieldData recipe, ObjectFieldData destination)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ChemistryStationConfigurationData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)recipe);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_StringFieldData_StationRecipeFieldData_ObjectFieldData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ChemistryStationConfigurationData(IntPtr pointer)
		: base(pointer)
	{
	}
}
