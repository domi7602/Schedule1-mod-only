using System;
using System.Runtime.CompilerServices;
using Il2CppFluffyUnderware.Curvy.Generator;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppToolBuddy.Pooling.Pools;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Pools;

public static class ArrayPools : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Vector2_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Vector3_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Vector4_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Int32_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Single_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__CGSpot_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vector2_Public_Static_get_ArrayPool_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vector3_Public_Static_get_ArrayPool_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Vector4_Public_Static_get_ArrayPool_1_Vector4_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Int32_Public_Static_get_ArrayPool_1_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Single_Public_Static_get_ArrayPool_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CGSpot_Public_Static_get_ArrayPool_1_CGSpot_0;

	public unsafe static ArrayPool<Vector2> _Vector2_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Vector2_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector2>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Vector2_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<Vector3> _Vector3_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Vector3_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector3>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Vector3_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<Vector4> _Vector4_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Vector4_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector4>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Vector4_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<int> _Int32_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Int32_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<int>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Int32_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<float> _Single_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Single_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<float>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Single_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<CGSpot> _CGSpot_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__CGSpot_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<CGSpot>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__CGSpot_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arrayPool));
		}
	}

	public unsafe static ArrayPool<Vector2> Vector2
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43190, XrefRangeEnd = 43194, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vector2_Public_Static_get_ArrayPool_1_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector2>>(intPtr) : null;
		}
	}

	public unsafe static ArrayPool<Vector3> Vector3
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43194, XrefRangeEnd = 43198, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vector3_Public_Static_get_ArrayPool_1_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector3>>(intPtr) : null;
		}
	}

	public unsafe static ArrayPool<Vector4> Vector4
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43198, XrefRangeEnd = 43202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Vector4_Public_Static_get_ArrayPool_1_Vector4_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<Vector4>>(intPtr) : null;
		}
	}

	public unsafe static ArrayPool<int> Int32
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43202, XrefRangeEnd = 43206, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Int32_Public_Static_get_ArrayPool_1_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<int>>(intPtr) : null;
		}
	}

	public unsafe static ArrayPool<float> Single
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43206, XrefRangeEnd = 43210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Single_Public_Static_get_ArrayPool_1_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<float>>(intPtr) : null;
		}
	}

	public unsafe static ArrayPool<CGSpot> CGSpot
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 43210, XrefRangeEnd = 43214, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CGSpot_Public_Static_get_ArrayPool_1_CGSpot_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<CGSpot>>(intPtr) : null;
		}
	}

	static ArrayPools()
	{
		Il2CppClassPointerStore<ArrayPools>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Pools", "ArrayPools");
		NativeFieldInfoPtr__Vector2_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<Vector2>k__BackingField");
		NativeFieldInfoPtr__Vector3_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<Vector3>k__BackingField");
		NativeFieldInfoPtr__Vector4_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<Vector4>k__BackingField");
		NativeFieldInfoPtr__Int32_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<Int32>k__BackingField");
		NativeFieldInfoPtr__Single_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<Single>k__BackingField");
		NativeFieldInfoPtr__CGSpot_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, "<CGSpot>k__BackingField");
		NativeMethodInfoPtr_get_Vector2_Public_Static_get_ArrayPool_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665249);
		NativeMethodInfoPtr_get_Vector3_Public_Static_get_ArrayPool_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665250);
		NativeMethodInfoPtr_get_Vector4_Public_Static_get_ArrayPool_1_Vector4_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665251);
		NativeMethodInfoPtr_get_Int32_Public_Static_get_ArrayPool_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665252);
		NativeMethodInfoPtr_get_Single_Public_Static_get_ArrayPool_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665253);
		NativeMethodInfoPtr_get_CGSpot_Public_Static_get_ArrayPool_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPools>.NativeClassPtr, 100665254);
	}

	public ArrayPools(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
