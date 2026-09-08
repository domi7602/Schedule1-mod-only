using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Health : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MaxHealth;

	private static readonly System.IntPtr NativeFieldInfoPtr_Invincible;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanRevive;

	private static readonly System.IntPtr NativeFieldInfoPtr_DaysToRevive;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Health_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float MaxHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxHealth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxHealth)) = num;
		}
	}

	public unsafe bool Invincible
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Invincible);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Invincible)) = flag;
		}
	}

	public unsafe bool CanRevive
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanRevive);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanRevive)) = flag;
		}
	}

	public unsafe int DaysToRevive
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysToRevive);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DaysToRevive)) = num;
		}
	}

	static Health()
	{
		Il2CppClassPointerStore<Health>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Health");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Health>.NativeClassPtr);
		NativeFieldInfoPtr_MaxHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Health>.NativeClassPtr, "MaxHealth");
		NativeFieldInfoPtr_Invincible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Health>.NativeClassPtr, "Invincible");
		NativeFieldInfoPtr_CanRevive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Health>.NativeClassPtr, "CanRevive");
		NativeFieldInfoPtr_DaysToRevive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Health>.NativeClassPtr, "DaysToRevive");
		NativeMethodInfoPtr_GetCopy_Public_Health_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Health>.NativeClassPtr, 100682809);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Health>.NativeClassPtr, 100682810);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272240, XrefRangeEnd = 272244, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Health GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Health_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Health>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272244, XrefRangeEnd = 272245, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Health()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Health>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Health(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
