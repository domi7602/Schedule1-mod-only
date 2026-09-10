using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Graffiti;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class SpraySurfaceData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_Strokes;

	private static readonly IntPtr NativeFieldInfoPtr_ContainsCartelGraffiti;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_0;

	public unsafe List<SprayStroke> Strokes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Strokes);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<SprayStroke>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Strokes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool ContainsCartelGraffiti
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContainsCartelGraffiti);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ContainsCartelGraffiti)) = flag;
		}
	}

	static SpraySurfaceData()
	{
		Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "SpraySurfaceData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr);
		NativeFieldInfoPtr_Strokes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr, "Strokes");
		NativeFieldInfoPtr_ContainsCartelGraffiti = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr, "ContainsCartelGraffiti");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr, 100669469);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135468, RefRangeEnd = 135469, XrefRangeStart = 135459, XrefRangeEnd = 135468, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SpraySurfaceData(List<SprayStroke> strokes, bool containsCartelGraffiti)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpraySurfaceData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)strokes);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &containsCartelGraffiti;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpraySurfaceData(IntPtr pointer)
		: base(pointer)
	{
	}
}
