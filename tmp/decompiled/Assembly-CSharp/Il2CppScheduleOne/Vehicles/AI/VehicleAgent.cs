using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppPathfinding;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Math;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;

namespace Il2CppScheduleOne.Vehicles.AI;

public class VehicleAgent : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "ENavigationResult")]
	public enum ENavigationResult
	{
		Failed,
		Complete,
		Stopped
	}

	[OriginalName("Assembly-CSharp.dll", "", "EAgentStatus")]
	public enum EAgentStatus
	{
		Inactive,
		MovingToRoad,
		OnRoad
	}

	[OriginalName("Assembly-CSharp.dll", "", "EPathGroupStatus")]
	public enum EPathGroupStatus
	{
		Inactive,
		Calculating
	}

	[OriginalName("Assembly-CSharp.dll", "", "ESweepType")]
	public enum ESweepType
	{
		FL,
		FR,
		RL,
		RR
	}

	public sealed class NavigationCallback : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ENavigationResult_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ENavigationResult_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static NavigationCallback()
		{
			Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "NavigationCallback");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr, 100666487);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ENavigationResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr, 100666488);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ENavigationResult_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr, 100666489);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr, 100666490);
		}

		[CallerCount(152)]
		[CachedScanResults(RefRangeStart = 95930, RefRangeEnd = 96082, XrefRangeStart = 95927, XrefRangeEnd = 95930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe NavigationCallback(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NavigationCallback>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual void Invoke(ENavigationResult status)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&status);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ENavigationResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96082, XrefRangeEnd = 96086, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(ENavigationResult status, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&status);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ENavigationResult_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public NavigationCallback(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator NavigationCallback(System.Action<ENavigationResult> P_0)
		{
			return DelegateSupport.ConvertDelegate<NavigationCallback>((System.Delegate)P_0);
		}

		public static NavigationCallback operator +(NavigationCallback P_0, NavigationCallback P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<NavigationCallback>();
		}

		public static NavigationCallback operator -(NavigationCallback P_0, NavigationCallback P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<NavigationCallback>();
			}
			return (NavigationCallback)obj;
		}
	}

	[System.Serializable]
	[ObfuscatedName("ScheduleOne.Vehicles.AI.VehicleAgent+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__123_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__UpdateKinematic_b__123_0_Internal_Single_RaycastHit_0;

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

		public unsafe static Il2CppSystem.Func<RaycastHit, float> __9__123_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__123_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<RaycastHit, float>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__123_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__123_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__123_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666492);
			NativeMethodInfoPtr__UpdateKinematic_b__123_0_Internal_Single_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666493);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96086, XrefRangeEnd = 96087, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe float _UpdateKinematic_b__123_0(RaycastHit h)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&h);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__UpdateKinematic_b__123_0_Internal_Single_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.Vehicles.AI.VehicleAgent+<>c__DisplayClass139_0")]
	public sealed class __c__DisplayClass139_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_castStart;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SweepTurn_b__0_Internal_Single_RaycastHit_0;

		public unsafe Vector3 castStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_castStart);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_castStart)) = vector;
			}
		}

		public unsafe Il2CppSystem.Func<RaycastHit, float> __9__0
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<RaycastHit, float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c__DisplayClass139_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<>c__DisplayClass139_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr);
			NativeFieldInfoPtr_castStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr, "castStart");
			NativeFieldInfoPtr___9__0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr, "<>9__0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr, 100666494);
			NativeMethodInfoPtr__SweepTurn_b__0_Internal_Single_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr, 100666495);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass139_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass139_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96087, XrefRangeEnd = 96093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe float _SweepTurn_b__0(RaycastHit x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SweepTurn_b__0_Internal_Single_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass139_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.Vehicles.AI.VehicleAgent+<>c__DisplayClass140_0")]
	public sealed class __c__DisplayClass140_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_castStart;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__BetterSweepTurn_b__0_Internal_Single_RaycastHit_0;

		public unsafe Vector3 castStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_castStart);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_castStart)) = vector;
			}
		}

		public unsafe Il2CppSystem.Func<RaycastHit, float> __9__0
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<RaycastHit, float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___9__0)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c__DisplayClass140_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<>c__DisplayClass140_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr);
			NativeFieldInfoPtr_castStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr, "castStart");
			NativeFieldInfoPtr___9__0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr, "<>9__0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr, 100666496);
			NativeMethodInfoPtr__BetterSweepTurn_b__0_Internal_Single_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr, 100666497);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass140_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass140_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe float _BetterSweepTurn_b__0(RaycastHit x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__BetterSweepTurn_b__0_Internal_Single_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass140_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.Vehicles.AI.VehicleAgent+<Reverse>d__142")]
	public sealed class _Reverse_d__142 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr___4__this;

		private static readonly System.IntPtr NativeFieldInfoPtr__futureTarget_5__2;

		private static readonly System.IntPtr NativeFieldInfoPtr__steerAngleNormal_5__3;

		private static readonly System.IntPtr NativeFieldInfoPtr__frontWheel_5__4;

		private static readonly System.IntPtr NativeFieldInfoPtr__sweepAngle_5__5;

		private static readonly System.IntPtr NativeFieldInfoPtr__reverseSweepDistanceMin_5__6;

		private static readonly System.IntPtr NativeFieldInfoPtr__canBeginSwing_5__7;

		private static readonly System.IntPtr NativeFieldInfoPtr__faceTarget_5__8;

		private static readonly System.IntPtr NativeFieldInfoPtr__continueReversing_5__9;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe Il2CppSystem.Object __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe VehicleAgent __4__this
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VehicleAgent>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleAgent));
			}
		}

		public unsafe Vector3 _futureTarget_5__2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__futureTarget_5__2);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__futureTarget_5__2)) = vector;
			}
		}

		public unsafe float _steerAngleNormal_5__3
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steerAngleNormal_5__3);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__steerAngleNormal_5__3)) = num;
			}
		}

		public unsafe ESweepType _frontWheel_5__4
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frontWheel_5__4);
				return *(ESweepType*)num;
			}
			set
			{
				*(ESweepType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frontWheel_5__4)) = eSweepType;
			}
		}

		public unsafe float _sweepAngle_5__5
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sweepAngle_5__5);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sweepAngle_5__5)) = num;
			}
		}

		public unsafe float _reverseSweepDistanceMin_5__6
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reverseSweepDistanceMin_5__6);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__reverseSweepDistanceMin_5__6)) = num;
			}
		}

		public unsafe bool _canBeginSwing_5__7
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canBeginSwing_5__7);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canBeginSwing_5__7)) = flag;
			}
		}

		public unsafe Vector3 _faceTarget_5__8
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__faceTarget_5__8);
				return *(Vector3*)num;
			}
			set
			{
				*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__faceTarget_5__8)) = vector;
			}
		}

		public unsafe bool _continueReversing_5__9
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__continueReversing_5__9);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__continueReversing_5__9)) = flag;
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EGeneric_002EIEnumerator_003CSystem_002EObject_003E_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _Reverse_d__142()
		{
			Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<Reverse>d__142");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<>4__this");
			NativeFieldInfoPtr__futureTarget_5__2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<futureTarget>5__2");
			NativeFieldInfoPtr__steerAngleNormal_5__3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<steerAngleNormal>5__3");
			NativeFieldInfoPtr__frontWheel_5__4 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<frontWheel>5__4");
			NativeFieldInfoPtr__sweepAngle_5__5 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<sweepAngle>5__5");
			NativeFieldInfoPtr__reverseSweepDistanceMin_5__6 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<reverseSweepDistanceMin>5__6");
			NativeFieldInfoPtr__canBeginSwing_5__7 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<canBeginSwing>5__7");
			NativeFieldInfoPtr__faceTarget_5__8 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<faceTarget>5__8");
			NativeFieldInfoPtr__continueReversing_5__9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, "<continueReversing>5__9");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666498);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666499);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666500);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666501);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666502);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr, 100666503);
		}

		[CallerCount(83)]
		[CachedScanResults(RefRangeStart = 65267, RefRangeEnd = 65350, XrefRangeStart = 65267, XrefRangeEnd = 65350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _Reverse_d__142(int _003C_003E1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_Reverse_d__142>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&_003C_003E1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96093, XrefRangeEnd = 96113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96113, XrefRangeEnd = 96118, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public _Reverse_d__142(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_VehicleGraphName;

	private static readonly System.IntPtr NativeFieldInfoPtr_RoadGraphName;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDistanceFromPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDistanceFromPathWhenReversing;

	private static readonly System.IntPtr NativeFieldInfoPtr_MainGraphSamplePoint;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinRenavigationRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_Steer_P;

	private static readonly System.IntPtr NativeFieldInfoPtr_Steer_I;

	private static readonly System.IntPtr NativeFieldInfoPtr_Steer_D;

	private static readonly System.IntPtr NativeFieldInfoPtr_Throttle_P;

	private static readonly System.IntPtr NativeFieldInfoPtr_Throttle_I;

	private static readonly System.IntPtr NativeFieldInfoPtr_Throttle_D;

	private static readonly System.IntPtr NativeFieldInfoPtr_Steer_Rate;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxAxlePositionShift;

	private static readonly System.IntPtr NativeFieldInfoPtr_OBSTACLE_MIN_RANGE;

	private static readonly System.IntPtr NativeFieldInfoPtr_OBSTACLE_MAX_RANGE;

	private static readonly System.IntPtr NativeFieldInfoPtr_MAX_STEER_ANGLE_OVERRIDE;

	private static readonly System.IntPtr NativeFieldInfoPtr_INFREQUENT_UPDATE_RATE;

	private static readonly System.IntPtr NativeFieldInfoPtr_KinematicModeRotationSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_KinematicModeSpeedMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr_DEBUG_MODE;

	private static readonly System.IntPtr NativeFieldInfoPtr__AutoDriving_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__TargetLocation_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_Flags;

	private static readonly System.IntPtr NativeFieldInfoPtr_roadSeeker;

	private static readonly System.IntPtr NativeFieldInfoPtr_generalSeeker;

	private static readonly System.IntPtr NativeFieldInfoPtr_CTE_Origin;

	private static readonly System.IntPtr NativeFieldInfoPtr_FrontAxlePosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_RearAxlePosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensor_FL;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensor_FM;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensor_FR;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensor_RR;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensor_RL;

	private static readonly System.IntPtr NativeFieldInfoPtr_sensors;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepOrigin_FL;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepOrigin_FR;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepOrigin_RL;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepOrigin_RR;

	private static readonly System.IntPtr NativeFieldInfoPtr_leftWheel;

	private static readonly System.IntPtr NativeFieldInfoPtr_rightWheel;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepSegment;

	private static readonly System.IntPtr NativeFieldInfoPtr_sampleStepSizeMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_sampleStepSizeMax;

	private static readonly System.IntPtr NativeFieldInfoPtr_aheadPointSamples;

	private static readonly System.IntPtr NativeFieldInfoPtr_DestinationDistanceSlowThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_DestinationArrivalThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_steerTargetFollowRate;

	private static readonly System.IntPtr NativeFieldInfoPtr_steerPID;

	private static readonly System.IntPtr NativeFieldInfoPtr_turnSpeedReductionMinRange;

	private static readonly System.IntPtr NativeFieldInfoPtr_turnSpeedReductionMaxRange;

	private static readonly System.IntPtr NativeFieldInfoPtr_turnSpeedReductionDivisor;

	private static readonly System.IntPtr NativeFieldInfoPtr_minTurnSpeedReductionAngleThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_minTurningSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_throttleMin;

	private static readonly System.IntPtr NativeFieldInfoPtr_throttleMax;

	private static readonly System.IntPtr NativeFieldInfoPtr_throttlePID;

	private static readonly System.IntPtr NativeFieldInfoPtr_UnmarkedSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_ReverseSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_speedReductionTracker;

	private static readonly System.IntPtr NativeFieldInfoPtr_PursuitModeEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_PursuitTarget;

	private static readonly System.IntPtr NativeFieldInfoPtr_PursuitDistanceUpdateThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_PursuitTargetLastPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_Teleporter;

	private static readonly System.IntPtr NativeFieldInfoPtr_PositionHistoryTracker;

	private static readonly System.IntPtr NativeFieldInfoPtr_StuckTimeThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_StuckSamples;

	private static readonly System.IntPtr NativeFieldInfoPtr_StuckDistanceThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_storedNavigationCallback;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentSpeedZone;

	private static readonly System.IntPtr NativeFieldInfoPtr__groundMask;

	private static readonly System.IntPtr NativeFieldInfoPtr_vehicle;

	private static readonly System.IntPtr NativeFieldInfoPtr_wheelbase;

	private static readonly System.IntPtr NativeFieldInfoPtr_wheeltrack;

	private static readonly System.IntPtr NativeFieldInfoPtr_vehicleLength;

	private static readonly System.IntPtr NativeFieldInfoPtr_vehicleWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_turnRadius;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepTrack;

	private static readonly System.IntPtr NativeFieldInfoPtr_wheelBottomOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_targetSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_targetSteerAngle_Normalized;

	private static readonly System.IntPtr NativeFieldInfoPtr_lateralOffset;

	private static readonly System.IntPtr NativeFieldInfoPtr_path;

	private static readonly System.IntPtr NativeFieldInfoPtr_timeOnLastNavigationCall;

	private static readonly System.IntPtr NativeFieldInfoPtr_sweepTestFailedTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentNavigationSettings;

	private static readonly System.IntPtr NativeFieldInfoPtr_navigationCalculationRoutine;

	private static readonly System.IntPtr NativeFieldInfoPtr_reverseCoroutine;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoDriving_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoDriving_Protected_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_KinematicMode_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsReversing_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TargetLocation_Public_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TargetLocation_Protected_set_Void_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_sampleStepSize_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_turnSpeedReductionRange_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_maxSteerAngle_Protected_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_frontOfVehiclePosition_Private_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NavigationCalculationInProgress_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_timeSinceLastNavigationCall_Private_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitializeVehicleData_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InfrequentUpdate_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateKinematic_Protected_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAxleGroundHit_Private_Vector3_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSweep_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSpeedReduction_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdatePursuitMode_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateStuckDetection_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckDistanceFromPath_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateOvertaking_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RefreshSpeedZone_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSpeed_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateSteering_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Navigate_Public_Void_Vector3_NavigationSettings_NavigationCallback_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NavigationCalculationCallback_Private_Void_ENavigationCalculationResult_SmoothedPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EndDriving_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopNavigating_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RecalculateNavigation_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SweepTurn_Public_Boolean_ESweepType_Single_Boolean_byref_Single_byref_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BetterSweepTurn_Public_Void_ESweepType_Single_Boolean_LayerMask_byref_Single_byref_RaycastHit_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StartReverse_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reverse_Public_IEnumerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_StopReversing_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetClosestForwardObstruction_Private_Collider_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsOnVehicleGraph_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDistanceFromVehicleGraph_Private_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPathLateralDirection_Private_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetIsStuck_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__Reverse_b__142_0_Private_Boolean_0;

	public unsafe static string VehicleGraphName
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VehicleGraphName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VehicleGraphName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string RoadGraphName
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_RoadGraphName, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_RoadGraphName, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static float MaxDistanceFromPath
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxDistanceFromPath, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxDistanceFromPath, (void*)(&num));
		}
	}

	public unsafe static float MaxDistanceFromPathWhenReversing
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxDistanceFromPathWhenReversing, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxDistanceFromPathWhenReversing, (void*)(&num));
		}
	}

	public unsafe static Vector3 MainGraphSamplePoint
	{
		get
		{
			Unsafe.SkipInit(out Vector3 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MainGraphSamplePoint, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MainGraphSamplePoint, (void*)(&vector));
		}
	}

	public unsafe static float MinRenavigationRate
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinRenavigationRate, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinRenavigationRate, (void*)(&num));
		}
	}

	public unsafe static float Steer_P
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Steer_P, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Steer_P, (void*)(&num));
		}
	}

	public unsafe static float Steer_I
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Steer_I, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Steer_I, (void*)(&num));
		}
	}

	public unsafe static float Steer_D
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Steer_D, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Steer_D, (void*)(&num));
		}
	}

	public unsafe static float Throttle_P
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Throttle_P, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Throttle_P, (void*)(&num));
		}
	}

	public unsafe static float Throttle_I
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Throttle_I, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Throttle_I, (void*)(&num));
		}
	}

	public unsafe static float Throttle_D
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Throttle_D, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Throttle_D, (void*)(&num));
		}
	}

	public unsafe static float Steer_Rate
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Steer_Rate, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Steer_Rate, (void*)(&num));
		}
	}

	public unsafe static float MaxAxlePositionShift
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxAxlePositionShift, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxAxlePositionShift, (void*)(&num));
		}
	}

	public unsafe static float OBSTACLE_MIN_RANGE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OBSTACLE_MIN_RANGE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OBSTACLE_MIN_RANGE, (void*)(&num));
		}
	}

	public unsafe static float OBSTACLE_MAX_RANGE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OBSTACLE_MAX_RANGE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OBSTACLE_MAX_RANGE, (void*)(&num));
		}
	}

	public unsafe static float MAX_STEER_ANGLE_OVERRIDE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MAX_STEER_ANGLE_OVERRIDE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MAX_STEER_ANGLE_OVERRIDE, (void*)(&num));
		}
	}

	public unsafe static float INFREQUENT_UPDATE_RATE
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_INFREQUENT_UPDATE_RATE, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_INFREQUENT_UPDATE_RATE, (void*)(&num));
		}
	}

	public unsafe static float KinematicModeRotationSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_KinematicModeRotationSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_KinematicModeRotationSpeed, (void*)(&num));
		}
	}

	public unsafe static float KinematicModeSpeedMultiplier
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_KinematicModeSpeedMultiplier, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_KinematicModeSpeedMultiplier, (void*)(&num));
		}
	}

	public unsafe bool DEBUG_MODE
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG_MODE);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DEBUG_MODE)) = flag;
		}
	}

	public unsafe bool _AutoDriving_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AutoDriving_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AutoDriving_k__BackingField)) = flag;
		}
	}

	public unsafe Vector3 _TargetLocation_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TargetLocation_k__BackingField);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TargetLocation_k__BackingField)) = vector;
		}
	}

	public unsafe DriveFlags Flags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Flags);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DriveFlags>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Flags)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)driveFlags));
		}
	}

	public unsafe Seeker roadSeeker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_roadSeeker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Seeker>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_roadSeeker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)seeker));
		}
	}

	public unsafe Seeker generalSeeker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generalSeeker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Seeker>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generalSeeker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)seeker));
		}
	}

	public unsafe Transform CTE_Origin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CTE_Origin);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CTE_Origin)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform FrontAxlePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrontAxlePosition);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FrontAxlePosition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform RearAxlePosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RearAxlePosition);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RearAxlePosition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Sensor sensor_FL
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FL);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sensor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FL)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sensor));
		}
	}

	public unsafe Sensor sensor_FM
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FM);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sensor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FM)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sensor));
		}
	}

	public unsafe Sensor sensor_FR
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FR);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sensor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_FR)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sensor));
		}
	}

	public unsafe Sensor sensor_RR
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_RR);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sensor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_RR)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sensor));
		}
	}

	public unsafe Sensor sensor_RL
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_RL);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sensor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensor_RL)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sensor));
		}
	}

	public unsafe Il2CppReferenceArray<Sensor> sensors
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensors);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Sensor>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sensors)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe LayerMask sweepMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepMask)) = layerMask;
		}
	}

	public unsafe Transform sweepOrigin_FL
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_FL);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_FL)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform sweepOrigin_FR
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_FR);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_FR)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform sweepOrigin_RL
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_RL);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_RL)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Transform sweepOrigin_RR
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_RR);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepOrigin_RR)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Wheel leftWheel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_leftWheel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Wheel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_leftWheel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheel));
		}
	}

	public unsafe Wheel rightWheel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rightWheel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Wheel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rightWheel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)wheel));
		}
	}

	public unsafe static float sweepSegment
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_sweepSegment, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_sweepSegment, (void*)(&num));
		}
	}

	public unsafe float sampleStepSizeMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sampleStepSizeMin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sampleStepSizeMin)) = num;
		}
	}

	public unsafe float sampleStepSizeMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sampleStepSizeMax);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sampleStepSizeMax)) = num;
		}
	}

	public unsafe int aheadPointSamples
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_aheadPointSamples);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_aheadPointSamples)) = num;
		}
	}

	public unsafe static float DestinationDistanceSlowThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DestinationDistanceSlowThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DestinationDistanceSlowThreshold, (void*)(&num));
		}
	}

	public unsafe static float DestinationArrivalThreshold
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DestinationArrivalThreshold, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DestinationArrivalThreshold, (void*)(&num));
		}
	}

	public unsafe float steerTargetFollowRate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_steerTargetFollowRate);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_steerTargetFollowRate)) = num;
		}
	}

	public unsafe SteerPID steerPID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_steerPID);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SteerPID>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_steerPID)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)steerPID));
		}
	}

	public unsafe float turnSpeedReductionMinRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionMinRange);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionMinRange)) = num;
		}
	}

	public unsafe float turnSpeedReductionMaxRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionMaxRange);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionMaxRange)) = num;
		}
	}

	public unsafe float turnSpeedReductionDivisor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionDivisor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnSpeedReductionDivisor)) = num;
		}
	}

	public unsafe float minTurnSpeedReductionAngleThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minTurnSpeedReductionAngleThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minTurnSpeedReductionAngleThreshold)) = num;
		}
	}

	public unsafe float minTurningSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minTurningSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_minTurningSpeed)) = num;
		}
	}

	public unsafe float throttleMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttleMin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttleMin)) = num;
		}
	}

	public unsafe float throttleMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttleMax);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttleMax)) = num;
		}
	}

	public unsafe PID throttlePID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttlePID);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PID>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_throttlePID)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pID));
		}
	}

	public unsafe static float UnmarkedSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_UnmarkedSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_UnmarkedSpeed, (void*)(&num));
		}
	}

	public unsafe static float ReverseSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ReverseSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ReverseSpeed, (void*)(&num));
		}
	}

	public unsafe ValueTracker speedReductionTracker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_speedReductionTracker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ValueTracker>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_speedReductionTracker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)valueTracker));
		}
	}

	public unsafe bool PursuitModeEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitModeEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitModeEnabled)) = flag;
		}
	}

	public unsafe Transform PursuitTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitTarget);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe float PursuitDistanceUpdateThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitDistanceUpdateThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitDistanceUpdateThreshold)) = num;
		}
	}

	public unsafe Vector3 PursuitTargetLastPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitTargetLastPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PursuitTargetLastPosition)) = vector;
		}
	}

	public unsafe VehicleTeleporter Teleporter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Teleporter);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VehicleTeleporter>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Teleporter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vehicleTeleporter));
		}
	}

	public unsafe PositionHistoryTracker PositionHistoryTracker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PositionHistoryTracker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PositionHistoryTracker>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PositionHistoryTracker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)positionHistoryTracker));
		}
	}

	public unsafe float StuckTimeThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckTimeThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckTimeThreshold)) = num;
		}
	}

	public unsafe int StuckSamples
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckSamples);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckSamples)) = num;
		}
	}

	public unsafe float StuckDistanceThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckDistanceThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_StuckDistanceThreshold)) = num;
		}
	}

	public unsafe NavigationCallback storedNavigationCallback
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_storedNavigationCallback);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NavigationCallback>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_storedNavigationCallback)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)navigationCallback));
		}
	}

	public unsafe SpeedZone currentSpeedZone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSpeedZone);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SpeedZone>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSpeedZone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)speedZone));
		}
	}

	public unsafe LayerMask _groundMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groundMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groundMask)) = layerMask;
		}
	}

	public unsafe LandVehicle vehicle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicle);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<LandVehicle>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicle)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)landVehicle));
		}
	}

	public unsafe float wheelbase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelbase);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelbase)) = num;
		}
	}

	public unsafe float wheeltrack
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheeltrack);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheeltrack)) = num;
		}
	}

	public unsafe float vehicleLength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleLength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleLength)) = num;
		}
	}

	public unsafe float vehicleWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleWidth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleWidth)) = num;
		}
	}

	public unsafe float turnRadius
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnRadius);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_turnRadius)) = num;
		}
	}

	public unsafe float sweepTrack
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepTrack);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepTrack)) = num;
		}
	}

	public unsafe float wheelBottomOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelBottomOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wheelBottomOffset)) = num;
		}
	}

	public unsafe float targetSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetSpeed)) = num;
		}
	}

	public unsafe float targetSteerAngle_Normalized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetSteerAngle_Normalized);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetSteerAngle_Normalized)) = num;
		}
	}

	public unsafe float lateralOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lateralOffset);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lateralOffset)) = num;
		}
	}

	public unsafe PathSmoothingUtility.SmoothedPath path
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PathSmoothingUtility.SmoothedPath>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)smoothedPath));
		}
	}

	public unsafe float timeOnLastNavigationCall
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeOnLastNavigationCall);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeOnLastNavigationCall)) = num;
		}
	}

	public unsafe float sweepTestFailedTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepTestFailedTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sweepTestFailedTime)) = num;
		}
	}

	public unsafe NavigationSettings currentNavigationSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNavigationSettings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NavigationSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentNavigationSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)navigationSettings));
		}
	}

	public unsafe Coroutine navigationCalculationRoutine
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_navigationCalculationRoutine);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Coroutine>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_navigationCalculationRoutine)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coroutine));
		}
	}

	public unsafe Coroutine reverseCoroutine
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reverseCoroutine);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Coroutine>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reverseCoroutine)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coroutine));
		}
	}

	public unsafe bool AutoDriving
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoDriving_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoDriving_Protected_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool KinematicMode
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96118, XrefRangeEnd = 96120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_KinematicMode_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsReversing
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 96120, RefRangeEnd = 96121, XrefRangeStart = 96120, XrefRangeEnd = 96120, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsReversing_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Vector3 TargetLocation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TargetLocation_Public_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TargetLocation_Protected_set_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float sampleStepSize
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 96122, RefRangeEnd = 96124, XrefRangeStart = 96121, XrefRangeEnd = 96122, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_sampleStepSize_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float turnSpeedReductionRange
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96124, XrefRangeEnd = 96125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_turnSpeedReductionRange_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float maxSteerAngle
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96125, XrefRangeEnd = 96127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_maxSteerAngle_Protected_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Vector3 frontOfVehiclePosition
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96127, XrefRangeEnd = 96131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_frontOfVehiclePosition_Private_get_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool NavigationCalculationInProgress
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 96131, RefRangeEnd = 96132, XrefRangeStart = 96131, XrefRangeEnd = 96131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NavigationCalculationInProgress_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float timeSinceLastNavigationCall
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96132, XrefRangeEnd = 96133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_timeSinceLastNavigationCall_Private_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static VehicleAgent()
	{
		Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles.AI", "VehicleAgent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr);
		NativeFieldInfoPtr_VehicleGraphName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "VehicleGraphName");
		NativeFieldInfoPtr_RoadGraphName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "RoadGraphName");
		NativeFieldInfoPtr_MaxDistanceFromPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MaxDistanceFromPath");
		NativeFieldInfoPtr_MaxDistanceFromPathWhenReversing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MaxDistanceFromPathWhenReversing");
		NativeFieldInfoPtr_MainGraphSamplePoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MainGraphSamplePoint");
		NativeFieldInfoPtr_MinRenavigationRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MinRenavigationRate");
		NativeFieldInfoPtr_Steer_P = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Steer_P");
		NativeFieldInfoPtr_Steer_I = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Steer_I");
		NativeFieldInfoPtr_Steer_D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Steer_D");
		NativeFieldInfoPtr_Throttle_P = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Throttle_P");
		NativeFieldInfoPtr_Throttle_I = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Throttle_I");
		NativeFieldInfoPtr_Throttle_D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Throttle_D");
		NativeFieldInfoPtr_Steer_Rate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Steer_Rate");
		NativeFieldInfoPtr_MaxAxlePositionShift = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MaxAxlePositionShift");
		NativeFieldInfoPtr_OBSTACLE_MIN_RANGE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "OBSTACLE_MIN_RANGE");
		NativeFieldInfoPtr_OBSTACLE_MAX_RANGE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "OBSTACLE_MAX_RANGE");
		NativeFieldInfoPtr_MAX_STEER_ANGLE_OVERRIDE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "MAX_STEER_ANGLE_OVERRIDE");
		NativeFieldInfoPtr_INFREQUENT_UPDATE_RATE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "INFREQUENT_UPDATE_RATE");
		NativeFieldInfoPtr_KinematicModeRotationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "KinematicModeRotationSpeed");
		NativeFieldInfoPtr_KinematicModeSpeedMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "KinematicModeSpeedMultiplier");
		NativeFieldInfoPtr_DEBUG_MODE = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "DEBUG_MODE");
		NativeFieldInfoPtr__AutoDriving_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<AutoDriving>k__BackingField");
		NativeFieldInfoPtr__TargetLocation_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "<TargetLocation>k__BackingField");
		NativeFieldInfoPtr_Flags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Flags");
		NativeFieldInfoPtr_roadSeeker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "roadSeeker");
		NativeFieldInfoPtr_generalSeeker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "generalSeeker");
		NativeFieldInfoPtr_CTE_Origin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "CTE_Origin");
		NativeFieldInfoPtr_FrontAxlePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "FrontAxlePosition");
		NativeFieldInfoPtr_RearAxlePosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "RearAxlePosition");
		NativeFieldInfoPtr_sensor_FL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensor_FL");
		NativeFieldInfoPtr_sensor_FM = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensor_FM");
		NativeFieldInfoPtr_sensor_FR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensor_FR");
		NativeFieldInfoPtr_sensor_RR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensor_RR");
		NativeFieldInfoPtr_sensor_RL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensor_RL");
		NativeFieldInfoPtr_sensors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sensors");
		NativeFieldInfoPtr_sweepMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepMask");
		NativeFieldInfoPtr_sweepOrigin_FL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepOrigin_FL");
		NativeFieldInfoPtr_sweepOrigin_FR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepOrigin_FR");
		NativeFieldInfoPtr_sweepOrigin_RL = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepOrigin_RL");
		NativeFieldInfoPtr_sweepOrigin_RR = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepOrigin_RR");
		NativeFieldInfoPtr_leftWheel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "leftWheel");
		NativeFieldInfoPtr_rightWheel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "rightWheel");
		NativeFieldInfoPtr_sweepSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepSegment");
		NativeFieldInfoPtr_sampleStepSizeMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sampleStepSizeMin");
		NativeFieldInfoPtr_sampleStepSizeMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sampleStepSizeMax");
		NativeFieldInfoPtr_aheadPointSamples = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "aheadPointSamples");
		NativeFieldInfoPtr_DestinationDistanceSlowThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "DestinationDistanceSlowThreshold");
		NativeFieldInfoPtr_DestinationArrivalThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "DestinationArrivalThreshold");
		NativeFieldInfoPtr_steerTargetFollowRate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "steerTargetFollowRate");
		NativeFieldInfoPtr_steerPID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "steerPID");
		NativeFieldInfoPtr_turnSpeedReductionMinRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "turnSpeedReductionMinRange");
		NativeFieldInfoPtr_turnSpeedReductionMaxRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "turnSpeedReductionMaxRange");
		NativeFieldInfoPtr_turnSpeedReductionDivisor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "turnSpeedReductionDivisor");
		NativeFieldInfoPtr_minTurnSpeedReductionAngleThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "minTurnSpeedReductionAngleThreshold");
		NativeFieldInfoPtr_minTurningSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "minTurningSpeed");
		NativeFieldInfoPtr_throttleMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "throttleMin");
		NativeFieldInfoPtr_throttleMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "throttleMax");
		NativeFieldInfoPtr_throttlePID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "throttlePID");
		NativeFieldInfoPtr_UnmarkedSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "UnmarkedSpeed");
		NativeFieldInfoPtr_ReverseSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "ReverseSpeed");
		NativeFieldInfoPtr_speedReductionTracker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "speedReductionTracker");
		NativeFieldInfoPtr_PursuitModeEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "PursuitModeEnabled");
		NativeFieldInfoPtr_PursuitTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "PursuitTarget");
		NativeFieldInfoPtr_PursuitDistanceUpdateThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "PursuitDistanceUpdateThreshold");
		NativeFieldInfoPtr_PursuitTargetLastPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "PursuitTargetLastPosition");
		NativeFieldInfoPtr_Teleporter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "Teleporter");
		NativeFieldInfoPtr_PositionHistoryTracker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "PositionHistoryTracker");
		NativeFieldInfoPtr_StuckTimeThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "StuckTimeThreshold");
		NativeFieldInfoPtr_StuckSamples = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "StuckSamples");
		NativeFieldInfoPtr_StuckDistanceThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "StuckDistanceThreshold");
		NativeFieldInfoPtr_storedNavigationCallback = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "storedNavigationCallback");
		NativeFieldInfoPtr_currentSpeedZone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "currentSpeedZone");
		NativeFieldInfoPtr__groundMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "_groundMask");
		NativeFieldInfoPtr_vehicle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "vehicle");
		NativeFieldInfoPtr_wheelbase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "wheelbase");
		NativeFieldInfoPtr_wheeltrack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "wheeltrack");
		NativeFieldInfoPtr_vehicleLength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "vehicleLength");
		NativeFieldInfoPtr_vehicleWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "vehicleWidth");
		NativeFieldInfoPtr_turnRadius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "turnRadius");
		NativeFieldInfoPtr_sweepTrack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepTrack");
		NativeFieldInfoPtr_wheelBottomOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "wheelBottomOffset");
		NativeFieldInfoPtr_targetSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "targetSpeed");
		NativeFieldInfoPtr_targetSteerAngle_Normalized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "targetSteerAngle_Normalized");
		NativeFieldInfoPtr_lateralOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "lateralOffset");
		NativeFieldInfoPtr_path = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "path");
		NativeFieldInfoPtr_timeOnLastNavigationCall = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "timeOnLastNavigationCall");
		NativeFieldInfoPtr_sweepTestFailedTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "sweepTestFailedTime");
		NativeFieldInfoPtr_currentNavigationSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "currentNavigationSettings");
		NativeFieldInfoPtr_navigationCalculationRoutine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "navigationCalculationRoutine");
		NativeFieldInfoPtr_reverseCoroutine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, "reverseCoroutine");
		NativeMethodInfoPtr_get_AutoDriving_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666440);
		NativeMethodInfoPtr_set_AutoDriving_Protected_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666441);
		NativeMethodInfoPtr_get_KinematicMode_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666442);
		NativeMethodInfoPtr_get_IsReversing_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666443);
		NativeMethodInfoPtr_get_TargetLocation_Public_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666444);
		NativeMethodInfoPtr_set_TargetLocation_Protected_set_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666445);
		NativeMethodInfoPtr_get_sampleStepSize_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666446);
		NativeMethodInfoPtr_get_turnSpeedReductionRange_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666447);
		NativeMethodInfoPtr_get_maxSteerAngle_Protected_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666448);
		NativeMethodInfoPtr_get_frontOfVehiclePosition_Private_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666449);
		NativeMethodInfoPtr_get_NavigationCalculationInProgress_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666450);
		NativeMethodInfoPtr_get_timeSinceLastNavigationCall_Private_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666451);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666452);
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666453);
		NativeMethodInfoPtr_InitializeVehicleData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666454);
		NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666455);
		NativeMethodInfoPtr_InfrequentUpdate_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666456);
		NativeMethodInfoPtr_LateUpdate_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666457);
		NativeMethodInfoPtr_UpdateKinematic_Protected_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666458);
		NativeMethodInfoPtr_GetAxleGroundHit_Private_Vector3_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666459);
		NativeMethodInfoPtr_UpdateSweep_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666460);
		NativeMethodInfoPtr_UpdateSpeedReduction_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666461);
		NativeMethodInfoPtr_UpdatePursuitMode_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666462);
		NativeMethodInfoPtr_UpdateStuckDetection_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666463);
		NativeMethodInfoPtr_CheckDistanceFromPath_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666464);
		NativeMethodInfoPtr_UpdateOvertaking_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666465);
		NativeMethodInfoPtr_RefreshSpeedZone_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666466);
		NativeMethodInfoPtr_UpdateSpeed_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666467);
		NativeMethodInfoPtr_UpdateSteering_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666468);
		NativeMethodInfoPtr_Navigate_Public_Void_Vector3_NavigationSettings_NavigationCallback_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666469);
		NativeMethodInfoPtr_NavigationCalculationCallback_Private_Void_ENavigationCalculationResult_SmoothedPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666470);
		NativeMethodInfoPtr_EndDriving_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666471);
		NativeMethodInfoPtr_StopNavigating_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666472);
		NativeMethodInfoPtr_RecalculateNavigation_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666473);
		NativeMethodInfoPtr_SweepTurn_Public_Boolean_ESweepType_Single_Boolean_byref_Single_byref_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666474);
		NativeMethodInfoPtr_BetterSweepTurn_Public_Void_ESweepType_Single_Boolean_LayerMask_byref_Single_byref_RaycastHit_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666475);
		NativeMethodInfoPtr_StartReverse_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666476);
		NativeMethodInfoPtr_Reverse_Public_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666477);
		NativeMethodInfoPtr_StopReversing_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666478);
		NativeMethodInfoPtr_GetClosestForwardObstruction_Private_Collider_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666479);
		NativeMethodInfoPtr_IsOnVehicleGraph_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666480);
		NativeMethodInfoPtr_GetDistanceFromVehicleGraph_Private_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666481);
		NativeMethodInfoPtr_GetPathLateralDirection_Private_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666482);
		NativeMethodInfoPtr_GetIsStuck_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666483);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666484);
		NativeMethodInfoPtr__Reverse_b__142_0_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr, 100666486);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96133, XrefRangeEnd = 96180, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96180, XrefRangeEnd = 96193, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96278, RefRangeEnd = 96279, XrefRangeStart = 96193, XrefRangeEnd = 96278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitializeVehicleData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeVehicleData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96279, XrefRangeEnd = 96280, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96280, XrefRangeEnd = 96310, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InfrequentUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InfrequentUpdate_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96310, XrefRangeEnd = 96323, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96402, RefRangeEnd = 96403, XrefRangeStart = 96323, XrefRangeEnd = 96402, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateKinematic(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateKinematic_Protected_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 96420, RefRangeEnd = 96422, XrefRangeStart = 96403, XrefRangeEnd = 96420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetAxleGroundHit(bool front)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&front);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAxleGroundHit_Private_Vector3_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96438, RefRangeEnd = 96439, XrefRangeStart = 96422, XrefRangeEnd = 96438, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSweep()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSweep_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96511, RefRangeEnd = 96512, XrefRangeStart = 96439, XrefRangeEnd = 96511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSpeedReduction()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSpeedReduction_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96512, XrefRangeEnd = 96525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdatePursuitMode()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdatePursuitMode_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96525, XrefRangeEnd = 96549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateStuckDetection()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateStuckDetection_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96580, RefRangeEnd = 96581, XrefRangeStart = 96549, XrefRangeEnd = 96580, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckDistanceFromPath()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckDistanceFromPath_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96581, XrefRangeEnd = 96591, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateOvertaking()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateOvertaking_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96591, XrefRangeEnd = 96602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void RefreshSpeedZone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RefreshSpeedZone_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96602, XrefRangeEnd = 96611, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void UpdateSpeed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UpdateSpeed_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96658, RefRangeEnd = 96659, XrefRangeStart = 96611, XrefRangeEnd = 96658, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateSteering()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateSteering_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 96684, RefRangeEnd = 96690, XrefRangeStart = 96659, XrefRangeEnd = 96684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Navigate(Vector3 location, NavigationSettings settings = null, NavigationCallback callback = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&location);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Navigate_Public_Void_Vector3_NavigationSettings_NavigationCallback_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96690, XrefRangeEnd = 96695, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath _path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&result);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NavigationCalculationCallback_Private_Void_ENavigationCalculationResult_SmoothedPath_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 96708, RefRangeEnd = 96710, XrefRangeStart = 96695, XrefRangeEnd = 96708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EndDriving()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndDriving_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 96712, RefRangeEnd = 96717, XrefRangeStart = 96710, XrefRangeEnd = 96712, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StopNavigating()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopNavigating_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 96718, RefRangeEnd = 96719, XrefRangeStart = 96717, XrefRangeEnd = 96718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RecalculateNavigation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RecalculateNavigation_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 96806, RefRangeEnd = 96808, XrefRangeStart = 96719, XrefRangeEnd = 96806, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool SweepTurn(ESweepType sweep, float sweepAngle, bool reverse, out float hitDistance, out Vector3 hitPoint, float steerAngle = 0f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&sweep);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &sweepAngle;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &reverse;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitDistance);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitPoint);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &steerAngle;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SweepTurn_Public_Boolean_ESweepType_Single_Boolean_byref_Single_byref_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 96940, RefRangeEnd = 96942, XrefRangeStart = 96808, XrefRangeEnd = 96940, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BetterSweepTurn(ESweepType sweep, float steerAngle, bool reverse, LayerMask mask, out float hitDistance, out RaycastHit hit)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&sweep);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &steerAngle;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &reverse;
		*(LayerMask**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mask;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitDistance);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hit);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BetterSweepTurn_Public_Void_ESweepType_Single_Boolean_LayerMask_byref_Single_byref_RaycastHit_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96942, XrefRangeEnd = 96950, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StartReverse()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StartReverse_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96950, XrefRangeEnd = 96955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IEnumerator Reverse()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reverse_Public_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 96963, RefRangeEnd = 96966, XrefRangeStart = 96955, XrefRangeEnd = 96963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StopReversing()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StopReversing_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 96966, XrefRangeEnd = 97024, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Collider GetClosestForwardObstruction(out float obstructionDist)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref obstructionDist);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetClosestForwardObstruction_Private_Collider_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 97025, RefRangeEnd = 97027, XrefRangeStart = 97024, XrefRangeEnd = 97025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsOnVehicleGraph()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsOnVehicleGraph_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 97051, RefRangeEnd = 97054, XrefRangeStart = 97027, XrefRangeEnd = 97051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetDistanceFromVehicleGraph()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDistanceFromVehicleGraph_Private_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 97065, RefRangeEnd = 97066, XrefRangeStart = 97054, XrefRangeEnd = 97065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetPathLateralDirection()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPathLateralDirection_Private_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 97085, RefRangeEnd = 97087, XrefRangeStart = 97066, XrefRangeEnd = 97085, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetIsStuck()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetIsStuck_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97087, XrefRangeEnd = 97092, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VehicleAgent()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VehicleAgent>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool _Reverse_b__142_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Reverse_b__142_0_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public VehicleAgent(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
