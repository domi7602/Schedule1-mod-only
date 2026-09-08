using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class TabAttribute : GroupAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_TabName;

	private static readonly IntPtr NativeFieldInfoPtr_TabBarName;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_split_Private_Static_Boolean_String_byref_String_byref_String_byref_String_0;

	public unsafe string TabName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TabName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TabName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string TabBarName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TabBarName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TabBarName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static TabAttribute()
	{
		Il2CppClassPointerStore<TabAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "TabAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_TabName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr, "TabName");
		NativeFieldInfoPtr_TabBarName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr, "TabBarName");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr, 100663887);
		NativeMethodInfoPtr_split_Private_Static_Boolean_String_byref_String_byref_String_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr, 100663888);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30666, XrefRangeEnd = 30693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TabAttribute(string pathAndName)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TabAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(pathAndName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30693, XrefRangeEnd = 30707, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool split(string pathAndName, out string path, out string tabBar, out string tabname)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(pathAndName);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		byte* num5 = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)));
		nint num6 = 0;
		*(nint**)num5 = &num6;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_split_Private_Static_Boolean_String_byref_String_byref_String_byref_String_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		path = IL2CPP.Il2CppStringToManaged((IntPtr)num2);
		tabBar = IL2CPP.Il2CppStringToManaged((IntPtr)num4);
		tabname = IL2CPP.Il2CppStringToManaged((IntPtr)num6);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public TabAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
