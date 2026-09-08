using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.UI;

public class CanvasScaler : MonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr__GlobalScaleFactor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnCanvasScaleFactorChanged;

	private static readonly System.IntPtr NativeFieldInfoPtr__scaleMultiplier;

	private static readonly System.IntPtr NativeFieldInfoPtr__globalScaleInfluence;

	private static readonly System.IntPtr NativeFieldInfoPtr__canvasScaler;

	private static readonly System.IntPtr NativeFieldInfoPtr__defaultReferenceResolution;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_GlobalScaleFactor_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_GlobalScaleFactor_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NormalizedCanvasScaleFactor_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RefreshScale_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetScaleFactor_Public_Static_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float _GlobalScaleFactor_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__GlobalScaleFactor_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__GlobalScaleFactor_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static Il2CppSystem.Action OnCanvasScaleFactorChanged
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnCanvasScaleFactorChanged, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnCanvasScaleFactorChanged, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe float _scaleMultiplier
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scaleMultiplier);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__scaleMultiplier)) = num;
		}
	}

	public unsafe float _globalScaleInfluence
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__globalScaleInfluence);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__globalScaleInfluence)) = num;
		}
	}

	public unsafe UnityEngine.UI.CanvasScaler _canvasScaler
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canvasScaler);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityEngine.UI.CanvasScaler>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__canvasScaler)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvasScaler));
		}
	}

	public unsafe Vector2 _defaultReferenceResolution
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultReferenceResolution);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultReferenceResolution)) = vector;
		}
	}

	public unsafe static float GlobalScaleFactor
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299338, XrefRangeEnd = 299342, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_GlobalScaleFactor_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299342, XrefRangeEnd = 299346, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_GlobalScaleFactor_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float NormalizedCanvasScaleFactor
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 299353, RefRangeEnd = 299356, XrefRangeStart = 299346, XrefRangeEnd = 299353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NormalizedCanvasScaleFactor_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CanvasScaler()
	{
		Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "CanvasScaler");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr);
		NativeFieldInfoPtr__GlobalScaleFactor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "<GlobalScaleFactor>k__BackingField");
		NativeFieldInfoPtr_OnCanvasScaleFactorChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "OnCanvasScaleFactorChanged");
		NativeFieldInfoPtr__scaleMultiplier = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "_scaleMultiplier");
		NativeFieldInfoPtr__globalScaleInfluence = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "_globalScaleInfluence");
		NativeFieldInfoPtr__canvasScaler = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "_canvasScaler");
		NativeFieldInfoPtr__defaultReferenceResolution = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, "_defaultReferenceResolution");
		NativeMethodInfoPtr_get_GlobalScaleFactor_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686417);
		NativeMethodInfoPtr_set_GlobalScaleFactor_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686418);
		NativeMethodInfoPtr_get_NormalizedCanvasScaleFactor_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686419);
		NativeMethodInfoPtr_Awake_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686420);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686421);
		NativeMethodInfoPtr_RefreshScale_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686422);
		NativeMethodInfoPtr_SetScaleFactor_Public_Static_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686423);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr, 100686424);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299356, XrefRangeEnd = 299390, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299390, XrefRangeEnd = 299410, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299410, XrefRangeEnd = 299420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RefreshScale()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RefreshScale_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 299428, RefRangeEnd = 299430, XrefRangeStart = 299420, XrefRangeEnd = 299428, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetScaleFactor(float scaleFactor)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&scaleFactor);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetScaleFactor_Public_Static_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 299430, XrefRangeEnd = 299431, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CanvasScaler()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CanvasScaler>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CanvasScaler(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
