using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public sealed class ModuleInfoAttribute : Il2CppSystem.Attribute
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MenuName;

	private static readonly System.IntPtr NativeFieldInfoPtr_ModuleName;

	private static readonly System.IntPtr NativeFieldInfoPtr_Description;

	private static readonly System.IntPtr NativeFieldInfoPtr_UsesRandom;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0;

	public unsafe string MenuName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MenuName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MenuName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string ModuleName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModuleName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModuleName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Description
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Description);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Description)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool UsesRandom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsesRandom);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UsesRandom)) = flag;
		}
	}

	static ModuleInfoAttribute()
	{
		Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "ModuleInfoAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_MenuName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, "MenuName");
		NativeFieldInfoPtr_ModuleName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, "ModuleName");
		NativeFieldInfoPtr_Description = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, "Description");
		NativeFieldInfoPtr_UsesRandom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, "UsesRandom");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, 100666462);
		NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr, 100666463);
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 31934, RefRangeEnd = 31959, XrefRangeStart = 31934, XrefRangeEnd = 31959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ModuleInfoAttribute(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ModuleInfoAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54468, XrefRangeEnd = 54473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual int CompareTo(Il2CppSystem.Object obj)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ModuleInfoAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
