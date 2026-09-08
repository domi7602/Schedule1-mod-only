using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Interaction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_CanBeSummoned;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Interaction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool CanBeSummoned
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanBeSummoned);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanBeSummoned)) = flag;
		}
	}

	static Interaction()
	{
		Il2CppClassPointerStore<Interaction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Interaction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Interaction>.NativeClassPtr);
		NativeFieldInfoPtr_CanBeSummoned = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Interaction>.NativeClassPtr, "CanBeSummoned");
		NativeMethodInfoPtr_GetCopy_Public_Interaction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Interaction>.NativeClassPtr, 100682813);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Interaction>.NativeClassPtr, 100682814);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272248, XrefRangeEnd = 272252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Interaction GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Interaction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Interaction>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 197152, RefRangeEnd = 197154, XrefRangeStart = 197152, XrefRangeEnd = 197154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Interaction()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Interaction>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Interaction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
