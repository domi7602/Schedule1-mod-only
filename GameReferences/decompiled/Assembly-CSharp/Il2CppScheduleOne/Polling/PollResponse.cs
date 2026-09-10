using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Polling;

[System.Serializable]
public class PollResponse : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_polls;

	private static readonly System.IntPtr NativeFieldInfoPtr_active;

	private static readonly System.IntPtr NativeFieldInfoPtr_confirmed;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetActive_Public_PollData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConfirmed_Public_PollData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__GetActive_b__3_0_Private_Boolean_PollData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__GetConfirmed_b__4_0_Private_Boolean_PollData_0;

	public unsafe Il2CppReferenceArray<PollData> polls
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polls);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<PollData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_polls)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int active
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_active)) = num;
		}
	}

	public unsafe int confirmed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_confirmed);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_confirmed)) = num;
		}
	}

	static PollResponse()
	{
		Il2CppClassPointerStore<PollResponse>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Polling", "PollResponse");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PollResponse>.NativeClassPtr);
		NativeFieldInfoPtr_polls = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, "polls");
		NativeFieldInfoPtr_active = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, "active");
		NativeFieldInfoPtr_confirmed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, "confirmed");
		NativeMethodInfoPtr_GetActive_Public_PollData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, 100668150);
		NativeMethodInfoPtr_GetConfirmed_Public_PollData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, 100668151);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, 100668152);
		NativeMethodInfoPtr__GetActive_b__3_0_Private_Boolean_PollData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, 100668153);
		NativeMethodInfoPtr__GetConfirmed_b__4_0_Private_Boolean_PollData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PollResponse>.NativeClassPtr, 100668154);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 115473, RefRangeEnd = 115475, XrefRangeStart = 115464, XrefRangeEnd = 115473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PollData GetActive()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetActive_Public_PollData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PollData>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 115484, RefRangeEnd = 115486, XrefRangeStart = 115475, XrefRangeEnd = 115484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PollData GetConfirmed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConfirmed_Public_PollData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PollData>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PollResponse()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PollResponse>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool _GetActive_b__3_0(PollData x)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetActive_b__3_0_Private_Boolean_PollData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool _GetConfirmed_b__4_0(PollData x)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetConfirmed_b__4_0_Private_Boolean_PollData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PollResponse(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
