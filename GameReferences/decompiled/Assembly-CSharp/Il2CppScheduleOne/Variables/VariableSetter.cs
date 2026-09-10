using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne.Variables;

[System.Serializable]
public class VariableSetter : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_VariableName;

	private static readonly System.IntPtr NativeFieldInfoPtr_NewValue;

	private static readonly System.IntPtr NativeMethodInfoPtr_Execute_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string VariableName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VariableName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_VariableName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string NewValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NewValue);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NewValue)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	static VariableSetter()
	{
		Il2CppClassPointerStore<VariableSetter>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Variables", "VariableSetter");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr);
		NativeFieldInfoPtr_VariableName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr, "VariableName");
		NativeFieldInfoPtr_NewValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr, "NewValue");
		NativeMethodInfoPtr_Execute_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr, 100666573);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr, 100666574);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 98078, RefRangeEnd = 98079, XrefRangeStart = 98073, XrefRangeEnd = 98078, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Execute()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Execute_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe VariableSetter()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<VariableSetter>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public VariableSetter(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
