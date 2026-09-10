using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGBounds : CGData
{
	private sealed class MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Clone_Public_Virtual_T_0, Il2CppClassPointerStore<CGBounds>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_mBounds;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Bounds_Public_set_Void_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Depth_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Bounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGBounds_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_Virtual_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Copy_Public_Static_Void_CGBounds_CGBounds_0;

	public unsafe Il2CppSystem.Nullable<Bounds> mBounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds);
			return new Il2CppSystem.Nullable<Bounds>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mBounds), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<Bounds>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Bounds Bounds
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 46642, RefRangeEnd = 46645, XrefRangeStart = 46638, XrefRangeEnd = 46642, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Bounds*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 46648, RefRangeEnd = 46650, XrefRangeStart = 46645, XrefRangeEnd = 46648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Bounds_Public_set_Void_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Depth
	{
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 46654, RefRangeEnd = 46663, XrefRangeStart = 46650, XrefRangeEnd = 46654, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Depth_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CGBounds()
	{
		Il2CppClassPointerStore<CGBounds>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGBounds");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGBounds>.NativeClassPtr);
		NativeFieldInfoPtr_mBounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, "mBounds");
		NativeMethodInfoPtr_get_Bounds_Public_get_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665711);
		NativeMethodInfoPtr_set_Bounds_Public_set_Void_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665712);
		NativeMethodInfoPtr_get_Depth_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665713);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665714);
		NativeMethodInfoPtr__ctor_Public_Void_Bounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665715);
		NativeMethodInfoPtr__ctor_Public_Void_CGBounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665716);
		NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665717);
		NativeMethodInfoPtr_Clone_Public_Virtual_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665718);
		NativeMethodInfoPtr_Copy_Public_Static_Void_CGBounds_CGBounds_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGBounds>.NativeClassPtr, 100665719);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGBounds()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGBounds>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46663, XrefRangeEnd = 46667, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGBounds(Bounds bounds)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGBounds>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&bounds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Bounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 46677, RefRangeEnd = 46680, XrefRangeStart = 46667, XrefRangeEnd = 46677, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGBounds(CGBounds source)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGBounds>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGBounds_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46680, XrefRangeEnd = 46683, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void RecalculateBounds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_RecalculateBounds_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46683, XrefRangeEnd = 46691, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 46691, XrefRangeEnd = 46699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Copy(CGBounds dest, CGBounds source)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dest);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Copy_Public_Static_Void_CGBounds_CGBounds_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CGBounds(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
