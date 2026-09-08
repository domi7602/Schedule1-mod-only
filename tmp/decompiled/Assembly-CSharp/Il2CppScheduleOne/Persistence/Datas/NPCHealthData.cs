using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Persistence.Datas;

public class NPCHealthData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Health;

	private static readonly IntPtr NativeFieldInfoPtr_IsDead;

	private static readonly IntPtr NativeFieldInfoPtr_DaysPassedSinceDeath;

	private static readonly IntPtr NativeFieldInfoPtr_HoursSinceAttackedByPlayer;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_Int32_Int32_0;

	public unsafe float Health
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Health);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Health)) = num;
		}
	}

	public unsafe bool IsDead
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsDead);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IsDead)) = flag;
		}
	}

	public unsafe int DaysPassedSinceDeath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysPassedSinceDeath);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysPassedSinceDeath)) = num;
		}
	}

	public unsafe int HoursSinceAttackedByPlayer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceAttackedByPlayer);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HoursSinceAttackedByPlayer)) = num;
		}
	}

	static NPCHealthData()
	{
		Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "NPCHealthData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr);
		NativeFieldInfoPtr_Health = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr, "Health");
		NativeFieldInfoPtr_IsDead = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr, "IsDead");
		NativeFieldInfoPtr_DaysPassedSinceDeath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr, "DaysPassedSinceDeath");
		NativeFieldInfoPtr_HoursSinceAttackedByPlayer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr, "HoursSinceAttackedByPlayer");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr, 100669409);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135004, RefRangeEnd = 135005, XrefRangeStart = 135003, XrefRangeEnd = 135004, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NPCHealthData(float health, bool isDead, int daysPassedSinceDeath, int hoursSinceAttackedByPlayer)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NPCHealthData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&health);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &isDead;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &daysPassedSinceDeath;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &hoursSinceAttackedByPlayer;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Boolean_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NPCHealthData(IntPtr pointer)
		: base(pointer)
	{
	}
}
