using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Il2Cpp;

public class DebugPanelAnimation : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "AnimationType")]
	public enum AnimationType
	{
		Alpha,
		Scale
	}

	private static readonly IntPtr NativeFieldInfoPtr_targetImage;

	private static readonly IntPtr NativeFieldInfoPtr_animationType;

	private static readonly IntPtr NativeFieldInfoPtr_alphaCurve;

	private static readonly IntPtr NativeFieldInfoPtr_duration;

	private static readonly IntPtr NativeFieldInfoPtr_timer;

	private static readonly IntPtr NativeFieldInfoPtr_isPlaying;

	private static readonly IntPtr NativeFieldInfoPtr_originalColor;

	private static readonly IntPtr NativeFieldInfoPtr_originalScale;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Play_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Image targetImage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetImage);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_targetImage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe AnimationType animationType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationType);
			return *(AnimationType*)num;
		}
		set
		{
			*(AnimationType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationType)) = animationType;
		}
	}

	public unsafe AnimationCurve alphaCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alphaCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alphaCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe float duration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_duration);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_duration)) = num;
		}
	}

	public unsafe float timer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timer);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timer)) = num;
		}
	}

	public unsafe bool isPlaying
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isPlaying);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isPlaying)) = flag;
		}
	}

	public unsafe Color originalColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalColor);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalColor)) = color;
		}
	}

	public unsafe Vector3 originalScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalScale);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalScale)) = vector;
		}
	}

	static DebugPanelAnimation()
	{
		Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "DebugPanelAnimation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr);
		NativeFieldInfoPtr_targetImage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "targetImage");
		NativeFieldInfoPtr_animationType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "animationType");
		NativeFieldInfoPtr_alphaCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "alphaCurve");
		NativeFieldInfoPtr_duration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "duration");
		NativeFieldInfoPtr_timer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "timer");
		NativeFieldInfoPtr_isPlaying = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "isPlaying");
		NativeFieldInfoPtr_originalColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "originalColor");
		NativeFieldInfoPtr_originalScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, "originalScale");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, 100663303);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, 100663304);
		NativeMethodInfoPtr_Play_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, 100663305);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr, 100663306);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64820, XrefRangeEnd = 64830, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64830, XrefRangeEnd = 64840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64840, XrefRangeEnd = 64844, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Play()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Play_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64844, XrefRangeEnd = 64847, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DebugPanelAnimation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DebugPanelAnimation>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DebugPanelAnimation(IntPtr pointer)
		: base(pointer)
	{
	}
}
