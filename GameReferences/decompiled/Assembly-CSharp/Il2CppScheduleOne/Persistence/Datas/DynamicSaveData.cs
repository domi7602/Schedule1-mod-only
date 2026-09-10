using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.Persistence.Datas;

[System.Serializable]
public class DynamicSaveData : SaveData
{
	[System.Serializable]
	public class AdditionalData : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Name;

		private static readonly System.IntPtr NativeFieldInfoPtr_Contents;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string Name
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Name)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Contents
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contents);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Contents)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static AdditionalData()
		{
			Il2CppClassPointerStore<AdditionalData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, "AdditionalData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AdditionalData>.NativeClassPtr);
			NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdditionalData>.NativeClassPtr, "Name");
			NativeFieldInfoPtr_Contents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AdditionalData>.NativeClassPtr, "Contents");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AdditionalData>.NativeClassPtr, 100669331);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe AdditionalData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AdditionalData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public AdditionalData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_GetData_Public_T_String_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetData_Public_T_String_Boolean_0, Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_TryGetData_Public_Boolean_String_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_T_0, Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ExtractBaseData_Public_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ExtractBaseData_Public_T_0, Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_TryExtractBaseData_Public_Boolean_byref_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_TryExtractBaseData_Public_Boolean_byref_T_0, Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_BaseData;

	private static readonly System.IntPtr NativeFieldInfoPtr_AdditionalDatas;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SaveData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddData_Public_Void_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddData_Public_Void_String_SaveData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetData_Public_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetData_Public_T_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExtractBaseData_Public_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryExtractBaseData_Public_Boolean_byref_T_0;

	public unsafe string BaseData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseData);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_BaseData)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe List<AdditionalData> AdditionalDatas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AdditionalDatas);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AdditionalData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AdditionalDatas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static DynamicSaveData()
	{
		Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Datas", "DynamicSaveData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr);
		NativeFieldInfoPtr_BaseData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, "BaseData");
		NativeFieldInfoPtr_AdditionalDatas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, "AdditionalDatas");
		NativeMethodInfoPtr__ctor_Public_Void_SaveData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669322);
		NativeMethodInfoPtr_AddData_Public_Void_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669323);
		NativeMethodInfoPtr_AddData_Public_Void_String_SaveData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669324);
		NativeMethodInfoPtr_GetData_Public_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669325);
		NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669326);
		NativeMethodInfoPtr_GetData_Public_T_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669327);
		NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669328);
		NativeMethodInfoPtr_ExtractBaseData_Public_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669329);
		NativeMethodInfoPtr_TryExtractBaseData_Public_Boolean_byref_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr, 100669330);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 134388, RefRangeEnd = 134390, XrefRangeStart = 134373, XrefRangeEnd = 134388, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DynamicSaveData(SaveData baseData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DynamicSaveData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)baseData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SaveData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(21)]
	[CachedScanResults(RefRangeStart = 134410, RefRangeEnd = 134431, XrefRangeStart = 134390, XrefRangeEnd = 134410, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddData(string name, string contents)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(contents);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddData_Public_Void_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134431, XrefRangeEnd = 134433, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddData(string name, SaveData data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddData_Public_Void_String_SaveData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 134445, RefRangeEnd = 134449, XrefRangeStart = 134433, XrefRangeEnd = 134445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetData(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetData_Public_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 134454, RefRangeEnd = 134455, XrefRangeStart = 134449, XrefRangeEnd = 134454, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetData(string name, out string data)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetData_Public_Boolean_String_byref_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		data = IL2CPP.Il2CppStringToManaged((System.IntPtr)num2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134455, XrefRangeEnd = 134469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T GetData<T>(string name, bool warn = true) where T : SaveData
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &warn;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetData_Public_T_String_Boolean_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(19)]
	[CachedScanResults(RefRangeStart = 134481, RefRangeEnd = 134500, XrefRangeStart = 134469, XrefRangeEnd = 134481, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetData<T>(string name, out T data) where T : SaveData
	{
		//IL_003f->IL0041: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_TryGetData_Public_Boolean_String_byref_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num3 = num2;
			data = ((num3 == 0) ? null : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num3, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 134500, XrefRangeEnd = 134510, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe T ExtractBaseData<T>() where T : SaveData
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ExtractBaseData_Public_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
	}

	[CallerCount(44)]
	[CachedScanResults(RefRangeStart = 134530, RefRangeEnd = 134574, XrefRangeStart = 134510, XrefRangeEnd = 134530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryExtractBaseData<T>(out T data) where T : SaveData
	{
		//IL_002d->IL002f: Incompatible stack types: I vs Ref
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		Unsafe.SkipInit(out nint num);
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			num = 0;
			reference = ref *(_003F*)(&num);
		}
		else
		{
			reference = ref data;
		}
		*ptr = (nint)Unsafe.AsPointer(ref reference);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_TryExtractBaseData_Public_Boolean_byref_T_0<T>.Pointer, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		if (!typeof(T).IsValueType)
		{
			nint num2 = num;
			data = ((num2 == 0) ? null : IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num2, false, false));
		}
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public DynamicSaveData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
