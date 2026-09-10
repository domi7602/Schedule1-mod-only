using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class StringIntPair : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_String;

	private static readonly System.IntPtr NativeFieldInfoPtr_Int;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string String
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_String);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_String)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int Int
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Int);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Int)) = num;
		}
	}

	static StringIntPair()
	{
		Il2CppClassPointerStore<StringIntPair>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "StringIntPair");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr);
		NativeFieldInfoPtr_String = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr, "String");
		NativeFieldInfoPtr_Int = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr, "Int");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr, 100675201);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr, 100675202);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 123667, RefRangeEnd = 123677, XrefRangeStart = 123667, XrefRangeEnd = 123677, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StringIntPair(string str, int i)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(str);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &i;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StringIntPair()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StringIntPair>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StringIntPair(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
