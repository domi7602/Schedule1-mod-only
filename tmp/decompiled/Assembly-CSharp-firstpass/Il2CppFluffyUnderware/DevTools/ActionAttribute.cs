using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools;

public class ActionAttribute : DTAttribute
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ActionEnum")]
	public enum ActionEnum
	{
		Show,
		Hide,
		Enable,
		Disable,
		ShowInfo,
		ShowWarning,
		ShowError,
		Callback
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ActionPositionEnum")]
	public enum ActionPositionEnum
	{
		Above,
		Below
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Action;

	private static readonly System.IntPtr NativeFieldInfoPtr_Position;

	private static readonly System.IntPtr NativeFieldInfoPtr_ActionData;

	private static readonly System.IntPtr NativeFieldInfoPtr_mCallback;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_ActionEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Callback_Public_Void_Object_0;

	public unsafe ActionEnum Action
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Action);
			return *(ActionEnum*)num;
		}
		set
		{
			*(ActionEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Action)) = actionEnum;
		}
	}

	public unsafe ActionPositionEnum Position
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position);
			return *(ActionPositionEnum*)num;
		}
		set
		{
			*(ActionPositionEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position)) = actionPositionEnum;
		}
	}

	public unsafe Il2CppSystem.Object ActionData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActionData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActionData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe MethodInfo mCallback
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallback);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MethodInfo>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCallback)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)methodInfo));
		}
	}

	static ActionAttribute()
	{
		Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "ActionAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Action = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, "Action");
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, "Position");
		NativeFieldInfoPtr_ActionData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, "ActionData");
		NativeFieldInfoPtr_mCallback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, "mCallback");
		NativeMethodInfoPtr__ctor_Protected_Void_String_ActionEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, 100663876);
		NativeMethodInfoPtr_Callback_Public_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr, 100663877);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30495, XrefRangeEnd = 30497, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActionAttribute(string actionData, ActionEnum action = ActionEnum.Callback)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ActionAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(actionData);
		*(ActionEnum**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &action;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_ActionEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30497, XrefRangeEnd = 30519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Callback(Il2CppSystem.Object classInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)classInstance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Callback_Public_Void_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ActionAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
