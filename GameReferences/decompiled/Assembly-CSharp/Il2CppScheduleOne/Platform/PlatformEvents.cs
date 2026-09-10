using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSteamworks;
using Il2CppSystem;

namespace Il2CppScheduleOne.Platform;

public static class PlatformEvents : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_OnOverlayActivated;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnOverlayDeactivated;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnGameLoseFocus;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnGameGainFocus;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnOverlayActivated_Public_Static_add_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnOverlayActivated_Public_Static_rem_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnOverlayDeactivated_Public_Static_add_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnOverlayDeactivated_Public_Static_rem_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnGameLoseFocus_Public_Static_add_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnGameLoseFocus_Public_Static_rem_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnGameGainFocus_Public_Static_add_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnGameGainFocus_Public_Static_rem_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitFocusCallback_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnApplicationFocusChanged_Private_Static_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitOverlayCallback_Private_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OverlayActivated_Private_Static_Void_GameOverlayActivated_t_0;

	public unsafe static Il2CppSystem.Action OnOverlayActivated
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnOverlayActivated, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnOverlayActivated, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static Il2CppSystem.Action OnOverlayDeactivated
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnOverlayDeactivated, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnOverlayDeactivated, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static Il2CppSystem.Action OnGameLoseFocus
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnGameLoseFocus, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnGameLoseFocus, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static Il2CppSystem.Action OnGameGainFocus
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnGameGainFocus, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnGameGainFocus, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	static PlatformEvents()
	{
		Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Platform", "PlatformEvents");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr);
		NativeFieldInfoPtr_OnOverlayActivated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, "OnOverlayActivated");
		NativeFieldInfoPtr_OnOverlayDeactivated = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, "OnOverlayDeactivated");
		NativeFieldInfoPtr_OnGameLoseFocus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, "OnGameLoseFocus");
		NativeFieldInfoPtr_OnGameGainFocus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, "OnGameGainFocus");
		NativeMethodInfoPtr_add_OnOverlayActivated_Public_Static_add_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668696);
		NativeMethodInfoPtr_remove_OnOverlayActivated_Public_Static_rem_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668697);
		NativeMethodInfoPtr_add_OnOverlayDeactivated_Public_Static_add_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668698);
		NativeMethodInfoPtr_remove_OnOverlayDeactivated_Public_Static_rem_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668699);
		NativeMethodInfoPtr_add_OnGameLoseFocus_Public_Static_add_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668700);
		NativeMethodInfoPtr_remove_OnGameLoseFocus_Public_Static_rem_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668701);
		NativeMethodInfoPtr_add_OnGameGainFocus_Public_Static_add_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668702);
		NativeMethodInfoPtr_remove_OnGameGainFocus_Public_Static_rem_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668703);
		NativeMethodInfoPtr_InitFocusCallback_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668704);
		NativeMethodInfoPtr_OnApplicationFocusChanged_Private_Static_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668705);
		NativeMethodInfoPtr_InitOverlayCallback_Private_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668706);
		NativeMethodInfoPtr_OverlayActivated_Private_Static_Void_GameOverlayActivated_t_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PlatformEvents>.NativeClassPtr, 100668707);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124111, RefRangeEnd = 124112, XrefRangeStart = 124104, XrefRangeEnd = 124111, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void add_OnOverlayActivated(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnOverlayActivated_Public_Static_add_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124119, RefRangeEnd = 124120, XrefRangeStart = 124112, XrefRangeEnd = 124119, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void remove_OnOverlayActivated(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnOverlayActivated_Public_Static_rem_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124127, RefRangeEnd = 124128, XrefRangeStart = 124120, XrefRangeEnd = 124127, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void add_OnOverlayDeactivated(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnOverlayDeactivated_Public_Static_add_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124135, RefRangeEnd = 124136, XrefRangeStart = 124128, XrefRangeEnd = 124135, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void remove_OnOverlayDeactivated(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnOverlayDeactivated_Public_Static_rem_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124143, RefRangeEnd = 124144, XrefRangeStart = 124136, XrefRangeEnd = 124143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void add_OnGameLoseFocus(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnGameLoseFocus_Public_Static_add_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 124151, RefRangeEnd = 124152, XrefRangeStart = 124144, XrefRangeEnd = 124151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void remove_OnGameLoseFocus(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnGameLoseFocus_Public_Static_rem_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124152, XrefRangeEnd = 124159, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void add_OnGameGainFocus(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnGameGainFocus_Public_Static_add_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124159, XrefRangeEnd = 124166, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void remove_OnGameGainFocus(Il2CppSystem.Action value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnGameGainFocus_Public_Static_rem_Void_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124166, XrefRangeEnd = 124176, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitFocusCallback()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitFocusCallback_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124176, XrefRangeEnd = 124186, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OnApplicationFocusChanged(bool hasFocus)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&hasFocus);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnApplicationFocusChanged_Private_Static_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124186, XrefRangeEnd = 124208, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitOverlayCallback()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitOverlayCallback_Private_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 124208, XrefRangeEnd = 124222, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OverlayActivated(GameOverlayActivated_t pCallback)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&pCallback);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OverlayActivated_Private_Static_Void_GameOverlayActivated_t_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PlatformEvents(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
