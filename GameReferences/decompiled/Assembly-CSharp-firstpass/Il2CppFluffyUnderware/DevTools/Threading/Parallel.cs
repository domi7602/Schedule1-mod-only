using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools.Threading;

public static class Parallel : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_For_Public_Static_Void_Int32_Int32_Action_1_Int32_0;

	static Parallel()
	{
		Il2CppClassPointerStore<Parallel>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools.Threading", "Parallel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Parallel>.NativeClassPtr);
		NativeMethodInfoPtr_For_Public_Static_Void_Int32_Int32_Action_1_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Parallel>.NativeClassPtr, 100664434);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 34625, XrefRangeEnd = 34629, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void For(int fromInclusive, int toExclusive, Il2CppSystem.Action<int> body)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&fromInclusive);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &toExclusive;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)body);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_For_Public_Static_Void_Int32_Int32_Action_1_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public Parallel(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
