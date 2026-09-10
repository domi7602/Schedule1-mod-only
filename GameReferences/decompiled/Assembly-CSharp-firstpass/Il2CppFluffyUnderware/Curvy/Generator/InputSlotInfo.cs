using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public sealed class InputSlotInfo : SlotInfo
{
	private static readonly System.IntPtr NativeFieldInfoPtr_RequestDataOnly;

	private static readonly System.IntPtr NativeFieldInfoPtr_Optional;

	private static readonly System.IntPtr NativeFieldInfoPtr_ModifiesData;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsValidFrom_Public_Boolean_Type_0;

	public unsafe bool RequestDataOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestDataOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequestDataOnly)) = flag;
		}
	}

	public unsafe bool Optional
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Optional);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Optional)) = flag;
		}
	}

	public unsafe bool ModifiesData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModifiesData);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ModifiesData)) = flag;
		}
	}

	static InputSlotInfo()
	{
		Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "InputSlotInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr);
		NativeFieldInfoPtr_RequestDataOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, "RequestDataOnly");
		NativeFieldInfoPtr_Optional = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, "Optional");
		NativeFieldInfoPtr_ModifiesData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, "ModifiesData");
		NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, 100666457);
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, 100666458);
		NativeMethodInfoPtr_IsValidFrom_Public_Boolean_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr, 100666459);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54461, XrefRangeEnd = 54462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputSlotInfo(string name, [Optional] Il2CppReferenceArray<Il2CppSystem.Type> type)
	{
		if (type == null)
		{
			type = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Il2CppReferenceArray_1_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54462, XrefRangeEnd = 54463, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputSlotInfo([Optional] Il2CppReferenceArray<Il2CppSystem.Type> type)
	{
		if (type == null)
		{
			type = new Il2CppReferenceArray<Il2CppSystem.Type>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputSlotInfo>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54463, XrefRangeEnd = 54468, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsValidFrom(Il2CppSystem.Type outType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)outType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsValidFrom_Public_Boolean_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public InputSlotInfo(string name, params Il2CppSystem.Type[] type)
		: this(name, new Il2CppReferenceArray<Il2CppSystem.Type>(type))
	{
	}

	public InputSlotInfo(params Il2CppSystem.Type[] type)
		: this(new Il2CppReferenceArray<Il2CppSystem.Type>(type))
	{
	}

	public InputSlotInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
