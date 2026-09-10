using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppVLB;

[System.Serializable]
public class RaymarchingQuality : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_stepCount;

	private static readonly System.IntPtr NativeFieldInfoPtr__UniqueID;

	private static readonly System.IntPtr NativeFieldInfoPtr_ms_DefaultInstance;

	private static readonly System.IntPtr NativeFieldInfoPtr_kRandomUniqueIdMinRange;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_uniqueID_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hasValidUniqueID_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_defaultInstance_Public_Static_get_RaymarchingQuality_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Private_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_String_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasRaymarchingQualityWithSameUniqueID_Private_Static_Boolean_Il2CppReferenceArray_1_RaymarchingQuality_Int32_0;

	public unsafe string name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int stepCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stepCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stepCount)) = num;
		}
	}

	public unsafe int _UniqueID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UniqueID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UniqueID)) = num;
		}
	}

	public unsafe static RaymarchingQuality ms_DefaultInstance
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ms_DefaultInstance, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ms_DefaultInstance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)raymarchingQuality));
		}
	}

	public unsafe static int kRandomUniqueIdMinRange
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_kRandomUniqueIdMinRange, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_kRandomUniqueIdMinRange, (void*)(&num));
		}
	}

	public unsafe int uniqueID
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 36888, RefRangeEnd = 36892, XrefRangeStart = 36888, XrefRangeEnd = 36892, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_uniqueID_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool hasValidUniqueID
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasValidUniqueID_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static RaymarchingQuality defaultInstance
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70094, XrefRangeEnd = 70098, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_defaultInstance_Public_Static_get_RaymarchingQuality_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr) : null;
		}
	}

	static RaymarchingQuality()
	{
		Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "RaymarchingQuality");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr);
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, "name");
		NativeFieldInfoPtr_stepCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, "stepCount");
		NativeFieldInfoPtr__UniqueID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, "_UniqueID");
		NativeFieldInfoPtr_ms_DefaultInstance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, "ms_DefaultInstance");
		NativeFieldInfoPtr_kRandomUniqueIdMinRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, "kRandomUniqueIdMinRange");
		NativeMethodInfoPtr_get_uniqueID_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663826);
		NativeMethodInfoPtr_get_hasValidUniqueID_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663827);
		NativeMethodInfoPtr_get_defaultInstance_Public_Static_get_RaymarchingQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663828);
		NativeMethodInfoPtr__ctor_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663829);
		NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663830);
		NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_String_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663831);
		NativeMethodInfoPtr_HasRaymarchingQualityWithSameUniqueID_Private_Static_Boolean_Il2CppReferenceArray_1_RaymarchingQuality_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr, 100663832);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70098, XrefRangeEnd = 70103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RaymarchingQuality(int uniqueID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RaymarchingQuality>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&uniqueID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70103, XrefRangeEnd = 70112, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RaymarchingQuality New()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 70121, RefRangeEnd = 70124, XrefRangeStart = 70112, XrefRangeEnd = 70121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static RaymarchingQuality New(string name, int forcedUniqueID, int stepCount)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &forcedUniqueID;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &stepCount;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_New_Public_Static_RaymarchingQuality_String_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RaymarchingQuality>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 70124, XrefRangeEnd = 70125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool HasRaymarchingQualityWithSameUniqueID(Il2CppReferenceArray<RaymarchingQuality> values, int id)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)values);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &id;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasRaymarchingQualityWithSameUniqueID_Private_Static_Boolean_Il2CppReferenceArray_1_RaymarchingQuality_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public RaymarchingQuality(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
