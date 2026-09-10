using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Messaging;

public class IMessageEntity : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_MsgConversation_Public_Abstract_Virtual_New_get_MSGConversation_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MsgConversation_Public_Abstract_Virtual_New_set_Void_MSGConversation_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_onResponseChosen_Public_Abstract_Virtual_New_add_Void_ResponseCallback_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_onResponseChosen_Public_Abstract_Virtual_New_rem_Void_ResponseCallback_0;

	public unsafe virtual MSGConversation MsgConversation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_MsgConversation_Public_Abstract_Virtual_New_get_MSGConversation_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MSGConversation>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_MsgConversation_Public_Abstract_Virtual_New_set_Void_MSGConversation_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static IMessageEntity()
	{
		Il2CppClassPointerStore<IMessageEntity>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Messaging", "IMessageEntity");
		NativeMethodInfoPtr_get_MsgConversation_Public_Abstract_Virtual_New_get_MSGConversation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IMessageEntity>.NativeClassPtr, 100669723);
		NativeMethodInfoPtr_set_MsgConversation_Public_Abstract_Virtual_New_set_Void_MSGConversation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IMessageEntity>.NativeClassPtr, 100669724);
		NativeMethodInfoPtr_add_onResponseChosen_Public_Abstract_Virtual_New_add_Void_ResponseCallback_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IMessageEntity>.NativeClassPtr, 100669725);
		NativeMethodInfoPtr_remove_onResponseChosen_Public_Abstract_Virtual_New_rem_Void_ResponseCallback_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IMessageEntity>.NativeClassPtr, 100669726);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void add_onResponseChosen(ResponseCallback value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_add_onResponseChosen_Public_Abstract_Virtual_New_add_Void_ResponseCallback_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	public unsafe virtual void remove_onResponseChosen(ResponseCallback value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_remove_onResponseChosen_Public_Abstract_Virtual_New_rem_Void_ResponseCallback_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IMessageEntity(IntPtr pointer)
		: base(pointer)
	{
	}
}
