using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Tools;

public class PreallocatedAction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__listeners;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Addition_Public_Static_PreallocatedAction_PreallocatedAction_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Subtraction_Public_Static_PreallocatedAction_PreallocatedAction_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_get_Int32_0;

	public unsafe List<Il2CppSystem.Action> _listeners
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__listeners);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Action>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__listeners)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int Count
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 227104, XrefRangeEnd = 227105, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Count_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PreallocatedAction()
	{
		Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tools", "PreallocatedAction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr);
		NativeFieldInfoPtr__listeners = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, "_listeners");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678163);
		NativeMethodInfoPtr_op_Addition_Public_Static_PreallocatedAction_PreallocatedAction_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678164);
		NativeMethodInfoPtr_op_Subtraction_Public_Static_PreallocatedAction_PreallocatedAction_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678165);
		NativeMethodInfoPtr_Add_Public_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678166);
		NativeMethodInfoPtr_Remove_Public_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678167);
		NativeMethodInfoPtr_Invoke_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678168);
		NativeMethodInfoPtr_get_Count_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr, 100678169);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 227056, RefRangeEnd = 227059, XrefRangeStart = 227048, XrefRangeEnd = 227056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PreallocatedAction(int capacity = 16)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PreallocatedAction>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&capacity);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 227062, RefRangeEnd = 227065, XrefRangeStart = 227059, XrefRangeEnd = 227062, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static PreallocatedAction operator +(PreallocatedAction evt, Il2CppSystem.Action listener)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)evt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Addition_Public_Static_PreallocatedAction_PreallocatedAction_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PreallocatedAction>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 227078, RefRangeEnd = 227081, XrefRangeStart = 227065, XrefRangeEnd = 227078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static PreallocatedAction operator -(PreallocatedAction evt, Il2CppSystem.Action listener)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)evt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Subtraction_Public_Static_PreallocatedAction_PreallocatedAction_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PreallocatedAction>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 227081, XrefRangeEnd = 227084, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Add(Il2CppSystem.Action listener)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Void_Action_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 227084, XrefRangeEnd = 227097, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Remove(Il2CppSystem.Action listener)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_Action_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 227102, RefRangeEnd = 227104, XrefRangeStart = 227097, XrefRangeEnd = 227102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Invoke()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PreallocatedAction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
