using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.PlayerTasks;
using UnityEngine;

namespace Il2CppScheduleOne.StationFramework;

public class PourableAngleLimit : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_Pourable;

	private static readonly IntPtr NativeFieldInfoPtr_Constraint;

	private static readonly IntPtr NativeFieldInfoPtr_AngleAtMaxFill;

	private static readonly IntPtr NativeFieldInfoPtr_AngleAtMinFill;

	private static readonly IntPtr NativeFieldInfoPtr_PourAngleMaxFill;

	private static readonly IntPtr NativeFieldInfoPtr_PourAngleMinFill;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe PourableModule Pourable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Pourable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PourableModule>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Pourable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pourableModule));
		}
	}

	public unsafe DraggableConstraint Constraint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Constraint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DraggableConstraint>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Constraint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)draggableConstraint));
		}
	}

	public unsafe float AngleAtMaxFill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleAtMaxFill);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleAtMaxFill)) = num;
		}
	}

	public unsafe float AngleAtMinFill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleAtMinFill);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleAtMinFill)) = num;
		}
	}

	public unsafe float PourAngleMaxFill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourAngleMaxFill);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourAngleMaxFill)) = num;
		}
	}

	public unsafe float PourAngleMinFill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourAngleMinFill);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PourAngleMinFill)) = num;
		}
	}

	static PourableAngleLimit()
	{
		Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.StationFramework", "PourableAngleLimit");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr);
		NativeFieldInfoPtr_Pourable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "Pourable");
		NativeFieldInfoPtr_Constraint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "Constraint");
		NativeFieldInfoPtr_AngleAtMaxFill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "AngleAtMaxFill");
		NativeFieldInfoPtr_AngleAtMinFill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "AngleAtMinFill");
		NativeFieldInfoPtr_PourAngleMaxFill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "PourAngleMaxFill");
		NativeFieldInfoPtr_PourAngleMinFill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, "PourAngleMinFill");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, 100679107);
		NativeMethodInfoPtr_FixedUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, 100679108);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr, 100679109);
	}

	[CallerCount(0)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235215, XrefRangeEnd = 235217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 235217, XrefRangeEnd = 235218, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PourableAngleLimit()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PourableAngleLimit>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PourableAngleLimit(IntPtr pointer)
		: base(pointer)
	{
	}
}
