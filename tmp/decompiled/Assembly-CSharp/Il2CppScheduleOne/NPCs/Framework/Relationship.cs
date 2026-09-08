using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Relationship : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultRelationshipValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_DisplayRelationshipValue;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Relationship_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float DefaultRelationshipValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultRelationshipValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultRelationshipValue)) = num;
		}
	}

	public unsafe bool DisplayRelationshipValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayRelationshipValue);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayRelationshipValue)) = flag;
		}
	}

	static Relationship()
	{
		Il2CppClassPointerStore<Relationship>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Relationship");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Relationship>.NativeClassPtr);
		NativeFieldInfoPtr_DefaultRelationshipValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Relationship>.NativeClassPtr, "DefaultRelationshipValue");
		NativeFieldInfoPtr_DisplayRelationshipValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Relationship>.NativeClassPtr, "DisplayRelationshipValue");
		NativeMethodInfoPtr_GetCopy_Public_Relationship_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Relationship>.NativeClassPtr, 100682830);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Relationship>.NativeClassPtr, 100682831);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272301, XrefRangeEnd = 272305, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Relationship GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Relationship_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Relationship>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272305, XrefRangeEnd = 272306, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Relationship()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Relationship>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Relationship(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
