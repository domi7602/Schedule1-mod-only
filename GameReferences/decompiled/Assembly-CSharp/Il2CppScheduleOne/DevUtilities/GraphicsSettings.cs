using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.DevUtilities;

[System.Serializable]
public class GraphicsSettings : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp.dll", "", "EAntiAliasingMode")]
	public enum EAntiAliasingMode
	{
		Off,
		FXAA,
		SMAA
	}

	[OriginalName("Assembly-CSharp.dll", "", "EGraphicsQuality")]
	public enum EGraphicsQuality
	{
		Low,
		Medium,
		High,
		Ultra
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_GraphicsQuality;

	private static readonly System.IntPtr NativeFieldInfoPtr_AntiAliasingMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_FOV;

	private static readonly System.IntPtr NativeFieldInfoPtr_SSAO;

	private static readonly System.IntPtr NativeFieldInfoPtr_GodRays;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe EGraphicsQuality GraphicsQuality
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GraphicsQuality);
			return *(EGraphicsQuality*)num;
		}
		set
		{
			*(EGraphicsQuality*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GraphicsQuality)) = eGraphicsQuality;
		}
	}

	public unsafe EAntiAliasingMode AntiAliasingMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AntiAliasingMode);
			return *(EAntiAliasingMode*)num;
		}
		set
		{
			*(EAntiAliasingMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AntiAliasingMode)) = eAntiAliasingMode;
		}
	}

	public unsafe float FOV
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FOV);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FOV)) = num;
		}
	}

	public unsafe bool SSAO
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SSAO);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SSAO)) = flag;
		}
	}

	public unsafe bool GodRays
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GodRays);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_GodRays)) = flag;
		}
	}

	static GraphicsSettings()
	{
		Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "GraphicsSettings");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr);
		NativeFieldInfoPtr_GraphicsQuality = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, "GraphicsQuality");
		NativeFieldInfoPtr_AntiAliasingMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, "AntiAliasingMode");
		NativeFieldInfoPtr_FOV = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, "FOV");
		NativeFieldInfoPtr_SSAO = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, "SSAO");
		NativeFieldInfoPtr_GodRays = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, "GodRays");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr, 100675252);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GraphicsSettings()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GraphicsSettings>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GraphicsSettings(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
