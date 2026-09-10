using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class InspectorNote : DTVersionedMonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_m_Note;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string m_Note
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Note);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_Note)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static InspectorNote()
	{
		Il2CppClassPointerStore<InspectorNote>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "InspectorNote");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InspectorNote>.NativeClassPtr);
		NativeFieldInfoPtr_m_Note = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InspectorNote>.NativeClassPtr, "m_Note");
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InspectorNote>.NativeClassPtr, 100664282);
	}

	[CallerCount(204)]
	[CachedScanResults(RefRangeStart = 152, RefRangeEnd = 356, XrefRangeStart = 152, XrefRangeEnd = 356, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InspectorNote()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InspectorNote>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InspectorNote(IntPtr pointer)
		: base(pointer)
	{
	}
}
