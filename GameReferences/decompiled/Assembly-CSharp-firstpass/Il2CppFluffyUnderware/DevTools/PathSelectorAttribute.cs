using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class PathSelectorAttribute : DTPropertyAttribute
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "DialogMode")]
	public enum DialogMode
	{
		OpenFile,
		OpenFolder,
		CreateFile
	}

	private static readonly IntPtr NativeFieldInfoPtr_Mode;

	private static readonly IntPtr NativeFieldInfoPtr_Title;

	private static readonly IntPtr NativeFieldInfoPtr_Directory;

	private static readonly IntPtr NativeFieldInfoPtr_Extension;

	private static readonly IntPtr NativeFieldInfoPtr_DefaultName;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_DialogMode_0;

	public unsafe DialogMode Mode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode);
			return *(DialogMode*)num;
		}
		set
		{
			*(DialogMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mode)) = dialogMode;
		}
	}

	public unsafe string Title
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Directory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Directory);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Directory)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Extension
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Extension);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Extension)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string DefaultName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static PathSelectorAttribute()
	{
		Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "PathSelectorAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Mode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, "Mode");
		NativeFieldInfoPtr_Title = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, "Title");
		NativeFieldInfoPtr_Directory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, "Directory");
		NativeFieldInfoPtr_Extension = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, "Extension");
		NativeFieldInfoPtr_DefaultName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, "DefaultName");
		NativeMethodInfoPtr__ctor_Public_Void_DialogMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr, 100664176);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32272, XrefRangeEnd = 32282, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PathSelectorAttribute(DialogMode mode = DialogMode.OpenFile)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathSelectorAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_DialogMode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PathSelectorAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
