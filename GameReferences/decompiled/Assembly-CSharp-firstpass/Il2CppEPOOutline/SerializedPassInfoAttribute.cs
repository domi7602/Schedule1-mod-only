using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppEPOOutline;

public class SerializedPassInfoAttribute : Il2CppSystem.Attribute
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Title;

	private static readonly System.IntPtr NativeFieldInfoPtr_ShadersFolder;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_0;

	public unsafe string Title
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ShadersFolder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShadersFolder);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShadersFolder)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static SerializedPassInfoAttribute()
	{
		Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "EPOOutline", "SerializedPassInfoAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Title = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr, "Title");
		NativeFieldInfoPtr_ShadersFolder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr, "ShadersFolder");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr, 100667454);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 64342, RefRangeEnd = 64345, XrefRangeStart = 64339, XrefRangeEnd = 64342, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SerializedPassInfoAttribute(string title, string shadersFolder)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SerializedPassInfoAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(shadersFolder);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SerializedPassInfoAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
