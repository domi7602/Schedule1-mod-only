using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.Employees;

namespace Il2CppScheduleOne.Persistence.Loaders;

public class LegacyEmployeeLoader : LegacyNPCLoader
{
	private static readonly IntPtr NativeMethodInfoPtr_get_NPCType_Public_Virtual_get_String_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LoadAndCreateEmployee_Public_Employee_String_0;

	public unsafe override string NPCType
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 130838, XrefRangeEnd = 130845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_NPCType_Public_Virtual_get_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static LegacyEmployeeLoader()
	{
		Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence.Loaders", "LegacyEmployeeLoader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr);
		NativeMethodInfoPtr_get_NPCType_Public_Virtual_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr, 100669135);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr, 100669136);
		NativeMethodInfoPtr_LoadAndCreateEmployee_Public_Employee_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr, 100669137);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 130514, RefRangeEnd = 130524, XrefRangeStart = 130514, XrefRangeEnd = 130524, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe LegacyEmployeeLoader()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LegacyEmployeeLoader>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 130912, RefRangeEnd = 130916, XrefRangeStart = 130845, XrefRangeEnd = 130912, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Employee LoadAndCreateEmployee(string mainPath)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(mainPath);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadAndCreateEmployee_Public_Employee_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Employee>(intPtr) : null;
	}

	public LegacyEmployeeLoader(IntPtr pointer)
		: base(pointer)
	{
	}
}
