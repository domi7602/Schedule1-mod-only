using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Serializing;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Core.Equipping.Framework;
using Il2CppSystem;

namespace Il2CppScheduleOne.Equipping.Framework;

public static class EquippableDataSerializer : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_WriteEquippableData_Public_Static_Void_Writer_EquippableData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadEquippableData_Public_Static_EquippableData_Reader_0;

	static EquippableDataSerializer()
	{
		Il2CppClassPointerStore<EquippableDataSerializer>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Equipping.Framework", "EquippableDataSerializer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EquippableDataSerializer>.NativeClassPtr);
		NativeMethodInfoPtr_WriteEquippableData_Public_Static_Void_Writer_EquippableData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableDataSerializer>.NativeClassPtr, 100679925);
		NativeMethodInfoPtr_ReadEquippableData_Public_Static_EquippableData_Reader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EquippableDataSerializer>.NativeClassPtr, 100679926);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245055, XrefRangeEnd = 245063, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void WriteEquippableData(this Writer writer, EquippableData value)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)writer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WriteEquippableData_Public_Static_Void_Writer_EquippableData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 245063, XrefRangeEnd = 245074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static EquippableData ReadEquippableData(this Reader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadEquippableData_Public_Static_EquippableData_Reader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EquippableData>(intPtr) : null;
	}

	public EquippableDataSerializer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
