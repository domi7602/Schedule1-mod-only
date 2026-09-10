using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.State;

public class IStateMachine : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_Stack_Public_Abstract_Virtual_New_get_List_1_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Push_Public_Abstract_Virtual_New_Void_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Pop_Public_Abstract_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Remove_Public_Abstract_Virtual_New_Void_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Peek_Public_Abstract_Virtual_New_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_PeekRecursive_Public_Abstract_Virtual_New_IState_0;

	private static readonly IntPtr NativeMethodInfoPtr_StackToString_Public_Virtual_New_String_Int32_0;

	public unsafe virtual List<IState> Stack
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Stack_Public_Abstract_Virtual_New_get_List_1_IState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<IState>>(intPtr) : null;
		}
	}

	static IStateMachine()
	{
		Il2CppClassPointerStore<IStateMachine>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.State", "IStateMachine");
		NativeMethodInfoPtr_get_Stack_Public_Abstract_Virtual_New_get_List_1_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667007);
		NativeMethodInfoPtr_Push_Public_Abstract_Virtual_New_Void_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667008);
		NativeMethodInfoPtr_Pop_Public_Abstract_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667009);
		NativeMethodInfoPtr_Remove_Public_Abstract_Virtual_New_Void_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667010);
		NativeMethodInfoPtr_Peek_Public_Abstract_Virtual_New_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667011);
		NativeMethodInfoPtr_PeekRecursive_Public_Abstract_Virtual_New_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667012);
		NativeMethodInfoPtr_StackToString_Public_Virtual_New_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStateMachine>.NativeClassPtr, 100667013);
	}

	[CallerCount(0)]
	public unsafe virtual void Push(IState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Push_Public_Abstract_Virtual_New_Void_IState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Pop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Pop_Public_Abstract_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Remove(IState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Remove_Public_Abstract_Virtual_New_Void_IState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual IState Peek()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Peek_Public_Abstract_Virtual_New_IState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual IState PeekRecursive()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_PeekRecursive_Public_Abstract_Virtual_New_IState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 102534, XrefRangeEnd = 102571, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual string StackToString(int indentation = 0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&indentation);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StackToString_Public_Virtual_New_String_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public IStateMachine(IntPtr pointer)
		: base(pointer)
	{
	}
}
