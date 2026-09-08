using System;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppVLB;

public static class DummyMaterial : Il2CppSystem.Object
{
	static DummyMaterial()
	{
		Il2CppClassPointerStore<DummyMaterial>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "VLB", "DummyMaterial");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DummyMaterial>.NativeClassPtr);
	}

	public DummyMaterial(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
