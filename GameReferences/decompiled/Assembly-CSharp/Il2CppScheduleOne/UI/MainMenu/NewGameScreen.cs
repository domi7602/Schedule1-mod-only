using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.UI.MainMenu;

public class NewGameScreen : MenuScreen
{
	private static readonly IntPtr NativeFieldInfoPtr_ConfirmOverwriteScreen;

	private static readonly IntPtr NativeFieldInfoPtr_SetupScreen;

	private static readonly IntPtr NativeMethodInfoPtr_SlotSelected_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe ConfirmOverwriteScreen ConfirmOverwriteScreen
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConfirmOverwriteScreen);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ConfirmOverwriteScreen>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ConfirmOverwriteScreen)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)confirmOverwriteScreen));
		}
	}

	public unsafe SetupScreen SetupScreen
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetupScreen);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SetupScreen>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SetupScreen)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)setupScreen));
		}
	}

	static NewGameScreen()
	{
		Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.UI.MainMenu", "NewGameScreen");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr);
		NativeFieldInfoPtr_ConfirmOverwriteScreen = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr, "ConfirmOverwriteScreen");
		NativeFieldInfoPtr_SetupScreen = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr, "SetupScreen");
		NativeMethodInfoPtr_SlotSelected_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr, 100689047);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr, 100689048);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 328117, XrefRangeEnd = 328126, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SlotSelected(int slotIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&slotIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SlotSelected_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 327636, RefRangeEnd = 327637, XrefRangeStart = 327636, XrefRangeEnd = 327637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe NewGameScreen()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NewGameScreen>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public NewGameScreen(IntPtr pointer)
		: base(pointer)
	{
	}
}
