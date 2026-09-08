using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.AvatarFramework;

[Serializable]
public class FaceLayer : AvatarLayer
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static FaceLayer()
	{
		Il2CppClassPointerStore<FaceLayer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.AvatarFramework", "FaceLayer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FaceLayer>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FaceLayer>.NativeClassPtr, 100677456);
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 79617, RefRangeEnd = 79648, XrefRangeStart = 79617, XrefRangeEnd = 79648, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FaceLayer()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FaceLayer>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FaceLayer(IntPtr pointer)
		: base(pointer)
	{
	}
}
