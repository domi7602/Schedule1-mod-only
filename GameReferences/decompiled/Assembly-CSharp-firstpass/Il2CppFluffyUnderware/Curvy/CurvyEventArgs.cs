using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyEventArgs : Il2CppSystem.EventArgs
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Sender;

	private static readonly System.IntPtr NativeFieldInfoPtr_Data;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_Object_0;

	public unsafe MonoBehaviour Sender
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sender);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MonoBehaviour>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Sender)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)monoBehaviour));
		}
	}

	public unsafe Il2CppSystem.Object Data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	static CurvyEventArgs()
	{
		Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyEventArgs");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr);
		NativeFieldInfoPtr_Sender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr, "Sender");
		NativeFieldInfoPtr_Data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr, "Data");
		NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr, 100664484);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 34819, RefRangeEnd = 34820, XrefRangeStart = 34813, XrefRangeEnd = 34819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyEventArgs(MonoBehaviour sender, Il2CppSystem.Object data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyEventArgs>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sender);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MonoBehaviour_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyEventArgs(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
