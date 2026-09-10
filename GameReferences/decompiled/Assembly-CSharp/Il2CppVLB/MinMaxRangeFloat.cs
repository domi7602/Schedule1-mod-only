using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppVLB;

[System.Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct MinMaxRangeFloat
{
	private static readonly System.IntPtr NativeFieldInfoPtr_m_MinValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxValue;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_minValue_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_maxValue_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_randomValue_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_asVector2_Public_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLerpedValue_Public_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_MinMaxRangeFloat_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0;

	[FieldOffset(0)]
	public float m_MinValue;

	[FieldOffset(4)]
	public float m_MaxValue;

	public unsafe float minValue
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_minValue_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float maxValue
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_maxValue_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float randomValue
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_randomValue_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Vector2 asVector2
	{
		[CallerCount(155)]
		[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_asVector2_Public_get_Vector2_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static MinMaxRangeFloat()
	{
		Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "MinMaxRangeFloat");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr);
		NativeFieldInfoPtr_m_MinValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, "m_MinValue");
		NativeFieldInfoPtr_m_MaxValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, "m_MaxValue");
		NativeMethodInfoPtr_get_minValue_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664075);
		NativeMethodInfoPtr_get_maxValue_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664076);
		NativeMethodInfoPtr_get_randomValue_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664077);
		NativeMethodInfoPtr_get_asVector2_Public_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664078);
		NativeMethodInfoPtr_GetLerpedValue_Public_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664079);
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664080);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664081);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_MinMaxRangeFloat_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664082);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664083);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664084);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, 100664085);
	}

	[CallerCount(0)]
	public unsafe float GetLerpedValue(float lerp01)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&lerp01);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLerpedValue_Public_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(30)]
	[CachedScanResults(RefRangeStart = 71922, RefRangeEnd = 71952, XrefRangeStart = 71922, XrefRangeEnd = 71922, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MinMaxRangeFloat(float min, float max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&min);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71952, XrefRangeEnd = 71955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	public unsafe virtual bool Equals(MinMaxRangeFloat other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_MinMaxRangeFloat_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 71955, XrefRangeEnd = 71961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 71961, RefRangeEnd = 71962, XrefRangeStart = 71961, XrefRangeEnd = 71961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(MinMaxRangeFloat**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 71962, RefRangeEnd = 71963, XrefRangeStart = 71962, XrefRangeEnd = 71962, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&lhs);
		*(MinMaxRangeFloat**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rhs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_MinMaxRangeFloat_MinMaxRangeFloat_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<MinMaxRangeFloat>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
