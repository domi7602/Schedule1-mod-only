using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class ShapeOutputSlotInfo : OutputSlotInfo
{
	private static readonly IntPtr NativeFieldInfoPtr_OutputsVariableShape;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	public unsafe bool OutputsVariableShape
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputsVariableShape);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutputsVariableShape)) = flag;
		}
	}

	static ShapeOutputSlotInfo()
	{
		Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "ShapeOutputSlotInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr);
		NativeFieldInfoPtr_OutputsVariableShape = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr, "OutputsVariableShape");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr, 100666502);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr, 100666503);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54744, XrefRangeEnd = 54751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShapeOutputSlotInfo()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54751, XrefRangeEnd = 54758, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ShapeOutputSlotInfo(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ShapeOutputSlotInfo>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ShapeOutputSlotInfo(IntPtr pointer)
		: base(pointer)
	{
	}
}
