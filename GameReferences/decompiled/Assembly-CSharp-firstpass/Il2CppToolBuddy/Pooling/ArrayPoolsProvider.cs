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
using Il2CppToolBuddy.Pooling.Pools;

namespace Il2CppToolBuddy.Pooling;

public class ArrayPoolsProvider : Il2CppSystem.Object
{
	private sealed class MethodInfoStoreGeneric_GetPool_Public_Static_ArrayPool_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_GetPool_Public_Static_ArrayPool_1_T_0, Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_arrayPools;

	private static readonly System.IntPtr NativeFieldInfoPtr_lockObject;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPool_Public_Static_ArrayPool_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Dictionary<Il2CppSystem.Type, Il2CppSystem.Object> arrayPools
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_arrayPools, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<Il2CppSystem.Type, Il2CppSystem.Object>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_arrayPools, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static Il2CppSystem.Object lockObject
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_lockObject, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_lockObject, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
		}
	}

	static ArrayPoolsProvider()
	{
		Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.Pooling", "ArrayPoolsProvider");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr);
		NativeFieldInfoPtr_arrayPools = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr, "arrayPools");
		NativeFieldInfoPtr_lockObject = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr, "lockObject");
		NativeMethodInfoPtr_GetPool_Public_Static_ArrayPool_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr, 100663790);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr, 100663791);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 29428, XrefRangeEnd = 29462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ArrayPool<T> GetPool<T>()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_GetPool_Public_Static_ArrayPool_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ArrayPool<T>>(intPtr) : null;
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ArrayPoolsProvider()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ArrayPoolsProvider>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ArrayPoolsProvider(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
