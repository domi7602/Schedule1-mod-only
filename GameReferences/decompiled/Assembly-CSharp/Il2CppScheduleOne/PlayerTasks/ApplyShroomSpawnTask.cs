using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Il2CppScheduleOne.PlayerTasks;

public class ApplyShroomSpawnTask : Task
{
	[OriginalName("Assembly-CSharp.dll", "", "EStage")]
	public enum EStage
	{
		BreakUpChunks,
		MixIntoSoil
	}

	private static readonly IntPtr NativeFieldInfoPtr_DistanceBetweenMixes;

	private static readonly IntPtr NativeFieldInfoPtr_MixRadius;

	private static readonly IntPtr NativeFieldInfoPtr_MaskTextureSize;

	private static readonly IntPtr NativeFieldInfoPtr_SmallChunkCount;

	private static readonly IntPtr NativeFieldInfoPtr__spawnDefinition;

	private static readonly IntPtr NativeFieldInfoPtr__mushroomBed;

	private static readonly IntPtr NativeFieldInfoPtr__baseSpawnChunk;

	private static readonly IntPtr NativeFieldInfoPtr__currentStage;

	private static readonly IntPtr NativeFieldInfoPtr__mixProjector;

	private static readonly IntPtr NativeFieldInfoPtr__lastMixPosition;

	private static readonly IntPtr NativeFieldInfoPtr__maskingTexture;

	private static readonly IntPtr NativeFieldInfoPtr__mixedChunks;

	private static readonly IntPtr NativeFieldInfoPtr__mixMouseUp;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ShroomSpawnDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Success_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateProgression_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetCursorHoverOnSoil_Private_Boolean_byref_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_TriggerMix_Private_Void_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_PaintMask_Private_Void_Int32_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateMaskTexture_Private_Texture2D_0;

	public unsafe static float DistanceBetweenMixes
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DistanceBetweenMixes, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DistanceBetweenMixes, (void*)(&num));
		}
	}

	public unsafe static float MixRadius
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MixRadius, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MixRadius, (void*)(&num));
		}
	}

	public unsafe static int MaskTextureSize
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MaskTextureSize, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MaskTextureSize, (void*)(&num));
		}
	}

	public unsafe static int SmallChunkCount
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SmallChunkCount, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SmallChunkCount, (void*)(&num));
		}
	}

	public unsafe ShroomSpawnDefinition _spawnDefinition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnDefinition);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomSpawnDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnDefinition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomSpawnDefinition));
		}
	}

	public unsafe MushroomBed _mushroomBed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mushroomBed);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<MushroomBed>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mushroomBed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomBed));
		}
	}

	public unsafe SpawnChunk _baseSpawnChunk
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baseSpawnChunk);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpawnChunk>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__baseSpawnChunk)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spawnChunk));
		}
	}

	public unsafe EStage _currentStage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentStage);
			return *(EStage*)num;
		}
		set
		{
			*(EStage*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentStage)) = eStage;
		}
	}

	public unsafe DecalProjector _mixProjector
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixProjector);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DecalProjector>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixProjector)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)decalProjector));
		}
	}

	public unsafe Vector3 _lastMixPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastMixPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lastMixPosition)) = vector;
		}
	}

	public unsafe Texture2D _maskingTexture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maskingTexture);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maskingTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe List<SpawnChunk> _mixedChunks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixedChunks);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<SpawnChunk>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixedChunks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool _mixMouseUp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixMouseUp);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mixMouseUp)) = flag;
		}
	}

	static ApplyShroomSpawnTask()
	{
		Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.PlayerTasks", "ApplyShroomSpawnTask");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr);
		NativeFieldInfoPtr_DistanceBetweenMixes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "DistanceBetweenMixes");
		NativeFieldInfoPtr_MixRadius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "MixRadius");
		NativeFieldInfoPtr_MaskTextureSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "MaskTextureSize");
		NativeFieldInfoPtr_SmallChunkCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "SmallChunkCount");
		NativeFieldInfoPtr__spawnDefinition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_spawnDefinition");
		NativeFieldInfoPtr__mushroomBed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_mushroomBed");
		NativeFieldInfoPtr__baseSpawnChunk = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_baseSpawnChunk");
		NativeFieldInfoPtr__currentStage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_currentStage");
		NativeFieldInfoPtr__mixProjector = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_mixProjector");
		NativeFieldInfoPtr__lastMixPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_lastMixPosition");
		NativeFieldInfoPtr__maskingTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_maskingTexture");
		NativeFieldInfoPtr__mixedChunks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_mixedChunks");
		NativeFieldInfoPtr__mixMouseUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, "_mixMouseUp");
		NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ShroomSpawnDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668291);
		NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668292);
		NativeMethodInfoPtr_Success_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668293);
		NativeMethodInfoPtr_Update_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668294);
		NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668295);
		NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668296);
		NativeMethodInfoPtr_UpdateProgression_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668297);
		NativeMethodInfoPtr_GetCursorHoverOnSoil_Private_Boolean_byref_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668298);
		NativeMethodInfoPtr_TriggerMix_Private_Void_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668299);
		NativeMethodInfoPtr_PaintMask_Private_Void_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668300);
		NativeMethodInfoPtr_CreateMaskTexture_Private_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr, 100668301);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 118382, RefRangeEnd = 118383, XrefRangeStart = 118274, XrefRangeEnd = 118382, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ApplyShroomSpawnTask(MushroomBed mushroomBed, ShroomSpawnDefinition spawnDefinition)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ApplyShroomSpawnTask>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mushroomBed);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spawnDefinition);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_MushroomBed_ShroomSpawnDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118383, XrefRangeEnd = 118418, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void StopTask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StopTask_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118418, XrefRangeEnd = 118445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Success()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Success_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118445, XrefRangeEnd = 118452, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118452, XrefRangeEnd = 118460, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 118486, RefRangeEnd = 118488, XrefRangeStart = 118460, XrefRangeEnd = 118486, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateInstructionText()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateInstructionText_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 118495, RefRangeEnd = 118496, XrefRangeStart = 118488, XrefRangeEnd = 118495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateProgression()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateProgression_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 118511, RefRangeEnd = 118512, XrefRangeStart = 118496, XrefRangeEnd = 118511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GetCursorHoverOnSoil(out Vector3 hitPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)Unsafe.AsPointer(ref hitPoint);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCursorHoverOnSoil_Private_Boolean_byref_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118512, XrefRangeEnd = 118556, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void TriggerMix(Vector3 mixPoint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&mixPoint);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TriggerMix_Private_Void_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 118568, RefRangeEnd = 118569, XrefRangeStart = 118556, XrefRangeEnd = 118568, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PaintMask(int x, int y)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&x);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &y;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PaintMask_Private_Void_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 118569, XrefRangeEnd = 118579, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Texture2D CreateMaskTexture()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateMaskTexture_Private_Texture2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	public ApplyShroomSpawnTask(IntPtr pointer)
		: base(pointer)
	{
	}
}
