using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Dialogue;

[System.Serializable]
public class DialogueNodeEvent : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_NodeLabel;

	private static readonly System.IntPtr NativeFieldInfoPtr_onNodeDisplayed;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string NodeLabel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NodeLabel);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NodeLabel)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe UnityEvent onNodeDisplayed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNodeDisplayed);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onNodeDisplayed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	static DialogueNodeEvent()
	{
		Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Dialogue", "DialogueNodeEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr);
		NativeFieldInfoPtr_NodeLabel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr, "NodeLabel");
		NativeFieldInfoPtr_onNodeDisplayed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr, "onNodeDisplayed");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr, 100674739);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DialogueNodeEvent()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DialogueNodeEvent>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DialogueNodeEvent(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
