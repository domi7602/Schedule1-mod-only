using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.DevTools;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class DeformMesh : ScalingModule
{
	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.Generator.Modules.DeformMesh+<>c")]
	public new sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__12_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__12_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__17_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__17_1;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Refresh_b__12_1_Internal_Void_CGSpots_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Refresh_b__12_0_Internal_Void_CGVMesh_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ToOneDimensionalArray_b__17_0_Internal_Boolean_CGSpots_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ToOneDimensionalArray_b__17_1_Internal_Int32_CGSpots_0;

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

		public unsafe static Il2CppSystem.Action<CGSpots> __9__12_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__12_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CGSpots>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__12_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe static Il2CppSystem.Action<CGVMesh> __9__12_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__12_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CGVMesh>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__12_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe static Il2CppSystem.Func<CGSpots, bool> __9__17_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__17_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGSpots, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__17_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGSpots, int> __9__17_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__17_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGSpots, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__17_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__12_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__12_1");
			NativeFieldInfoPtr___9__12_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__12_0");
			NativeFieldInfoPtr___9__17_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__17_0");
			NativeFieldInfoPtr___9__17_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__17_1");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666923);
			NativeMethodInfoPtr__Refresh_b__12_1_Internal_Void_CGSpots_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666924);
			NativeMethodInfoPtr__Refresh_b__12_0_Internal_Void_CGVMesh_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666925);
			NativeMethodInfoPtr__ToOneDimensionalArray_b__17_0_Internal_Boolean_CGSpots_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666926);
			NativeMethodInfoPtr__ToOneDimensionalArray_b__17_1_Internal_Int32_CGSpots_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666927);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _Refresh_b__12_1(CGSpots s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Refresh_b__12_1_Internal_Void_CGSpots_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _Refresh_b__12_0(CGVMesh m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Refresh_b__12_0_Internal_Void_CGVMesh_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe bool _ToOneDimensionalArray_b__17_0(CGSpots s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ToOneDimensionalArray_b__17_0_Internal_Boolean_CGSpots_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _ToOneDimensionalArray_b__17_1(CGSpots s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ToOneDimensionalArray_b__17_1_Internal_Int32_CGSpots_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.Modules.DeformMesh+<>c__DisplayClass14_0")]
	public sealed class __c__DisplayClass14_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_path;

		private static readonly System.IntPtr NativeFieldInfoPtr_inputMeshes;

		private static readonly System.IntPtr NativeFieldInfoPtr_outputMeshes;

		private static readonly System.IntPtr NativeFieldInfoPtr_outputSpots;

		private static readonly System.IntPtr NativeFieldInfoPtr_stretchToEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_smallestVertexDistance;

		private static readonly System.IntPtr NativeFieldInfoPtr_stretchingAdditionalDistanceRatio;

		private static readonly System.IntPtr NativeFieldInfoPtr_scaleParameters;

		private static readonly System.IntPtr NativeFieldInfoPtr_isCurveEvaluationNeeded;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__DeformMeshes_b__0_Internal_Void_CGSpot_Int32_Int32_0;

		public unsafe CGPath path
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGPath>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_path)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGPath));
			}
		}

		public unsafe List<CGVMesh> inputMeshes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inputMeshes);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGVMesh>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_inputMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe Il2CppReferenceArray<CGVMesh> outputMeshes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outputMeshes);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CGVMesh>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outputMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe SubArray<CGSpot> outputSpots
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outputSpots);
				return new SubArray<CGSpot>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<CGSpot>>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_outputSpots), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<CGSpot>>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe bool stretchToEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchToEnd);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchToEnd)) = flag;
			}
		}

		public unsafe float smallestVertexDistance
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_smallestVertexDistance);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_smallestVertexDistance)) = num;
			}
		}

		public unsafe float stretchingAdditionalDistanceRatio
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchingAdditionalDistanceRatio);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchingAdditionalDistanceRatio)) = num;
			}
		}

		public unsafe ScaleParameters scaleParameters
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleParameters);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ScaleParameters>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleParameters)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleParameters));
			}
		}

		public unsafe bool isCurveEvaluationNeeded
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isCurveEvaluationNeeded);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isCurveEvaluationNeeded)) = flag;
			}
		}

		static __c__DisplayClass14_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "<>c__DisplayClass14_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr);
			NativeFieldInfoPtr_path = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "path");
			NativeFieldInfoPtr_inputMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "inputMeshes");
			NativeFieldInfoPtr_outputMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "outputMeshes");
			NativeFieldInfoPtr_outputSpots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "outputSpots");
			NativeFieldInfoPtr_stretchToEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "stretchToEnd");
			NativeFieldInfoPtr_smallestVertexDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "smallestVertexDistance");
			NativeFieldInfoPtr_stretchingAdditionalDistanceRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "stretchingAdditionalDistanceRatio");
			NativeFieldInfoPtr_scaleParameters = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "scaleParameters");
			NativeFieldInfoPtr_isCurveEvaluationNeeded = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, "isCurveEvaluationNeeded");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, 100666928);
			NativeMethodInfoPtr__DeformMeshes_b__0_Internal_Void_CGSpot_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr, 100666929);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass14_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass14_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57507, XrefRangeEnd = 57544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _DeformMeshes_b__0(CGSpot spot, int spotIndex, int elementsCount)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&spot);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &spotIndex;
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &elementsCount;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__DeformMeshes_b__0_Internal_Void_CGSpot_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c__DisplayClass14_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_InVMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_InPath;

	private static readonly System.IntPtr NativeFieldInfoPtr_InSpots;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutVMeshes;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutSpots;

	private static readonly System.IntPtr NativeFieldInfoPtr_stretchToEnd;

	private static readonly System.IntPtr NativeFieldInfoPtr_threadWorker;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StretchToEnd_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StretchToEnd_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_ScaleParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsCurveEvaluationNeeded_Private_Static_Boolean_ScaleParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSpotDistance_Private_Static_Single_CGPath_Vector3_Il2CppStructArray_1_Vector3_Int32_Il2CppStructArray_1_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToOneDimensionalArray_Private_Static_Nullable_1_SubArray_1_CGSpot_List_1_CGSpots_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CGModuleInputSlot InVMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InVMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InVMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleInputSlot InPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InPath);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleInputSlot InSpots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InSpots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InSpots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleOutputSlot OutVMeshes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutVMeshes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutVMeshes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleOutputSlot));
		}
	}

	public unsafe CGModuleOutputSlot OutSpots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutSpots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutSpots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleOutputSlot));
		}
	}

	public unsafe bool stretchToEnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchToEnd);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stretchToEnd)) = flag;
		}
	}

	public unsafe ThreadPoolWorker<CGSpot> threadWorker
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadWorker);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ThreadPoolWorker<CGSpot>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_threadWorker)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)threadPoolWorker));
		}
	}

	public unsafe bool StretchToEnd
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StretchToEnd_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StretchToEnd_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static DeformMesh()
	{
		Il2CppClassPointerStore<DeformMesh>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "DeformMesh");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr);
		NativeFieldInfoPtr_InVMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "InVMeshes");
		NativeFieldInfoPtr_InPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "InPath");
		NativeFieldInfoPtr_InSpots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "InSpots");
		NativeFieldInfoPtr_OutVMeshes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "OutVMeshes");
		NativeFieldInfoPtr_OutSpots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "OutSpots");
		NativeFieldInfoPtr_stretchToEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "stretchToEnd");
		NativeFieldInfoPtr_threadWorker = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, "threadWorker");
		NativeMethodInfoPtr_get_StretchToEnd_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666911);
		NativeMethodInfoPtr_set_StretchToEnd_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666912);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666913);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666914);
		NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666915);
		NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666916);
		NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_ScaleParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666917);
		NativeMethodInfoPtr_IsCurveEvaluationNeeded_Private_Static_Boolean_ScaleParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666918);
		NativeMethodInfoPtr_GetSpotDistance_Private_Static_Single_CGPath_Vector3_Il2CppStructArray_1_Vector3_Int32_Il2CppStructArray_1_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666919);
		NativeMethodInfoPtr_ToOneDimensionalArray_Private_Static_Nullable_1_SubArray_1_CGSpot_List_1_CGSpots_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666920);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr, 100666921);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57544, XrefRangeEnd = 57546, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57546, XrefRangeEnd = 57551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57551, XrefRangeEnd = 57675, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Refresh_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57675, XrefRangeEnd = 57684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DeformMeshes(List<CGVMesh> inputMeshes, SubArray<CGSpot> inputSpots, SubArray<CGSpot> outputSpots, Il2CppReferenceArray<CGVMesh> outputMeshes, CGPath path, bool stretchToEnd, ThreadPoolWorker<CGSpot> threadPoolWorker)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputMeshes);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)inputSpots));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)outputSpots));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputMeshes);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &stretchToEnd;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)threadPoolWorker);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 57794, RefRangeEnd = 57796, XrefRangeStart = 57684, XrefRangeEnd = 57794, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DeformMeshes(List<CGVMesh> inputMeshes, SubArray<CGSpot> inputSpots, SubArray<CGSpot> outputSpots, Il2CppReferenceArray<CGVMesh> outputMeshes, CGPath path, bool stretchToEnd, ThreadPoolWorker<CGSpot> threadPoolWorker, ScaleParameters scaleParameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputMeshes);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)inputSpots));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)outputSpots));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputMeshes);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &stretchToEnd;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)threadPoolWorker);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleParameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeformMeshes_Public_Static_Void_List_1_CGVMesh_SubArray_1_CGSpot_SubArray_1_CGSpot_Il2CppReferenceArray_1_CGVMesh_CGPath_Boolean_ThreadPoolWorker_1_CGSpot_ScaleParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe static bool IsCurveEvaluationNeeded(ScaleParameters scaleParameters)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scaleParameters);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsCurveEvaluationNeeded_Private_Static_Boolean_ScaleParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 57801, RefRangeEnd = 57804, XrefRangeStart = 57796, XrefRangeEnd = 57801, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetSpotDistance(CGPath path, Vector3 spotPosition, Il2CppStructArray<Vector3> pathPoints, int maxIndex, Il2CppStructArray<float> pathRelativeDistances, float pathLength)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &spotPosition;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathPoints);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathRelativeDistances);
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &pathLength;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSpotDistance_Private_Static_Single_CGPath_Vector3_Il2CppStructArray_1_Vector3_Int32_Il2CppStructArray_1_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 57885, RefRangeEnd = 57886, XrefRangeStart = 57804, XrefRangeEnd = 57885, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppSystem.Nullable<SubArray<CGSpot>> ToOneDimensionalArray(List<CGSpots> spotsList, out bool arrayIsCopy)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spotsList);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref arrayIsCopy);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToOneDimensionalArray_Private_Static_Nullable_1_SubArray_1_CGSpot_List_1_CGSpots_byref_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Il2CppSystem.Nullable<SubArray<CGSpot>>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 57886, XrefRangeEnd = 57916, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DeformMesh()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeformMesh>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeformMesh(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
