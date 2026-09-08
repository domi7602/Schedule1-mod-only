using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy;

public sealed class CGResourceCollectionManagerAttribute : CGResourceManagerAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_ShowCount;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe bool ShowCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowCount);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowCount)) = flag;
		}
	}

	static CGResourceCollectionManagerAttribute()
	{
		Il2CppClassPointerStore<CGResourceCollectionManagerAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CGResourceCollectionManagerAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGResourceCollectionManagerAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_ShowCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGResourceCollectionManagerAttribute>.NativeClassPtr, "ShowCount");
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGResourceCollectionManagerAttribute>.NativeClassPtr, 100664436);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34635, XrefRangeEnd = 34641, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGResourceCollectionManagerAttribute(string resourceName)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGResourceCollectionManagerAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGResourceCollectionManagerAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
