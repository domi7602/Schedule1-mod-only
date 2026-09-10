using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.Graffiti;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class WorldSpraySurfaceData : SpraySurfaceData
{
	private static readonly IntPtr NativeFieldInfoPtr_GUID;

	private static readonly IntPtr NativeFieldInfoPtr_HasDrawingBeenFinalized;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_String_Boolean_0;

	public unsafe string GUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GUID);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool HasDrawingBeenFinalized
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasDrawingBeenFinalized);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HasDrawingBeenFinalized)) = flag;
		}
	}

	static WorldSpraySurfaceData()
	{
		Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "WorldSpraySurfaceData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr);
		NativeFieldInfoPtr_GUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr, "GUID");
		NativeFieldInfoPtr_HasDrawingBeenFinalized = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr, "HasDrawingBeenFinalized");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr, 100669484);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135553, RefRangeEnd = 135554, XrefRangeStart = 135542, XrefRangeEnd = 135553, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe WorldSpraySurfaceData(List<SprayStroke> strokes, bool containsCartelGraffiti, string guid, bool hasBeenFinalized)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<WorldSpraySurfaceData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)strokes);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &containsCartelGraffiti;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(guid);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &hasBeenFinalized;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_SprayStroke_Boolean_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public WorldSpraySurfaceData(IntPtr pointer)
		: base(pointer)
	{
	}
}
