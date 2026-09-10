using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Il2Cpp;

public class ScheduleOneFogPass : ScriptableRenderPass
{
	private static readonly IntPtr NativeFieldInfoPtr__material;

	private static readonly IntPtr NativeFieldInfoPtr__cameraColorTarget;

	private static readonly IntPtr NativeFieldInfoPtr__tempTexture;

	private static readonly IntPtr NativeFieldInfoPtr__color;

	private static readonly IntPtr NativeFieldInfoPtr__start;

	private static readonly IntPtr NativeFieldInfoPtr__end;

	private static readonly IntPtr NativeFieldInfoPtr__density;

	private static readonly IntPtr NativeFieldInfoPtr__blurStrength;

	private static readonly IntPtr NativeFieldInfoPtr__startHeightFade;

	private static readonly IntPtr NativeFieldInfoPtr__endHeightFade;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_Setup_Public_Void_Settings_RTHandle_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Dispose_Public_Void_0;

	public unsafe Material _material
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__material);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__material)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe RTHandle _cameraColorTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cameraColorTarget);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cameraColorTarget)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
		}
	}

	public unsafe RTHandle _tempTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tempTexture);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tempTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
		}
	}

	public unsafe Color _color
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__color);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__color)) = color;
		}
	}

	public unsafe float _start
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__start);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__start)) = num;
		}
	}

	public unsafe float _end
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__end);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__end)) = num;
		}
	}

	public unsafe float _density
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__density);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__density)) = num;
		}
	}

	public unsafe float _blurStrength
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__blurStrength);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__blurStrength)) = num;
		}
	}

	public unsafe float _startHeightFade
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startHeightFade);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startHeightFade)) = num;
		}
	}

	public unsafe float _endHeightFade
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__endHeightFade);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__endHeightFade)) = num;
		}
	}

	static ScheduleOneFogPass()
	{
		Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "ScheduleOneFogPass");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr);
		NativeFieldInfoPtr__material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_material");
		NativeFieldInfoPtr__cameraColorTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_cameraColorTarget");
		NativeFieldInfoPtr__tempTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_tempTexture");
		NativeFieldInfoPtr__color = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_color");
		NativeFieldInfoPtr__start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_start");
		NativeFieldInfoPtr__end = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_end");
		NativeFieldInfoPtr__density = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_density");
		NativeFieldInfoPtr__blurStrength = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_blurStrength");
		NativeFieldInfoPtr__startHeightFade = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_startHeightFade");
		NativeFieldInfoPtr__endHeightFade = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, "_endHeightFade");
		NativeMethodInfoPtr__ctor_Public_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, 100663343);
		NativeMethodInfoPtr_Setup_Public_Void_Settings_RTHandle_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, 100663344);
		NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, 100663345);
		NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, 100663346);
		NativeMethodInfoPtr_Dispose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr, 100663347);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65129, XrefRangeEnd = 65134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ScheduleOneFogPass(Material material)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ScheduleOneFogPass>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65134, XrefRangeEnd = 65136, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Setup(ScheduleOneFogFeature.Settings settings, RTHandle cameraColorTarget)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cameraColorTarget);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Setup_Public_Void_Settings_RTHandle_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65136, XrefRangeEnd = 65143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cmd);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65143, XrefRangeEnd = 65200, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&context);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)renderingData));
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 65200, XrefRangeEnd = 65201, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Dispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ScheduleOneFogPass(IntPtr pointer)
		: base(pointer)
	{
	}
}
