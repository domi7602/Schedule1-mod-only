using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class BuildableItemData : SaveData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_GUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_ItemString;

	private static readonly System.IntPtr NativeFieldInfoPtr_LoadOrder;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_0;

	public unsafe string GUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ItemString
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemString);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemString)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int LoadOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LoadOrder);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LoadOrder)) = num;
		}
	}

	static BuildableItemData()
	{
		Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "BuildableItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr);
		NativeFieldInfoPtr_GUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr, "GUID");
		NativeFieldInfoPtr_ItemString = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr, "ItemString");
		NativeFieldInfoPtr_LoadOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr, "LoadOrder");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr, 100669420);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135071, RefRangeEnd = 135072, XrefRangeStart = 135067, XrefRangeEnd = 135071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BuildableItemData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BuildableItemData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BuildableItemData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
