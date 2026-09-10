using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class WorldStorageEntityData : SaveData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_GUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_Contents;

	private static readonly System.IntPtr NativeFieldInfoPtr_LastContentChangeTime;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemSet_GameDateTime_0;

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

	public unsafe ItemSet Contents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ItemSet>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemSet));
		}
	}

	public unsafe GameDateTime LastContentChangeTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastContentChangeTime);
			return *(GameDateTime*)num;
		}
		set
		{
			*(GameDateTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastContentChangeTime)) = gameDateTime;
		}
	}

	static WorldStorageEntityData()
	{
		Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "WorldStorageEntityData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr);
		NativeFieldInfoPtr_GUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr, "GUID");
		NativeFieldInfoPtr_Contents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr, "Contents");
		NativeFieldInfoPtr_LastContentChangeTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr, "LastContentChangeTime");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemSet_GameDateTime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr, 100669486);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135558, RefRangeEnd = 135559, XrefRangeStart = 135554, XrefRangeEnd = 135558, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WorldStorageEntityData(Il2CppSystem.Guid guid, ItemSet contents, GameDateTime lastContentChangeTime)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WorldStorageEntityData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contents);
		*(GameDateTime**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &lastContentChangeTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemSet_GameDateTime_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WorldStorageEntityData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
