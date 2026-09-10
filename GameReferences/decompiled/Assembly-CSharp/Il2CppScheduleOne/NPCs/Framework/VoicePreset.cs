using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core;

namespace Il2CppScheduleOne.NPCs.Framework;

public class VoicePreset : ValueProviderScriptableObject<Voice>
{
	private static readonly IntPtr NativeFieldInfoPtr_value;

	private static readonly IntPtr NativeMethodInfoPtr_GetValue_Public_Virtual_Voice_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Voice value
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_value);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Voice>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_value)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)voice));
		}
	}

	static VoicePreset()
	{
		Il2CppClassPointerStore<VoicePreset>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.NPCs.Framework", "VoicePreset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VoicePreset>.NativeClassPtr);
		NativeFieldInfoPtr_value = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VoicePreset>.NativeClassPtr, "value");
		NativeMethodInfoPtr_GetValue_Public_Virtual_Voice_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VoicePreset>.NativeClassPtr, 100682836);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VoicePreset>.NativeClassPtr, 100682837);
	}

	[CallerCount(24)]
	[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override Voice GetValue()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetValue_Public_Virtual_Voice_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Voice>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 272314, XrefRangeEnd = 272317, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VoicePreset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VoicePreset>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VoicePreset(IntPtr pointer)
		: base(pointer)
	{
	}
}
