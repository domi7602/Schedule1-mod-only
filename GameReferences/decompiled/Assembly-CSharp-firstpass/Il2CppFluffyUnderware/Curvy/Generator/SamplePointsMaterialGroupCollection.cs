using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Collections.Generic;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class SamplePointsMaterialGroupCollection : List<SamplePointsMaterialGroup>
{
	private static readonly IntPtr NativeFieldInfoPtr_MaterialID;

	private static readonly IntPtr NativeFieldInfoPtr_AspectCorrectionU;

	private static readonly IntPtr NativeFieldInfoPtr_AspectCorrectionV;

	private static readonly IntPtr NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_AspectCorrection_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_AspectCorrection_Public_set_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_IEnumerable_1_SamplePointsMaterialGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_CalculateAspectCorrection_Public_Void_CGVolume_CGMaterialSettingsEx_0;

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

	public unsafe float AspectCorrectionU
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AspectCorrectionU);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AspectCorrectionU)) = num;
		}
	}

	public unsafe float AspectCorrectionV
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AspectCorrectionV);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AspectCorrectionV)) = num;
		}
	}

	public unsafe int TriangleCount
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 54582, RefRangeEnd = 54583, XrefRangeStart = 54573, XrefRangeEnd = 54582, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float AspectCorrection
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 29130, RefRangeEnd = 29131, XrefRangeStart = 29130, XrefRangeEnd = 29131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AspectCorrection_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29131, RefRangeEnd = 29133, XrefRangeStart = 29131, XrefRangeEnd = 29133, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AspectCorrection_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SamplePointsMaterialGroupCollection()
	{
		Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "SamplePointsMaterialGroupCollection");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr);
		NativeFieldInfoPtr_MaterialID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, "MaterialID");
		NativeFieldInfoPtr_AspectCorrectionU = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, "AspectCorrectionU");
		NativeFieldInfoPtr_AspectCorrectionV = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, "AspectCorrectionV");
		NativeMethodInfoPtr_get_TriangleCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666476);
		NativeMethodInfoPtr_get_AspectCorrection_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666477);
		NativeMethodInfoPtr_set_AspectCorrection_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666478);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666479);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666480);
		NativeMethodInfoPtr__ctor_Public_Void_IEnumerable_1_SamplePointsMaterialGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666481);
		NativeMethodInfoPtr_CalculateAspectCorrection_Public_Void_CGVolume_CGMaterialSettingsEx_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr, 100666482);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 54589, RefRangeEnd = 54590, XrefRangeStart = 54583, XrefRangeEnd = 54589, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroupCollection()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54590, XrefRangeEnd = 54596, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroupCollection(int capacity)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&capacity);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 54602, RefRangeEnd = 54606, XrefRangeStart = 54596, XrefRangeEnd = 54602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SamplePointsMaterialGroupCollection(IEnumerable<SamplePointsMaterialGroup> collection)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SamplePointsMaterialGroupCollection>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collection);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_IEnumerable_1_SamplePointsMaterialGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 54608, RefRangeEnd = 54609, XrefRangeStart = 54606, XrefRangeEnd = 54608, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CalculateAspectCorrection(CGVolume volume, CGMaterialSettingsEx matSettings)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)volume);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)matSettings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateAspectCorrection_Public_Void_CGVolume_CGMaterialSettingsEx_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SamplePointsMaterialGroupCollection(IntPtr pointer)
		: base(pointer)
	{
	}
}
