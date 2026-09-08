using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSFB;

public class StandaloneFileBrowser : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__platformWrapper;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_String_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanel_Public_Static_Il2CppStringArray_String_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Static_Void_String_String_Boolean_Action_1_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_String_Action_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static IStandaloneFileBrowser _platformWrapper
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__platformWrapper, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<IStandaloneFileBrowser>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__platformWrapper, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)standaloneFileBrowser));
		}
	}

	static StandaloneFileBrowser()
	{
		Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "SFB", "StandaloneFileBrowser");
		NativeFieldInfoPtr__platformWrapper = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, "_platformWrapper");
		NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664484);
		NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664485);
		NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_String_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664486);
		NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664487);
		NativeMethodInfoPtr_OpenFolderPanel_Public_Static_Il2CppStringArray_String_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664488);
		NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Static_Void_String_String_Boolean_Action_1_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664489);
		NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664490);
		NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664491);
		NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_String_Action_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664492);
		NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664493);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr, 100664494);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 74930, RefRangeEnd = 74937, XrefRangeStart = 74906, XrefRangeEnd = 74930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStringArray OpenFilePanel(string title, string directory, string extension, bool multiselect)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(extension);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 74944, RefRangeEnd = 74947, XrefRangeStart = 74937, XrefRangeEnd = 74944, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStringArray OpenFilePanel(string title, string directory, Il2CppReferenceArray<ExtensionFilter> extensions, bool multiselect)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanel_Public_Static_Il2CppStringArray_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74971, RefRangeEnd = 74972, XrefRangeStart = 74947, XrefRangeEnd = 74971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OpenFilePanelAsync(string title, string directory, string extension, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(extension);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_String_Boolean_Action_1_Il2CppStringArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 74972, XrefRangeEnd = 74979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OpenFilePanelAsync(string title, string directory, Il2CppReferenceArray<ExtensionFilter> extensions, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFilePanelAsync_Public_Static_Void_String_String_Il2CppReferenceArray_1_ExtensionFilter_Boolean_Action_1_Il2CppStringArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 74986, RefRangeEnd = 74988, XrefRangeStart = 74979, XrefRangeEnd = 74986, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStringArray OpenFolderPanel(string title, string directory, bool multiselect)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFolderPanel_Public_Static_Il2CppStringArray_String_String_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 74995, RefRangeEnd = 74996, XrefRangeStart = 74988, XrefRangeEnd = 74995, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void OpenFolderPanelAsync(string title, string directory, bool multiselect, Il2CppSystem.Action<Il2CppStringArray> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &multiselect;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenFolderPanelAsync_Public_Static_Void_String_String_Boolean_Action_1_Il2CppStringArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 75020, RefRangeEnd = 75026, XrefRangeStart = 74996, XrefRangeEnd = 75020, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string SaveFilePanel(string title, string directory, string defaultName, string extension)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(extension);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 75033, RefRangeEnd = 75036, XrefRangeStart = 75026, XrefRangeEnd = 75033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string SaveFilePanel(string title, string directory, string defaultName, Il2CppReferenceArray<ExtensionFilter> extensions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanel_Public_Static_String_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 75060, RefRangeEnd = 75062, XrefRangeStart = 75036, XrefRangeEnd = 75060, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SaveFilePanelAsync(string title, string directory, string defaultName, string extension, Il2CppSystem.Action<string> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(extension);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_String_Action_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 75062, XrefRangeEnd = 75069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SaveFilePanelAsync(string title, string directory, string defaultName, Il2CppReferenceArray<ExtensionFilter> extensions, Il2CppSystem.Action<string> cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(title);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(directory);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(defaultName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)extensions);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SaveFilePanelAsync_Public_Static_Void_String_String_String_Il2CppReferenceArray_1_ExtensionFilter_Action_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StandaloneFileBrowser()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StandaloneFileBrowser>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StandaloneFileBrowser(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
