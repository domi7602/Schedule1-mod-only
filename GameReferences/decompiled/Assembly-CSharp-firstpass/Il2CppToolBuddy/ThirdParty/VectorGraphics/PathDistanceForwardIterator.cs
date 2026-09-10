using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class PathDistanceForwardIterator : Il2CppSystem.Object
{
	public class BezierLoop : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_OpenPath;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_Virtual_Final_New_get_BezierPathSegment_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_Item_Public_Virtual_Final_New_set_Void_Int32_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_Virtual_Final_New_get_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_IsReadOnly_Public_Virtual_Final_New_get_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Add_Public_Virtual_Final_New_Void_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Contains_Public_Virtual_Final_New_Boolean_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_CopyTo_Public_Virtual_Final_New_Void_Il2CppStructArray_1_BezierPathSegment_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_IndexOf_Public_Virtual_Final_New_Int32_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Insert_Public_Virtual_Final_New_Void_Int32_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Boolean_BezierPathSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_RemoveAt_Public_Virtual_Final_New_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

		public unsafe IList<BezierPathSegment> OpenPath
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OpenPath);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IList<BezierPathSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OpenPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe virtual BezierPathSegment this[int index]
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29240, XrefRangeEnd = 29247, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&index);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_Virtual_Final_New_get_BezierPathSegment_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(BezierPathSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29247, XrefRangeEnd = 29252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[2];
				*ptr = (nint)(&index);
				*(BezierPathSegment**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &value;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Item_Public_Virtual_Final_New_set_Void_Int32_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe virtual int Count
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29252, XrefRangeEnd = 29255, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Count_Public_Virtual_Final_New_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe virtual bool IsReadOnly
		{
			[CallerCount(18)]
			[CachedScanResults(RefRangeStart = 29255, RefRangeEnd = 29273, XrefRangeStart = 29255, XrefRangeEnd = 29255, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsReadOnly_Public_Virtual_Final_New_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static BezierLoop()
		{
			Il2CppClassPointerStore<BezierLoop>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "BezierLoop");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr);
			NativeFieldInfoPtr_OpenPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, "OpenPath");
			NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663769);
			NativeMethodInfoPtr_get_Item_Public_Virtual_Final_New_get_BezierPathSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663770);
			NativeMethodInfoPtr_set_Item_Public_Virtual_Final_New_set_Void_Int32_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663771);
			NativeMethodInfoPtr_get_Count_Public_Virtual_Final_New_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663772);
			NativeMethodInfoPtr_get_IsReadOnly_Public_Virtual_Final_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663773);
			NativeMethodInfoPtr_Add_Public_Virtual_Final_New_Void_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663774);
			NativeMethodInfoPtr_Clear_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663775);
			NativeMethodInfoPtr_Contains_Public_Virtual_Final_New_Boolean_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663776);
			NativeMethodInfoPtr_CopyTo_Public_Virtual_Final_New_Void_Il2CppStructArray_1_BezierPathSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663777);
			NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663778);
			NativeMethodInfoPtr_IndexOf_Public_Virtual_Final_New_Int32_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663779);
			NativeMethodInfoPtr_Insert_Public_Virtual_Final_New_Void_Int32_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663780);
			NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Boolean_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663781);
			NativeMethodInfoPtr_RemoveAt_Public_Virtual_Final_New_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663782);
			NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr, 100663783);
		}

		[CallerCount(203)]
		[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19776, XrefRangeEnd = 19979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe BezierLoop(IList<BezierPathSegment> openPath)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BezierLoop>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)openPath);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29273, XrefRangeEnd = 29278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Add(BezierPathSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&item);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Add_Public_Virtual_Final_New_Void_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Clear()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clear_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29278, XrefRangeEnd = 29283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool Contains(BezierPathSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&item);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Contains_Public_Virtual_Final_New_Boolean_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29283, XrefRangeEnd = 29288, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void CopyTo(Il2CppStructArray<BezierPathSegment> array, int arrayIndex)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
			*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &arrayIndex;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CopyTo_Public_Virtual_Final_New_Void_Il2CppStructArray_1_BezierPathSegment_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29288, XrefRangeEnd = 29293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator<BezierPathSegment> GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<BezierPathSegment>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29293, XrefRangeEnd = 29298, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual int IndexOf(BezierPathSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&item);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IndexOf_Public_Virtual_Final_New_Int32_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29298, XrefRangeEnd = 29303, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Insert(int index, BezierPathSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&index);
			*(BezierPathSegment**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &item;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Insert_Public_Virtual_Final_New_Void_Int32_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29303, XrefRangeEnd = 29308, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool Remove(BezierPathSegment item)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&item);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Virtual_Final_New_Boolean_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29308, XrefRangeEnd = 29313, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void RemoveAt(int index)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&index);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveAt_Public_Virtual_Final_New_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29313, XrefRangeEnd = 29318, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
		}

		public BezierLoop(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "Result")]
	public enum Result
	{
		Stepped,
		NewSegment,
		Ended
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Segments_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_closed;

	private static readonly System.IntPtr NativeFieldInfoPtr_needTangentsDuringEval;

	private static readonly System.IntPtr NativeFieldInfoPtr_maxCordDeviationSq;

	private static readonly System.IntPtr NativeFieldInfoPtr_maxTanAngleDevCosine;

	private static readonly System.IntPtr NativeFieldInfoPtr_stepSizeT;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentSegment;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentT;

	private static readonly System.IntPtr NativeFieldInfoPtr_segmentLengthSoFar;

	private static readonly System.IntPtr NativeFieldInfoPtr_lengthSoFar;

	private static readonly System.IntPtr NativeFieldInfoPtr_lastPointEval;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentTTangent;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentBezSeg;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_Boolean_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointToLineDistanceSq_Private_Single_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdvanceBy_Public_Result_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Segments_Public_get_IList_1_BezierPathSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentSegment_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentT_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LengthSoFar_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SegmentLengthSoFar_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Ended_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalCurrent_Public_Vector2_0;

	public unsafe IList<BezierPathSegment> _Segments_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Segments_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IList<BezierPathSegment>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Segments_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool closed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed)) = flag;
		}
	}

	public unsafe bool needTangentsDuringEval
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_needTangentsDuringEval);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_needTangentsDuringEval)) = flag;
		}
	}

	public unsafe float maxCordDeviationSq
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxCordDeviationSq);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxCordDeviationSq)) = num;
		}
	}

	public unsafe float maxTanAngleDevCosine
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxTanAngleDevCosine);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maxTanAngleDevCosine)) = num;
		}
	}

	public unsafe float stepSizeT
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stepSizeT);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stepSizeT)) = num;
		}
	}

	public unsafe int currentSegment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSegment);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSegment)) = num;
		}
	}

	public unsafe float currentT
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentT);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentT)) = num;
		}
	}

	public unsafe float segmentLengthSoFar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentLengthSoFar);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segmentLengthSoFar)) = num;
		}
	}

	public unsafe float lengthSoFar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengthSoFar);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengthSoFar)) = num;
		}
	}

	public unsafe Vector2 lastPointEval
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPointEval);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lastPointEval)) = vector;
		}
	}

	public unsafe Vector2 currentTTangent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentTTangent);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentTTangent)) = vector;
		}
	}

	public unsafe BezierSegment currentBezSeg
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentBezSeg);
			return *(BezierSegment*)num;
		}
		set
		{
			*(BezierSegment*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentBezSeg)) = bezierSegment;
		}
	}

	public unsafe IList<BezierPathSegment> Segments
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 3712, RefRangeEnd = 3724, XrefRangeStart = 3712, XrefRangeEnd = 3724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Segments_Public_get_IList_1_BezierPathSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IList<BezierPathSegment>>(intPtr) : null;
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
	}

	public unsafe int CurrentSegment
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 29072, RefRangeEnd = 29101, XrefRangeStart = 29072, XrefRangeEnd = 29101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentSegment_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float CurrentT
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentT_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float LengthSoFar
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LengthSoFar_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float SegmentLengthSoFar
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29130, RefRangeEnd = 29131, XrefRangeStart = 29130, XrefRangeEnd = 29131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SegmentLengthSoFar_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool Ended
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29397, RefRangeEnd = 29399, XrefRangeStart = 29394, XrefRangeEnd = 29397, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Ended_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PathDistanceForwardIterator()
	{
		Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "PathDistanceForwardIterator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr);
		NativeFieldInfoPtr__Segments_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "<Segments>k__BackingField");
		NativeFieldInfoPtr_closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "closed");
		NativeFieldInfoPtr_needTangentsDuringEval = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "needTangentsDuringEval");
		NativeFieldInfoPtr_maxCordDeviationSq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "maxCordDeviationSq");
		NativeFieldInfoPtr_maxTanAngleDevCosine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "maxTanAngleDevCosine");
		NativeFieldInfoPtr_stepSizeT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "stepSizeT");
		NativeFieldInfoPtr_currentSegment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "currentSegment");
		NativeFieldInfoPtr_currentT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "currentT");
		NativeFieldInfoPtr_segmentLengthSoFar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "segmentLengthSoFar");
		NativeFieldInfoPtr_lengthSoFar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "lengthSoFar");
		NativeFieldInfoPtr_lastPointEval = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "lastPointEval");
		NativeFieldInfoPtr_currentTTangent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "currentTTangent");
		NativeFieldInfoPtr_currentBezSeg = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, "currentBezSeg");
		NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_Boolean_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663758);
		NativeMethodInfoPtr_PointToLineDistanceSq_Private_Single_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663759);
		NativeMethodInfoPtr_AdvanceBy_Public_Result_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663760);
		NativeMethodInfoPtr_get_Segments_Public_get_IList_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663761);
		NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663762);
		NativeMethodInfoPtr_get_CurrentSegment_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663763);
		NativeMethodInfoPtr_get_CurrentT_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663764);
		NativeMethodInfoPtr_get_LengthSoFar_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663765);
		NativeMethodInfoPtr_get_SegmentLengthSoFar_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663766);
		NativeMethodInfoPtr_get_Ended_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663767);
		NativeMethodInfoPtr_EvalCurrent_Public_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr, 100663768);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 29350, RefRangeEnd = 29352, XrefRangeStart = 29318, XrefRangeEnd = 29350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PathDistanceForwardIterator(IList<BezierPathSegment> pathSegments, bool closed, float maxCordDeviationSq, float maxTanAngleDevCosine, float stepSizeT)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathDistanceForwardIterator>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathSegments);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxCordDeviationSq;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxTanAngleDevCosine;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &stepSizeT;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_IList_1_BezierPathSegment_Boolean_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29352, XrefRangeEnd = 29357, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float PointToLineDistanceSq(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&point);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineStart;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineEnd;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointToLineDistanceSq_Private_Single_Vector2_Vector2_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 29391, RefRangeEnd = 29394, XrefRangeStart = 29357, XrefRangeEnd = 29391, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Result AdvanceBy(float units, out float unitsRemaining)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&units);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref unitsRemaining);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdvanceBy_Public_Result_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Result*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 29403, RefRangeEnd = 29407, XrefRangeStart = 29399, XrefRangeEnd = 29403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 EvalCurrent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvalCurrent_Public_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public PathDistanceForwardIterator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
