using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.NPCs.CharacterClasses;

namespace Il2CppScheduleOne.Quests;

public class Quest_GettingStarted : Quest
{
	private static readonly IntPtr NativeFieldInfoPtr_CashAmount;

	private static readonly IntPtr NativeFieldInfoPtr_CashDrop;

	private static readonly IntPtr NativeFieldInfoPtr_Nelson;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float CashAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashAmount);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashAmount)) = num;
		}
	}

	public unsafe DeadDrop CashDrop
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashDrop);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DeadDrop>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CashDrop)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)deadDrop));
		}
	}

	public unsafe UncleNelson Nelson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Nelson);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UncleNelson>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Nelson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uncleNelson));
		}
	}

	static Quest_GettingStarted()
	{
		Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Quests", "Quest_GettingStarted");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr);
		NativeFieldInfoPtr_CashAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr, "CashAmount");
		NativeFieldInfoPtr_CashDrop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr, "CashDrop");
		NativeFieldInfoPtr_Nelson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr, "Nelson");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr, 100667671);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 110962, XrefRangeEnd = 110966, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quest_GettingStarted()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Quest_GettingStarted>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Quest_GettingStarted(IntPtr pointer)
		: base(pointer)
	{
	}
}
