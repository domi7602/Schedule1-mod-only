using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Collections.Generic;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGPostponedFills : Dictionary<IFill, string>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static SVGPostponedFills()
	{
		Il2CppClassPointerStore<SVGPostponedFills>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGPostponedFills");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGPostponedFills>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGPostponedFills>.NativeClassPtr, 100663386);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20068, XrefRangeEnd = 20071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGPostponedFills()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGPostponedFills>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SVGPostponedFills(IntPtr pointer)
		: base(pointer)
	{
	}
}
