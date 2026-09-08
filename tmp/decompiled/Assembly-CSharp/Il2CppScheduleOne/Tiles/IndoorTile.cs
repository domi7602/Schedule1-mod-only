using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.Tiles;

public class IndoorTile : Tile
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static IndoorTile()
	{
		Il2CppClassPointerStore<IndoorTile>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Tiles", "IndoorTile");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IndoorTile>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IndoorTile>.NativeClassPtr, 100666910);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 101415, XrefRangeEnd = 101430, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IndoorTile()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IndoorTile>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IndoorTile(IntPtr pointer)
		: base(pointer)
	{
	}
}
