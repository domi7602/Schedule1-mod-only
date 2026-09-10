using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Misc;

public class UtilityPole : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_CABLE_CULL_DISTANCE;

	private static readonly IntPtr NativeFieldInfoPtr_CABLE_CULL_DISTANCE_SQR;

	private static readonly IntPtr NativeFieldInfoPtr_previousPole;

	private static readonly IntPtr NativeFieldInfoPtr_nextPole;

	private static readonly IntPtr NativeFieldInfoPtr_Connection1Enabled;

	private static readonly IntPtr NativeFieldInfoPtr_Connection2Enabled;

	private static readonly IntPtr NativeFieldInfoPtr_LengthFactor;

	private static readonly IntPtr NativeFieldInfoPtr_cable1Connection;

	private static readonly IntPtr NativeFieldInfoPtr_cable2Connection;

	private static readonly IntPtr NativeFieldInfoPtr_cable1Segments;

	private static readonly IntPtr NativeFieldInfoPtr_cable2Segments;

	private static readonly IntPtr NativeFieldInfoPtr_Cable1Container;

	private static readonly IntPtr NativeFieldInfoPtr_Cable2Container;

	private static readonly IntPtr NativeMethodInfoPtr_Orient_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DrawLines_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float CABLE_CULL_DISTANCE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CABLE_CULL_DISTANCE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CABLE_CULL_DISTANCE, (void*)(&num));
		}
	}

	public unsafe static float CABLE_CULL_DISTANCE_SQR
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CABLE_CULL_DISTANCE_SQR, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CABLE_CULL_DISTANCE_SQR, (void*)(&num));
		}
	}

	public unsafe UtilityPole previousPole
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_previousPole);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UtilityPole>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_previousPole)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)utilityPole));
		}
	}

	public unsafe UtilityPole nextPole
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextPole);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UtilityPole>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextPole)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)utilityPole));
		}
	}

	public unsafe bool Connection1Enabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Connection1Enabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Connection1Enabled)) = flag;
		}
	}

	public unsafe bool Connection2Enabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Connection2Enabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Connection2Enabled)) = flag;
		}
	}

	public unsafe float LengthFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LengthFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LengthFactor)) = num;
		}
	}

	public unsafe Transform cable1Connection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable1Connection);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable1Connection)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform cable2Connection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable2Connection);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable2Connection)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe List<Transform> cable1Segments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable1Segments);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Transform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable1Segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<Transform> cable2Segments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable2Segments);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Transform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cable2Segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Transform Cable1Container
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cable1Container);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cable1Container)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform Cable2Container
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cable2Container);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cable2Container)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	static UtilityPole()
	{
		Il2CppClassPointerStore<UtilityPole>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Misc", "UtilityPole");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr);
		NativeFieldInfoPtr_CABLE_CULL_DISTANCE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "CABLE_CULL_DISTANCE");
		NativeFieldInfoPtr_CABLE_CULL_DISTANCE_SQR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "CABLE_CULL_DISTANCE_SQR");
		NativeFieldInfoPtr_previousPole = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "previousPole");
		NativeFieldInfoPtr_nextPole = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "nextPole");
		NativeFieldInfoPtr_Connection1Enabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "Connection1Enabled");
		NativeFieldInfoPtr_Connection2Enabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "Connection2Enabled");
		NativeFieldInfoPtr_LengthFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "LengthFactor");
		NativeFieldInfoPtr_cable1Connection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "cable1Connection");
		NativeFieldInfoPtr_cable2Connection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "cable2Connection");
		NativeFieldInfoPtr_cable1Segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "cable1Segments");
		NativeFieldInfoPtr_cable2Segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "cable2Segments");
		NativeFieldInfoPtr_Cable1Container = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "Cable1Container");
		NativeFieldInfoPtr_Cable2Container = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, "Cable2Container");
		NativeMethodInfoPtr_Orient_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, 100671009);
		NativeMethodInfoPtr_DrawLines_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, 100671010);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr, 100671011);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 150910, XrefRangeEnd = 150964, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Orient()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Orient_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 150964, XrefRangeEnd = 151032, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DrawLines()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawLines_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 151032, XrefRangeEnd = 151045, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UtilityPole()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UtilityPole>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UtilityPole(IntPtr pointer)
		: base(pointer)
	{
	}
}
