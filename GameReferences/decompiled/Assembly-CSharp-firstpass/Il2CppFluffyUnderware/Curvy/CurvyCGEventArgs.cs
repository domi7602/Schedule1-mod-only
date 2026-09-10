using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy.Generator;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvyCGEventArgs : Il2CppSystem.EventArgs
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Sender;

	private static readonly System.IntPtr NativeFieldInfoPtr_Generator;

	private static readonly System.IntPtr NativeFieldInfoPtr_Module;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvyGenerator_CGModule_0;

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

	public unsafe CurvyGenerator Generator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Generator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyGenerator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Generator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyGenerator));
		}
	}

	public unsafe CGModule Module
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Module);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Module)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
		}
	}

	static CurvyCGEventArgs()
	{
		Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvyCGEventArgs");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr);
		NativeFieldInfoPtr_Sender = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr, "Sender");
		NativeFieldInfoPtr_Generator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr, "Generator");
		NativeFieldInfoPtr_Module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr, "Module");
		NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr, 100664439);
		NativeMethodInfoPtr__ctor_Public_Void_CurvyGenerator_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr, 100664440);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34664, RefRangeEnd = 34666, XrefRangeStart = 34655, XrefRangeEnd = 34664, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyCGEventArgs(CGModule module)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 34673, RefRangeEnd = 34674, XrefRangeStart = 34666, XrefRangeEnd = 34673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyCGEventArgs(CurvyGenerator generator, CGModule module)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyCGEventArgs>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)generator);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvyGenerator_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CurvyCGEventArgs(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
