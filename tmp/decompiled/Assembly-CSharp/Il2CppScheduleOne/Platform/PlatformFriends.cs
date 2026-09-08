using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Platform;

public class PlatformFriends : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetAvatarTexture_Public_Static_Texture2D_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsLocalPlayerFriendsWith_Public_Static_Boolean_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static PlatformFriends()
	{
		Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "PlatformFriends");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr);
		NativeMethodInfoPtr_GetAvatarTexture_Public_Static_Texture2D_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr, 100668708);
		NativeMethodInfoPtr_IsLocalPlayerFriendsWith_Public_Static_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr, 100668709);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr, 100668710);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124238, RefRangeEnd = 124239, XrefRangeStart = 124222, XrefRangeEnd = 124238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Texture2D GetAvatarTexture(string id)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAvatarTexture_Public_Static_Texture2D_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124242, RefRangeEnd = 124243, XrefRangeStart = 124239, XrefRangeEnd = 124242, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsLocalPlayerFriendsWith(string otherPlayerID)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(otherPlayerID);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsLocalPlayerFriendsWith_Public_Static_Boolean_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlatformFriends()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlatformFriends>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlatformFriends(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
