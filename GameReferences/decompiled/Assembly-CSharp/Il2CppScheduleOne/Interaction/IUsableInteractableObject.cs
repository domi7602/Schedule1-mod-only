using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Management;
using UnityEngine;

namespace Il2CppScheduleOne.Interaction;

public class IUsableInteractableObject : InteractableObject
{
	private static readonly IntPtr NativeFieldInfoPtr__iUsableMonoBehaviour;

	private static readonly IntPtr NativeFieldInfoPtr__defaultMessage;

	private static readonly IntPtr NativeFieldInfoPtr__iUsable;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Hovered_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe MonoBehaviour _iUsableMonoBehaviour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iUsableMonoBehaviour);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MonoBehaviour>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iUsableMonoBehaviour)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)monoBehaviour));
		}
	}

	public unsafe string _defaultMessage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultMessage);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultMessage)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe IUsable _iUsable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iUsable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IUsable>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__iUsable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)usable));
		}
	}

	static IUsableInteractableObject()
	{
		Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Interaction", "IUsableInteractableObject");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr);
		NativeFieldInfoPtr__iUsableMonoBehaviour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, "_iUsableMonoBehaviour");
		NativeFieldInfoPtr__defaultMessage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, "_defaultMessage");
		NativeFieldInfoPtr__iUsable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, "_iUsable");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, 100672369);
		NativeMethodInfoPtr_Hovered_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, 100672370);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr, 100672371);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165810, XrefRangeEnd = 165817, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165817, XrefRangeEnd = 165837, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Hovered()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Hovered_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165332, RefRangeEnd = 165333, XrefRangeStart = 165332, XrefRangeEnd = 165333, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IUsableInteractableObject()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IUsableInteractableObject>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IUsableInteractableObject(IntPtr pointer)
		: base(pointer)
	{
	}
}
