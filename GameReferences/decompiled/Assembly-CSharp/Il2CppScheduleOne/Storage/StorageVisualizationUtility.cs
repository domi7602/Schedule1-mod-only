using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppScheduleOne.Storage;

public static class StorageVisualizationUtility : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetVisualRepresentation_Public_Static_Dictionary_2_StorableItemInstance_Int32_Dictionary_2_StorableItemInstance_Int32_Int32_0;

	static StorageVisualizationUtility()
	{
		Il2CppClassPointerStore<StorageVisualizationUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Storage", "StorageVisualizationUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StorageVisualizationUtility>.NativeClassPtr);
		NativeMethodInfoPtr_GetVisualRepresentation_Public_Static_Dictionary_2_StorableItemInstance_Int32_Dictionary_2_StorableItemInstance_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StorageVisualizationUtility>.NativeClassPtr, 100678812);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 232859, RefRangeEnd = 232861, XrefRangeStart = 232795, XrefRangeEnd = 232859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<StorableItemInstance, int> GetVisualRepresentation(Dictionary<StorableItemInstance, int> inputDictionary, int TotalFootprintSize)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputDictionary);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &TotalFootprintSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVisualRepresentation_Public_Static_Dictionary_2_StorableItemInstance_Int32_Dictionary_2_StorableItemInstance_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StorableItemInstance, int>>(intPtr) : null;
	}

	public StorageVisualizationUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
