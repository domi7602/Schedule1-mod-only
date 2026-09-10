using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class MinMaxAttribute : DTPropertyAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_MaxValueField;

	private static readonly IntPtr NativeFieldInfoPtr_Min;

	private static readonly IntPtr NativeFieldInfoPtr_MinBoundFieldOrPropertyName;

	private static readonly IntPtr NativeFieldInfoPtr_Max;

	private static readonly IntPtr NativeFieldInfoPtr_MaxBoundFieldOrPropertyName;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0;

	public unsafe string MaxValueField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxValueField);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxValueField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float Min
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Min);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Min)) = num;
		}
	}

	public unsafe string MinBoundFieldOrPropertyName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinBoundFieldOrPropertyName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinBoundFieldOrPropertyName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float Max
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Max);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Max)) = num;
		}
	}

	public unsafe string MaxBoundFieldOrPropertyName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxBoundFieldOrPropertyName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxBoundFieldOrPropertyName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static MinMaxAttribute()
	{
		Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "MinMaxAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_MaxValueField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, "MaxValueField");
		NativeFieldInfoPtr_Min = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, "Min");
		NativeFieldInfoPtr_MinBoundFieldOrPropertyName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, "MinBoundFieldOrPropertyName");
		NativeFieldInfoPtr_Max = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, "Max");
		NativeFieldInfoPtr_MaxBoundFieldOrPropertyName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, "MaxBoundFieldOrPropertyName");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr, 100664173);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32268, XrefRangeEnd = 32272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MinMaxAttribute(string maxValueField, string label = "", string tooltip = "")
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MinMaxAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(maxValueField);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(label);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(tooltip);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MinMaxAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
