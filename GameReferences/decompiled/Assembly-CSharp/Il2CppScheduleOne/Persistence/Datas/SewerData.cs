using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

public class SewerData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_IsSewerUnlocked;

	private static readonly IntPtr NativeFieldInfoPtr_IsRandomWorldKeyCollected;

	private static readonly IntPtr NativeFieldInfoPtr_RandomSewerKeyLocationIndex;

	private static readonly IntPtr NativeFieldInfoPtr_HasSewerKingBeenDefeated;

	private static readonly IntPtr NativeFieldInfoPtr_HoursSinceLastSewerGoblinAppearance;

	private static readonly IntPtr NativeFieldInfoPtr_RandomKeyPossessorIndex;

	private static readonly IntPtr NativeFieldInfoPtr_RandomKeyPossessorSet;

	private static readonly IntPtr NativeFieldInfoPtr_ActiveMushroomLocationIndices;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Int32_Boolean_Int32_Int32_List_1_Int32_0;

	public unsafe bool IsSewerUnlocked
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsSewerUnlocked);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsSewerUnlocked)) = flag;
		}
	}

	public unsafe bool IsRandomWorldKeyCollected
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsRandomWorldKeyCollected);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsRandomWorldKeyCollected)) = flag;
		}
	}

	public unsafe int RandomSewerKeyLocationIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomSewerKeyLocationIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomSewerKeyLocationIndex)) = num;
		}
	}

	public unsafe bool HasSewerKingBeenDefeated
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasSewerKingBeenDefeated);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasSewerKingBeenDefeated)) = flag;
		}
	}

	public unsafe int HoursSinceLastSewerGoblinAppearance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceLastSewerGoblinAppearance);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceLastSewerGoblinAppearance)) = num;
		}
	}

	public unsafe int RandomKeyPossessorIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomKeyPossessorIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomKeyPossessorIndex)) = num;
		}
	}

	public unsafe bool RandomKeyPossessorSet
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomKeyPossessorSet);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RandomKeyPossessorSet)) = flag;
		}
	}

	public unsafe List<int> ActiveMushroomLocationIndices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMushroomLocationIndices);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<int>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMushroomLocationIndices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static SewerData()
	{
		Il2CppClassPointerStore<SewerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "SewerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SewerData>.NativeClassPtr);
		NativeFieldInfoPtr_IsSewerUnlocked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "IsSewerUnlocked");
		NativeFieldInfoPtr_IsRandomWorldKeyCollected = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "IsRandomWorldKeyCollected");
		NativeFieldInfoPtr_RandomSewerKeyLocationIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "RandomSewerKeyLocationIndex");
		NativeFieldInfoPtr_HasSewerKingBeenDefeated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "HasSewerKingBeenDefeated");
		NativeFieldInfoPtr_HoursSinceLastSewerGoblinAppearance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "HoursSinceLastSewerGoblinAppearance");
		NativeFieldInfoPtr_RandomKeyPossessorIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "RandomKeyPossessorIndex");
		NativeFieldInfoPtr_RandomKeyPossessorSet = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "RandomKeyPossessorSet");
		NativeFieldInfoPtr_ActiveMushroomLocationIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SewerData>.NativeClassPtr, "ActiveMushroomLocationIndices");
		NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Int32_Boolean_Int32_Int32_List_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SewerData>.NativeClassPtr, 100669465);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 135453, RefRangeEnd = 135455, XrefRangeStart = 135444, XrefRangeEnd = 135453, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SewerData(bool isSewerUnlocked, bool isRandomWorldKeyCollected, int randomSewerKeyLocationIndex, bool hasSewerKingBeenDefeated, int hoursSinceLastSewerGoblinAppearance, int randomKeyPossessorIndex, List<int> activeMushroomLocationIndices)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SewerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = (nint)(&isSewerUnlocked);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &isRandomWorldKeyCollected;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &randomSewerKeyLocationIndex;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &hasSewerKingBeenDefeated;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &hoursSinceLastSewerGoblinAppearance;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &randomKeyPossessorIndex;
		*(IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeMushroomLocationIndices);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Boolean_Boolean_Int32_Boolean_Int32_Int32_List_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SewerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
