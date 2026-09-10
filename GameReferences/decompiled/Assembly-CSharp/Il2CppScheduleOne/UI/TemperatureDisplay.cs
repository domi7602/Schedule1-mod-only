using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppTMPro;
using UnityEngine;

namespace Il2CppScheduleOne.UI;

public class TemperatureDisplay : MonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr_MaxCameraDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_MinCameraDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_FadeInDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_FadeOutDistance;

	private static readonly System.IntPtr NativeFieldInfoPtr_UseColor;

	private static readonly System.IntPtr NativeFieldInfoPtr__temperatureColorGradient;

	private static readonly System.IntPtr NativeFieldInfoPtr__label;

	private static readonly System.IntPtr NativeFieldInfoPtr__getCelsiusTemperature;

	private static readonly System.IntPtr NativeFieldInfoPtr__getIsVisible;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateCanvas_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetTemperatureGetter_Public_Void_Func_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetVisibilityGetter_Public_Void_Func_1_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetEnabled_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static float MaxCameraDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaxCameraDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaxCameraDistance, (void*)(&num));
		}
	}

	public unsafe static float MinCameraDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MinCameraDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MinCameraDistance, (void*)(&num));
		}
	}

	public unsafe static float FadeInDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeInDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeInDistance, (void*)(&num));
		}
	}

	public unsafe static float FadeOutDistance
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_FadeOutDistance, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_FadeOutDistance, (void*)(&num));
		}
	}

	public unsafe bool UseColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseColor);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UseColor)) = flag;
		}
	}

	public unsafe Gradient _temperatureColorGradient
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__temperatureColorGradient);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Gradient>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__temperatureColorGradient)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradient));
		}
	}

	public unsafe TextMeshPro _label
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__label);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TextMeshPro>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__label)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshPro));
		}
	}

	public unsafe Il2CppSystem.Func<float> _getCelsiusTemperature
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__getCelsiusTemperature);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__getCelsiusTemperature)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
		}
	}

	public unsafe Il2CppSystem.Func<bool> _getIsVisible
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__getIsVisible);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<bool>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__getIsVisible)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
		}
	}

	static TemperatureDisplay()
	{
		Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "TemperatureDisplay");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr);
		NativeFieldInfoPtr_MaxCameraDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "MaxCameraDistance");
		NativeFieldInfoPtr_MinCameraDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "MinCameraDistance");
		NativeFieldInfoPtr_FadeInDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "FadeInDistance");
		NativeFieldInfoPtr_FadeOutDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "FadeOutDistance");
		NativeFieldInfoPtr_UseColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "UseColor");
		NativeFieldInfoPtr__temperatureColorGradient = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "_temperatureColorGradient");
		NativeFieldInfoPtr__label = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "_label");
		NativeFieldInfoPtr__getCelsiusTemperature = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "_getCelsiusTemperature");
		NativeFieldInfoPtr__getIsVisible = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, "_getIsVisible");
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687427);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687428);
		NativeMethodInfoPtr_UpdateCanvas_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687429);
		NativeMethodInfoPtr_SetTemperatureGetter_Public_Void_Func_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687430);
		NativeMethodInfoPtr_SetVisibilityGetter_Public_Void_Func_1_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687431);
		NativeMethodInfoPtr_SetEnabled_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687432);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr, 100687433);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 309107, XrefRangeEnd = 309108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 309150, RefRangeEnd = 309151, XrefRangeStart = 309108, XrefRangeEnd = 309150, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateCanvas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCanvas_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetTemperatureGetter(Il2CppSystem.Func<float> getCelsiusTemperature)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)getCelsiusTemperature);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetTemperatureGetter_Public_Void_Func_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetVisibilityGetter(Il2CppSystem.Func<bool> getIsVisible)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)getIsVisible);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetVisibilityGetter_Public_Void_Func_1_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 230566, RefRangeEnd = 230573, XrefRangeStart = 230566, XrefRangeEnd = 230573, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetEnabled(bool enabled)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&enabled);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetEnabled_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TemperatureDisplay()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TemperatureDisplay>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TemperatureDisplay(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
