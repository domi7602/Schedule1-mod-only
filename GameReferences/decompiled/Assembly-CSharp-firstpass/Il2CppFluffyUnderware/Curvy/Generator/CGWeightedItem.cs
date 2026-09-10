using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[System.Serializable]
public class CGWeightedItem : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_m_Weight;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Weight_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Weight_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float m_Weight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Weight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Weight)) = num;
		}
	}

	public unsafe float Weight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Weight_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52900, XrefRangeEnd = 52901, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Weight_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CGWeightedItem()
	{
		Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGWeightedItem");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr);
		NativeFieldInfoPtr_m_Weight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr, "m_Weight");
		NativeMethodInfoPtr_get_Weight_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr, 100666318);
		NativeMethodInfoPtr_set_Weight_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr, 100666319);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr, 100666320);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 52902, RefRangeEnd = 52904, XrefRangeStart = 52901, XrefRangeEnd = 52902, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGWeightedItem()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGWeightedItem>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGWeightedItem(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
