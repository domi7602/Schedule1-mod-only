using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Persistence.Datas;

public class GraffitiData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_SpraySurfaces;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_WorldSpraySurfaceData_0;

	public unsafe List<WorldSpraySurfaceData> SpraySurfaces
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpraySurfaces);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<WorldSpraySurfaceData>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SpraySurfaces)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static GraffitiData()
	{
		Il2CppClassPointerStore<GraffitiData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "GraffitiData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GraffitiData>.NativeClassPtr);
		NativeFieldInfoPtr_SpraySurfaces = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraffitiData>.NativeClassPtr, "SpraySurfaces");
		NativeMethodInfoPtr__ctor_Public_Void_List_1_WorldSpraySurfaceData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraffitiData>.NativeClassPtr, 100669359);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134750, RefRangeEnd = 134751, XrefRangeStart = 134741, XrefRangeEnd = 134750, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GraffitiData(List<WorldSpraySurfaceData> spraySurfaces)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GraffitiData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spraySurfaces);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_WorldSpraySurfaceData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GraffitiData(IntPtr pointer)
		: base(pointer)
	{
	}
}
