using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class SectionAttribute : GroupAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_Fixed;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_Int32_0;

	public unsafe bool Fixed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fixed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fixed)) = flag;
		}
	}

	static SectionAttribute()
	{
		Il2CppClassPointerStore<SectionAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "SectionAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SectionAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Fixed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SectionAttribute>.NativeClassPtr, "Fixed");
		NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SectionAttribute>.NativeClassPtr, 100663889);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30707, XrefRangeEnd = 30708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SectionAttribute(string name, bool expanded = true, bool fix = false, int sort = 100)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SectionAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &expanded;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &fix;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &sort;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Boolean_Boolean_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SectionAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
