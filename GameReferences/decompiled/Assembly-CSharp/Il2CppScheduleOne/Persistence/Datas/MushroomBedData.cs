using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Tiles;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

public class MushroomBedData : GrowContainerData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ShroomColonyData;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_String_Single_Int32_Single_Il2CppStringArray_ShroomColonyData_0;

	public unsafe ShroomColonyData ShroomColonyData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShroomColonyData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ShroomColonyData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShroomColonyData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomColonyData));
		}
	}

	static MushroomBedData()
	{
		Il2CppClassPointerStore<MushroomBedData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MushroomBedData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MushroomBedData>.NativeClassPtr);
		NativeFieldInfoPtr_ShroomColonyData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedData>.NativeClassPtr, "ShroomColonyData");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_String_Single_Int32_Single_Il2CppStringArray_ShroomColonyData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MushroomBedData>.NativeClassPtr, 100669431);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 135184, RefRangeEnd = 135186, XrefRangeStart = 135182, XrefRangeEnd = 135184, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MushroomBedData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, string soilID, float soilLevel, int remainingSoilUses, float waterLevel, Il2CppStringArray appliedAdditives, ShroomColonyData colonyData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MushroomBedData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[12];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &originCoordinate;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(soilID);
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &soilLevel;
		*(int**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &remainingSoilUses;
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &waterLevel;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)appliedAdditives);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colonyData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Grid_Vector2_Int32_String_Single_Int32_Single_Il2CppStringArray_ShroomColonyData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MushroomBedData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
