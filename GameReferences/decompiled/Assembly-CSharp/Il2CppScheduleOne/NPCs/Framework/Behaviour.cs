using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Behaviour : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_IgnorePhysicsImpacts;

	private static readonly System.IntPtr NativeFieldInfoPtr_IgnoreCombatImpacts;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultAggression;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanCallPolice;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Behaviour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool IgnorePhysicsImpacts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnorePhysicsImpacts);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnorePhysicsImpacts)) = flag;
		}
	}

	public unsafe bool IgnoreCombatImpacts
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnoreCombatImpacts);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IgnoreCombatImpacts)) = flag;
		}
	}

	public unsafe float DefaultAggression
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultAggression);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DefaultAggression)) = num;
		}
	}

	public unsafe bool CanCallPolice
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanCallPolice);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanCallPolice)) = flag;
		}
	}

	static Behaviour()
	{
		Il2CppClassPointerStore<Behaviour>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Behaviour");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Behaviour>.NativeClassPtr);
		NativeFieldInfoPtr_IgnorePhysicsImpacts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, "IgnorePhysicsImpacts");
		NativeFieldInfoPtr_IgnoreCombatImpacts = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, "IgnoreCombatImpacts");
		NativeFieldInfoPtr_DefaultAggression = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, "DefaultAggression");
		NativeFieldInfoPtr_CanCallPolice = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, "CanCallPolice");
		NativeMethodInfoPtr_GetCopy_Public_Behaviour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, 100682801);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Behaviour>.NativeClassPtr, 100682802);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272224, XrefRangeEnd = 272228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Behaviour GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Behaviour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Behaviour>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272228, XrefRangeEnd = 272229, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Behaviour()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Behaviour>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Behaviour(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
