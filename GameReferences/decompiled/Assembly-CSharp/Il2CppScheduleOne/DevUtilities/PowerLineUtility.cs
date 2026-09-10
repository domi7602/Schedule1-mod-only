using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppScheduleOne.DevUtilities;

public class PowerLineUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MinSegmentCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxSegmentCount;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSegmentCount_Public_Static_Int32_Vector3_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawPowerLine_Public_Static_Void_Vector3_Vector3_List_1_Transform_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PositionSegments_Private_Static_Void_List_1_Vector3_List_1_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCatenaryPoints_Private_Static_List_1_Vector3_Vector3_Vector3_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static int MinSegmentCount
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinSegmentCount, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinSegmentCount, (void*)(&num));
		}
	}

	public unsafe static int MaxSegmentCount
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxSegmentCount, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxSegmentCount, (void*)(&num));
		}
	}

	static PowerLineUtility()
	{
		Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "PowerLineUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr);
		NativeFieldInfoPtr_MinSegmentCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, "MinSegmentCount");
		NativeFieldInfoPtr_MaxSegmentCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, "MaxSegmentCount");
		NativeMethodInfoPtr_GetSegmentCount_Public_Static_Int32_Vector3_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, 100675160);
		NativeMethodInfoPtr_DrawPowerLine_Public_Static_Void_Vector3_Vector3_List_1_Transform_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, 100675161);
		NativeMethodInfoPtr_PositionSegments_Private_Static_Void_List_1_Vector3_List_1_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, 100675162);
		NativeMethodInfoPtr_GetCatenaryPoints_Private_Static_List_1_Vector3_Vector3_Vector3_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, 100675163);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr, 100675164);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 196115, XrefRangeEnd = 196125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int GetSegmentCount(Vector3 startPoint, Vector3 endPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&startPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPoint;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSegmentCount_Public_Static_Int32_Vector3_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 196132, RefRangeEnd = 196134, XrefRangeStart = 196125, XrefRangeEnd = 196132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DrawPowerLine(Vector3 startPoint, Vector3 endPoint, List<Transform> segments, float lengthFactor)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&startPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPoint;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &lengthFactor;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawPowerLine_Public_Static_Void_Vector3_Vector3_List_1_Transform_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 196172, RefRangeEnd = 196173, XrefRangeStart = 196134, XrefRangeEnd = 196172, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void PositionSegments(List<Vector3> points, List<Transform> segments)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)points);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PositionSegments_Private_Static_Void_List_1_Vector3_List_1_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 196246, RefRangeEnd = 196247, XrefRangeStart = 196173, XrefRangeEnd = 196246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<Vector3> GetCatenaryPoints(Vector3 startPoint, Vector3 endPoint, int pointCount, float l)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&startPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &endPoint;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pointCount;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &l;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCatenaryPoints_Private_Static_List_1_Vector3_Vector3_Vector3_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Vector3>>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PowerLineUtility()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PowerLineUtility>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PowerLineUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
