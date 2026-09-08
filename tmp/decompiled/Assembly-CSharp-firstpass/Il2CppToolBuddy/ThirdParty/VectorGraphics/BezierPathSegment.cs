using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

[StructLayout(LayoutKind.Explicit)]
public struct BezierPathSegment
{
	private static readonly System.IntPtr NativeFieldInfoPtr_P0;

	private static readonly System.IntPtr NativeFieldInfoPtr_P1;

	private static readonly System.IntPtr NativeFieldInfoPtr_P2;

	[FieldOffset(0)]
	public Vector2 P0;

	[FieldOffset(8)]
	public Vector2 P1;

	[FieldOffset(16)]
	public Vector2 P2;

	static BezierPathSegment()
	{
		Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "BezierPathSegment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr);
		NativeFieldInfoPtr_P0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr, "P0");
		NativeFieldInfoPtr_P1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr, "P1");
		NativeFieldInfoPtr_P2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr, "P2");
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<BezierPathSegment>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
