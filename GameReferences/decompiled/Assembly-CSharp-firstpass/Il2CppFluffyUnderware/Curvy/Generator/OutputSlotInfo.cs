using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.Generator;

public class OutputSlotInfo : SlotInfo
{
	private static readonly System.IntPtr NativeMethodInfoPtr_get_DataType_Public_get_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Type_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_Type_0;

	public unsafe Il2CppSystem.Type DataType
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DataType_Public_get_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Type>(intPtr) : null;
		}
	}

	static OutputSlotInfo()
	{
		Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator", "OutputSlotInfo");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr);
		NativeMethodInfoPtr_get_DataType_Public_get_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr, 100666464);
		NativeMethodInfoPtr__ctor_Public_Void_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr, 100666465);
		NativeMethodInfoPtr__ctor_Public_Void_String_Type_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr, 100666466);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 54473, XrefRangeEnd = 54483, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutputSlotInfo(Il2CppSystem.Type type)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 54493, RefRangeEnd = 54495, XrefRangeStart = 54483, XrefRangeEnd = 54493, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe OutputSlotInfo(string name, Il2CppSystem.Type type)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OutputSlotInfo>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_Type_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public OutputSlotInfo(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
