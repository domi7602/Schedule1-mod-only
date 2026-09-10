using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.AvatarFramework;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Appearance : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_AvatarSettings;

	private static readonly System.IntPtr NativeFieldInfoPtr_Mugshot;

	private static readonly System.IntPtr NativeFieldInfoPtr_ChristmasAppearance;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Appearance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe AvatarSettings AvatarSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarSettings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AvatarSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AvatarSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatarSettings));
		}
	}

	public unsafe Sprite Mugshot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mugshot);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Mugshot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	public unsafe AvatarSettings ChristmasAppearance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChristmasAppearance);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AvatarSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ChristmasAppearance)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatarSettings));
		}
	}

	static Appearance()
	{
		Il2CppClassPointerStore<Appearance>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Appearance");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Appearance>.NativeClassPtr);
		NativeFieldInfoPtr_AvatarSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Appearance>.NativeClassPtr, "AvatarSettings");
		NativeFieldInfoPtr_Mugshot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Appearance>.NativeClassPtr, "Mugshot");
		NativeFieldInfoPtr_ChristmasAppearance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Appearance>.NativeClassPtr, "ChristmasAppearance");
		NativeMethodInfoPtr_GetCopy_Public_Appearance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Appearance>.NativeClassPtr, 100682793);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Appearance>.NativeClassPtr, 100682794);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272204, XrefRangeEnd = 272211, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Appearance GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Appearance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Appearance>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Appearance()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Appearance>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Appearance(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
