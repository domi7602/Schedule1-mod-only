using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.Persistence.Loaders;

public class Loader : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_TryDeserialize_Public_Static_Boolean_String_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_T_0, Il2CppClassPointerStore<Loader>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Load_Public_Virtual_New_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_String_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_byref_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirectories_Protected_List_1_DirectoryInfo_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetFiles_Protected_List_1_FileInfo_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_T_0;

	static Loader()
	{
		Il2CppClassPointerStore<Loader>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Loaders", "Loader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Loader>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669016);
		NativeMethodInfoPtr_Load_Public_Virtual_New_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669017);
		NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_String_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669018);
		NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_byref_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669019);
		NativeMethodInfoPtr_GetDirectories_Protected_List_1_DirectoryInfo_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669020);
		NativeMethodInfoPtr_GetFiles_Protected_List_1_FileInfo_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669021);
		NativeMethodInfoPtr_TryDeserialize_Public_Static_Boolean_String_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Loader>.NativeClassPtr, 100669022);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Loader()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Loader>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Load(string mainPath)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(mainPath);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Load_Public_Virtual_New_Void_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(18)]
	[CachedScanResults(RefRangeStart = 128669, RefRangeEnd = 128687, XrefRangeStart = 128664, XrefRangeEnd = 128669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryLoadFile(string parentPath, string fileName, out string contents)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(parentPath);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(fileName);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_String_byref_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		contents = IL2CPP.Il2CppStringToManaged((System.IntPtr)num2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(65)]
	[CachedScanResults(RefRangeStart = 128699, RefRangeEnd = 128764, XrefRangeStart = 128687, XrefRangeEnd = 128699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryLoadFile(string path, out string contents, bool autoAddExtension = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(path);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &autoAddExtension;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryLoadFile_Public_Boolean_String_byref_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		contents = IL2CPP.Il2CppStringToManaged((System.IntPtr)num2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 128782, RefRangeEnd = 128789, XrefRangeStart = 128764, XrefRangeEnd = 128782, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<DirectoryInfo> GetDirectories(string parentPath)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(parentPath);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirectories_Protected_List_1_DirectoryInfo_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<DirectoryInfo>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 128807, RefRangeEnd = 128809, XrefRangeStart = 128789, XrefRangeEnd = 128807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<FileInfo> GetFiles(string parenPath)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(parenPath);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetFiles_Protected_List_1_FileInfo_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<FileInfo>>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 128813, RefRangeEnd = 128817, XrefRangeStart = 128809, XrefRangeEnd = 128813, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryDeserialize<T>(string json, out T data)
	{
		//IL_0038->IL003a: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		Unsafe.SkipInit(out nint num2);
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			num2 = 0;
			reference = ref *(_003F*)(&num2);
		}
		else
		{
			reference = ref data;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_TryDeserialize_Public_Static_Boolean_String_byref_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num3 = num2;
			Unsafe.As<T, object>(ref data) = ((num3 == 0) ? ((T)null) : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public Loader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
