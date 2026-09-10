using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using UnityEngine;

namespace Il2CppScheduleOne.Map;

public class AutoshopAccessZone : NPCPresenceAccessZone
{
	private static readonly IntPtr NativeFieldInfoPtr_RollerDoorAnim;

	private static readonly IntPtr NativeFieldInfoPtr_VehicleDetection;

	private static readonly IntPtr NativeFieldInfoPtr_rollerDoorOpen;

	private static readonly IntPtr NativeMethodInfoPtr_SetIsOpen_Public_Virtual_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_MinPass_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Animation RollerDoorAnim
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RollerDoorAnim);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RollerDoorAnim)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
		}
	}

	public unsafe VehicleDetector VehicleDetection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VehicleDetection);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VehicleDetector>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VehicleDetection)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleDetector));
		}
	}

	public unsafe bool rollerDoorOpen
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rollerDoorOpen);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rollerDoorOpen)) = flag;
		}
	}

	static AutoshopAccessZone()
	{
		Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Map", "AutoshopAccessZone");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr);
		NativeFieldInfoPtr_RollerDoorAnim = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, "RollerDoorAnim");
		NativeFieldInfoPtr_VehicleDetection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, "VehicleDetection");
		NativeFieldInfoPtr_rollerDoorOpen = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, "rollerDoorOpen");
		NativeMethodInfoPtr_SetIsOpen_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, 100670054);
		NativeMethodInfoPtr_MinPass_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, 100670055);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr, 100670056);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141221, XrefRangeEnd = 141227, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void SetIsOpen(bool open)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&open);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetIsOpen_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141227, XrefRangeEnd = 141238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void MinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_MinPass_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 141238, XrefRangeEnd = 141239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AutoshopAccessZone()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AutoshopAccessZone>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AutoshopAccessZone(IntPtr pointer)
		: base(pointer)
	{
	}
}
