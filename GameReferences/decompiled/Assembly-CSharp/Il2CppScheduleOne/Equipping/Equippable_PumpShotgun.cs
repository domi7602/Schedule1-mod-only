using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Equipping;

public class Equippable_PumpShotgun : Equippable_RangedWeapon
{
	private static readonly IntPtr NativeFieldInfoPtr_PelletCount;

	private static readonly IntPtr NativeMethodInfoPtr_GetBulletDirections_Protected_Virtual_Il2CppStructArray_1_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int PelletCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelletCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PelletCount)) = num;
		}
	}

	static Equippable_PumpShotgun()
	{
		Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping", "Equippable_PumpShotgun");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr);
		NativeFieldInfoPtr_PelletCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr, "PelletCount");
		NativeMethodInfoPtr_GetBulletDirections_Protected_Virtual_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr, 100679799);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr, 100679800);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 243862, XrefRangeEnd = 243873, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Il2CppStructArray<Vector3> GetBulletDirections()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetBulletDirections_Protected_Virtual_Il2CppStructArray_1_Vector3_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 243873, XrefRangeEnd = 243874, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Equippable_PumpShotgun()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Equippable_PumpShotgun>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Equippable_PumpShotgun(IntPtr pointer)
		: base(pointer)
	{
	}
}
