using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using Il2CppToolBuddy.Pooling.Collections;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGPath : CGShape
{
	private sealed class MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Clone_Public_Virtual_T_0, Il2CppClassPointerStore<CGPath>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_directions;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Directions_Public_get_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Directions_Public_set_Void_SubArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Direction_Public_get_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Direction_Public_set_Void_Il2CppStructArray_1_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_Virtual_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Copy_Public_Static_Void_CGPath_CGPath_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interpolate_Public_Void_Single_Single_byref_Vector3_byref_Vector3_byref_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InterpolateDirection_Public_Vector3_Single_0;

	public unsafe SubArray<Vector3> directions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_directions);
			return new SubArray<Vector3>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_directions), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<Vector3>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<Vector3> Directions
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Directions_Public_get_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<Vector3>(pointer);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 50195, RefRangeEnd = 50197, XrefRangeStart = 50183, XrefRangeEnd = 50195, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Directions_Public_set_Void_SubArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<Vector3> Direction
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50197, XrefRangeEnd = 50207, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Direction_Public_get_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector3>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50207, XrefRangeEnd = 50221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Direction_Public_set_Void_Il2CppStructArray_1_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static CGPath()
	{
		Il2CppClassPointerStore<CGPath>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGPath");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGPath>.NativeClassPtr);
		NativeFieldInfoPtr_directions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGPath>.NativeClassPtr, "directions");
		NativeMethodInfoPtr_get_Directions_Public_get_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666099);
		NativeMethodInfoPtr_set_Directions_Public_set_Void_SubArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666100);
		NativeMethodInfoPtr_get_Direction_Public_get_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666101);
		NativeMethodInfoPtr_set_Direction_Public_set_Void_Il2CppStructArray_1_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666102);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666103);
		NativeMethodInfoPtr__ctor_Public_Void_CGPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666104);
		NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666105);
		NativeMethodInfoPtr_Clone_Public_Virtual_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666106);
		NativeMethodInfoPtr_Copy_Public_Static_Void_CGPath_CGPath_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666107);
		NativeMethodInfoPtr_Interpolate_Public_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666108);
		NativeMethodInfoPtr_Interpolate_Public_Void_Single_Single_byref_Vector3_byref_Vector3_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666109);
		NativeMethodInfoPtr_InterpolateDirection_Public_Vector3_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGPath>.NativeClassPtr, 100666110);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 50234, RefRangeEnd = 50238, XrefRangeStart = 50221, XrefRangeEnd = 50234, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGPath()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGPath>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 50252, RefRangeEnd = 50255, XrefRangeStart = 50238, XrefRangeEnd = 50252, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGPath(CGPath source)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGPath>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGPath_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50255, XrefRangeEnd = 50258, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool Dispose(bool disposing)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&disposing);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50258, XrefRangeEnd = 50266, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override T Clone<T>()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>.Pointer), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50266, XrefRangeEnd = 50278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Copy(CGPath dest, CGPath source)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dest);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Copy_Public_Static_Void_CGPath_CGPath_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 50283, RefRangeEnd = 50287, XrefRangeStart = 50278, XrefRangeEnd = 50283, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Interpolate(float f, out Vector3 position, out Vector3 direction, out Vector3 up)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&f);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref position);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref direction);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interpolate_Public_Void_Single_byref_Vector3_byref_Vector3_byref_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 50287, XrefRangeEnd = 50290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Interpolate(float f, float angleF, out Vector3 pos, out Vector3 dir, out Vector3 up)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&f);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &angleF;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref pos);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref dir);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref up);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interpolate_Public_Void_Single_Single_byref_Vector3_byref_Vector3_byref_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 50293, RefRangeEnd = 50294, XrefRangeStart = 50290, XrefRangeEnd = 50293, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector3 InterpolateDirection(float f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&f);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InterpolateDirection_Public_Vector3_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CGPath(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
