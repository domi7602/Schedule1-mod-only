using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Growing;
using Il2CppScheduleOne.PlayerTasks;
using Il2CppScheduleOne.Product;
using UnityEngine.Rendering.Universal;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class ShroomSpawnDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr__ColonyPrefab_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__Shroom_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__ChunkPrefab_k__BackingField;

	private static readonly IntPtr NativeFieldInfoPtr__MixTaskProjectorPrefab_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_ColonyPrefab_Public_get_ShroomColony_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ColonyPrefab_Private_set_Void_ShroomColony_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Shroom_Public_get_ShroomDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Shroom_Private_set_Void_ShroomDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ChunkPrefab_Public_get_SpawnChunk_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_ChunkPrefab_Private_set_Void_SpawnChunk_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MixTaskProjectorPrefab_Public_get_DecalProjector_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MixTaskProjectorPrefab_Private_set_Void_DecalProjector_0;

	private static readonly IntPtr NativeMethodInfoPtr_ValidateDefinition_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ShroomColony _ColonyPrefab_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ColonyPrefab_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomColony>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ColonyPrefab_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomColony));
		}
	}

	public unsafe ShroomDefinition _Shroom_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Shroom_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Shroom_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomDefinition));
		}
	}

	public unsafe SpawnChunk _ChunkPrefab_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ChunkPrefab_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpawnChunk>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ChunkPrefab_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spawnChunk));
		}
	}

	public unsafe DecalProjector _MixTaskProjectorPrefab_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MixTaskProjectorPrefab_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DecalProjector>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MixTaskProjectorPrefab_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)decalProjector));
		}
	}

	public unsafe ShroomColony ColonyPrefab
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38421, RefRangeEnd = 38424, XrefRangeStart = 38421, XrefRangeEnd = 38424, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ColonyPrefab_Public_get_ShroomColony_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomColony>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ColonyPrefab_Private_set_Void_ShroomColony_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe ShroomDefinition Shroom
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Shroom_Public_get_ShroomDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomDefinition>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Shroom_Private_set_Void_ShroomDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe SpawnChunk ChunkPrefab
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ChunkPrefab_Public_get_SpawnChunk_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpawnChunk>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 38427, RefRangeEnd = 38428, XrefRangeStart = 38427, XrefRangeEnd = 38428, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_ChunkPrefab_Private_set_Void_SpawnChunk_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe DecalProjector MixTaskProjectorPrefab
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MixTaskProjectorPrefab_Public_get_DecalProjector_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DecalProjector>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MixTaskProjectorPrefab_Private_set_Void_DecalProjector_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ShroomSpawnDefinition()
	{
		Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "ShroomSpawnDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr);
		NativeFieldInfoPtr__ColonyPrefab_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, "<ColonyPrefab>k__BackingField");
		NativeFieldInfoPtr__Shroom_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, "<Shroom>k__BackingField");
		NativeFieldInfoPtr__ChunkPrefab_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, "<ChunkPrefab>k__BackingField");
		NativeFieldInfoPtr__MixTaskProjectorPrefab_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, "<MixTaskProjectorPrefab>k__BackingField");
		NativeMethodInfoPtr_get_ColonyPrefab_Public_get_ShroomColony_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672651);
		NativeMethodInfoPtr_set_ColonyPrefab_Private_set_Void_ShroomColony_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672652);
		NativeMethodInfoPtr_get_Shroom_Public_get_ShroomDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672653);
		NativeMethodInfoPtr_set_Shroom_Private_set_Void_ShroomDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672654);
		NativeMethodInfoPtr_get_ChunkPrefab_Public_get_SpawnChunk_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672655);
		NativeMethodInfoPtr_set_ChunkPrefab_Private_set_Void_SpawnChunk_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672656);
		NativeMethodInfoPtr_get_MixTaskProjectorPrefab_Public_get_DecalProjector_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672657);
		NativeMethodInfoPtr_set_MixTaskProjectorPrefab_Private_set_Void_DecalProjector_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672658);
		NativeMethodInfoPtr_ValidateDefinition_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672659);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr, 100672660);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ValidateDefinition()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ValidateDefinition_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 166837, RefRangeEnd = 166838, XrefRangeStart = 166837, XrefRangeEnd = 166838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShroomSpawnDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShroomSpawnDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ShroomSpawnDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
