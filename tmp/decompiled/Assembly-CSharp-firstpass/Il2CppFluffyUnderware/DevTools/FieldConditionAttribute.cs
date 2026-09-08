using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.DevTools;

public class FieldConditionAttribute : ConditionalAttribute
{
	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_ActionEnum_Object_ActionPositionEnum_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	static FieldConditionAttribute()
	{
		Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "FieldConditionAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_ActionEnum_Object_ActionPositionEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr, 100663896);
		NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr, 100663897);
		NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr, 100663898);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr, 100663899);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30714, XrefRangeEnd = 30716, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FieldConditionAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse = false, ActionEnum action = ActionEnum.Show, Il2CppSystem.Object actionData = null, ActionPositionEnum position = ActionPositionEnum.Below)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse;
		*(ActionEnum**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &action;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)actionData);
		*(ActionPositionEnum**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &position;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_ActionEnum_Object_ActionPositionEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30716, XrefRangeEnd = 30717, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FieldConditionAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse, OperatorEnum op, string fieldOrProperty2, Il2CppSystem.Object compareTo2, bool compareFalse2)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse;
		*(OperatorEnum**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &op;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty2);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo2);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30717, XrefRangeEnd = 30718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FieldConditionAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse, OperatorEnum op, string fieldOrProperty2, Il2CppSystem.Object compareTo2, bool compareFalse2, string fieldOrProperty3, Il2CppSystem.Object compareTo3, bool compareFalse3)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[10];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse;
		*(OperatorEnum**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &op;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty2);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo2);
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse2;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty3);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo3);
		*(bool**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse3;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30718, XrefRangeEnd = 30719, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FieldConditionAttribute(string methodToQuery)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FieldConditionAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(methodToQuery);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FieldConditionAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
