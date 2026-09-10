using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class TextResponseData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Text;

	private static readonly System.IntPtr NativeFieldInfoPtr_Label;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string Text
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Text);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Text)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Label
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Label);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Label)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static TextResponseData()
	{
		Il2CppClassPointerStore<TextResponseData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "TextResponseData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr);
		NativeFieldInfoPtr_Text = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr, "Text");
		NativeFieldInfoPtr_Label = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr, "Label");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr, 100669472);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr, 100669473);
	}

	[CallerCount(53)]
	[CachedScanResults(RefRangeStart = 100943, RefRangeEnd = 100996, XrefRangeStart = 100943, XrefRangeEnd = 100996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextResponseData(string text, string label)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(text);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(label);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135485, RefRangeEnd = 135486, XrefRangeStart = 135477, XrefRangeEnd = 135485, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TextResponseData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TextResponseData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TextResponseData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
