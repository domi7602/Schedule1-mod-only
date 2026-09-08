using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Unity.Collections;
using UnityEngine;

namespace Il2Cpp;

public static class BurstFunctions : Il2CppSystem.Object
{
	[ObfuscatedName("BurstFunctions+Average_00000093$PostfixBurstDelegate")]
	public sealed class MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0 : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Single_byref_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Single_byref_Single_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0()
		{
			Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, "Average_00000093$PostfixBurstDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr, 100663447);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr, 100663448);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Single_byref_Single_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr, 100663449);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr, 100663450);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66451, XrefRangeEnd = 66455, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0(Il2CppSystem.Object A_1, System.IntPtr A_2)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_1);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &A_2;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual void Invoke(ref NativeArray<float> arr, out float result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66455, XrefRangeEnd = 66462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(ref NativeArray<float> arr, out float result, Il2CppSystem.AsyncCallback A_3, Il2CppSystem.Object A_4)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_3);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_4);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Single_byref_Single_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult A_1)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_1);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("BurstFunctions+Average_00000093$BurstDirectCall")]
	public static class ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Pointer;

		private static readonly System.IntPtr NativeFieldInfoPtr_DeferredCompilation;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Constructor_Public_Static_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Static_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0;

		public unsafe static System.IntPtr Pointer
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Pointer, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Pointer, (void*)(&intPtr));
			}
		}

		public unsafe static System.IntPtr DeferredCompilation
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DeferredCompilation, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DeferredCompilation, (void*)(&intPtr));
			}
		}

		static ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0()
		{
			Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, "Average_00000093$BurstDirectCall");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr);
			NativeFieldInfoPtr_Pointer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, "Pointer");
			NativeFieldInfoPtr_DeferredCompilation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, "DeferredCompilation");
			NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, 100663451);
			NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, 100663452);
			NativeMethodInfoPtr_Constructor_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, 100663453);
			NativeMethodInfoPtr_Initialize_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, 100663454);
			NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0>.NativeClassPtr, 100663456);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66462, XrefRangeEnd = 66480, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void GetFunctionPointerDiscard(ref System.IntPtr A_0)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)Unsafe.AsPointer(ref A_0);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66480, XrefRangeEnd = 66502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static System.IntPtr GetFunctionPointer()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(System.IntPtr*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66502, XrefRangeEnd = 66512, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Constructor()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Constructor_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Initialize()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 66541, RefRangeEnd = 66542, XrefRangeStart = 66512, XrefRangeEnd = 66541, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Invoke(ref NativeArray<float> arr, out float result)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("BurstFunctions+Average_00000094$PostfixBurstDelegate")]
	public sealed class MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1 : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Vector3_byref_Vector3_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Vector3_byref_Vector3_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1()
		{
			Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, "Average_00000094$PostfixBurstDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr, 100663457);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr, 100663458);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Vector3_byref_Vector3_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr, 100663459);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr, 100663460);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1(Il2CppSystem.Object A_1, System.IntPtr A_2)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_1);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &A_2;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual void Invoke(ref NativeArray<Vector3> arr, out Vector3 result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_byref_NativeArray_1_Vector3_byref_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66542, XrefRangeEnd = 66549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(ref NativeArray<Vector3> arr, out Vector3 result, Il2CppSystem.AsyncCallback A_3, Il2CppSystem.Object A_4)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_3);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_4);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_NativeArray_1_Vector3_byref_Vector3_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult A_1)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)A_1);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MulticastDelegateNPublicSealedVoObA_ObVoInA_arInre1(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("BurstFunctions+Average_00000094$BurstDirectCall")]
	public static class ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Pointer;

		private static readonly System.IntPtr NativeFieldInfoPtr_DeferredCompilation;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Constructor_Public_Static_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Static_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0;

		public unsafe static System.IntPtr Pointer
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Pointer, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Pointer, (void*)(&intPtr));
			}
		}

		public unsafe static System.IntPtr DeferredCompilation
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr result);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DeferredCompilation, (void*)(&result));
				return result;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DeferredCompilation, (void*)(&intPtr));
			}
		}

		static ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1()
		{
			Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, "Average_00000094$BurstDirectCall");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr);
			NativeFieldInfoPtr_Pointer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, "Pointer");
			NativeFieldInfoPtr_DeferredCompilation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, "DeferredCompilation");
			NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, 100663461);
			NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, 100663462);
			NativeMethodInfoPtr_Constructor_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, 100663463);
			NativeMethodInfoPtr_Initialize_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, 100663464);
			NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1>.NativeClassPtr, 100663466);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66549, XrefRangeEnd = 66567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void GetFunctionPointerDiscard(ref System.IntPtr A_0)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)Unsafe.AsPointer(ref A_0);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFunctionPointerDiscard_Private_Static_Void_byref_IntPtr_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66567, XrefRangeEnd = 66589, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static System.IntPtr GetFunctionPointer()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFunctionPointer_Private_Static_IntPtr_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(System.IntPtr*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66589, XrefRangeEnd = 66599, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Constructor()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Constructor_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Initialize()
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 66629, RefRangeEnd = 66630, XrefRangeStart = 66599, XrefRangeEnd = 66629, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static void Invoke(ref NativeArray<Vector3> arr, out Vector3 result)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ObjectNInternalAbstractSealedInPoDeInGeVoA_ObCoGe1(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_PDM_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_PDM_0;

	static BurstFunctions()
	{
		Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "BurstFunctions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr);
		NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, 100663443);
		NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, 100663444);
		NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, 100663445);
		NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_PDM_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BurstFunctions>.NativeClassPtr, 100663446);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66630, XrefRangeEnd = 66634, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Average(ref NativeArray<float> arr, out float result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 66638, RefRangeEnd = 66640, XrefRangeStart = 66634, XrefRangeEnd = 66638, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Average(ref NativeArray<Vector3> arr, out Vector3 result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Average_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66640, XrefRangeEnd = 66641, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_PDM_0(ref NativeArray<float> arr, out float result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Single_byref_Single_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 66641, XrefRangeEnd = 66643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Method_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_PDM_0(ref NativeArray<Vector3> arr, out Vector3 result)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)arr));
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Public_Static_Void_byref_NativeArray_1_Vector3_byref_Vector3_PDM_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BurstFunctions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
