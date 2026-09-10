using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class SamplePointsMaterialGroup : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MaterialID;

	private static readonly System.IntPtr NativeFieldInfoPtr_Patches;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StartVertex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EndVertex_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_List_1_SamplePointsPatch_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLengths_Public_Void_CGVolume_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clone_Public_SamplePointsMaterialGroup_0;

	public unsafe int MaterialID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaterialID)) = num;
		}
	}

	public unsafe List<SamplePointsPatch> Patches
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Patches);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SamplePointsPatch>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Patches)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe int TriangleCount
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54495, XrefRangeEnd = 54500, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int StartVertex
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 54504, RefRangeEnd = 54508, XrefRangeStart = 54500, XrefRangeEnd = 54504, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StartVertex_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int EndVertex
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 54512, RefRangeEnd = 54516, XrefRangeStart = 54508, XrefRangeEnd = 54512, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EndVertex_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int VertexCount
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54516, XrefRangeEnd = 54523, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SamplePointsMaterialGroup()
	{
		Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "SamplePointsMaterialGroup");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr);
		NativeFieldInfoPtr_MaterialID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, "MaterialID");
		NativeFieldInfoPtr_Patches = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, "Patches");
		NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666468);
		NativeMethodInfoPtr_get_StartVertex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666469);
		NativeMethodInfoPtr_get_EndVertex_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666470);
		NativeMethodInfoPtr_get_VertexCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666471);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666472);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_List_1_SamplePointsPatch_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666473);
		NativeMethodInfoPtr_GetLengths_Public_Void_CGVolume_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666474);
		NativeMethodInfoPtr_Clone_Public_SamplePointsMaterialGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr, 100666475);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54531, RefRangeEnd = 54533, XrefRangeStart = 54523, XrefRangeEnd = 54531, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroup(int materialID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&materialID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 54535, RefRangeEnd = 54539, XrefRangeStart = 54533, XrefRangeEnd = 54535, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroup(int materialID, List<SamplePointsPatch> patches)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SamplePointsMaterialGroup>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&materialID);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)patches);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_List_1_SamplePointsPatch_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54539, XrefRangeEnd = 54559, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GetLengths(CGVolume volume, out float worldLength, out float uLength)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref worldLength);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref uLength);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLengths_Public_Void_CGVolume_byref_Single_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 54570, RefRangeEnd = 54573, XrefRangeStart = 54559, XrefRangeEnd = 54570, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroup Clone()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clone_Public_SamplePointsMaterialGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SamplePointsMaterialGroup>(intPtr) : null;
	}

	public SamplePointsMaterialGroup(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
