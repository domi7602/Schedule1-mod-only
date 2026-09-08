using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppScheduleOne.PlayerScripts;

namespace Il2CppScheduleOne.Variables;

public class BoolVariable : Variable<bool>
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_TryDeserialize_Public_Virtual_Boolean_String_byref_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_Boolean_EConditionType_String_0;

	static BoolVariable()
	{
		Il2CppClassPointerStore<BoolVariable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Variables", "BoolVariable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoolVariable>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoolVariable>.NativeClassPtr, 100666514);
		NativeMethodInfoPtr_TryDeserialize_Public_Virtual_Boolean_String_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoolVariable>.NativeClassPtr, 100666515);
		NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_Boolean_EConditionType_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoolVariable>.NativeClassPtr, 100666516);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97191, XrefRangeEnd = 97194, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BoolVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner, bool value)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoolVariable>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[6];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(EVariableReplicationMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &replicationMode;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &persistent;
		*(EVariableMode**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &mode;
		*(IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)owner);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &value;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97194, XrefRangeEnd = 97202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool TryDeserialize(string valueString, out bool value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(valueString);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_TryDeserialize_Public_Virtual_Boolean_String_byref_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 97202, XrefRangeEnd = 97214, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool EvaluateCondition(Condition.EConditionType operation, string value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&operation);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_Boolean_EConditionType_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public BoolVariable(IntPtr pointer)
		: base(pointer)
	{
	}
}
