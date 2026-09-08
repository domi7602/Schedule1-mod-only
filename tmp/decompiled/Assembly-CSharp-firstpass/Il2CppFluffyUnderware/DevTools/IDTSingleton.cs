using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class IDTSingleton : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_MergeDoubleLoaded_Public_Abstract_Virtual_New_Void_IDTSingleton_0;

	static IDTSingleton()
	{
		Il2CppClassPointerStore<IDTSingleton>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "IDTSingleton");
		NativeMethodInfoPtr_MergeDoubleLoaded_Public_Abstract_Virtual_New_Void_IDTSingleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IDTSingleton>.NativeClassPtr, 100664268);
	}

	[CallerCount(0)]
	public unsafe virtual void MergeDoubleLoaded(IDTSingleton newInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newInstance);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_MergeDoubleLoaded_Public_Abstract_Virtual_New_Void_IDTSingleton_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IDTSingleton(IntPtr pointer)
		: base(pointer)
	{
	}
}
