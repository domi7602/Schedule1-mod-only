using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

public sealed class CGDataReferenceSelectorAttribute : DTPropertyAttribute
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DataType;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Type_0;

	public unsafe Il2CppSystem.Type DataType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataType)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type));
		}
	}

	static CGDataReferenceSelectorAttribute()
	{
		Il2CppClassPointerStore<CGDataReferenceSelectorAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CGDataReferenceSelectorAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGDataReferenceSelectorAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_DataType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGDataReferenceSelectorAttribute>.NativeClassPtr, "DataType");
		NativeMethodInfoPtr__ctor_Public_Void_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGDataReferenceSelectorAttribute>.NativeClassPtr, 100664435);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34629, XrefRangeEnd = 34635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGDataReferenceSelectorAttribute(Il2CppSystem.Type dataType)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGDataReferenceSelectorAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGDataReferenceSelectorAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
