using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Storage;

public class StoredItem_GenericBox : StoredItem
{
	private static readonly IntPtr NativeFieldInfoPtr_ReferenceIconWidth;

	private static readonly IntPtr NativeFieldInfoPtr_icon1;

	private static readonly IntPtr NativeFieldInfoPtr_icon2;

	private static readonly IntPtr NativeFieldInfoPtr_IconScale;

	private static readonly IntPtr NativeMethodInfoPtr_InitializeStoredItem_Public_Virtual_Void_StorableItemInstance_StorageGrid_Vector2_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float ReferenceIconWidth
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ReferenceIconWidth, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ReferenceIconWidth, (void*)(&num));
		}
	}

	public unsafe SpriteRenderer icon1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_icon1);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpriteRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_icon1)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteRenderer));
		}
	}

	public unsafe SpriteRenderer icon2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_icon2);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpriteRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_icon2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteRenderer));
		}
	}

	public unsafe float IconScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IconScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IconScale)) = num;
		}
	}

	static StoredItem_GenericBox()
	{
		Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Storage", "StoredItem_GenericBox");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr);
		NativeFieldInfoPtr_ReferenceIconWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, "ReferenceIconWidth");
		NativeFieldInfoPtr_icon1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, "icon1");
		NativeFieldInfoPtr_icon2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, "icon2");
		NativeFieldInfoPtr_IconScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, "IconScale");
		NativeMethodInfoPtr_InitializeStoredItem_Public_Virtual_Void_StorableItemInstance_StorageGrid_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, 100678847);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr, 100678848);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233386, XrefRangeEnd = 233401, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void InitializeStoredItem(StorableItemInstance _item, StorageGrid grid, Vector2 _originCoordinate, float _rotation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_item);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &_originCoordinate;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &_rotation;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_InitializeStoredItem_Public_Virtual_Void_StorableItemInstance_StorageGrid_Vector2_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 233401, XrefRangeEnd = 233402, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StoredItem_GenericBox()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StoredItem_GenericBox>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StoredItem_GenericBox(IntPtr pointer)
		: base(pointer)
	{
	}
}
