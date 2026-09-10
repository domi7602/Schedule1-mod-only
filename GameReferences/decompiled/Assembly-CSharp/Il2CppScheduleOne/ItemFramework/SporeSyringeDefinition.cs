using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class SporeSyringeDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr__SpawnDefinition_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_SpawnDefinition_Public_get_ShroomSpawnDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_SpawnDefinition_Private_set_Void_ShroomSpawnDefinition_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ShroomSpawnDefinition _SpawnDefinition_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SpawnDefinition_k__BackingField);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomSpawnDefinition>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SpawnDefinition_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shroomSpawnDefinition));
		}
	}

	public unsafe ShroomSpawnDefinition SpawnDefinition
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 38421, RefRangeEnd = 38424, XrefRangeStart = 38421, XrefRangeEnd = 38424, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SpawnDefinition_Public_get_ShroomSpawnDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ShroomSpawnDefinition>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SpawnDefinition_Private_set_Void_ShroomSpawnDefinition_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SporeSyringeDefinition()
	{
		Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "SporeSyringeDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr);
		NativeFieldInfoPtr__SpawnDefinition_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr, "<SpawnDefinition>k__BackingField");
		NativeMethodInfoPtr_get_SpawnDefinition_Public_get_ShroomSpawnDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr, 100672665);
		NativeMethodInfoPtr_set_SpawnDefinition_Private_set_Void_ShroomSpawnDefinition_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr, 100672666);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr, 100672667);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 166837, RefRangeEnd = 166838, XrefRangeStart = 166837, XrefRangeEnd = 166838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SporeSyringeDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SporeSyringeDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SporeSyringeDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
