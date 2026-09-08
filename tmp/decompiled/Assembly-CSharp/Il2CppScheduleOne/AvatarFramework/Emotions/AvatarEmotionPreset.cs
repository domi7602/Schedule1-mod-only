using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.AvatarFramework.Emotions;

[System.Serializable]
public class AvatarEmotionPreset : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_PresetName;

	private static readonly System.IntPtr NativeFieldInfoPtr_FaceTexture;

	private static readonly System.IntPtr NativeFieldInfoPtr_LeftEyeRestingState;

	private static readonly System.IntPtr NativeFieldInfoPtr_RightEyeRestingState;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrowAngleChange_L;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrowAngleChange_R;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrowHeightChange_L;

	private static readonly System.IntPtr NativeFieldInfoPtr_BrowHeightChange_R;

	private static readonly System.IntPtr NativeMethodInfoPtr_Lerp_Public_Static_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string PresetName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PresetName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PresetName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Texture2D FaceTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FaceTexture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FaceTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Eye.EyeLidConfiguration LeftEyeRestingState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftEyeRestingState);
			return *(Eye.EyeLidConfiguration*)num;
		}
		set
		{
			*(Eye.EyeLidConfiguration*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LeftEyeRestingState)) = eyeLidConfiguration;
		}
	}

	public unsafe Eye.EyeLidConfiguration RightEyeRestingState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightEyeRestingState);
			return *(Eye.EyeLidConfiguration*)num;
		}
		set
		{
			*(Eye.EyeLidConfiguration*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RightEyeRestingState)) = eyeLidConfiguration;
		}
	}

	public unsafe float BrowAngleChange_L
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowAngleChange_L);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowAngleChange_L)) = num;
		}
	}

	public unsafe float BrowAngleChange_R
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowAngleChange_R);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowAngleChange_R)) = num;
		}
	}

	public unsafe float BrowHeightChange_L
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowHeightChange_L);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowHeightChange_L)) = num;
		}
	}

	public unsafe float BrowHeightChange_R
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowHeightChange_R);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BrowHeightChange_R)) = num;
		}
	}

	static AvatarEmotionPreset()
	{
		Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Emotions", "AvatarEmotionPreset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr);
		NativeFieldInfoPtr_PresetName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "PresetName");
		NativeFieldInfoPtr_FaceTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "FaceTexture");
		NativeFieldInfoPtr_LeftEyeRestingState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "LeftEyeRestingState");
		NativeFieldInfoPtr_RightEyeRestingState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "RightEyeRestingState");
		NativeFieldInfoPtr_BrowAngleChange_L = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "BrowAngleChange_L");
		NativeFieldInfoPtr_BrowAngleChange_R = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "BrowAngleChange_R");
		NativeFieldInfoPtr_BrowHeightChange_L = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "BrowHeightChange_L");
		NativeFieldInfoPtr_BrowHeightChange_R = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, "BrowHeightChange_R");
		NativeMethodInfoPtr_Lerp_Public_Static_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, 100677535);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr, 100677536);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 221759, RefRangeEnd = 221760, XrefRangeStart = 221732, XrefRangeEnd = 221759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static AvatarEmotionPreset Lerp(AvatarEmotionPreset start, AvatarEmotionPreset end, AvatarEmotionPreset neutralPreset, float lerp)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)start);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)end);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)neutralPreset);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &lerp;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Lerp_Public_Static_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_AvatarEmotionPreset_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AvatarEmotionPreset>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 221760, XrefRangeEnd = 221765, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe AvatarEmotionPreset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AvatarEmotionPreset>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AvatarEmotionPreset(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
