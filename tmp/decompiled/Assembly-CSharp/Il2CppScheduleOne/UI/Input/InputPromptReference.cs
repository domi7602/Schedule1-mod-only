using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.UI.Input;

public class InputPromptReference : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Id;

	private static readonly System.IntPtr NativeFieldInfoPtr_PositionCategory;

	private static readonly System.IntPtr NativeFieldInfoPtr_CanvasSortingOrder;

	private static readonly System.IntPtr NativeFieldInfoPtr_CustomPosition;

	private static readonly System.IntPtr NativeFieldInfoPtr_DisplayTextOverride;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_EInputPromptPosition_String_Vector3_Int32_0;

	public unsafe string Id
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Id)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EInputPromptPosition PositionCategory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PositionCategory);
			return *(EInputPromptPosition*)num;
		}
		set
		{
			*(EInputPromptPosition*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PositionCategory)) = eInputPromptPosition;
		}
	}

	public unsafe int CanvasSortingOrder
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanvasSortingOrder);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CanvasSortingOrder)) = num;
		}
	}

	public unsafe Vector3 CustomPosition
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomPosition);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CustomPosition)) = vector;
		}
	}

	public unsafe string DisplayTextOverride
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayTextOverride);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DisplayTextOverride)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static InputPromptReference()
	{
		Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.Input", "InputPromptReference");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr);
		NativeFieldInfoPtr_Id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, "Id");
		NativeFieldInfoPtr_PositionCategory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, "PositionCategory");
		NativeFieldInfoPtr_CanvasSortingOrder = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, "CanvasSortingOrder");
		NativeFieldInfoPtr_CustomPosition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, "CustomPosition");
		NativeFieldInfoPtr_DisplayTextOverride = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, "DisplayTextOverride");
		NativeMethodInfoPtr__ctor_Public_Void_String_EInputPromptPosition_String_Vector3_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr, 100689177);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 330199, RefRangeEnd = 330205, XrefRangeStart = 330194, XrefRangeEnd = 330199, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputPromptReference(string id, EInputPromptPosition positionCategory, string displayTextOverride, Vector3 customPosition = default(Vector3), int canvasSortingOrder = 1)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InputPromptReference>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(id);
		*(EInputPromptPosition**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &positionCategory;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(displayTextOverride);
		*(Vector3**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &customPosition;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &canvasSortingOrder;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_EInputPromptPosition_String_Vector3_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InputPromptReference(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
