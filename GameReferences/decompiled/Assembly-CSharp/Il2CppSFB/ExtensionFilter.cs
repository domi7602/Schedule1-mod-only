using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSFB;

public sealed class ExtensionFilter : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Name;

	private static readonly System.IntPtr NativeFieldInfoPtr_Extensions;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppStringArray_0;

	public unsafe string Name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppStringArray Extensions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Extensions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Extensions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ExtensionFilter()
	{
		Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "SFB", "ExtensionFilter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr);
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr, "Name");
		NativeFieldInfoPtr_Extensions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr, "Extensions");
		NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr, 100664482);
	}

	[CallerCount(80)]
	[CachedScanResults(RefRangeStart = 62123, RefRangeEnd = 62203, XrefRangeStart = 62123, XrefRangeEnd = 62203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExtensionFilter(string filterName, [Optional] Il2CppStringArray filterExtensions)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		if (filterExtensions == null)
		{
			filterExtensions = new Il2CppStringArray(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(filterName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)filterExtensions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppStringArray_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ExtensionFilter(string filterName, params string[] filterExtensions)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		this._002Ector(filterName, new Il2CppStringArray(filterExtensions));
	}

	public ExtensionFilter(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public ExtensionFilter()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExtensionFilter>.NativeClassPtr))
	{
	}
}
