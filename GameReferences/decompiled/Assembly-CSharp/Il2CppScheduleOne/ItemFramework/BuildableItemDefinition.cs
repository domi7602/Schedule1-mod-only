using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.EntityFramework;

namespace Il2CppScheduleOne.ItemFramework;

[Serializable]
public class BuildableItemDefinition : StorableItemDefinition
{
	[OriginalName("Assembly-CSharp.dll", "", "EBuildSoundType")]
	public enum EBuildSoundType
	{
		Cardboard,
		Wood,
		Metal
	}

	private static readonly IntPtr NativeFieldInfoPtr_BuiltItem;

	private static readonly IntPtr NativeFieldInfoPtr_BuildSoundType;

	private static readonly IntPtr NativeMethodInfoPtr_ValidateDefinition_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe BuildableItem BuiltItem
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BuiltItem);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BuildableItem>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BuiltItem)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buildableItem));
		}
	}

	public unsafe EBuildSoundType BuildSoundType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BuildSoundType);
			return *(EBuildSoundType*)num;
		}
		set
		{
			*(EBuildSoundType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BuildSoundType)) = eBuildSoundType;
		}
	}

	static BuildableItemDefinition()
	{
		Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.ItemFramework", "BuildableItemDefinition");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr);
		NativeFieldInfoPtr_BuiltItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr, "BuiltItem");
		NativeFieldInfoPtr_BuildSoundType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr, "BuildSoundType");
		NativeMethodInfoPtr_ValidateDefinition_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr, 100672451);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr, 100672452);
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
	[CachedScanResults(RefRangeStart = 166837, RefRangeEnd = 166838, XrefRangeStart = 166836, XrefRangeEnd = 166837, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BuildableItemDefinition()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BuildableItemDefinition>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BuildableItemDefinition(IntPtr pointer)
		: base(pointer)
	{
	}
}
