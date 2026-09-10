using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles;

public class VehicleRecoveryPoint : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_recoveryPoints;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetClosestRecoveryPoint_Public_Static_VehicleRecoveryPoint_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static List<VehicleRecoveryPoint> recoveryPoints
	{
		get
		{
			Unsafe.SkipInit(out IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_recoveryPoints, (void*)(&intPtr));
			IntPtr intPtr2 = intPtr;
			return (intPtr2 != (IntPtr)0) ? Il2CppObjectPool.Get<List<VehicleRecoveryPoint>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_recoveryPoints, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static VehicleRecoveryPoint()
	{
		Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles", "VehicleRecoveryPoint");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr);
		NativeFieldInfoPtr_recoveryPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr, "recoveryPoints");
		NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr, 100666314);
		NativeMethodInfoPtr_GetClosestRecoveryPoint_Public_Static_VehicleRecoveryPoint_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr, 100666315);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr, 100666316);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 94904, XrefRangeEnd = 94914, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 94941, RefRangeEnd = 94942, XrefRangeStart = 94914, XrefRangeEnd = 94941, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static VehicleRecoveryPoint GetClosestRecoveryPoint(Vector3 pos)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&pos);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClosestRecoveryPoint_Public_Static_VehicleRecoveryPoint_Vector3_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VehicleRecoveryPoint>(intPtr) : null;
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleRecoveryPoint()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehicleRecoveryPoint>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VehicleRecoveryPoint(IntPtr pointer)
		: base(pointer)
	{
	}
}
