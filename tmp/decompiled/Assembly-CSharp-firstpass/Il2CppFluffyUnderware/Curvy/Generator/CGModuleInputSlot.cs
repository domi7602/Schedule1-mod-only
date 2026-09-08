using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.Curvy.Generator;

[System.Serializable]
public class CGModuleInputSlot : CGModuleSlot
{
	[System.Serializable]
	[ObfuscatedName("FluffyUnderware.Curvy.Generator.CGModuleInputSlot+<>c__13`1")]
	public sealed class __c__13<T> : Il2CppSystem.Object where T : CGData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__13_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__13_1;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetData_b__13_0_Internal_Boolean_CGData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetData_b__13_1_Internal_Boolean_CGData_0;

		public unsafe static __c__13<T> __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c__13<T>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c__));
			}
		}

		public unsafe static Il2CppSystem.Func<CGData, bool> __9__13_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__13_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGData, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__13_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<CGData, bool> __9__13_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__13_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<CGData, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__13_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c__13()
		{
			Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, "<>c__13`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__13_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, "<>9__13_0");
			NativeFieldInfoPtr___9__13_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, "<>9__13_1");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, 100666030);
			NativeMethodInfoPtr__GetData_b__13_0_Internal_Boolean_CGData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, 100666031);
			NativeMethodInfoPtr__GetData_b__13_1_Internal_Boolean_CGData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr, 100666032);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__13()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__13<T>>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe bool _GetData_b__13_0(CGData d)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)d);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetData_b__13_0_Internal_Boolean_CGData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe bool _GetData_b__13_1(CGData d)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)d);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetData_b__13_1_Internal_Boolean_CGData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__13(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_GetData_Public_T_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetData_Public_T_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetData_Public_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetData_Public_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetAllData_Public_List_1_T_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetAllData_Public_List_1_T_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetAllData_Public_List_1_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetAllData_Public_List_1_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_GetData_Private_Il2CppReferenceArray_1_CGData_Int32_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetData_Private_Il2CppReferenceArray_1_CGData_Int32_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_CloneData_Private_Static_Il2CppReferenceArray_1_CGData_Il2CppReferenceArray_1_CGData_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_CloneData_Private_Static_Il2CppReferenceArray_1_CGData_Il2CppReferenceArray_1_CGData_0, Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_get_InputInfo_Public_get_InputSlotInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LinkTo_Public_Virtual_Void_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_Void_CGModuleSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SourceSlot_Public_CGModuleOutputSlot_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CanLinkTo_Public_Boolean_CGModuleOutputSlot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AreInputAndOutputSlotsCompatible_Public_Static_Boolean_InputSlotInfo_Boolean_OutputSlotInfo_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SourceModule_Private_CGModule_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetData_Public_T_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetData_Public_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllData_Public_List_1_T_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllData_Public_List_1_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetData_Private_Il2CppReferenceArray_1_CGData_Int32_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneData_Private_Static_Il2CppReferenceArray_1_CGData_Il2CppReferenceArray_1_CGData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe InputSlotInfo InputInfo
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 49333, RefRangeEnd = 49334, XrefRangeStart = 49331, XrefRangeEnd = 49333, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InputInfo_Public_get_InputSlotInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InputSlotInfo>(intPtr) : null;
		}
	}

	static CGModuleInputSlot()
	{
		Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGModuleInputSlot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr);
		NativeMethodInfoPtr_get_InputInfo_Public_get_InputSlotInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666014);
		NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666015);
		NativeMethodInfoPtr_LinkTo_Public_Virtual_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666016);
		NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_Void_CGModuleSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666017);
		NativeMethodInfoPtr_SourceSlot_Public_CGModuleOutputSlot_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666018);
		NativeMethodInfoPtr_CanLinkTo_Public_Boolean_CGModuleOutputSlot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666019);
		NativeMethodInfoPtr_AreInputAndOutputSlotsCompatible_Public_Static_Boolean_InputSlotInfo_Boolean_OutputSlotInfo_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666020);
		NativeMethodInfoPtr_SourceModule_Private_CGModule_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666021);
		NativeMethodInfoPtr_GetData_Public_T_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666022);
		NativeMethodInfoPtr_GetData_Public_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666023);
		NativeMethodInfoPtr_GetAllData_Public_List_1_T_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666024);
		NativeMethodInfoPtr_GetAllData_Public_List_1_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666025);
		NativeMethodInfoPtr_GetData_Private_Il2CppReferenceArray_1_CGData_Int32_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666026);
		NativeMethodInfoPtr_CloneData_Private_Static_Il2CppReferenceArray_1_CGData_Il2CppReferenceArray_1_CGData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666027);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr, 100666028);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49334, XrefRangeEnd = 49386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void LoadLinkedSlots()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LoadLinkedSlots_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49386, XrefRangeEnd = 49389, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void LinkTo(CGModuleSlot outputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LinkTo_Public_Virtual_Void_CGModuleSlot_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49389, XrefRangeEnd = 49401, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void UnlinkFrom(CGModuleSlot outputSlot)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlot);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UnlinkFrom_Public_Virtual_Void_CGModuleSlot_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 49410, RefRangeEnd = 49421, XrefRangeStart = 49401, XrefRangeEnd = 49410, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleOutputSlot SourceSlot(int index = 0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SourceSlot_Public_CGModuleOutputSlot_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49421, XrefRangeEnd = 49441, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CanLinkTo(CGModuleOutputSlot source)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CanLinkTo_Public_Boolean_CGModuleOutputSlot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49441, XrefRangeEnd = 49446, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool AreInputAndOutputSlotsCompatible(InputSlotInfo inputSlotInfo, bool inputSlotModuleIsOnRequest, OutputSlotInfo outputSlotInfo, bool outputSlotModuleIsOnRequest)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputSlotInfo);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &inputSlotModuleIsOnRequest;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outputSlotInfo);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &outputSlotModuleIsOnRequest;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AreInputAndOutputSlotsCompatible_Public_Static_Boolean_InputSlotInfo_Boolean_OutputSlotInfo_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49446, XrefRangeEnd = 49452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModule SourceModule(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SourceModule_Private_CGModule_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CGModule>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49452, XrefRangeEnd = 49460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetData<T>([Optional] Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGData
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetData_Public_T_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 49467, RefRangeEnd = 49483, XrefRangeStart = 49460, XrefRangeEnd = 49467, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetData<T>(out bool isDataDisposable, [Optional] Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGData
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref isDataDisposable);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetData_Public_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49483, XrefRangeEnd = 49485, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<T> GetAllData<T>([Optional] Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGData
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetAllData_Public_List_1_T_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 49503, RefRangeEnd = 49513, XrefRangeStart = 49485, XrefRangeEnd = 49503, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<T> GetAllData<T>(out bool isDataDisposable, [Optional] Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGData
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref isDataDisposable);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetAllData_Public_List_1_T_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<T>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 49599, RefRangeEnd = 49602, XrefRangeStart = 49513, XrefRangeEnd = 49599, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppReferenceArray<CGData> GetData<T>(int slotIndex, out bool isDataDisposable, [Optional] Il2CppReferenceArray<CGDataRequestParameter> requests) where T : CGData
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&slotIndex);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isDataDisposable);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetData_Private_Il2CppReferenceArray_1_CGData_Int32_byref_Boolean_Il2CppReferenceArray_1_CGDataRequestParameter_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CGData>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 49602, XrefRangeEnd = 49609, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppReferenceArray<CGData> CloneData<T>(Il2CppReferenceArray<CGData> source) where T : CGData
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_CloneData_Private_Static_Il2CppReferenceArray_1_CGData_Il2CppReferenceArray_1_CGData_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CGData>>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGModuleInputSlot()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGModuleInputSlot>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public T GetData<T>(params CGDataRequestParameter[] requests) where T : CGData
	{
		return (T)(object)this.GetData<T>(new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public T GetData<T>(out bool isDataDisposable, params CGDataRequestParameter[] requests) where T : CGData
	{
		return (T)(object)this.GetData<T>(out isDataDisposable, new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public List<T> GetAllData<T>(params CGDataRequestParameter[] requests) where T : CGData
	{
		return (List<T>)(object)this.GetAllData<T>(new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public List<T> GetAllData<T>(out bool isDataDisposable, params CGDataRequestParameter[] requests) where T : CGData
	{
		return (List<T>)(object)this.GetAllData<T>(out isDataDisposable, new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public Il2CppReferenceArray<CGData> GetData<T>(int slotIndex, out bool isDataDisposable, params CGDataRequestParameter[] requests) where T : CGData
	{
		return this.GetData<T>(slotIndex, out isDataDisposable, new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public CGModuleInputSlot(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
