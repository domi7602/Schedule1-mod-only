using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Management;
using Il2CppScheduleOne.Management.UI;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.UI.Management;

public class MushroomBedConfigPanel : ConfigPanel
{
	private static readonly IntPtr NativeFieldInfoPtr_SpawnUI;

	private static readonly IntPtr NativeFieldInfoPtr_Additive1UI;

	private static readonly IntPtr NativeFieldInfoPtr_Additive2UI;

	private static readonly IntPtr NativeFieldInfoPtr_Additive3UI;

	private static readonly IntPtr NativeFieldInfoPtr_DestinationUI;

	private static readonly IntPtr NativeMethodInfoPtr_BindInternal_Protected_Virtual_Void_List_1_EntityConfiguration_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ItemFieldUI SpawnUI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpawnUI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpawnUI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldUI));
		}
	}

	public unsafe ItemFieldUI Additive1UI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive1UI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive1UI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldUI));
		}
	}

	public unsafe ItemFieldUI Additive2UI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive2UI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive2UI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldUI));
		}
	}

	public unsafe ItemFieldUI Additive3UI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive3UI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemFieldUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Additive3UI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemFieldUI));
		}
	}

	public unsafe ObjectFieldUI DestinationUI
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationUI);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ObjectFieldUI>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DestinationUI)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)objectFieldUI));
		}
	}

	static MushroomBedConfigPanel()
	{
		Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Management", "MushroomBedConfigPanel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr);
		NativeFieldInfoPtr_SpawnUI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, "SpawnUI");
		NativeFieldInfoPtr_Additive1UI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, "Additive1UI");
		NativeFieldInfoPtr_Additive2UI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, "Additive2UI");
		NativeFieldInfoPtr_Additive3UI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, "Additive3UI");
		NativeFieldInfoPtr_DestinationUI = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, "DestinationUI");
		NativeMethodInfoPtr_BindInternal_Protected_Virtual_Void_List_1_EntityConfiguration_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, 100688649);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr, 100688650);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 323191, XrefRangeEnd = 323267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void BindInternal(List<EntityConfiguration> configs)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)configs);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_BindInternal_Protected_Virtual_Void_List_1_EntityConfiguration_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 34977, RefRangeEnd = 34989, XrefRangeStart = 34977, XrefRangeEnd = 34989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MushroomBedConfigPanel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MushroomBedConfigPanel>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MushroomBedConfigPanel(IntPtr pointer)
		: base(pointer)
	{
	}
}
