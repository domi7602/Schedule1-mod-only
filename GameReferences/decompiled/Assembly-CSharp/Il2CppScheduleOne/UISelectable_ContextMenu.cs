using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppTMPro;

namespace Il2CppScheduleOne;

public class UISelectable_ContextMenu : UISelectable
{
	private static readonly IntPtr NativeFieldInfoPtr_labelText;

	private static readonly IntPtr NativeMethodInfoPtr_Setup_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe TMP_Text labelText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_labelText);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TMP_Text>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_labelText)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tMP_Text));
		}
	}

	static UISelectable_ContextMenu()
	{
		Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "UISelectable_ContextMenu");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr);
		NativeFieldInfoPtr_labelText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr, "labelText");
		NativeMethodInfoPtr_Setup_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr, 100665367);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr, 100665368);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 84801, XrefRangeEnd = 84805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Setup(string label)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(label);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Setup_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 84806, RefRangeEnd = 84807, XrefRangeStart = 84805, XrefRangeEnd = 84806, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UISelectable_ContextMenu()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UISelectable_ContextMenu>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UISelectable_ContextMenu(IntPtr pointer)
		: base(pointer)
	{
	}
}
