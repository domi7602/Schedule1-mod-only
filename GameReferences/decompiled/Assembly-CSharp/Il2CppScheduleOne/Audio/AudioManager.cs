using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Audio;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Tools;
using UnityEngine.Audio;

namespace Il2CppScheduleOne.Audio;

public class AudioManager : PersistentSingleton<AudioManager>
{
	private static readonly IntPtr NativeFieldInfoPtr_MinGameVolume;

	private static readonly IntPtr NativeFieldInfoPtr_MaxGameVolume;

	private static readonly IntPtr NativeFieldInfoPtr_GameVolumeLerpSpeed;

	private static readonly IntPtr NativeFieldInfoPtr_onVolumeSettingsChanged;

	private static readonly IntPtr NativeFieldInfoPtr__MainGameMixer_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__MenuMixer_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__MusicMixer_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__defaultSnapshot;

	private static readonly IntPtr NativeFieldInfoPtr__distortedSnapshot;

	private static readonly IntPtr NativeFieldInfoPtr__masterVolume;

	private static readonly IntPtr NativeFieldInfoPtr__ambientVolume;

	private static readonly IntPtr NativeFieldInfoPtr__footstepsVolume;

	private static readonly IntPtr NativeFieldInfoPtr__fxVolume;

	private static readonly IntPtr NativeFieldInfoPtr__uiVolume;

	private static readonly IntPtr NativeFieldInfoPtr__musicVolume;

	private static readonly IntPtr NativeFieldInfoPtr__voiceVolume;

	private static readonly IntPtr NativeFieldInfoPtr__weatherVolume;

	private static readonly IntPtr NativeFieldInfoPtr__currentMainMixerVolume;

	private static readonly IntPtr NativeMethodInfoPtr_get_MasterVolume_Public_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MainGameMixer_Public_get_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MainGameMixer_Private_set_Void_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MenuMixer_Public_get_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MenuMixer_Private_set_Void_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MusicMixer_Public_get_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MusicMixer_Private_set_Void_AudioMixerGroup_0;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetDistorted_Public_Void_Boolean_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetVolume_Public_Single_EAudioType_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMasterVolume_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetVolume_Public_Void_EAudioType_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMainMixerVolume_Private_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ValueToVolume_Private_Static_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__Start_b__30_0_Private_Void_0;

	public unsafe static float MinGameVolume
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinGameVolume, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinGameVolume, (void*)(&num));
		}
	}

	public unsafe static float MaxGameVolume
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxGameVolume, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxGameVolume, (void*)(&num));
		}
	}

	public unsafe static float GameVolumeLerpSpeed
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_GameVolumeLerpSpeed, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_GameVolumeLerpSpeed, (void*)(&num));
		}
	}

	public unsafe PreallocatedAction onVolumeSettingsChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onVolumeSettingsChanged);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PreallocatedAction>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onVolumeSettingsChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)preallocatedAction));
		}
	}

	public unsafe AudioMixerGroup _MainGameMixer_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MainGameMixer_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MainGameMixer_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioMixerGroup));
		}
	}

	public unsafe AudioMixerGroup _MenuMixer_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MenuMixer_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MenuMixer_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioMixerGroup));
		}
	}

	public unsafe AudioMixerGroup _MusicMixer_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MusicMixer_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MusicMixer_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioMixerGroup));
		}
	}

	public unsafe AudioMixerSnapshot _defaultSnapshot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultSnapshot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerSnapshot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultSnapshot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioMixerSnapshot));
		}
	}

	public unsafe AudioMixerSnapshot _distortedSnapshot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortedSnapshot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerSnapshot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__distortedSnapshot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioMixerSnapshot));
		}
	}

	public unsafe float _masterVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__masterVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__masterVolume)) = num;
		}
	}

	public unsafe float _ambientVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ambientVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ambientVolume)) = num;
		}
	}

	public unsafe float _footstepsVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__footstepsVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__footstepsVolume)) = num;
		}
	}

	public unsafe float _fxVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fxVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__fxVolume)) = num;
		}
	}

	public unsafe float _uiVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiVolume)) = num;
		}
	}

	public unsafe float _musicVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__musicVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__musicVolume)) = num;
		}
	}

	public unsafe float _voiceVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__voiceVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__voiceVolume)) = num;
		}
	}

	public unsafe float _weatherVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weatherVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weatherVolume)) = num;
		}
	}

	public unsafe float _currentMainMixerVolume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentMainMixerVolume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentMainMixerVolume)) = num;
		}
	}

	public unsafe float MasterVolume
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MasterVolume_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe AudioMixerGroup MainGameMixer
	{
		[CallerCount(13)]
		[CachedScanResults(RefRangeStart = 2964, RefRangeEnd = 2977, XrefRangeStart = 2964, XrefRangeEnd = 2977, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MainGameMixer_Public_get_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 2978, RefRangeEnd = 2980, XrefRangeStart = 2978, XrefRangeEnd = 2980, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MainGameMixer_Private_set_Void_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe AudioMixerGroup MenuMixer
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 2980, RefRangeEnd = 2987, XrefRangeStart = 2980, XrefRangeEnd = 2987, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MenuMixer_Public_get_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MenuMixer_Private_set_Void_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe AudioMixerGroup MusicMixer
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 30474, RefRangeEnd = 30475, XrefRangeStart = 30474, XrefRangeEnd = 30475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MusicMixer_Public_get_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioMixerGroup>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MusicMixer_Private_set_Void_AudioMixerGroup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static AudioManager()
	{
		Il2CppClassPointerStore<AudioManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "AudioManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AudioManager>.NativeClassPtr);
		NativeFieldInfoPtr_MinGameVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "MinGameVolume");
		NativeFieldInfoPtr_MaxGameVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "MaxGameVolume");
		NativeFieldInfoPtr_GameVolumeLerpSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "GameVolumeLerpSpeed");
		NativeFieldInfoPtr_onVolumeSettingsChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "onVolumeSettingsChanged");
		NativeFieldInfoPtr__MainGameMixer_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "<MainGameMixer>k__BackingField");
		NativeFieldInfoPtr__MenuMixer_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "<MenuMixer>k__BackingField");
		NativeFieldInfoPtr__MusicMixer_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "<MusicMixer>k__BackingField");
		NativeFieldInfoPtr__defaultSnapshot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_defaultSnapshot");
		NativeFieldInfoPtr__distortedSnapshot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_distortedSnapshot");
		NativeFieldInfoPtr__masterVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_masterVolume");
		NativeFieldInfoPtr__ambientVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_ambientVolume");
		NativeFieldInfoPtr__footstepsVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_footstepsVolume");
		NativeFieldInfoPtr__fxVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_fxVolume");
		NativeFieldInfoPtr__uiVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_uiVolume");
		NativeFieldInfoPtr__musicVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_musicVolume");
		NativeFieldInfoPtr__voiceVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_voiceVolume");
		NativeFieldInfoPtr__weatherVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_weatherVolume");
		NativeFieldInfoPtr__currentMainMixerVolume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, "_currentMainMixerVolume");
		NativeMethodInfoPtr_get_MasterVolume_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676715);
		NativeMethodInfoPtr_get_MainGameMixer_Public_get_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676716);
		NativeMethodInfoPtr_set_MainGameMixer_Private_set_Void_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676717);
		NativeMethodInfoPtr_get_MenuMixer_Public_get_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676718);
		NativeMethodInfoPtr_set_MenuMixer_Private_set_Void_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676719);
		NativeMethodInfoPtr_get_MusicMixer_Public_get_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676720);
		NativeMethodInfoPtr_set_MusicMixer_Private_set_Void_AudioMixerGroup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676721);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676722);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676723);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676724);
		NativeMethodInfoPtr_SetDistorted_Public_Void_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676725);
		NativeMethodInfoPtr_GetVolume_Public_Single_EAudioType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676726);
		NativeMethodInfoPtr_SetMasterVolume_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676727);
		NativeMethodInfoPtr_SetVolume_Public_Void_EAudioType_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676728);
		NativeMethodInfoPtr_SetMainMixerVolume_Private_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676729);
		NativeMethodInfoPtr_ValueToVolume_Private_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676730);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676731);
		NativeMethodInfoPtr__Start_b__30_0_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AudioManager>.NativeClassPtr, 100676732);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213893, XrefRangeEnd = 213911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213911, XrefRangeEnd = 213938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213938, XrefRangeEnd = 213944, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 213947, RefRangeEnd = 213957, XrefRangeStart = 213944, XrefRangeEnd = 213947, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDistorted(bool distorted, float transition = 5f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&distorted);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &transition;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDistorted_Public_Void_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 213960, RefRangeEnd = 213961, XrefRangeStart = 213957, XrefRangeEnd = 213960, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float GetVolume(EAudioType audioType, bool scaled = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&audioType);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &scaled;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVolume_Public_Single_EAudioType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 213962, RefRangeEnd = 213963, XrefRangeStart = 213961, XrefRangeEnd = 213962, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMasterVolume(float volume)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&volume);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMasterVolume_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 213970, RefRangeEnd = 213977, XrefRangeStart = 213963, XrefRangeEnd = 213970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetVolume(EAudioType type, float volume)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&type);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &volume;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetVolume_Public_Void_EAudioType_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 213985, RefRangeEnd = 213987, XrefRangeStart = 213977, XrefRangeEnd = 213985, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMainMixerVolume(float value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMainMixerVolume_Private_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213987, XrefRangeEnd = 213989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float ValueToVolume(float value)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ValueToVolume_Private_Static_Single_Single_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213989, XrefRangeEnd = 213997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AudioManager()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AudioManager>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213997, XrefRangeEnd = 213999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void _Start_b__30_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Start_b__30_0_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AudioManager(IntPtr pointer)
		: base(pointer)
	{
	}
}
