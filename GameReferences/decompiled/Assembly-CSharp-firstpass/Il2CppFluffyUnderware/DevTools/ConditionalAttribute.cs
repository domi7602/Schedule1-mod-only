using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Reflection;

namespace Il2CppFluffyUnderware.DevTools;

public class ConditionalAttribute : ActionAttribute
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "OperatorEnum")]
	public enum OperatorEnum
	{
		AND,
		OR
	}

	public class Condition : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_FieldName;

		private static readonly System.IntPtr NativeFieldInfoPtr_FieldInfo;

		private static readonly System.IntPtr NativeFieldInfoPtr_PropertyInfo;

		private static readonly System.IntPtr NativeFieldInfoPtr_CompareTo;

		private static readonly System.IntPtr NativeFieldInfoPtr_CompareFalse;

		private static readonly System.IntPtr NativeFieldInfoPtr_Operator;

		private static readonly System.IntPtr NativeFieldInfoPtr_MethodInfo;

		private static readonly System.IntPtr NativeFieldInfoPtr_MethodName;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string FieldName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe FieldInfo FieldInfo
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldInfo);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<FieldInfo>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FieldInfo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fieldInfo));
			}
		}

		public unsafe PropertyInfo PropertyInfo
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertyInfo);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PropertyInfo>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PropertyInfo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)propertyInfo));
			}
		}

		public unsafe Il2CppSystem.Object CompareTo
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompareTo);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompareTo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe bool CompareFalse
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompareFalse);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CompareFalse)) = flag;
			}
		}

		public unsafe OperatorEnum Operator
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Operator);
				return *(OperatorEnum*)num;
			}
			set
			{
				*(OperatorEnum*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Operator)) = operatorEnum;
			}
		}

		public unsafe MethodInfo MethodInfo
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MethodInfo);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MethodInfo>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MethodInfo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)methodInfo));
			}
		}

		public unsafe string MethodName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MethodName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MethodName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static Condition()
		{
			Il2CppClassPointerStore<Condition>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, "Condition");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Condition>.NativeClassPtr);
			NativeFieldInfoPtr_FieldName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "FieldName");
			NativeFieldInfoPtr_FieldInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "FieldInfo");
			NativeFieldInfoPtr_PropertyInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "PropertyInfo");
			NativeFieldInfoPtr_CompareTo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "CompareTo");
			NativeFieldInfoPtr_CompareFalse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "CompareFalse");
			NativeFieldInfoPtr_Operator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "Operator");
			NativeFieldInfoPtr_MethodInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "MethodInfo");
			NativeFieldInfoPtr_MethodName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Condition>.NativeClassPtr, "MethodName");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Condition>.NativeClassPtr, 100663884);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Condition()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Condition>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Condition(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_Conditions;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConditionMet_Public_Virtual_New_Boolean_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_evaluate_Private_Boolean_Condition_Object_0;

	public unsafe Il2CppReferenceArray<Condition> Conditions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Conditions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Condition>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Conditions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static ConditionalAttribute()
	{
		Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "ConditionalAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Conditions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, "Conditions");
		NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663878);
		NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663879);
		NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663880);
		NativeMethodInfoPtr__ctor_Protected_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663881);
		NativeMethodInfoPtr_ConditionMet_Public_Virtual_New_Boolean_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663882);
		NativeMethodInfoPtr_evaluate_Private_Boolean_Condition_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr, 100663883);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 30537, RefRangeEnd = 30539, XrefRangeStart = 30519, XrefRangeEnd = 30537, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConditionalAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse = false)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fieldOrProperty);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)compareTo);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &compareFalse;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 30566, RefRangeEnd = 30568, XrefRangeStart = 30539, XrefRangeEnd = 30566, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConditionalAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse, OperatorEnum op, string fieldOrProperty2, Il2CppSystem.Object compareTo2, bool compareFalse2)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr))
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 30604, RefRangeEnd = 30606, XrefRangeStart = 30568, XrefRangeEnd = 30604, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConditionalAttribute(string fieldOrProperty, Il2CppSystem.Object compareTo, bool compareFalse, OperatorEnum op, string fieldOrProperty2, Il2CppSystem.Object compareTo2, bool compareFalse2, string fieldOrProperty3, Il2CppSystem.Object compareTo3, bool compareFalse3)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr))
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_Object_Boolean_OperatorEnum_String_Object_Boolean_String_Object_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 30624, RefRangeEnd = 30626, XrefRangeStart = 30606, XrefRangeEnd = 30624, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ConditionalAttribute(string methodToQuery)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ConditionalAttribute>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(methodToQuery);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30626, XrefRangeEnd = 30631, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual bool ConditionMet(Il2CppSystem.Object classInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)classInstance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ConditionMet_Public_Virtual_New_Boolean_Object_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 30662, RefRangeEnd = 30664, XrefRangeStart = 30631, XrefRangeEnd = 30662, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool evaluate(Condition cond, Il2CppSystem.Object classInstance)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cond);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)classInstance);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_evaluate_Private_Boolean_Condition_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ConditionalAttribute(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
