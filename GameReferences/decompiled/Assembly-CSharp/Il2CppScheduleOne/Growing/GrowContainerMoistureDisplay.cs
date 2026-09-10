using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Il2CppScheduleOne.Growing;

public class GrowContainerMoistureDisplay : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_MaxCameraDistance;

	private static readonly IntPtr NativeFieldInfoPtr_MinCameraDistance;

	private static readonly IntPtr NativeFieldInfoPtr_FadeInDistance;

	private static readonly IntPtr NativeFieldInfoPtr_FadeOutDistance;

	private static readonly IntPtr NativeFieldInfoPtr_SnapToRightAngles;

	private static readonly IntPtr NativeFieldInfoPtr_GrowContainer;

	private static readonly IntPtr NativeFieldInfoPtr_WaterCanvasContainer;

	private static readonly IntPtr NativeFieldInfoPtr_WaterLevelCanvas;

	private static readonly IntPtr NativeFieldInfoPtr_WaterLevelCanvasGroup;

	private static readonly IntPtr NativeFieldInfoPtr_WaterLevelSlider;

	private static readonly IntPtr NativeFieldInfoPtr_NoWaterIcon;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateCanvas_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateCanvasContents_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

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

	public unsafe bool SnapToRightAngles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SnapToRightAngles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SnapToRightAngles)) = flag;
		}
	}

	public unsafe GrowContainer GrowContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GrowContainer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GrowContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)growContainer));
		}
	}

	public unsafe Transform WaterCanvasContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterCanvasContainer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterCanvasContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe Canvas WaterLevelCanvas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelCanvas);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Canvas>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelCanvas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas));
		}
	}

	public unsafe CanvasGroup WaterLevelCanvasGroup
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelCanvasGroup);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CanvasGroup>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelCanvasGroup)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvasGroup));
		}
	}

	public unsafe Slider WaterLevelSlider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelSlider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WaterLevelSlider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slider));
		}
	}

	public unsafe GameObject NoWaterIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoWaterIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NoWaterIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	static GrowContainerMoistureDisplay()
	{
		Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "GrowContainerMoistureDisplay");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr);
		NativeFieldInfoPtr_MaxCameraDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "MaxCameraDistance");
		NativeFieldInfoPtr_MinCameraDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "MinCameraDistance");
		NativeFieldInfoPtr_FadeInDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "FadeInDistance");
		NativeFieldInfoPtr_FadeOutDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "FadeOutDistance");
		NativeFieldInfoPtr_SnapToRightAngles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "SnapToRightAngles");
		NativeFieldInfoPtr_GrowContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "GrowContainer");
		NativeFieldInfoPtr_WaterCanvasContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "WaterCanvasContainer");
		NativeFieldInfoPtr_WaterLevelCanvas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "WaterLevelCanvas");
		NativeFieldInfoPtr_WaterLevelCanvasGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "WaterLevelCanvasGroup");
		NativeFieldInfoPtr_WaterLevelSlider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "WaterLevelSlider");
		NativeFieldInfoPtr_NoWaterIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, "NoWaterIcon");
		NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, 100678475);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, 100678476);
		NativeMethodInfoPtr_UpdateCanvas_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, 100678477);
		NativeMethodInfoPtr_UpdateCanvasContents_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, 100678478);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr, 100678479);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 229661, RefRangeEnd = 229663, XrefRangeStart = 229658, XrefRangeEnd = 229661, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 229663, XrefRangeEnd = 229664, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 229732, RefRangeEnd = 229733, XrefRangeStart = 229664, XrefRangeEnd = 229732, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateCanvas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateCanvas_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 229736, RefRangeEnd = 229738, XrefRangeStart = 229733, XrefRangeEnd = 229736, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void UpdateCanvasContents()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_UpdateCanvasContents_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GrowContainerMoistureDisplay()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GrowContainerMoistureDisplay>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GrowContainerMoistureDisplay(IntPtr pointer)
		: base(pointer)
	{
	}
}
