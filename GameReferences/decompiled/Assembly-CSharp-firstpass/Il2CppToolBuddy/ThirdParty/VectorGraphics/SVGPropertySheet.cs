using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Collections.Generic;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGPropertySheet : Dictionary<string, string>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static SVGPropertySheet()
	{
		Il2CppClassPointerStore<SVGPropertySheet>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGPropertySheet");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGPropertySheet>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGPropertySheet>.NativeClassPtr, 100663509);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 26292, RefRangeEnd = 26294, XrefRangeStart = 26289, XrefRangeEnd = 26292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGPropertySheet()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGPropertySheet>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SVGPropertySheet(IntPtr pointer)
		: base(pointer)
	{
	}
}
