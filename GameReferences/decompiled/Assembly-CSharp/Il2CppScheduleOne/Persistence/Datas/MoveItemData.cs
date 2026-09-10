using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class MoveItemData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TemplateItemJSON;

	private static readonly System.IntPtr NativeFieldInfoPtr_GrabbedItemQuantity;

	private static readonly System.IntPtr NativeFieldInfoPtr_SourceGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_DestinationGUID;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Guid_Guid_0;

	public unsafe string TemplateItemJSON
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemplateItemJSON);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TemplateItemJSON)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int GrabbedItemQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrabbedItemQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrabbedItemQuantity)) = num;
		}
	}

	public unsafe string SourceGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SourceGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SourceGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string DestinationGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static MoveItemData()
	{
		Il2CppClassPointerStore<MoveItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MoveItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr);
		NativeFieldInfoPtr_TemplateItemJSON = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr, "TemplateItemJSON");
		NativeFieldInfoPtr_GrabbedItemQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr, "GrabbedItemQuantity");
		NativeFieldInfoPtr_SourceGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr, "SourceGUID");
		NativeFieldInfoPtr_DestinationGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr, "DestinationGUID");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Guid_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr, 100669415);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135040, RefRangeEnd = 135041, XrefRangeStart = 135031, XrefRangeEnd = 135040, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MoveItemData(string templateItemJson, int grabbedItemQuantity, Il2CppSystem.Guid sourceGUID, Il2CppSystem.Guid destinationGUID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MoveItemData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(templateItemJson);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &grabbedItemQuantity;
		*(Il2CppSystem.Guid**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sourceGUID;
		*(Il2CppSystem.Guid**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &destinationGUID;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Guid_Guid_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MoveItemData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
