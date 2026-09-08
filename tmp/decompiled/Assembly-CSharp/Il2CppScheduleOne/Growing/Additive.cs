using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using UnityEngine;

namespace Il2CppScheduleOne.Growing;

public class Additive : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_AdditiveName;

	private static readonly IntPtr NativeFieldInfoPtr_Definition;

	private static readonly IntPtr NativeFieldInfoPtr_QualityChange;

	private static readonly IntPtr NativeFieldInfoPtr_YieldChange;

	private static readonly IntPtr NativeFieldInfoPtr_GrowSpeedMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr_InstantGrowth;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string AdditiveName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AdditiveName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AdditiveName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe AdditiveDefinition Definition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Definition);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AdditiveDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Definition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additiveDefinition));
		}
	}

	public unsafe float QualityChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QualityChange);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_QualityChange)) = num;
		}
	}

	public unsafe float YieldChange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_YieldChange);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_YieldChange)) = num;
		}
	}

	public unsafe float GrowSpeedMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowSpeedMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowSpeedMultiplier)) = num;
		}
	}

	public unsafe float InstantGrowth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InstantGrowth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InstantGrowth)) = num;
		}
	}

	static Additive()
	{
		Il2CppClassPointerStore<Additive>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "Additive");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Additive>.NativeClassPtr);
		NativeFieldInfoPtr_AdditiveName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "AdditiveName");
		NativeFieldInfoPtr_Definition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "Definition");
		NativeFieldInfoPtr_QualityChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "QualityChange");
		NativeFieldInfoPtr_YieldChange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "YieldChange");
		NativeFieldInfoPtr_GrowSpeedMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "GrowSpeedMultiplier");
		NativeFieldInfoPtr_InstantGrowth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Additive>.NativeClassPtr, "InstantGrowth");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Additive>.NativeClassPtr, 100678337);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 228588, XrefRangeEnd = 228593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Additive()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Additive>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Additive(IntPtr pointer)
		: base(pointer)
	{
	}
}
