using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Audio;
using UnityEngine;

namespace Il2Cpp;

public class FlockChildSound : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_controller;

	private static readonly IntPtr NativeFieldInfoPtr__idleSounds;

	private static readonly IntPtr NativeFieldInfoPtr__idleSoundRandomChance;

	private static readonly IntPtr NativeFieldInfoPtr__flightSounds;

	private static readonly IntPtr NativeFieldInfoPtr__flightSoundRandomChance;

	private static readonly IntPtr NativeFieldInfoPtr__scareSounds;

	private static readonly IntPtr NativeFieldInfoPtr__pitchMin;

	private static readonly IntPtr NativeFieldInfoPtr__pitchMax;

	private static readonly IntPtr NativeFieldInfoPtr__volumeMin;

	private static readonly IntPtr NativeFieldInfoPtr__volumeMax;

	private static readonly IntPtr NativeFieldInfoPtr__flockChild;

	private static readonly IntPtr NativeFieldInfoPtr__audio;

	private static readonly IntPtr NativeFieldInfoPtr__hasLanded;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_PlayRandomSound_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ScareSound_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe AudioSourceController controller
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controller);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_controller)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	public unsafe Il2CppReferenceArray<AudioClip> _idleSounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleSounds);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<AudioClip>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleSounds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float _idleSoundRandomChance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleSoundRandomChance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleSoundRandomChance)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<AudioClip> _flightSounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flightSounds);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<AudioClip>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flightSounds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float _flightSoundRandomChance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flightSoundRandomChance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flightSoundRandomChance)) = num;
		}
	}

	public unsafe Il2CppReferenceArray<AudioClip> _scareSounds
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scareSounds);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<AudioClip>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scareSounds)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe float _pitchMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pitchMin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pitchMin)) = num;
		}
	}

	public unsafe float _pitchMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pitchMax);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pitchMax)) = num;
		}
	}

	public unsafe float _volumeMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMin)) = num;
		}
	}

	public unsafe float _volumeMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMax);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volumeMax)) = num;
		}
	}

	public unsafe FlockChild _flockChild
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flockChild);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FlockChild>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flockChild)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flockChild));
		}
	}

	public unsafe AudioSource _audio
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audio);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSource>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audio)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSource));
		}
	}

	public unsafe bool _hasLanded
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasLanded);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasLanded)) = flag;
		}
	}

	static FlockChildSound()
	{
		Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "FlockChildSound");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr);
		NativeFieldInfoPtr_controller = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "controller");
		NativeFieldInfoPtr__idleSounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_idleSounds");
		NativeFieldInfoPtr__idleSoundRandomChance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_idleSoundRandomChance");
		NativeFieldInfoPtr__flightSounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_flightSounds");
		NativeFieldInfoPtr__flightSoundRandomChance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_flightSoundRandomChance");
		NativeFieldInfoPtr__scareSounds = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_scareSounds");
		NativeFieldInfoPtr__pitchMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_pitchMin");
		NativeFieldInfoPtr__pitchMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_pitchMax");
		NativeFieldInfoPtr__volumeMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_volumeMin");
		NativeFieldInfoPtr__volumeMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_volumeMax");
		NativeFieldInfoPtr__flockChild = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_flockChild");
		NativeFieldInfoPtr__audio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_audio");
		NativeFieldInfoPtr__hasLanded = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, "_hasLanded");
		NativeMethodInfoPtr_Start_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, 100663591);
		NativeMethodInfoPtr_PlayRandomSound_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, 100663592);
		NativeMethodInfoPtr_ScareSound_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, 100663593);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr, 100663594);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67807, XrefRangeEnd = 67822, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67822, XrefRangeEnd = 67832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PlayRandomSound()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PlayRandomSound_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67832, XrefRangeEnd = 67841, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ScareSound()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ScareSound_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67841, XrefRangeEnd = 67842, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FlockChildSound()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FlockChildSound>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FlockChildSound(IntPtr pointer)
		: base(pointer)
	{
	}
}
