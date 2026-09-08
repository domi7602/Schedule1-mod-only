using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Tools;

public class MotionEvent : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Actions;

	private static readonly System.IntPtr NativeFieldInfoPtr_LastUpdatedDistance;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<Il2CppSystem.Action> Actions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Actions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Action>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Actions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector3 LastUpdatedDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastUpdatedDistance);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LastUpdatedDistance)) = vector;
		}
	}

	static MotionEvent()
	{
		Il2CppClassPointerStore<MotionEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "MotionEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr);
		NativeFieldInfoPtr_Actions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr, "Actions");
		NativeFieldInfoPtr_LastUpdatedDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr, "LastUpdatedDistance");
		NativeMethodInfoPtr_Update_Public_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr, 100678085);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr, 100678086);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 226254, RefRangeEnd = 226256, XrefRangeStart = 226240, XrefRangeEnd = 226254, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update(Vector3 newPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&newPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 226266, RefRangeEnd = 226268, XrefRangeStart = 226256, XrefRangeEnd = 226266, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MotionEvent()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MotionEvent>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MotionEvent(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
