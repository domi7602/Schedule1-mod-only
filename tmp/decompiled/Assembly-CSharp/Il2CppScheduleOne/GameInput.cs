using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.UI.Input;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Il2CppScheduleOne;

public class GameInput : PersistentSingleton<GameInput>
{
	[OriginalName("Assembly-CSharp.dll", "", "ButtonCode")]
	public enum ButtonCode
	{
		PrimaryClick,
		SecondaryClick,
		TertiaryClick,
		Forward,
		Backward,
		Left,
		Right,
		Jump,
		Crouch,
		Sprint,
		Interact,
		Submit,
		VehicleToggleLights,
		VehicleHandbrake,
		Reload,
		InventoryLeft,
		InventoryRight,
		VehicleResetCamera,
		SkateboardDismount,
		SkateboardMount
	}

	[OriginalName("Assembly-CSharp.dll", "", "InputDeviceType")]
	public enum InputDeviceType
	{
		KeyboardMouse,
		Gamepad
	}

	public class ExitListener : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_listenerFunction;

		private static readonly System.IntPtr NativeFieldInfoPtr_priority;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe ExitDelegate listenerFunction
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_listenerFunction);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExitDelegate>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_listenerFunction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exitDelegate));
			}
		}

		public unsafe int priority
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_priority);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_priority)) = num;
			}
		}

		static ExitListener()
		{
			Il2CppClassPointerStore<ExitListener>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "ExitListener");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExitListener>.NativeClassPtr);
			NativeFieldInfoPtr_listenerFunction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExitListener>.NativeClassPtr, "listenerFunction");
			NativeFieldInfoPtr_priority = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExitListener>.NativeClassPtr, "priority");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitListener>.NativeClassPtr, 100665903);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ExitListener()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExitListener>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ExitListener(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class ExitDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ExitAction_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ExitAction_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static ExitDelegate()
		{
			Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "ExitDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr, 100665904);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ExitAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr, 100665905);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ExitAction_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr, 100665906);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr, 100665907);
		}

		[CallerCount(628)]
		[CachedScanResults(RefRangeStart = 71168, RefRangeEnd = 71796, XrefRangeStart = 71168, XrefRangeEnd = 71796, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ExitDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ExitDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual void Invoke(ExitAction exitAction)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exitAction);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_ExitAction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 71797, RefRangeEnd = 71798, XrefRangeStart = 71797, XrefRangeEnd = 71798, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(ExitAction exitAction, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exitAction);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_ExitAction_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ExitDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator ExitDelegate(System.Action<ExitAction> P_0)
		{
			return DelegateSupport.ConvertDelegate<ExitDelegate>((System.Delegate)P_0);
		}

		public static ExitDelegate operator +(ExitDelegate P_0, ExitDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<ExitDelegate>();
		}

		public static ExitDelegate operator -(ExitDelegate P_0, ExitDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<ExitDelegate>();
			}
			return (ExitDelegate)obj;
		}
	}

	[System.Serializable]
	[ObfuscatedName("ScheduleOne.GameInput+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__86_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Start_b__86_0_Internal_Void_0;

		public unsafe static __c __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c));
			}
		}

		public unsafe static UnityAction __9__86_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__86_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<UnityAction>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__86_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)unityAction));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__86_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__86_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665909);
			NativeMethodInfoPtr__Start_b__86_0_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100665910);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89362, XrefRangeEnd = 89368, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _Start_b__86_0()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Start_b__86_0_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ScheduleOne.GameInput+<>c__DisplayClass137_0")]
	public sealed class __c__DisplayClass137_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_primaryExitOnly;

		private static readonly System.IntPtr NativeFieldInfoPtr_condition;

		private static readonly System.IntPtr NativeFieldInfoPtr_exitMethod;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__RegisterExitListener_b__0_Internal_Void_ExitAction_0;

		public unsafe bool primaryExitOnly
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryExitOnly);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_primaryExitOnly)) = flag;
			}
		}

		public unsafe Il2CppSystem.Func<bool> condition
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_condition);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<bool>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_condition)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe Il2CppSystem.Action exitMethod
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_exitMethod);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_exitMethod)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		static __c__DisplayClass137_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<>c__DisplayClass137_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr);
			NativeFieldInfoPtr_primaryExitOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr, "primaryExitOnly");
			NativeFieldInfoPtr_condition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr, "condition");
			NativeFieldInfoPtr_exitMethod = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr, "exitMethod");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr, 100665911);
			NativeMethodInfoPtr__RegisterExitListener_b__0_Internal_Void_ExitAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr, 100665912);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass137_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass137_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89368, XrefRangeEnd = 89369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void _RegisterExitListener_b__0(ExitAction action)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__RegisterExitListener_b__0_Internal_Void_ExitAction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public __c__DisplayClass137_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__CurrentInputDevice_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__CurrentPlatformType_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_OnInputDeviceChanged;

	private static readonly System.IntPtr NativeFieldInfoPtr_exitListeners;

	private static readonly System.IntPtr NativeFieldInfoPtr_PlayerInput;

	private static readonly System.IntPtr NativeFieldInfoPtr_PrimaryExitAction;

	private static readonly System.IntPtr NativeFieldInfoPtr_SecondaryExitAction;

	private static readonly System.IntPtr NativeFieldInfoPtr__isTyping;

	private static readonly System.IntPtr NativeFieldInfoPtr_MotionAxis;

	private static readonly System.IntPtr NativeFieldInfoPtr_CameraAxis;

	private static readonly System.IntPtr NativeFieldInfoPtr_systemMouse;

	private static readonly System.IntPtr NativeFieldInfoPtr_MouseWheelAxis;

	private static readonly System.IntPtr NativeFieldInfoPtr_ControllerComboActive;

	private static readonly System.IntPtr NativeFieldInfoPtr_vehicleDriveAxis;

	private static readonly System.IntPtr NativeFieldInfoPtr__UINavigationDirection_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UICyclePanelDirection_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UITabNavigationPrimaryAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UITabNavigationSecondaryAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UITabNavigationTertiaryAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIScrollbarAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIMapNavigationDirection_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIMapZoomAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIModifyAmountIncrementTierOneAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIModifyAmountIncrementTierTwoAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__UIModifyAmountIncrementTierThreeAxis_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_buttonsDownThisFrame;

	private static readonly System.IntPtr NativeFieldInfoPtr_buttonsDown;

	private static readonly System.IntPtr NativeFieldInfoPtr_buttonsUpThisFrame;

	private static readonly System.IntPtr NativeFieldInfoPtr__timeOnLastRebind;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentInputDevice_Public_Static_get_InputDeviceType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CurrentInputDevice_Private_Static_set_Void_InputDeviceType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_CurrentPlatformType_Public_Static_get_EPlatformType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_CurrentPlatformType_Private_Static_set_Void_EPlatformType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsTyping_Public_Static_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_IsTyping_Public_Static_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MouseDelta_Public_Static_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MousePosition_Public_Static_get_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MouseScrollDelta_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_VehicleDriveAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_VehicleDriveAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UINavigationDirection_Public_Static_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UINavigationDirection_Private_Static_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UICyclePanelDirection_Public_Static_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UICyclePanelDirection_Private_Static_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UITabNavigationPrimaryAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UITabNavigationPrimaryAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UITabNavigationSecondaryAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UITabNavigationSecondaryAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UITabNavigationTertiaryAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UITabNavigationTertiaryAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIScrollbarAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIScrollbarAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIMapNavigationDirection_Public_Static_get_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIMapNavigationDirection_Private_Static_set_Void_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIMapZoomAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIMapZoomAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIModifyAmountIncrementTierOneAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIModifyAmountIncrementTierOneAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIModifyAmountIncrementTierTwoAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIModifyAmountIncrementTierTwoAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_UIModifyAmountIncrementTierThreeAxis_Public_Static_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_UIModifyAmountIncrementTierThreeAxis_Private_Static_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnApplicationFocus_Private_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetButton_Public_Static_Boolean_ButtonCode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetButtonDown_Public_Static_Boolean_ButtonCode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetButtonUp_Public_Static_Boolean_ButtonCode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurrentInputDeviceIsKeyboardMouse_Public_Static_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurrentInputDeviceIsGamepad_Public_Static_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurrentInputDevice_Public_Static_InputDeviceType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleExitInputs_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Exit_Private_Void_ExitType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExitAll_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPointerPosition_Public_Static_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnControlsChanged_Private_Void_PlayerInput_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetAction_Public_Boolean_String_byref_InputAction_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCurrentPlatformType_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnMotion_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnPrimaryClick_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnSecondaryClick_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnTertiaryClick_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnJump_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnCrouch_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnSprint_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnInteract_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnSubmit_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnVehicleToggleLights_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnVehicleHandbrake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnReload_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnCamera_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnScrollWheel_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnInventoryLeft_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnInventoryRight_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnControllerCombo_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnVehicleResetCamera_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnVehicleDrive_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnSkateboardDismount_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnSkateboardMount_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUINavigationDirection_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUICyclePanelDirection_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUITabNavigationPrimary_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUITabNavigationSecondary_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUITabNavigationTertiary_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIScrollbar_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIMapNavigationDirection_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIMapZoom_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIModifyAmountIncrementTierOne_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIModifyAmountIncrementTierTwo_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnUIModifyAmountIncrementTierThree_Private_Void_InputValue_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_ExitDelegate_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DeregisterExitListener_Public_Static_Void_ExitDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_Action_Func_1_Boolean_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAction_Public_InputAction_ButtonCode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static InputDeviceType _CurrentInputDevice_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out InputDeviceType result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__CurrentInputDevice_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__CurrentInputDevice_k__BackingField, (void*)(&inputDeviceType));
		}
	}

	public unsafe static EPlatformType _CurrentPlatformType_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out EPlatformType result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__CurrentPlatformType_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__CurrentPlatformType_k__BackingField, (void*)(&ePlatformType));
		}
	}

	public unsafe static Il2CppSystem.Action<InputDeviceType> OnInputDeviceChanged
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_OnInputDeviceChanged, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action<InputDeviceType>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_OnInputDeviceChanged, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	public unsafe static List<ExitListener> exitListeners
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_exitListeners, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ExitListener>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_exitListeners, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe PlayerInput PlayerInput
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayerInput);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PlayerInput>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PlayerInput)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerInput));
		}
	}

	public unsafe InputActionReference PrimaryExitAction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryExitAction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InputActionReference>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PrimaryExitAction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputActionReference));
		}
	}

	public unsafe InputActionReference SecondaryExitAction
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SecondaryExitAction);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InputActionReference>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SecondaryExitAction)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inputActionReference));
		}
	}

	public unsafe static bool _isTyping
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__isTyping, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__isTyping, (void*)(&flag));
		}
	}

	public unsafe static Vector2 MotionAxis
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MotionAxis, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MotionAxis, (void*)(&vector));
		}
	}

	public unsafe static Vector2 CameraAxis
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_CameraAxis, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_CameraAxis, (void*)(&vector));
		}
	}

	public unsafe static Mouse systemMouse
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_systemMouse, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mouse>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_systemMouse, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mouse));
		}
	}

	public unsafe static Vector2 MouseWheelAxis
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MouseWheelAxis, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MouseWheelAxis, (void*)(&vector));
		}
	}

	public unsafe static bool ControllerComboActive
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_ControllerComboActive, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_ControllerComboActive, (void*)(&flag));
		}
	}

	public unsafe float vehicleDriveAxis
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleDriveAxis);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_vehicleDriveAxis)) = num;
		}
	}

	public unsafe static Vector2 _UINavigationDirection_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UINavigationDirection_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UINavigationDirection_k__BackingField, (void*)(&vector));
		}
	}

	public unsafe static Vector2 _UICyclePanelDirection_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UICyclePanelDirection_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UICyclePanelDirection_k__BackingField, (void*)(&vector));
		}
	}

	public unsafe static float _UITabNavigationPrimaryAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UITabNavigationPrimaryAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UITabNavigationPrimaryAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UITabNavigationSecondaryAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UITabNavigationSecondaryAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UITabNavigationSecondaryAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UITabNavigationTertiaryAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UITabNavigationTertiaryAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UITabNavigationTertiaryAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UIScrollbarAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIScrollbarAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIScrollbarAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static Vector2 _UIMapNavigationDirection_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out Vector2 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIMapNavigationDirection_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIMapNavigationDirection_k__BackingField, (void*)(&vector));
		}
	}

	public unsafe static float _UIMapZoomAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIMapZoomAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIMapZoomAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UIModifyAmountIncrementTierOneAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierOneAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierOneAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UIModifyAmountIncrementTierTwoAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierTwoAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierTwoAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe static float _UIModifyAmountIncrementTierThreeAxis_k__BackingField
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierThreeAxis_k__BackingField, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__UIModifyAmountIncrementTierThreeAxis_k__BackingField, (void*)(&num));
		}
	}

	public unsafe List<ButtonCode> buttonsDownThisFrame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsDownThisFrame);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ButtonCode>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsDownThisFrame)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ButtonCode> buttonsDown
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsDown);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ButtonCode>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsDown)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ButtonCode> buttonsUpThisFrame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsUpThisFrame);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ButtonCode>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_buttonsUpThisFrame)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float _timeOnLastRebind
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOnLastRebind);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__timeOnLastRebind)) = num;
		}
	}

	public unsafe static InputDeviceType CurrentInputDevice
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89369, XrefRangeEnd = 89373, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentInputDevice_Public_Static_get_InputDeviceType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(InputDeviceType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89373, XrefRangeEnd = 89377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentInputDevice_Private_Static_set_Void_InputDeviceType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static EPlatformType CurrentPlatformType
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89377, XrefRangeEnd = 89381, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentPlatformType_Public_Static_get_EPlatformType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(EPlatformType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89381, XrefRangeEnd = 89385, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_CurrentPlatformType_Private_Static_set_Void_EPlatformType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static bool IsTyping
	{
		[CallerCount(26)]
		[CachedScanResults(RefRangeStart = 89390, RefRangeEnd = 89416, XrefRangeStart = 89385, XrefRangeEnd = 89390, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsTyping_Public_Static_get_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89416, XrefRangeEnd = 89420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_IsTyping_Public_Static_set_Void_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static Vector2 MouseDelta
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89420, XrefRangeEnd = 89424, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MouseDelta_Public_Static_get_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static Vector3 MousePosition
	{
		[CallerCount(24)]
		[CachedScanResults(RefRangeStart = 89445, RefRangeEnd = 89469, XrefRangeStart = 89424, XrefRangeEnd = 89445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MousePosition_Public_Static_get_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static float MouseScrollDelta
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 89473, RefRangeEnd = 89480, XrefRangeStart = 89469, XrefRangeEnd = 89473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MouseScrollDelta_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe static float VehicleDriveAxis
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 89492, RefRangeEnd = 89495, XrefRangeStart = 89480, XrefRangeEnd = 89492, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_VehicleDriveAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89495, XrefRangeEnd = 89499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_VehicleDriveAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static Vector2 UINavigationDirection
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89499, XrefRangeEnd = 89503, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UINavigationDirection_Public_Static_get_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89503, XrefRangeEnd = 89507, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UINavigationDirection_Private_Static_set_Void_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static Vector2 UICyclePanelDirection
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89507, XrefRangeEnd = 89511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UICyclePanelDirection_Public_Static_get_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89511, XrefRangeEnd = 89515, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UICyclePanelDirection_Private_Static_set_Void_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UITabNavigationPrimaryAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89515, XrefRangeEnd = 89519, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UITabNavigationPrimaryAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89519, XrefRangeEnd = 89523, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UITabNavigationPrimaryAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UITabNavigationSecondaryAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89523, XrefRangeEnd = 89527, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UITabNavigationSecondaryAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89527, XrefRangeEnd = 89531, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UITabNavigationSecondaryAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UITabNavigationTertiaryAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89531, XrefRangeEnd = 89535, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UITabNavigationTertiaryAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89535, XrefRangeEnd = 89539, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UITabNavigationTertiaryAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UIScrollbarAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89539, XrefRangeEnd = 89543, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIScrollbarAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89543, XrefRangeEnd = 89547, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIScrollbarAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static Vector2 UIMapNavigationDirection
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89547, XrefRangeEnd = 89551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIMapNavigationDirection_Public_Static_get_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89551, XrefRangeEnd = 89555, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIMapNavigationDirection_Private_Static_set_Void_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UIMapZoomAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89555, XrefRangeEnd = 89559, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIMapZoomAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89559, XrefRangeEnd = 89563, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIMapZoomAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UIModifyAmountIncrementTierOneAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89563, XrefRangeEnd = 89567, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIModifyAmountIncrementTierOneAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89567, XrefRangeEnd = 89571, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIModifyAmountIncrementTierOneAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UIModifyAmountIncrementTierTwoAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89571, XrefRangeEnd = 89575, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIModifyAmountIncrementTierTwoAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89575, XrefRangeEnd = 89579, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIModifyAmountIncrementTierTwoAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe static float UIModifyAmountIncrementTierThreeAxis
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89579, XrefRangeEnd = 89583, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UIModifyAmountIncrementTierThreeAxis_Public_Static_get_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89583, XrefRangeEnd = 89587, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_UIModifyAmountIncrementTierThreeAxis_Private_Static_set_Void_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static GameInput()
	{
		Il2CppClassPointerStore<GameInput>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "GameInput");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GameInput>.NativeClassPtr);
		NativeFieldInfoPtr__CurrentInputDevice_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<CurrentInputDevice>k__BackingField");
		NativeFieldInfoPtr__CurrentPlatformType_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<CurrentPlatformType>k__BackingField");
		NativeFieldInfoPtr_OnInputDeviceChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "OnInputDeviceChanged");
		NativeFieldInfoPtr_exitListeners = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "exitListeners");
		NativeFieldInfoPtr_PlayerInput = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "PlayerInput");
		NativeFieldInfoPtr_PrimaryExitAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "PrimaryExitAction");
		NativeFieldInfoPtr_SecondaryExitAction = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "SecondaryExitAction");
		NativeFieldInfoPtr__isTyping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "_isTyping");
		NativeFieldInfoPtr_MotionAxis = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "MotionAxis");
		NativeFieldInfoPtr_CameraAxis = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "CameraAxis");
		NativeFieldInfoPtr_systemMouse = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "systemMouse");
		NativeFieldInfoPtr_MouseWheelAxis = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "MouseWheelAxis");
		NativeFieldInfoPtr_ControllerComboActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "ControllerComboActive");
		NativeFieldInfoPtr_vehicleDriveAxis = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "vehicleDriveAxis");
		NativeFieldInfoPtr__UINavigationDirection_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UINavigationDirection>k__BackingField");
		NativeFieldInfoPtr__UICyclePanelDirection_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UICyclePanelDirection>k__BackingField");
		NativeFieldInfoPtr__UITabNavigationPrimaryAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UITabNavigationPrimaryAxis>k__BackingField");
		NativeFieldInfoPtr__UITabNavigationSecondaryAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UITabNavigationSecondaryAxis>k__BackingField");
		NativeFieldInfoPtr__UITabNavigationTertiaryAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UITabNavigationTertiaryAxis>k__BackingField");
		NativeFieldInfoPtr__UIScrollbarAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIScrollbarAxis>k__BackingField");
		NativeFieldInfoPtr__UIMapNavigationDirection_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIMapNavigationDirection>k__BackingField");
		NativeFieldInfoPtr__UIMapZoomAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIMapZoomAxis>k__BackingField");
		NativeFieldInfoPtr__UIModifyAmountIncrementTierOneAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIModifyAmountIncrementTierOneAxis>k__BackingField");
		NativeFieldInfoPtr__UIModifyAmountIncrementTierTwoAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIModifyAmountIncrementTierTwoAxis>k__BackingField");
		NativeFieldInfoPtr__UIModifyAmountIncrementTierThreeAxis_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "<UIModifyAmountIncrementTierThreeAxis>k__BackingField");
		NativeFieldInfoPtr_buttonsDownThisFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "buttonsDownThisFrame");
		NativeFieldInfoPtr_buttonsDown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "buttonsDown");
		NativeFieldInfoPtr_buttonsUpThisFrame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "buttonsUpThisFrame");
		NativeFieldInfoPtr__timeOnLastRebind = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GameInput>.NativeClassPtr, "_timeOnLastRebind");
		NativeMethodInfoPtr_get_CurrentInputDevice_Public_Static_get_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665813);
		NativeMethodInfoPtr_set_CurrentInputDevice_Private_Static_set_Void_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665814);
		NativeMethodInfoPtr_get_CurrentPlatformType_Public_Static_get_EPlatformType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665815);
		NativeMethodInfoPtr_set_CurrentPlatformType_Private_Static_set_Void_EPlatformType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665816);
		NativeMethodInfoPtr_get_IsTyping_Public_Static_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665817);
		NativeMethodInfoPtr_set_IsTyping_Public_Static_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665818);
		NativeMethodInfoPtr_get_MouseDelta_Public_Static_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665819);
		NativeMethodInfoPtr_get_MousePosition_Public_Static_get_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665820);
		NativeMethodInfoPtr_get_MouseScrollDelta_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665821);
		NativeMethodInfoPtr_get_VehicleDriveAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665822);
		NativeMethodInfoPtr_set_VehicleDriveAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665823);
		NativeMethodInfoPtr_get_UINavigationDirection_Public_Static_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665824);
		NativeMethodInfoPtr_set_UINavigationDirection_Private_Static_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665825);
		NativeMethodInfoPtr_get_UICyclePanelDirection_Public_Static_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665826);
		NativeMethodInfoPtr_set_UICyclePanelDirection_Private_Static_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665827);
		NativeMethodInfoPtr_get_UITabNavigationPrimaryAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665828);
		NativeMethodInfoPtr_set_UITabNavigationPrimaryAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665829);
		NativeMethodInfoPtr_get_UITabNavigationSecondaryAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665830);
		NativeMethodInfoPtr_set_UITabNavigationSecondaryAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665831);
		NativeMethodInfoPtr_get_UITabNavigationTertiaryAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665832);
		NativeMethodInfoPtr_set_UITabNavigationTertiaryAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665833);
		NativeMethodInfoPtr_get_UIScrollbarAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665834);
		NativeMethodInfoPtr_set_UIScrollbarAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665835);
		NativeMethodInfoPtr_get_UIMapNavigationDirection_Public_Static_get_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665836);
		NativeMethodInfoPtr_set_UIMapNavigationDirection_Private_Static_set_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665837);
		NativeMethodInfoPtr_get_UIMapZoomAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665838);
		NativeMethodInfoPtr_set_UIMapZoomAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665839);
		NativeMethodInfoPtr_get_UIModifyAmountIncrementTierOneAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665840);
		NativeMethodInfoPtr_set_UIModifyAmountIncrementTierOneAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665841);
		NativeMethodInfoPtr_get_UIModifyAmountIncrementTierTwoAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665842);
		NativeMethodInfoPtr_set_UIModifyAmountIncrementTierTwoAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665843);
		NativeMethodInfoPtr_get_UIModifyAmountIncrementTierThreeAxis_Public_Static_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665844);
		NativeMethodInfoPtr_set_UIModifyAmountIncrementTierThreeAxis_Private_Static_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665845);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665846);
		NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665847);
		NativeMethodInfoPtr_Start_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665848);
		NativeMethodInfoPtr_OnApplicationFocus_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665849);
		NativeMethodInfoPtr_GetButton_Public_Static_Boolean_ButtonCode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665850);
		NativeMethodInfoPtr_GetButtonDown_Public_Static_Boolean_ButtonCode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665851);
		NativeMethodInfoPtr_GetButtonUp_Public_Static_Boolean_ButtonCode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665852);
		NativeMethodInfoPtr_GetCurrentInputDeviceIsKeyboardMouse_Public_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665853);
		NativeMethodInfoPtr_GetCurrentInputDeviceIsGamepad_Public_Static_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665854);
		NativeMethodInfoPtr_GetCurrentInputDevice_Public_Static_InputDeviceType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665855);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665856);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665857);
		NativeMethodInfoPtr_HandleExitInputs_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665858);
		NativeMethodInfoPtr_Exit_Private_Void_ExitType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665859);
		NativeMethodInfoPtr_ExitAll_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665860);
		NativeMethodInfoPtr_GetPointerPosition_Public_Static_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665861);
		NativeMethodInfoPtr_OnControlsChanged_Private_Void_PlayerInput_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665862);
		NativeMethodInfoPtr_TryGetAction_Public_Boolean_String_byref_InputAction_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665863);
		NativeMethodInfoPtr_SetCurrentPlatformType_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665864);
		NativeMethodInfoPtr_OnMotion_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665865);
		NativeMethodInfoPtr_OnPrimaryClick_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665866);
		NativeMethodInfoPtr_OnSecondaryClick_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665867);
		NativeMethodInfoPtr_OnTertiaryClick_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665868);
		NativeMethodInfoPtr_OnJump_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665869);
		NativeMethodInfoPtr_OnCrouch_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665870);
		NativeMethodInfoPtr_OnSprint_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665871);
		NativeMethodInfoPtr_OnInteract_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665872);
		NativeMethodInfoPtr_OnSubmit_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665873);
		NativeMethodInfoPtr_OnVehicleToggleLights_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665874);
		NativeMethodInfoPtr_OnVehicleHandbrake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665875);
		NativeMethodInfoPtr_OnReload_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665876);
		NativeMethodInfoPtr_OnCamera_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665877);
		NativeMethodInfoPtr_OnScrollWheel_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665878);
		NativeMethodInfoPtr_OnInventoryLeft_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665879);
		NativeMethodInfoPtr_OnInventoryRight_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665880);
		NativeMethodInfoPtr_OnControllerCombo_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665881);
		NativeMethodInfoPtr_OnVehicleResetCamera_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665882);
		NativeMethodInfoPtr_OnVehicleDrive_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665883);
		NativeMethodInfoPtr_OnSkateboardDismount_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665884);
		NativeMethodInfoPtr_OnSkateboardMount_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665885);
		NativeMethodInfoPtr_OnUINavigationDirection_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665886);
		NativeMethodInfoPtr_OnUICyclePanelDirection_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665887);
		NativeMethodInfoPtr_OnUITabNavigationPrimary_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665888);
		NativeMethodInfoPtr_OnUITabNavigationSecondary_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665889);
		NativeMethodInfoPtr_OnUITabNavigationTertiary_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665890);
		NativeMethodInfoPtr_OnUIScrollbar_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665891);
		NativeMethodInfoPtr_OnUIMapNavigationDirection_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665892);
		NativeMethodInfoPtr_OnUIMapZoom_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665893);
		NativeMethodInfoPtr_OnUIModifyAmountIncrementTierOne_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665894);
		NativeMethodInfoPtr_OnUIModifyAmountIncrementTierTwo_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665895);
		NativeMethodInfoPtr_OnUIModifyAmountIncrementTierThree_Private_Void_InputValue_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665896);
		NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_ExitDelegate_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665897);
		NativeMethodInfoPtr_DeregisterExitListener_Public_Static_Void_ExitDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665898);
		NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_Action_Func_1_Boolean_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665899);
		NativeMethodInfoPtr_GetAction_Public_InputAction_ButtonCode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665900);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GameInput>.NativeClassPtr, 100665901);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89587, XrefRangeEnd = 89598, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89598, XrefRangeEnd = 89619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Awake_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89619, XrefRangeEnd = 89671, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89671, XrefRangeEnd = 89690, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnApplicationFocus(bool focus)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&focus);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnApplicationFocus_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(57)]
	[CachedScanResults(RefRangeStart = 89698, RefRangeEnd = 89755, XrefRangeStart = 89690, XrefRangeEnd = 89698, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetButton(ButtonCode buttonCode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&buttonCode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetButton_Public_Static_Boolean_ButtonCode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(74)]
	[CachedScanResults(RefRangeStart = 89763, RefRangeEnd = 89837, XrefRangeStart = 89755, XrefRangeEnd = 89763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetButtonDown(ButtonCode buttonCode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&buttonCode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetButtonDown_Public_Static_Boolean_ButtonCode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 89845, RefRangeEnd = 89846, XrefRangeStart = 89837, XrefRangeEnd = 89845, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetButtonUp(ButtonCode buttonCode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&buttonCode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetButtonUp_Public_Static_Boolean_ButtonCode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(21)]
	[CachedScanResults(RefRangeStart = 89853, RefRangeEnd = 89874, XrefRangeStart = 89846, XrefRangeEnd = 89853, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetCurrentInputDeviceIsKeyboardMouse()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurrentInputDeviceIsKeyboardMouse_Public_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(34)]
	[CachedScanResults(RefRangeStart = 89881, RefRangeEnd = 89915, XrefRangeStart = 89874, XrefRangeEnd = 89881, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool GetCurrentInputDeviceIsGamepad()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurrentInputDeviceIsGamepad_Public_Static_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89915, XrefRangeEnd = 89923, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static InputDeviceType GetCurrentInputDevice()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurrentInputDevice_Public_Static_InputDeviceType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(InputDeviceType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89923, XrefRangeEnd = 89926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 89926, XrefRangeEnd = 89927, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 89955, RefRangeEnd = 89956, XrefRangeStart = 89927, XrefRangeEnd = 89955, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleExitInputs()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleExitInputs_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 89971, RefRangeEnd = 89972, XrefRangeStart = 89956, XrefRangeEnd = 89971, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Exit(ExitType type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&type);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Exit_Private_Void_ExitType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 89976, RefRangeEnd = 89985, XrefRangeStart = 89972, XrefRangeEnd = 89976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ExitAll()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExitAll_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 89994, RefRangeEnd = 90007, XrefRangeStart = 89985, XrefRangeEnd = 89994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetPointerPosition()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPointerPosition_Public_Static_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90007, XrefRangeEnd = 90054, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnControlsChanged(PlayerInput input)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)input);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnControlsChanged_Private_Void_PlayerInput_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 90057, RefRangeEnd = 90058, XrefRangeStart = 90054, XrefRangeEnd = 90057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool TryGetAction(string actionName, out InputAction action)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(actionName);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetAction_Public_Boolean_String_byref_InputAction_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		action = ((num3 == 0) ? null : new InputAction(num3));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90071, RefRangeEnd = 90073, XrefRangeStart = 90058, XrefRangeEnd = 90071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetCurrentPlatformType()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCurrentPlatformType_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90073, XrefRangeEnd = 90145, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnMotion(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnMotion_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90145, XrefRangeEnd = 90155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnPrimaryClick()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnPrimaryClick_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90155, XrefRangeEnd = 90165, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSecondaryClick()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSecondaryClick_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90165, XrefRangeEnd = 90175, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnTertiaryClick()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnTertiaryClick_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90175, XrefRangeEnd = 90189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnJump()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnJump_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90189, XrefRangeEnd = 90203, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnCrouch()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnCrouch_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90203, XrefRangeEnd = 90213, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSprint()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSprint_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90213, XrefRangeEnd = 90227, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInteract()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInteract_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90227, XrefRangeEnd = 90237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSubmit()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSubmit_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90237, XrefRangeEnd = 90247, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnVehicleToggleLights()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnVehicleToggleLights_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90247, XrefRangeEnd = 90257, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnVehicleHandbrake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnVehicleHandbrake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90257, XrefRangeEnd = 90271, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnReload()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnReload_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90271, XrefRangeEnd = 90285, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnCamera(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnCamera_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90285, XrefRangeEnd = 90292, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnScrollWheel(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnScrollWheel_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90292, XrefRangeEnd = 90302, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInventoryLeft()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInventoryLeft_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90302, XrefRangeEnd = 90312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInventoryRight()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInventoryRight_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90312, XrefRangeEnd = 90319, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnControllerCombo(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnControllerCombo_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90319, XrefRangeEnd = 90329, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnVehicleResetCamera()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnVehicleResetCamera_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90329, XrefRangeEnd = 90339, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnVehicleDrive(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnVehicleDrive_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90339, XrefRangeEnd = 90349, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSkateboardDismount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSkateboardDismount_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90349, XrefRangeEnd = 90359, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnSkateboardMount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnSkateboardMount_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90359, XrefRangeEnd = 90369, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUINavigationDirection(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUINavigationDirection_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90369, XrefRangeEnd = 90379, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUICyclePanelDirection(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUICyclePanelDirection_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90379, XrefRangeEnd = 90389, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUITabNavigationPrimary(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUITabNavigationPrimary_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90389, XrefRangeEnd = 90399, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUITabNavigationSecondary(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUITabNavigationSecondary_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90399, XrefRangeEnd = 90409, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUITabNavigationTertiary(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUITabNavigationTertiary_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90409, XrefRangeEnd = 90419, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIScrollbar(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIScrollbar_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90419, XrefRangeEnd = 90429, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIMapNavigationDirection(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIMapNavigationDirection_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90429, XrefRangeEnd = 90439, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIMapZoom(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIMapZoom_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90439, XrefRangeEnd = 90449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIModifyAmountIncrementTierOne(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIModifyAmountIncrementTierOne_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90449, XrefRangeEnd = 90459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIModifyAmountIncrementTierTwo(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIModifyAmountIncrementTierTwo_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90459, XrefRangeEnd = 90469, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnUIModifyAmountIncrementTierThree(InputValue value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnUIModifyAmountIncrementTierThree_Private_Void_InputValue_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(40)]
	[CachedScanResults(RefRangeStart = 90499, RefRangeEnd = 90539, XrefRangeStart = 90469, XrefRangeEnd = 90499, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RegisterExitListener(ExitDelegate listener, int priority = 0)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &priority;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_ExitDelegate_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 90557, RefRangeEnd = 90562, XrefRangeStart = 90539, XrefRangeEnd = 90557, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void DeregisterExitListener(ExitDelegate listener)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)listener);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DeregisterExitListener_Public_Static_Void_ExitDelegate_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 90579, RefRangeEnd = 90581, XrefRangeStart = 90562, XrefRangeEnd = 90579, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RegisterExitListener(Il2CppSystem.Action exitMethod, Il2CppSystem.Func<bool> condition, int priority = 0, bool primaryExitOnly = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exitMethod);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)condition);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &priority;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &primaryExitOnly;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterExitListener_Public_Static_Void_Action_Func_1_Boolean_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90581, XrefRangeEnd = 90585, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe InputAction GetAction(ButtonCode code)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&code);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAction_Public_InputAction_ButtonCode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<InputAction>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 90585, XrefRangeEnd = 90605, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameInput()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GameInput>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public GameInput(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
