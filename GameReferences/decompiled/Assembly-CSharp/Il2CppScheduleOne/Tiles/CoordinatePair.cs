using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Tiles;

public class CoordinatePair : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_coord1;

	private static readonly System.IntPtr NativeFieldInfoPtr_coord2;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Coordinate_Coordinate_0;

	public unsafe Coordinate coord1
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord1);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Coordinate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord1)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coordinate));
		}
	}

	public unsafe Coordinate coord2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord2);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Coordinate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_coord2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)coordinate));
		}
	}

	static CoordinatePair()
	{
		Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tiles", "CoordinatePair");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr);
		NativeFieldInfoPtr_coord1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr, "coord1");
		NativeFieldInfoPtr_coord2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr, "coord2");
		NativeMethodInfoPtr__ctor_Public_Void_Coordinate_Coordinate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr, 100666876);
	}

	[CallerCount(53)]
	[CachedScanResults(RefRangeStart = 100943, RefRangeEnd = 100996, XrefRangeStart = 100940, XrefRangeEnd = 100943, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CoordinatePair(Coordinate _c1, Coordinate _c2)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CoordinatePair>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c1);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Coordinate_Coordinate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CoordinatePair(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
