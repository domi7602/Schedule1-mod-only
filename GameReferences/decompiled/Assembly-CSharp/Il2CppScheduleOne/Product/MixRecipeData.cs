using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Product;

[System.Serializable]
public class MixRecipeData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Product;

	private static readonly System.IntPtr NativeFieldInfoPtr_Mixer;

	private static readonly System.IntPtr NativeFieldInfoPtr_Output;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0;

	public unsafe string Product
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Product)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Mixer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mixer);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mixer)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Output
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Output);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Output)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static MixRecipeData()
	{
		Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Product", "MixRecipeData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr);
		NativeFieldInfoPtr_Product = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr, "Product");
		NativeFieldInfoPtr_Mixer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr, "Mixer");
		NativeFieldInfoPtr_Output = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr, "Output");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr, 100679248);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 29770, RefRangeEnd = 29775, XrefRangeStart = 29770, XrefRangeEnd = 29775, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MixRecipeData(string product, string mixer, string output)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MixRecipeData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(product);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(mixer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(output);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MixRecipeData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
