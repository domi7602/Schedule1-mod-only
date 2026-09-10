using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Text;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Il2Cpp;

public class VirtualMouseDebugger : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_msg;

	private static readonly IntPtr NativeFieldInfoPtr_vmi;

	private static readonly IntPtr NativeFieldInfoPtr_systemMouse;

	private static readonly IntPtr NativeFieldInfoPtr_virtualMouse;

	private static readonly IntPtr NativeFieldInfoPtr_sb;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe TMP_Text msg
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_msg);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TMP_Text>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_msg)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tMP_Text));
		}
	}

	public unsafe VirtualMouseInput vmi
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vmi);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VirtualMouseInput>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vmi)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)virtualMouseInput));
		}
	}

	public unsafe Mouse systemMouse
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_systemMouse);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mouse>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_systemMouse)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mouse));
		}
	}

	public unsafe Mouse virtualMouse
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_virtualMouse);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mouse>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_virtualMouse)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mouse));
		}
	}

	public unsafe StringBuilder sb
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sb);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StringBuilder>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sb)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stringBuilder));
		}
	}

	static VirtualMouseDebugger()
	{
		Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "VirtualMouseDebugger");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr);
		NativeFieldInfoPtr_msg = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, "msg");
		NativeFieldInfoPtr_vmi = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, "vmi");
		NativeFieldInfoPtr_systemMouse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, "systemMouse");
		NativeFieldInfoPtr_virtualMouse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, "virtualMouse");
		NativeFieldInfoPtr_sb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, "sb");
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, 100663297);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, 100663298);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr, 100663299);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64731, XrefRangeEnd = 64746, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64746, XrefRangeEnd = 64806, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 64806, XrefRangeEnd = 64812, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VirtualMouseDebugger()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VirtualMouseDebugger>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VirtualMouseDebugger(IntPtr pointer)
		: base(pointer)
	{
	}
}
