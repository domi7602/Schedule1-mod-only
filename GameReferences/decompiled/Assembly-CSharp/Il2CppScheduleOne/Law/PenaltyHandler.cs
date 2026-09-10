using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Law;

public static class PenaltyHandler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_CONTROLLED_SUBSTANCE_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_LOW_SEVERITY_DRUG_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_MED_SEVERITY_DRUG_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_HIGH_SEVERITY_DRUG_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_FAILURE_TO_COMPLY_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_EVADING_ARREST_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_VIOLATING_CURFEW_TIME;

	private static readonly System.IntPtr NativeFieldInfoPtr_ATTEMPT_TO_SELL_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_ASSAULT_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_DEADLY_ASSAULT_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_VANDALISM_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_THEFT_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_BRANDISHING_FINE;

	private static readonly System.IntPtr NativeFieldInfoPtr_DISCHARGE_FIREARM_FINE;

	private static readonly System.IntPtr NativeMethodInfoPtr_ProcessCrimeList_Public_Static_List_1_String_Dictionary_2_Crime_Int32_0;

	public unsafe static float CONTROLLED_SUBSTANCE_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CONTROLLED_SUBSTANCE_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CONTROLLED_SUBSTANCE_FINE, (void*)(&num));
		}
	}

	public unsafe static float LOW_SEVERITY_DRUG_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LOW_SEVERITY_DRUG_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LOW_SEVERITY_DRUG_FINE, (void*)(&num));
		}
	}

	public unsafe static float MED_SEVERITY_DRUG_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MED_SEVERITY_DRUG_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MED_SEVERITY_DRUG_FINE, (void*)(&num));
		}
	}

	public unsafe static float HIGH_SEVERITY_DRUG_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HIGH_SEVERITY_DRUG_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HIGH_SEVERITY_DRUG_FINE, (void*)(&num));
		}
	}

	public unsafe static float FAILURE_TO_COMPLY_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FAILURE_TO_COMPLY_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FAILURE_TO_COMPLY_FINE, (void*)(&num));
		}
	}

	public unsafe static float EVADING_ARREST_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EVADING_ARREST_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EVADING_ARREST_FINE, (void*)(&num));
		}
	}

	public unsafe static float VIOLATING_CURFEW_TIME
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VIOLATING_CURFEW_TIME, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VIOLATING_CURFEW_TIME, (void*)(&num));
		}
	}

	public unsafe static float ATTEMPT_TO_SELL_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ATTEMPT_TO_SELL_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ATTEMPT_TO_SELL_FINE, (void*)(&num));
		}
	}

	public unsafe static float ASSAULT_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ASSAULT_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ASSAULT_FINE, (void*)(&num));
		}
	}

	public unsafe static float DEADLY_ASSAULT_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DEADLY_ASSAULT_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DEADLY_ASSAULT_FINE, (void*)(&num));
		}
	}

	public unsafe static float VANDALISM_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VANDALISM_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VANDALISM_FINE, (void*)(&num));
		}
	}

	public unsafe static float THEFT_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_THEFT_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_THEFT_FINE, (void*)(&num));
		}
	}

	public unsafe static float BRANDISHING_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BRANDISHING_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BRANDISHING_FINE, (void*)(&num));
		}
	}

	public unsafe static float DISCHARGE_FIREARM_FINE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DISCHARGE_FIREARM_FINE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DISCHARGE_FIREARM_FINE, (void*)(&num));
		}
	}

	static PenaltyHandler()
	{
		Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Law", "PenaltyHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr);
		NativeFieldInfoPtr_CONTROLLED_SUBSTANCE_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "CONTROLLED_SUBSTANCE_FINE");
		NativeFieldInfoPtr_LOW_SEVERITY_DRUG_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "LOW_SEVERITY_DRUG_FINE");
		NativeFieldInfoPtr_MED_SEVERITY_DRUG_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "MED_SEVERITY_DRUG_FINE");
		NativeFieldInfoPtr_HIGH_SEVERITY_DRUG_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "HIGH_SEVERITY_DRUG_FINE");
		NativeFieldInfoPtr_FAILURE_TO_COMPLY_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "FAILURE_TO_COMPLY_FINE");
		NativeFieldInfoPtr_EVADING_ARREST_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "EVADING_ARREST_FINE");
		NativeFieldInfoPtr_VIOLATING_CURFEW_TIME = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "VIOLATING_CURFEW_TIME");
		NativeFieldInfoPtr_ATTEMPT_TO_SELL_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "ATTEMPT_TO_SELL_FINE");
		NativeFieldInfoPtr_ASSAULT_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "ASSAULT_FINE");
		NativeFieldInfoPtr_DEADLY_ASSAULT_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "DEADLY_ASSAULT_FINE");
		NativeFieldInfoPtr_VANDALISM_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "VANDALISM_FINE");
		NativeFieldInfoPtr_THEFT_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "THEFT_FINE");
		NativeFieldInfoPtr_BRANDISHING_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "BRANDISHING_FINE");
		NativeFieldInfoPtr_DISCHARGE_FIREARM_FINE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, "DISCHARGE_FIREARM_FINE");
		NativeMethodInfoPtr_ProcessCrimeList_Public_Static_List_1_String_Dictionary_2_Crime_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PenaltyHandler>.NativeClassPtr, 100671283);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 153116, RefRangeEnd = 153117, XrefRangeStart = 153000, XrefRangeEnd = 153116, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<string> ProcessCrimeList(Dictionary<Crime, int> crimes)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)crimes);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessCrimeList_Public_Static_List_1_String_Dictionary_2_Crime_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
	}

	public PenaltyHandler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
