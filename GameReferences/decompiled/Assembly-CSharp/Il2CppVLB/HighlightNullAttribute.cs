using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppVLB;

public sealed class HighlightNullAttribute : PropertyAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static HighlightNullAttribute()
	{
		Il2CppClassPointerStore<HighlightNullAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "HighlightNullAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HighlightNullAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HighlightNullAttribute>.NativeClassPtr, 100664004);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 66195, RefRangeEnd = 66198, XrefRangeStart = 66195, XrefRangeEnd = 66198, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HighlightNullAttribute()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HighlightNullAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HighlightNullAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
