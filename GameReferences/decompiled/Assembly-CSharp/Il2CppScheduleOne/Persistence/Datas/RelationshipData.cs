using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.NPCs.Relation;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class RelationshipData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_RelationDelta;

	private static readonly IntPtr NativeFieldInfoPtr_Unlocked;

	private static readonly IntPtr NativeFieldInfoPtr_UnlockType;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_EUnlockType_0;

	public unsafe float RelationDelta
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelationDelta);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RelationDelta)) = num;
		}
	}

	public unsafe bool Unlocked
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Unlocked);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Unlocked)) = flag;
		}
	}

	public unsafe NPCRelationData.EUnlockType UnlockType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockType);
			return *(NPCRelationData.EUnlockType*)num;
		}
		set
		{
			*(NPCRelationData.EUnlockType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockType)) = eUnlockType;
		}
	}

	static RelationshipData()
	{
		Il2CppClassPointerStore<RelationshipData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "RelationshipData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr);
		NativeFieldInfoPtr_RelationDelta = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr, "RelationDelta");
		NativeFieldInfoPtr_Unlocked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr, "Unlocked");
		NativeFieldInfoPtr_UnlockType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr, "UnlockType");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_EUnlockType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr, 100669369);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134778, RefRangeEnd = 134779, XrefRangeStart = 134777, XrefRangeEnd = 134778, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RelationshipData(float relationDelta, bool unlocked, NPCRelationData.EUnlockType unlockType)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RelationshipData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&relationDelta);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &unlocked;
		*(NPCRelationData.EUnlockType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &unlockType;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_EUnlockType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RelationshipData(IntPtr pointer)
		: base(pointer)
	{
	}
}
