using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Audio;

public class AmbientOneShot : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EPlayTime")]
	public enum EPlayTime
	{
		All,
		Day,
		Night
	}

	private static readonly IntPtr NativeFieldInfoPtr__volume;

	private static readonly IntPtr NativeFieldInfoPtr__playChancePerHour;

	private static readonly IntPtr NativeFieldInfoPtr__cooldownTime;

	private static readonly IntPtr NativeFieldInfoPtr__playTime;

	private static readonly IntPtr NativeFieldInfoPtr__minDistanceFromCameraToPlay;

	private static readonly IntPtr NativeFieldInfoPtr__maxDistanceFromCameraToPlay;

	private static readonly IntPtr NativeFieldInfoPtr__canPlayWhilePlayerInSewer;

	private static readonly IntPtr NativeFieldInfoPtr__timeSinceLastPlay;

	private static readonly IntPtr NativeFieldInfoPtr__audioSource;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnUncappedMinPass_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Play_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe float _volume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__volume)) = num;
		}
	}

	public unsafe float _playChancePerHour
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playChancePerHour);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playChancePerHour)) = num;
		}
	}

	public unsafe int _cooldownTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cooldownTime);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cooldownTime)) = num;
		}
	}

	public unsafe EPlayTime _playTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playTime);
			return *(EPlayTime*)num;
		}
		set
		{
			*(EPlayTime*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playTime)) = ePlayTime;
		}
	}

	public unsafe float _minDistanceFromCameraToPlay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minDistanceFromCameraToPlay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minDistanceFromCameraToPlay)) = num;
		}
	}

	public unsafe float _maxDistanceFromCameraToPlay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDistanceFromCameraToPlay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDistanceFromCameraToPlay)) = num;
		}
	}

	public unsafe bool _canPlayWhilePlayerInSewer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canPlayWhilePlayerInSewer);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canPlayWhilePlayerInSewer)) = flag;
		}
	}

	public unsafe int _timeSinceLastPlay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeSinceLastPlay);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeSinceLastPlay)) = num;
		}
	}

	public unsafe AudioSourceController _audioSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSource);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AudioSourceController>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__audioSource)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)audioSourceController));
		}
	}

	static AmbientOneShot()
	{
		Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Audio", "AmbientOneShot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr);
		NativeFieldInfoPtr__volume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_volume");
		NativeFieldInfoPtr__playChancePerHour = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_playChancePerHour");
		NativeFieldInfoPtr__cooldownTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_cooldownTime");
		NativeFieldInfoPtr__playTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_playTime");
		NativeFieldInfoPtr__minDistanceFromCameraToPlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_minDistanceFromCameraToPlay");
		NativeFieldInfoPtr__maxDistanceFromCameraToPlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_maxDistanceFromCameraToPlay");
		NativeFieldInfoPtr__canPlayWhilePlayerInSewer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_canPlayWhilePlayerInSewer");
		NativeFieldInfoPtr__timeSinceLastPlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_timeSinceLastPlay");
		NativeFieldInfoPtr__audioSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, "_audioSource");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, 100676699);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, 100676700);
		NativeMethodInfoPtr_OnUncappedMinPass_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, 100676701);
		NativeMethodInfoPtr_Play_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, 100676702);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr, 100676703);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213622, XrefRangeEnd = 213626, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213626, XrefRangeEnd = 213639, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213639, XrefRangeEnd = 213670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUncappedMinPass()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUncappedMinPass_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213670, XrefRangeEnd = 213672, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Play()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Play_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 213672, XrefRangeEnd = 213673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AmbientOneShot()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AmbientOneShot>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AmbientOneShot(IntPtr pointer)
		: base(pointer)
	{
	}
}
