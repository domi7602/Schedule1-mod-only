using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Vehicles;

[System.Serializable]
public class ParkData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_lotGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_spotIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_alignment;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Guid_Int32_EParkingAlignment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Il2CppSystem.Guid lotGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lotGUID);
			return *(Il2CppSystem.Guid*)num;
		}
		set
		{
			*(Il2CppSystem.Guid*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lotGUID)) = guid;
		}
	}

	public unsafe int spotIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spotIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spotIndex)) = num;
		}
	}

	public unsafe EParkingAlignment alignment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alignment);
			return *(EParkingAlignment*)num;
		}
		set
		{
			*(EParkingAlignment*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alignment)) = eParkingAlignment;
		}
	}

	static ParkData()
	{
		Il2CppClassPointerStore<ParkData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vehicles", "ParkData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ParkData>.NativeClassPtr);
		NativeFieldInfoPtr_lotGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ParkData>.NativeClassPtr, "lotGUID");
		NativeFieldInfoPtr_spotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ParkData>.NativeClassPtr, "spotIndex");
		NativeFieldInfoPtr_alignment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ParkData>.NativeClassPtr, "alignment");
		NativeMethodInfoPtr__ctor_Public_Void_Guid_Int32_EParkingAlignment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ParkData>.NativeClassPtr, 100666205);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ParkData>.NativeClassPtr, 100666206);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 93713, RefRangeEnd = 93715, XrefRangeStart = 93712, XrefRangeEnd = 93713, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ParkData(Il2CppSystem.Guid lotGUID, int spotIndex, EParkingAlignment alignment)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ParkData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&lotGUID);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &spotIndex;
		*(EParkingAlignment**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &alignment;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Guid_Int32_EParkingAlignment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ParkData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ParkData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ParkData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
