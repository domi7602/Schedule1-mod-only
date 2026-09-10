using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy;

[StructLayout(LayoutKind.Explicit)]
public struct TcbParameters
{
	private static readonly System.IntPtr NativeFieldInfoPtr__StartTension_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__EndTension_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__StartContinuity_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__EndContinuity_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__StartBias_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__EndBias_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartTension_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndTension_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndBias_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_TcbParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_TcbParameters_TcbParameters_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_TcbParameters_TcbParameters_0;

	[FieldOffset(0)]
	public float _StartTension_k__BackingField;

	[FieldOffset(4)]
	public float _EndTension_k__BackingField;

	[FieldOffset(8)]
	public float _StartContinuity_k__BackingField;

	[FieldOffset(12)]
	public float _EndContinuity_k__BackingField;

	[FieldOffset(16)]
	public float _StartBias_k__BackingField;

	[FieldOffset(20)]
	public float _EndBias_k__BackingField;

	public unsafe float StartTension
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartTension_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(39)]
		[CachedScanResults(RefRangeStart = 42756, RefRangeEnd = 42795, XrefRangeStart = 42756, XrefRangeEnd = 42756, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndTension
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndTension_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 42795, RefRangeEnd = 42811, XrefRangeStart = 42795, XrefRangeEnd = 42795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StartContinuity
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 42811, RefRangeEnd = 42819, XrefRangeStart = 42811, XrefRangeEnd = 42811, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndContinuity
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 42819, RefRangeEnd = 42826, XrefRangeStart = 42819, XrefRangeEnd = 42819, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float StartBias
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartBias_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 29030, RefRangeEnd = 29033, XrefRangeStart = 29030, XrefRangeEnd = 29033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float EndBias
	{
		[CallerCount(0)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndBias_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 26895, RefRangeEnd = 26896, XrefRangeStart = 26895, XrefRangeEnd = 26896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static TcbParameters()
	{
		Il2CppClassPointerStore<TcbParameters>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy", "TcbParameters");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr);
		NativeFieldInfoPtr__StartTension_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<StartTension>k__BackingField");
		NativeFieldInfoPtr__EndTension_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<EndTension>k__BackingField");
		NativeFieldInfoPtr__StartContinuity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<StartContinuity>k__BackingField");
		NativeFieldInfoPtr__EndContinuity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<EndContinuity>k__BackingField");
		NativeFieldInfoPtr__StartBias_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<StartBias>k__BackingField");
		NativeFieldInfoPtr__EndBias_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, "<EndBias>k__BackingField");
		NativeMethodInfoPtr_get_StartTension_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665176);
		NativeMethodInfoPtr_set_StartTension_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665177);
		NativeMethodInfoPtr_get_EndTension_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665178);
		NativeMethodInfoPtr_set_EndTension_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665179);
		NativeMethodInfoPtr_get_StartContinuity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665180);
		NativeMethodInfoPtr_set_StartContinuity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665181);
		NativeMethodInfoPtr_get_EndContinuity_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665182);
		NativeMethodInfoPtr_set_EndContinuity_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665183);
		NativeMethodInfoPtr_get_StartBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665184);
		NativeMethodInfoPtr_set_StartBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665185);
		NativeMethodInfoPtr_get_EndBias_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665186);
		NativeMethodInfoPtr_set_EndBias_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665187);
		NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_TcbParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665188);
		NativeMethodInfoPtr_Equals_Public_Virtual_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665189);
		NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665190);
		NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_TcbParameters_TcbParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665191);
		NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_TcbParameters_TcbParameters_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, 100665192);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 42832, RefRangeEnd = 42835, XrefRangeStart = 42826, XrefRangeEnd = 42832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool Equals(TcbParameters other)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_TcbParameters_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42835, XrefRangeEnd = 42839, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42839, XrefRangeEnd = 42845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override int GetHashCode()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Int32_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42845, XrefRangeEnd = 42846, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator ==(TcbParameters left, TcbParameters right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(TcbParameters**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Equality_Public_Static_Boolean_TcbParameters_TcbParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 42846, XrefRangeEnd = 42847, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool operator !=(TcbParameters left, TcbParameters right)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&left);
		*(TcbParameters**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &right;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Inequality_Public_Static_Boolean_TcbParameters_TcbParameters_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<TcbParameters>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
