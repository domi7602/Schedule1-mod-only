using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.State;

namespace Il2CppScheduleOne.UI;

public class UIScreenMonoState : MonoState
{
	private static readonly IntPtr NativeFieldInfoPtr__screen;

	private static readonly IntPtr NativeFieldInfoPtr__attachToPlayerInventory;

	private static readonly IntPtr NativeFieldInfoPtr__hasBeenActivatedSinceAddedToStack;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDeactivate_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_NotifyRemovedFromStack_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe UIScreen _screen
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__screen);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UIScreen>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__screen)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uIScreen));
		}
	}

	public unsafe bool _attachToPlayerInventory
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__attachToPlayerInventory);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__attachToPlayerInventory)) = flag;
		}
	}

	public unsafe bool _hasBeenActivatedSinceAddedToStack
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasBeenActivatedSinceAddedToStack);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasBeenActivatedSinceAddedToStack)) = flag;
		}
	}

	static UIScreenMonoState()
	{
		Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI", "UIScreenMonoState");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr);
		NativeFieldInfoPtr__screen = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, "_screen");
		NativeFieldInfoPtr__attachToPlayerInventory = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, "_attachToPlayerInventory");
		NativeFieldInfoPtr__hasBeenActivatedSinceAddedToStack = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, "_hasBeenActivatedSinceAddedToStack");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, 100686279);
		NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, 100686280);
		NativeMethodInfoPtr_OnDeactivate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, 100686281);
		NativeMethodInfoPtr_NotifyRemovedFromStack_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, 100686282);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr, 100686283);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298041, XrefRangeEnd = 298042, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298042, XrefRangeEnd = 298053, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnActivate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnActivate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298053, XrefRangeEnd = 298064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDeactivate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDeactivate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298064, XrefRangeEnd = 298065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void NotifyRemovedFromStack()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_NotifyRemovedFromStack_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 298065, XrefRangeEnd = 298066, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe UIScreenMonoState()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<UIScreenMonoState>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public UIScreenMonoState(IntPtr pointer)
		: base(pointer)
	{
	}
}
