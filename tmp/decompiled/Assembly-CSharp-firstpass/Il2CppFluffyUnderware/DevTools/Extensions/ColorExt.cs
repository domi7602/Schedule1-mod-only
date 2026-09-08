using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools.Extensions;

public static class ColorExt : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_ToHtml_Public_Static_String_Color_0;

	static ColorExt()
	{
		Il2CppClassPointerStore<ColorExt>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Extensions", "ColorExt");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ColorExt>.NativeClassPtr);
		NativeMethodInfoPtr_ToHtml_Public_Static_String_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ColorExt>.NativeClassPtr, 100664380);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 34064, RefRangeEnd = 34066, XrefRangeStart = 34040, XrefRangeEnd = 34064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ToHtml(this Color c)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&c);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToHtml_Public_Static_String_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public ColorExt(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
