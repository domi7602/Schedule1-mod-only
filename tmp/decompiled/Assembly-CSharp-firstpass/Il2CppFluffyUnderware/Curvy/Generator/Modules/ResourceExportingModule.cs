using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class ResourceExportingModule : CGModule
{
	private static readonly IntPtr NativeMethodInfoPtr_SaveToScene_Public_GameObject_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr_SaveResourceToScene_Protected_Abstract_Virtual_New_GameObject_Component_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	static ResourceExportingModule()
	{
		Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "ResourceExportingModule");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr);
		NativeMethodInfoPtr_SaveToScene_Public_GameObject_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr, 100667066);
		NativeMethodInfoPtr_SaveResourceToScene_Protected_Abstract_Virtual_New_GameObject_Component_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr, 100667067);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr, 100667068);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 59148, RefRangeEnd = 59149, XrefRangeStart = 59119, XrefRangeEnd = 59148, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameObject SaveToScene(Transform parent = null)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveToScene_Public_GameObject_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual GameObject SaveResourceToScene(Component managedResource, Transform newParent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)managedResource);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newParent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SaveResourceToScene_Protected_Abstract_Virtual_New_GameObject_Component_Transform_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 46598, RefRangeEnd = 46602, XrefRangeStart = 46598, XrefRangeEnd = 46602, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ResourceExportingModule()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ResourceExportingModule>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ResourceExportingModule(IntPtr pointer)
		: base(pointer)
	{
	}
}
