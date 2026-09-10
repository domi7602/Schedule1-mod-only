using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.NPCs.Behaviour;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Police;
using Il2CppSystem;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Law;

public class LawManager : Singleton<LawManager>
{
	[System.Serializable]
	[ObfuscatedName("ScheduleOne.Law.LawManager+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Start_b__3_0_Internal_Void_0;

		public unsafe static __c __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c));
			}
		}

		public unsafe static UnityAction __9__3_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__3_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityAction>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__3_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityAction));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LawManager>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__3_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__3_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100671274);
			NativeMethodInfoPtr__Start_b__3_0_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100671275);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152822, XrefRangeEnd = 152828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _Start_b__3_0()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Start_b__3_0_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_OfficerDispatchMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_OfficerDispatchMax;

	private static readonly System.IntPtr NativeFieldInfoPtr_DISPATCH_VEHICLE_USE_THRESHOLD;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PoliceCalled_Public_Void_Player_Crime_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartFootpatrol_Public_PatrolGroup_FootPatrolRoute_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartVehiclePatrol_Public_PoliceOfficer_VehiclePatrolRoute_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static int OfficerDispatchMin
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OfficerDispatchMin, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OfficerDispatchMin, (void*)(&num));
		}
	}

	public unsafe static int OfficerDispatchMax
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OfficerDispatchMax, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OfficerDispatchMax, (void*)(&num));
		}
	}

	public unsafe static float DISPATCH_VEHICLE_USE_THRESHOLD
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DISPATCH_VEHICLE_USE_THRESHOLD, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DISPATCH_VEHICLE_USE_THRESHOLD, (void*)(&num));
		}
	}

	static LawManager()
	{
		Il2CppClassPointerStore<LawManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Law", "LawManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LawManager>.NativeClassPtr);
		NativeFieldInfoPtr_OfficerDispatchMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LawManager>.NativeClassPtr, "OfficerDispatchMin");
		NativeFieldInfoPtr_OfficerDispatchMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LawManager>.NativeClassPtr, "OfficerDispatchMax");
		NativeFieldInfoPtr_DISPATCH_VEHICLE_USE_THRESHOLD = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LawManager>.NativeClassPtr, "DISPATCH_VEHICLE_USE_THRESHOLD");
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawManager>.NativeClassPtr, 100671267);
		NativeMethodInfoPtr_PoliceCalled_Public_Void_Player_Crime_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawManager>.NativeClassPtr, 100671268);
		NativeMethodInfoPtr_StartFootpatrol_Public_PatrolGroup_FootPatrolRoute_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawManager>.NativeClassPtr, 100671269);
		NativeMethodInfoPtr_StartVehiclePatrol_Public_PoliceOfficer_VehiclePatrolRoute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawManager>.NativeClassPtr, 100671270);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LawManager>.NativeClassPtr, 100671271);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152828, XrefRangeEnd = 152854, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 152875, RefRangeEnd = 152878, XrefRangeStart = 152854, XrefRangeEnd = 152875, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PoliceCalled(Player target, Crime crime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)crime);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PoliceCalled_Public_Void_Player_Crime_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 152906, RefRangeEnd = 152907, XrefRangeStart = 152878, XrefRangeEnd = 152906, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PatrolGroup StartFootpatrol(FootPatrolRoute route, int requestedMembers)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)route);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &requestedMembers;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartFootpatrol_Public_PatrolGroup_FootPatrolRoute_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PatrolGroup>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 152921, RefRangeEnd = 152923, XrefRangeStart = 152907, XrefRangeEnd = 152921, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PoliceOfficer StartVehiclePatrol(VehiclePatrolRoute route)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)route);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartVehiclePatrol_Public_PoliceOfficer_VehiclePatrolRoute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PoliceOfficer>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 152923, XrefRangeEnd = 152926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LawManager()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LawManager>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public LawManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
