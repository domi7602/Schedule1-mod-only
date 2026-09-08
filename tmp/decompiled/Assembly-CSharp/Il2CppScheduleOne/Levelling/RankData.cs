using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Levelling;

public class RankData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Rank;

	private static readonly IntPtr NativeFieldInfoPtr_Tier;

	private static readonly IntPtr NativeFieldInfoPtr_XP;

	private static readonly IntPtr NativeFieldInfoPtr_TotalXP;

	private static readonly IntPtr NativeFieldInfoPtr_UnlockedRegions;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_Int32_List_1_EMapRegion_0;

	public unsafe int Rank
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rank);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rank)) = num;
		}
	}

	public unsafe int Tier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tier);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tier)) = num;
		}
	}

	public unsafe int XP
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_XP);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_XP)) = num;
		}
	}

	public unsafe int TotalXP
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalXP);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TotalXP)) = num;
		}
	}

	public unsafe List<EMapRegion> UnlockedRegions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedRegions);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<EMapRegion>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedRegions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static RankData()
	{
		Il2CppClassPointerStore<RankData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Levelling", "RankData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RankData>.NativeClassPtr);
		NativeFieldInfoPtr_Rank = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RankData>.NativeClassPtr, "Rank");
		NativeFieldInfoPtr_Tier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RankData>.NativeClassPtr, "Tier");
		NativeFieldInfoPtr_XP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RankData>.NativeClassPtr, "XP");
		NativeFieldInfoPtr_TotalXP = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RankData>.NativeClassPtr, "TotalXP");
		NativeFieldInfoPtr_UnlockedRegions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RankData>.NativeClassPtr, "UnlockedRegions");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_Int32_List_1_EMapRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RankData>.NativeClassPtr, 100671127);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152033, XrefRangeEnd = 152042, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RankData(int rank, int tier, int xp, int totalXP, List<EMapRegion> unlockedRegions)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RankData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = (nint)(&rank);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &tier;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &xp;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &totalXP;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unlockedRegions);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_Int32_Int32_Int32_List_1_EMapRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RankData(IntPtr pointer)
		: base(pointer)
	{
	}
}
