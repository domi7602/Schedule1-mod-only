using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppScheduleOne.State;

public class InGameStateMachine : MonoStateMachine
{
	private static readonly IntPtr NativeMethodInfoPtr_PopUntilDefault_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static InGameStateMachine()
	{
		Il2CppClassPointerStore<InGameStateMachine>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.State", "InGameStateMachine");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InGameStateMachine>.NativeClassPtr);
		NativeMethodInfoPtr_PopUntilDefault_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InGameStateMachine>.NativeClassPtr, 100666993);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InGameStateMachine>.NativeClassPtr, 100666994);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 102504, RefRangeEnd = 102505, XrefRangeStart = 102472, XrefRangeEnd = 102504, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PopUntilDefault()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PopUntilDefault_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 102528, RefRangeEnd = 102529, XrefRangeStart = 102505, XrefRangeEnd = 102528, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InGameStateMachine()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InGameStateMachine>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InGameStateMachine(IntPtr pointer)
		: base(pointer)
	{
	}
}
