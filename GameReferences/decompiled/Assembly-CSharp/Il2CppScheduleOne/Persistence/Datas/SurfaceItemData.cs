using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.ItemFramework;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class SurfaceItemData : BuildableItemData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ParentSurfaceGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_RelativePosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_RelativeRotation;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_String_Vector3_Quaternion_0;

	public unsafe string ParentSurfaceGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParentSurfaceGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParentSurfaceGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Vector3 RelativePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelativePosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelativePosition)) = vector;
		}
	}

	public unsafe Quaternion RelativeRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelativeRotation);
			return *(Quaternion*)num;
		}
		set
		{
			*(Quaternion*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelativeRotation)) = quaternion;
		}
	}

	static SurfaceItemData()
	{
		Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "SurfaceItemData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr);
		NativeFieldInfoPtr_ParentSurfaceGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr, "ParentSurfaceGUID");
		NativeFieldInfoPtr_RelativePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr, "RelativePosition");
		NativeFieldInfoPtr_RelativeRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr, "RelativeRotation");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_String_Vector3_Quaternion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr, 100669439);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135197, RefRangeEnd = 135198, XrefRangeStart = 135192, XrefRangeEnd = 135197, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SurfaceItemData(Il2CppSystem.Guid guid, ItemInstance item, int loadOrder, string parentSurfaceGUID, Vector3 pos, Quaternion rot)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SurfaceItemData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&guid);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loadOrder;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(parentSurfaceGUID);
		*(Vector3**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(Quaternion**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &rot;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_ItemInstance_Int32_String_Vector3_Quaternion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SurfaceItemData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
