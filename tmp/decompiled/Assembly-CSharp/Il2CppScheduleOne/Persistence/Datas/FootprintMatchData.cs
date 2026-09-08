using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class FootprintMatchData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_TileOwnerGUID;

	private static readonly System.IntPtr NativeFieldInfoPtr_TileIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_FootprintCoordinate;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Vector2_0;

	public unsafe string TileOwnerGUID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TileOwnerGUID);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TileOwnerGUID)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int TileIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TileIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TileIndex)) = num;
		}
	}

	public unsafe Vector2 FootprintCoordinate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootprintCoordinate);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FootprintCoordinate)) = vector;
		}
	}

	static FootprintMatchData()
	{
		Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "FootprintMatchData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr);
		NativeFieldInfoPtr_TileOwnerGUID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr, "TileOwnerGUID");
		NativeFieldInfoPtr_TileIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr, "TileIndex");
		NativeFieldInfoPtr_FootprintCoordinate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr, "FootprintCoordinate");
		NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr, 100669332);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134576, RefRangeEnd = 134577, XrefRangeStart = 134574, XrefRangeEnd = 134576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FootprintMatchData(string tileOwnerGUID, int tileIndex, Vector2 footprintCoordinate)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FootprintMatchData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(tileOwnerGUID);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tileIndex;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &footprintCoordinate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Int32_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FootprintMatchData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
