using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.ObjectScripts;

namespace Il2CppScheduleOne.Growing;

public class PotInteraction : GrowContainerInteraction
{
	private static readonly IntPtr NativeFieldInfoPtr__pot;

	private static readonly IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_TryGetFallbackInteractionMessage_Protected_Virtual_Boolean_byref_String_byref_EInteractableState_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Pot _pot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Pot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pot));
		}
	}

	static PotInteraction()
	{
		Il2CppClassPointerStore<PotInteraction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Growing", "PotInteraction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr);
		NativeFieldInfoPtr__pot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr, "_pot");
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr, 100678573);
		NativeMethodInfoPtr_TryGetFallbackInteractionMessage_Protected_Virtual_Boolean_byref_String_byref_EInteractableState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr, 100678574);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr, 100678575);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230368, XrefRangeEnd = 230369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230369, XrefRangeEnd = 230382, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override bool TryGetFallbackInteractionMessage(out string message, out InteractableObject.EInteractableState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		nint num = 0;
		*ptr = (nint)(&num);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_TryGetFallbackInteractionMessage_Protected_Virtual_Boolean_byref_String_byref_EInteractableState_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		message = IL2CPP.Il2CppStringToManaged((IntPtr)num);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 230382, XrefRangeEnd = 230383, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PotInteraction()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PotInteraction>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PotInteraction(IntPtr pointer)
		: base(pointer)
	{
	}
}
