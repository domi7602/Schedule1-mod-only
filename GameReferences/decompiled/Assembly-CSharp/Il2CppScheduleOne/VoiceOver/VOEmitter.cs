using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using Il2CppScheduleOne.NPCs.Framework;
using UnityEngine;

namespace Il2CppScheduleOne.VoiceOver;

public class VOEmitter : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_PitchVariation;

	private static readonly IntPtr NativeFieldInfoPtr__runtimePitchMultiplier;

	private static readonly IntPtr NativeFieldInfoPtr__audioSourceController;

	private static readonly IntPtr NativeFieldInfoPtr__defaultVODatabase;

	private static readonly IntPtr NativeFieldInfoPtr__currentDatabase;

	private static readonly IntPtr NativeFieldInfoPtr__defaultDatabase;

	private static readonly IntPtr NativeFieldInfoPtr__defaultPitch;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_NPCData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Play_Public_Virtual_New_Void_EVOLineType_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRuntimePitchMultiplier_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetDatabase_Public_Void_VODatabase_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetDefaultPitch_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ResetDatabase_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float PitchVariation
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PitchVariation, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PitchVariation, (void*)(&num));
		}
	}

	public unsafe float _runtimePitchMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__runtimePitchMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__runtimePitchMultiplier)) = num;
		}
	}

	public unsafe AudioSourceController _audioSourceController
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSourceController);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSourceController)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe VODatabase _defaultVODatabase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultVODatabase);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VODatabase>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultVODatabase)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vODatabase));
		}
	}

	public unsafe VODatabase _currentDatabase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDatabase);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VODatabase>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentDatabase)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vODatabase));
		}
	}

	public unsafe VODatabase _defaultDatabase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultDatabase);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VODatabase>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultDatabase)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vODatabase));
		}
	}

	public unsafe float _defaultPitch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultPitch);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultPitch)) = num;
		}
	}

	static VOEmitter()
	{
		Il2CppClassPointerStore<VOEmitter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.VoiceOver", "VOEmitter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr);
		NativeFieldInfoPtr_PitchVariation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "PitchVariation");
		NativeFieldInfoPtr__runtimePitchMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_runtimePitchMultiplier");
		NativeFieldInfoPtr__audioSourceController = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_audioSourceController");
		NativeFieldInfoPtr__defaultVODatabase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_defaultVODatabase");
		NativeFieldInfoPtr__currentDatabase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_currentDatabase");
		NativeFieldInfoPtr__defaultDatabase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_defaultDatabase");
		NativeFieldInfoPtr__defaultPitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, "_defaultPitch");
		NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665966);
		NativeMethodInfoPtr_Initialize_Public_Void_NPCData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665967);
		NativeMethodInfoPtr_Play_Public_Virtual_New_Void_EVOLineType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665968);
		NativeMethodInfoPtr_SetRuntimePitchMultiplier_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665969);
		NativeMethodInfoPtr_SetDatabase_Public_Void_VODatabase_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665970);
		NativeMethodInfoPtr_SetDefaultPitch_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665971);
		NativeMethodInfoPtr_ResetDatabase_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665972);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr, 100665973);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 91030, XrefRangeEnd = 91034, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 91038, RefRangeEnd = 91040, XrefRangeStart = 91034, XrefRangeEnd = 91038, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Initialize(NPCData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_NPCData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 91080, RefRangeEnd = 91082, XrefRangeStart = 91040, XrefRangeEnd = 91080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Play(EVOLineType lineType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&lineType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Play_Public_Virtual_New_Void_EVOLineType_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29040, RefRangeEnd = 29041, XrefRangeStart = 29040, XrefRangeEnd = 29041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRuntimePitchMultiplier(float pitchMultiplier)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&pitchMultiplier);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRuntimePitchMultiplier_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 91084, RefRangeEnd = 91088, XrefRangeStart = 91082, XrefRangeEnd = 91084, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDatabase(VODatabase database, bool writeDefault = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)database);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &writeDefault;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDatabase_Public_Void_VODatabase_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetDefaultPitch(float pitch)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&pitch);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDefaultPitch_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 91089, RefRangeEnd = 91090, XrefRangeStart = 91088, XrefRangeEnd = 91089, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetDatabase()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetDatabase_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VOEmitter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VOEmitter>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VOEmitter(IntPtr pointer)
		: base(pointer)
	{
	}
}
