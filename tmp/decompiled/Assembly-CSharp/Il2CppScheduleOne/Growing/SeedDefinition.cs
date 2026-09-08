using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.ItemFramework;

namespace Il2CppScheduleOne.Growing;

[Serializable]
public class SeedDefinition : StorableItemDefinition
{
	private static readonly IntPtr NativeFieldInfoPtr_FunctionSeedPrefab;

	private static readonly IntPtr NativeFieldInfoPtr_PlantPrefab;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe FunctionalSeed FunctionSeedPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FunctionSeedPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FunctionalSeed>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FunctionSeedPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)functionalSeed));
		}
	}

	public unsafe Plant PlantPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlantPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Plant>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlantPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)plant));
		}
	}

	static SeedDefinition()
	{
		Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "SeedDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_FunctionSeedPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr, "FunctionSeedPrefab");
		NativeFieldInfoPtr_PlantPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr, "PlantPrefab");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr, 100678581);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 166837, RefRangeEnd = 166838, XrefRangeStart = 166837, XrefRangeEnd = 166838, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SeedDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SeedDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SeedDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
