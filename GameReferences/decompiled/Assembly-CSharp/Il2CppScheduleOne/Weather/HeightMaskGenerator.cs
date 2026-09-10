using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppScheduleOne.Weather;

public class HeightMaskGenerator : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__maskShader;

	private static readonly IntPtr NativeFieldInfoPtr__size;

	private static readonly IntPtr NativeFieldInfoPtr__resolution;

	private static readonly IntPtr NativeFieldInfoPtr__minMaxHeight;

	private static readonly IntPtr NativeFieldInfoPtr__heightmapLayerMask;

	private static readonly IntPtr NativeFieldInfoPtr__debugTileSize;

	private static readonly IntPtr NativeFieldInfoPtr__heightTexture;

	private static readonly IntPtr NativeFieldInfoPtr__debugMaterial;

	private static readonly IntPtr NativeFieldInfoPtr__kernal;

	private static readonly IntPtr NativeFieldInfoPtr__tileSize;

	private static readonly IntPtr NativeFieldInfoPtr__tileHalfSize;

	private static readonly IntPtr NativeFieldInfoPtr__origin;

	private static readonly IntPtr NativeFieldInfoPtr__heightBuffer;

	private static readonly IntPtr NativeMethodInfoPtr_InitialiseMaskMap_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GenerateMaskMap_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GenerateHeightMapDebug_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Dispose_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ComputeShader _maskShader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maskShader);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ComputeShader>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maskShader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)computeShader));
		}
	}

	public unsafe float _size
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__size)) = num;
		}
	}

	public unsafe int _resolution
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__resolution);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__resolution)) = num;
		}
	}

	public unsafe Vector2 _minMaxHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minMaxHeight);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minMaxHeight)) = vector;
		}
	}

	public unsafe LayerMask _heightmapLayerMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightmapLayerMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightmapLayerMask)) = layerMask;
		}
	}

	public unsafe float _debugTileSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugTileSize);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugTileSize)) = num;
		}
	}

	public unsafe RenderTexture _heightTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightTexture);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RenderTexture>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)renderTexture));
		}
	}

	public unsafe Material _debugMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugMaterial);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__debugMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe int _kernal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__kernal);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__kernal)) = num;
		}
	}

	public unsafe float _tileSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileSize);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileSize)) = num;
		}
	}

	public unsafe float _tileHalfSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileHalfSize);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileHalfSize)) = num;
		}
	}

	public unsafe Vector3 _origin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__origin);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__origin)) = vector;
		}
	}

	public unsafe ComputeBuffer _heightBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightBuffer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ComputeBuffer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__heightBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)computeBuffer));
		}
	}

	static HeightMaskGenerator()
	{
		Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Weather", "HeightMaskGenerator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr);
		NativeFieldInfoPtr__maskShader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_maskShader");
		NativeFieldInfoPtr__size = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_size");
		NativeFieldInfoPtr__resolution = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_resolution");
		NativeFieldInfoPtr__minMaxHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_minMaxHeight");
		NativeFieldInfoPtr__heightmapLayerMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_heightmapLayerMask");
		NativeFieldInfoPtr__debugTileSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_debugTileSize");
		NativeFieldInfoPtr__heightTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_heightTexture");
		NativeFieldInfoPtr__debugMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_debugMaterial");
		NativeFieldInfoPtr__kernal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_kernal");
		NativeFieldInfoPtr__tileSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_tileSize");
		NativeFieldInfoPtr__tileHalfSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_tileHalfSize");
		NativeFieldInfoPtr__origin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_origin");
		NativeFieldInfoPtr__heightBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, "_heightBuffer");
		NativeMethodInfoPtr_InitialiseMaskMap_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685653);
		NativeMethodInfoPtr_GenerateMaskMap_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685654);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685655);
		NativeMethodInfoPtr_GenerateHeightMapDebug_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685656);
		NativeMethodInfoPtr_Dispose_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685657);
		NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685658);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr, 100685659);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 292473, RefRangeEnd = 292474, XrefRangeStart = 292442, XrefRangeEnd = 292473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitialiseMaskMap()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitialiseMaskMap_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 292498, RefRangeEnd = 292499, XrefRangeStart = 292474, XrefRangeEnd = 292498, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GenerateMaskMap()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateMaskMap_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292499, XrefRangeEnd = 292506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292506, XrefRangeEnd = 292508, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GenerateHeightMapDebug()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateHeightMapDebug_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292508, XrefRangeEnd = 292513, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Dispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292513, XrefRangeEnd = 292521, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDrawGizmos()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDrawGizmos_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 292521, XrefRangeEnd = 292522, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HeightMaskGenerator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HeightMaskGenerator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HeightMaskGenerator(IntPtr pointer)
		: base(pointer)
	{
	}
}
