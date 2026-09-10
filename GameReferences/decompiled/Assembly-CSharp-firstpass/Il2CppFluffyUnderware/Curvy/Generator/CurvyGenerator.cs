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
using Il2CppSystem.Linq;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CurvyGenerator : DTVersionedMonoBehaviour
{
	public class ModuleSorter : Il2CppSystem.Object
	{
		[System.Serializable]
		[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+ModuleSorter+<>c")]
		public sealed class __c : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr___9;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__8_1;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__8_2;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__8_0;

			private static readonly System.IntPtr NativeFieldInfoPtr___9__8_3;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__Sort_b__8_1_Internal_Boolean_CGModuleInputSlot_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__Sort_b__8_2_Internal_Int32_CGModuleInputSlot_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__Sort_b__8_0_Internal_Int32_CGModule_CGModule_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__Sort_b__8_3_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0;

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

			public unsafe static Il2CppSystem.Func<CGModuleInputSlot, bool> __9__8_1
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__8_1, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, bool>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__8_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Func<CGModuleInputSlot, int> __9__8_2
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__8_2, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, int>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__8_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			public unsafe static Il2CppSystem.Comparison<CGModule> __9__8_0
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__8_0, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Comparison<CGModule>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__8_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)comparison));
				}
			}

			public unsafe static Il2CppSystem.Func<CGModuleOutputSlot, IEnumerable<CGModuleSlot>> __9__8_3
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__8_3, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleOutputSlot, IEnumerable<CGModuleSlot>>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__8_3, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
				}
			}

			static __c()
			{
				Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, "<>c");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
				NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
				NativeFieldInfoPtr___9__8_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__8_1");
				NativeFieldInfoPtr___9__8_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__8_2");
				NativeFieldInfoPtr___9__8_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__8_0");
				NativeFieldInfoPtr___9__8_3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__8_3");
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666401);
				NativeMethodInfoPtr__Sort_b__8_1_Internal_Boolean_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666402);
				NativeMethodInfoPtr__Sort_b__8_2_Internal_Int32_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666403);
				NativeMethodInfoPtr__Sort_b__8_0_Internal_Int32_CGModule_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666404);
				NativeMethodInfoPtr__Sort_b__8_3_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666405);
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
			public unsafe bool _Sort_b__8_1(CGModuleInputSlot t)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Sort_b__8_1_Internal_Boolean_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52930, XrefRangeEnd = 52932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe int _Sort_b__8_2(CGModuleInputSlot t)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Sort_b__8_2_Internal_Int32_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52932, XrefRangeEnd = 52935, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe int _Sort_b__8_0(CGModule a, CGModule b)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[2];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a);
				*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)b);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Sort_b__8_0_Internal_Int32_CGModule_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe IEnumerable<CGModuleSlot> _Sort_b__8_3(CGModuleOutputSlot outputSlot)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Sort_b__8_3_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CGModuleSlot>>(intPtr) : null;
			}

			public __c(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_modulesWithCircularReferences;

		private static readonly System.IntPtr NativeFieldInfoPtr_modulesAncestorCount;

		private static readonly System.IntPtr NativeFieldInfoPtr__SortingNeeded_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SortingNeeded_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_SortingNeeded_Public_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EnsureIsSorted_Public_Void_List_1_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Sort_Private_Void_List_1_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe HashSet<CGModule> modulesWithCircularReferences
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesWithCircularReferences);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<CGModule>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesWithCircularReferences)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
			}
		}

		public unsafe Dictionary<CGModule, int> modulesAncestorCount
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesAncestorCount);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CGModule, int>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesAncestorCount)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe bool _SortingNeeded_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SortingNeeded_k__BackingField);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SortingNeeded_k__BackingField)) = flag;
			}
		}

		public unsafe bool SortingNeeded
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 30481, RefRangeEnd = 30482, XrefRangeStart = 30481, XrefRangeEnd = 30482, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SortingNeeded_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(2)]
			[CachedScanResults(RefRangeStart = 30482, RefRangeEnd = 30484, XrefRangeStart = 30482, XrefRangeEnd = 30484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SortingNeeded_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static ModuleSorter()
		{
			Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "ModuleSorter");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr);
			NativeFieldInfoPtr_modulesWithCircularReferences = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, "modulesWithCircularReferences");
			NativeFieldInfoPtr_modulesAncestorCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, "modulesAncestorCount");
			NativeFieldInfoPtr__SortingNeeded_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, "<SortingNeeded>k__BackingField");
			NativeMethodInfoPtr_get_SortingNeeded_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666394);
			NativeMethodInfoPtr_set_SortingNeeded_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666395);
			NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666396);
			NativeMethodInfoPtr_EnsureIsSorted_Public_Void_List_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666397);
			NativeMethodInfoPtr_Sort_Private_Void_List_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666398);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr, 100666399);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52935, XrefRangeEnd = 52939, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool HasCircularReference(CGModule module)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 52939, XrefRangeEnd = 52940, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void EnsureIsSorted(List<CGModule> modules)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)modules);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureIsSorted_Public_Void_List_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 53127, RefRangeEnd = 53129, XrefRangeStart = 52940, XrefRangeEnd = 53127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Sort(List<CGModule> modules)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)modules);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Sort_Private_Void_List_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53129, XrefRangeEnd = 53144, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ModuleSorter()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ModuleSorter>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ModuleSorter(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class ModulesSynchronizer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_hasPendingRequest;

		private static readonly System.IntPtr NativeMethodInfoPtr_RequestSynchronization_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CancelRequests_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ProcessRequests_Public_Void_CurvyGenerator_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_AddMissingChildModules_Private_Static_Void_CurvyGenerator_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe bool hasPendingRequest
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPendingRequest);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasPendingRequest)) = flag;
			}
		}

		static ModulesSynchronizer()
		{
			Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "ModulesSynchronizer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr);
			NativeFieldInfoPtr_hasPendingRequest = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, "hasPendingRequest");
			NativeMethodInfoPtr_RequestSynchronization_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, 100666406);
			NativeMethodInfoPtr_CancelRequests_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, 100666407);
			NativeMethodInfoPtr_ProcessRequests_Public_Void_CurvyGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, 100666408);
			NativeMethodInfoPtr_AddMissingChildModules_Private_Static_Void_CurvyGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, 100666409);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr, 100666410);
		}

		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 53144, RefRangeEnd = 53148, XrefRangeStart = 53144, XrefRangeEnd = 53144, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void RequestSynchronization()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RequestSynchronization_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
		public unsafe void ProcessRequests(CurvyGenerator curvyGenerator)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyGenerator);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ProcessRequests_Public_Void_CurvyGenerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53148, XrefRangeEnd = 53167, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void AddMissingChildModules(CurvyGenerator curvyGenerator)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyGenerator);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddMissingChildModules_Private_Static_Void_CurvyGenerator_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ModulesSynchronizer()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ModulesSynchronizer>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ModulesSynchronizer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Timer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_lastTimestamp;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Now_Private_Static_get_Double_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ValidateTimes_Private_Void_Single_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Boolean_Single_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe double lastTimestamp
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastTimestamp);
				return *(double*)num;
			}
			set
			{
				*(double*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastTimestamp)) = num;
			}
		}

		public unsafe static double Now
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Now_Private_Static_get_Double_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(double*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Timer()
		{
			Il2CppClassPointerStore<Timer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "Timer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Timer>.NativeClassPtr);
			NativeFieldInfoPtr_lastTimestamp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Timer>.NativeClassPtr, "lastTimestamp");
			NativeMethodInfoPtr_get_Now_Private_Static_get_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Timer>.NativeClassPtr, 100666411);
			NativeMethodInfoPtr_ValidateTimes_Private_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Timer>.NativeClassPtr, 100666412);
			NativeMethodInfoPtr_Update_Public_Boolean_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Timer>.NativeClassPtr, 100666413);
			NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Timer>.NativeClassPtr, 100666414);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Timer>.NativeClassPtr, 100666415);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53167, XrefRangeEnd = 53168, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ValidateTimes(float timeLimit, float editorTimeLimit)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&timeLimit);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &editorTimeLimit;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValidateTimes_Private_Void_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53168, XrefRangeEnd = 53174, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool Update(float timeLimit, float editorTimeLimit)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&timeLimit);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &editorTimeLimit;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Boolean_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Timer()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Timer>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Timer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__7_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__33_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__33_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__53_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__53_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__59_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__81_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__82_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__84_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__84_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__85_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__88_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__88_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__90_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__get_m_LastModuleID_b__7_0_Internal_Int32_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__get_ModulesByID_b__33_0_Internal_Int32_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__get_ModulesByID_b__33_1_Internal_CGModule_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ReorderModules_b__53_0_Internal_Boolean_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ReorderModules_b__53_1_Internal_Single_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetModules_b__59_0_Internal_Boolean_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__CorrectDuplicateModuleIDs_b__81_0_Internal_Int32_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ResetAllModuleLinks_b__82_0_Internal_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_1_Internal_Single_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__85_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_0_Internal_Int32_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_1_Internal_Boolean_IGrouping_2_Int32_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SaveAllOutputManagedResources_b__90_0_Internal_Boolean_CGModule_0;

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

		public unsafe static Il2CppSystem.Func<CGModule, int> __9__7_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__7_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__7_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, int> __9__33_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__33_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__33_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, CGModule> __9__33_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__33_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, CGModule>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__33_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, bool> __9__53_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__53_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__53_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, float> __9__53_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__53_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, float>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__53_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, bool> __9__59_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__59_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__59_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, int> __9__81_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__81_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__81_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Action<CGModule> __9__82_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__82_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CGModule>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__82_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleInputSlot, IEnumerable<CGModule>> __9__84_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__84_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, IEnumerable<CGModule>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__84_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, float> __9__84_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__84_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, float>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__84_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleInputSlot, IEnumerable<CGModule>> __9__85_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__85_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, IEnumerable<CGModule>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__85_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, int> __9__88_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__88_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__88_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<IGrouping<int, CGModule>, bool> __9__88_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__88_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<IGrouping<int, CGModule>, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__88_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModule, bool> __9__90_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__90_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModule, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__90_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__7_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__7_0");
			NativeFieldInfoPtr___9__33_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__33_0");
			NativeFieldInfoPtr___9__33_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__33_1");
			NativeFieldInfoPtr___9__53_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__53_0");
			NativeFieldInfoPtr___9__53_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__53_1");
			NativeFieldInfoPtr___9__59_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__59_0");
			NativeFieldInfoPtr___9__81_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__81_0");
			NativeFieldInfoPtr___9__82_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__82_0");
			NativeFieldInfoPtr___9__84_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__84_0");
			NativeFieldInfoPtr___9__84_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__84_1");
			NativeFieldInfoPtr___9__85_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__85_0");
			NativeFieldInfoPtr___9__88_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__88_0");
			NativeFieldInfoPtr___9__88_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__88_1");
			NativeFieldInfoPtr___9__90_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__90_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666417);
			NativeMethodInfoPtr__get_m_LastModuleID_b__7_0_Internal_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666418);
			NativeMethodInfoPtr__get_ModulesByID_b__33_0_Internal_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666419);
			NativeMethodInfoPtr__get_ModulesByID_b__33_1_Internal_CGModule_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666420);
			NativeMethodInfoPtr__ReorderModules_b__53_0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666421);
			NativeMethodInfoPtr__ReorderModules_b__53_1_Internal_Single_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666422);
			NativeMethodInfoPtr__GetModules_b__59_0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666423);
			NativeMethodInfoPtr__CorrectDuplicateModuleIDs_b__81_0_Internal_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666424);
			NativeMethodInfoPtr__ResetAllModuleLinks_b__82_0_Internal_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666425);
			NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666426);
			NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_1_Internal_Single_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666427);
			NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__85_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666428);
			NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_0_Internal_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666429);
			NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_1_Internal_Boolean_IGrouping_2_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666430);
			NativeMethodInfoPtr__SaveAllOutputManagedResources_b__90_0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666431);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53174, XrefRangeEnd = 53176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _get_m_LastModuleID_b__7_0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_m_LastModuleID_b__7_0_Internal_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _get_ModulesByID_b__33_0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_ModulesByID_b__33_0_Internal_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe CGModule _get_ModulesByID_b__33_1(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_ModulesByID_b__33_1_Internal_CGModule_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53176, XrefRangeEnd = 53179, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _ReorderModules_b__53_0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ReorderModules_b__53_0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe float _ReorderModules_b__53_1(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ReorderModules_b__53_1_Internal_Single_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53179, XrefRangeEnd = 53182, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetModules_b__59_0(CGModule t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetModules_b__59_0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _CorrectDuplicateModuleIDs_b__81_0(CGModule module)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__CorrectDuplicateModuleIDs_b__81_0_Internal_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53182, XrefRangeEnd = 53187, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _ResetAllModuleLinks_b__82_0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ResetAllModuleLinks_b__82_0_Internal_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53187, XrefRangeEnd = 53189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IEnumerable<CGModule> _ReorderEndpointRecursiveInputs_b__84_0(CGModuleInputSlot i)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)i);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CGModule>>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe float _ReorderEndpointRecursiveInputs_b__84_1(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ReorderEndpointRecursiveInputs_b__84_1_Internal_Single_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IEnumerable<CGModule> _UpdateModulesRecursiveInputs_b__85_0(CGModuleInputSlot i)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)i);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__85_0_Internal_IEnumerable_1_CGModule_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CGModule>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _get_HasModulesWithSameID_b__88_0(CGModule module)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_0_Internal_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53189, XrefRangeEnd = 53192, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _get_HasModulesWithSameID_b__88_1(IGrouping<int, CGModule> group)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)group);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__get_HasModulesWithSameID_b__88_1_Internal_Boolean_IGrouping_2_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53192, XrefRangeEnd = 53194, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _SaveAllOutputManagedResources_b__90_0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SaveAllOutputManagedResources_b__90_0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c__DisplayClass61_0")]
	public sealed class __c__DisplayClass61_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_moduleID;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetModule_b__0_Internal_Boolean_CGModule_0;

		public unsafe int moduleID
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleID);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleID)) = num;
			}
		}

		static __c__DisplayClass61_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c__DisplayClass61_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr);
			NativeFieldInfoPtr_moduleID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr, "moduleID");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr, 100666432);
			NativeMethodInfoPtr__GetModule_b__0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr, 100666433);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass61_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53194, XrefRangeEnd = 53195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetModule_b__0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetModule_b__0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass61_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c__DisplayClass75_0")]
	public sealed class __c__DisplayClass75_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr_id;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetModuleUniqueID_b__0_Internal_Boolean_CGModule_0;

		public unsafe CGModule module
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_module);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_module)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
			}
		}

		public unsafe int id
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id)) = num;
			}
		}

		static __c__DisplayClass75_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c__DisplayClass75_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, "module");
			NativeFieldInfoPtr_id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, "id");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, 100666434);
			NativeMethodInfoPtr__GetModuleUniqueID_b__0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr, 100666435);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass75_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass75_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe bool _GetModuleUniqueID_b__0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetModuleUniqueID_b__0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass75_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c__DisplayClass78_0")]
	public sealed class __c__DisplayClass78_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr_uniqueName;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__IsModuleNameUnique_b__0_Internal_Boolean_CGModule_0;

		public unsafe CGModule module
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_module);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_module)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
			}
		}

		public unsafe string uniqueName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniqueName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_uniqueName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static __c__DisplayClass78_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c__DisplayClass78_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr, "module");
			NativeFieldInfoPtr_uniqueName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr, "uniqueName");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr, 100666436);
			NativeMethodInfoPtr__IsModuleNameUnique_b__0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr, 100666437);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass78_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass78_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe bool _IsModuleNameUnique_b__0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__IsModuleNameUnique_b__0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass78_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c__DisplayClass85_0")]
	public sealed class __c__DisplayClass85_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_modulesRecursiveInputs;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__1_Internal_IEnumerable_1_CGModule_CGModule_0;

		public unsafe Dictionary<CGModule, HashSet<CGModule>> modulesRecursiveInputs
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesRecursiveInputs);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CGModule, HashSet<CGModule>>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesRecursiveInputs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		static __c__DisplayClass85_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c__DisplayClass85_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr);
			NativeFieldInfoPtr_modulesRecursiveInputs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr, "modulesRecursiveInputs");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr, 100666438);
			NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__1_Internal_IEnumerable_1_CGModule_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr, 100666439);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass85_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass85_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53195, XrefRangeEnd = 53196, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IEnumerable<CGModule> _UpdateModulesRecursiveInputs_b__1(CGModule i)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)i);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__UpdateModulesRecursiveInputs_b__1_Internal_IEnumerable_1_CGModule_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CGModule>>(intPtr) : null;
		}

		public __c__DisplayClass85_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CurvyGenerator+<>c__DisplayClass90_0")]
	public sealed class __c__DisplayClass90_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_result;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SaveAllOutputManagedResources_b__1_Internal_Void_CGModule_0;

		public unsafe GameObject result
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_result);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_result)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
			}
		}

		static __c__DisplayClass90_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<>c__DisplayClass90_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr);
			NativeFieldInfoPtr_result = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr, "result");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr, 100666440);
			NativeMethodInfoPtr__SaveAllOutputManagedResources_b__1_Internal_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr, 100666441);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass90_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass90_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53196, XrefRangeEnd = 53202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _SaveAllOutputManagedResources_b__1(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SaveAllOutputManagedResources_b__1_Internal_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c__DisplayClass90_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_AddModule_Public_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_AddModule_Public_T_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_FindModules_Public_List_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_FindModules_Public_List_1_T_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_FindModules_Public_List_1_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_FindModules_Public_List_1_T_Boolean_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetModule_Public_T_Int32_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetModule_Public_T_Int32_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetModule_Public_T_Int32_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetModule_Public_T_Int32_Boolean_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetModule_Public_T_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetModule_Public_T_String_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetModule_Public_T_String_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetModule_Public_T_String_Boolean_0, Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_m_ShowDebug;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_AutoRefresh;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RefreshDelay;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RefreshDelayEditor;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnRefresh;

	private static readonly System.IntPtr NativeFieldInfoPtr_Modules;

	private static readonly System.IntPtr NativeFieldInfoPtr__Destroying_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_isInitialized;

	private static readonly System.IntPtr NativeFieldInfoPtr_isInitializedPhaseOne;

	private static readonly System.IntPtr NativeFieldInfoPtr_poolManager;

	private static readonly System.IntPtr NativeFieldInfoPtr_autoRefreshTimer;

	private static readonly System.IntPtr NativeFieldInfoPtr_moduleSorter;

	private static readonly System.IntPtr NativeFieldInfoPtr_modulesSynchronizer;

	private static readonly System.IntPtr NativeFieldInfoPtr_ModulesReorderingDeltaX;

	private static readonly System.IntPtr NativeFieldInfoPtr_ModulesReorderingDeltaY;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_m_LastModuleID_Internal_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_m_LastModuleID_Internal_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ShowDebug_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ShowDebug_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AutoRefresh_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AutoRefresh_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RefreshDelay_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RefreshDelay_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RefreshDelayEditor_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RefreshDelayEditor_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsInitialized_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Destroying_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Destroying_Private_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ModulesByID_Public_get_Dictionary_2_Int32_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ModulesByID_Public_set_Void_Dictionary_2_Int32_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Create_Public_Static_CurvyGenerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddModule_Public_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddModule_Public_CGModule_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddModule_Public_Void_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveModule_Public_Void_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ArrangeModules_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReorderModules_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteModule_Public_Void_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindModules_Public_List_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindModules_Public_List_1_T_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_T_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_T_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_CGModule_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_CGModule_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_T_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModule_Public_T_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryAutoRefresh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Boolean_byref_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModuleUniqueName_Public_String_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetModuleUniqueID_Public_Int32_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsModuleNameUnique_Private_Boolean_CGModule_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_getUniqueModuleNameINTERNAL_Public_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_sortModulesINTERNAL_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CorrectDuplicateModuleIDs_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetAllModuleLinks_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReorderEndpointRecursiveInputs_Private_Static_Void_CGModule_HashSet_1_Int32_Dictionary_2_CGModule_HashSet_1_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateModulesRecursiveInputs_Private_Static_HashSet_1_CGModule_Dictionary_2_CGModule_HashSet_1_CGModule_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetModulesFromChildren_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasModulesWithSameID_Private_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteAllOutputManagedResourcesFromAssociatedPrefab_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveAllOutputManagedResources_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__SetModulesFromChildren_b__86_0_Private_Boolean_CGModule_0;

	public unsafe bool m_ShowDebug
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ShowDebug);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ShowDebug)) = flag;
		}
	}

	public unsafe bool m_AutoRefresh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoRefresh);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_AutoRefresh)) = flag;
		}
	}

	public unsafe int m_RefreshDelay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RefreshDelay);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RefreshDelay)) = num;
		}
	}

	public unsafe int m_RefreshDelayEditor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RefreshDelayEditor);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RefreshDelayEditor)) = num;
		}
	}

	public unsafe CurvyCGEvent m_OnRefresh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnRefresh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnRefresh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyCGEvent));
		}
	}

	public unsafe List<CGModule> Modules
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Modules);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModule>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Modules)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool _Destroying_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Destroying_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Destroying_k__BackingField)) = flag;
		}
	}

	public unsafe bool isInitialized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitialized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitialized)) = flag;
		}
	}

	public unsafe bool isInitializedPhaseOne
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitializedPhaseOne);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInitializedPhaseOne)) = flag;
		}
	}

	public unsafe PoolManager poolManager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolManager);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PoolManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_poolManager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)poolManager));
		}
	}

	public unsafe Timer autoRefreshTimer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_autoRefreshTimer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Timer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_autoRefreshTimer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timer));
		}
	}

	public unsafe ModuleSorter moduleSorter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleSorter);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleSorter>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleSorter)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)moduleSorter));
		}
	}

	public unsafe ModulesSynchronizer modulesSynchronizer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesSynchronizer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModulesSynchronizer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_modulesSynchronizer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)modulesSynchronizer));
		}
	}

	public unsafe static int ModulesReorderingDeltaX
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ModulesReorderingDeltaX, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ModulesReorderingDeltaX, (void*)(&num));
		}
	}

	public unsafe static int ModulesReorderingDeltaY
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ModulesReorderingDeltaY, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ModulesReorderingDeltaY, (void*)(&num));
		}
	}

	public unsafe int m_LastModuleID
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53202, XrefRangeEnd = 53220, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_m_LastModuleID_Internal_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53220, XrefRangeEnd = 53226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_m_LastModuleID_Internal_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool ShowDebug
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ShowDebug_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ShowDebug_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool AutoRefresh
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AutoRefresh_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 53226, RefRangeEnd = 53227, XrefRangeStart = 53226, XrefRangeEnd = 53226, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AutoRefresh_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int RefreshDelay
	{
		[CallerCount(126)]
		[CachedScanResults(RefRangeStart = 41326, RefRangeEnd = 41452, XrefRangeStart = 41326, XrefRangeEnd = 41452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RefreshDelay_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RefreshDelay_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int RefreshDelayEditor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RefreshDelayEditor_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RefreshDelayEditor_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PoolManager PoolManager
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 53235, RefRangeEnd = 53243, XrefRangeStart = 53227, XrefRangeEnd = 53235, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PoolManager>(intPtr) : null;
		}
	}

	public unsafe CurvyCGEvent OnRefresh
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvyCGEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvyCGEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

	public unsafe bool Destroying
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Destroying_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Destroying_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<int, CGModule> ModulesByID
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53243, XrefRangeEnd = 53275, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ModulesByID_Public_get_Dictionary_2_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<int, CGModule>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53275, XrefRangeEnd = 53281, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ModulesByID_Public_set_Void_Dictionary_2_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool HasModulesWithSameID
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54158, XrefRangeEnd = 54193, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasModulesWithSameID_Private_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CurvyGenerator()
	{
		Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CurvyGenerator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr);
		NativeFieldInfoPtr_m_ShowDebug = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "m_ShowDebug");
		NativeFieldInfoPtr_m_AutoRefresh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "m_AutoRefresh");
		NativeFieldInfoPtr_m_RefreshDelay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "m_RefreshDelay");
		NativeFieldInfoPtr_m_RefreshDelayEditor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "m_RefreshDelayEditor");
		NativeFieldInfoPtr_m_OnRefresh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "m_OnRefresh");
		NativeFieldInfoPtr_Modules = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "Modules");
		NativeFieldInfoPtr__Destroying_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "<Destroying>k__BackingField");
		NativeFieldInfoPtr_isInitialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "isInitialized");
		NativeFieldInfoPtr_isInitializedPhaseOne = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "isInitializedPhaseOne");
		NativeFieldInfoPtr_poolManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "poolManager");
		NativeFieldInfoPtr_autoRefreshTimer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "autoRefreshTimer");
		NativeFieldInfoPtr_moduleSorter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "moduleSorter");
		NativeFieldInfoPtr_modulesSynchronizer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "modulesSynchronizer");
		NativeFieldInfoPtr_ModulesReorderingDeltaX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "ModulesReorderingDeltaX");
		NativeFieldInfoPtr_ModulesReorderingDeltaY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, "ModulesReorderingDeltaY");
		NativeMethodInfoPtr_get_m_LastModuleID_Internal_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666327);
		NativeMethodInfoPtr_set_m_LastModuleID_Internal_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666328);
		NativeMethodInfoPtr_get_ShowDebug_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666329);
		NativeMethodInfoPtr_set_ShowDebug_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666330);
		NativeMethodInfoPtr_get_AutoRefresh_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666331);
		NativeMethodInfoPtr_set_AutoRefresh_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666332);
		NativeMethodInfoPtr_get_RefreshDelay_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666333);
		NativeMethodInfoPtr_set_RefreshDelay_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666334);
		NativeMethodInfoPtr_get_RefreshDelayEditor_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666335);
		NativeMethodInfoPtr_set_RefreshDelayEditor_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666336);
		NativeMethodInfoPtr_get_PoolManager_Public_get_PoolManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666337);
		NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666338);
		NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666339);
		NativeMethodInfoPtr_get_IsInitialized_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666340);
		NativeMethodInfoPtr_get_Destroying_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666341);
		NativeMethodInfoPtr_set_Destroying_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666342);
		NativeMethodInfoPtr_get_ModulesByID_Public_get_Dictionary_2_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666343);
		NativeMethodInfoPtr_set_ModulesByID_Public_set_Void_Dictionary_2_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666344);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666345);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666346);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666347);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666348);
		NativeMethodInfoPtr_Create_Public_Static_CurvyGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666349);
		NativeMethodInfoPtr_AddModule_Public_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666350);
		NativeMethodInfoPtr_AddModule_Public_CGModule_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666351);
		NativeMethodInfoPtr_AddModule_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666352);
		NativeMethodInfoPtr_RemoveModule_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666353);
		NativeMethodInfoPtr_ArrangeModules_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666354);
		NativeMethodInfoPtr_ReorderModules_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666355);
		NativeMethodInfoPtr_Clear_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666356);
		NativeMethodInfoPtr_DeleteModule_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666357);
		NativeMethodInfoPtr_FindModules_Public_List_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666358);
		NativeMethodInfoPtr_FindModules_Public_List_1_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666359);
		NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666360);
		NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666361);
		NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666362);
		NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666363);
		NativeMethodInfoPtr_GetModule_Public_T_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666364);
		NativeMethodInfoPtr_GetModule_Public_T_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666365);
		NativeMethodInfoPtr_GetModule_Public_CGModule_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666366);
		NativeMethodInfoPtr_GetModule_Public_CGModule_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666367);
		NativeMethodInfoPtr_GetModule_Public_T_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666368);
		NativeMethodInfoPtr_GetModule_Public_T_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666369);
		NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666370);
		NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666371);
		NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666372);
		NativeMethodInfoPtr_Refresh_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666373);
		NativeMethodInfoPtr_TryAutoRefresh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666374);
		NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Boolean_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666375);
		NativeMethodInfoPtr_GetModuleUniqueName_Public_String_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666376);
		NativeMethodInfoPtr_GetModuleUniqueID_Public_Int32_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666377);
		NativeMethodInfoPtr_OnRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666378);
		NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666379);
		NativeMethodInfoPtr_IsModuleNameUnique_Private_Boolean_CGModule_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666380);
		NativeMethodInfoPtr_getUniqueModuleNameINTERNAL_Public_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666381);
		NativeMethodInfoPtr_sortModulesINTERNAL_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666382);
		NativeMethodInfoPtr_CorrectDuplicateModuleIDs_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666383);
		NativeMethodInfoPtr_ResetAllModuleLinks_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666384);
		NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666385);
		NativeMethodInfoPtr_ReorderEndpointRecursiveInputs_Private_Static_Void_CGModule_HashSet_1_Int32_Dictionary_2_CGModule_HashSet_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666386);
		NativeMethodInfoPtr_UpdateModulesRecursiveInputs_Private_Static_HashSet_1_CGModule_Dictionary_2_CGModule_HashSet_1_CGModule_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666387);
		NativeMethodInfoPtr_SetModulesFromChildren_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666388);
		NativeMethodInfoPtr_get_HasModulesWithSameID_Private_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666389);
		NativeMethodInfoPtr_DeleteAllOutputManagedResourcesFromAssociatedPrefab_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666390);
		NativeMethodInfoPtr_SaveAllOutputManagedResources_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666391);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666392);
		NativeMethodInfoPtr__SetModulesFromChildren_b__86_0_Private_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr, 100666393);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53281, XrefRangeEnd = 53292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53292, XrefRangeEnd = 53293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53293, RefRangeEnd = 53294, XrefRangeStart = 53293, XrefRangeEnd = 53293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53294, XrefRangeEnd = 53301, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53301, XrefRangeEnd = 53325, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CurvyGenerator Create()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Static_CurvyGenerator_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyGenerator>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53325, XrefRangeEnd = 53332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T AddModule<T>() where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_AddModule_Public_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53345, RefRangeEnd = 53346, XrefRangeStart = 53332, XrefRangeEnd = 53345, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule AddModule(Il2CppSystem.Type type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddModule_Public_CGModule_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 53383, RefRangeEnd = 53386, XrefRangeStart = 53346, XrefRangeEnd = 53383, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddModule(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddModule_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53392, RefRangeEnd = 53393, XrefRangeStart = 53386, XrefRangeEnd = 53392, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveModule(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveModule_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53417, RefRangeEnd = 53418, XrefRangeStart = 53393, XrefRangeEnd = 53417, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ArrangeModules()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ArrangeModules_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53418, XrefRangeEnd = 53523, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReorderModules()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReorderModules_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53523, XrefRangeEnd = 53538, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53538, XrefRangeEnd = 53543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeleteModule(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteModule_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53543, XrefRangeEnd = 53545, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<T> FindModules<T>() where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_FindModules_Public_List_1_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53567, RefRangeEnd = 53568, XrefRangeStart = 53545, XrefRangeEnd = 53567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<T> FindModules<T>(bool includeOnRequestProcessing) where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&includeOnRequestProcessing);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_FindModules_Public_List_1_T_Boolean_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53568, XrefRangeEnd = 53591, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModule> GetModules()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModule>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53591, XrefRangeEnd = 53618, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModule> GetModules(bool includeOnRequestProcessing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&includeOnRequestProcessing);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModules_Public_List_1_CGModule_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModule>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53618, XrefRangeEnd = 53619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule GetModule(int moduleID)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&moduleID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 53639, RefRangeEnd = 53645, XrefRangeStart = 53619, XrefRangeEnd = 53639, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule GetModule(int moduleID, bool includeOnRequestProcessing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&moduleID);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeOnRequestProcessing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModule_Public_CGModule_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53645, XrefRangeEnd = 53651, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetModule<T>(int moduleID) where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&moduleID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetModule_Public_T_Int32_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53651, XrefRangeEnd = 53656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetModule<T>(int moduleID, bool includeOnRequestProcessing) where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&moduleID);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeOnRequestProcessing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetModule_Public_T_Int32_Boolean_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53656, XrefRangeEnd = 53657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule GetModule(string moduleName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(moduleName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModule_Public_CGModule_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 53671, RefRangeEnd = 53677, XrefRangeStart = 53657, XrefRangeEnd = 53671, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule GetModule(string moduleName, bool includeOnRequestProcessing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(moduleName);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeOnRequestProcessing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModule_Public_CGModule_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53677, XrefRangeEnd = 53683, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetModule<T>(string moduleName) where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(moduleName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetModule_Public_T_String_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53683, XrefRangeEnd = 53688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetModule<T>(string moduleName, bool includeOnRequestProcessing) where T : CGModule
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(moduleName);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeOnRequestProcessing;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetModule_Public_T_String_Boolean_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53688, XrefRangeEnd = 53693, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleOutputSlot GetModuleOutputSlot(int moduleId, string slotName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&moduleId);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(slotName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53693, XrefRangeEnd = 53698, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleOutputSlot GetModuleOutputSlot(string moduleName, string slotName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(moduleName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(slotName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModuleOutputSlot_Public_CGModuleOutputSlot_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53698, XrefRangeEnd = 53765, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialize(bool force = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&force);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 53796, RefRangeEnd = 53799, XrefRangeStart = 53765, XrefRangeEnd = 53796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh(bool forceUpdate = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&forceUpdate);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53799, XrefRangeEnd = 53806, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TryAutoRefresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryAutoRefresh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53806, XrefRangeEnd = 53820, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DeleteAllOutputManagedResources(out bool associatedPrefabWasModified)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref associatedPrefabWasModified);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Boolean_byref_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53840, RefRangeEnd = 53841, XrefRangeStart = 53820, XrefRangeEnd = 53840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetModuleUniqueName(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModuleUniqueName_Public_String_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 53860, RefRangeEnd = 53863, XrefRangeStart = 53841, XrefRangeEnd = 53860, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetModuleUniqueID(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetModuleUniqueID_Public_Int32_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53863, XrefRangeEnd = 53866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyCGEventArgs OnRefreshEvent(CurvyCGEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEventArgs>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53866, XrefRangeEnd = 53867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetOnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53867, XrefRangeEnd = 53883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsModuleNameUnique(CGModule module, string uniqueName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(uniqueName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsModuleNameUnique_Private_Boolean_CGModule_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53883, XrefRangeEnd = 53908, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string getUniqueModuleNameINTERNAL(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getUniqueModuleNameINTERNAL_Public_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53908, RefRangeEnd = 53909, XrefRangeStart = 53908, XrefRangeEnd = 53908, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void sortModulesINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_sortModulesINTERNAL_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 53982, RefRangeEnd = 53983, XrefRangeStart = 53909, XrefRangeEnd = 53982, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CorrectDuplicateModuleIDs()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CorrectDuplicateModuleIDs_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 53983, XrefRangeEnd = 54002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetAllModuleLinks()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetAllModuleLinks_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 54006, RefRangeEnd = 54010, XrefRangeStart = 54002, XrefRangeEnd = 54006, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool HasCircularReference(CGModule module)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasCircularReference_Public_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 54081, RefRangeEnd = 54084, XrefRangeStart = 54010, XrefRangeEnd = 54081, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ReorderEndpointRecursiveInputs(CGModule endPoint, HashSet<int> reordredModuleIds, Dictionary<CGModule, HashSet<CGModule>> modulesRecursiveInputs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)endPoint);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reordredModuleIds);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)modulesRecursiveInputs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReorderEndpointRecursiveInputs_Private_Static_Void_CGModule_HashSet_1_Int32_Dictionary_2_CGModule_HashSet_1_CGModule_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54139, RefRangeEnd = 54141, XrefRangeStart = 54084, XrefRangeEnd = 54139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static HashSet<CGModule> UpdateModulesRecursiveInputs(Dictionary<CGModule, HashSet<CGModule>> modulesRecursiveInputs, CGModule moduleToAdd)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)modulesRecursiveInputs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)moduleToAdd);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateModulesRecursiveInputs_Private_Static_HashSet_1_CGModule_Dictionary_2_CGModule_HashSet_1_CGModule_CGModule_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<CGModule>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54141, XrefRangeEnd = 54158, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetModulesFromChildren()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetModulesFromChildren_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(170)]
	[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool DeleteAllOutputManagedResourcesFromAssociatedPrefab()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteAllOutputManagedResourcesFromAssociatedPrefab_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54193, XrefRangeEnd = 54246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SaveAllOutputManagedResources()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveAllOutputManagedResources_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54246, XrefRangeEnd = 54288, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyGenerator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurvyGenerator>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54288, XrefRangeEnd = 54296, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool _SetModulesFromChildren_b__86_0(CGModule m)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetModulesFromChildren_b__86_0_Private_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CurvyGenerator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
