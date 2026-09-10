using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools;

public class DTSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static readonly System.IntPtr NativeFieldInfoPtr__instance;

	private static readonly System.IntPtr NativeFieldInfoPtr__lock;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitializeStaticFields_Protected_Static_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasInstance_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Instance_Public_Static_get_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_New_Void_IDTSingleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static T _instance
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__instance, (void*)(&intPtr));
			return IL2CPP.PointerToValueGeneric<T>(intPtr, false, false);
		}
		set
		{
			//IL_0052->IL0055: Incompatible stack types: I vs Ref
			//IL_0031->IL0055: Incompatible stack types: I vs Ref
			//IL_003e->IL0055: Incompatible stack types: I vs Ref
			//IL_0045->IL0055: Incompatible stack types: I vs Ref
			System.IntPtr nativeFieldInfoPtr__instance = NativeFieldInfoPtr__instance;
			ref T reference;
			if (!typeof(T).IsValueType)
			{
				T val = val2;
				if (val is string)
				{
					reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(val as string);
				}
				else
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)((val is Il2CppObjectBase) ? val : null));
					reference = ref *(_003F*)intPtr;
					if (intPtr != (System.IntPtr)0)
					{
						reference = ref *(_003F*)intPtr;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
						}
					}
				}
			}
			else
			{
				reference = ref val2;
			}
			IL2CPP.il2cpp_field_static_set_value(nativeFieldInfoPtr__instance, Unsafe.AsPointer(ref reference));
		}
	}

	public unsafe static Il2CppSystem.Object _lock
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__lock, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__lock, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	public unsafe static bool HasInstance
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 32928, XrefRangeEnd = 32938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasInstance_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static T Instance
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 33033, RefRangeEnd = 33037, XrefRangeStart = 32938, XrefRangeEnd = 33033, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Instance_Public_Static_get_T_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.PointerToValueGeneric<T>(intPtr, false, true);
		}
	}

	static DTSingleton()
	{
		Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTSingleton`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr);
		NativeFieldInfoPtr__instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, "_instance");
		NativeFieldInfoPtr__lock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, "_lock");
		NativeMethodInfoPtr_InitializeStaticFields_Protected_Static_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664261);
		NativeMethodInfoPtr_get_HasInstance_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664262);
		NativeMethodInfoPtr_get_Instance_Public_Static_get_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664263);
		NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664264);
		NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_New_Void_IDTSingleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664265);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr, 100664266);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 32927, RefRangeEnd = 32928, XrefRangeStart = 32909, XrefRangeEnd = 32927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void InitializeStaticFields()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitializeStaticFields_Protected_Static_Void_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 33069, RefRangeEnd = 33070, XrefRangeStart = 33037, XrefRangeEnd = 33069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(14950)]
	[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void MergeDoubleLoaded(IDTSingleton newInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newInstance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_MergeDoubleLoaded_Public_Virtual_New_Void_IDTSingleton_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DTSingleton()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DTSingleton<T>>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DTSingleton(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
