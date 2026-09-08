using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[Serializable]
public class CGBoundsGroupItem : CGWeightedItem
{
	private static readonly IntPtr NativeFieldInfoPtr_Index;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int Index
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Index);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Index)) = num;
		}
	}

	static CGBoundsGroupItem()
	{
		Il2CppClassPointerStore<CGBoundsGroupItem>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGBoundsGroupItem");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGBoundsGroupItem>.NativeClassPtr);
		NativeFieldInfoPtr_Index = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBoundsGroupItem>.NativeClassPtr, "Index");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBoundsGroupItem>.NativeClassPtr, 100665771);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 46777, RefRangeEnd = 46779, XrefRangeStart = 46776, XrefRangeEnd = 46777, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGBoundsGroupItem()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGBoundsGroupItem>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGBoundsGroupItem(IntPtr pointer)
		: base(pointer)
	{
	}
}
