using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Connection;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppSystem;

namespace Il2CppScheduleOne.Variables;

public class BaseVariable : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_ReplicationMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_Name;

	private static readonly System.IntPtr NativeFieldInfoPtr_Persistent;

	private static readonly System.IntPtr NativeFieldInfoPtr_VariableMode;

	private static readonly System.IntPtr NativeFieldInfoPtr__Owner_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Owner_Public_get_Player_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Owner_Private_set_Void_Player_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetValue_Public_Abstract_Virtual_New_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetValue_Public_Abstract_Virtual_New_Void_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReplicateValue_Public_Abstract_Virtual_New_Void_NetworkConnection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_New_Boolean_EConditionType_String_0;

	public unsafe EVariableReplicationMode ReplicationMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReplicationMode);
			return *(EVariableReplicationMode*)num;
		}
		set
		{
			*(EVariableReplicationMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReplicationMode)) = eVariableReplicationMode;
		}
	}

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

	public unsafe bool Persistent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Persistent);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Persistent)) = flag;
		}
	}

	public unsafe EVariableMode VariableMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VariableMode);
			return *(EVariableMode*)num;
		}
		set
		{
			*(EVariableMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VariableMode)) = eVariableMode;
		}
	}

	public unsafe Player _Owner_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Owner_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Player>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Owner_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player));
		}
	}

	public unsafe Player Owner
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 2960, RefRangeEnd = 2963, XrefRangeStart = 2960, XrefRangeEnd = 2963, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Owner_Public_get_Player_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Player>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Owner_Private_set_Void_Player_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static BaseVariable()
	{
		Il2CppClassPointerStore<BaseVariable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Variables", "BaseVariable");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr);
		NativeFieldInfoPtr_ReplicationMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, "ReplicationMode");
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, "Name");
		NativeFieldInfoPtr_Persistent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, "Persistent");
		NativeFieldInfoPtr_VariableMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, "VariableMode");
		NativeFieldInfoPtr__Owner_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, "<Owner>k__BackingField");
		NativeMethodInfoPtr_get_Owner_Public_get_Player_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666507);
		NativeMethodInfoPtr_set_Owner_Private_set_Void_Player_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666508);
		NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666509);
		NativeMethodInfoPtr_GetValue_Public_Abstract_Virtual_New_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666510);
		NativeMethodInfoPtr_SetValue_Public_Abstract_Virtual_New_Void_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666511);
		NativeMethodInfoPtr_ReplicateValue_Public_Abstract_Virtual_New_Void_NetworkConnection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666512);
		NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_New_Boolean_EConditionType_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr, 100666513);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 97189, RefRangeEnd = 97191, XrefRangeStart = 97146, XrefRangeEnd = 97189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BaseVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BaseVariable>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(EVariableReplicationMode**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &replicationMode;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &persistent;
		*(EVariableMode**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)owner);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EVariableReplicationMode_Boolean_EVariableMode_Player_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual Il2CppSystem.Object GetValue()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetValue_Public_Abstract_Virtual_New_Object_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe virtual void SetValue(Il2CppSystem.Object value, bool replicate = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &replicate;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_SetValue_Public_Abstract_Virtual_New_Void_Object_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void ReplicateValue(NetworkConnection conn)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)conn);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ReplicateValue_Public_Abstract_Virtual_New_Void_NetworkConnection_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(170)]
	[CachedScanResults(RefRangeStart = 31151, RefRangeEnd = 31321, XrefRangeStart = 31151, XrefRangeEnd = 31321, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool EvaluateCondition(Condition.EConditionType operation, string value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&operation);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EvaluateCondition_Public_Virtual_New_Boolean_EConditionType_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public BaseVariable(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
