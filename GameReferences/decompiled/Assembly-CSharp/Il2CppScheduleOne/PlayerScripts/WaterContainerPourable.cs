using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerTasks;
using Il2CppScheduleOne.Tools;

namespace Il2CppScheduleOne.PlayerScripts;

public class WaterContainerPourable : Pourable
{
	private static readonly IntPtr NativeFieldInfoPtr__visuals;

	private static readonly IntPtr NativeFieldInfoPtr__waterContainerItem;

	private static readonly IntPtr NativeMethodInfoPtr_SetupWaterContainerPourable_Public_Void_WaterContainerInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_PourAmount_Protected_Virtual_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe WaterContainerVisualizer _visuals
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__visuals);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WaterContainerVisualizer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__visuals)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)waterContainerVisualizer));
		}
	}

	public unsafe WaterContainerInstance _waterContainerItem
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waterContainerItem);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WaterContainerInstance>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waterContainerItem)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)waterContainerInstance));
		}
	}

	static WaterContainerPourable()
	{
		Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerScripts", "WaterContainerPourable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr);
		NativeFieldInfoPtr__visuals = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, "_visuals");
		NativeFieldInfoPtr__waterContainerItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, "_waterContainerItem");
		NativeMethodInfoPtr_SetupWaterContainerPourable_Public_Void_WaterContainerInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, 100672265);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, 100672266);
		NativeMethodInfoPtr_PourAmount_Protected_Virtual_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, 100672267);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr, 100672268);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 164743, RefRangeEnd = 164744, XrefRangeStart = 164735, XrefRangeEnd = 164743, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetupWaterContainerPourable(WaterContainerInstance waterContainer)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)waterContainer);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupWaterContainerPourable_Public_Void_WaterContainerInstance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164744, XrefRangeEnd = 164750, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164750, XrefRangeEnd = 164755, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void PourAmount(float amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&amount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_PourAmount_Protected_Virtual_Void_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164755, XrefRangeEnd = 164756, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WaterContainerPourable()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WaterContainerPourable>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WaterContainerPourable(IntPtr pointer)
		: base(pointer)
	{
	}
}
