using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Gamepad;

public static class HapticsHandler : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__hapticsManager;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetManager_Public_Static_Void_IHapticsManager_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveManager_Public_Static_Void_IHapticsManager_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Begin_Public_Static_Void_String_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Begin_Public_Static_Void_HapticsData_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_End_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Cancel_Public_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMultiplier_Public_Static_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForceToMultiplier_Public_Static_Single_EHapticImpact_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsManagerValid_Private_Static_Boolean_0;

	public unsafe static IHapticsManager _hapticsManager
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__hapticsManager, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<IHapticsManager>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__hapticsManager, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hapticsManager));
		}
	}

	static HapticsHandler()
	{
		Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Gamepad", "HapticsHandler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr);
		NativeFieldInfoPtr__hapticsManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, "_hapticsManager");
		NativeMethodInfoPtr_SetManager_Public_Static_Void_IHapticsManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685924);
		NativeMethodInfoPtr_RemoveManager_Public_Static_Void_IHapticsManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685925);
		NativeMethodInfoPtr_Begin_Public_Static_Void_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685926);
		NativeMethodInfoPtr_Begin_Public_Static_Void_HapticsData_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685927);
		NativeMethodInfoPtr_End_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685928);
		NativeMethodInfoPtr_Cancel_Public_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685929);
		NativeMethodInfoPtr_SetMultiplier_Public_Static_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685930);
		NativeMethodInfoPtr_ForceToMultiplier_Public_Static_Single_EHapticImpact_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685931);
		NativeMethodInfoPtr_IsManagerValid_Private_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HapticsHandler>.NativeClassPtr, 100685932);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294623, XrefRangeEnd = 294627, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetManager(IHapticsManager hapticsManager)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hapticsManager);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetManager_Public_Static_Void_IHapticsManager_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294627, XrefRangeEnd = 294631, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RemoveManager(IHapticsManager hapticsManager)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hapticsManager);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveManager_Public_Static_Void_IHapticsManager_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 294637, RefRangeEnd = 294644, XrefRangeStart = 294631, XrefRangeEnd = 294637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Begin(string preset, float intensityMultiplier = 1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(preset);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &intensityMultiplier;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Begin_Public_Static_Void_String_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294644, XrefRangeEnd = 294650, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Begin(HapticsData data, float intensityMultiplier = 1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &intensityMultiplier;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Begin_Public_Static_Void_HapticsData_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 294656, RefRangeEnd = 294659, XrefRangeStart = 294650, XrefRangeEnd = 294656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void End()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_End_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 294659, XrefRangeEnd = 294665, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Cancel()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Cancel_Public_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 294671, RefRangeEnd = 294674, XrefRangeStart = 294665, XrefRangeEnd = 294671, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetMultiplier(float multiplier)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&multiplier);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMultiplier_Public_Static_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 294681, RefRangeEnd = 294683, XrefRangeStart = 294674, XrefRangeEnd = 294681, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ForceToMultiplier(EHapticImpact impact, float force)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&impact);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &force;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceToMultiplier_Public_Static_Single_EHapticImpact_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 294687, RefRangeEnd = 294694, XrefRangeStart = 294683, XrefRangeEnd = 294687, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsManagerValid()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsManagerValid_Private_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public HapticsHandler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
