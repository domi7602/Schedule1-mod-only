using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Levelling;

public class Unlockable : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Rank;

	private static readonly System.IntPtr NativeFieldInfoPtr_Title;

	private static readonly System.IntPtr NativeFieldInfoPtr_Icon;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_FullRank_String_Sprite_0;

	public unsafe FullRank Rank
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rank);
			return *(FullRank*)num;
		}
		set
		{
			*(FullRank*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Rank)) = fullRank;
		}
	}

	public unsafe string Title
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Title)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Sprite Icon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Icon);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Icon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	static Unlockable()
	{
		Il2CppClassPointerStore<Unlockable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Levelling", "Unlockable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Unlockable>.NativeClassPtr);
		NativeFieldInfoPtr_Rank = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Unlockable>.NativeClassPtr, "Rank");
		NativeFieldInfoPtr_Title = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Unlockable>.NativeClassPtr, "Title");
		NativeFieldInfoPtr_Icon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Unlockable>.NativeClassPtr, "Icon");
		NativeMethodInfoPtr__ctor_Public_Void_FullRank_String_Sprite_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Unlockable>.NativeClassPtr, 100671126);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 152030, RefRangeEnd = 152033, XrefRangeStart = 152027, XrefRangeEnd = 152030, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Unlockable(FullRank rank, string title, Sprite icon)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Unlockable>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rank);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)icon);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_FullRank_String_Sprite_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Unlockable(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
