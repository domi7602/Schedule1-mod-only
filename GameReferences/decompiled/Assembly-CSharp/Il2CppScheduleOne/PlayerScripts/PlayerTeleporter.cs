using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.PlayerScripts;

public class PlayerTeleporter : MonoBehaviour
{
	private static readonly IntPtr NativeMethodInfoPtr_Teleport_Public_Void_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static PlayerTeleporter()
	{
		Il2CppClassPointerStore<PlayerTeleporter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerScripts", "PlayerTeleporter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlayerTeleporter>.NativeClassPtr);
		NativeMethodInfoPtr_Teleport_Public_Void_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerTeleporter>.NativeClassPtr, 100672236);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlayerTeleporter>.NativeClassPtr, 100672237);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 164324, XrefRangeEnd = 164343, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Teleport(Transform destination)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)destination);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Teleport_Public_Void_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PlayerTeleporter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PlayerTeleporter>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlayerTeleporter(IntPtr pointer)
		: base(pointer)
	{
	}
}
