using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Economy;

public class CustomerSatisfaction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetRelationshipChange_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static CustomerSatisfaction()
	{
		Il2CppClassPointerStore<CustomerSatisfaction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Economy", "CustomerSatisfaction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CustomerSatisfaction>.NativeClassPtr);
		NativeMethodInfoPtr_GetRelationshipChange_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomerSatisfaction>.NativeClassPtr, 100674255);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomerSatisfaction>.NativeClassPtr, 100674256);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 186522, RefRangeEnd = 186523, XrefRangeStart = 186522, XrefRangeEnd = 186522, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetRelationshipChange(float satisfaction)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&satisfaction);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRelationshipChange_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CustomerSatisfaction()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CustomerSatisfaction>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CustomerSatisfaction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
