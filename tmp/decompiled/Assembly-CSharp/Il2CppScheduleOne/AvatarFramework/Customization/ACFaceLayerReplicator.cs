using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.AvatarFramework.Customization;

public class ACFaceLayerReplicator : ACAssetPathReplicator<FaceLayer>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static ACFaceLayerReplicator()
	{
		Il2CppClassPointerStore<ACFaceLayerReplicator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework.Customization", "ACFaceLayerReplicator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ACFaceLayerReplicator>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ACFaceLayerReplicator>.NativeClassPtr, 100677609);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 222227, XrefRangeEnd = 222230, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ACFaceLayerReplicator()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ACFaceLayerReplicator>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ACFaceLayerReplicator(IntPtr pointer)
		: base(pointer)
	{
	}
}
