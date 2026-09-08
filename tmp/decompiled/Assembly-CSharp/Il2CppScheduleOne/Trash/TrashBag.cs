using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Datas;

namespace Il2CppScheduleOne.Trash;

public class TrashBag : TrashItem
{
	private static readonly IntPtr NativeFieldInfoPtr__Content_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_Content_Public_get_TrashContent_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Content_Private_set_Void_TrashContent_0;

	private static readonly IntPtr NativeMethodInfoPtr_LoadContent_Public_Void_TrashContentData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetData_Public_Virtual_TrashItemData_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe TrashContent _Content_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Content_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashContent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Content_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trashContent));
		}
	}

	public unsafe TrashContent Content
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38414, RefRangeEnd = 38415, XrefRangeStart = 38414, XrefRangeEnd = 38415, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Content_Public_get_TrashContent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashContent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Content_Private_set_Void_TrashContent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static TrashBag()
	{
		Il2CppClassPointerStore<TrashBag>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Trash", "TrashBag");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrashBag>.NativeClassPtr);
		NativeFieldInfoPtr__Content_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, "<Content>k__BackingField");
		NativeMethodInfoPtr_get_Content_Public_get_TrashContent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, 100676971);
		NativeMethodInfoPtr_set_Content_Private_set_Void_TrashContent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, 100676972);
		NativeMethodInfoPtr_LoadContent_Public_Void_TrashContentData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, 100676973);
		NativeMethodInfoPtr_GetData_Public_Virtual_TrashItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, 100676974);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrashBag>.NativeClassPtr, 100676975);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 216424, RefRangeEnd = 216425, XrefRangeStart = 216422, XrefRangeEnd = 216424, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadContent(TrashContentData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadContent_Public_Void_TrashContentData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216425, XrefRangeEnd = 216435, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override TrashItemData GetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetData_Public_Virtual_TrashItemData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TrashItemData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 216435, XrefRangeEnd = 216459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrashBag()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrashBag>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrashBag(IntPtr pointer)
		: base(pointer)
	{
	}
}
