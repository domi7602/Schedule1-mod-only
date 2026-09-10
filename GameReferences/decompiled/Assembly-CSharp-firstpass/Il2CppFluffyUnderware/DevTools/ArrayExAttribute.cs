using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class ArrayExAttribute : DTAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_Draggable;

	private static readonly IntPtr NativeFieldInfoPtr_ShowHeader;

	private static readonly IntPtr NativeFieldInfoPtr_ShowAdd;

	private static readonly IntPtr NativeFieldInfoPtr_ShowDelete;

	private static readonly IntPtr NativeFieldInfoPtr_DropTarget;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe bool Draggable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Draggable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Draggable)) = flag;
		}
	}

	public unsafe bool ShowHeader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowHeader);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowHeader)) = flag;
		}
	}

	public unsafe bool ShowAdd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowAdd);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowAdd)) = flag;
		}
	}

	public unsafe bool ShowDelete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowDelete);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ShowDelete)) = flag;
		}
	}

	public unsafe bool DropTarget
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DropTarget);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DropTarget)) = flag;
		}
	}

	static ArrayExAttribute()
	{
		Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "ArrayExAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr);
		NativeFieldInfoPtr_Draggable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, "Draggable");
		NativeFieldInfoPtr_ShowHeader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, "ShowHeader");
		NativeFieldInfoPtr_ShowAdd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, "ShowAdd");
		NativeFieldInfoPtr_ShowDelete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, "ShowDelete");
		NativeFieldInfoPtr_DropTarget = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, "DropTarget");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr, 100663893);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 30713, XrefRangeEnd = 30714, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ArrayExAttribute()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ArrayExAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ArrayExAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
