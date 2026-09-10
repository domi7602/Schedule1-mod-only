using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Il2CppScheduleOne.Interaction;

public class InteractableObject : MonoBehaviour
{
	[OriginalName("Assembly-CSharp.dll", "", "EInteractionType")]
	public enum EInteractionType
	{
		Key_Press,
		LeftMouse_Click
	}

	[OriginalName("Assembly-CSharp.dll", "", "EInteractableState")]
	public enum EInteractableState
	{
		Default,
		Invalid,
		Disabled,
		Label
	}

	private static readonly IntPtr NativeFieldInfoPtr_message;

	private static readonly IntPtr NativeFieldInfoPtr_interactionType;

	private static readonly IntPtr NativeFieldInfoPtr_interactionState;

	private static readonly IntPtr NativeFieldInfoPtr_MaxInteractionRange;

	private static readonly IntPtr NativeFieldInfoPtr_RequiresUniqueClick;

	private static readonly IntPtr NativeFieldInfoPtr_Priority;

	private static readonly IntPtr NativeFieldInfoPtr_displayLocationCollider;

	private static readonly IntPtr NativeFieldInfoPtr_displayLocationPoint;

	private static readonly IntPtr NativeFieldInfoPtr_LimitInteractionAngle;

	private static readonly IntPtr NativeFieldInfoPtr_AngleLimit;

	private static readonly IntPtr NativeFieldInfoPtr_onHovered;

	private static readonly IntPtr NativeFieldInfoPtr_onInteractStart;

	private static readonly IntPtr NativeFieldInfoPtr_onInteractEnd;

	private static readonly IntPtr NativeFieldInfoPtr__isMessageActive;

	private static readonly IntPtr NativeFieldInfoPtr__currentBindingData;

	private static readonly IntPtr NativeFieldInfoPtr__descriptorData;

	private static readonly IntPtr NativeMethodInfoPtr_get__interactionType_Public_get_EInteractionType_0;

	private static readonly IntPtr NativeMethodInfoPtr_get__interactionState_Public_get_EInteractableState_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetInteractionType_Public_Void_EInteractionType_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetInteractableState_Public_Void_EInteractableState_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetMessage_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Hovered_Public_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Exited_Public_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_StartInteract_Public_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_EndInteract_Public_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ShowMessage_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_CheckAngleLimit_Public_Boolean_Vector3_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetInputData_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnInputChange_Private_Void_InputDeviceType_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe string message
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_message);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_message)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EInteractionType interactionType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interactionType);
			return *(EInteractionType*)num;
		}
		set
		{
			*(EInteractionType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interactionType)) = eInteractionType;
		}
	}

	public unsafe EInteractableState interactionState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interactionState);
			return *(EInteractableState*)num;
		}
		set
		{
			*(EInteractableState*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interactionState)) = eInteractableState;
		}
	}

	public unsafe float MaxInteractionRange
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxInteractionRange);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_MaxInteractionRange)) = num;
		}
	}

	public unsafe bool RequiresUniqueClick
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiresUniqueClick);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RequiresUniqueClick)) = flag;
		}
	}

	public unsafe int Priority
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Priority);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Priority)) = num;
		}
	}

	public unsafe Collider displayLocationCollider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayLocationCollider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Collider>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayLocationCollider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider));
		}
	}

	public unsafe Transform displayLocationPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayLocationPoint);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_displayLocationPoint)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool LimitInteractionAngle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LimitInteractionAngle);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LimitInteractionAngle)) = flag;
		}
	}

	public unsafe float AngleLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleLimit);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AngleLimit)) = num;
		}
	}

	public unsafe UnityEvent onHovered
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onHovered);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onHovered)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onInteractStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInteractStart);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInteractStart)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe UnityEvent onInteractEnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInteractEnd);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UnityEvent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_onInteractEnd)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityEvent));
		}
	}

	public unsafe bool _isMessageActive
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isMessageActive);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isMessageActive)) = flag;
		}
	}

	public unsafe InputPromptsBindingData _currentBindingData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentBindingData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<InputPromptsBindingData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentBindingData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputPromptsBindingData));
		}
	}

	public unsafe InputPromptsDescriptorData _descriptorData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__descriptorData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<InputPromptsDescriptorData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__descriptorData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputPromptsDescriptorData));
		}
	}

	public unsafe EInteractionType _interactionType
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 29072, RefRangeEnd = 29101, XrefRangeStart = 29072, XrefRangeEnd = 29101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__interactionType_Public_get_EInteractionType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EInteractionType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe EInteractableState _interactionState
	{
		[CallerCount(27)]
		[CachedScanResults(RefRangeStart = 70643, RefRangeEnd = 70670, XrefRangeStart = 70643, XrefRangeEnd = 70670, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get__interactionState_Public_get_EInteractableState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EInteractableState*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static InteractableObject()
	{
		Il2CppClassPointerStore<InteractableObject>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Interaction", "InteractableObject");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr);
		NativeFieldInfoPtr_message = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "message");
		NativeFieldInfoPtr_interactionType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "interactionType");
		NativeFieldInfoPtr_interactionState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "interactionState");
		NativeFieldInfoPtr_MaxInteractionRange = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "MaxInteractionRange");
		NativeFieldInfoPtr_RequiresUniqueClick = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "RequiresUniqueClick");
		NativeFieldInfoPtr_Priority = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "Priority");
		NativeFieldInfoPtr_displayLocationCollider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "displayLocationCollider");
		NativeFieldInfoPtr_displayLocationPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "displayLocationPoint");
		NativeFieldInfoPtr_LimitInteractionAngle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "LimitInteractionAngle");
		NativeFieldInfoPtr_AngleLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "AngleLimit");
		NativeFieldInfoPtr_onHovered = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "onHovered");
		NativeFieldInfoPtr_onInteractStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "onInteractStart");
		NativeFieldInfoPtr_onInteractEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "onInteractEnd");
		NativeFieldInfoPtr__isMessageActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "_isMessageActive");
		NativeFieldInfoPtr__currentBindingData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "_currentBindingData");
		NativeFieldInfoPtr__descriptorData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, "_descriptorData");
		NativeMethodInfoPtr_get__interactionType_Public_get_EInteractionType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672313);
		NativeMethodInfoPtr_get__interactionState_Public_get_EInteractableState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672314);
		NativeMethodInfoPtr_Start_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672315);
		NativeMethodInfoPtr_SetInteractionType_Public_Void_EInteractionType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672316);
		NativeMethodInfoPtr_SetInteractableState_Public_Void_EInteractableState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672317);
		NativeMethodInfoPtr_SetMessage_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672318);
		NativeMethodInfoPtr_Hovered_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672319);
		NativeMethodInfoPtr_Exited_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672320);
		NativeMethodInfoPtr_StartInteract_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672321);
		NativeMethodInfoPtr_EndInteract_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672322);
		NativeMethodInfoPtr_ShowMessage_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672323);
		NativeMethodInfoPtr_CheckAngleLimit_Public_Boolean_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672324);
		NativeMethodInfoPtr_SetInputData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672325);
		NativeMethodInfoPtr_OnInputChange_Private_Void_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672326);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672327);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr, 100672328);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165183, XrefRangeEnd = 165205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 29101, RefRangeEnd = 29102, XrefRangeStart = 29101, XrefRangeEnd = 29102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInteractionType(EInteractionType type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInteractionType_Public_Void_EInteractionType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165205, RefRangeEnd = 165206, XrefRangeStart = 165205, XrefRangeEnd = 165205, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInteractableState(EInteractableState state)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&state);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInteractableState_Public_Void_EInteractableState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMessage(string _message)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(_message);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMessage_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165208, RefRangeEnd = 165209, XrefRangeStart = 165206, XrefRangeEnd = 165208, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Hovered()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Hovered_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual void Exited()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Exited_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165215, RefRangeEnd = 165216, XrefRangeStart = 165209, XrefRangeEnd = 165215, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void StartInteract()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_StartInteract_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165216, XrefRangeEnd = 165223, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void EndInteract()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_EndInteract_Public_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165223, XrefRangeEnd = 165250, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void ShowMessage()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ShowMessage_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165259, RefRangeEnd = 165260, XrefRangeStart = 165250, XrefRangeEnd = 165259, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool CheckAngleLimit(Vector3 interactionSource)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&interactionSource);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CheckAngleLimit_Public_Boolean_Vector3_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 165281, RefRangeEnd = 165283, XrefRangeStart = 165260, XrefRangeEnd = 165281, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInputData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInputData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165283, XrefRangeEnd = 165292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInputChange(GameInput.InputDeviceType deviceType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&deviceType);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInputChange_Private_Void_InputDeviceType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 165292, XrefRangeEnd = 165314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 165332, RefRangeEnd = 165333, XrefRangeStart = 165314, XrefRangeEnd = 165332, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InteractableObject()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<InteractableObject>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public InteractableObject(IntPtr pointer)
		: base(pointer)
	{
	}
}
