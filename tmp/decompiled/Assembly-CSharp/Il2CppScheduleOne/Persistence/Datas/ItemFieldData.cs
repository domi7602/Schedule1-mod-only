using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ItemFieldData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ItemID;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe string ItemID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ItemID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ItemFieldData()
	{
		Il2CppClassPointerStore<ItemFieldData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ItemFieldData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ItemFieldData>.NativeClassPtr);
		NativeFieldInfoPtr_ItemID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ItemFieldData>.NativeClassPtr, "ItemID");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ItemFieldData>.NativeClassPtr, 100669385);
	}

	[CallerCount(203)]
	[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemFieldData(string itemID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ItemFieldData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(itemID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ItemFieldData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
