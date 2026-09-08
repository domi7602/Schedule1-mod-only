using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2Cpp;

public class ReadOnlyAttribute : PropertyAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static ReadOnlyAttribute()
	{
		Il2CppClassPointerStore<ReadOnlyAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "ReadOnlyAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ReadOnlyAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ReadOnlyAttribute>.NativeClassPtr, 100663416);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 66195, RefRangeEnd = 66198, XrefRangeStart = 66194, XrefRangeEnd = 66195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ReadOnlyAttribute()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ReadOnlyAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ReadOnlyAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
