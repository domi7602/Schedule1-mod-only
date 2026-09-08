using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppScheduleOne.Building;

public class ActivateDuringBuild : MonoBehaviour
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static ActivateDuringBuild()
	{
		Il2CppClassPointerStore<ActivateDuringBuild>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Building", "ActivateDuringBuild");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ActivateDuringBuild>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ActivateDuringBuild>.NativeClassPtr, 100676551);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ActivateDuringBuild()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ActivateDuringBuild>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ActivateDuringBuild(IntPtr pointer)
		: base(pointer)
	{
	}
}
