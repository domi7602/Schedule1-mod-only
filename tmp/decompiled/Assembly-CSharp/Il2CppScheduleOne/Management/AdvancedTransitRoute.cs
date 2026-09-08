using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence.Datas;

namespace Il2CppScheduleOne.Management;

public class AdvancedTransitRoute : TransitRoute
{
	private static readonly IntPtr NativeFieldInfoPtr__Filter_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_Filter_Public_get_ManagementItemFilter_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Filter_Private_set_Void_ManagementItemFilter_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_ITransitEntity_ITransitEntity_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_AdvancedTransitRouteData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetItemReadyToMove_Public_ItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetData_Public_AdvancedTransitRouteData_0;

	public unsafe ManagementItemFilter _Filter_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Filter_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ManagementItemFilter>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Filter_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)managementItemFilter));
		}
	}

	public unsafe ManagementItemFilter Filter
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 2980, RefRangeEnd = 2987, XrefRangeStart = 2980, XrefRangeEnd = 2987, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Filter_Public_get_ManagementItemFilter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ManagementItemFilter>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Filter_Private_set_Void_ManagementItemFilter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static AdvancedTransitRoute()
	{
		Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Management", "AdvancedTransitRoute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr);
		NativeFieldInfoPtr__Filter_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, "<Filter>k__BackingField");
		NativeMethodInfoPtr_get_Filter_Public_get_ManagementItemFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670444);
		NativeMethodInfoPtr_set_Filter_Private_set_Void_ManagementItemFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670445);
		NativeMethodInfoPtr__ctor_Public_Void_ITransitEntity_ITransitEntity_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670446);
		NativeMethodInfoPtr__ctor_Public_Void_AdvancedTransitRouteData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670447);
		NativeMethodInfoPtr_GetItemReadyToMove_Public_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670448);
		NativeMethodInfoPtr_GetData_Public_AdvancedTransitRouteData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr, 100670449);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 144874, RefRangeEnd = 144876, XrefRangeStart = 144868, XrefRangeEnd = 144874, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AdvancedTransitRoute(ITransitEntity source, ITransitEntity destination)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ITransitEntity_ITransitEntity_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 144910, RefRangeEnd = 144911, XrefRangeStart = 144876, XrefRangeEnd = 144910, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AdvancedTransitRoute(AdvancedTransitRouteData data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AdvancedTransitRoute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_AdvancedTransitRouteData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 144927, RefRangeEnd = 144928, XrefRangeStart = 144911, XrefRangeEnd = 144927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemInstance GetItemReadyToMove()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetItemReadyToMove_Public_ItemInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemInstance>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 144968, RefRangeEnd = 144970, XrefRangeStart = 144928, XrefRangeEnd = 144968, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AdvancedTransitRouteData GetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetData_Public_AdvancedTransitRouteData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AdvancedTransitRouteData>(intPtr) : null;
	}

	public AdvancedTransitRoute(IntPtr pointer)
		: base(pointer)
	{
	}
}
