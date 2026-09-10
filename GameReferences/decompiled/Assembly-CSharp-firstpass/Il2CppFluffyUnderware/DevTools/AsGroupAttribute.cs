using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class AsGroupAttribute : GroupAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	static AsGroupAttribute()
	{
		Il2CppClassPointerStore<AsGroupAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "AsGroupAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AsGroupAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AsGroupAttribute>.NativeClassPtr, 100663886);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30665, XrefRangeEnd = 30666, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AsGroupAttribute(string pathAndName = null)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AsGroupAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(pathAndName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AsGroupAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
