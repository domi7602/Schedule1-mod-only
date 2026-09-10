using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.AvatarFramework;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Cutscenes;

public class EndCutscene : Cutscene
{
	private static readonly IntPtr NativeFieldInfoPtr_onStandUp;

	private static readonly IntPtr NativeFieldInfoPtr_onRunStart;

	private static readonly IntPtr NativeFieldInfoPtr_onEngineStart;

	private static readonly IntPtr NativeFieldInfoPtr_onLightsOn;

	private static readonly IntPtr NativeFieldInfoPtr_Avatar;

	private static readonly IntPtr NativeMethodInfoPtr_Play_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StandUp_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RunStart_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_EngineStart_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LightsOn_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_On3rdPerson_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe UnityEvent onStandUp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onStandUp);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onStandUp)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onRunStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onRunStart);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onRunStart)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onEngineStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEngineStart);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onEngineStart)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onLightsOn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onLightsOn);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onLightsOn)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe Avatar Avatar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Avatar);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Avatar>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Avatar)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)avatar));
		}
	}

	static EndCutscene()
	{
		Il2CppClassPointerStore<EndCutscene>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Cutscenes", "EndCutscene");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr);
		NativeFieldInfoPtr_onStandUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, "onStandUp");
		NativeFieldInfoPtr_onRunStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, "onRunStart");
		NativeFieldInfoPtr_onEngineStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, "onEngineStart");
		NativeFieldInfoPtr_onLightsOn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, "onLightsOn");
		NativeFieldInfoPtr_Avatar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, "Avatar");
		NativeMethodInfoPtr_Play_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675480);
		NativeMethodInfoPtr_StandUp_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675481);
		NativeMethodInfoPtr_RunStart_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675482);
		NativeMethodInfoPtr_EngineStart_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675483);
		NativeMethodInfoPtr_LightsOn_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675484);
		NativeMethodInfoPtr_On3rdPerson_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675485);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr, 100675486);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199567, XrefRangeEnd = 199574, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Play()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Play_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199574, XrefRangeEnd = 199575, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void StandUp()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_StandUp_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199575, XrefRangeEnd = 199576, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RunStart()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RunStart_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199576, XrefRangeEnd = 199577, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EngineStart()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EngineStart_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199577, XrefRangeEnd = 199578, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LightsOn()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LightsOn_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 199578, XrefRangeEnd = 199584, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void On3rdPerson()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_On3rdPerson_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EndCutscene()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EndCutscene>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EndCutscene(IntPtr pointer)
		: base(pointer)
	{
	}
}
