using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Il2CppScheduleOne;

public static class CustomUIUtils : Il2CppSystem.Object
{
	public class CandidateInfo<T> : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Candidate;

		private static readonly System.IntPtr NativeFieldInfoPtr_DirectionMatch;

		private static readonly System.IntPtr NativeFieldInfoPtr_Distance;

		private static readonly System.IntPtr NativeFieldInfoPtr_Score;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_T_Single_Single_Single_0;

		public unsafe T Candidate
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Candidate);
				return IL2CPP.PointerToValueGeneric<T>((System.IntPtr)num, true, false);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				nint num2 = (nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Candidate);
				System.Type typeFromHandle = typeof(T);
				if (!typeFromHandle.IsValueType)
				{
					if (!string.Equals(typeFromHandle.FullName, "System.String"))
					{
						System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((((object)val) is Il2CppObjectBase) ? ((object)val) : null));
						if (intPtr != (System.IntPtr)0 && IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							System.IntPtr intPtr2 = intPtr;
							// IL cpblk instruction
							Unsafe.CopyBlock(num2, IL2CPP.il2cpp_object_unbox(intPtr), IL2CPP.il2cpp_class_value_size(IL2CPP.il2cpp_object_get_class(intPtr2), ref *(uint*)null));
						}
						else
						{
							IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, intPtr);
						}
					}
					else
					{
						IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)num2, IL2CPP.ManagedStringToIl2Cpp(val as string));
					}
				}
				else
				{
					Unsafe.Write((void*)num2, val);
				}
			}
		}

		public unsafe float DirectionMatch
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionMatch);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_DirectionMatch)) = num;
			}
		}

		public unsafe float Distance
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distance);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Distance)) = num;
			}
		}

		public unsafe float Score
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Score);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Score)) = num;
			}
		}

		static CandidateInfo()
		{
			Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr = IL2CPP.il2cpp_class_from_type(Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, "CandidateInfo`1"))).MakeGenericType(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) })).TypeHandle.value);
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr);
			NativeFieldInfoPtr_Candidate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr, "Candidate");
			NativeFieldInfoPtr_DirectionMatch = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr, "DirectionMatch");
			NativeFieldInfoPtr_Distance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr, "Distance");
			NativeFieldInfoPtr_Score = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr, "Score");
			NativeMethodInfoPtr__ctor_Public_Void_T_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr, 100664953);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 79275, RefRangeEnd = 79276, XrefRangeStart = 79273, XrefRangeEnd = 79275, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CandidateInfo(T candidate, float directionMatch, float distance, float score)
		{
			//IL_006a->IL006d: Incompatible stack types: I vs Ref
			//IL_0049->IL006d: Incompatible stack types: I vs Ref
			//IL_0056->IL006d: Incompatible stack types: I vs Ref
			//IL_005d->IL006d: Incompatible stack types: I vs Ref
			this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CandidateInfo<T>>.NativeClassPtr));
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			ref T reference;
			if (!typeof(T).IsValueType)
			{
				object obj = candidate;
				if (obj is string)
				{
					reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
				}
				else
				{
					System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
					reference = ref *(_003F*)intPtr;
					if (intPtr != (System.IntPtr)0)
					{
						reference = ref *(_003F*)intPtr;
						if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
						{
							reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
						}
					}
				}
			}
			else
			{
				reference = ref candidate;
			}
			*ptr = (nint)Unsafe.AsPointer(ref reference);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &directionMatch;
			*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
			*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &score;
			Unsafe.SkipInit(out System.IntPtr intPtr3);
			System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_T_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr3);
			Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		}

		public CandidateInfo(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("ScheduleOne.CustomUIUtils+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__4_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetSelectables_b__4_0_Internal_Boolean_UISelectable_0;

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

		public unsafe static Il2CppSystem.Func<UISelectable, bool> __9__4_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__4_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<UISelectable, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__4_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__4_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__4_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664955);
			NativeMethodInfoPtr__GetSelectables_b__4_0_Internal_Boolean_UISelectable_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100664956);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79276, XrefRangeEnd = 79278, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetSelectables_b__4_0(UISelectable s)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetSelectables_b__4_0_Internal_Boolean_UISelectable_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private sealed class MethodInfoStoreGeneric_FindBestElementWeighted_Public_Static_T_Vector2_Canvas_T_List_1_T_NavigationSettings_byref_List_1_CandidateInfo_1_T_0<T>
	{
		internal static System.IntPtr Pointer = IL2CPP.il2cpp_method_get_from_reflection(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)new MethodInfo(IL2CPP.il2cpp_method_get_object(NativeMethodInfoPtr_FindBestElementWeighted_Public_Static_T_Vector2_Canvas_T_List_1_T_NavigationSettings_byref_List_1_CandidateInfo_1_T_0, Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr)).MakeGenericMethod(new Il2CppReferenceArray<Il2CppSystem.Type>(new Il2CppSystem.Type[1] { Il2CppSystem.Type.internal_from_handle(IL2CPP.il2cpp_class_get_type(Il2CppClassPointerStore<T>.NativeClassPtr)) }))));
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSystemMouse_Public_Static_Mouse_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetVirtualMouse_Public_Static_Mouse_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_RectTransform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_Vector3_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetSelectables_Public_Static_List_1_UISelectable_UIScreen_UIPanel_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBestElementWeighted_Public_Static_T_Vector2_Canvas_T_List_1_T_NavigationSettings_byref_List_1_CandidateInfo_1_T_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RaycastRectTransform_Public_Static_Boolean_RectTransform_Canvas_Vector2_Vector2_byref_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWeightedScore_Private_Static_Single_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirectionMatch_Public_Static_Single_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDirectionMatchForCorners_Public_Static_Single_Vector2_Vector2_RectTransform_Canvas_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetDistanceOfClosestPoint_Public_Static_Single_Vector2_RectTransform_Canvas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SnapDirection_Public_Static_Vector2_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsHorizontal_Public_Static_Boolean_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsVertical_Public_Static_Boolean_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToScreenDirection_Public_Static_ScreenDirection_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampToDirection_Public_Static_Vector2_Vector2_Vector2_0;

	static CustomUIUtils()
	{
		Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne", "CustomUIUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr);
		NativeMethodInfoPtr_GetSystemMouse_Public_Static_Mouse_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664937);
		NativeMethodInfoPtr_GetVirtualMouse_Public_Static_Mouse_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664938);
		NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_RectTransform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664939);
		NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_Vector3_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664940);
		NativeMethodInfoPtr_GetSelectables_Public_Static_List_1_UISelectable_UIScreen_UIPanel_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664941);
		NativeMethodInfoPtr_FindBestElementWeighted_Public_Static_T_Vector2_Canvas_T_List_1_T_NavigationSettings_byref_List_1_CandidateInfo_1_T_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664942);
		NativeMethodInfoPtr_RaycastRectTransform_Public_Static_Boolean_RectTransform_Canvas_Vector2_Vector2_byref_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664943);
		NativeMethodInfoPtr_GetWeightedScore_Private_Static_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664944);
		NativeMethodInfoPtr_GetDirectionMatch_Public_Static_Single_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664945);
		NativeMethodInfoPtr_GetDirectionMatchForCorners_Public_Static_Single_Vector2_Vector2_RectTransform_Canvas_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664946);
		NativeMethodInfoPtr_GetDistanceOfClosestPoint_Public_Static_Single_Vector2_RectTransform_Canvas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664947);
		NativeMethodInfoPtr_SnapDirection_Public_Static_Vector2_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664948);
		NativeMethodInfoPtr_IsHorizontal_Public_Static_Boolean_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664949);
		NativeMethodInfoPtr_IsVertical_Public_Static_Boolean_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664950);
		NativeMethodInfoPtr_ToScreenDirection_Public_Static_ScreenDirection_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664951);
		NativeMethodInfoPtr_ClampToDirection_Public_Static_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CustomUIUtils>.NativeClassPtr, 100664952);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 79289, RefRangeEnd = 79291, XrefRangeStart = 79278, XrefRangeEnd = 79289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mouse GetSystemMouse()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSystemMouse_Public_Static_Mouse_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mouse>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79302, RefRangeEnd = 79303, XrefRangeStart = 79291, XrefRangeEnd = 79302, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Mouse GetVirtualMouse()
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetVirtualMouse_Public_Static_Mouse_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Mouse>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79325, RefRangeEnd = 79326, XrefRangeStart = 79303, XrefRangeEnd = 79325, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 GetScreenPosition(Canvas canvas, RectTransform selectable)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)selectable);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_RectTransform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 79344, RefRangeEnd = 79353, XrefRangeStart = 79326, XrefRangeEnd = 79344, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 GetScreenPosition(Canvas canvas, Vector3 worldPosition)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &worldPosition;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetScreenPosition_Public_Static_Vector2_Canvas_Vector3_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79353, XrefRangeEnd = 79403, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static List<UISelectable> GetSelectables(UIScreen screen, UIPanel current = null, bool includeCurrentPanel = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)screen);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)current);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeCurrentPanel;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetSelectables_Public_Static_List_1_UISelectable_UIScreen_UIPanel_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<UISelectable>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79434, RefRangeEnd = 79435, XrefRangeStart = 79403, XrefRangeEnd = 79434, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static T FindBestElementWeighted<T>(Vector2 dir, Canvas canvas, T current, List<T> candidates, UIContentPanel.NavigationSettings settings, out List<CandidateInfo<T>> validCandidates)
	{
		//IL_007a->IL007d: Incompatible stack types: I vs Ref
		//IL_0059->IL007d: Incompatible stack types: I vs Ref
		//IL_0066->IL007d: Incompatible stack types: I vs Ref
		//IL_006d->IL007d: Incompatible stack types: I vs Ref
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&dir);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		ref T reference;
		if (!typeof(T).IsValueType)
		{
			object obj = current;
			if (obj is string)
			{
				reference = ref *(_003F*)IL2CPP.ManagedStringToIl2Cpp(obj as string);
			}
			else
			{
				System.IntPtr intPtr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)((obj is Il2CppObjectBase) ? obj : null));
				reference = ref *(_003F*)intPtr;
				if (intPtr != (System.IntPtr)0)
				{
					reference = ref *(_003F*)intPtr;
					if (IL2CPP.il2cpp_class_is_valuetype(IL2CPP.il2cpp_object_get_class(intPtr)))
					{
						reference = ref *(_003F*)IL2CPP.il2cpp_object_unbox(intPtr);
					}
				}
			}
		}
		else
		{
			reference = ref current;
		}
		*(void**)num = Unsafe.AsPointer(ref reference);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)candidates);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		byte* num2 = (byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num3 = 0;
		*(nint**)num2 = &num3;
		Unsafe.SkipInit(out System.IntPtr intPtr3);
		System.IntPtr intPtr2 = IL2CPP.il2cpp_runtime_invoke(MethodInfoStoreGeneric_FindBestElementWeighted_Public_Static_T_Vector2_Canvas_T_List_1_T_NavigationSettings_byref_List_1_CandidateInfo_1_T_0<T>.Pointer, (System.IntPtr)0, (void**)ptr, ref intPtr3);
		Il2CppException.RaiseExceptionIfNecessary(intPtr3);
		nint num4 = num3;
		validCandidates = ((num4 == 0) ? null : new List<CandidateInfo<T>>(num4));
		return IL2CPP.PointerToValueGeneric<T>(intPtr2, false, true);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 79444, RefRangeEnd = 79446, XrefRangeStart = 79435, XrefRangeEnd = 79444, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool RaycastRectTransform(RectTransform rect, Canvas canvas, Vector2 p, Vector2 d, out Vector2 hitPoint)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rect);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &d;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref hitPoint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RaycastRectTransform_Public_Static_Boolean_RectTransform_Canvas_Vector2_Vector2_byref_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79446, RefRangeEnd = 79447, XrefRangeStart = 79446, XrefRangeEnd = 79446, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetWeightedScore(float directionMatch, float distance, float directionWeight, float distanceWeight)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&directionMatch);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &directionWeight;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distanceWeight;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWeightedScore_Private_Static_Single_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79449, RefRangeEnd = 79450, XrefRangeStart = 79447, XrefRangeEnd = 79449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetDirectionMatch(Vector2 dir, Vector2 fromScreenPos, Vector2 toScreenPos)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&dir);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &fromScreenPos;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &toScreenPos;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirectionMatch_Public_Static_Single_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 79459, RefRangeEnd = 79460, XrefRangeStart = 79450, XrefRangeEnd = 79459, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetDirectionMatchForCorners(Vector2 dir, Vector2 screenPos, RectTransform rectTransform, Canvas canvas, float intersectionOffset = 50f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&dir);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &screenPos;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &intersectionOffset;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDirectionMatchForCorners_Public_Static_Single_Vector2_Vector2_RectTransform_Canvas_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79460, XrefRangeEnd = 79472, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float GetDistanceOfClosestPoint(Vector2 screenPos, RectTransform rectTransform, Canvas canvas)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&screenPos);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetDistanceOfClosestPoint_Public_Static_Single_Vector2_RectTransform_Canvas_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79472, XrefRangeEnd = 79473, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 SnapDirection(Vector2 dir, float threshold = 0.1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&dir);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &threshold;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SnapDirection_Public_Static_Vector2_Vector2_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79473, XrefRangeEnd = 79474, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsHorizontal(Vector2 val, float threshold = 0.2f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&val);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &threshold;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsHorizontal_Public_Static_Boolean_Vector2_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79474, XrefRangeEnd = 79475, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsVertical(Vector2 val, float threshold = 0.2f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&val);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &threshold;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsVertical_Public_Static_Boolean_Vector2_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 79476, RefRangeEnd = 79478, XrefRangeStart = 79475, XrefRangeEnd = 79476, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static ScreenDirection ToScreenDirection(Vector2 val)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&val);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToScreenDirection_Public_Static_ScreenDirection_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ScreenDirection*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 79478, XrefRangeEnd = 79481, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 ClampToDirection(Vector2 val, Vector2 dir)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&val);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dir;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampToDirection_Public_Static_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CustomUIUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
