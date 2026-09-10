using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.State;
using Il2CppSystem;

namespace Il2CppScheduleOne;

public class SceneState : MonoStateMachine
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Current_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__LastFrameActiveState_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__inGameState;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnActiveStateChanged;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Current_Public_Static_get_SceneState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Current_Private_Static_set_Void_SceneState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_ActiveState_Public_Static_get_IState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LastFrameActiveState_Public_Static_get_IState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_LastFrameActiveState_Private_Static_set_Void_IState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_InGame_Public_get_InGameStateMachine_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnActiveStateChanged_Public_add_Void_Action_1_IState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnActiveStateChanged_Public_rem_Void_Action_1_IState_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static SceneState _Current_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__Current_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneState>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__Current_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneState));
		}
	}

	public unsafe static IState _LastFrameActiveState_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__LastFrameActiveState_k__BackingField, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__LastFrameActiveState_k__BackingField, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state));
		}
	}

	public unsafe InGameStateMachine _inGameState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inGameState);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InGameStateMachine>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inGameState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inGameStateMachine));
		}
	}

	public unsafe Il2CppSystem.Action<IState> OnActiveStateChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnActiveStateChanged);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<IState>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnActiveStateChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static SceneState Current
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90605, XrefRangeEnd = 90607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Current_Public_Static_get_SceneState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneState>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90607, XrefRangeEnd = 90611, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Current_Private_Static_set_Void_SceneState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static IState ActiveState
	{
		[CallerCount(19)]
		[CachedScanResults(RefRangeStart = 90615, RefRangeEnd = 90634, XrefRangeStart = 90611, XrefRangeEnd = 90615, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ActiveState_Public_Static_get_IState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
		}
	}

	public unsafe static IState LastFrameActiveState
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90634, XrefRangeEnd = 90636, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LastFrameActiveState_Public_Static_get_IState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IState>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90636, XrefRangeEnd = 90640, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_LastFrameActiveState_Private_Static_set_Void_IState_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe InGameStateMachine InGame
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InGame_Public_get_InGameStateMachine_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InGameStateMachine>(intPtr) : null;
		}
	}

	static SceneState()
	{
		Il2CppClassPointerStore<SceneState>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "SceneState");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SceneState>.NativeClassPtr);
		NativeFieldInfoPtr__Current_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneState>.NativeClassPtr, "<Current>k__BackingField");
		NativeFieldInfoPtr__LastFrameActiveState_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneState>.NativeClassPtr, "<LastFrameActiveState>k__BackingField");
		NativeFieldInfoPtr__inGameState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneState>.NativeClassPtr, "_inGameState");
		NativeFieldInfoPtr_OnActiveStateChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneState>.NativeClassPtr, "OnActiveStateChanged");
		NativeMethodInfoPtr_get_Current_Public_Static_get_SceneState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665913);
		NativeMethodInfoPtr_set_Current_Private_Static_set_Void_SceneState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665914);
		NativeMethodInfoPtr_get_ActiveState_Public_Static_get_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665915);
		NativeMethodInfoPtr_get_LastFrameActiveState_Public_Static_get_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665916);
		NativeMethodInfoPtr_set_LastFrameActiveState_Private_Static_set_Void_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665917);
		NativeMethodInfoPtr_get_InGame_Public_get_InGameStateMachine_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665918);
		NativeMethodInfoPtr_add_OnActiveStateChanged_Public_add_Void_Action_1_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665919);
		NativeMethodInfoPtr_remove_OnActiveStateChanged_Public_rem_Void_Action_1_IState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665920);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665921);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665922);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665923);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneState>.NativeClassPtr, 100665924);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90645, RefRangeEnd = 90646, XrefRangeStart = 90640, XrefRangeEnd = 90645, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void add_OnActiveStateChanged(Il2CppSystem.Action<IState> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnActiveStateChanged_Public_add_Void_Action_1_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90651, RefRangeEnd = 90652, XrefRangeStart = 90646, XrefRangeEnd = 90651, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void remove_OnActiveStateChanged(Il2CppSystem.Action<IState> value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnActiveStateChanged_Public_rem_Void_Action_1_IState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90652, XrefRangeEnd = 90684, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90684, XrefRangeEnd = 90699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90699, XrefRangeEnd = 90709, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90709, XrefRangeEnd = 90710, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SceneState()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SceneState>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SceneState(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
