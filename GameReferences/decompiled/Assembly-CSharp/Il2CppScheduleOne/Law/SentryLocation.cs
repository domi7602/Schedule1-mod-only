using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Police;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.Law;

public class SentryLocation : MonoBehaviour
{
	[System.Serializable]
	public class SentryRoute : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_RoutePoints;

		private static readonly System.IntPtr NativeFieldInfoPtr_MinutesPerPoint;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Il2CppReferenceArray<Transform> RoutePoints
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RoutePoints);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Transform>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RoutePoints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe int MinutesPerPoint
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinutesPerPoint);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MinutesPerPoint)) = num;
			}
		}

		static SentryRoute()
		{
			Il2CppClassPointerStore<SentryRoute>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, "SentryRoute");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SentryRoute>.NativeClassPtr);
			NativeFieldInfoPtr_RoutePoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SentryRoute>.NativeClassPtr, "RoutePoints");
			NativeFieldInfoPtr_MinutesPerPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SentryRoute>.NativeClassPtr, "MinutesPerPoint");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SentryRoute>.NativeClassPtr, 100671293);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153230, XrefRangeEnd = 153231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SentryRoute()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SentryRoute>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SentryRoute(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Routes;

	private static readonly System.IntPtr NativeFieldInfoPtr__AssignedOfficers_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AssignedOfficers_Public_get_List_1_PoliceOfficer_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AssignedOfficers_Private_set_Void_List_1_PoliceOfficer_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe List<SentryRoute> Routes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Routes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SentryRoute>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Routes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<PoliceOfficer> _AssignedOfficers_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AssignedOfficers_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PoliceOfficer>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AssignedOfficers_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<PoliceOfficer> AssignedOfficers
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AssignedOfficers_Public_get_List_1_PoliceOfficer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PoliceOfficer>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AssignedOfficers_Private_set_Void_List_1_PoliceOfficer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SentryLocation()
	{
		Il2CppClassPointerStore<SentryLocation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Law", "SentryLocation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr);
		NativeFieldInfoPtr_Routes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, "Routes");
		NativeFieldInfoPtr__AssignedOfficers_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, "<AssignedOfficers>k__BackingField");
		NativeMethodInfoPtr_get_AssignedOfficers_Public_get_List_1_PoliceOfficer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, 100671290);
		NativeMethodInfoPtr_set_AssignedOfficers_Private_set_Void_List_1_PoliceOfficer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, 100671291);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr, 100671292);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 153231, XrefRangeEnd = 153246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SentryLocation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SentryLocation>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SentryLocation(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
