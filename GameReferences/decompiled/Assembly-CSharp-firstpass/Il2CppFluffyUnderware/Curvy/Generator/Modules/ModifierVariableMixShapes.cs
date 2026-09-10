using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppFluffyUnderware.Curvy.Generator.Modules;

public class ModifierVariableMixShapes : CGModule
{
	private static readonly IntPtr NativeFieldInfoPtr_InShapeA;

	private static readonly IntPtr NativeFieldInfoPtr_InShapeB;

	private static readonly IntPtr NativeFieldInfoPtr_OutShape;

	private static readonly IntPtr NativeFieldInfoPtr_m_MixCurve;

	private static readonly IntPtr NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MixCurve_Public_get_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_MixCurve_Public_set_Void_AnimationCurve_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSlotDataRequest_Public_Virtual_Final_New_Il2CppReferenceArray_1_CGData_CGModuleInputSlot_CGModuleOutputSlot_Il2CppReferenceArray_1_CGDataRequestParameter_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe CGModuleInputSlot InShapeA
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InShapeA);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InShapeA)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleInputSlot InShapeB
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InShapeB);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleInputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InShapeB)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleInputSlot));
		}
	}

	public unsafe CGModuleOutputSlot OutShape
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutShape);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CGModuleOutputSlot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OutShape)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cGModuleOutputSlot));
		}
	}

	public unsafe AnimationCurve m_MixCurve
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MixCurve);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_m_MixCurve)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationCurve));
		}
	}

	public unsafe virtual bool PathIsClosed
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59061, XrefRangeEnd = 59070, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe AnimationCurve MixCurve
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MixCurve_Public_get_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationCurve>(intPtr) : null;
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MixCurve_Public_set_Void_AnimationCurve_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ModifierVariableMixShapes()
	{
		Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.Generator.Modules", "ModifierVariableMixShapes");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr);
		NativeFieldInfoPtr_InShapeA = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, "InShapeA");
		NativeFieldInfoPtr_InShapeB = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, "InShapeB");
		NativeFieldInfoPtr_OutShape = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, "OutShape");
		NativeFieldInfoPtr_m_MixCurve = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, "m_MixCurve");
		NativeMethodInfoPtr_get_PathIsClosed_Public_Virtual_Final_New_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667055);
		NativeMethodInfoPtr_get_MixCurve_Public_get_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667056);
		NativeMethodInfoPtr_set_MixCurve_Public_set_Void_AnimationCurve_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667057);
		NativeMethodInfoPtr_Reset_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667058);
		NativeMethodInfoPtr_OnSlotDataRequest_Public_Virtual_Final_New_Il2CppReferenceArray_1_CGData_CGModuleInputSlot_CGModuleOutputSlot_Il2CppReferenceArray_1_CGDataRequestParameter_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667059);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr, 100667060);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59070, XrefRangeEnd = 59073, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59073, XrefRangeEnd = 59098, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 59098, XrefRangeEnd = 59115, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ModifierVariableMixShapes()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ModifierVariableMixShapes>.NativeClassPtr))
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

	public ModifierVariableMixShapes(IntPtr pointer)
		: base(pointer)
	{
	}
}
