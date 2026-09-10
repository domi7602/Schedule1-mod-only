using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class ICGResourceLoader : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_Create_Public_Abstract_Virtual_New_Component_CGModule_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Destroy_Public_Abstract_Virtual_New_Void_CGModule_Component_String_Boolean_0;

	static ICGResourceLoader()
	{
		Il2CppClassPointerStore<ICGResourceLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "ICGResourceLoader");
		NativeMethodInfoPtr_Create_Public_Abstract_Virtual_New_Component_CGModule_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ICGResourceLoader>.NativeClassPtr, 100666454);
		NativeMethodInfoPtr_Destroy_Public_Abstract_Virtual_New_Void_CGModule_Component_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ICGResourceLoader>.NativeClassPtr, 100666455);
	}

	[CallerCount(0)]
	public unsafe virtual Component Create(CGModule cgModule, string context)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cgModule);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Create_Public_Abstract_Virtual_New_Component_CGModule_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Component>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual void Destroy(CGModule cgModule, Component obj, string context, bool kill)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cgModule);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(context);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &kill;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Destroy_Public_Abstract_Virtual_New_Void_CGModule_Component_String_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ICGResourceLoader(IntPtr pointer)
		: base(pointer)
	{
	}
}
