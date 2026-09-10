using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSFB;

public class StandaloneFileBrowserWindows : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetActiveWindow_Private_Static_IntPtr_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanelAsync_Public_Virtual_Final_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Virtual_Final_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanel_Public_Virtual_Final_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanelAsync_Public_Virtual_Final_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFilterFromFileExtensionList_Private_Static_String_Il2CppReferenceArray_1_ExtensionFilter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirectoryPath_Private_Static_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static StandaloneFileBrowserWindows()
	{
		Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "SFB", "StandaloneFileBrowserWindows");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr);
		NativeMethodInfoPtr_GetActiveWindow_Private_Static_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664497);
		NativeMethodInfoPtr_OpenFilePanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664498);
		NativeMethodInfoPtr_OpenFilePanelAsync_Public_Virtual_Final_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664499);
		NativeMethodInfoPtr_OpenFolderPanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664500);
		NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Virtual_Final_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664501);
		NativeMethodInfoPtr_SaveFilePanel_Public_Virtual_Final_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664502);
		NativeMethodInfoPtr_SaveFilePanelAsync_Public_Virtual_Final_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664503);
		NativeMethodInfoPtr_GetFilterFromFileExtensionList_Private_Static_String_Il2CppReferenceArray_1_ExtensionFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664504);
		NativeMethodInfoPtr_GetDirectoryPath_Private_Static_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664505);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr, 100664506);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75069, XrefRangeEnd = 75071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static System.IntPtr GetActiveWindow()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetActiveWindow_Private_Static_IntPtr_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(System.IntPtr*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75100, RefRangeEnd = 75101, XrefRangeStart = 75071, XrefRangeEnd = 75100, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Il2CppStringArray OpenFilePanel(string title, string directory, Il2CppReferenceArray<ExtensionFilter> extensions, bool multiselect)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75101, XrefRangeEnd = 75103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanelAsync_Public_Virtual_Final_New_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75103, XrefRangeEnd = 75126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Il2CppStringArray OpenFolderPanel(string title, string directory, bool multiselect)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFolderPanel_Public_Virtual_Final_New_Il2CppStringArray_String_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75126, XrefRangeEnd = 75151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void OpenFolderPanelAsync(string title, string directory, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Virtual_Final_New_Void_String_String_Boolean_Action_1_Il2CppStringArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75185, RefRangeEnd = 75186, XrefRangeStart = 75151, XrefRangeEnd = 75185, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual string SaveFilePanel(string title, string directory, string defaultName, Il2CppReferenceArray<ExtensionFilter> extensions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanel_Public_Virtual_Final_New_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75186, XrefRangeEnd = 75188, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanelAsync_Public_Virtual_Final_New_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 75215, RefRangeEnd = 75217, XrefRangeStart = 75188, XrefRangeEnd = 75215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetFilterFromFileExtensionList(Il2CppReferenceArray<ExtensionFilter> extensions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFilterFromFileExtensionList_Private_Static_String_Il2CppReferenceArray_1_ExtensionFilter_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 75239, RefRangeEnd = 75243, XrefRangeStart = 75217, XrefRangeEnd = 75239, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string GetDirectoryPath(string directory)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(directory);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirectoryPath_Private_Static_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StandaloneFileBrowserWindows()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StandaloneFileBrowserWindows>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StandaloneFileBrowserWindows(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
