using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class ToggleButtonAttribute : DTPropertyAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_0;

	static ToggleButtonAttribute()
	{
		Il2CppClassPointerStore<ToggleButtonAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "ToggleButtonAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ToggleButtonAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ToggleButtonAttribute>.NativeClassPtr, 100664159);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ToggleButtonAttribute(string label = "", string tooltip = "")
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ToggleButtonAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(label);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ToggleButtonAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
