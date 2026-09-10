using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.Persistence.Loaders;

public class DynamicLoader : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_ExtractBaseData_Public_Static_T_DynamicSaveData_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ExtractBaseData_Public_Static_T_DynamicSaveData_0, Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_TryExtractBaseData_Public_Static_Boolean_DynamicSaveData_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_TryExtractBaseData_Public_Static_Boolean_DynamicSaveData_byref_T_0, Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_Load_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Load_Public_Virtual_New_Void_DynamicSaveData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExtractBaseData_Public_Static_T_DynamicSaveData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryExtractBaseData_Public_Static_Boolean_DynamicSaveData_byref_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static DynamicLoader()
	{
		Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Loaders", "DynamicLoader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr);
		NativeMethodInfoPtr_Load_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr, 100669001);
		NativeMethodInfoPtr_Load_Public_Virtual_New_Void_DynamicSaveData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr, 100669002);
		NativeMethodInfoPtr_ExtractBaseData_Public_Static_T_DynamicSaveData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr, 100669003);
		NativeMethodInfoPtr_TryExtractBaseData_Public_Static_Boolean_DynamicSaveData_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr, 100669004);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr, 100669005);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 128511, XrefRangeEnd = 128529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Load(string serializedDynamicSaveData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(serializedDynamicSaveData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Load_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Load(DynamicSaveData saveData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)saveData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Load_Public_Virtual_New_Void_DynamicSaveData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 128541, RefRangeEnd = 128543, XrefRangeStart = 128529, XrefRangeEnd = 128541, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T ExtractBaseData<T>(DynamicSaveData saveData) where T : SaveData
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)saveData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ExtractBaseData_Public_Static_T_DynamicSaveData_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 128548, RefRangeEnd = 128552, XrefRangeStart = 128543, XrefRangeEnd = 128548, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryExtractBaseData<T>(DynamicSaveData saveData, out T baseData) where T : SaveData
	{
		//IL_0038->IL003a: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)saveData);
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
			reference = ref baseData;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_TryExtractBaseData_Public_Static_Boolean_DynamicSaveData_byref_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num3 = num2;
			baseData = ((num3 == 0) ? null : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DynamicLoader()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DynamicLoader>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DynamicLoader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
