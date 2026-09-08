using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class GameSettings : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ConsoleEnabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseRandomizedMixMaps;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool ConsoleEnabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConsoleEnabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConsoleEnabled)) = flag;
		}
	}

	public unsafe bool UseRandomizedMixMaps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseRandomizedMixMaps);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseRandomizedMixMaps)) = flag;
		}
	}

	static GameSettings()
	{
		Il2CppClassPointerStore<GameSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "GameSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameSettings>.NativeClassPtr);
		NativeFieldInfoPtr_ConsoleEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameSettings>.NativeClassPtr, "ConsoleEnabled");
		NativeFieldInfoPtr_UseRandomizedMixMaps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameSettings>.NativeClassPtr, "UseRandomizedMixMaps");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameSettings>.NativeClassPtr, 100675251);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GameSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
