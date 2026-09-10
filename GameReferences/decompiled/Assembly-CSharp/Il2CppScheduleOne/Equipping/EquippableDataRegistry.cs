using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppScheduleOne.DevUtilities;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Equipping;

public class EquippableDataRegistry : PersistentSingleton<EquippableDataRegistry>
{
	private static readonly System.IntPtr NativeFieldInfoPtr__equippableDataList;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEquippableData_Public_EquippableData_Guid_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterEquippableData_Private_Void_EquippableData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<EquippableData> _equippableDataList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__equippableDataList);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<EquippableData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__equippableDataList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static EquippableDataRegistry()
	{
		Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping", "EquippableDataRegistry");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr);
		NativeFieldInfoPtr__equippableDataList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr, "_equippableDataList");
		NativeMethodInfoPtr_GetEquippableData_Public_EquippableData_Guid_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr, 100679879);
		NativeMethodInfoPtr_RegisterEquippableData_Private_Void_EquippableData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr, 100679880);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr, 100679881);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 244722, RefRangeEnd = 244723, XrefRangeStart = 244708, XrefRangeEnd = 244722, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EquippableData GetEquippableData(Il2CppSystem.Guid guid)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&guid);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEquippableData_Public_EquippableData_Guid_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EquippableData>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 244723, XrefRangeEnd = 244735, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RegisterEquippableData(EquippableData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterEquippableData_Private_Void_EquippableData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 244735, XrefRangeEnd = 244745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EquippableDataRegistry()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EquippableDataRegistry>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EquippableDataRegistry(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
