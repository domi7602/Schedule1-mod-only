using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class NamedWebColorDictionary : Dictionary<string, Color>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static NamedWebColorDictionary()
	{
		Il2CppClassPointerStore<NamedWebColorDictionary>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "NamedWebColorDictionary");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NamedWebColorDictionary>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NamedWebColorDictionary>.NativeClassPtr, 100663508);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 26288, RefRangeEnd = 26289, XrefRangeStart = 25692, XrefRangeEnd = 26288, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NamedWebColorDictionary()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NamedWebColorDictionary>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NamedWebColorDictionary(IntPtr pointer)
		: base(pointer)
	{
	}
}
