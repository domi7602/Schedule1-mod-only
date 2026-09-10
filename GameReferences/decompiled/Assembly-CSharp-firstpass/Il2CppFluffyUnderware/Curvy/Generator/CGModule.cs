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
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGModule : DTVersionedMonoBehaviour
{
	public class DirtinessManager : Il2CppSystem.Object
	{
		[System.Serializable]
		[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModule+DirtinessManager+<>c")]
		public sealed class __c : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr___9;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			private static readonly System.IntPtr NativeMethodInfoPtr___cctor_b__16_0_Internal_Void_CGModule_0;

			private static readonly System.IntPtr NativeMethodInfoPtr___cctor_b__16_1_Internal_Void_CGModule_0;

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

			static __c()
			{
				Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "<>c");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
				NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665968);
				NativeMethodInfoPtr___cctor_b__16_0_Internal_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665969);
				NativeMethodInfoPtr___cctor_b__16_1_Internal_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665970);
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
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47634, XrefRangeEnd = 47636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe void __cctor_b__16_0(CGModule m)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___cctor_b__16_0_Internal_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47636, XrefRangeEnd = 47642, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe void __cctor_b__16_1(CGModule m)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___cctor_b__16_1_Internal_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			public __c(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr_isDirty;

		private static readonly System.IntPtr NativeFieldInfoPtr_isStateChangeDirty;

		private static readonly System.IntPtr NativeFieldInfoPtr_lastIsConfiguredState;

		private static readonly System.IntPtr NativeFieldInfoPtr_SetDirtyAction;

		private static readonly System.IntPtr NativeFieldInfoPtr_SetTreeDirtyStateChangeAction;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsDirty_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_IsDirty_Public_set_Void_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_UnsetDirtyFlag_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CheckOnStateChanged_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_SetTreeDirtyStateChange_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ForEachValidOutputModule_Private_Void_Action_1_CGModule_0;

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

		public unsafe bool isDirty
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isDirty);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isDirty)) = flag;
			}
		}

		public unsafe bool isStateChangeDirty
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isStateChangeDirty);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isStateChangeDirty)) = flag;
			}
		}

		public unsafe bool lastIsConfiguredState
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastIsConfiguredState);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastIsConfiguredState)) = flag;
			}
		}

		public unsafe static Il2CppSystem.Action<CGModule> SetDirtyAction
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SetDirtyAction, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CGModule>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SetDirtyAction, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe static Il2CppSystem.Action<CGModule> SetTreeDirtyStateChangeAction
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SetTreeDirtyStateChangeAction, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<CGModule>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SetTreeDirtyStateChangeAction, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe bool IsDirty
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsDirty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(23)]
			[CachedScanResults(RefRangeStart = 47657, RefRangeEnd = 47680, XrefRangeStart = 47645, XrefRangeEnd = 47657, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsDirty_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static DirtinessManager()
		{
			Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "DirtinessManager");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "module");
			NativeFieldInfoPtr_isDirty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "isDirty");
			NativeFieldInfoPtr_isStateChangeDirty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "isStateChangeDirty");
			NativeFieldInfoPtr_lastIsConfiguredState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "lastIsConfiguredState");
			NativeFieldInfoPtr_SetDirtyAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "SetDirtyAction");
			NativeFieldInfoPtr_SetTreeDirtyStateChangeAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, "SetTreeDirtyStateChangeAction");
			NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665957);
			NativeMethodInfoPtr_get_IsDirty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665958);
			NativeMethodInfoPtr_set_IsDirty_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665959);
			NativeMethodInfoPtr_UnsetDirtyFlag_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665960);
			NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665961);
			NativeMethodInfoPtr_CheckOnStateChanged_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665962);
			NativeMethodInfoPtr_OnDestroy_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665963);
			NativeMethodInfoPtr_SetTreeDirtyStateChange_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665964);
			NativeMethodInfoPtr_ForEachValidOutputModule_Private_Void_Action_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr, 100665965);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47644, RefRangeEnd = 47645, XrefRangeStart = 47642, XrefRangeEnd = 47644, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe DirtinessManager(CGModule module)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DirtinessManager>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 37917, RefRangeEnd = 37919, XrefRangeStart = 37917, XrefRangeEnd = 37919, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void UnsetDirtyFlag()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnsetDirtyFlag_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47680, RefRangeEnd = 47681, XrefRangeStart = 47680, XrefRangeEnd = 47680, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47681, RefRangeEnd = 47682, XrefRangeStart = 47681, XrefRangeEnd = 47681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CheckOnStateChanged()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckOnStateChanged_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47687, RefRangeEnd = 47688, XrefRangeStart = 47682, XrefRangeEnd = 47687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnDestroy()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47687, RefRangeEnd = 47688, XrefRangeStart = 47687, XrefRangeEnd = 47688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void SetTreeDirtyStateChange()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetTreeDirtyStateChange_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 47709, RefRangeEnd = 47713, XrefRangeStart = 47688, XrefRangeEnd = 47709, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ForEachValidOutputModule(Il2CppSystem.Action<CGModule> action)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForEachValidOutputModule_Private_Void_Action_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public DirtinessManager(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Identifier : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr_cachedStringID;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_ID_Public_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_ID_Public_set_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_StringID_Public_get_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

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

		public unsafe string cachedStringID
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedStringID);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cachedStringID)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe int ID
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 47713, RefRangeEnd = 47717, XrefRangeStart = 47713, XrefRangeEnd = 47713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ID_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 47719, RefRangeEnd = 47722, XrefRangeStart = 47717, XrefRangeEnd = 47719, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ID_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe string StringID
		{
			[CallerCount(5)]
			[CachedScanResults(RefRangeStart = 47728, RefRangeEnd = 47733, XrefRangeStart = 47722, XrefRangeEnd = 47728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StringID_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
		}

		static Identifier()
		{
			Il2CppClassPointerStore<Identifier>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "Identifier");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Identifier>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Identifier>.NativeClassPtr, "module");
			NativeFieldInfoPtr_cachedStringID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Identifier>.NativeClassPtr, "cachedStringID");
			NativeMethodInfoPtr_get_ID_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Identifier>.NativeClassPtr, 100665971);
			NativeMethodInfoPtr_set_ID_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Identifier>.NativeClassPtr, 100665972);
			NativeMethodInfoPtr_get_StringID_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Identifier>.NativeClassPtr, 100665973);
			NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Identifier>.NativeClassPtr, 100665974);
			NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Identifier>.NativeClassPtr, 100665975);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Identifier(CGModule module)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Identifier>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47734, RefRangeEnd = 47736, XrefRangeStart = 47733, XrefRangeEnd = 47734, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Identifier(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class InformationProvider : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr_moduleInformation;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Information_Public_get_ModuleInfoAttribute_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetInformation_Private_ModuleInfoAttribute_0;

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

		public unsafe ModuleInfoAttribute moduleInformation
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleInformation);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleInfoAttribute>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_moduleInformation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)moduleInfoAttribute));
			}
		}

		public unsafe ModuleInfoAttribute Information
		{
			[CallerCount(7)]
			[CachedScanResults(RefRangeStart = 47747, RefRangeEnd = 47754, XrefRangeStart = 47736, XrefRangeEnd = 47747, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Information_Public_get_ModuleInfoAttribute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleInfoAttribute>(intPtr) : null;
			}
		}

		static InformationProvider()
		{
			Il2CppClassPointerStore<InformationProvider>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "InformationProvider");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr, "module");
			NativeFieldInfoPtr_moduleInformation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr, "moduleInformation");
			NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr, 100665976);
			NativeMethodInfoPtr_get_Information_Public_get_ModuleInfoAttribute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr, 100665977);
			NativeMethodInfoPtr_GetInformation_Private_ModuleInfoAttribute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr, 100665978);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe InformationProvider(CGModule module)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InformationProvider>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47754, XrefRangeEnd = 47764, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ModuleInfoAttribute GetInformation()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInformation_Private_ModuleInfoAttribute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleInfoAttribute>(intPtr) : null;
		}

		public InformationProvider(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class ResourceNamer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_cgModule;

		private static readonly System.IntPtr NativeFieldInfoPtr_resourcesNameCache;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ClearCache_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetResourceName_Private_String_String_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Rename_Public_Void_String_Component_Int32_0;

		public unsafe CGModule cgModule
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cgModule);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cgModule)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
			}
		}

		public unsafe Dictionary<string, Dictionary<int, string>> resourcesNameCache
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourcesNameCache);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, Dictionary<int, string>>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourcesNameCache)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		static ResourceNamer()
		{
			Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "ResourceNamer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr);
			NativeFieldInfoPtr_cgModule = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, "cgModule");
			NativeFieldInfoPtr_resourcesNameCache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, "resourcesNameCache");
			NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, 100665979);
			NativeMethodInfoPtr_ClearCache_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, 100665980);
			NativeMethodInfoPtr_GetResourceName_Private_String_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, 100665981);
			NativeMethodInfoPtr_Rename_Public_Void_String_Component_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr, 100665982);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47773, RefRangeEnd = 47774, XrefRangeStart = 47764, XrefRangeEnd = 47773, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ResourceNamer(CGModule cgModule)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ResourceNamer>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cgModule);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 47778, RefRangeEnd = 47787, XrefRangeStart = 47774, XrefRangeEnd = 47778, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ClearCache()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearCache_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47836, RefRangeEnd = 47837, XrefRangeStart = 47787, XrefRangeEnd = 47836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe string GetResourceName(string resourceName, int index)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetResourceName_Private_String_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 47841, RefRangeEnd = 47844, XrefRangeStart = 47837, XrefRangeEnd = 47841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Rename(string resourceName, Component resource, int index)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)resource);
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Rename_Public_Void_String_Component_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ResourceNamer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class Slots : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_module;

		private static readonly System.IntPtr NativeFieldInfoPtr__InputSlotsByName_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__OutputSlotsByName_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__InputSlots_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__OutputSlots_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_InputSlotsByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_OutputSlotsByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_InputSlots_Public_get_List_1_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_OutputSlots_Public_get_List_1_CGModuleOutputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsConfigured_Public_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGModule_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Setup_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetSlot_Private_CGModuleSlot_FieldInfo_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Store_Private_Void_CGModuleSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Private_Static_Void_CGModuleSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResetInputSlotsLastDataCount_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ResetLasRequestedParameters_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ClearOutputData_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CheckInputModulesNotDirty_Public_Void_0;

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

		public unsafe Dictionary<string, CGModuleInputSlot> _InputSlotsByName_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InputSlotsByName_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleInputSlot>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InputSlotsByName_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe Dictionary<string, CGModuleOutputSlot> _OutputSlotsByName_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OutputSlotsByName_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleOutputSlot>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OutputSlotsByName_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe List<CGModuleInputSlot> _InputSlots_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InputSlots_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleInputSlot>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InputSlots_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe List<CGModuleOutputSlot> _OutputSlots_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OutputSlots_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleOutputSlot>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OutputSlots_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe Dictionary<string, CGModuleInputSlot> InputSlotsByName
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InputSlotsByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleInputSlot>>(intPtr) : null;
			}
		}

		public unsafe Dictionary<string, CGModuleOutputSlot> OutputSlotsByName
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OutputSlotsByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleOutputSlot>>(intPtr) : null;
			}
		}

		public unsafe List<CGModuleInputSlot> InputSlots
		{
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InputSlots_Public_get_List_1_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleInputSlot>>(intPtr) : null;
			}
		}

		public unsafe List<CGModuleOutputSlot> OutputSlots
		{
			[CallerCount(13)]
			[CachedScanResults(RefRangeStart = 2964, RefRangeEnd = 2977, XrefRangeStart = 2964, XrefRangeEnd = 2977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OutputSlots_Public_get_List_1_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleOutputSlot>>(intPtr) : null;
			}
		}

		public unsafe bool IsConfigured
		{
			[CallerCount(2)]
			[CachedScanResults(RefRangeStart = 47867, RefRangeEnd = 47869, XrefRangeStart = 47844, XrefRangeEnd = 47867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsConfigured_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Slots()
		{
			Il2CppClassPointerStore<Slots>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "Slots");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Slots>.NativeClassPtr);
			NativeFieldInfoPtr_module = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slots>.NativeClassPtr, "module");
			NativeFieldInfoPtr__InputSlotsByName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slots>.NativeClassPtr, "<InputSlotsByName>k__BackingField");
			NativeFieldInfoPtr__OutputSlotsByName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slots>.NativeClassPtr, "<OutputSlotsByName>k__BackingField");
			NativeFieldInfoPtr__InputSlots_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slots>.NativeClassPtr, "<InputSlots>k__BackingField");
			NativeFieldInfoPtr__OutputSlots_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slots>.NativeClassPtr, "<OutputSlots>k__BackingField");
			NativeMethodInfoPtr_get_InputSlotsByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665983);
			NativeMethodInfoPtr_get_OutputSlotsByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665984);
			NativeMethodInfoPtr_get_InputSlots_Public_get_List_1_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665985);
			NativeMethodInfoPtr_get_OutputSlots_Public_get_List_1_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665986);
			NativeMethodInfoPtr_get_IsConfigured_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665987);
			NativeMethodInfoPtr__ctor_Public_Void_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665988);
			NativeMethodInfoPtr_Setup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665989);
			NativeMethodInfoPtr_GetSlot_Private_CGModuleSlot_FieldInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665990);
			NativeMethodInfoPtr_Store_Private_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665991);
			NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665992);
			NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Private_Static_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665993);
			NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665994);
			NativeMethodInfoPtr_ResetInputSlotsLastDataCount_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665995);
			NativeMethodInfoPtr_ResetLasRequestedParameters_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665996);
			NativeMethodInfoPtr_ClearOutputData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665997);
			NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665998);
			NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100665999);
			NativeMethodInfoPtr_CheckInputModulesNotDirty_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slots>.NativeClassPtr, 100666000);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47906, RefRangeEnd = 47907, XrefRangeStart = 47869, XrefRangeEnd = 47906, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Slots(CGModule module)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Slots>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)module);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 47907, XrefRangeEnd = 47914, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Setup()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Setup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47933, RefRangeEnd = 47935, XrefRangeStart = 47914, XrefRangeEnd = 47933, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CGModuleSlot GetSlot(FieldInfo fieldInfo)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fieldInfo);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSlot_Private_CGModuleSlot_FieldInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleSlot>(intPtr) : null;
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47946, RefRangeEnd = 47948, XrefRangeStart = 47935, XrefRangeEnd = 47946, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void Store(CGModuleSlot slot)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Store_Private_Void_CGModuleSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 47978, RefRangeEnd = 47979, XrefRangeStart = 47948, XrefRangeEnd = 47978, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ReinitializeLinkedModulesLinkedSlots()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47990, RefRangeEnd = 47992, XrefRangeStart = 47979, XrefRangeEnd = 47990, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void ReinitializeLinkedModulesLinkedSlots(CGModuleSlot slot)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReinitializeLinkedModulesLinkedSlots_Private_Static_Void_CGModuleSlot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 48022, RefRangeEnd = 48026, XrefRangeStart = 47992, XrefRangeEnd = 48022, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ReInitializeLinkedSlots()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResetInputSlotsLastDataCount()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetInputSlotsLastDataCount_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48041, RefRangeEnd = 48042, XrefRangeStart = 48026, XrefRangeEnd = 48041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ResetLasRequestedParameters()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetLasRequestedParameters_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48060, RefRangeEnd = 48061, XrefRangeStart = 48042, XrefRangeEnd = 48060, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void ClearOutputData()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearOutputData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48067, RefRangeEnd = 48068, XrefRangeStart = 48061, XrefRangeEnd = 48067, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CGModuleInputSlot GetInputSlot(string name)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 48074, RefRangeEnd = 48076, XrefRangeStart = 48068, XrefRangeEnd = 48074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CGModuleOutputSlot GetOutputSlot(string name)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48110, RefRangeEnd = 48111, XrefRangeStart = 48076, XrefRangeEnd = 48110, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void CheckInputModulesNotDirty()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckInputModulesNotDirty_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Slots(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModule+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__102_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__102_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__103_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__103_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__103_2;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__initializeSort_b__102_0_Internal_Boolean_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__initializeSort_b__102_1_Internal_Int32_CGModuleInputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__decrementChilds_b__103_0_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__decrementChilds_b__103_1_Internal_Boolean_CGModuleSlot_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__decrementChilds_b__103_2_Internal_CGModule_CGModuleSlot_0;

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

		public unsafe static Il2CppSystem.Func<CGModuleInputSlot, bool> __9__102_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__102_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__102_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleInputSlot, int> __9__102_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__102_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleInputSlot, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__102_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleOutputSlot, IEnumerable<CGModuleSlot>> __9__103_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__103_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleOutputSlot, IEnumerable<CGModuleSlot>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__103_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleSlot, bool> __9__103_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__103_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleSlot, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__103_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGModuleSlot, CGModule> __9__103_2
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__103_2, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGModuleSlot, CGModule>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__103_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__102_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__102_0");
			NativeFieldInfoPtr___9__102_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__102_1");
			NativeFieldInfoPtr___9__103_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__103_0");
			NativeFieldInfoPtr___9__103_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__103_1");
			NativeFieldInfoPtr___9__103_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__103_2");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666002);
			NativeMethodInfoPtr__initializeSort_b__102_0_Internal_Boolean_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666003);
			NativeMethodInfoPtr__initializeSort_b__102_1_Internal_Int32_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666004);
			NativeMethodInfoPtr__decrementChilds_b__103_0_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666005);
			NativeMethodInfoPtr__decrementChilds_b__103_1_Internal_Boolean_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666006);
			NativeMethodInfoPtr__decrementChilds_b__103_2_Internal_CGModule_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100666007);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48111, XrefRangeEnd = 48113, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _initializeSort_b__102_0(CGModuleInputSlot t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__initializeSort_b__102_0_Internal_Boolean_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48113, XrefRangeEnd = 48115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe int _initializeSort_b__102_1(CGModuleInputSlot t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__initializeSort_b__102_1_Internal_Int32_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48115, XrefRangeEnd = 48117, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe IEnumerable<CGModuleSlot> _decrementChilds_b__103_0(CGModuleOutputSlot outputSlot)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__decrementChilds_b__103_0_Internal_IEnumerable_1_CGModuleSlot_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<CGModuleSlot>>(intPtr) : null;
		}

		[CallerCount(0)]
		public unsafe bool _decrementChilds_b__103_1(CGModuleSlot t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__decrementChilds_b__103_1_Internal_Boolean_CGModuleSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe CGModule _decrementChilds_b__103_2(CGModuleSlot t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__decrementChilds_b__103_2_Internal_CGModule_CGModuleSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModule+<>c__DisplayClass105_0")]
	public sealed class __c__DisplayClass105_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___4__this;

		private static readonly System.IntPtr NativeFieldInfoPtr_id;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__SetUniqueIdINTERNAL_b__0_Internal_Boolean_CGModule_0;

		public unsafe CGModule __4__this
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModule));
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

		static __c__DisplayClass105_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "<>c__DisplayClass105_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr);
			NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr, "<>4__this");
			NativeFieldInfoPtr_id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr, "id");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr, 100666008);
			NativeMethodInfoPtr__SetUniqueIdINTERNAL_b__0_Internal_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr, 100666009);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass105_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass105_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe bool _SetUniqueIdINTERNAL_b__0(CGModule m)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetUniqueIdINTERNAL_b__0_Internal_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass105_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModule+<>c__DisplayClass92_0")]
	public sealed class __c__DisplayClass92_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_source;

		private static readonly System.IntPtr NativeFieldInfoPtr_target;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetLink_b__0_Internal_Boolean_CGModuleLink_0;

		public unsafe CGModuleSlot source
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleSlot>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleSlot));
			}
		}

		public unsafe CGModuleSlot target
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleSlot>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_target)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleSlot));
			}
		}

		static __c__DisplayClass92_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "<>c__DisplayClass92_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr);
			NativeFieldInfoPtr_source = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr, "source");
			NativeFieldInfoPtr_target = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr, "target");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr, 100666010);
			NativeMethodInfoPtr__GetLink_b__0_Internal_Boolean_CGModuleLink_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr, 100666011);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass92_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass92_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48117, XrefRangeEnd = 48127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetLink_b__0(CGModuleLink t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetLink_b__0_Internal_Boolean_CGModuleLink_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass92_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModule+<>c__DisplayClass93_0")]
	public sealed class __c__DisplayClass93_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_source;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetLinks_b__0_Internal_Boolean_CGModuleLink_0;

		public unsafe CGModuleSlot source
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleSlot>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_source)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleSlot));
			}
		}

		static __c__DisplayClass93_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "<>c__DisplayClass93_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr);
			NativeFieldInfoPtr_source = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr, "source");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr, 100666012);
			NativeMethodInfoPtr__GetLinks_b__0_Internal_Boolean_CGModuleLink_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr, 100666013);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass93_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass93_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48127, XrefRangeEnd = 48128, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetLinks_b__0(CGModuleLink t)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)t);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetLinks_b__0_Internal_Boolean_CGModuleLink_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass93_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_GetRequestParameter_Protected_Static_T_byref_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetRequestParameter_Protected_Static_T_byref_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModule>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnBeforeRefresh;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_OnRefresh;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_ModuleName;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Active;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_RandomizeSeed;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_Seed;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_UniqueID;

	private static readonly System.IntPtr NativeFieldInfoPtr_generator;

	private static readonly System.IntPtr NativeFieldInfoPtr_isInitialized;

	private static readonly System.IntPtr NativeFieldInfoPtr_resourceNamer;

	private static readonly System.IntPtr NativeFieldInfoPtr_informationProvider;

	private static readonly System.IntPtr NativeFieldInfoPtr_dirtinessManager;

	private static readonly System.IntPtr NativeFieldInfoPtr_slots;

	private static readonly System.IntPtr NativeFieldInfoPtr_identifier;

	private static readonly System.IntPtr NativeFieldInfoPtr_resourceManagers;

	private static readonly System.IntPtr NativeFieldInfoPtr_UIMessages;

	private static readonly System.IntPtr NativeFieldInfoPtr_Properties;

	private static readonly System.IntPtr NativeFieldInfoPtr_InputLinks;

	private static readonly System.IntPtr NativeFieldInfoPtr_OutputLinks;

	private static readonly System.IntPtr NativeFieldInfoPtr_SortAncestors;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnBeforeRefresh_Public_get_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnBeforeRefresh_Public_set_Void_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvyCGEvent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnBeforeRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ModuleName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_ModuleName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Active_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Active_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Seed_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Seed_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_RandomizeSeed_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_RandomizeSeed_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Dirty_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_New_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_New_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Generator_Public_get_CurvyGenerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UniqueID_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UniqueID_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CircularReferenceError_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CircularReferenceError_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_InputByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_OutputByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Input_Public_get_List_1_CGModuleInputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Output_Public_get_List_1_CGModuleOutputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Virtual_New_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnStateChange_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnTemplateCreated_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetRequestParameter_Protected_Static_T_byref_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveRequestParameter_Protected_Static_Void_byref_Il2CppReferenceArray_1_CGDataRequestParameter_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutputLink_Public_CGModuleLink_CGModuleOutputSlot_CGModuleInputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutputLinks_Public_List_1_CGModuleLink_CGModuleOutputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInputLink_Public_CGModuleLink_CGModuleInputSlot_CGModuleOutputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInputLinks_Public_List_1_CGModuleLink_CGModuleInputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CopyTo_Public_CGModule_CurvyGenerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddManagedResource_Public_Component_String_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteManagedResource_Public_Void_String_Component_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsManagedResource_Public_Boolean_Component_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllPrefabPools_Public_List_1_IPool_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeleteAllPrefabPools_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Delete_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetManagedResources_Public_Boolean_byref_List_1_Component_byref_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetModuleName_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenameResource_Protected_Void_String_Component_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLink_Private_Static_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLinks_Private_Static_List_1_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPrefabPool_Protected_PrefabPool_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryDeleteChildrenFromAssociatedPrefab_Protected_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_doRefresh_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_checkOnStateChangedINTERNAL_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetResourceManagers_Private_List_1_ValueTuple_2_Component_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UsesRandom_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_initializeSort_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_decrementChilds_Internal_List_1_CGModule_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetUniqueIdINTERNAL_Public_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Info_Public_get_ModuleInfoAttribute_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_getInfo_Internal_ModuleInfoAttribute_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_renameManagedResourcesINTERNAL_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetInputSlots_Public_List_1_CGModuleInputSlot_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOutputSlots_Public_List_1_CGModuleOutputSlot_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__Reset_b__67_0_Private_Boolean_CGModule_0;

	public unsafe CurvyCGEvent m_OnBeforeRefresh
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeRefresh);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEvent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_OnBeforeRefresh)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyCGEvent));
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

	public unsafe string m_ModuleName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ModuleName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_ModuleName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool m_Active
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Active);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Active)) = flag;
		}
	}

	public unsafe bool m_RandomizeSeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RandomizeSeed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_RandomizeSeed)) = flag;
		}
	}

	public unsafe int m_Seed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Seed);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Seed)) = num;
		}
	}

	public unsafe int m_UniqueID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UniqueID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_UniqueID)) = num;
		}
	}

	public unsafe CurvyGenerator generator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyGenerator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_generator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)curvyGenerator));
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

	public unsafe ResourceNamer resourceNamer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourceNamer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ResourceNamer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourceNamer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)resourceNamer));
		}
	}

	public unsafe InformationProvider informationProvider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_informationProvider);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InformationProvider>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_informationProvider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)informationProvider));
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

	public unsafe Slots slots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Slots>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slots)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slots));
		}
	}

	public unsafe Identifier identifier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_identifier);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Identifier>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_identifier)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)identifier));
		}
	}

	public unsafe List<Il2CppSystem.ValueTuple<Component, string>> resourceManagers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourceManagers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.ValueTuple<Component, string>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_resourceManagers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<string> UIMessages
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UIMessages);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UIMessages)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe CGModuleProperties Properties
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Properties);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleProperties>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Properties)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleProperties));
		}
	}

	public unsafe List<CGModuleLink> InputLinks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputLinks);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleLink>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InputLinks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<CGModuleLink> OutputLinks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputLinks);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleLink>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputLinks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int SortAncestors
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SortAncestors);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SortAncestors)) = num;
		}
	}

	public unsafe CurvyCGEvent OnBeforeRefresh
	{
		[CallerCount(13)]
		[CachedScanResults(RefRangeStart = 2964, RefRangeEnd = 2977, XrefRangeStart = 2964, XrefRangeEnd = 2977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OnBeforeRefresh_Public_get_CurvyCGEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEvent>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 2978, RefRangeEnd = 2980, XrefRangeStart = 2978, XrefRangeEnd = 2980, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_OnBeforeRefresh_Public_set_Void_CurvyCGEvent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe CurvyCGEvent OnRefresh
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 2980, RefRangeEnd = 2987, XrefRangeStart = 2980, XrefRangeEnd = 2987, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe string ModuleName
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 48135, RefRangeEnd = 48145, XrefRangeStart = 48134, XrefRangeEnd = 48135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ModuleName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48150, RefRangeEnd = 48151, XrefRangeStart = 48145, XrefRangeEnd = 48150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ModuleName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Active
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Active_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48151, XrefRangeEnd = 48154, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Active_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int Seed
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 44533, RefRangeEnd = 44544, XrefRangeStart = 44533, XrefRangeEnd = 44544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Seed_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48154, XrefRangeEnd = 48155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Seed_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool RandomizeSeed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_RandomizeSeed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 46699, RefRangeEnd = 46708, XrefRangeStart = 46699, XrefRangeEnd = 46708, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_RandomizeSeed_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Dirty
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 48155, RefRangeEnd = 48158, XrefRangeStart = 48155, XrefRangeEnd = 48155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(219)]
		[CachedScanResults(RefRangeStart = 48160, RefRangeEnd = 48379, XrefRangeStart = 48158, XrefRangeEnd = 48160, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Dirty_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual bool IsConfigured
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48379, XrefRangeEnd = 48382, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_New_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual bool IsInitialized
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_New_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe CurvyGenerator Generator
	{
		[CallerCount(45)]
		[CachedScanResults(RefRangeStart = 48398, RefRangeEnd = 48443, XrefRangeStart = 48382, XrefRangeEnd = 48398, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Generator_Public_get_CurvyGenerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyGenerator>(intPtr) : null;
		}
	}

	public unsafe int UniqueID
	{
		[CallerCount(22)]
		[CachedScanResults(RefRangeStart = 48445, RefRangeEnd = 48467, XrefRangeStart = 48443, XrefRangeEnd = 48445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UniqueID_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 48471, RefRangeEnd = 48473, XrefRangeStart = 48467, XrefRangeEnd = 48471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UniqueID_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool CircularReferenceError
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48473, XrefRangeEnd = 48476, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CircularReferenceError_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48476, XrefRangeEnd = 48482, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CircularReferenceError_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<string, CGModuleInputSlot> InputByName
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48482, RefRangeEnd = 48483, XrefRangeStart = 48482, XrefRangeEnd = 48482, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InputByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleInputSlot>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<string, CGModuleOutputSlot> OutputByName
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 48483, RefRangeEnd = 48484, XrefRangeStart = 48483, XrefRangeEnd = 48483, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_OutputByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, CGModuleOutputSlot>>(intPtr) : null;
		}
	}

	public unsafe List<CGModuleInputSlot> Input
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 48484, RefRangeEnd = 48487, XrefRangeStart = 48484, XrefRangeEnd = 48484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Input_Public_get_List_1_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleInputSlot>>(intPtr) : null;
		}
	}

	public unsafe List<CGModuleOutputSlot> Output
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 48487, RefRangeEnd = 48489, XrefRangeStart = 48487, XrefRangeEnd = 48487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Output_Public_get_List_1_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleOutputSlot>>(intPtr) : null;
		}
	}

	public unsafe ModuleInfoAttribute Info
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49225, XrefRangeEnd = 49227, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Info_Public_get_ModuleInfoAttribute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleInfoAttribute>(intPtr) : null;
		}
	}

	static CGModule()
	{
		Il2CppClassPointerStore<CGModule>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGModule");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGModule>.NativeClassPtr);
		NativeFieldInfoPtr_m_OnBeforeRefresh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_OnBeforeRefresh");
		NativeFieldInfoPtr_m_OnRefresh = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_OnRefresh");
		NativeFieldInfoPtr_m_ModuleName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_ModuleName");
		NativeFieldInfoPtr_m_Active = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_Active");
		NativeFieldInfoPtr_m_RandomizeSeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_RandomizeSeed");
		NativeFieldInfoPtr_m_Seed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_Seed");
		NativeFieldInfoPtr_m_UniqueID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "m_UniqueID");
		NativeFieldInfoPtr_generator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "generator");
		NativeFieldInfoPtr_isInitialized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "isInitialized");
		NativeFieldInfoPtr_resourceNamer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "resourceNamer");
		NativeFieldInfoPtr_informationProvider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "informationProvider");
		NativeFieldInfoPtr_dirtinessManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "dirtinessManager");
		NativeFieldInfoPtr_slots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "slots");
		NativeFieldInfoPtr_identifier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "identifier");
		NativeFieldInfoPtr_resourceManagers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "resourceManagers");
		NativeFieldInfoPtr_UIMessages = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "UIMessages");
		NativeFieldInfoPtr_Properties = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "Properties");
		NativeFieldInfoPtr_InputLinks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "InputLinks");
		NativeFieldInfoPtr_OutputLinks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "OutputLinks");
		NativeFieldInfoPtr_SortAncestors = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGModule>.NativeClassPtr, "SortAncestors");
		NativeMethodInfoPtr_get_OnBeforeRefresh_Public_get_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665881);
		NativeMethodInfoPtr_set_OnBeforeRefresh_Public_set_Void_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665882);
		NativeMethodInfoPtr_get_OnRefresh_Public_get_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665883);
		NativeMethodInfoPtr_set_OnRefresh_Public_set_Void_CurvyCGEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665884);
		NativeMethodInfoPtr_OnBeforeRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665885);
		NativeMethodInfoPtr_OnRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665886);
		NativeMethodInfoPtr_get_ModuleName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665887);
		NativeMethodInfoPtr_set_ModuleName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665888);
		NativeMethodInfoPtr_get_Active_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665889);
		NativeMethodInfoPtr_set_Active_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665890);
		NativeMethodInfoPtr_get_Seed_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665891);
		NativeMethodInfoPtr_set_Seed_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665892);
		NativeMethodInfoPtr_get_RandomizeSeed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665893);
		NativeMethodInfoPtr_set_RandomizeSeed_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665894);
		NativeMethodInfoPtr_get_Dirty_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665895);
		NativeMethodInfoPtr_set_Dirty_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665896);
		NativeMethodInfoPtr_get_IsConfigured_Public_Virtual_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665897);
		NativeMethodInfoPtr_get_IsInitialized_Public_Virtual_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665898);
		NativeMethodInfoPtr_get_Generator_Public_get_CurvyGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665899);
		NativeMethodInfoPtr_get_UniqueID_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665900);
		NativeMethodInfoPtr_set_UniqueID_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665901);
		NativeMethodInfoPtr_get_CircularReferenceError_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665902);
		NativeMethodInfoPtr_set_CircularReferenceError_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665903);
		NativeMethodInfoPtr_get_InputByName_Public_get_Dictionary_2_String_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665904);
		NativeMethodInfoPtr_get_OutputByName_Public_get_Dictionary_2_String_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665905);
		NativeMethodInfoPtr_get_Input_Public_get_List_1_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665906);
		NativeMethodInfoPtr_get_Output_Public_get_List_1_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665907);
		NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665908);
		NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665909);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665910);
		NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665911);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665912);
		NativeMethodInfoPtr_Reset_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665913);
		NativeMethodInfoPtr_Refresh_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665914);
		NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Virtual_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665915);
		NativeMethodInfoPtr_OnStateChange_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665916);
		NativeMethodInfoPtr_OnTemplateCreated_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665917);
		NativeMethodInfoPtr_GetRequestParameter_Protected_Static_T_byref_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665918);
		NativeMethodInfoPtr_RemoveRequestParameter_Protected_Static_Void_byref_Il2CppReferenceArray_1_CGDataRequestParameter_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665919);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665920);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665921);
		NativeMethodInfoPtr_GetOutputLink_Public_CGModuleLink_CGModuleOutputSlot_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665922);
		NativeMethodInfoPtr_GetOutputLinks_Public_List_1_CGModuleLink_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665923);
		NativeMethodInfoPtr_GetInputLink_Public_CGModuleLink_CGModuleInputSlot_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665924);
		NativeMethodInfoPtr_GetInputLinks_Public_List_1_CGModuleLink_CGModuleInputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665925);
		NativeMethodInfoPtr_CopyTo_Public_CGModule_CurvyGenerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665926);
		NativeMethodInfoPtr_AddManagedResource_Public_Component_String_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665927);
		NativeMethodInfoPtr_DeleteManagedResource_Public_Void_String_Component_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665928);
		NativeMethodInfoPtr_IsManagedResource_Public_Boolean_Component_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665929);
		NativeMethodInfoPtr_GetAllPrefabPools_Public_List_1_IPool_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665930);
		NativeMethodInfoPtr_DeleteAllPrefabPools_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665931);
		NativeMethodInfoPtr_Delete_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665932);
		NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665933);
		NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665934);
		NativeMethodInfoPtr_GetManagedResources_Public_Boolean_byref_List_1_Component_byref_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665935);
		NativeMethodInfoPtr_SetModuleName_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665936);
		NativeMethodInfoPtr_RenameResource_Protected_Void_String_Component_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665937);
		NativeMethodInfoPtr_GetLink_Private_Static_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665938);
		NativeMethodInfoPtr_GetLinks_Private_Static_List_1_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665939);
		NativeMethodInfoPtr_GetPrefabPool_Protected_PrefabPool_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665940);
		NativeMethodInfoPtr_TryDeleteChildrenFromAssociatedPrefab_Protected_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665941);
		NativeMethodInfoPtr_doRefresh_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665942);
		NativeMethodInfoPtr_checkOnStateChangedINTERNAL_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665943);
		NativeMethodInfoPtr_GetResourceManagers_Private_List_1_ValueTuple_2_Component_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665944);
		NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665945);
		NativeMethodInfoPtr_UsesRandom_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665946);
		NativeMethodInfoPtr_initializeSort_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665947);
		NativeMethodInfoPtr_decrementChilds_Internal_List_1_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665948);
		NativeMethodInfoPtr_SetUniqueIdINTERNAL_Public_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665949);
		NativeMethodInfoPtr_get_Info_Public_get_ModuleInfoAttribute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665950);
		NativeMethodInfoPtr_getInfo_Internal_ModuleInfoAttribute_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665951);
		NativeMethodInfoPtr_renameManagedResourcesINTERNAL_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665952);
		NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665953);
		NativeMethodInfoPtr_GetInputSlots_Public_List_1_CGModuleInputSlot_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665954);
		NativeMethodInfoPtr_GetOutputSlots_Public_List_1_CGModuleOutputSlot_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665955);
		NativeMethodInfoPtr__Reset_b__67_0_Private_Boolean_CGModule_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModule>.NativeClassPtr, 100665956);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48128, XrefRangeEnd = 48131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CurvyCGEventArgs OnBeforeRefreshEvent(CurvyCGEventArgs e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnBeforeRefreshEvent_Protected_CurvyCGEventArgs_CurvyCGEventArgs_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CurvyCGEventArgs>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48131, XrefRangeEnd = 48134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 48505, RefRangeEnd = 48517, XrefRangeStart = 48489, XrefRangeEnd = 48505, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48544, RefRangeEnd = 48546, XrefRangeStart = 48517, XrefRangeEnd = 48544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48551, RefRangeEnd = 48553, XrefRangeStart = 48546, XrefRangeEnd = 48551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnValidate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnValidate_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 48597, RefRangeEnd = 48621, XrefRangeStart = 48553, XrefRangeEnd = 48597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe virtual void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 48624, RefRangeEnd = 48639, XrefRangeStart = 48621, XrefRangeEnd = 48624, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Refresh_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(170)]
	[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool DeleteAllOutputManagedResources()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_DeleteAllOutputManagedResources_Public_Virtual_New_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48641, RefRangeEnd = 48642, XrefRangeStart = 48639, XrefRangeEnd = 48641, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnStateChange()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnStateChange_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 48642, RefRangeEnd = 48646, XrefRangeStart = 48642, XrefRangeEnd = 48642, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OnTemplateCreated()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnTemplateCreated_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 48653, RefRangeEnd = 48661, XrefRangeStart = 48646, XrefRangeEnd = 48653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T GetRequestParameter<T>(ref Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGDataRequestParameter
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		*ptr = (nint)(&intPtr);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetRequestParameter_Protected_Static_T_byref_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		requests = ((intPtr4 == (System.IntPtr)0) ? null : new Il2CppReferenceArray<CGDataRequestParameter>(intPtr4));
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48661, XrefRangeEnd = 48666, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RemoveRequestParameter(ref Il2CppReferenceArray<CGDataRequestParameter> requests, CGDataRequestParameter request)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		*ptr = (nint)(&intPtr);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)request);
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveRequestParameter_Protected_Static_Void_byref_Il2CppReferenceArray_1_CGDataRequestParameter_CGDataRequestParameter_0, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		System.IntPtr intPtr4 = intPtr;
		requests = ((intPtr4 == (System.IntPtr)0) ? null : new Il2CppReferenceArray<CGDataRequestParameter>(intPtr4));
	}

	[CallerCount(23)]
	[CachedScanResults(RefRangeStart = 48751, RefRangeEnd = 48774, XrefRangeStart = 48666, XrefRangeEnd = 48751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGModule>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48784, RefRangeEnd = 48786, XrefRangeStart = 48774, XrefRangeEnd = 48784, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 48787, RefRangeEnd = 48790, XrefRangeStart = 48786, XrefRangeEnd = 48787, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleLink GetOutputLink(CGModuleOutputSlot outputSlot, CGModuleInputSlot inputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutputLink_Public_CGModuleLink_CGModuleOutputSlot_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleLink>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48791, RefRangeEnd = 48792, XrefRangeStart = 48790, XrefRangeEnd = 48791, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModuleLink> GetOutputLinks(CGModuleOutputSlot outputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutputLinks_Public_List_1_CGModuleLink_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleLink>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 48793, RefRangeEnd = 48796, XrefRangeStart = 48792, XrefRangeEnd = 48793, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleLink GetInputLink(CGModuleInputSlot inputSlot, CGModuleOutputSlot outputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSlot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInputLink_Public_CGModuleLink_CGModuleInputSlot_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleLink>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48797, RefRangeEnd = 48798, XrefRangeStart = 48796, XrefRangeEnd = 48797, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModuleLink> GetInputLinks(CGModuleInputSlot inputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInputLinks_Public_List_1_CGModuleLink_CGModuleInputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleLink>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48798, XrefRangeEnd = 48809, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule CopyTo(CurvyGenerator targetGenerator)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)targetGenerator);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTo_Public_CGModule_CurvyGenerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 48821, RefRangeEnd = 48824, XrefRangeStart = 48809, XrefRangeEnd = 48821, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Component AddManagedResource(string resourceName, string context = "", int index = -1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddManagedResource_Public_Component_String_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Component>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 48832, RefRangeEnd = 48836, XrefRangeStart = 48824, XrefRangeEnd = 48832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeleteManagedResource(string resourceName, Component res, string context = "", bool dontUsePool = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)res);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &dontUsePool;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteManagedResource_Public_Void_String_Component_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48840, RefRangeEnd = 48841, XrefRangeStart = 48836, XrefRangeEnd = 48840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsManagedResource(Component res)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)res);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsManagedResource_Public_Boolean_Component_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48849, RefRangeEnd = 48850, XrefRangeStart = 48841, XrefRangeEnd = 48849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<IPool> GetAllPrefabPools()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllPrefabPools_Public_List_1_IPool_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<IPool>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48858, RefRangeEnd = 48859, XrefRangeStart = 48850, XrefRangeEnd = 48858, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DeleteAllPrefabPools()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeleteAllPrefabPools_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 48861, RefRangeEnd = 48862, XrefRangeStart = 48859, XrefRangeEnd = 48861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Delete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Delete_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48862, XrefRangeEnd = 48864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleInputSlot GetInputSlot(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInputSlot_Public_CGModuleInputSlot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48864, XrefRangeEnd = 48866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleOutputSlot GetOutputSlot(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutputSlot_Public_CGModuleOutputSlot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48933, RefRangeEnd = 48935, XrefRangeStart = 48866, XrefRangeEnd = 48933, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetManagedResources(out List<Component> components, out List<string> resourceNames)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		nint num = 0;
		*ptr = (nint)(&num);
		byte* num2 = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num3 = 0;
		*(nint**)num2 = &num3;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetManagedResources_Public_Boolean_byref_List_1_Component_byref_List_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num4 = num;
		components = ((num4 == 0) ? null : new List<Component>(num4));
		nint num5 = num3;
		resourceNames = ((num5 == 0) ? null : new List<string>(num5));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48955, RefRangeEnd = 48957, XrefRangeStart = 48935, XrefRangeEnd = 48955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetModuleName()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetModuleName_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 48957, XrefRangeEnd = 48959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RenameResource(string resourceName, Component resource, int index = -1)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(resourceName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)resource);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenameResource_Protected_Void_String_Component_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48975, RefRangeEnd = 48977, XrefRangeStart = 48959, XrefRangeEnd = 48975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static CGModuleLink GetLink(List<CGModuleLink> lst, CGModuleSlot source, CGModuleSlot target)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lst);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLink_Private_Static_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_CGModuleSlot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleLink>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 48995, RefRangeEnd = 48997, XrefRangeStart = 48977, XrefRangeEnd = 48995, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<CGModuleLink> GetLinks(List<CGModuleLink> lst, CGModuleSlot source)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)lst);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLinks_Private_Static_List_1_CGModuleLink_List_1_CGModuleLink_CGModuleSlot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleLink>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 49011, RefRangeEnd = 49012, XrefRangeStart = 48997, XrefRangeEnd = 49011, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PrefabPool GetPrefabPool(GameObject prefab)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)prefab);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPrefabPool_Protected_PrefabPool_GameObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PrefabPool>(intPtr) : null;
	}

	[CallerCount(170)]
	[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryDeleteChildrenFromAssociatedPrefab()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryDeleteChildrenFromAssociatedPrefab_Protected_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 49038, RefRangeEnd = 49039, XrefRangeStart = 49012, XrefRangeEnd = 49038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void doRefresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_doRefresh_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 49041, RefRangeEnd = 49042, XrefRangeStart = 49039, XrefRangeEnd = 49041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void checkOnStateChangedINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_checkOnStateChangedINTERNAL_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49042, XrefRangeEnd = 49062, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<Il2CppSystem.ValueTuple<Component, string>> GetResourceManagers()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetResourceManagers_Private_List_1_ValueTuple_2_Component_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.ValueTuple<Component, string>>>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 49069, RefRangeEnd = 49074, XrefRangeStart = 49062, XrefRangeEnd = 49069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ResetOnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ResetOnEnable_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49074, XrefRangeEnd = 49075, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool UsesRandom()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UsesRandom_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49075, XrefRangeEnd = 49110, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void initializeSort()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_initializeSort_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49110, XrefRangeEnd = 49205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModule> decrementChilds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_decrementChilds_Internal_List_1_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModule>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49205, XrefRangeEnd = 49225, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int SetUniqueIdINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetUniqueIdINTERNAL_Public_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49227, XrefRangeEnd = 49237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ModuleInfoAttribute getInfo()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_getInfo_Internal_ModuleInfoAttribute_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ModuleInfoAttribute>(intPtr) : null;
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 49262, RefRangeEnd = 49270, XrefRangeStart = 49237, XrefRangeEnd = 49262, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void renameManagedResourcesINTERNAL()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_renameManagedResourcesINTERNAL_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 49272, RefRangeEnd = 49274, XrefRangeStart = 49270, XrefRangeEnd = 49272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReInitializeLinkedSlots()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReInitializeLinkedSlots_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49274, XrefRangeEnd = 49301, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModuleInputSlot> GetInputSlots(Il2CppSystem.Type filterType = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)filterType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetInputSlots_Public_List_1_CGModuleInputSlot_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleInputSlot>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49301, XrefRangeEnd = 49327, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<CGModuleOutputSlot> GetOutputSlots(Il2CppSystem.Type filterType = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)filterType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOutputSlots_Public_List_1_CGModuleOutputSlot_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CGModuleOutputSlot>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49327, XrefRangeEnd = 49331, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool _Reset_b__67_0(CGModule m)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)m);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Reset_b__67_0_Private_Boolean_CGModule_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CGModule(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
