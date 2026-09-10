using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Persistence.Datas;

[Serializable]
public class PlayerData : SaveData
{
	private static readonly IntPtr NativeFieldInfoPtr_PlayerCode;

	private static readonly IntPtr NativeFieldInfoPtr_Position;

	private static readonly IntPtr NativeFieldInfoPtr_Rotation;

	private static readonly IntPtr NativeFieldInfoPtr_IntroCompleted;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Vector3_Single_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string PlayerCode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayerCode);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayerCode)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Vector3 Position
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Position)) = vector;
		}
	}

	public unsafe float Rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rotation)) = num;
		}
	}

	public unsafe bool IntroCompleted
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IntroCompleted);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_IntroCompleted)) = flag;
		}
	}

	static PlayerData()
	{
		Il2CppClassPointerStore<PlayerData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "PlayerData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlayerData>.NativeClassPtr);
		NativeFieldInfoPtr_PlayerCode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, "PlayerCode");
		NativeFieldInfoPtr_Position = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, "Position");
		NativeFieldInfoPtr_Rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, "Rotation");
		NativeFieldInfoPtr_IntroCompleted = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, "IntroCompleted");
		NativeMethodInfoPtr__ctor_Public_Void_String_Vector3_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, 100669445);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerData>.NativeClassPtr, 100669446);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 135221, RefRangeEnd = 135224, XrefRangeStart = 135217, XrefRangeEnd = 135221, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlayerData(string playerCode, Vector3 playerPos, float playerRot, bool introCompleted)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlayerData>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(playerCode);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &playerPos;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &playerRot;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &introCompleted;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Vector3_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 135227, RefRangeEnd = 135228, XrefRangeStart = 135224, XrefRangeEnd = 135227, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlayerData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlayerData>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlayerData(IntPtr pointer)
		: base(pointer)
	{
	}
}
