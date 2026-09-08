using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Vision;

[Serializable]
public class UniqueVisibilityAttribute : VisibilityAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_uniquenessCode;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Single_String_Single_Int32_0;

	public unsafe string uniquenessCode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniquenessCode);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniquenessCode)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static UniqueVisibilityAttribute()
	{
		Il2CppClassPointerStore<UniqueVisibilityAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vision", "UniqueVisibilityAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UniqueVisibilityAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_uniquenessCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UniqueVisibilityAttribute>.NativeClassPtr, "uniquenessCode");
		NativeMethodInfoPtr__ctor_Public_Void_String_Single_String_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UniqueVisibilityAttribute>.NativeClassPtr, 100668594);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 122766, XrefRangeEnd = 122768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UniqueVisibilityAttribute(string _name, float _pointsChange, string _uniquenessCode, float _multiplier = 1f, int attributeIndex = -1)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UniqueVisibilityAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(_name);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &_pointsChange;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(_uniquenessCode);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &_multiplier;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &attributeIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Single_String_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UniqueVisibilityAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
