using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

[StructLayout(LayoutKind.Explicit)]
public struct PQHandle
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Invalid;

	private static readonly System.IntPtr NativeFieldInfoPtr__handle;

	[FieldOffset(0)]
	public int _handle;

	public unsafe static int Invalid
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Invalid, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Invalid, (void*)(&num));
		}
	}

	static PQHandle()
	{
		Il2CppClassPointerStore<PQHandle>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "PQHandle");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PQHandle>.NativeClassPtr);
		NativeFieldInfoPtr_Invalid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PQHandle>.NativeClassPtr, "Invalid");
		NativeFieldInfoPtr__handle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PQHandle>.NativeClassPtr, "_handle");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<PQHandle>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
