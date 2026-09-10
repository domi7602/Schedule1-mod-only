using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Collections.ObjectModel;
using Il2CppSystem.Reflection;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy;

public class CurvySpline : DTVersionedMonoBehaviour
{
	public class ControlPointsSynchronizer : Il2CppSystem.Object
	{
		[OriginalName("Assembly-CSharp-firstpass.dll", "", "SynchronizationRequest")]
		public enum SynchronizationRequest
		{
			None,
			SplineToHierarchy,
			HierarchyToSpline
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_spline;

		private static readonly System.IntPtr NativeFieldInfoPtr_processing;

		private static readonly System.IntPtr NativeFieldInfoPtr__CurrentRequest_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentRequest_Public_get_SynchronizationRequest_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_CurrentRequest_Private_set_Void_SynchronizationRequest_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RequestSplineToHierarchy_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RequestHierarchyToSpline_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessRequests_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CancelRequests_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SynchronizeHierarchyToSpline_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SynchronizeSplineToHierarchy_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_DebugLog_Private_Static_Void_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_LogIgnoredRequest_Private_Static_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_AssertIsNotProcessing_Private_Void_0;

		public unsafe CurvySpline spline
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe bool processing
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_processing);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_processing)) = flag;
			}
		}

		public unsafe SynchronizationRequest _CurrentRequest_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentRequest_k__BackingField);
				return *(SynchronizationRequest*)num;
			}
			set
			{
				*(SynchronizationRequest*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CurrentRequest_k__BackingField)) = synchronizationRequest;
			}
		}

		public unsafe SynchronizationRequest CurrentRequest
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 36888, RefRangeEnd = 36892, XrefRangeStart = 36888, XrefRangeEnd = 36888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentRequest_Public_get_SynchronizationRequest_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(SynchronizationRequest*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(0)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentRequest_Private_set_Void_SynchronizationRequest_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static ControlPointsSynchronizer()
		{
			Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "ControlPointsSynchronizer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr);
			NativeFieldInfoPtr_spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, "spline");
			NativeFieldInfoPtr_processing = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, "processing");
			NativeFieldInfoPtr__CurrentRequest_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, "<CurrentRequest>k__BackingField");
			NativeMethodInfoPtr_get_CurrentRequest_Public_get_SynchronizationRequest_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664859);
			NativeMethodInfoPtr_set_CurrentRequest_Private_set_Void_SynchronizationRequest_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664860);
			NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664861);
			NativeMethodInfoPtr_RequestSplineToHierarchy_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664862);
			NativeMethodInfoPtr_RequestHierarchyToSpline_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664863);
			NativeMethodInfoPtr_ProcessRequests_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664864);
			NativeMethodInfoPtr_CancelRequests_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664865);
			NativeMethodInfoPtr_SynchronizeHierarchyToSpline_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664866);
			NativeMethodInfoPtr_SynchronizeSplineToHierarchy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664867);
			NativeMethodInfoPtr_DebugLog_Private_Static_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664868);
			NativeMethodInfoPtr_LogIgnoredRequest_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664869);
			NativeMethodInfoPtr_AssertIsNotProcessing_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr, 100664870);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ControlPointsSynchronizer(CurvySpline spline)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ControlPointsSynchronizer>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe void RequestSplineToHierarchy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RequestSplineToHierarchy_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe void RequestHierarchyToSpline()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RequestHierarchyToSpline_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 36907, RefRangeEnd = 36911, XrefRangeStart = 36892, XrefRangeEnd = 36907, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ProcessRequests()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessRequests_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe void CancelRequests()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CancelRequests_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36911, XrefRangeEnd = 36924, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SynchronizeHierarchyToSpline()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SynchronizeHierarchyToSpline_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36924, XrefRangeEnd = 36935, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SynchronizeSplineToHierarchy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SynchronizeSplineToHierarchy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36935, XrefRangeEnd = 36939, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void DebugLog(string message)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DebugLog_Private_Static_Void_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36939, XrefRangeEnd = 36945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void LogIgnoredRequest()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogIgnoredRequest_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void AssertIsNotProcessing()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AssertIsNotProcessing_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ControlPointsSynchronizer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class DirtinessManager : Il2CppSystem.Object
	{
		[System.Serializable]
		[ObfuscatedName("FluffyUnderware.Curvy.CurvySpline+DirtinessManager+<>c")]
		public sealed class __c : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr___9;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__21_0;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__35_0;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__35_1;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__36_0;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__36_1;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__36_2;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__37_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			private static readonly System.IntPtr NativeMethodInfoPtr___ctor_b__21_0_Internal_Void_CurvySplineSegment_Int32_Int32_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__ValidateConnectedSplines_b__35_0_Internal_Boolean_CurvySplineSegment_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__ValidateConnectedSplines_b__35_1_Internal_IEnumerable_1_CurvySplineSegment_CurvySplineSegment_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_0_Internal_Boolean_CurvySplineSegment_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_1_Internal_CurvySpline_CurvySplineSegment_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_2_Internal_Boolean_CurvySplineSegment_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__SynchronizeUninitializedSplines_b__37_0_Internal_Boolean_CurvySplineSegment_0;

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

			public unsafe static Il2CppSystem.Action<CurvySplineSegment, int, int> __9__21_0
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__21_0, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySplineSegment, int, int>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__21_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
				}
			}

			public unsafe static Il2CppSystem.Func<CurvySplineSegment, bool> __9__35_0
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__35_0, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__35_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Func<CurvySplineSegment, IEnumerable<CurvySplineSegment>> __9__35_1
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__35_1, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, IEnumerable<CurvySplineSegment>>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__35_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Func<CurvySplineSegment, bool> __9__36_0
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__36_0, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__36_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Func<CurvySplineSegment, CurvySpline> __9__36_1
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__36_1, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, CurvySpline>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__36_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Predicate<CurvySplineSegment> __9__36_2
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__36_2, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Predicate<CurvySplineSegment>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__36_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)predicate));
				}
			}

			public unsafe static Il2CppSystem.Func<CurvySplineSegment, bool> __9__37_0
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__37_0, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__37_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			static __c()
			{
				Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "<>c");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
				NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
				NativeFieldInfoPtr___9__21_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__21_0");
				NativeFieldInfoPtr___9__35_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__35_0");
				NativeFieldInfoPtr___9__35_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__35_1");
				NativeFieldInfoPtr___9__36_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__36_0");
				NativeFieldInfoPtr___9__36_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__36_1");
				NativeFieldInfoPtr___9__36_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__36_2");
				NativeFieldInfoPtr___9__37_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__37_0");
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664898);
				NativeMethodInfoPtr___ctor_b__21_0_Internal_Void_CurvySplineSegment_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664899);
				NativeMethodInfoPtr__ValidateConnectedSplines_b__35_0_Internal_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664900);
				NativeMethodInfoPtr__ValidateConnectedSplines_b__35_1_Internal_IEnumerable_1_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664901);
				NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_0_Internal_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664902);
				NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_1_Internal_CurvySpline_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664903);
				NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_2_Internal_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664904);
				NativeMethodInfoPtr__SynchronizeUninitializedSplines_b__37_0_Internal_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664905);
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
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36945, XrefRangeEnd = 36947, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe void __ctor_b__21_0(CurvySplineSegment controlPoint, int cpIndex, int cpsCount)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[3];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
				*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &cpIndex;
				*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cpsCount;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___ctor_b__21_0_Internal_Void_CurvySplineSegment_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36947, XrefRangeEnd = 36952, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe bool _ValidateConnectedSplines_b__35_0(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ValidateConnectedSplines_b__35_0_Internal_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36952, XrefRangeEnd = 36956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe IEnumerable<CurvySplineSegment> _ValidateConnectedSplines_b__35_1(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ValidateConnectedSplines_b__35_1_Internal_IEnumerable_1_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CurvySplineSegment>>(intPtr) : null;
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36956, XrefRangeEnd = 36961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe bool _SynchronizeSplinesWithNullCps_b__36_0(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_0_Internal_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			public unsafe CurvySpline _SynchronizeSplinesWithNullCps_b__36_1(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_1_Internal_CurvySpline_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36961, XrefRangeEnd = 36965, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe bool _SynchronizeSplinesWithNullCps_b__36_2(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SynchronizeSplinesWithNullCps_b__36_2_Internal_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36965, XrefRangeEnd = 36970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe bool _SynchronizeUninitializedSplines_b__37_0(CurvySplineSegment cp)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SynchronizeUninitializedSplines_b__37_0_Internal_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			public __c(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_dirtyCurve;

		private static readonly System.IntPtr NativeFieldInfoPtr_dirtyOrientation;

		private static readonly System.IntPtr NativeFieldInfoPtr_allControlPointsAreDirty;

		private static readonly System.IntPtr NativeFieldInfoPtr_dirtyControlPointsMinimalSet;

		private static readonly System.IntPtr NativeFieldInfoPtr_spline;

		private static readonly System.IntPtr NativeFieldInfoPtr_processingDirtyControlPoints;

		private static readonly System.IntPtr NativeFieldInfoPtr_threadWorker;

		private static readonly System.IntPtr NativeFieldInfoPtr_persistedSegmentsList;

		private static readonly System.IntPtr NativeFieldInfoPtr_persistedOrientationGroup;

		private static readonly System.IntPtr NativeFieldInfoPtr_refreshOrientationStaticAction;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_DirtyCurve_Private_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_DirtyCurve_Private_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_DirtyOrientation_Private_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_DirtyOrientation_Private_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_AllControlPointsAreDirty_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_AllControlPointsAreDirty_Private_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_CurvySplineSegment_CurvySplineSegment_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ClearMinimalSet_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RemoveFromMinimalSet_Public_Void_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessDirtyControlPoints_Public_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessDirtyOrientation_Private_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessDirtyDynamicOrientation_Private_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessDirtyCurve_Private_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_SplineDirtyingType_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_FillDirtyCpsExtendedList_Private_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_AddToMinimalSetAndSetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_CurvySplineSegment_SplineDirtyingType_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ValidateConnectedSplines_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SynchronizeSplinesWithNullCps_Private_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SynchronizeUninitializedSplines_Private_Static_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_DoSanityChecks_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ValidateConnectedSplines_b__35_2_Private_Boolean_CurvySplineSegment_0;

		public unsafe bool dirtyCurve
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyCurve);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyCurve)) = flag;
			}
		}

		public unsafe bool dirtyOrientation
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyOrientation);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyOrientation)) = flag;
			}
		}

		public unsafe bool allControlPointsAreDirty
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allControlPointsAreDirty);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allControlPointsAreDirty)) = flag;
			}
		}

		public unsafe HashSet<CurvySplineSegment> dirtyControlPointsMinimalSet
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyControlPointsMinimalSet);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<CurvySplineSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtyControlPointsMinimalSet)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
			}
		}

		public unsafe CurvySpline spline
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe bool processingDirtyControlPoints
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_processingDirtyControlPoints);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_processingDirtyControlPoints)) = flag;
			}
		}

		public unsafe ThreadPoolWorker<CurvySplineSegment> threadWorker
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadWorker);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ThreadPoolWorker<CurvySplineSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadWorker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)threadPoolWorker));
			}
		}

		public unsafe List<CurvySplineSegment> persistedSegmentsList
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_persistedSegmentsList);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_persistedSegmentsList)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe OrientationGroup persistedOrientationGroup
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_persistedOrientationGroup);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OrientationGroup>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_persistedOrientationGroup)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)orientationGroup));
			}
		}

		public unsafe Il2CppSystem.Action<CurvySplineSegment, int, int> refreshOrientationStaticAction
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refreshOrientationStaticAction);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySplineSegment, int, int>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refreshOrientationStaticAction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe bool DirtyCurve
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DirtyCurve_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 32215, RefRangeEnd = 32216, XrefRangeStart = 32215, XrefRangeEnd = 32216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DirtyCurve_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe bool DirtyOrientation
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DirtyOrientation_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DirtyOrientation_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe bool AllControlPointsAreDirty
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllControlPointsAreDirty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllControlPointsAreDirty_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe bool Dirty
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 36970, XrefRangeEnd = 36971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static DirtinessManager()
		{
			Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "DirtinessManager");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr);
			NativeFieldInfoPtr_dirtyCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "dirtyCurve");
			NativeFieldInfoPtr_dirtyOrientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "dirtyOrientation");
			NativeFieldInfoPtr_allControlPointsAreDirty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "allControlPointsAreDirty");
			NativeFieldInfoPtr_dirtyControlPointsMinimalSet = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "dirtyControlPointsMinimalSet");
			NativeFieldInfoPtr_spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "spline");
			NativeFieldInfoPtr_processingDirtyControlPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "processingDirtyControlPoints");
			NativeFieldInfoPtr_threadWorker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "threadWorker");
			NativeFieldInfoPtr_persistedSegmentsList = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "persistedSegmentsList");
			NativeFieldInfoPtr_persistedOrientationGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "persistedOrientationGroup");
			NativeFieldInfoPtr_refreshOrientationStaticAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "refreshOrientationStaticAction");
			NativeMethodInfoPtr_get_DirtyCurve_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664871);
			NativeMethodInfoPtr_set_DirtyCurve_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664872);
			NativeMethodInfoPtr_get_DirtyOrientation_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664873);
			NativeMethodInfoPtr_set_DirtyOrientation_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664874);
			NativeMethodInfoPtr_get_AllControlPointsAreDirty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664875);
			NativeMethodInfoPtr_set_AllControlPointsAreDirty_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664876);
			NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664877);
			NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664878);
			NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664879);
			NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_CurvySplineSegment_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664880);
			NativeMethodInfoPtr_ClearMinimalSet_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664881);
			NativeMethodInfoPtr_RemoveFromMinimalSet_Public_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664882);
			NativeMethodInfoPtr_ProcessDirtyControlPoints_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664883);
			NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664884);
			NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664885);
			NativeMethodInfoPtr_ProcessDirtyOrientation_Private_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664886);
			NativeMethodInfoPtr_ProcessDirtyDynamicOrientation_Private_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664887);
			NativeMethodInfoPtr_ProcessDirtyCurve_Private_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664888);
			NativeMethodInfoPtr_SetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_SplineDirtyingType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664889);
			NativeMethodInfoPtr_FillDirtyCpsExtendedList_Private_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664890);
			NativeMethodInfoPtr_AddToMinimalSetAndSetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_CurvySplineSegment_SplineDirtyingType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664891);
			NativeMethodInfoPtr_ValidateConnectedSplines_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664892);
			NativeMethodInfoPtr_SynchronizeSplinesWithNullCps_Private_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664893);
			NativeMethodInfoPtr_SynchronizeUninitializedSplines_Private_Static_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664894);
			NativeMethodInfoPtr_DoSanityChecks_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664895);
			NativeMethodInfoPtr__ValidateConnectedSplines_b__35_2_Private_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100664896);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37036, RefRangeEnd = 37037, XrefRangeStart = 36971, XrefRangeEnd = 37036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DirtinessManager(CurvySpline spline)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(15)]
		[CachedScanResults(RefRangeStart = 37069, RefRangeEnd = 37084, XrefRangeStart = 37037, XrefRangeEnd = 37069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetDirtyAll(SplineDirtyingType dirtyingType, bool dirtyConnectedControlPoints)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&dirtyingType);
			*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyConnectedControlPoints;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 37145, RefRangeEnd = 37149, XrefRangeStart = 37084, XrefRangeEnd = 37145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetDirty(CurvySplineSegment controlPoint, SplineDirtyingType dirtyingType, CurvySplineSegment previousControlPoint, CurvySplineSegment nextControlPoint, bool ignoreConnectionOfInputControlPoint)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[5];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
			*(SplineDirtyingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyingType;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)previousControlPoint);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nextControlPoint);
			*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &ignoreConnectionOfInputControlPoint;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_CurvySplineSegment_CurvySplineSegment_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37149, XrefRangeEnd = 37153, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ClearMinimalSet()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearMinimalSet_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37153, XrefRangeEnd = 37157, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void RemoveFromMinimalSet(CurvySplineSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveFromMinimalSet_Public_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(19)]
		[CachedScanResults(RefRangeStart = 37160, RefRangeEnd = 37179, XrefRangeStart = 37157, XrefRangeEnd = 37160, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool ProcessDirtyControlPoints()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessDirtyControlPoints_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37179, XrefRangeEnd = 37182, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37182, XrefRangeEnd = 37186, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37186, XrefRangeEnd = 37220, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ProcessDirtyOrientation(List<CurvySplineSegment> dirtyCpsExtendedList)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyCpsExtendedList);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessDirtyOrientation_Private_Void_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37256, RefRangeEnd = 37257, XrefRangeStart = 37220, XrefRangeEnd = 37256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ProcessDirtyDynamicOrientation(List<CurvySplineSegment> dirtyCpsExtendedList)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyCpsExtendedList);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessDirtyDynamicOrientation_Private_Void_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37257, XrefRangeEnd = 37286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ProcessDirtyCurve(List<CurvySplineSegment> dirtyCpsExtendedList)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyCpsExtendedList);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessDirtyCurve_Private_Void_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37286, XrefRangeEnd = 37287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded(SplineDirtyingType dirtyingType)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&dirtyingType);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_SplineDirtyingType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37287, XrefRangeEnd = 37350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void FillDirtyCpsExtendedList(List<CurvySplineSegment> dirtyCpsExtendedList)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyCpsExtendedList);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FillDirtyCpsExtendedList_Private_Void_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 37354, RefRangeEnd = 37359, XrefRangeStart = 37350, XrefRangeEnd = 37354, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void AddToMinimalSetAndSetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded(CurvySplineSegment controlPoint, SplineDirtyingType dirtyingType)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
			*(SplineDirtyingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyingType;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddToMinimalSetAndSetDirtyingFlagsAndInvalidateSplineCurveCachesIfNeeded_Private_Void_CurvySplineSegment_SplineDirtyingType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37359, XrefRangeEnd = 37407, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ValidateConnectedSplines()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValidateConnectedSplines_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37480, RefRangeEnd = 37481, XrefRangeStart = 37407, XrefRangeEnd = 37480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SynchronizeSplinesWithNullCps(List<CurvySplineSegment> controlPoints)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoints);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SynchronizeSplinesWithNullCps_Private_Void_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37526, RefRangeEnd = 37527, XrefRangeStart = 37481, XrefRangeEnd = 37526, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void SynchronizeUninitializedSplines(List<CurvySplineSegment> connectedCPs)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)connectedCPs);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SynchronizeUninitializedSplines_Private_Static_Void_List_1_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe void DoSanityChecks()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoSanityChecks_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37527, XrefRangeEnd = 37532, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _ValidateConnectedSplines_b__35_2(CurvySplineSegment cp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cp);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ValidateConnectedSplines_b__35_2_Private_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public DirtinessManager(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class OrientationGroup : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_segments;

		private static readonly System.IntPtr NativeFieldInfoPtr_currentMetrics;

		private static readonly System.IntPtr NativeFieldInfoPtr_accumulatedSwirlAngles;

		private static readonly System.IntPtr NativeFieldInfoPtr_accumulatedCacheSizes;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Segments_Public_get_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetupOrientationGroup_Public_Void_Int16_List_1_CurvySplineSegment_Il2CppStructArray_1_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_UpdateOrientation_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplySwirlAndSmoothing_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyParallelTransport_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationGap_Private_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAccumulatedSwirlAngles_Private_Il2CppStructArray_1_Single_0;

		public unsafe List<CurvySplineSegment> segments
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe SegmentGroupMetrics currentMetrics
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentMetrics);
				return *(SegmentGroupMetrics*)num;
			}
			set
			{
				*(SegmentGroupMetrics*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentMetrics)) = segmentGroupMetrics;
			}
		}

		public unsafe Il2CppStructArray<float> accumulatedSwirlAngles
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_accumulatedSwirlAngles);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_accumulatedSwirlAngles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe List<int> accumulatedCacheSizes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_accumulatedCacheSizes);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<int>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_accumulatedCacheSizes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe List<CurvySplineSegment> Segments
		{
			[CallerCount(12)]
			[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Segments_Public_get_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
			}
		}

		static OrientationGroup()
		{
			Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "OrientationGroup");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr);
			NativeFieldInfoPtr_segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, "segments");
			NativeFieldInfoPtr_currentMetrics = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, "currentMetrics");
			NativeFieldInfoPtr_accumulatedSwirlAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, "accumulatedSwirlAngles");
			NativeFieldInfoPtr_accumulatedCacheSizes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, "accumulatedCacheSizes");
			NativeMethodInfoPtr_get_Segments_Public_get_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664906);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664907);
			NativeMethodInfoPtr_SetupOrientationGroup_Public_Void_Int16_List_1_CurvySplineSegment_Il2CppStructArray_1_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664908);
			NativeMethodInfoPtr_UpdateOrientation_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664909);
			NativeMethodInfoPtr_ApplySwirlAndSmoothing_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664910);
			NativeMethodInfoPtr_ApplyParallelTransport_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664911);
			NativeMethodInfoPtr_GetOrientationGap_Private_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664912);
			NativeMethodInfoPtr_GetAccumulatedSwirlAngles_Private_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr, 100664913);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37532, XrefRangeEnd = 37551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe OrientationGroup()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OrientationGroup>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37568, RefRangeEnd = 37569, XrefRangeStart = 37551, XrefRangeEnd = 37568, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetupOrientationGroup(short anchorIndex, List<CurvySplineSegment> splineControlPoints, Il2CppStructArray<short> orientationAnchorIndices)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&anchorIndex);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splineControlPoints);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)orientationAnchorIndices);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupOrientationGroup_Public_Void_Int16_List_1_CurvySplineSegment_Il2CppStructArray_1_Int16_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37569, XrefRangeEnd = 37582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void UpdateOrientation()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateOrientation_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 37609, RefRangeEnd = 37611, XrefRangeStart = 37582, XrefRangeEnd = 37609, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ApplySwirlAndSmoothing()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplySwirlAndSmoothing_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37611, XrefRangeEnd = 37621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ApplyParallelTransport()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyParallelTransport_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37621, XrefRangeEnd = 37630, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe float GetOrientationGap()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationGap_Private_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37652, RefRangeEnd = 37653, XrefRangeStart = 37630, XrefRangeEnd = 37652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Il2CppStructArray<float> GetAccumulatedSwirlAngles()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAccumulatedSwirlAngles_Private_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}

		public OrientationGroup(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class RelationshipCache : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_spline;

		private static readonly System.IntPtr NativeFieldInfoPtr_lockObject;

		private static readonly System.IntPtr NativeFieldInfoPtr_firstSegment;

		private static readonly System.IntPtr NativeFieldInfoPtr_lastSegment;

		private static readonly System.IntPtr NativeFieldInfoPtr_firstVisibleControlPoint;

		private static readonly System.IntPtr NativeFieldInfoPtr_lastVisibleControlPoint;

		private static readonly System.IntPtr NativeFieldInfoPtr__IsValid_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_IsValid_Private_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invalidate_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EnsureIsValid_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RebuildAndFixNonCoherentControlPoints_Private_Void_0;

		public unsafe CurvySpline spline
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe Il2CppSystem.Object lockObject
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lockObject);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lockObject)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe CurvySplineSegment firstSegment
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_firstSegment);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_firstSegment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
			}
		}

		public unsafe CurvySplineSegment lastSegment
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastSegment);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastSegment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
			}
		}

		public unsafe CurvySplineSegment firstVisibleControlPoint
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_firstVisibleControlPoint);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_firstVisibleControlPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
			}
		}

		public unsafe CurvySplineSegment lastVisibleControlPoint
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastVisibleControlPoint);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastVisibleControlPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineSegment));
			}
		}

		public unsafe bool _IsValid_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsValid_k__BackingField);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__IsValid_k__BackingField)) = flag;
			}
		}

		public unsafe CurvySplineSegment FirstVisibleControlPoint
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37653, XrefRangeEnd = 37654, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
		}

		public unsafe CurvySplineSegment LastVisibleControlPoint
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37654, XrefRangeEnd = 37655, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
		}

		public unsafe CurvySplineSegment FirstSegment
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37655, XrefRangeEnd = 37656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
		}

		public unsafe CurvySplineSegment LastSegment
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37656, XrefRangeEnd = 37657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
			}
		}

		public unsafe bool IsValid
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsValid_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static RelationshipCache()
		{
			Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "RelationshipCache");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr);
			NativeFieldInfoPtr_spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "spline");
			NativeFieldInfoPtr_lockObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "lockObject");
			NativeFieldInfoPtr_firstSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "firstSegment");
			NativeFieldInfoPtr_lastSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "lastSegment");
			NativeFieldInfoPtr_firstVisibleControlPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "firstVisibleControlPoint");
			NativeFieldInfoPtr_lastVisibleControlPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "lastVisibleControlPoint");
			NativeFieldInfoPtr__IsValid_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, "<IsValid>k__BackingField");
			NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664914);
			NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664915);
			NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664916);
			NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664917);
			NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664918);
			NativeMethodInfoPtr_set_IsValid_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664919);
			NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664920);
			NativeMethodInfoPtr_Invalidate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664921);
			NativeMethodInfoPtr_EnsureIsValid_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664922);
			NativeMethodInfoPtr_RebuildAndFixNonCoherentControlPoints_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr, 100664923);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37657, XrefRangeEnd = 37664, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RelationshipCache(CurvySpline spline)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RelationshipCache>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(15)]
		[CachedScanResults(RefRangeStart = 37672, RefRangeEnd = 37687, XrefRangeStart = 37664, XrefRangeEnd = 37672, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Invalidate()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invalidate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37687, XrefRangeEnd = 37688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void EnsureIsValid()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureIsValid_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(102)]
		[CachedScanResults(RefRangeStart = 37759, RefRangeEnd = 37861, XrefRangeStart = 37688, XrefRangeEnd = 37759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void RebuildAndFixNonCoherentControlPoints()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RebuildAndFixNonCoherentControlPoints_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public RelationshipCache(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class SanityChecker : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_spline;

		private static readonly System.IntPtr NativeFieldInfoPtr_sanityErrorLogsThisFrame;

		private static readonly System.IntPtr NativeFieldInfoPtr_sanityWaringLogsThisFrame;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_OnUpdate_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Check_Public_Void_0;

		public unsafe CurvySpline spline
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe int sanityErrorLogsThisFrame
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityErrorLogsThisFrame);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityErrorLogsThisFrame)) = num;
			}
		}

		public unsafe int sanityWaringLogsThisFrame
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityWaringLogsThisFrame);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityWaringLogsThisFrame)) = num;
			}
		}

		static SanityChecker()
		{
			Il2CppClassPointerStore<SanityChecker>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "SanityChecker");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr);
			NativeFieldInfoPtr_spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, "spline");
			NativeFieldInfoPtr_sanityErrorLogsThisFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, "sanityErrorLogsThisFrame");
			NativeFieldInfoPtr_sanityWaringLogsThisFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, "sanityWaringLogsThisFrame");
			NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, 100664924);
			NativeMethodInfoPtr_OnUpdate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, 100664925);
			NativeMethodInfoPtr_Check_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr, 100664926);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SanityChecker(CurvySpline spline)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SanityChecker>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spline);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe void OnUpdate()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUpdate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37861, XrefRangeEnd = 37878, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Check()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Check_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SanityChecker(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct SegmentGroupMetrics
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_CacheSize;

		private static readonly System.IntPtr NativeFieldInfoPtr_SegmentCount;

		private static readonly System.IntPtr NativeFieldInfoPtr_Length;

		private static readonly System.IntPtr NativeMethodInfoPtr_Increment_Public_Void_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SegmentGroupMetrics_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0;

		[FieldOffset(0)]
		public int CacheSize;

		[FieldOffset(4)]
		public int SegmentCount;

		[FieldOffset(8)]
		public float Length;

		static SegmentGroupMetrics()
		{
			Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "SegmentGroupMetrics");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr);
			NativeFieldInfoPtr_CacheSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, "CacheSize");
			NativeFieldInfoPtr_SegmentCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, "SegmentCount");
			NativeFieldInfoPtr_Length = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, "Length");
			NativeMethodInfoPtr_Increment_Public_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664927);
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SegmentGroupMetrics_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664928);
			NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664929);
			NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664930);
			NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664931);
			NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, 100664932);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 37878, RefRangeEnd = 37879, XrefRangeStart = 37878, XrefRangeEnd = 37878, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Increment(CurvySplineSegment segment)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Increment_Public_Void_CurvySplineSegment_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37879, XrefRangeEnd = 37880, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool Equals(SegmentGroupMetrics other)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&other);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SegmentGroupMetrics_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37880, XrefRangeEnd = 37884, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool Equals(Il2CppSystem.Object obj)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37884, XrefRangeEnd = 37885, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe override int GetHashCode()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37885, XrefRangeEnd = 37886, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool operator ==(SegmentGroupMetrics left, SegmentGroupMetrics right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&left);
			*(SegmentGroupMetrics**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37886, XrefRangeEnd = 37887, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static bool operator !=(SegmentGroupMetrics left, SegmentGroupMetrics right)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&left);
			*(SegmentGroupMetrics**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_SegmentGroupMetrics_SegmentGroupMetrics_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SegmentGroupMetrics>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public class ControlPointNamer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_spline;

		private static readonly System.IntPtr NativeFieldInfoPtr_requestRename;

		private static readonly System.IntPtr NativeFieldInfoPtr_ControlPointNames;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RequestRename_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessRequests_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CancelRequests_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RenameControlPoints_Private_Static_Void_List_1_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointName_Private_Static_String_Int16_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointNames_Private_Static_Il2CppStringArray_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MakeControlPointName_Private_Static_String_Int16_0;

		public unsafe CurvySpline spline
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spline)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline));
			}
		}

		public unsafe bool requestRename
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_requestRename);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_requestRename)) = flag;
			}
		}

		public unsafe static Il2CppStringArray ControlPointNames
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ControlPointNames, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ControlPointNames, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		static ControlPointNamer()
		{
			Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "ControlPointNamer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr);
			NativeFieldInfoPtr_spline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, "spline");
			NativeFieldInfoPtr_requestRename = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, "requestRename");
			NativeFieldInfoPtr_ControlPointNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, "ControlPointNames");
			NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664933);
			NativeMethodInfoPtr_RequestRename_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664934);
			NativeMethodInfoPtr_ProcessRequests_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664935);
			NativeMethodInfoPtr_CancelRequests_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664936);
			NativeMethodInfoPtr_RenameControlPoints_Private_Static_Void_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664937);
			NativeMethodInfoPtr_GetControlPointName_Private_Static_String_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664938);
			NativeMethodInfoPtr_GetControlPointNames_Private_Static_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664939);
			NativeMethodInfoPtr_MakeControlPointName_Private_Static_String_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr, 100664940);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ControlPointNamer(CurvySpline curvySpline)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ControlPointNamer>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySpline);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 37887, RefRangeEnd = 37898, XrefRangeStart = 37887, XrefRangeEnd = 37887, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void RequestRename()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RequestRename_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37898, XrefRangeEnd = 37917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ProcessRequests()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessRequests_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 37917, RefRangeEnd = 37919, XrefRangeStart = 37917, XrefRangeEnd = 37917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CancelRequests()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CancelRequests_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37919, XrefRangeEnd = 37935, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void RenameControlPoints(List<CurvySplineSegment> splineControlPoints)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)splineControlPoints);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenameControlPoints_Private_Static_Void_List_1_CurvySplineSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37935, XrefRangeEnd = 37939, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string GetControlPointName(short controlPointIndex)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&controlPointIndex);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointName_Private_Static_String_Int16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37939, XrefRangeEnd = 37956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static Il2CppStringArray GetControlPointNames()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointNames_Private_Static_Il2CppStringArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 37966, RefRangeEnd = 37968, XrefRangeStart = 37956, XrefRangeEnd = 37966, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static string MakeControlPointName(short controlPointIndex)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&controlPointIndex);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeControlPointName_Private_Static_String_Int16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		public ControlPointNamer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.CurvySpline+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__5_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__168_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__171_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__173_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___ctor_b__5_0_Internal_Void_CurvySplineSegment_Int32_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetPositionsCache_b__168_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetTangentsCache_b__171_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetNormalsCache_b__173_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0;

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

		public unsafe static Il2CppSystem.Action<CurvySplineSegment, int, int> __9__5_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__5_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySplineSegment, int, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__5_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe static Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>> __9__168_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__168_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__168_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>> __9__171_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__171_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__171_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>> __9__173_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__173_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__173_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__5_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__5_0");
			NativeFieldInfoPtr___9__168_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__168_0");
			NativeFieldInfoPtr___9__171_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__171_0");
			NativeFieldInfoPtr___9__173_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__173_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664943);
			NativeMethodInfoPtr___ctor_b__5_0_Internal_Void_CurvySplineSegment_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664944);
			NativeMethodInfoPtr__GetPositionsCache_b__168_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664945);
			NativeMethodInfoPtr__GetTangentsCache_b__171_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664946);
			NativeMethodInfoPtr__GetNormalsCache_b__173_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664947);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 37968, XrefRangeEnd = 37970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void __ctor_b__5_0(CurvySplineSegment controlPoint, int controlPointIndex, int controlPointsCount)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointIndex;
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointsCount;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___ctor_b__5_0_Internal_Void_CurvySplineSegment_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe SubArray<Vector3> _GetPositionsCache_b__168_0(CurvySplineSegment s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetPositionsCache_b__168_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}

		[CallerCount(0)]
		public unsafe SubArray<Vector3> _GetTangentsCache_b__171_0(CurvySplineSegment s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetTangentsCache_b__171_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}

		[CallerCount(0)]
		public unsafe SubArray<Vector3> _GetNormalsCache_b__173_0(CurvySplineSegment s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetNormalsCache_b__173_0_Internal_SubArray_1_Vector3_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_GetMetadata_Public_T_Single_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetMetadata_Public_T_Single_0, Il2CppClassPointerStore<CurvySpline>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetInterpolatedMetadata_Public_U_Single_0<T, U>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0, Il2CppClassPointerStore<CurvySpline>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[2]
		{
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)),
			Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<U>.NativeClassPtr))
		}))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_VERSION;

	private static readonly System.IntPtr NativeFieldInfoPtr_APIVERSION;

	private static readonly System.IntPtr NativeFieldInfoPtr_WEBROOT;

	private static readonly System.IntPtr NativeFieldInfoPtr_DOCLINK;

	private static readonly System.IntPtr NativeFieldInfoPtr_ShowGizmos;

	private static readonly System.IntPtr NativeFieldInfoPtr_ControlPoints;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Interpolation;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RestrictTo2D;

	private static readonly System.IntPtr NativeFieldInfoPtr_restricted2DPlane;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Closed;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoEndTangents;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Orientation;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoHandleDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Tension;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Continuity;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Bias;

	private static readonly System.IntPtr NativeFieldInfoPtr_bSplineDegree;

	private static readonly System.IntPtr NativeFieldInfoPtr_isBSplineClamped;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_GizmoColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_GizmoSelectionColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_CacheDensity;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxPointsPerUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_UsePooling;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_UseThreading;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_CheckTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_UpdateIn;

	private static readonly System.IntPtr NativeFieldInfoPtr_onInitialized;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnRefresh;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnAfterControlPointChanges;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnBeforeControlPointAdd;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnAfterControlPointAdd;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnBeforeControlPointDelete;

	private static readonly System.IntPtr NativeFieldInfoPtr_onGlobalCoordinatesChanged;

	private static readonly System.IntPtr NativeFieldInfoPtr_mIsInitialized;

	private static readonly System.IntPtr NativeFieldInfoPtr_isStarted;

	private static readonly System.IntPtr NativeFieldInfoPtr_sendOnRefreshEventNextUpdate;

	private static readonly System.IntPtr NativeFieldInfoPtr_mSegments;

	private static readonly System.IntPtr NativeFieldInfoPtr_dirtinessManager;

	private static readonly System.IntPtr NativeFieldInfoPtr_relationshipCache;

	private static readonly System.IntPtr NativeFieldInfoPtr_sanityChecker;

	private static readonly System.IntPtr NativeFieldInfoPtr_cpsSynchronizer;

	private static readonly System.IntPtr NativeFieldInfoPtr_controlPointNamer;

	private static readonly System.IntPtr NativeFieldInfoPtr_transformMonitor;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedTransform;

	private static readonly System.IntPtr NativeFieldInfoPtr_readOnlyControlPoints;

	private static readonly System.IntPtr NativeFieldInfoPtr_cachedShortsArray;

	private static readonly System.IntPtr NativeFieldInfoPtr_controlPointsDistances;

	private static readonly System.IntPtr NativeFieldInfoPtr_refreshCurveAction;

	private static readonly System.IntPtr NativeFieldInfoPtr_length;

	private static readonly System.IntPtr NativeFieldInfoPtr_mCacheSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_mBounds;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultSplineEventArgs;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultAddAfterEventArgs;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultDeleteEventArgs;

	private static readonly System.IntPtr NativeFieldInfoPtr_CachedControlPointsNameCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinimalMaxPointsPerUnit;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxSegmentCacheSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_InvalidCPErrorMessage;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinBSplineDegree;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Interpolation_Public_get_CurvyInterpolation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Interpolation_Public_set_Void_CurvyInterpolation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RestrictTo2D_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RestrictTo2D_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Restricted2DPlane_Public_get_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Restricted2DPlane_Public_set_Void_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoEndTangents_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoEndTangents_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Orientation_Public_get_CurvyOrientation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Orientation_Public_set_Void_CurvyOrientation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UpdateIn_Public_get_CurvyUpdateMethod_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UpdateIn_Public_set_Void_CurvyUpdateMethod_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_GizmoColor_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_GizmoColor_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_GizmoSelectionColor_Public_get_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_GizmoSelectionColor_Public_set_Void_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CacheDensity_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CacheDensity_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxPointsPerUnit_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_MaxPointsPerUnit_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UsePooling_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UsePooling_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UseThreading_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UseThreading_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CheckTransform_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CheckTransform_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Tension_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Tension_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Continuity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Continuity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Bias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BSplineDegree_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_BSplineDegree_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsBSplineClamped_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_IsBSplineClamped_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsInitialized_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ControlPointCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Length_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_CurvySplineSegment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ControlPointsList_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_GlobalCoordinatesChangedThisFrame_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnGlobalCoordinatesChanged_Public_get_Action_1_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnGlobalCoordinatesChanged_Public_set_Void_Action_1_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnInitialized_Public_get_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnInitialized_Public_set_Void_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnAfterControlPointChanges_Public_get_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnAfterControlPointChanges_Public_set_Void_CurvySplineEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnBeforeControlPointAdd_Public_get_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnBeforeControlPointAdd_Public_set_Void_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnAfterControlPointAdd_Public_get_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnAfterControlPointAdd_Public_set_Void_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnBeforeControlPointDelete_Public_get_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnBeforeControlPointDelete_Public_set_Void_CurvyControlPointEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Static_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Static_CurvySpline_CurvySpline_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateCacheSize_Public_Static_Int32_Int32_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateSamplingPointsPerUnit_Public_Static_Single_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Bezier_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BezierTangent_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CatmullRom_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TCB_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_Single_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFollowUpHeadingControlPoint_Public_Static_CurvySplineSegment_CurvySplineSegment_ConnectionHeadingEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateByDistance_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateByDistanceFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangentByDistance_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangentByDistanceFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMetadata_Public_T_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToDistance_Public_Single_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceToTF_Public_Single_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_CurvySplineSegment_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPositionsCache_Public_SubArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Single_Single_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTangentsCache_Public_SubArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetApproximationT_Public_Il2CppStructArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNormalsCache_Public_SubArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetApproximationUpVectors_Public_Il2CppStructArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPoint_Public_Vector3_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Int32_Int32_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Int32_Int32_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_byref_CurvySplineSegment_byref_Single_Int32_Int32_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetDirtyAll_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetDirtyPartial_Public_Void_CurvySplineSegment_SplineDirtyingType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToWorldPosition_Public_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToWorldDirection_Public_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToLocalPosition_Public_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToLocalDirection_Public_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyControlPointsNames_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SyncSplineFromHierarchy_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Boolean_byref_Boolean_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsPlanar_Public_Boolean_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakePlanar_Public_Void_CurvyPlane_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakePlanar_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Subdivide_Public_Void_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Simplify_Public_Void_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equalize_Public_Void_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Normalize_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPivot_Public_Vector3_Single_Single_Single_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Flip_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MoveControlPoints_Public_Void_Int32_Int32_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_JoinWith_Public_Void_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Split_Public_CurvySpline_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetFirstControlPoint_Public_Void_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Public_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanControlPointHaveFollowUp_Public_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointIndex_Public_Int16_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSegmentIndex_Public_Int16_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextControlPointIndex_Public_Int16_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreviousControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreviousControlPointIndex_Public_Int16_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreviousControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextSegment_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreviousSegment_Public_CurvySplineSegment_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsControlPointASegment_Public_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsControlPointVisible_Public_Boolean_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetControlPointOrientationAnchorIndex_Public_Int16_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetFromString_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TransformMonitor_Private_get_TransformMonitor_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FixedUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Segments_Private_get_List_1_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxBSplineDegree_Private_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HookEditorUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnhookEditorUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DoUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearBounds_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanHaveManualEndCp_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanBeClamped_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReverseControlPoints_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNextControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPreviousControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsControlPointASegment_Private_Static_Boolean_Int32_Int32_Boolean_Boolean_Boolean_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Private_Static_Boolean_Boolean_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddControlPoint_Private_Void_CurvySplineSegment_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertControlPoint_Private_Void_Int32_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveControlPoint_Private_Void_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearControlPoints_Private_Void_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InvalidateControlPointsRelationshipCacheINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateControlPointDistances_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnforceTangentContinuity_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PrepareThreadCompatibleData_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrientationAnchorIndices_Private_Il2CppStructArray_1_Int16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InvalidateAccumulators_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NotifyMetaDataModification_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisposeOfControlPoint_Private_Void_CurvySplineSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShouldUseControlPointPooling_Private_Boolean_byref_CurvyGlobalManager_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InsertAt_Private_CurvySplineSegment_CurvySplineSegment_Vector3_Int32_ModeEnum_Boolean_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AcquireNewControlPoint_Private_CurvySplineSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSegmentApproximationsInSpace_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_Space_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConcatenateSegmentApproximations_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransformToWorldSpace_Private_Void_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PushChildCPsToPool_Private_Void_ComponentPool_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnRefreshEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBeforeControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnAfterControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBeforeControlPointDeleteEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnAfterControlPointChangesEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0;

	public unsafe static string VERSION
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_VERSION, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_VERSION, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string APIVERSION
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_APIVERSION, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_APIVERSION, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string WEBROOT
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_WEBROOT, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_WEBROOT, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string DOCLINK
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DOCLINK, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DOCLINK, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool ShowGizmos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowGizmos);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowGizmos)) = flag;
		}
	}

	public unsafe List<CurvySplineSegment> ControlPoints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ControlPoints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe CurvyInterpolation m_Interpolation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Interpolation);
			return *(CurvyInterpolation*)num;
		}
		set
		{
			*(CurvyInterpolation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Interpolation)) = curvyInterpolation;
		}
	}

	public unsafe bool m_RestrictTo2D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RestrictTo2D);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RestrictTo2D)) = flag;
		}
	}

	public unsafe CurvyPlane restricted2DPlane
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_restricted2DPlane);
			return *(CurvyPlane*)num;
		}
		set
		{
			*(CurvyPlane*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_restricted2DPlane)) = curvyPlane;
		}
	}

	public unsafe bool m_Closed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Closed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Closed)) = flag;
		}
	}

	public unsafe bool m_AutoEndTangents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoEndTangents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoEndTangents)) = flag;
		}
	}

	public unsafe CurvyOrientation m_Orientation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Orientation);
			return *(CurvyOrientation*)num;
		}
		set
		{
			*(CurvyOrientation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Orientation)) = curvyOrientation;
		}
	}

	public unsafe float m_AutoHandleDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandleDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoHandleDistance)) = num;
		}
	}

	public unsafe float m_Tension
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Tension);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Tension)) = num;
		}
	}

	public unsafe float m_Continuity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Continuity);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Continuity)) = num;
		}
	}

	public unsafe float m_Bias
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Bias);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Bias)) = num;
		}
	}

	public unsafe int bSplineDegree
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bSplineDegree);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bSplineDegree)) = num;
		}
	}

	public unsafe bool isBSplineClamped
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isBSplineClamped);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isBSplineClamped)) = flag;
		}
	}

	public unsafe Color m_GizmoColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GizmoColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GizmoColor)) = color;
		}
	}

	public unsafe Color m_GizmoSelectionColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GizmoSelectionColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_GizmoSelectionColor)) = color;
		}
	}

	public unsafe int m_CacheDensity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CacheDensity);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CacheDensity)) = num;
		}
	}

	public unsafe float m_MaxPointsPerUnit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MaxPointsPerUnit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MaxPointsPerUnit)) = num;
		}
	}

	public unsafe bool m_UsePooling
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UsePooling);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UsePooling)) = flag;
		}
	}

	public unsafe bool m_UseThreading
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseThreading);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UseThreading)) = flag;
		}
	}

	public unsafe bool m_CheckTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CheckTransform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_CheckTransform)) = flag;
		}
	}

	public unsafe CurvyUpdateMethod m_UpdateIn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UpdateIn);
			return *(CurvyUpdateMethod*)num;
		}
		set
		{
			*(CurvyUpdateMethod*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UpdateIn)) = curvyUpdateMethod;
		}
	}

	public unsafe CurvySplineEvent onInitialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInitialized);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInitialized)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineEvent));
		}
	}

	public unsafe CurvySplineEvent m_OnRefresh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnRefresh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnRefresh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineEvent));
		}
	}

	public unsafe CurvySplineEvent m_OnAfterControlPointChanges
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnAfterControlPointChanges);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnAfterControlPointChanges)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvySplineEvent));
		}
	}

	public unsafe CurvyControlPointEvent m_OnBeforeControlPointAdd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeControlPointAdd);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeControlPointAdd)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyControlPointEvent));
		}
	}

	public unsafe CurvyControlPointEvent m_OnAfterControlPointAdd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnAfterControlPointAdd);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnAfterControlPointAdd)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyControlPointEvent));
		}
	}

	public unsafe CurvyControlPointEvent m_OnBeforeControlPointDelete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeControlPointDelete);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeControlPointDelete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyControlPointEvent));
		}
	}

	public unsafe Il2CppSystem.Action<CurvySpline> onGlobalCoordinatesChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onGlobalCoordinatesChanged);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySpline>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onGlobalCoordinatesChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe bool mIsInitialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mIsInitialized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mIsInitialized)) = flag;
		}
	}

	public unsafe bool isStarted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isStarted);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isStarted)) = flag;
		}
	}

	public unsafe bool sendOnRefreshEventNextUpdate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sendOnRefreshEventNextUpdate);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sendOnRefreshEventNextUpdate)) = flag;
		}
	}

	public unsafe List<CurvySplineSegment> mSegments
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSegments);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mSegments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe DirtinessManager dirtinessManager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtinessManager);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DirtinessManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dirtinessManager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtinessManager));
		}
	}

	public unsafe RelationshipCache relationshipCache
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_relationshipCache);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RelationshipCache>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_relationshipCache)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)relationshipCache));
		}
	}

	public unsafe SanityChecker sanityChecker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityChecker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SanityChecker>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sanityChecker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sanityChecker));
		}
	}

	public unsafe ControlPointsSynchronizer cpsSynchronizer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cpsSynchronizer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ControlPointsSynchronizer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cpsSynchronizer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPointsSynchronizer));
		}
	}

	public unsafe ControlPointNamer controlPointNamer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointNamer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ControlPointNamer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointNamer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPointNamer));
		}
	}

	public unsafe TransformMonitor transformMonitor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformMonitor);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TransformMonitor>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformMonitor)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transformMonitor));
		}
	}

	public unsafe Transform cachedTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedTransform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe ReadOnlyCollection<CurvySplineSegment> readOnlyControlPoints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_readOnlyControlPoints);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ReadOnlyCollection<CurvySplineSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_readOnlyControlPoints)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)readOnlyCollection));
		}
	}

	public unsafe Il2CppStructArray<short> cachedShortsArray
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedShortsArray);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<short>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedShortsArray)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStructArray<float> controlPointsDistances
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointsDistances);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controlPointsDistances)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppSystem.Action<CurvySplineSegment, int, int> refreshCurveAction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refreshCurveAction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySplineSegment, int, int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refreshCurveAction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe float length
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length)) = num;
		}
	}

	public unsafe int mCacheSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCacheSize);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mCacheSize)) = num;
		}
	}

	public unsafe Il2CppSystem.Nullable<Bounds> mBounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds);
			return new Il2CppSystem.Nullable<Bounds>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe CurvySplineEventArgs defaultSplineEventArgs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSplineEventArgs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEventArgs>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultSplineEventArgs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e));
		}
	}

	public unsafe CurvyControlPointEventArgs defaultAddAfterEventArgs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultAddAfterEventArgs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEventArgs>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultAddAfterEventArgs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e));
		}
	}

	public unsafe CurvyControlPointEventArgs defaultDeleteEventArgs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultDeleteEventArgs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEventArgs>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultDeleteEventArgs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e));
		}
	}

	public unsafe static short CachedControlPointsNameCount
	{
		get
		{
			Unsafe.SkipInit(out short result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CachedControlPointsNameCount, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CachedControlPointsNameCount, (void*)(&num));
		}
	}

	public unsafe static float MinimalMaxPointsPerUnit
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinimalMaxPointsPerUnit, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinimalMaxPointsPerUnit, (void*)(&num));
		}
	}

	public unsafe static float MaxSegmentCacheSize
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxSegmentCacheSize, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxSegmentCacheSize, (void*)(&num));
		}
	}

	public unsafe static string InvalidCPErrorMessage
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_InvalidCPErrorMessage, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_InvalidCPErrorMessage, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static int MinBSplineDegree
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinBSplineDegree, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinBSplineDegree, (void*)(&num));
		}
	}

	public unsafe CurvyInterpolation Interpolation
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Interpolation_Public_get_CurvyInterpolation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyInterpolation*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 38102, RefRangeEnd = 38107, XrefRangeStart = 38094, XrefRangeEnd = 38102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Interpolation_Public_set_Void_CurvyInterpolation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RestrictTo2D
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RestrictTo2D_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 38112, RefRangeEnd = 38121, XrefRangeStart = 38107, XrefRangeEnd = 38112, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RestrictTo2D_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyPlane Restricted2DPlane
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38121, RefRangeEnd = 38127, XrefRangeStart = 38121, XrefRangeEnd = 38121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Restricted2DPlane_Public_get_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyPlane*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38127, XrefRangeEnd = 38132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Restricted2DPlane_Public_set_Void_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float AutoHandleDistance
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38138, RefRangeEnd = 38139, XrefRangeStart = 38132, XrefRangeEnd = 38138, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Closed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38143, RefRangeEnd = 38146, XrefRangeStart = 38139, XrefRangeEnd = 38143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool AutoEndTangents
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoEndTangents_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38150, RefRangeEnd = 38157, XrefRangeStart = 38146, XrefRangeEnd = 38150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoEndTangents_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyOrientation Orientation
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38157, RefRangeEnd = 38163, XrefRangeStart = 38157, XrefRangeEnd = 38157, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Orientation_Public_get_CurvyOrientation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyOrientation*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 38168, RefRangeEnd = 38177, XrefRangeStart = 38163, XrefRangeEnd = 38168, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Orientation_Public_set_Void_CurvyOrientation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyUpdateMethod UpdateIn
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38177, RefRangeEnd = 38183, XrefRangeStart = 38177, XrefRangeEnd = 38177, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UpdateIn_Public_get_CurvyUpdateMethod_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CurvyUpdateMethod*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UpdateIn_Public_set_Void_CurvyUpdateMethod_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color GizmoColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GizmoColor_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_GizmoColor_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Color GizmoSelectionColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GizmoSelectionColor_Public_get_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_GizmoSelectionColor_Public_set_Void_Color_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int CacheDensity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CacheDensity_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 38189, RefRangeEnd = 38194, XrefRangeStart = 38183, XrefRangeEnd = 38189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CacheDensity_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float MaxPointsPerUnit
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxPointsPerUnit_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38199, RefRangeEnd = 38205, XrefRangeStart = 38194, XrefRangeEnd = 38199, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MaxPointsPerUnit_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UsePooling
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UsePooling_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UsePooling_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool UseThreading
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UseThreading_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UseThreading_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool CheckTransform
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CheckTransform_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38210, RefRangeEnd = 38211, XrefRangeStart = 38205, XrefRangeEnd = 38210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CheckTransform_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Tension
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Tension_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38211, XrefRangeEnd = 38216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Tension_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Continuity
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Continuity_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38216, XrefRangeEnd = 38221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Continuity_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Bias
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bias_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38221, XrefRangeEnd = 38226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Bias_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int BSplineDegree
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38226, RefRangeEnd = 38232, XrefRangeStart = 38226, XrefRangeEnd = 38226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BSplineDegree_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38238, RefRangeEnd = 38241, XrefRangeStart = 38232, XrefRangeEnd = 38238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BSplineDegree_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsBSplineClamped
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38241, RefRangeEnd = 38244, XrefRangeStart = 38241, XrefRangeEnd = 38241, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsBSplineClamped_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38249, RefRangeEnd = 38250, XrefRangeStart = 38244, XrefRangeEnd = 38249, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsBSplineClamped_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool IsInitialized
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsInitialized_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Bounds Bounds
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38276, RefRangeEnd = 38277, XrefRangeStart = 38250, XrefRangeEnd = 38276, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Bounds*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int Count
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38279, RefRangeEnd = 38285, XrefRangeStart = 38277, XrefRangeEnd = 38279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Count_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int ControlPointCount
	{
		[CallerCount(20)]
		[CachedScanResults(RefRangeStart = 38286, RefRangeEnd = 38306, XrefRangeStart = 38285, XrefRangeEnd = 38286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPointCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int CacheSize
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38306, XrefRangeEnd = 38308, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float Length
	{
		[CallerCount(35)]
		[CachedScanResults(RefRangeStart = 38309, RefRangeEnd = 38344, XrefRangeStart = 38308, XrefRangeEnd = 38309, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Length_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool Dirty
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38345, RefRangeEnd = 38352, XrefRangeStart = 38344, XrefRangeEnd = 38345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvySplineSegment this[int idx]
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 38357, RefRangeEnd = 38363, XrefRangeStart = 38352, XrefRangeEnd = 38357, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&idx);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_CurvySplineSegment_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
	}

	public unsafe ReadOnlyCollection<CurvySplineSegment> ControlPointsList
	{
		[CallerCount(23)]
		[CachedScanResults(RefRangeStart = 38367, RefRangeEnd = 38390, XrefRangeStart = 38363, XrefRangeEnd = 38367, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ControlPointsList_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ReadOnlyCollection<CurvySplineSegment>>(intPtr) : null;
		}
	}

	public unsafe CurvySplineSegment FirstVisibleControlPoint
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38391, RefRangeEnd = 38398, XrefRangeStart = 38390, XrefRangeEnd = 38391, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
	}

	public unsafe CurvySplineSegment LastVisibleControlPoint
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 38399, RefRangeEnd = 38406, XrefRangeStart = 38398, XrefRangeEnd = 38399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
	}

	public unsafe CurvySplineSegment FirstSegment
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38407, RefRangeEnd = 38410, XrefRangeStart = 38406, XrefRangeEnd = 38407, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
	}

	public unsafe CurvySplineSegment LastSegment
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38410, XrefRangeEnd = 38411, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
		}
	}

	public unsafe bool GlobalCoordinatesChangedThisFrame
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38411, XrefRangeEnd = 38412, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GlobalCoordinatesChangedThisFrame_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Il2CppSystem.Action<CurvySpline> OnGlobalCoordinatesChanged
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnGlobalCoordinatesChanged_Public_get_Action_1_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CurvySpline>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38412, XrefRangeEnd = 38413, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnGlobalCoordinatesChanged_Public_set_Void_Action_1_CurvySpline_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineEvent OnRefresh
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38413, XrefRangeEnd = 38414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineEvent OnInitialized
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38414, RefRangeEnd = 38415, XrefRangeStart = 38414, XrefRangeEnd = 38414, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnInitialized_Public_get_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38415, XrefRangeEnd = 38416, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnInitialized_Public_set_Void_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvySplineEvent OnAfterControlPointChanges
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnAfterControlPointChanges_Public_get_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEvent>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 38417, RefRangeEnd = 38421, XrefRangeStart = 38416, XrefRangeEnd = 38417, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnAfterControlPointChanges_Public_set_Void_CurvySplineEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyControlPointEvent OnBeforeControlPointAdd
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38421, RefRangeEnd = 38424, XrefRangeStart = 38421, XrefRangeEnd = 38421, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnBeforeControlPointAdd_Public_get_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38424, XrefRangeEnd = 38425, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnBeforeControlPointAdd_Public_set_Void_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyControlPointEvent OnAfterControlPointAdd
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnAfterControlPointAdd_Public_get_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38425, XrefRangeEnd = 38426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnAfterControlPointAdd_Public_set_Void_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyControlPointEvent OnBeforeControlPointDelete
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnBeforeControlPointDelete_Public_get_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEvent>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38427, RefRangeEnd = 38428, XrefRangeStart = 38426, XrefRangeEnd = 38427, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnBeforeControlPointDelete_Public_set_Void_CurvyControlPointEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TransformMonitor TransformMonitor
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 39921, RefRangeEnd = 39925, XrefRangeStart = 39915, XrefRangeEnd = 39921, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TransformMonitor_Private_get_TransformMonitor_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TransformMonitor>(intPtr) : null;
		}
	}

	public unsafe List<CurvySplineSegment> Segments
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39982, XrefRangeEnd = 39983, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Segments_Private_get_List_1_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CurvySplineSegment>>(intPtr) : null;
		}
	}

	public unsafe int MaxBSplineDegree
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39983, XrefRangeEnd = 39984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxBSplineDegree_Private_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CurvySpline()
	{
		Il2CppClassPointerStore<CurvySpline>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "CurvySpline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr);
		NativeFieldInfoPtr_VERSION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "VERSION");
		NativeFieldInfoPtr_APIVERSION = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "APIVERSION");
		NativeFieldInfoPtr_WEBROOT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "WEBROOT");
		NativeFieldInfoPtr_DOCLINK = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "DOCLINK");
		NativeFieldInfoPtr_ShowGizmos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "ShowGizmos");
		NativeFieldInfoPtr_ControlPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "ControlPoints");
		NativeFieldInfoPtr_m_Interpolation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Interpolation");
		NativeFieldInfoPtr_m_RestrictTo2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_RestrictTo2D");
		NativeFieldInfoPtr_restricted2DPlane = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "restricted2DPlane");
		NativeFieldInfoPtr_m_Closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Closed");
		NativeFieldInfoPtr_m_AutoEndTangents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_AutoEndTangents");
		NativeFieldInfoPtr_m_Orientation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Orientation");
		NativeFieldInfoPtr_m_AutoHandleDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_AutoHandleDistance");
		NativeFieldInfoPtr_m_Tension = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Tension");
		NativeFieldInfoPtr_m_Continuity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Continuity");
		NativeFieldInfoPtr_m_Bias = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_Bias");
		NativeFieldInfoPtr_bSplineDegree = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "bSplineDegree");
		NativeFieldInfoPtr_isBSplineClamped = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "isBSplineClamped");
		NativeFieldInfoPtr_m_GizmoColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_GizmoColor");
		NativeFieldInfoPtr_m_GizmoSelectionColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_GizmoSelectionColor");
		NativeFieldInfoPtr_m_CacheDensity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_CacheDensity");
		NativeFieldInfoPtr_m_MaxPointsPerUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_MaxPointsPerUnit");
		NativeFieldInfoPtr_m_UsePooling = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_UsePooling");
		NativeFieldInfoPtr_m_UseThreading = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_UseThreading");
		NativeFieldInfoPtr_m_CheckTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_CheckTransform");
		NativeFieldInfoPtr_m_UpdateIn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_UpdateIn");
		NativeFieldInfoPtr_onInitialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "onInitialized");
		NativeFieldInfoPtr_m_OnRefresh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_OnRefresh");
		NativeFieldInfoPtr_m_OnAfterControlPointChanges = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_OnAfterControlPointChanges");
		NativeFieldInfoPtr_m_OnBeforeControlPointAdd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_OnBeforeControlPointAdd");
		NativeFieldInfoPtr_m_OnAfterControlPointAdd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_OnAfterControlPointAdd");
		NativeFieldInfoPtr_m_OnBeforeControlPointDelete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "m_OnBeforeControlPointDelete");
		NativeFieldInfoPtr_onGlobalCoordinatesChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "onGlobalCoordinatesChanged");
		NativeFieldInfoPtr_mIsInitialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "mIsInitialized");
		NativeFieldInfoPtr_isStarted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "isStarted");
		NativeFieldInfoPtr_sendOnRefreshEventNextUpdate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "sendOnRefreshEventNextUpdate");
		NativeFieldInfoPtr_mSegments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "mSegments");
		NativeFieldInfoPtr_dirtinessManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "dirtinessManager");
		NativeFieldInfoPtr_relationshipCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "relationshipCache");
		NativeFieldInfoPtr_sanityChecker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "sanityChecker");
		NativeFieldInfoPtr_cpsSynchronizer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "cpsSynchronizer");
		NativeFieldInfoPtr_controlPointNamer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "controlPointNamer");
		NativeFieldInfoPtr_transformMonitor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "transformMonitor");
		NativeFieldInfoPtr_cachedTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "cachedTransform");
		NativeFieldInfoPtr_readOnlyControlPoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "readOnlyControlPoints");
		NativeFieldInfoPtr_cachedShortsArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "cachedShortsArray");
		NativeFieldInfoPtr_controlPointsDistances = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "controlPointsDistances");
		NativeFieldInfoPtr_refreshCurveAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "refreshCurveAction");
		NativeFieldInfoPtr_length = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "length");
		NativeFieldInfoPtr_mCacheSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "mCacheSize");
		NativeFieldInfoPtr_mBounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "mBounds");
		NativeFieldInfoPtr_defaultSplineEventArgs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "defaultSplineEventArgs");
		NativeFieldInfoPtr_defaultAddAfterEventArgs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "defaultAddAfterEventArgs");
		NativeFieldInfoPtr_defaultDeleteEventArgs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "defaultDeleteEventArgs");
		NativeFieldInfoPtr_CachedControlPointsNameCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "CachedControlPointsNameCount");
		NativeFieldInfoPtr_MinimalMaxPointsPerUnit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "MinimalMaxPointsPerUnit");
		NativeFieldInfoPtr_MaxSegmentCacheSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "MaxSegmentCacheSize");
		NativeFieldInfoPtr_InvalidCPErrorMessage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "InvalidCPErrorMessage");
		NativeFieldInfoPtr_MinBSplineDegree = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, "MinBSplineDegree");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664632);
		NativeMethodInfoPtr_get_Interpolation_Public_get_CurvyInterpolation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664633);
		NativeMethodInfoPtr_set_Interpolation_Public_set_Void_CurvyInterpolation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664634);
		NativeMethodInfoPtr_get_RestrictTo2D_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664635);
		NativeMethodInfoPtr_set_RestrictTo2D_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664636);
		NativeMethodInfoPtr_get_Restricted2DPlane_Public_get_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664637);
		NativeMethodInfoPtr_set_Restricted2DPlane_Public_set_Void_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664638);
		NativeMethodInfoPtr_get_AutoHandleDistance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664639);
		NativeMethodInfoPtr_set_AutoHandleDistance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664640);
		NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664641);
		NativeMethodInfoPtr_set_Closed_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664642);
		NativeMethodInfoPtr_get_AutoEndTangents_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664643);
		NativeMethodInfoPtr_set_AutoEndTangents_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664644);
		NativeMethodInfoPtr_get_Orientation_Public_get_CurvyOrientation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664645);
		NativeMethodInfoPtr_set_Orientation_Public_set_Void_CurvyOrientation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664646);
		NativeMethodInfoPtr_get_UpdateIn_Public_get_CurvyUpdateMethod_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664647);
		NativeMethodInfoPtr_set_UpdateIn_Public_set_Void_CurvyUpdateMethod_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664648);
		NativeMethodInfoPtr_get_GizmoColor_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664649);
		NativeMethodInfoPtr_set_GizmoColor_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664650);
		NativeMethodInfoPtr_get_GizmoSelectionColor_Public_get_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664651);
		NativeMethodInfoPtr_set_GizmoSelectionColor_Public_set_Void_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664652);
		NativeMethodInfoPtr_get_CacheDensity_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664653);
		NativeMethodInfoPtr_set_CacheDensity_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664654);
		NativeMethodInfoPtr_get_MaxPointsPerUnit_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664655);
		NativeMethodInfoPtr_set_MaxPointsPerUnit_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664656);
		NativeMethodInfoPtr_get_UsePooling_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664657);
		NativeMethodInfoPtr_set_UsePooling_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664658);
		NativeMethodInfoPtr_get_UseThreading_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664659);
		NativeMethodInfoPtr_set_UseThreading_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664660);
		NativeMethodInfoPtr_get_CheckTransform_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664661);
		NativeMethodInfoPtr_set_CheckTransform_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664662);
		NativeMethodInfoPtr_get_Tension_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664663);
		NativeMethodInfoPtr_set_Tension_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664664);
		NativeMethodInfoPtr_get_Continuity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664665);
		NativeMethodInfoPtr_set_Continuity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664666);
		NativeMethodInfoPtr_get_Bias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664667);
		NativeMethodInfoPtr_set_Bias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664668);
		NativeMethodInfoPtr_get_BSplineDegree_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664669);
		NativeMethodInfoPtr_set_BSplineDegree_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664670);
		NativeMethodInfoPtr_get_IsBSplineClamped_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664671);
		NativeMethodInfoPtr_set_IsBSplineClamped_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664672);
		NativeMethodInfoPtr_get_IsInitialized_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664673);
		NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664674);
		NativeMethodInfoPtr_get_Count_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664675);
		NativeMethodInfoPtr_get_ControlPointCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664676);
		NativeMethodInfoPtr_get_CacheSize_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664677);
		NativeMethodInfoPtr_get_Length_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664678);
		NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664679);
		NativeMethodInfoPtr_get_Item_Public_get_CurvySplineSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664680);
		NativeMethodInfoPtr_get_ControlPointsList_Public_get_ReadOnlyCollection_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664681);
		NativeMethodInfoPtr_get_FirstVisibleControlPoint_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664682);
		NativeMethodInfoPtr_get_LastVisibleControlPoint_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664683);
		NativeMethodInfoPtr_get_FirstSegment_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664684);
		NativeMethodInfoPtr_get_LastSegment_Public_get_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664685);
		NativeMethodInfoPtr_get_GlobalCoordinatesChangedThisFrame_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664686);
		NativeMethodInfoPtr_get_OnGlobalCoordinatesChanged_Public_get_Action_1_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664687);
		NativeMethodInfoPtr_set_OnGlobalCoordinatesChanged_Public_set_Void_Action_1_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664688);
		NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664689);
		NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664690);
		NativeMethodInfoPtr_get_OnInitialized_Public_get_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664691);
		NativeMethodInfoPtr_set_OnInitialized_Public_set_Void_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664692);
		NativeMethodInfoPtr_get_OnAfterControlPointChanges_Public_get_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664693);
		NativeMethodInfoPtr_set_OnAfterControlPointChanges_Public_set_Void_CurvySplineEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664694);
		NativeMethodInfoPtr_get_OnBeforeControlPointAdd_Public_get_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664695);
		NativeMethodInfoPtr_set_OnBeforeControlPointAdd_Public_set_Void_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664696);
		NativeMethodInfoPtr_get_OnAfterControlPointAdd_Public_get_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664697);
		NativeMethodInfoPtr_set_OnAfterControlPointAdd_Public_set_Void_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664698);
		NativeMethodInfoPtr_get_OnBeforeControlPointDelete_Public_get_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664699);
		NativeMethodInfoPtr_set_OnBeforeControlPointDelete_Public_set_Void_CurvyControlPointEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664700);
		NativeMethodInfoPtr_Create_Public_Static_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664701);
		NativeMethodInfoPtr_Create_Public_Static_CurvySpline_CurvySpline_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664702);
		NativeMethodInfoPtr_CalculateCacheSize_Public_Static_Int32_Int32_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664703);
		NativeMethodInfoPtr_CalculateSamplingPointsPerUnit_Public_Static_Single_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664704);
		NativeMethodInfoPtr_Bezier_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664705);
		NativeMethodInfoPtr_BezierTangent_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664706);
		NativeMethodInfoPtr_CatmullRom_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664707);
		NativeMethodInfoPtr_TCB_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_Single_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664708);
		NativeMethodInfoPtr_GetFollowUpHeadingControlPoint_Public_Static_CurvySplineSegment_CurvySplineSegment_ConnectionHeadingEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664709);
		NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664710);
		NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664711);
		NativeMethodInfoPtr_InterpolateByDistance_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664712);
		NativeMethodInfoPtr_InterpolateByDistanceFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664713);
		NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664714);
		NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664715);
		NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664716);
		NativeMethodInfoPtr_GetTangentByDistance_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664717);
		NativeMethodInfoPtr_GetTangentByDistanceFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664718);
		NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664719);
		NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664720);
		NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664721);
		NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664722);
		NativeMethodInfoPtr_GetMetadata_Public_T_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664723);
		NativeMethodInfoPtr_GetInterpolatedMetadata_Public_U_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664724);
		NativeMethodInfoPtr_TFToDistance_Public_Single_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664725);
		NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664726);
		NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664727);
		NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664728);
		NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664729);
		NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664730);
		NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664731);
		NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664732);
		NativeMethodInfoPtr_DistanceToTF_Public_Single_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664733);
		NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664734);
		NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664735);
		NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664736);
		NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664737);
		NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664738);
		NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664739);
		NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664740);
		NativeMethodInfoPtr_Add_Public_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664741);
		NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664742);
		NativeMethodInfoPtr_Add_Public_CurvySplineSegment_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664743);
		NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664744);
		NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664745);
		NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664746);
		NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664747);
		NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664748);
		NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664749);
		NativeMethodInfoPtr_Clear_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664750);
		NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664751);
		NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664752);
		NativeMethodInfoPtr_GetPositionsCache_Public_SubArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664753);
		NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664754);
		NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Single_Single_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664755);
		NativeMethodInfoPtr_GetTangentsCache_Public_SubArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664756);
		NativeMethodInfoPtr_GetApproximationT_Public_Il2CppStructArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664757);
		NativeMethodInfoPtr_GetNormalsCache_Public_SubArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664758);
		NativeMethodInfoPtr_GetApproximationUpVectors_Public_Il2CppStructArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664759);
		NativeMethodInfoPtr_GetNearestPoint_Public_Vector3_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664760);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664761);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664762);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664763);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664764);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Int32_Int32_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664765);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Int32_Int32_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664766);
		NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_byref_CurvySplineSegment_byref_Single_Int32_Int32_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664767);
		NativeMethodInfoPtr_Refresh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664768);
		NativeMethodInfoPtr_SetDirtyAll_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664769);
		NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664770);
		NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664771);
		NativeMethodInfoPtr_SetDirtyPartial_Public_Void_CurvySplineSegment_SplineDirtyingType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664772);
		NativeMethodInfoPtr_ToWorldPosition_Public_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664773);
		NativeMethodInfoPtr_ToWorldDirection_Public_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664774);
		NativeMethodInfoPtr_ToLocalPosition_Public_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664775);
		NativeMethodInfoPtr_ToLocalDirection_Public_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664776);
		NativeMethodInfoPtr_ApplyControlPointsNames_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664777);
		NativeMethodInfoPtr_SyncSplineFromHierarchy_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664778);
		NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664779);
		NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Boolean_byref_Boolean_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664780);
		NativeMethodInfoPtr_IsPlanar_Public_Boolean_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664781);
		NativeMethodInfoPtr_MakePlanar_Public_Void_CurvyPlane_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664782);
		NativeMethodInfoPtr_MakePlanar_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664783);
		NativeMethodInfoPtr_Subdivide_Public_Void_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664784);
		NativeMethodInfoPtr_Simplify_Public_Void_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664785);
		NativeMethodInfoPtr_Equalize_Public_Void_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664786);
		NativeMethodInfoPtr_Normalize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664787);
		NativeMethodInfoPtr_SetPivot_Public_Vector3_Single_Single_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664788);
		NativeMethodInfoPtr_Flip_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664789);
		NativeMethodInfoPtr_MoveControlPoints_Public_Void_Int32_Int32_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664790);
		NativeMethodInfoPtr_JoinWith_Public_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664791);
		NativeMethodInfoPtr_Split_Public_CurvySpline_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664792);
		NativeMethodInfoPtr_SetFirstControlPoint_Public_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664793);
		NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Public_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664794);
		NativeMethodInfoPtr_CanControlPointHaveFollowUp_Public_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664795);
		NativeMethodInfoPtr_GetControlPointIndex_Public_Int16_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664796);
		NativeMethodInfoPtr_GetSegmentIndex_Public_Int16_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664797);
		NativeMethodInfoPtr_GetNextControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664798);
		NativeMethodInfoPtr_GetNextControlPointIndex_Public_Int16_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664799);
		NativeMethodInfoPtr_GetNextControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664800);
		NativeMethodInfoPtr_GetPreviousControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664801);
		NativeMethodInfoPtr_GetPreviousControlPointIndex_Public_Int16_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664802);
		NativeMethodInfoPtr_GetPreviousControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664803);
		NativeMethodInfoPtr_GetNextSegment_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664804);
		NativeMethodInfoPtr_GetPreviousSegment_Public_CurvySplineSegment_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664805);
		NativeMethodInfoPtr_IsControlPointASegment_Public_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664806);
		NativeMethodInfoPtr_IsControlPointVisible_Public_Boolean_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664807);
		NativeMethodInfoPtr_GetControlPointOrientationAnchorIndex_Public_Int16_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664808);
		NativeMethodInfoPtr_SetFromString_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664809);
		NativeMethodInfoPtr_get_TransformMonitor_Private_get_TransformMonitor_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664810);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664811);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664812);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664813);
		NativeMethodInfoPtr_Start_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664814);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664815);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664816);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664817);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664818);
		NativeMethodInfoPtr_FixedUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664819);
		NativeMethodInfoPtr_get_Segments_Private_get_List_1_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664820);
		NativeMethodInfoPtr_get_MaxBSplineDegree_Private_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664821);
		NativeMethodInfoPtr_Initialize_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664822);
		NativeMethodInfoPtr_HookEditorUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664823);
		NativeMethodInfoPtr_UnhookEditorUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664824);
		NativeMethodInfoPtr_DoUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664825);
		NativeMethodInfoPtr_ClearBounds_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664826);
		NativeMethodInfoPtr_CanHaveManualEndCp_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664827);
		NativeMethodInfoPtr_CanBeClamped_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664828);
		NativeMethodInfoPtr_ReverseControlPoints_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664829);
		NativeMethodInfoPtr_GetNextControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664830);
		NativeMethodInfoPtr_GetPreviousControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664831);
		NativeMethodInfoPtr_IsControlPointASegment_Private_Static_Boolean_Int32_Int32_Boolean_Boolean_Boolean_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664832);
		NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Private_Static_Boolean_Boolean_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664833);
		NativeMethodInfoPtr_AddControlPoint_Private_Void_CurvySplineSegment_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664834);
		NativeMethodInfoPtr_InsertControlPoint_Private_Void_Int32_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664835);
		NativeMethodInfoPtr_RemoveControlPoint_Private_Void_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664836);
		NativeMethodInfoPtr_ClearControlPoints_Private_Void_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664837);
		NativeMethodInfoPtr_InvalidateControlPointsRelationshipCacheINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664838);
		NativeMethodInfoPtr_UpdateControlPointDistances_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664839);
		NativeMethodInfoPtr_EnforceTangentContinuity_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664840);
		NativeMethodInfoPtr_PrepareThreadCompatibleData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664841);
		NativeMethodInfoPtr_GetOrientationAnchorIndices_Private_Il2CppStructArray_1_Int16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664842);
		NativeMethodInfoPtr_InvalidateAccumulators_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664843);
		NativeMethodInfoPtr_NotifyMetaDataModification_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664844);
		NativeMethodInfoPtr_DisposeOfControlPoint_Private_Void_CurvySplineSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664845);
		NativeMethodInfoPtr_ShouldUseControlPointPooling_Private_Boolean_byref_CurvyGlobalManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664846);
		NativeMethodInfoPtr_InsertAt_Private_CurvySplineSegment_CurvySplineSegment_Vector3_Int32_ModeEnum_Boolean_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664847);
		NativeMethodInfoPtr_AcquireNewControlPoint_Private_CurvySplineSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664848);
		NativeMethodInfoPtr_GetSegmentApproximationsInSpace_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_Space_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664849);
		NativeMethodInfoPtr_ConcatenateSegmentApproximations_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664850);
		NativeMethodInfoPtr_TransformToWorldSpace_Private_Void_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664851);
		NativeMethodInfoPtr_PushChildCPsToPool_Private_Void_ComponentPool_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664852);
		NativeMethodInfoPtr_OnRefreshEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664853);
		NativeMethodInfoPtr_OnBeforeControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664854);
		NativeMethodInfoPtr_OnAfterControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664855);
		NativeMethodInfoPtr_OnBeforeControlPointDeleteEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664856);
		NativeMethodInfoPtr_OnAfterControlPointChangesEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664857);
		NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr, 100664858);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38093, RefRangeEnd = 38094, XrefRangeStart = 37970, XrefRangeEnd = 38093, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySpline()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvySpline>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 38458, RefRangeEnd = 38467, XrefRangeStart = 38428, XrefRangeEnd = 38458, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvySpline Create()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Static_CurvySpline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38486, RefRangeEnd = 38487, XrefRangeStart = 38467, XrefRangeEnd = 38486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvySpline Create(CurvySpline takeOptionsFrom)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)takeOptionsFrom);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Static_CurvySpline_CurvySpline_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38502, RefRangeEnd = 38504, XrefRangeStart = 38487, XrefRangeEnd = 38502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int CalculateCacheSize(int density, float segmentLength, float maxPointsPerUnit)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&density);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &segmentLength;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxPointsPerUnit;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateCacheSize_Public_Static_Int32_Int32_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38511, RefRangeEnd = 38512, XrefRangeStart = 38504, XrefRangeEnd = 38511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float CalculateSamplingPointsPerUnit(int density, float maxPointsPerUnit)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&density);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxPointsPerUnit;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateSamplingPointsPerUnit_Public_Static_Single_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 Bezier(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&T0);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &P0;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &P1;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &T1;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &f;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bezier_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 BezierTangent(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&T0);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &P0;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &P1;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &T1;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &f;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BezierTangent_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 CatmullRom(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&T0);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &P0;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &P1;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &T1;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &f;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CatmullRom_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static Vector3 TCB(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f, float FT0, float FC0, float FB0, float FT1, float FC1, float FB1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[11];
		*ptr = (nint)(&T0);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &P0;
		*(Vector3**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &P1;
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &T1;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &f;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &FT0;
		*(float**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &FC0;
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &FB0;
		*(float**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &FT1;
		*(float**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &FC1;
		*(float**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = &FB1;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TCB_Public_Static_Vector3_Vector3_Vector3_Vector3_Vector3_Single_Single_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 38519, RefRangeEnd = 38523, XrefRangeStart = 38512, XrefRangeEnd = 38519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvySplineSegment GetFollowUpHeadingControlPoint(CurvySplineSegment followUp, ConnectionHeadingEnum headingDirection)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)followUp);
		*(ConnectionHeadingEnum**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &headingDirection;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFollowUpHeadingControlPoint_Public_Static_CurvySplineSegment_CurvySplineSegment_ConnectionHeadingEnum_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 38530, RefRangeEnd = 38536, XrefRangeStart = 38523, XrefRangeEnd = 38530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 Interpolate(float tf, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interpolate_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 38543, RefRangeEnd = 38547, XrefRangeStart = 38536, XrefRangeEnd = 38543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 InterpolateFast(float tf, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38547, XrefRangeEnd = 38555, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 InterpolateByDistance(float distance, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateByDistance_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38555, XrefRangeEnd = 38563, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 InterpolateByDistanceFast(float distance, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateByDistanceFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38570, RefRangeEnd = 38572, XrefRangeStart = 38563, XrefRangeEnd = 38570, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangent(float tf, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38572, XrefRangeEnd = 38579, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangent(float tf, Vector3 position, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&tf);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		*(Space**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangent_Public_Vector3_Single_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38586, RefRangeEnd = 38587, XrefRangeStart = 38579, XrefRangeEnd = 38586, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangentFast(float tf, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangentFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38587, XrefRangeEnd = 38595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangentByDistance(float distance, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangentByDistance_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38595, XrefRangeEnd = 38603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetTangentByDistanceFast(float distance, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangentByDistanceFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 38606, RefRangeEnd = 38609, XrefRangeStart = 38603, XrefRangeEnd = 38606, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InterpolateAndGetTangent(float tf, out Vector3 position, out Vector3 tangent, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref position);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateAndGetTangent_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38612, RefRangeEnd = 38614, XrefRangeStart = 38609, XrefRangeEnd = 38612, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InterpolateAndGetTangentFast(float tf, out Vector3 position, out Vector3 tangent, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref position);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateAndGetTangentFast_Public_Void_Single_byref_Vector3_byref_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 38621, RefRangeEnd = 38628, XrefRangeStart = 38614, XrefRangeEnd = 38621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetOrientationUpFast(float tf, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationUpFast_Public_Vector3_Single_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38628, XrefRangeEnd = 38635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Quaternion GetOrientationFast(float tf, bool inverse = false, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&tf);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &inverse;
		*(Space**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationFast_Public_Quaternion_Single_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Quaternion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38635, XrefRangeEnd = 38638, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetMetadata<T>(float tf) where T : CurvyMetadataBase
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetMetadata_Public_T_Single_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38638, XrefRangeEnd = 38648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe U GetInterpolatedMetadata<T, U>(float tf) where T : CurvyInterpolatableMetadataBase<U>
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetInterpolatedMetadata_Public_U_Single_0<T, U>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<U>(intPtr, false, true);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 38652, RefRangeEnd = 38657, XrefRangeStart = 38648, XrefRangeEnd = 38652, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float TFToDistance(float tf, CurvyClamping clamping = CurvyClamping.Clamp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToDistance_Public_Single_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38663, RefRangeEnd = 38664, XrefRangeStart = 38657, XrefRangeEnd = 38663, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment TFToSegment(float tf, out float localF, out bool isOnSegmentStart, out bool isOnSegmentEnd, CurvyClamping clamping)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref localF);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isOnSegmentStart);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isOnSegmentEnd);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 38668, RefRangeEnd = 38691, XrefRangeStart = 38664, XrefRangeEnd = 38668, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment TFToSegment(float tf, out float localF, CurvyClamping clamping)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref localF);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38691, XrefRangeEnd = 38692, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment TFToSegment(float tf, CurvyClamping clamping)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 38693, RefRangeEnd = 38696, XrefRangeStart = 38692, XrefRangeEnd = 38693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment TFToSegment(float tf)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&tf);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 38697, RefRangeEnd = 38703, XrefRangeStart = 38696, XrefRangeEnd = 38697, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment TFToSegment(float tf, out float localF)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&tf);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref localF);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TFToSegment_Public_CurvySplineSegment_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 38705, RefRangeEnd = 38708, XrefRangeStart = 38703, XrefRangeEnd = 38705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float SegmentToTF(CurvySplineSegment segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 38713, RefRangeEnd = 38720, XrefRangeStart = 38708, XrefRangeEnd = 38713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float SegmentToTF(CurvySplineSegment segment, float localF)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &localF;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentToTF_Public_Single_CurvySplineSegment_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 38727, RefRangeEnd = 38741, XrefRangeStart = 38720, XrefRangeEnd = 38727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float DistanceToTF(float distance, CurvyClamping clamping = CurvyClamping.Clamp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceToTF_Public_Single_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38742, RefRangeEnd = 38744, XrefRangeStart = 38741, XrefRangeEnd = 38742, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment DistanceToSegment(float distance, CurvyClamping clamping = CurvyClamping.Clamp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38744, XrefRangeEnd = 38745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment DistanceToSegment(float distance, out float localDistance, CurvyClamping clamping = CurvyClamping.Clamp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref localDistance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 38751, RefRangeEnd = 38755, XrefRangeStart = 38745, XrefRangeEnd = 38751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment DistanceToSegment(float distance, out float localDistance, out bool isOnSegmentStart, out bool isOnSegmentEnd, CurvyClamping clamping = CurvyClamping.Clamp)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref localDistance);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isOnSegmentStart);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isOnSegmentEnd);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DistanceToSegment_Public_CurvySplineSegment_Single_byref_Single_byref_Boolean_byref_Boolean_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38757, RefRangeEnd = 38759, XrefRangeStart = 38755, XrefRangeEnd = 38757, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ClampDistance(float distance, CurvyClamping clamping)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38759, XrefRangeEnd = 38761, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ClampDistance(float distance, CurvyClamping clamping, float min, float max)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&distance);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Single_Single_CurvyClamping_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38763, RefRangeEnd = 38764, XrefRangeStart = 38761, XrefRangeEnd = 38763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ClampDistance(float distance, ref int dir, CurvyClamping clamping)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38764, XrefRangeEnd = 38766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ClampDistance(float distance, ref int dir, CurvyClamping clamping, float min, float max)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&distance);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(CurvyClamping**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clamping;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampDistance_Public_Single_Single_byref_Int32_CurvyClamping_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38766, XrefRangeEnd = 38767, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment Add()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38767, XrefRangeEnd = 38771, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<CurvySplineSegment> Add(int controlPointsCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&controlPointsCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvySplineSegment>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38771, XrefRangeEnd = 38783, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment Add(Vector3 controlPointPosition, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&controlPointPosition);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_CurvySplineSegment_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38804, RefRangeEnd = 38806, XrefRangeStart = 38783, XrefRangeEnd = 38804, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<CurvySplineSegment> Add([Optional] Il2CppStructArray<Vector3> controlPointsLocalPositions)
	{
		if (controlPointsLocalPositions == null)
		{
			controlPointsLocalPositions = new Il2CppStructArray<Vector3>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPointsLocalPositions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvySplineSegment>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38827, RefRangeEnd = 38828, XrefRangeStart = 38806, XrefRangeEnd = 38827, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<CurvySplineSegment> Add(Il2CppStructArray<Vector3> controlPointsPositions, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPointsPositions);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Il2CppReferenceArray_1_CurvySplineSegment_Il2CppStructArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CurvySplineSegment>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38828, XrefRangeEnd = 38858, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment InsertBefore(CurvySplineSegment controlPoint, bool skipRefreshingAndEvents = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38858, XrefRangeEnd = 38862, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment InsertBefore(CurvySplineSegment controlPoint, Vector3 position, bool skipRefreshingAndEvents = false, Space space = Space.World)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertBefore_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38891, RefRangeEnd = 38893, XrefRangeStart = 38862, XrefRangeEnd = 38891, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment InsertAfter(CurvySplineSegment controlPoint, bool skipRefreshingAndEvents = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38893, XrefRangeEnd = 38898, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment InsertAfter(CurvySplineSegment controlPoint, Vector3 position, bool skipRefreshingAndEvents = false, Space space = Space.World)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertAfter_Public_CurvySplineSegment_CurvySplineSegment_Vector3_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38917, RefRangeEnd = 38918, XrefRangeStart = 38898, XrefRangeEnd = 38917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear(bool isUndoable = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&isUndoable);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38918, XrefRangeEnd = 38919, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Delete(CurvySplineSegment controlPoint, bool skipRefreshingAndEvents = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 38942, RefRangeEnd = 38944, XrefRangeStart = 38919, XrefRangeEnd = 38942, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Delete(CurvySplineSegment controlPoint, bool skipRefreshingAndEvents, bool isUndoableDeletion)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isUndoableDeletion;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Delete_Public_Void_CurvySplineSegment_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 38960, RefRangeEnd = 38965, XrefRangeStart = 38944, XrefRangeEnd = 38960, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> GetPositionsCache(Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPositionsCache_Public_SubArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 38976, RefRangeEnd = 38977, XrefRangeStart = 38965, XrefRangeEnd = 38976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Vector3> GetApproximation(Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 38977, XrefRangeEnd = 39017, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Vector3> GetApproximation(float fromTF, float toTF, bool includeEndPoint = true, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&fromTF);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &toTF;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeEndPoint;
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetApproximation_Public_Il2CppStructArray_1_Vector3_Single_Single_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39017, XrefRangeEnd = 39033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> GetTangentsCache(Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTangentsCache_Public_SubArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39033, XrefRangeEnd = 39059, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Vector3> GetApproximationT(Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetApproximationT_Public_Il2CppStructArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39059, XrefRangeEnd = 39075, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> GetNormalsCache(Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNormalsCache_Public_SubArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39075, XrefRangeEnd = 39101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<Vector3> GetApproximationUpVectors(Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&space);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetApproximationUpVectors_Public_Il2CppStructArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39101, XrefRangeEnd = 39102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 GetNearestPoint(Vector3 position, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&position);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPoint_Public_Vector3_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39102, XrefRangeEnd = 39103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 localPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39103, XrefRangeEnd = 39104, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 position, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&position);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39104, XrefRangeEnd = 39105, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 localPosition, out Vector3 nearestPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&localPosition);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nearestPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39105, XrefRangeEnd = 39106, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 position, out Vector3 nearestPoint, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&position);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nearestPoint);
		*(Space**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39106, XrefRangeEnd = 39107, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 position, int searchStartSegmentIndex = 0, int searchEndSegmentIndex = -1, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&position);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchStartSegmentIndex;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchEndSegmentIndex;
		*(Space**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_Int32_Int32_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39107, XrefRangeEnd = 39108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 position, out Vector3 nearestPoint, int searchStartSegmentIndex = 0, int searchEndSegmentIndex = -1, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&position);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nearestPoint);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchStartSegmentIndex;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchEndSegmentIndex;
		*(Space**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_Int32_Int32_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 39131, RefRangeEnd = 39139, XrefRangeStart = 39108, XrefRangeEnd = 39131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetNearestPointTF(Vector3 position, out Vector3 nearestPoint, out CurvySplineSegment nearestSegment, out float nearestPointLocalF, int searchStartSegmentIndex = 0, int searchEndSegmentIndex = -1, Space space = Space.Self)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&position);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nearestPoint);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nearestPointLocalF);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchStartSegmentIndex;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &searchEndSegmentIndex;
		*(Space**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNearestPointTF_Public_Single_Vector3_byref_Vector3_byref_CurvySplineSegment_byref_Single_Int32_Int32_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		nearestSegment = ((num3 == 0) ? null : new CurvySplineSegment(num3));
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 39143, RefRangeEnd = 39152, XrefRangeStart = 39139, XrefRangeEnd = 39143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39154, RefRangeEnd = 39155, XrefRangeStart = 39152, XrefRangeEnd = 39154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDirtyAll()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirtyAll_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39157, RefRangeEnd = 39158, XrefRangeStart = 39155, XrefRangeEnd = 39157, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDirtyAll(SplineDirtyingType dirtyingType, bool dirtyConnectedControlPoints)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&dirtyingType);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyConnectedControlPoints;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirtyAll_Public_Void_SplineDirtyingType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(33)]
	[CachedScanResults(RefRangeStart = 39171, RefRangeEnd = 39204, XrefRangeStart = 39158, XrefRangeEnd = 39171, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDirty(CurvySplineSegment dirtyControlPoint, SplineDirtyingType dirtyingType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyControlPoint);
		*(SplineDirtyingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyingType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirty_Public_Void_CurvySplineSegment_SplineDirtyingType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 39217, RefRangeEnd = 39222, XrefRangeStart = 39204, XrefRangeEnd = 39217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDirtyPartial(CurvySplineSegment dirtyControlPoint, SplineDirtyingType dirtyingType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dirtyControlPoint);
		*(SplineDirtyingType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dirtyingType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDirtyPartial_Public_Void_CurvySplineSegment_SplineDirtyingType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 39223, RefRangeEnd = 39225, XrefRangeStart = 39222, XrefRangeEnd = 39223, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ToWorldPosition(Vector3 localPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToWorldPosition_Public_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 39226, RefRangeEnd = 39231, XrefRangeStart = 39225, XrefRangeEnd = 39226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ToWorldDirection(Vector3 localDirection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localDirection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToWorldDirection_Public_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39232, RefRangeEnd = 39233, XrefRangeStart = 39231, XrefRangeEnd = 39232, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ToLocalPosition(Vector3 worldPosition)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&worldPosition);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToLocalPosition_Public_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39233, XrefRangeEnd = 39234, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 ToLocalDirection(Vector3 localDirection)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&localDirection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToLocalDirection_Public_Vector3_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyControlPointsNames()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyControlPointsNames_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 39236, RefRangeEnd = 39238, XrefRangeStart = 39234, XrefRangeEnd = 39236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SyncSplineFromHierarchy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SyncSplineFromHierarchy_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39238, XrefRangeEnd = 39262, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPlanar(out int ignoreAxis)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref ignoreAxis);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39262, XrefRangeEnd = 39285, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPlanar(out bool isYZ, out bool isXZ, out bool isXY)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)Unsafe.AsPointer(ref isYZ);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isXZ);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isXY);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPlanar_Public_Boolean_byref_Boolean_byref_Boolean_byref_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39285, XrefRangeEnd = 39307, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPlanar(CurvyPlane plane)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&plane);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPlanar_Public_Boolean_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39307, XrefRangeEnd = 39363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakePlanar(CurvyPlane plane)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&plane);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakePlanar_Public_Void_CurvyPlane_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39363, XrefRangeEnd = 39385, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakePlanar(int axis)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&axis);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakePlanar_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39385, XrefRangeEnd = 39473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Subdivide(CurvySplineSegment fromCP = null, CurvySplineSegment toCP = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fromCP);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toCP);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Subdivide_Public_Void_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39473, XrefRangeEnd = 39522, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Simplify(CurvySplineSegment fromCP = null, CurvySplineSegment toCP = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fromCP);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toCP);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Simplify_Public_Void_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39522, XrefRangeEnd = 39582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Equalize(CurvySplineSegment fromCP = null, CurvySplineSegment toCP = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fromCP);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)toCP);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equalize_Public_Void_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39582, XrefRangeEnd = 39604, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Normalize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Normalize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39604, XrefRangeEnd = 39621, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 SetPivot(float xRel = 0f, float yRel = 0f, float zRel = 0f, bool preview = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&xRel);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &yRel;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &zRel;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &preview;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPivot_Public_Vector3_Single_Single_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39621, XrefRangeEnd = 39658, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Flip()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Flip_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39690, RefRangeEnd = 39691, XrefRangeStart = 39658, XrefRangeEnd = 39690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MoveControlPoints(int startIndex, int count, CurvySplineSegment destCP)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&startIndex);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &count;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destCP);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveControlPoints_Public_Void_Int32_Int32_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39691, XrefRangeEnd = 39699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void JoinWith(CurvySplineSegment destCP)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destCP);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_JoinWith_Public_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39699, XrefRangeEnd = 39748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySpline Split(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Split_Public_CurvySpline_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySpline>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39748, XrefRangeEnd = 39766, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFirstControlPoint(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFirstControlPoint_Public_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39770, RefRangeEnd = 39771, XrefRangeStart = 39766, XrefRangeEnd = 39770, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsControlPointAnOrientationAnchor(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Public_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39773, RefRangeEnd = 39774, XrefRangeStart = 39771, XrefRangeEnd = 39773, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CanControlPointHaveFollowUp(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanControlPointHaveFollowUp_Public_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 39776, RefRangeEnd = 39784, XrefRangeStart = 39774, XrefRangeEnd = 39776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe short GetControlPointIndex(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointIndex_Public_Int16_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39784, XrefRangeEnd = 39786, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe short GetSegmentIndex(CurvySplineSegment segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSegmentIndex_Public_Int16_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 39789, RefRangeEnd = 39803, XrefRangeStart = 39786, XrefRangeEnd = 39789, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetNextControlPoint(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39805, RefRangeEnd = 39806, XrefRangeStart = 39803, XrefRangeEnd = 39805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe short GetNextControlPointIndex(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextControlPointIndex_Public_Int16_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39816, RefRangeEnd = 39817, XrefRangeStart = 39806, XrefRangeEnd = 39816, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetNextControlPointUsingFollowUp(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 39820, RefRangeEnd = 39826, XrefRangeStart = 39817, XrefRangeEnd = 39820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetPreviousControlPoint(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreviousControlPoint_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 39828, RefRangeEnd = 39830, XrefRangeStart = 39826, XrefRangeEnd = 39828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe short GetPreviousControlPointIndex(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreviousControlPointIndex_Public_Int16_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39840, RefRangeEnd = 39841, XrefRangeStart = 39830, XrefRangeEnd = 39840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetPreviousControlPointUsingFollowUp(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreviousControlPointUsingFollowUp_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 39846, RefRangeEnd = 39849, XrefRangeStart = 39841, XrefRangeEnd = 39846, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetNextSegment(CurvySplineSegment segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextSegment_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 39854, RefRangeEnd = 39857, XrefRangeStart = 39849, XrefRangeEnd = 39854, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment GetPreviousSegment(CurvySplineSegment segment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreviousSegment_Public_CurvySplineSegment_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 39861, RefRangeEnd = 39865, XrefRangeStart = 39857, XrefRangeEnd = 39861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsControlPointASegment(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsControlPointASegment_Public_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 39867, RefRangeEnd = 39868, XrefRangeStart = 39865, XrefRangeEnd = 39867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsControlPointVisible(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsControlPointVisible_Public_Boolean_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39868, XrefRangeEnd = 39871, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe short GetControlPointOrientationAnchorIndex(CurvySplineSegment controlPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetControlPointOrientationAnchorIndex_Public_Int16_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39871, XrefRangeEnd = 39915, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFromString(string fieldAndValue)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldAndValue);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFromString_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39925, XrefRangeEnd = 39930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39930, XrefRangeEnd = 39938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39938, XrefRangeEnd = 39944, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39944, XrefRangeEnd = 39948, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39948, XrefRangeEnd = 39950, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39950, XrefRangeEnd = 39967, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39967, XrefRangeEnd = 39972, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39972, XrefRangeEnd = 39977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 39977, XrefRangeEnd = 39982, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FixedUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 39997, RefRangeEnd = 40000, XrefRangeStart = 39984, XrefRangeEnd = 39997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HookEditorUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HookEditorUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnhookEditorUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnhookEditorUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 40031, RefRangeEnd = 40034, XrefRangeStart = 40000, XrefRangeEnd = 40031, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40034, XrefRangeEnd = 40039, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearBounds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearBounds_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool CanHaveManualEndCp()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanHaveManualEndCp_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool CanBeClamped()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanBeClamped_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40039, XrefRangeEnd = 40045, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReverseControlPoints()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReverseControlPoints_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static short GetNextControlPointIndex(short controlPointIndex, bool isSplineClosed, int controlPointsCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&controlPointIndex);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isSplineClosed;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointsCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNextControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static short GetPreviousControlPointIndex(short controlPointIndex, bool isSplineClosed, int controlPointsCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&controlPointIndex);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isSplineClosed;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointsCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPreviousControlPointIndex_Private_Static_Int16_Int16_Boolean_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(short*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool IsControlPointASegment(int controlPointIndex, int controlPointCount, bool isClosed, bool notAutoEndTangentsAndIsCatmullRomOrTCB, bool isBSpline, int bSplineDegree)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&controlPointIndex);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &controlPointCount;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isClosed;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &notAutoEndTangentsAndIsCatmullRomOrTCB;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &isBSpline;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &bSplineDegree;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsControlPointASegment_Private_Static_Boolean_Int32_Int32_Boolean_Boolean_Boolean_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool IsControlPointAnOrientationAnchor(bool isVisible, bool isSerializedOrientationAnchor, bool isFirstVisibleControlPoint, bool isLastVisibleControlPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&isVisible);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isSerializedOrientationAnchor;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isFirstVisibleControlPoint;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &isLastVisibleControlPoint;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsControlPointAnOrientationAnchor_Private_Static_Boolean_Boolean_Boolean_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 40064, RefRangeEnd = 40068, XrefRangeStart = 40045, XrefRangeEnd = 40064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddControlPoint(CurvySplineSegment item, bool invalidateAndDirty, bool requestSplineToHierarchySynchronization)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &invalidateAndDirty;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &requestSplineToHierarchySynchronization;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddControlPoint_Private_Void_CurvySplineSegment_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 40086, RefRangeEnd = 40088, XrefRangeStart = 40068, XrefRangeEnd = 40086, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InsertControlPoint(int index, CurvySplineSegment item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&index);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertControlPoint_Private_Void_Int32_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 40113, RefRangeEnd = 40118, XrefRangeStart = 40088, XrefRangeEnd = 40113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveControlPoint(CurvySplineSegment item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveControlPoint_Private_Void_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 40135, RefRangeEnd = 40139, XrefRangeStart = 40118, XrefRangeEnd = 40135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearControlPoints(bool invalidateAndDirty, bool requestSplineToHierarchySynchronization)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&invalidateAndDirty);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &requestSplineToHierarchySynchronization;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearControlPoints_Private_Void_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40139, XrefRangeEnd = 40141, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InvalidateControlPointsRelationshipCacheINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InvalidateControlPointsRelationshipCacheINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40141, XrefRangeEnd = 40154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateControlPointDistances()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateControlPointDistances_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40173, RefRangeEnd = 40174, XrefRangeStart = 40154, XrefRangeEnd = 40173, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnforceTangentContinuity()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnforceTangentContinuity_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40174, XrefRangeEnd = 40221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PrepareThreadCompatibleData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PrepareThreadCompatibleData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 40233, RefRangeEnd = 40235, XrefRangeStart = 40221, XrefRangeEnd = 40233, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppStructArray<short> GetOrientationAnchorIndices()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrientationAnchorIndices_Private_Il2CppStructArray_1_Int16_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<short>>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe void InvalidateAccumulators()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InvalidateAccumulators_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40235, RefRangeEnd = 40236, XrefRangeStart = 40235, XrefRangeEnd = 40235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void NotifyMetaDataModification()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NotifyMetaDataModification_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40236, XrefRangeEnd = 40239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DisposeOfControlPoint(CurvySplineSegment controlPoint, bool isUndoable)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPoint);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isUndoable;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisposeOfControlPoint_Private_Void_CurvySplineSegment_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 40254, RefRangeEnd = 40260, XrefRangeStart = 40239, XrefRangeEnd = 40254, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool ShouldUseControlPointPooling(out CurvyGlobalManager curvyGlobalManager)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		nint num = 0;
		*ptr = (nint)(&num);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShouldUseControlPointPooling_Private_Boolean_byref_CurvyGlobalManager_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num2 = num;
		curvyGlobalManager = ((num2 == 0) ? null : new CurvyGlobalManager(num2));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 40300, RefRangeEnd = 40309, XrefRangeStart = 40260, XrefRangeEnd = 40300, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment InsertAt(CurvySplineSegment beforeEventCP, Vector3 position, int insertionIndex, CurvyControlPointEventArgs.ModeEnum insertionMode, bool skipRefreshingAndEvents, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)beforeEventCP);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &insertionIndex;
		*(CurvyControlPointEventArgs.ModeEnum**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &insertionMode;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &skipRefreshingAndEvents;
		*(Space**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InsertAt_Private_CurvySplineSegment_CurvySplineSegment_Vector3_Int32_ModeEnum_Boolean_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40317, RefRangeEnd = 40318, XrefRangeStart = 40309, XrefRangeEnd = 40317, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineSegment AcquireNewControlPoint()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AcquireNewControlPoint_Private_CurvySplineSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineSegment>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 40322, RefRangeEnd = 40327, XrefRangeStart = 40318, XrefRangeEnd = 40322, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> GetSegmentApproximationsInSpace(Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>> approximationGetter, Space space)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximationGetter);
		*(Space**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &space;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSegmentApproximationsInSpace_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_Space_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40354, RefRangeEnd = 40355, XrefRangeStart = 40327, XrefRangeEnd = 40354, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SubArray<Vector3> ConcatenateSegmentApproximations(Il2CppSystem.Func<CurvySplineSegment, SubArray<Vector3>> approximationGetter)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)approximationGetter);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConcatenateSegmentApproximations_Private_SubArray_1_Vector3_Func_2_CurvySplineSegment_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SubArray<Vector3>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40355, XrefRangeEnd = 40358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TransformToWorldSpace(SubArray<Vector3> localSpaceVectors)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)localSpaceVectors));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransformToWorldSpace_Private_Void_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40358, XrefRangeEnd = 40371, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PushChildCPsToPool(ComponentPool controlPointPool)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)controlPointPool);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PushChildCPsToPool_Private_Void_ComponentPool_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40371, XrefRangeEnd = 40374, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineEventArgs OnRefreshEvent(CurvySplineEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnRefreshEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEventArgs>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40377, RefRangeEnd = 40378, XrefRangeStart = 40374, XrefRangeEnd = 40377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyControlPointEventArgs OnBeforeControlPointAddEvent(CurvyControlPointEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBeforeControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEventArgs>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40378, XrefRangeEnd = 40381, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyControlPointEventArgs OnAfterControlPointAddEvent(CurvyControlPointEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnAfterControlPointAddEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEventArgs>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 40384, RefRangeEnd = 40385, XrefRangeStart = 40381, XrefRangeEnd = 40384, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyControlPointEventArgs OnBeforeControlPointDeleteEvent(CurvyControlPointEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBeforeControlPointDeleteEvent_Private_CurvyControlPointEventArgs_CurvyControlPointEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyControlPointEventArgs>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40385, XrefRangeEnd = 40388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvySplineEventArgs OnAfterControlPointChangesEvent(CurvySplineEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnAfterControlPointChangesEvent_Private_CurvySplineEventArgs_CurvySplineEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvySplineEventArgs>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 40388, XrefRangeEnd = 40397, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetOnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Il2CppReferenceArray<CurvySplineSegment> Add(params Vector3[] controlPointsLocalPositions)
	{
		return Add(new Il2CppStructArray<Vector3>(controlPointsLocalPositions));
	}

	public CurvySpline(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
