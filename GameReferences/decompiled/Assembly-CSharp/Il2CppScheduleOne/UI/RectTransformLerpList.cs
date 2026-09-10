using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.UI;

public class RectTransformLerpList : RectTransformLerp
{
	private static readonly IntPtr NativeFieldInfoPtr__targetPositions;

	private static readonly IntPtr NativeFieldInfoPtr__scaleDurationWithDistance;

	private static readonly IntPtr NativeFieldInfoPtr__lerpLocalPosition;

	private static readonly IntPtr NativeFieldInfoPtr__lerpScale;

	private static readonly IntPtr NativeFieldInfoPtr__longestDistance;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LerpTo_Public_Void_Int32_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_LerpTo_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetDurationMultiplier_Private_Single_Vector2_Vector2_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppReferenceArray<RectTransform> _targetPositions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__targetPositions);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<RectTransform>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__targetPositions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool _scaleDurationWithDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scaleDurationWithDistance);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scaleDurationWithDistance)) = flag;
		}
	}

	public unsafe bool _lerpLocalPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpLocalPosition);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpLocalPosition)) = flag;
		}
	}

	public unsafe bool _lerpScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpScale);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lerpScale)) = flag;
		}
	}

	public unsafe float _longestDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__longestDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__longestDistance)) = num;
		}
	}

	static RectTransformLerpList()
	{
		Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "RectTransformLerpList");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr);
		NativeFieldInfoPtr__targetPositions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, "_targetPositions");
		NativeFieldInfoPtr__scaleDurationWithDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, "_scaleDurationWithDistance");
		NativeFieldInfoPtr__lerpLocalPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, "_lerpLocalPosition");
		NativeFieldInfoPtr__lerpScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, "_lerpScale");
		NativeFieldInfoPtr__longestDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, "_longestDistance");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, 100687465);
		NativeMethodInfoPtr_LerpTo_Public_Void_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, 100687466);
		NativeMethodInfoPtr_LerpTo_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, 100687467);
		NativeMethodInfoPtr_GetDurationMultiplier_Private_Single_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, 100687468);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr, 100687469);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 309330, XrefRangeEnd = 309339, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 309363, RefRangeEnd = 309364, XrefRangeStart = 309339, XrefRangeEnd = 309363, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LerpTo(int index, float duration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&index);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &duration;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LerpTo_Public_Void_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 309364, XrefRangeEnd = 309365, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LerpTo(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LerpTo_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 309365, XrefRangeEnd = 309372, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetDurationMultiplier(Vector2 start, Vector2 end)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&start);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &end;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDurationMultiplier_Private_Single_Vector2_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 309372, XrefRangeEnd = 309373, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RectTransformLerpList()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RectTransformLerpList>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RectTransformLerpList(IntPtr pointer)
		: base(pointer)
	{
	}
}
