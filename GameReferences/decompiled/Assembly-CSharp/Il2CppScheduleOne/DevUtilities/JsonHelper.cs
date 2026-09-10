using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppScheduleOne.DevUtilities;

public static class JsonHelper : Il2CppSystem.Object
{
	[System.Serializable]
	public class Wrapper<T> : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Items;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Il2CppArrayBase<T> Items
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items);
				return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Items)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		static Wrapper()
		{
			Il2CppClassPointerStore<Wrapper<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<JsonHelper>.NativeClassPtr, "Wrapper`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Wrapper<T>>.NativeClassPtr);
			NativeFieldInfoPtr_Items = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Wrapper<T>>.NativeClassPtr, "Items");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Wrapper<T>>.NativeClassPtr, 100675060);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Wrapper()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Wrapper<T>>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Wrapper(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_FromJson_Public_Static_Il2CppArrayBase_1_T_String_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_FromJson_Public_Static_Il2CppArrayBase_1_T_String_0, Il2CppClassPointerStore<JsonHelper>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ToJson_Public_Static_String_Il2CppArrayBase_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_0, Il2CppClassPointerStore<JsonHelper>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private sealed class MethodInfoStoreGeneric_ToJson_Public_Static_String_Il2CppArrayBase_1_T_Boolean_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_Boolean_0, Il2CppClassPointerStore<JsonHelper>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_FromJson_Public_Static_Il2CppArrayBase_1_T_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_Boolean_0;

	static JsonHelper()
	{
		Il2CppClassPointerStore<JsonHelper>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.DevUtilities", "JsonHelper");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<JsonHelper>.NativeClassPtr);
		NativeMethodInfoPtr_FromJson_Public_Static_Il2CppArrayBase_1_T_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonHelper>.NativeClassPtr, 100675057);
		NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonHelper>.NativeClassPtr, 100675058);
		NativeMethodInfoPtr_ToJson_Public_Static_String_Il2CppArrayBase_1_T_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JsonHelper>.NativeClassPtr, 100675059);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194549, XrefRangeEnd = 194550, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppArrayBase<T> FromJson<T>(string json)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(json);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_FromJson_Public_Static_Il2CppArrayBase_1_T_String_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return Il2CppArrayBase<T>.WrapNativeGenericArrayPointer(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194550, XrefRangeEnd = 194566, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ToJson<T>(Il2CppArrayBase<T> array)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ToJson_Public_Static_String_Il2CppArrayBase_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 194566, XrefRangeEnd = 194572, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ToJson<T>(Il2CppArrayBase<T> array, bool prettyPrint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)array);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &prettyPrint;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_ToJson_Public_Static_String_Il2CppArrayBase_1_T_Boolean_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public JsonHelper(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
