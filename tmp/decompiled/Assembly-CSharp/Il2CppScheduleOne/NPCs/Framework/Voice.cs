using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.VoiceOver;
using Il2CppSystem;

namespace Il2CppScheduleOne.NPCs.Framework;

[System.Serializable]
public class Voice : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_VoiceDatabase;

	private static readonly System.IntPtr NativeFieldInfoPtr_VoicePitch;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCopy_Public_Voice_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe VODatabase VoiceDatabase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoiceDatabase);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<VODatabase>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoiceDatabase)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vODatabase));
		}
	}

	public unsafe float VoicePitch
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoicePitch);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VoicePitch)) = num;
		}
	}

	static Voice()
	{
		Il2CppClassPointerStore<Voice>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "Voice");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Voice>.NativeClassPtr);
		NativeFieldInfoPtr_VoiceDatabase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Voice>.NativeClassPtr, "VoiceDatabase");
		NativeFieldInfoPtr_VoicePitch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Voice>.NativeClassPtr, "VoicePitch");
		NativeMethodInfoPtr_GetCopy_Public_Voice_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Voice>.NativeClassPtr, 100682834);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Voice>.NativeClassPtr, 100682835);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272309, XrefRangeEnd = 272314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Voice GetCopy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCopy_Public_Voice_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Voice>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Voice()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Voice>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Voice(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
