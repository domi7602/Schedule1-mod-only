using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class SlotInfo : Il2CppSystem.Attribute
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "SlotArrayType")]
	public enum SlotArrayType
	{
		Unknown,
		Normal,
		Hidden
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_DataTypes;

	private static readonly System.IntPtr NativeFieldInfoPtr_Name;

	private static readonly System.IntPtr NativeFieldInfoPtr_displayName;

	private static readonly System.IntPtr NativeFieldInfoPtr_Tooltip;

	private static readonly System.IntPtr NativeFieldInfoPtr_Array;

	private static readonly System.IntPtr NativeFieldInfoPtr_ArrayType;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DisplayName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_DisplayName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_String_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Protected_Void_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CheckDataTypes_Public_Void_0;

	public unsafe Il2CppReferenceArray<Il2CppSystem.Type> DataTypes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataTypes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Il2CppSystem.Type>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DataTypes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
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

	public unsafe string displayName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string Tooltip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tooltip);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Tooltip)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool Array
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Array);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Array)) = flag;
		}
	}

	public unsafe SlotArrayType ArrayType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ArrayType);
			return *(SlotArrayType*)num;
		}
		set
		{
			*(SlotArrayType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ArrayType)) = slotArrayType;
		}
	}

	public unsafe string DisplayName
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 54758, RefRangeEnd = 54759, XrefRangeStart = 54758, XrefRangeEnd = 54758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DisplayName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DisplayName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SlotInfo()
	{
		Il2CppClassPointerStore<SlotInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "SlotInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr);
		NativeFieldInfoPtr_DataTypes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "DataTypes");
		NativeFieldInfoPtr_Name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "Name");
		NativeFieldInfoPtr_displayName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "displayName");
		NativeFieldInfoPtr_Tooltip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "Tooltip");
		NativeFieldInfoPtr_Array = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "Array");
		NativeFieldInfoPtr_ArrayType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, "ArrayType");
		NativeMethodInfoPtr_get_DisplayName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666504);
		NativeMethodInfoPtr_set_DisplayName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666505);
		NativeMethodInfoPtr__ctor_Protected_Void_String_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666506);
		NativeMethodInfoPtr__ctor_Protected_Void_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666507);
		NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666508);
		NativeMethodInfoPtr_CheckDataTypes_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr, 100666509);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 54762, RefRangeEnd = 54766, XrefRangeStart = 54759, XrefRangeEnd = 54762, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SlotInfo(string name, [Optional] Il2CppReferenceArray<Il2CppSystem.Type> type)
	{
		if (type == null)
		{
			type = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_String_Il2CppReferenceArray_1_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54766, XrefRangeEnd = 54769, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SlotInfo([Optional] Il2CppReferenceArray<Il2CppSystem.Type> type)
	{
		if (type == null)
		{
			type = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotInfo>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_Il2CppReferenceArray_1_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54769, XrefRangeEnd = 54774, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual int CompareTo(Il2CppSystem.Object obj)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CompareTo_Public_Virtual_Final_New_Int32_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 54792, RefRangeEnd = 54793, XrefRangeStart = 54774, XrefRangeEnd = 54792, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CheckDataTypes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckDataTypes_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SlotInfo(string name, params Il2CppSystem.Type[] type)
		: this(name, new Il2CppReferenceArray<Il2CppSystem.Type>(type))
	{
	}

	public SlotInfo(params Il2CppSystem.Type[] type)
		: this(new Il2CppReferenceArray<Il2CppSystem.Type>(type))
	{
	}

	public SlotInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
