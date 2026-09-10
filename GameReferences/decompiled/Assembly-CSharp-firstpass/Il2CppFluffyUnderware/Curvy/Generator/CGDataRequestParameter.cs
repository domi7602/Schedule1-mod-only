using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGDataRequestParameter : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	static CGDataRequestParameter()
	{
		Il2CppClassPointerStore<CGDataRequestParameter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGDataRequestParameter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGDataRequestParameter>.NativeClassPtr);
		NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestParameter>.NativeClassPtr, 100665801);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataRequestParameter>.NativeClassPtr, 100665802);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 46815, RefRangeEnd = 46828, XrefRangeStart = 46815, XrefRangeEnd = 46828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static implicit operator bool(CGDataRequestParameter a)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Implicit_Public_Static_Boolean_CGDataRequestParameter_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGDataRequestParameter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGDataRequestParameter>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGDataRequestParameter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
