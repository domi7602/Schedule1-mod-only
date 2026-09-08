using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppScheduleOne;

public class ExitAction : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__Type_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_used;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Type_Public_get_ExitType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Type_Private_set_Void_ExitType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Used_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Used_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_ExitType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Use_Public_Void_0;

	public unsafe ExitType _Type_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField);
			return *(ExitType*)num;
		}
		set
		{
			*(ExitType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField)) = exitType;
		}
	}

	public unsafe bool used
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_used);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_used)) = flag;
		}
	}

	public unsafe ExitType Type
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 29049, RefRangeEnd = 29051, XrefRangeStart = 29049, XrefRangeEnd = 29051, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Type_Public_get_ExitType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(ExitType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 29051, RefRangeEnd = 29056, XrefRangeStart = 29051, XrefRangeEnd = 29056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Type_Private_set_Void_ExitType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool Used
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Used_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(38)]
		[CachedScanResults(RefRangeStart = 89314, RefRangeEnd = 89352, XrefRangeStart = 89314, XrefRangeEnd = 89314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Used_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ExitAction()
	{
		Il2CppClassPointerStore<ExitAction>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "ExitAction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExitAction>.NativeClassPtr);
		NativeFieldInfoPtr__Type_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, "<Type>k__BackingField");
		NativeFieldInfoPtr_used = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, "used");
		NativeMethodInfoPtr_get_Type_Public_get_ExitType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665807);
		NativeMethodInfoPtr_set_Type_Private_set_Void_ExitType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665808);
		NativeMethodInfoPtr_get_Used_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665809);
		NativeMethodInfoPtr_set_Used_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665810);
		NativeMethodInfoPtr__ctor_Public_Void_ExitType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665811);
		NativeMethodInfoPtr_Use_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitAction>.NativeClassPtr, 100665812);
	}

	[CallerCount(83)]
	[CachedScanResults(RefRangeStart = 65267, RefRangeEnd = 65350, XrefRangeStart = 65267, XrefRangeEnd = 65350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExitAction(ExitType type)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExitAction>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_ExitType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 89352, RefRangeEnd = 89362, XrefRangeStart = 89352, XrefRangeEnd = 89352, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Use()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Use_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public ExitAction(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
