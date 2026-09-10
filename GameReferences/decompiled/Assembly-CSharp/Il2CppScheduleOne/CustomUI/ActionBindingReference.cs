using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine.InputSystem;

namespace Il2CppScheduleOne.CustomUI;

[System.Serializable]
public class ActionBindingReference : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ActionReference;

	private static readonly System.IntPtr NativeFieldInfoPtr_BindingId;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe InputActionReference ActionReference
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActionReference);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InputActionReference>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActionReference)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputActionReference));
		}
	}

	public unsafe string BindingId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BindingId);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BindingId)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static ActionBindingReference()
	{
		Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.CustomUI", "ActionBindingReference");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr);
		NativeFieldInfoPtr_ActionReference = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr, "ActionReference");
		NativeFieldInfoPtr_BindingId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr, "BindingId");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr, 100689823);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActionBindingReference()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ActionBindingReference>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ActionBindingReference(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
