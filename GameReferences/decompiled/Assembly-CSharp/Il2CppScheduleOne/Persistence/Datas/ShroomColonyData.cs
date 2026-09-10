using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class ShroomColonyData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MushroomSpawnID;

	private static readonly System.IntPtr NativeFieldInfoPtr_GrowthProgress;

	private static readonly System.IntPtr NativeFieldInfoPtr_Quality;

	private static readonly System.IntPtr NativeFieldInfoPtr_ActiveMushroomAlignmentIndices;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_Il2CppStructArray_1_Int32_0;

	public unsafe string MushroomSpawnID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MushroomSpawnID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MushroomSpawnID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float GrowthProgress
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowthProgress);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowthProgress)) = num;
		}
	}

	public unsafe float Quality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quality);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Quality)) = num;
		}
	}

	public unsafe Il2CppStructArray<int> ActiveMushroomAlignmentIndices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMushroomAlignmentIndices);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ActiveMushroomAlignmentIndices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ShroomColonyData()
	{
		Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "ShroomColonyData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr);
		NativeFieldInfoPtr_MushroomSpawnID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr, "MushroomSpawnID");
		NativeFieldInfoPtr_GrowthProgress = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr, "GrowthProgress");
		NativeFieldInfoPtr_Quality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr, "Quality");
		NativeFieldInfoPtr_ActiveMushroomAlignmentIndices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr, "ActiveMushroomAlignmentIndices");
		NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_Il2CppStructArray_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr, 100669468);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135458, RefRangeEnd = 135459, XrefRangeStart = 135455, XrefRangeEnd = 135458, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShroomColonyData(string mushroomSpawnID, float growthProgress, float quality, Il2CppStructArray<int> activeMushroomAlignmentIndices)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShroomColonyData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(mushroomSpawnID);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &growthProgress;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &quality;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)activeMushroomAlignmentIndices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Single_Single_Il2CppStructArray_1_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ShroomColonyData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
