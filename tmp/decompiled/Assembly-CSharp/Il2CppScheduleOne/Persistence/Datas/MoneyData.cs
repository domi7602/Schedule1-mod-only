using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class MoneyData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_OnlineBalance;

	private static readonly IntPtr NativeFieldInfoPtr_Networth;

	private static readonly IntPtr NativeFieldInfoPtr_LifetimeEarnings;

	private static readonly IntPtr NativeFieldInfoPtr_WeeklyDepositSum;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Single_0;

	public unsafe float OnlineBalance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlineBalance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnlineBalance)) = num;
		}
	}

	public unsafe float Networth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Networth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Networth)) = num;
		}
	}

	public unsafe float LifetimeEarnings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LifetimeEarnings);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LifetimeEarnings)) = num;
		}
	}

	public unsafe float WeeklyDepositSum
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WeeklyDepositSum);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WeeklyDepositSum)) = num;
		}
	}

	static MoneyData()
	{
		Il2CppClassPointerStore<MoneyData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "MoneyData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MoneyData>.NativeClassPtr);
		NativeFieldInfoPtr_OnlineBalance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoneyData>.NativeClassPtr, "OnlineBalance");
		NativeFieldInfoPtr_Networth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoneyData>.NativeClassPtr, "Networth");
		NativeFieldInfoPtr_LifetimeEarnings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoneyData>.NativeClassPtr, "LifetimeEarnings");
		NativeFieldInfoPtr_WeeklyDepositSum = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MoneyData>.NativeClassPtr, "WeeklyDepositSum");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MoneyData>.NativeClassPtr, 100669404);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134988, RefRangeEnd = 134989, XrefRangeStart = 134987, XrefRangeEnd = 134988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MoneyData(float onlineBalance, float netWorth, float lifetimeEarnings, float weeklyDepositSum)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MoneyData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&onlineBalance);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &netWorth;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &lifetimeEarnings;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &weeklyDepositSum;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MoneyData(IntPtr pointer)
		: base(pointer)
	{
	}
}
