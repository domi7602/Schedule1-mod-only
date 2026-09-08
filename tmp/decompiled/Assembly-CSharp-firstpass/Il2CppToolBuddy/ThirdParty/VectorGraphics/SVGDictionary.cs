using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGDictionary : Dictionary<string, Il2CppSystem.Object>
{
	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static SVGDictionary()
	{
		Il2CppClassPointerStore<SVGDictionary>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGDictionary");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGDictionary>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDictionary>.NativeClassPtr, 100663385);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20065, XrefRangeEnd = 20068, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGDictionary()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGDictionary>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SVGDictionary(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
