using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.State;

public class MonoStateMachine : MonoState
{
	private static readonly IntPtr NativeFieldInfoPtr__Stack_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_Stack_Public_Virtual_Final_New_get_List_1_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Stack_Private_set_Void_List_1_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Push_Public_Virtual_Final_New_Void_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Pop_Public_Virtual_Final_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Void_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Peek_Public_Virtual_Final_New_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_PeekRecursive_Public_Virtual_Final_New_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsAnyChildStateAcceptingInput_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScheduleOne_State_IState_get_name_Private_Virtual_Final_New_get_String_0;

	public unsafe List<IState> _Stack_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stack_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<IState>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stack_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe virtual List<IState> Stack
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Stack_Public_Virtual_Final_New_get_List_1_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<IState>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Stack_Private_set_Void_List_1_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public new unsafe virtual string ScheduleOne_002EState_002EIState_002Ename
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 48135, RefRangeEnd = 48145, XrefRangeStart = 48135, XrefRangeEnd = 48145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScheduleOne_State_IState_get_name_Private_Virtual_Final_New_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static MonoStateMachine()
	{
		Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.State", "MonoStateMachine");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr);
		NativeFieldInfoPtr__Stack_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, "<Stack>k__BackingField");
		NativeMethodInfoPtr_get_Stack_Public_Virtual_Final_New_get_List_1_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667056);
		NativeMethodInfoPtr_set_Stack_Private_set_Void_List_1_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667057);
		NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667058);
		NativeMethodInfoPtr_Push_Public_Virtual_Final_New_Void_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667059);
		NativeMethodInfoPtr_Pop_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667060);
		NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Void_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667061);
		NativeMethodInfoPtr_Peek_Public_Virtual_Final_New_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667062);
		NativeMethodInfoPtr_PeekRecursive_Public_Virtual_Final_New_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667063);
		NativeMethodInfoPtr_IsAnyChildStateAcceptingInput_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667064);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667065);
		NativeMethodInfoPtr_ScheduleOne_State_IState_get_name_Private_Virtual_Final_New_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr, 100667066);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 103173, XrefRangeEnd = 103174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnActivate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 103203, RefRangeEnd = 103208, XrefRangeStart = 103174, XrefRangeEnd = 103203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Push(IState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Push_Public_Virtual_Final_New_Void_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 103233, RefRangeEnd = 103237, XrefRangeStart = 103208, XrefRangeEnd = 103233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Pop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Pop_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 103260, RefRangeEnd = 103263, XrefRangeStart = 103237, XrefRangeEnd = 103260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Remove(IState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Void_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 103265, RefRangeEnd = 103273, XrefRangeStart = 103263, XrefRangeEnd = 103265, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual IState Peek()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Peek_Public_Virtual_Final_New_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 103276, RefRangeEnd = 103286, XrefRangeStart = 103273, XrefRangeEnd = 103276, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual IState PeekRecursive()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PeekRecursive_Public_Virtual_Final_New_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 103294, RefRangeEnd = 103297, XrefRangeStart = 103286, XrefRangeEnd = 103294, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsAnyChildStateAcceptingInput()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsAnyChildStateAcceptingInput_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 102528, RefRangeEnd = 102529, XrefRangeStart = 102528, XrefRangeEnd = 102529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MonoStateMachine()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MonoStateMachine>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MonoStateMachine(IntPtr pointer)
		: base(pointer)
	{
	}
}
