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
using Il2CppToolBuddy.Pooling.Collections;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class CGSpots : CGData
{
	private sealed class MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_Clone_Public_Virtual_T_0, Il2CppClassPointerStore<CGSpots>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_spots;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Spots_Public_get_SubArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Spots_Public_set_Void_SubArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Points_Public_get_Il2CppStructArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Points_Public_set_Void_Il2CppStructArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SubArray_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_List_1_CGSpot_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_CGSpots_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_Virtual_T_0;

	public unsafe SubArray<CGSpot> spots
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spots);
			return new SubArray<CGSpot>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SubArray<CGSpot>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spots), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)subArray)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SubArray<CGSpot>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe SubArray<CGSpot> Spots
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Spots_Public_get_SubArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new SubArray<CGSpot>(pointer);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51103, XrefRangeEnd = 51115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Spots_Public_set_Void_SubArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppStructArray<CGSpot> Points
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51115, XrefRangeEnd = 51125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Points_Public_get_Il2CppStructArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<CGSpot>>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51125, XrefRangeEnd = 51139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Points_Public_set_Void_Il2CppStructArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe override int Count
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 29072, RefRangeEnd = 29101, XrefRangeStart = 29072, XrefRangeEnd = 29101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CGSpots()
	{
		Il2CppClassPointerStore<CGSpots>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "CGSpots");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CGSpots>.NativeClassPtr);
		NativeFieldInfoPtr_spots = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, "spots");
		NativeMethodInfoPtr_get_Spots_Public_get_SubArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666185);
		NativeMethodInfoPtr_set_Spots_Public_set_Void_SubArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666186);
		NativeMethodInfoPtr_get_Points_Public_get_Il2CppStructArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666187);
		NativeMethodInfoPtr_set_Points_Public_set_Void_Il2CppStructArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666188);
		NativeMethodInfoPtr_get_Count_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666189);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666190);
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666191);
		NativeMethodInfoPtr__ctor_Public_Void_SubArray_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666192);
		NativeMethodInfoPtr__ctor_Public_Void_List_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666193);
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_List_1_CGSpot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666194);
		NativeMethodInfoPtr__ctor_Public_Void_CGSpots_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666195);
		NativeMethodInfoPtr_Dispose_Protected_Virtual_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666196);
		NativeMethodInfoPtr_Clone_Public_Virtual_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CGSpots>.NativeClassPtr, 100666197);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 51152, RefRangeEnd = 51157, XrefRangeStart = 51139, XrefRangeEnd = 51152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51157, XrefRangeEnd = 51162, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots([Optional] Il2CppStructArray<CGSpot> points)
	{
		if (points == null)
		{
			points = new Il2CppStructArray<CGSpot>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)points);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppStructArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 51164, RefRangeEnd = 51166, XrefRangeStart = 51162, XrefRangeEnd = 51164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots(SubArray<CGSpot> spots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)spots));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SubArray_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 51182, RefRangeEnd = 51184, XrefRangeStart = 51166, XrefRangeEnd = 51182, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots(List<CGSpot> spots)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51184, XrefRangeEnd = 51202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots([Optional] Il2CppReferenceArray<List<CGSpot>> spots)
	{
		if (spots == null)
		{
			spots = new Il2CppReferenceArray<List<CGSpot>>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spots);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_List_1_CGSpot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 51216, RefRangeEnd = 51217, XrefRangeStart = 51202, XrefRangeEnd = 51216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CGSpots(CGSpots source)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CGSpots>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)source);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_CGSpots_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51217, XrefRangeEnd = 51220, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 51220, XrefRangeEnd = 51228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override T Clone<T>()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), MethodInfoStoreGeneric_Clone_Public_Virtual_T_0<T>.Pointer), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	public CGSpots(params CGSpot[] points)
		: this(new Il2CppStructArray<CGSpot>(points))
	{
	}

	public CGSpots(params List<CGSpot>[] spots)
		: this(new Il2CppReferenceArray<List<CGSpot>>(spots))
	{
	}

	public CGSpots(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
