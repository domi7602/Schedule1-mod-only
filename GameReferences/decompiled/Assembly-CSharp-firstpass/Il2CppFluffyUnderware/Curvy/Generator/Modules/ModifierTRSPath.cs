using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class ModifierTRSPath : TRSModuleBase
{
	private static readonly IntPtr NativeFieldInfoPtr_InPath;

	private static readonly IntPtr NativeFieldInfoPtr_OutPath;

	private static readonly IntPtr NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSlotDataRequest_Public_Virtual_Final_New_Il2CppReferenceArray_1_CGData_CGModuleInputSlot_CGModuleOutputSlot_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CGModuleInputSlot InPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InPath);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleOutputSlot OutPath
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutPath);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutPath)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleOutputSlot));
		}
	}

	public unsafe virtual bool PathIsClosed
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59012, XrefRangeEnd = 59013, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static ModifierTRSPath()
	{
		Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "ModifierTRSPath");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr);
		NativeFieldInfoPtr_InPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr, "InPath");
		NativeFieldInfoPtr_OutPath = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr, "OutPath");
		NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr, 100667049);
		NativeMethodInfoPtr_OnSlotDataRequest_Public_Virtual_Final_New_Il2CppReferenceArray_1_CGData_CGModuleInputSlot_CGModuleOutputSlot_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr, 100667050);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr, 100667051);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59013, XrefRangeEnd = 59028, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Il2CppReferenceArray<CGData> OnSlotDataRequest(CGModuleInputSlot requestedBy, CGModuleOutputSlot requestedSlot, [Optional] Il2CppReferenceArray<CGDataRequestParameter> requests)
	{
		if (requests == null)
		{
			requests = new Il2CppReferenceArray<CGDataRequestParameter>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requestedBy);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requestedSlot);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)requests);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSlotDataRequest_Public_Virtual_Final_New_Il2CppReferenceArray_1_CGData_CGModuleInputSlot_CGModuleOutputSlot_Il2CppReferenceArray_1_CGDataRequestParameter_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<CGData>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59028, XrefRangeEnd = 59039, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ModifierTRSPath()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ModifierTRSPath>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public virtual Il2CppReferenceArray<CGData> OnSlotDataRequest(CGModuleInputSlot requestedBy, CGModuleOutputSlot requestedSlot, params CGDataRequestParameter[] requests)
	{
		return OnSlotDataRequest(requestedBy, requestedSlot, new Il2CppReferenceArray<CGDataRequestParameter>(requests));
	}

	public ModifierTRSPath(IntPtr pointer)
		: base(pointer)
	{
	}
}
