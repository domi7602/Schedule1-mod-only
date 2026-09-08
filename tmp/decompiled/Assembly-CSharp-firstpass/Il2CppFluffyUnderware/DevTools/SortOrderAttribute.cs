using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class SortOrderAttribute : DTAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	static SortOrderAttribute()
	{
		Il2CppClassPointerStore<SortOrderAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "SortOrderAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SortOrderAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SortOrderAttribute>.NativeClassPtr, 100663885);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30664, XrefRangeEnd = 30665, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SortOrderAttribute(int sort = 100)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SortOrderAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&sort);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SortOrderAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
