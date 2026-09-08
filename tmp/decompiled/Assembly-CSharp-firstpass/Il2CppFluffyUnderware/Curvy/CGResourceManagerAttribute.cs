using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy;

public class CGResourceManagerAttribute : DTPropertyAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_ResourceName;

	private static readonly IntPtr NativeFieldInfoPtr_ReadOnly;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe string ResourceName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ResourceName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ResourceName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool ReadOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReadOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReadOnly)) = flag;
		}
	}

	static CGResourceManagerAttribute()
	{
		Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CGResourceManagerAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_ResourceName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr, "ResourceName");
		NativeFieldInfoPtr_ReadOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr, "ReadOnly");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr, 100664437);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34641, XrefRangeEnd = 34647, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGResourceManagerAttribute(string resourceName)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGResourceManagerAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGResourceManagerAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
