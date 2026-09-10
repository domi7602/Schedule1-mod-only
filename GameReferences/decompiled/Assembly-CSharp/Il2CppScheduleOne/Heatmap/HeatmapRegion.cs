using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Tiles;
using UnityEngine;

namespace Il2CppScheduleOne.Heatmap;

public class HeatmapRegion : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__textureIndex;

	private static readonly IntPtr NativeFieldInfoPtr__renderer;

	private static readonly IntPtr NativeMethodInfoPtr_Create_Public_Void_Grid_Int32_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int _textureIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__textureIndex)) = num;
		}
	}

	public unsafe MeshRenderer _renderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__renderer);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MeshRenderer>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__renderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)meshRenderer));
		}
	}

	static HeatmapRegion()
	{
		Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Heatmap", "HeatmapRegion");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr);
		NativeFieldInfoPtr__textureIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr, "_textureIndex");
		NativeFieldInfoPtr__renderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr, "_renderer");
		NativeMethodInfoPtr_Create_Public_Void_Grid_Int32_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr, 100672438);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr, 100672439);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 166832, RefRangeEnd = 166833, XrefRangeStart = 166787, XrefRangeEnd = 166832, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Create(Grid grid, int textureIndex, Material heatmapMat)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)grid);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &textureIndex;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)heatmapMat);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Create_Public_Void_Grid_Int32_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe HeatmapRegion()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HeatmapRegion>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public HeatmapRegion(IntPtr pointer)
		: base(pointer)
	{
	}
}
