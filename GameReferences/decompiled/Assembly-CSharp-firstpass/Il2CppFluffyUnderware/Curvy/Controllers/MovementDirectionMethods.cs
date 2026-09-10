using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Controllers;

public static class MovementDirectionMethods : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_FromInt_Public_Static_MovementDirection_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOpposite_Public_Static_MovementDirection_MovementDirection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToInt_Public_Static_Int32_MovementDirection_0;

	static MovementDirectionMethods()
	{
		Il2CppClassPointerStore<MovementDirectionMethods>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Controllers", "MovementDirectionMethods");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MovementDirectionMethods>.NativeClassPtr);
		NativeMethodInfoPtr_FromInt_Public_Static_MovementDirection_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MovementDirectionMethods>.NativeClassPtr, 100665484);
		NativeMethodInfoPtr_GetOpposite_Public_Static_MovementDirection_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MovementDirectionMethods>.NativeClassPtr, 100665485);
		NativeMethodInfoPtr_ToInt_Public_Static_Int32_MovementDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MovementDirectionMethods>.NativeClassPtr, 100665486);
	}

	[CallerCount(0)]
	public unsafe static MovementDirection FromInt(int value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FromInt_Public_Static_MovementDirection_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static MovementDirection GetOpposite(this MovementDirection value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOpposite_Public_Static_MovementDirection_MovementDirection_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(MovementDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static int ToInt(this MovementDirection direction)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&direction);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToInt_Public_Static_Int32_MovementDirection_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public MovementDirectionMethods(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
