using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools;

public class EnvironmentAgnosticInitializeOnLoadMethodAttribute : RuntimeInitializeOnLoadMethodAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_RuntimeInitializeLoadType_0;

	static EnvironmentAgnosticInitializeOnLoadMethodAttribute()
	{
		Il2CppClassPointerStore<EnvironmentAgnosticInitializeOnLoadMethodAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "EnvironmentAgnosticInitializeOnLoadMethodAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EnvironmentAgnosticInitializeOnLoadMethodAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_RuntimeInitializeLoadType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EnvironmentAgnosticInitializeOnLoadMethodAttribute>.NativeClassPtr, 100664039);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31959, XrefRangeEnd = 31960, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EnvironmentAgnosticInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType loadType)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EnvironmentAgnosticInitializeOnLoadMethodAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&loadType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_RuntimeInitializeLoadType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EnvironmentAgnosticInitializeOnLoadMethodAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
