using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ProceduralGridItemData : BuildableItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_FootprintMatches;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Int32_Il2CppReferenceArray_1_FootprintMatchData_0;

	public unsafe int Rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<FootprintMatchData> FootprintMatches
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootprintMatches);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<FootprintMatchData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootprintMatches)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ProceduralGridItemData()
	{
		Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ProceduralGridItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr);
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr, "Rotation");
		NativeFieldInfoPtr_FootprintMatches = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr, "FootprintMatches");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Int32_Il2CppReferenceArray_1_FootprintMatchData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr, 100669435);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135191, RefRangeEnd = 135192, XrefRangeStart = 135186, XrefRangeEnd = 135191, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ProceduralGridItemData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, int rotation, Il2CppReferenceArray<FootprintMatchData> footprintMatches)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ProceduralGridItemData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)footprintMatches);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_Int32_Il2CppReferenceArray_1_FootprintMatchData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ProceduralGridItemData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
