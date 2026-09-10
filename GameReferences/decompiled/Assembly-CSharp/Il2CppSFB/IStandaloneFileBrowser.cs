using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSFB;

public class IStandaloneFileBrowser : Il2CppObjectBase
{
	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanel_Public_Abstract_Virtual_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Abstract_Virtual_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0;

	static IStandaloneFileBrowser()
	{
		Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "SFB", "IStandaloneFileBrowser");
		NativeMethodInfoPtr_OpenFilePanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664476);
		NativeMethodInfoPtr_OpenFolderPanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664477);
		NativeMethodInfoPtr_SaveFilePanel_Public_Abstract_Virtual_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664478);
		NativeMethodInfoPtr_OpenFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664479);
		NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Abstract_Virtual_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664480);
		NativeMethodInfoPtr_SaveFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IStandaloneFileBrowser>.NativeClassPtr, 100664481);
	}

	[CallerCount(0)]
	public unsafe virtual Il2CppStringArray OpenFilePanel(string title, string directory, Il2CppReferenceArray<ExtensionFilter> extensions, bool multiselect)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OpenFilePanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual Il2CppStringArray OpenFolderPanel(string title, string directory, bool multiselect)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OpenFolderPanel_Public_Abstract_Virtual_New_Il2CppStringArray_String_String_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual string SaveFilePanel(string title, string directory, string defaultName, Il2CppReferenceArray<ExtensionFilter> extensions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SaveFilePanel_Public_Abstract_Virtual_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	public unsafe virtual void OpenFilePanelAsync(string title, string directory, Il2CppReferenceArray<ExtensionFilter> extensions, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OpenFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void OpenFolderPanelAsync(string title, string directory, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Abstract_Virtual_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void SaveFilePanelAsync(string title, string directory, string defaultName, Il2CppReferenceArray<ExtensionFilter> extensions, Il2CppSystem.Action<string> cb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SaveFilePanelAsync_Public_Abstract_Virtual_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public IStandaloneFileBrowser(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
