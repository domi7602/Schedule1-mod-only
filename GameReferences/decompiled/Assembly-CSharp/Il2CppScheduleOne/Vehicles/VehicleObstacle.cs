using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles;

public class VehicleObstacle : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EObstacleType")]
	public enum EObstacleType
	{
		Generic,
		TrafficLight
	}

	private static readonly IntPtr NativeFieldInfoPtr_col;

	private static readonly IntPtr NativeFieldInfoPtr_twoSided;

	private static readonly IntPtr NativeFieldInfoPtr_type;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Collider col
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_col);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_col)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider));
		}
	}

	public unsafe bool twoSided
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_twoSided);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_twoSided)) = flag;
		}
	}

	public unsafe EObstacleType type
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_type);
			return *(EObstacleType*)num;
		}
		set
		{
			*(EObstacleType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_type)) = eObstacleType;
		}
	}

	static VehicleObstacle()
	{
		Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles", "VehicleObstacle");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr);
		NativeFieldInfoPtr_col = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr, "col");
		NativeFieldInfoPtr_twoSided = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr, "twoSided");
		NativeFieldInfoPtr_type = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr, "type");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr, 100666313);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 94903, XrefRangeEnd = 94904, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleObstacle()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehicleObstacle>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VehicleObstacle(IntPtr pointer)
		: base(pointer)
	{
	}
}
