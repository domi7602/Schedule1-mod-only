using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Object;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vision;

[System.Serializable]
public class VisionEventReceipt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Target;

	private static readonly System.IntPtr NativeFieldInfoPtr_State;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_NetworkObject_EVisualState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe NetworkObject Target
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NetworkObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)networkObject));
		}
	}

	public unsafe EVisualState State
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State);
			return *(EVisualState*)num;
		}
		set
		{
			*(EVisualState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_State)) = eVisualState;
		}
	}

	static VisionEventReceipt()
	{
		Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vision", "VisionEventReceipt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr);
		NativeFieldInfoPtr_Target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr, "Target");
		NativeFieldInfoPtr_State = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr, "State");
		NativeMethodInfoPtr__ctor_Public_Void_NetworkObject_EVisualState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr, 100668660);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr, 100668661);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 123667, RefRangeEnd = 123677, XrefRangeStart = 123665, XrefRangeEnd = 123667, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VisionEventReceipt(NetworkObject target, EVisualState state)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		*(EVisualState**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &state;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_NetworkObject_EVisualState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VisionEventReceipt()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VisionEventReceipt>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VisionEventReceipt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
