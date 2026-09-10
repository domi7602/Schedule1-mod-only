using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.UI.Input;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Building;

public class BuildStart_Grid : BuildStart_Base
{
	private static readonly IntPtr NativeFieldInfoPtr_GhostModelScale;

	private static readonly IntPtr NativeFieldInfoPtr_ghostModelClass;

	private static readonly IntPtr NativeMethodInfoPtr_StartBuilding_Public_Virtual_Void_ItemInstance_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetInputPromptsModules_Protected_Virtual_List_1_InputPromptsData_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateGhostModel_Protected_Virtual_New_GridItem_BuildableItemDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float GhostModelScale
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GhostModelScale, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GhostModelScale, (void*)(&num));
		}
	}

	public unsafe GridItem ghostModelClass
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ghostModelClass);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GridItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ghostModelClass)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gridItem));
		}
	}

	static BuildStart_Grid()
	{
		Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Building", "BuildStart_Grid");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr);
		NativeFieldInfoPtr_GhostModelScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, "GhostModelScale");
		NativeFieldInfoPtr_ghostModelClass = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, "ghostModelClass");
		NativeMethodInfoPtr_StartBuilding_Public_Virtual_Void_ItemInstance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, 100676586);
		NativeMethodInfoPtr_GetInputPromptsModules_Protected_Virtual_List_1_InputPromptsData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, 100676587);
		NativeMethodInfoPtr_CreateGhostModel_Protected_Virtual_New_GridItem_BuildableItemDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, 100676588);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr, 100676589);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 211825, RefRangeEnd = 211826, XrefRangeStart = 211803, XrefRangeEnd = 211825, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void StartBuilding(ItemInstance itemInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemInstance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StartBuilding_Public_Virtual_Void_ItemInstance_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 211826, XrefRangeEnd = 211848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override List<InputPromptsData> GetInputPromptsModules()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetInputPromptsModules_Protected_Virtual_List_1_InputPromptsData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<InputPromptsData>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 211848, XrefRangeEnd = 211913, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual GridItem CreateGhostModel(BuildableItemDefinition itemDefinition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemDefinition);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CreateGhostModel_Protected_Virtual_New_GridItem_BuildableItemDefinition_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GridItem>(intPtr) : null;
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BuildStart_Grid()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BuildStart_Grid>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BuildStart_Grid(IntPtr pointer)
		: base(pointer)
	{
	}
}
