using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.AvatarFramework.Customization;

public class ACAvatarLayerReplicator : ACAssetPathReplicator<AvatarLayer>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static ACAvatarLayerReplicator()
	{
		Il2CppClassPointerStore<ACAvatarLayerReplicator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Customization", "ACAvatarLayerReplicator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ACAvatarLayerReplicator>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ACAvatarLayerReplicator>.NativeClassPtr, 100677606);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222214, XrefRangeEnd = 222217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ACAvatarLayerReplicator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ACAvatarLayerReplicator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ACAvatarLayerReplicator(IntPtr pointer)
		: base(pointer)
	{
	}
}
