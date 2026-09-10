using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using UnityEngine.Events;

namespace Il2CppFluffyUnderware.DevTools;

[System.Serializable]
public class UnityEventEx<T0> : UnityEvent<T0>
{
	private static readonly System.IntPtr NativeFieldInfoPtr_mCallerList;

	private static readonly System.IntPtr NativeFieldInfoPtr_mCallsCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_mCount;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddListenerOnce_Public_Void_UnityAction_1_T0_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasListeners_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckForListeners_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppSystem.Object mCallerList
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallerList);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallerList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe MethodInfo mCallsCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallsCount);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MethodInfo>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallsCount)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)methodInfo));
		}
	}

	public unsafe int mCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCount)) = num;
		}
	}

	static UnityEventEx()
	{
		Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "UnityEventEx`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T0>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr);
		NativeFieldInfoPtr_mCallerList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, "mCallerList");
		NativeFieldInfoPtr_mCallsCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, "mCallsCount");
		NativeFieldInfoPtr_mCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, "mCount");
		NativeMethodInfoPtr_AddListenerOnce_Public_Void_UnityAction_1_T0_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, 100664019);
		NativeMethodInfoPtr_HasListeners_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, 100664020);
		NativeMethodInfoPtr_CheckForListeners_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, 100664021);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr, 100664022);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 31788, RefRangeEnd = 31792, XrefRangeStart = 31780, XrefRangeEnd = 31788, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddListenerOnce(UnityAction<T0> call)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)call);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddListenerOnce_Public_Void_UnityAction_1_T0_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31792, XrefRangeEnd = 31816, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool HasListeners()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasListeners_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe void CheckForListeners()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckForListeners_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 31818, RefRangeEnd = 31832, XrefRangeStart = 31816, XrefRangeEnd = 31818, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UnityEventEx()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UnityEventEx<T0>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UnityEventEx(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
