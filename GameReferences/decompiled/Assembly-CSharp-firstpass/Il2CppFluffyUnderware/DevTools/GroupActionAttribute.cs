using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppFluffyUnderware.DevTools;

public class GroupActionAttribute : ActionAttribute
{
	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_ActionEnum_0;

	static GroupActionAttribute()
	{
		Il2CppClassPointerStore<GroupActionAttribute>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "GroupActionAttribute");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GroupActionAttribute>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_String_ActionEnum_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GroupActionAttribute>.NativeClassPtr, 100663895);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GroupActionAttribute(string actionData, ActionEnum action = ActionEnum.Callback)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GroupActionAttribute>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(actionData);
		*(ActionEnum**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &action;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_ActionEnum_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GroupActionAttribute(IntPtr pointer)
		: base(pointer)
	{
	}
}
