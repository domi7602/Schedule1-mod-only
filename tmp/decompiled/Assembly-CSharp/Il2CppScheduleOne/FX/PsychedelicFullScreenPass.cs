using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Il2CppScheduleOne.FX;

public class PsychedelicFullScreenPass : ScriptableRenderPass
{
	private static readonly IntPtr NativeFieldInfoPtr__settings;

	private static readonly IntPtr NativeFieldInfoPtr__source;

	private static readonly IntPtr NativeFieldInfoPtr__tempTexture;

	private static readonly IntPtr NativeFieldInfoPtr__material;

	private static readonly IntPtr NativeFieldInfoPtr_BLEND_ID;

	private static readonly IntPtr NativeFieldInfoPtr_NOISE_SCALE_ID;

	private static readonly IntPtr NativeFieldInfoPtr_PAN_SPEED_ID;

	private static readonly IntPtr NativeFieldInfoPtr_DOES_BOUNCE_ID;

	private static readonly IntPtr NativeFieldInfoPtr_AMPLITUDE_ID;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Settings_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnCameraCleanup_Public_Virtual_Void_CommandBuffer_0;

	public unsafe PsychedelicFullScreenFeature.Settings _settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PsychedelicFullScreenFeature.Settings>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings));
		}
	}

	public unsafe RTHandle _source
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__source);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RTHandle>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__source)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rTHandle));
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

	public unsafe static int BLEND_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BLEND_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BLEND_ID, (void*)(&num));
		}
	}

	public unsafe static int NOISE_SCALE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_NOISE_SCALE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_NOISE_SCALE_ID, (void*)(&num));
		}
	}

	public unsafe static int PAN_SPEED_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PAN_SPEED_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PAN_SPEED_ID, (void*)(&num));
		}
	}

	public unsafe static int DOES_BOUNCE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DOES_BOUNCE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DOES_BOUNCE_ID, (void*)(&num));
		}
	}

	public unsafe static int AMPLITUDE_ID
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_AMPLITUDE_ID, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_AMPLITUDE_ID, (void*)(&num));
		}
	}

	static PsychedelicFullScreenPass()
	{
		Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.FX", "PsychedelicFullScreenPass");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr);
		NativeFieldInfoPtr__settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "_settings");
		NativeFieldInfoPtr__source = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "_source");
		NativeFieldInfoPtr__tempTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "_tempTexture");
		NativeFieldInfoPtr__material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "_material");
		NativeFieldInfoPtr_BLEND_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "BLEND_ID");
		NativeFieldInfoPtr_NOISE_SCALE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "NOISE_SCALE_ID");
		NativeFieldInfoPtr_PAN_SPEED_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "PAN_SPEED_ID");
		NativeFieldInfoPtr_DOES_BOUNCE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "DOES_BOUNCE_ID");
		NativeFieldInfoPtr_AMPLITUDE_ID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, "AMPLITUDE_ID");
		NativeMethodInfoPtr__ctor_Public_Void_Settings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, 100673735);
		NativeMethodInfoPtr_OnCameraSetup_Public_Virtual_Void_CommandBuffer_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, 100673736);
		NativeMethodInfoPtr_Execute_Public_Virtual_Void_ScriptableRenderContext_byref_RenderingData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, 100673737);
		NativeMethodInfoPtr_OnCameraCleanup_Public_Virtual_Void_CommandBuffer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr, 100673738);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179139, XrefRangeEnd = 179146, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PsychedelicFullScreenPass(PsychedelicFullScreenFeature.Settings settings)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PsychedelicFullScreenPass>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Settings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179146, XrefRangeEnd = 179155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 179155, XrefRangeEnd = 179190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnCameraCleanup(CommandBuffer cmd)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cmd);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnCameraCleanup_Public_Virtual_Void_CommandBuffer_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PsychedelicFullScreenPass(IntPtr pointer)
		: base(pointer)
	{
	}
}
