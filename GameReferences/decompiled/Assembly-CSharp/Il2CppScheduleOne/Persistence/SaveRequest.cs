using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Persistence;

public class SaveRequest : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_Saveable;

	private static readonly System.IntPtr NativeFieldInfoPtr_ParentFolderPath;

	private static readonly System.IntPtr NativeFieldInfoPtr__SaveString_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SaveString_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_SaveString_Private_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_ISaveable_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Complete_Public_Void_0;

	public unsafe ISaveable Saveable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saveable);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISaveable>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Saveable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)saveable));
		}
	}

	public unsafe string ParentFolderPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParentFolderPath);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ParentFolderPath)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _SaveString_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SaveString_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SaveString_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string SaveString
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SaveString_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SaveString_Private_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SaveRequest()
	{
		Il2CppClassPointerStore<SaveRequest>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Persistence", "SaveRequest");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr);
		NativeFieldInfoPtr_Saveable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, "Saveable");
		NativeFieldInfoPtr_ParentFolderPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, "ParentFolderPath");
		NativeFieldInfoPtr__SaveString_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, "<SaveString>k__BackingField");
		NativeMethodInfoPtr_get_SaveString_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, 100668966);
		NativeMethodInfoPtr_set_SaveString_Private_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, 100668967);
		NativeMethodInfoPtr__ctor_Public_Void_ISaveable_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, 100668968);
		NativeMethodInfoPtr_Complete_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr, 100668969);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 128100, RefRangeEnd = 128107, XrefRangeStart = 128066, XrefRangeEnd = 128100, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SaveRequest(ISaveable saveable, string parentFolderPath)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SaveRequest>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)saveable);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(parentFolderPath);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ISaveable_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 128119, RefRangeEnd = 128120, XrefRangeStart = 128107, XrefRangeEnd = 128119, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Complete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Complete_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SaveRequest(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
