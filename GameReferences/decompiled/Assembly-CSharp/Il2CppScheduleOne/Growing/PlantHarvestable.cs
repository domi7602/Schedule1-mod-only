using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Gamepad;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace Il2CppScheduleOne.Growing;

public class PlantHarvestable : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_Product;

	private static readonly IntPtr NativeFieldInfoPtr_ProductQuantity;

	private static readonly IntPtr NativeFieldInfoPtr__gamepadLure;

	private static readonly IntPtr NativeFieldInfoPtr__gamepadLureLocationOverride;

	private static readonly IntPtr NativeFieldInfoPtr__isLureActive;

	private static readonly IntPtr NativeFieldInfoPtr__ScheduleOne_Gamepad_IGamepadPointerLure_Offset_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_GamepadLure_Public_get_IGamepadPointerLure_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_RegisterDefaultLureWhenEmpty_Public_Virtual_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Data_Private_Virtual_Final_New_get_GamepadPointerLureData_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_IsActive_Private_Virtual_Final_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Position_Private_Virtual_Final_New_get_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Offset_Private_Virtual_Final_New_get_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Public_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Harvest_Public_Virtual_New_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_SetActive_Private_Virtual_Final_New_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe StorableItemDefinition Product
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StorableItemDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)storableItemDefinition));
		}
	}

	public unsafe int ProductQuantity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuantity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ProductQuantity)) = num;
		}
	}

	public unsafe GamepadPointerLureData _gamepadLure
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gamepadLure);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GamepadPointerLureData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gamepadLure)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gamepadPointerLureData));
		}
	}

	public unsafe Transform _gamepadLureLocationOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gamepadLureLocationOverride);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gamepadLureLocationOverride)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool _isLureActive
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isLureActive);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isLureActive)) = flag;
		}
	}

	public unsafe Vector3 _ScheduleOne_Gamepad_IGamepadPointerLure_Offset_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ScheduleOne_Gamepad_IGamepadPointerLure_Offset_k__BackingField);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ScheduleOne_Gamepad_IGamepadPointerLure_Offset_k__BackingField)) = vector;
		}
	}

	public unsafe IGamepadPointerLure GamepadLure
	{
		[CallerCount(1485)]
		[CachedScanResults(RefRangeStart = 115488, RefRangeEnd = 116973, XrefRangeStart = 115488, XrefRangeEnd = 116973, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GamepadLure_Public_get_IGamepadPointerLure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IGamepadPointerLure>(intPtr) : null;
		}
	}

	public unsafe virtual bool RegisterDefaultLureWhenEmpty
	{
		[CallerCount(18)]
		[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29273, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_RegisterDefaultLureWhenEmpty_Public_Virtual_New_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual GamepadPointerLureData ScheduleOne_002EGamepad_002EIGamepadPointerLure_002EData
	{
		[CallerCount(13)]
		[CachedScanResults(RefRangeStart = 2964, RefRangeEnd = 2977, XrefRangeStart = 2964, XrefRangeEnd = 2977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Data_Private_Virtual_Final_New_get_GamepadPointerLureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GamepadPointerLureData>(intPtr) : null;
		}
	}

	public unsafe virtual bool ScheduleOne_002EGamepad_002EIGamepadPointerLure_002EIsActive
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_IsActive_Private_Virtual_Final_New_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual Vector3 ScheduleOne_002EGamepad_002EIGamepadPointerLure_002EPosition
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230239, XrefRangeEnd = 230244, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Position_Private_Virtual_Final_New_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual Vector3 ScheduleOne_002EGamepad_002EIGamepadPointerLure_002EOffset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Offset_Private_Virtual_Final_New_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PlantHarvestable()
	{
		Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "PlantHarvestable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr);
		NativeFieldInfoPtr_Product = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "Product");
		NativeFieldInfoPtr_ProductQuantity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "ProductQuantity");
		NativeFieldInfoPtr__gamepadLure = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "_gamepadLure");
		NativeFieldInfoPtr__gamepadLureLocationOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "_gamepadLureLocationOverride");
		NativeFieldInfoPtr__isLureActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "_isLureActive");
		NativeFieldInfoPtr__ScheduleOne_Gamepad_IGamepadPointerLure_Offset_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, "<ScheduleOne.Gamepad.IGamepadPointerLure.Offset>k__BackingField");
		NativeMethodInfoPtr_get_GamepadLure_Public_get_IGamepadPointerLure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678561);
		NativeMethodInfoPtr_get_RegisterDefaultLureWhenEmpty_Public_Virtual_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678562);
		NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Data_Private_Virtual_Final_New_get_GamepadPointerLureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678563);
		NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_IsActive_Private_Virtual_Final_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678564);
		NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Position_Private_Virtual_Final_New_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678565);
		NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_get_Offset_Private_Virtual_Final_New_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678566);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678567);
		NativeMethodInfoPtr_Start_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678568);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678569);
		NativeMethodInfoPtr_Harvest_Public_Virtual_New_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678570);
		NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_SetActive_Private_Virtual_Final_New_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678571);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr, 100678572);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230244, XrefRangeEnd = 230249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230249, XrefRangeEnd = 230267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230267, XrefRangeEnd = 230279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230279, XrefRangeEnd = 230367, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Harvest(bool giveProduct = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&giveProduct);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Harvest_Public_Virtual_New_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void ScheduleOne_Gamepad_IGamepadPointerLure_SetActive(bool value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_Gamepad_IGamepadPointerLure_SetActive_Private_Virtual_Final_New_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230367, XrefRangeEnd = 230368, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlantHarvestable()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlantHarvestable>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlantHarvestable(IntPtr pointer)
		: base(pointer)
	{
	}
}
