using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public static class VectorUtils : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "Alignment")]
	public enum Alignment
	{
		Center,
		TopLeft,
		TopCenter,
		TopRight,
		LeftCenter,
		RightCenter,
		BottomLeft,
		BottomCenter,
		BottomRight,
		Custom,
		SVGOrigin
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "WindingDir")]
	public enum WindingDir
	{
		CW,
		CCW
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct TessellationOptions
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxCordDev;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxCordDevSq;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxTanAngleDev;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_MaxTanAngleDevCosine;

		private static readonly System.IntPtr NativeFieldInfoPtr_m_StepSize;

		private static readonly System.IntPtr NativeFieldInfoPtr__StepDistance_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_StepDistance_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_StepDistance_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxCordDeviation_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_MaxCordDeviation_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxCordDeviationSquared_Internal_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxTanAngleDeviation_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_MaxTanAngleDeviation_Public_set_Void_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_MaxTanAngleDeviationCosine_Internal_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SamplingStepSize_Public_get_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_set_SamplingStepSize_Public_set_Void_Single_0;

		[FieldOffset(0)]
		public float m_MaxCordDev;

		[FieldOffset(4)]
		public float m_MaxCordDevSq;

		[FieldOffset(8)]
		public float m_MaxTanAngleDev;

		[FieldOffset(12)]
		public float m_MaxTanAngleDevCosine;

		[FieldOffset(16)]
		public float m_StepSize;

		[FieldOffset(20)]
		public float _StepDistance_k__BackingField;

		public unsafe float StepDistance
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StepDistance_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 26895, RefRangeEnd = 26896, XrefRangeStart = 26895, XrefRangeEnd = 26895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_StepDistance_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float MaxCordDeviation
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxCordDeviation_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(0)]
			set
			{
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MaxCordDeviation_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float MaxCordDeviationSquared
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxCordDeviationSquared_Internal_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe float MaxTanAngleDeviation
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxTanAngleDeviation_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 26901, RefRangeEnd = 26902, XrefRangeStart = 26896, XrefRangeEnd = 26901, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_MaxTanAngleDeviation_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		public unsafe float MaxTanAngleDeviationCosine
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaxTanAngleDeviationCosine_Internal_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe float SamplingStepSize
		{
			[CallerCount(0)]
			get
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SamplingStepSize_Public_get_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26902, XrefRangeEnd = 26906, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			set
			{
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&value);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_SamplingStepSize_Public_set_Void_Single_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}
		}

		static TessellationOptions()
		{
			Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "TessellationOptions");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr);
			NativeFieldInfoPtr_m_MaxCordDev = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "m_MaxCordDev");
			NativeFieldInfoPtr_m_MaxCordDevSq = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "m_MaxCordDevSq");
			NativeFieldInfoPtr_m_MaxTanAngleDev = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "m_MaxTanAngleDev");
			NativeFieldInfoPtr_m_MaxTanAngleDevCosine = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "m_MaxTanAngleDevCosine");
			NativeFieldInfoPtr_m_StepSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "m_StepSize");
			NativeFieldInfoPtr__StepDistance_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, "<StepDistance>k__BackingField");
			NativeMethodInfoPtr_get_StepDistance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663616);
			NativeMethodInfoPtr_set_StepDistance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663617);
			NativeMethodInfoPtr_get_MaxCordDeviation_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663618);
			NativeMethodInfoPtr_set_MaxCordDeviation_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663619);
			NativeMethodInfoPtr_get_MaxCordDeviationSquared_Internal_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663620);
			NativeMethodInfoPtr_get_MaxTanAngleDeviation_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663621);
			NativeMethodInfoPtr_set_MaxTanAngleDeviation_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663622);
			NativeMethodInfoPtr_get_MaxTanAngleDeviationCosine_Internal_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663623);
			NativeMethodInfoPtr_get_SamplingStepSize_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663624);
			NativeMethodInfoPtr_set_SamplingStepSize_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, 100663625);
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<TessellationOptions>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public class JoiningInfo : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_JoinPos;

		private static readonly System.IntPtr NativeFieldInfoPtr_TanAtEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_TanAtStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_NormAtEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_NormAtStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_PosThicknessStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_NegThicknessStart;

		private static readonly System.IntPtr NativeFieldInfoPtr_PosThicknessEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_NegThicknessEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_PosThicknessClosingPoint;

		private static readonly System.IntPtr NativeFieldInfoPtr_NegThicknessClosingPoint;

		private static readonly System.IntPtr NativeFieldInfoPtr_RoundPosThickness;

		private static readonly System.IntPtr NativeFieldInfoPtr_SimpleJoin;

		private static readonly System.IntPtr NativeFieldInfoPtr_InnerCornerVertex;

		private static readonly System.IntPtr NativeFieldInfoPtr_InnerCornerDistToEnd;

		private static readonly System.IntPtr NativeFieldInfoPtr_InnerCornerDistFromStart;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Vector2 JoinPos
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JoinPos);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_JoinPos)) = vector;
			}
		}

		public unsafe Vector2 TanAtEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TanAtEnd);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TanAtEnd)) = vector;
			}
		}

		public unsafe Vector2 TanAtStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TanAtStart);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_TanAtStart)) = vector;
			}
		}

		public unsafe Vector2 NormAtEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormAtEnd);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormAtEnd)) = vector;
			}
		}

		public unsafe Vector2 NormAtStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormAtStart);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NormAtStart)) = vector;
			}
		}

		public unsafe Vector2 PosThicknessStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessStart);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessStart)) = vector;
			}
		}

		public unsafe Vector2 NegThicknessStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessStart);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessStart)) = vector;
			}
		}

		public unsafe Vector2 PosThicknessEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessEnd);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessEnd)) = vector;
			}
		}

		public unsafe Vector2 NegThicknessEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessEnd);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessEnd)) = vector;
			}
		}

		public unsafe Vector2 PosThicknessClosingPoint
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessClosingPoint);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_PosThicknessClosingPoint)) = vector;
			}
		}

		public unsafe Vector2 NegThicknessClosingPoint
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessClosingPoint);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NegThicknessClosingPoint)) = vector;
			}
		}

		public unsafe bool RoundPosThickness
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RoundPosThickness);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_RoundPosThickness)) = flag;
			}
		}

		public unsafe bool SimpleJoin
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SimpleJoin);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_SimpleJoin)) = flag;
			}
		}

		public unsafe Vector2 InnerCornerVertex
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerVertex);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerVertex)) = vector;
			}
		}

		public unsafe float InnerCornerDistToEnd
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerDistToEnd);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerDistToEnd)) = num;
			}
		}

		public unsafe float InnerCornerDistFromStart
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerDistFromStart);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_InnerCornerDistFromStart)) = num;
			}
		}

		static JoiningInfo()
		{
			Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "JoiningInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr);
			NativeFieldInfoPtr_JoinPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "JoinPos");
			NativeFieldInfoPtr_TanAtEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "TanAtEnd");
			NativeFieldInfoPtr_TanAtStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "TanAtStart");
			NativeFieldInfoPtr_NormAtEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "NormAtEnd");
			NativeFieldInfoPtr_NormAtStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "NormAtStart");
			NativeFieldInfoPtr_PosThicknessStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "PosThicknessStart");
			NativeFieldInfoPtr_NegThicknessStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "NegThicknessStart");
			NativeFieldInfoPtr_PosThicknessEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "PosThicknessEnd");
			NativeFieldInfoPtr_NegThicknessEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "NegThicknessEnd");
			NativeFieldInfoPtr_PosThicknessClosingPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "PosThicknessClosingPoint");
			NativeFieldInfoPtr_NegThicknessClosingPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "NegThicknessClosingPoint");
			NativeFieldInfoPtr_RoundPosThickness = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "RoundPosThickness");
			NativeFieldInfoPtr_SimpleJoin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "SimpleJoin");
			NativeFieldInfoPtr_InnerCornerVertex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "InnerCornerVertex");
			NativeFieldInfoPtr_InnerCornerDistToEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "InnerCornerDistToEnd");
			NativeFieldInfoPtr_InnerCornerDistFromStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, "InnerCornerDistFromStart");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr, 100663626);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe JoiningInfo()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<JoiningInfo>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public JoiningInfo(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class SceneNodeWorldTransform : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Node;

		private static readonly System.IntPtr NativeFieldInfoPtr_Parent;

		private static readonly System.IntPtr NativeFieldInfoPtr_WorldTransform;

		private static readonly System.IntPtr NativeFieldInfoPtr_WorldOpacity;

		public unsafe SceneNode Node
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Node);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Node)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNode Parent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parent);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe Matrix2D WorldTransform
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldTransform);
				return *(Matrix2D*)num;
			}
			set
			{
				*(Matrix2D*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldTransform)) = matrix2D;
			}
		}

		public unsafe float WorldOpacity
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldOpacity);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldOpacity)) = num;
			}
		}

		static SceneNodeWorldTransform()
		{
			Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "SceneNodeWorldTransform");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr);
			NativeFieldInfoPtr_Node = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, "Node");
			NativeFieldInfoPtr_Parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, "Parent");
			NativeFieldInfoPtr_WorldTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, "WorldTransform");
			NativeFieldInfoPtr_WorldOpacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, "WorldOpacity");
		}

		public SceneNodeWorldTransform(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public SceneNodeWorldTransform()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr))
		{
		}
	}

	[System.Serializable]
	[ObfuscatedName("ToolBuddy.ThirdParty.VectorGraphics.VectorUtils+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__10_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__81_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__81_1;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__RenderSprite_b__10_1_Internal_Color_Color32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__CubicRoots_b__81_0_Internal_Boolean_Double_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__CubicRoots_b__81_1_Internal_Single_Double_0;

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

		public unsafe static Il2CppSystem.Func<Color32, Color> __9__10_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__10_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<Color32, Color>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__10_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Predicate<double> __9__81_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__81_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Predicate<double>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__81_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)predicate));
			}
		}

		public unsafe static Il2CppSystem.Func<double, float> __9__81_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__81_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<double, float>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__81_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__10_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__10_1");
			NativeFieldInfoPtr___9__81_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__81_0");
			NativeFieldInfoPtr___9__81_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__81_1");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100663628);
			NativeMethodInfoPtr__RenderSprite_b__10_1_Internal_Color_Color32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100663629);
			NativeMethodInfoPtr__CubicRoots_b__81_0_Internal_Boolean_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100663630);
			NativeMethodInfoPtr__CubicRoots_b__81_1_Internal_Single_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100663631);
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
		public unsafe Color _RenderSprite_b__10_1(Color32 c)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&c);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__RenderSprite_b__10_1_Internal_Color_Color32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Color*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26906, XrefRangeEnd = 26913, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _CubicRoots_b__81_0(double x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__CubicRoots_b__81_0_Internal_Boolean_Double_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe float _CubicRoots_b__81_1(double x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__CubicRoots_b__81_1_Internal_Single_Double_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ToolBuddy.ThirdParty.VectorGraphics.VectorUtils+<>c__DisplayClass10_0")]
	public sealed class __c__DisplayClass10_0 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_pixelsToUnits;

		private static readonly System.IntPtr NativeFieldInfoPtr_pivot;

		private static readonly System.IntPtr NativeFieldInfoPtr_spriteWidth;

		private static readonly System.IntPtr NativeFieldInfoPtr_spriteHeight;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__RenderSprite_b__0_Internal_Vector2_Vector2_0;

		public unsafe float pixelsToUnits
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pixelsToUnits);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pixelsToUnits)) = num;
			}
		}

		public unsafe Vector2 pivot
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pivot);
				return *(Vector2*)num;
			}
			set
			{
				*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pivot)) = vector;
			}
		}

		public unsafe float spriteWidth
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteWidth);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteWidth)) = num;
			}
		}

		public unsafe float spriteHeight
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteHeight);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteHeight)) = num;
			}
		}

		static __c__DisplayClass10_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "<>c__DisplayClass10_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr);
			NativeFieldInfoPtr_pixelsToUnits = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "pixelsToUnits");
			NativeFieldInfoPtr_pivot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "pivot");
			NativeFieldInfoPtr_spriteWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "spriteWidth");
			NativeFieldInfoPtr_spriteHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, "spriteHeight");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, 100663632);
			NativeMethodInfoPtr__RenderSprite_b__0_Internal_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr, 100663633);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c__DisplayClass10_0()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass10_0>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe Vector2 _RenderSprite_b__0(Vector2 v)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&v);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__RenderSprite_b__0_Internal_Vector2_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c__DisplayClass10_0(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ToolBuddy.ThirdParty.VectorGraphics.VectorUtils+<SceneNodes>d__68")]
	public sealed class _SceneNodes_d__68 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr___l__initialThreadId;

		private static readonly System.IntPtr NativeFieldInfoPtr_root;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__root;

		private static readonly System.IntPtr NativeFieldInfoPtr___7__wrap1;

		private static readonly System.IntPtr NativeFieldInfoPtr___7__wrap2;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___m__Finally1_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___m__Finally2_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__get_Current_Private_Virtual_Final_New_get_SceneNode_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNode_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe SceneNode __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe int __l__initialThreadId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId)) = num;
			}
		}

		public unsafe SceneNode root
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_root);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_root)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNode __3__root
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__root);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__root)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe List<SceneNode>.Enumerator __7__wrap1
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap1);
				return new List<SceneNode>.Enumerator(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<List<SceneNode>.Enumerator>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap1), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerator)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<List<SceneNode>.Enumerator>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe IEnumerator<SceneNode> __7__wrap2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap2);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<SceneNode>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerator));
			}
		}

		public unsafe virtual SceneNode System_002ECollections_002EGeneric_002EIEnumerator_003CToolBuddy_002EThirdParty_002EVectorGraphics_002ESceneNode_003E_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__get_Current_Private_Virtual_Final_New_get_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _SceneNodes_d__68()
		{
			Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "<SceneNodes>d__68");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___l__initialThreadId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>l__initialThreadId");
			NativeFieldInfoPtr_root = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "root");
			NativeFieldInfoPtr___3__root = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>3__root");
			NativeFieldInfoPtr___7__wrap1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>7__wrap1");
			NativeFieldInfoPtr___7__wrap2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, "<>7__wrap2");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663634);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663635);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663636);
			NativeMethodInfoPtr___m__Finally1_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663637);
			NativeMethodInfoPtr___m__Finally2_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663638);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__get_Current_Private_Virtual_Final_New_get_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663639);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663640);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663641);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663642);
			NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr, 100663643);
		}

		[CallerCount(21)]
		[CachedScanResults(RefRangeStart = 24684, RefRangeEnd = 24705, XrefRangeStart = 24684, XrefRangeEnd = 24705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _SceneNodes_d__68(int _003C_003E1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_SceneNodes_d__68>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&_003C_003E1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26913, XrefRangeEnd = 26926, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26926, XrefRangeEnd = 26969, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26969, XrefRangeEnd = 26972, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void __m__Finally1()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___m__Finally1_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26972, XrefRangeEnd = 26975, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void __m__Finally2()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___m__Finally2_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26975, XrefRangeEnd = 26980, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26980, XrefRangeEnd = 26988, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator<SceneNode> System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_SceneNode__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<SceneNode>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
		}

		public _SceneNodes_d__68(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ToolBuddy.ThirdParty.VectorGraphics.VectorUtils+<SegmentsInPath>d__75")]
	public sealed class _SegmentsInPath_d__75 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr___l__initialThreadId;

		private static readonly System.IntPtr NativeFieldInfoPtr_segments;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__segments;

		private static readonly System.IntPtr NativeFieldInfoPtr_closed;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__closed;

		private static readonly System.IntPtr NativeFieldInfoPtr__e_5__2;

		private static readonly System.IntPtr NativeFieldInfoPtr__s2_5__3;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__get_Current_Private_Virtual_Final_New_get_BezierSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_BezierSegment_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe BezierSegment __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				return *(BezierSegment*)num;
			}
			set
			{
				*(BezierSegment*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)) = bezierSegment;
			}
		}

		public unsafe int __l__initialThreadId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId)) = num;
			}
		}

		public unsafe IEnumerable<BezierPathSegment> segments
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<BezierPathSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerable));
			}
		}

		public unsafe IEnumerable<BezierPathSegment> __3__segments
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__segments);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<BezierPathSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__segments)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerable));
			}
		}

		public unsafe bool closed
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed)) = flag;
			}
		}

		public unsafe bool __3__closed
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__closed);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__closed)) = flag;
			}
		}

		public unsafe IEnumerator<BezierPathSegment> _e_5__2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__e_5__2);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<BezierPathSegment>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__e_5__2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerator));
			}
		}

		public unsafe BezierPathSegment _s2_5__3
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__s2_5__3);
				return *(BezierPathSegment*)num;
			}
			set
			{
				*(BezierPathSegment*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__s2_5__3)) = bezierPathSegment;
			}
		}

		public unsafe virtual BezierSegment System_002ECollections_002EGeneric_002EIEnumerator_003CToolBuddy_002EThirdParty_002EVectorGraphics_002EBezierSegment_003E_002ECurrent
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__get_Current_Private_Virtual_Final_New_get_BezierSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26999, XrefRangeEnd = 27002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _SegmentsInPath_d__75()
		{
			Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "<SegmentsInPath>d__75");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___l__initialThreadId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<>l__initialThreadId");
			NativeFieldInfoPtr_segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "segments");
			NativeFieldInfoPtr___3__segments = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<>3__segments");
			NativeFieldInfoPtr_closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "closed");
			NativeFieldInfoPtr___3__closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<>3__closed");
			NativeFieldInfoPtr__e_5__2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<e>5__2");
			NativeFieldInfoPtr__s2_5__3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, "<s2>5__3");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663644);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663645);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663646);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__get_Current_Private_Virtual_Final_New_get_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663647);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663648);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663649);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663650);
			NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr, 100663651);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26988, XrefRangeEnd = 26990, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _SegmentsInPath_d__75(int _003C_003E1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_SegmentsInPath_d__75>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&_003C_003E1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(14950)]
		[CachedScanResults(RefRangeStart = 4192, RefRangeEnd = 19142, XrefRangeStart = 4192, XrefRangeEnd = 19142, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26990, XrefRangeEnd = 26994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 26994, XrefRangeEnd = 26999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27002, XrefRangeEnd = 27010, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator<BezierSegment> System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_BezierSegment__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_BezierSegment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<BezierSegment>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
		}

		public _SegmentsInPath_d__75(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("ToolBuddy.ThirdParty.VectorGraphics.VectorUtils+<WorldTransformedSceneNodes>d__70")]
	public sealed class _WorldTransformedSceneNodes_d__70 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr___l__initialThreadId;

		private static readonly System.IntPtr NativeFieldInfoPtr_nodeOpacities;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__nodeOpacities;

		private static readonly System.IntPtr NativeFieldInfoPtr_child;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__child;

		private static readonly System.IntPtr NativeFieldInfoPtr_parent;

		private static readonly System.IntPtr NativeFieldInfoPtr___3__parent;

		private static readonly System.IntPtr NativeFieldInfoPtr__childWorldTransform_5__2;

		private static readonly System.IntPtr NativeFieldInfoPtr___7__wrap2;

		private static readonly System.IntPtr NativeFieldInfoPtr___7__wrap3;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___m__Finally1_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr___m__Finally2_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__get_Current_Private_Virtual_Final_New_get_SceneNodeWorldTransform_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNodeWorldTransform_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe SceneNodeWorldTransform __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				return new SceneNodeWorldTransform(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNodeWorldTransform)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe int __l__initialThreadId
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___l__initialThreadId)) = num;
			}
		}

		public unsafe Dictionary<SceneNode, float> nodeOpacities
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeOpacities);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeOpacities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe Dictionary<SceneNode, float> __3__nodeOpacities
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__nodeOpacities);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__nodeOpacities)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe SceneNode child
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_child);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_child)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNode __3__child
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__child);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__child)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNodeWorldTransform parent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent);
				return new SceneNodeWorldTransform(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNodeWorldTransform)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe SceneNodeWorldTransform __3__parent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__parent);
				return new SceneNodeWorldTransform(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___3__parent), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNodeWorldTransform)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe SceneNodeWorldTransform _childWorldTransform_5__2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childWorldTransform_5__2);
				return new SceneNodeWorldTransform(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childWorldTransform_5__2), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNodeWorldTransform)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<SceneNodeWorldTransform>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe List<SceneNode>.Enumerator __7__wrap2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap2);
				return new List<SceneNode>.Enumerator(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<List<SceneNode>.Enumerator>.NativeClassPtr, (System.IntPtr)num));
			}
			set
			{
				// IL cpblk instruction
				Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap2), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerator)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<List<SceneNode>.Enumerator>.NativeClassPtr, ref *(uint*)null));
			}
		}

		public unsafe IEnumerator<SceneNodeWorldTransform> __7__wrap3
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap3);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<SceneNodeWorldTransform>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___7__wrap3)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enumerator));
			}
		}

		public unsafe virtual SceneNodeWorldTransform System_002ECollections_002EGeneric_002EIEnumerator_003CToolBuddy_002EThirdParty_002EVectorGraphics_002EVectorUtils_002ESceneNodeWorldTransform_003E_002ECurrent
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr);
				System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__get_Current_Private_Virtual_Final_New_get_SceneNodeWorldTransform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
				Il2CppException.RaiseExceptionIfNecessary(intPtr);
				return new SceneNodeWorldTransform(pointer);
			}
		}

		public unsafe virtual Il2CppSystem.Object System_002ECollections_002EIEnumerator_002ECurrent
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27088, XrefRangeEnd = 27091, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _WorldTransformedSceneNodes_d__70()
		{
			Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "<WorldTransformedSceneNodes>d__70");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___l__initialThreadId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>l__initialThreadId");
			NativeFieldInfoPtr_nodeOpacities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "nodeOpacities");
			NativeFieldInfoPtr___3__nodeOpacities = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>3__nodeOpacities");
			NativeFieldInfoPtr_child = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "child");
			NativeFieldInfoPtr___3__child = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>3__child");
			NativeFieldInfoPtr_parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "parent");
			NativeFieldInfoPtr___3__parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>3__parent");
			NativeFieldInfoPtr__childWorldTransform_5__2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<childWorldTransform>5__2");
			NativeFieldInfoPtr___7__wrap2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>7__wrap2");
			NativeFieldInfoPtr___7__wrap3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, "<>7__wrap3");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663652);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663653);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663654);
			NativeMethodInfoPtr___m__Finally1_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663655);
			NativeMethodInfoPtr___m__Finally2_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663656);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__get_Current_Private_Virtual_Final_New_get_SceneNodeWorldTransform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663657);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663658);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663659);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNodeWorldTransform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663660);
			NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr, 100663661);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27010, XrefRangeEnd = 27012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _WorldTransformedSceneNodes_d__70(int _003C_003E1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_WorldTransformedSceneNodes_d__70>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&_003C_003E1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27012, XrefRangeEnd = 27023, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27023, XrefRangeEnd = 27077, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27077, XrefRangeEnd = 27080, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void __m__Finally1()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___m__Finally1_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27080, XrefRangeEnd = 27083, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void __m__Finally2()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___m__Finally2_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27083, XrefRangeEnd = 27088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27091, XrefRangeEnd = 27101, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator<SceneNodeWorldTransform> System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerable_ToolBuddy_ThirdParty_VectorGraphics_VectorUtils_SceneNodeWorldTransform__GetEnumerator_Private_Virtual_Final_New_IEnumerator_1_SceneNodeWorldTransform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<SceneNodeWorldTransform>>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
		}

		public _WorldTransformedSceneNodes_d__70(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_s_ExpandEdgesMat;

	private static readonly System.IntPtr NativeFieldInfoPtr_Epsilon;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildEllipsePath_Internal_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeEllipseParameters_Private_Static_Void_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_byref_Vector2_byref_Single_byref_Single_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnsureRadiiAreLargeEnough_Private_Static_Void_Vector2_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildRectangleContour_Public_Static_BezierContour_Rect_Vector2_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FlipYAxis_Private_Static_Void_IList_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustWinding_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_WindingDir_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FlipRangeIfNecessary_Private_Static_Void_List_1_Vector2_List_1_UInt16_Int32_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenderFromArrays_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Color_Il2CppStructArray_1_Vector2_Texture2D_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenderSprite_Public_Static_Void_Sprite_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RenderSpriteToTexture2D_Public_Static_Texture2D_Sprite_Int32_Int32_Material_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPivot_Internal_Static_Vector2_Alignment_Vector2_Rect_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellatePath_Public_Static_Void_BezierContour_PathProperties_TessellationOptions_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TraceShape_Private_Static_Il2CppStructArray_1_Vector2_BezierContour_Stroke_TessellationOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TryGetMoreRemainingUnits_Private_Static_Boolean_byref_Single_PathDistanceForwardIterator_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleNewSegmentJoining_Private_Static_Void_PathDistanceForwardIterator_PathPatternIterator_Il2CppReferenceArray_1_JoiningInfo_Single_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkipRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellateRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_TessellationOptions_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_Single_Int32_List_1_Vector2_List_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddSegment_Private_Static_Void_BezierSegment_Single_Single_Il2CppReferenceArray_1_JoiningInfo_Single_List_1_Vector2_List_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ForeseeJoining_Private_Static_JoiningInfo_BezierSegment_BezierSegment_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LineBezierThicknessIntersect_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_Single_Vector2_Vector2_byref_Single_byref_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointOnLineIsWithinSegment_Private_Static_Boolean_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateJoining_Private_Static_Void_JoiningInfo_PathCorner_Single_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateTip_Private_Static_Void_BezierSegment_Boolean_Single_PathEnding_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CalculateArcSteps_Private_Static_Int32_Single_Single_Single_TessellationOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellateRect_Public_Static_Void_Rect_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TessellateRectBorder_Public_Static_Void_Rect_Single_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BezierSegmentToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BezierSegmentsToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PathSegmentAtIndex_Public_Static_BezierSegment_IList_1_BezierPathSegment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PathEndsPerfectlyMatch_Public_Static_Boolean_IList_1_BezierPathSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_Vector2_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeEllipseShape_Public_Static_Void_Shape_Vector2_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeCircleShape_Public_Static_Void_Shape_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Bounds_Public_Static_Rect_Il2CppStructArray_1_BezierPathSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Bounds_Public_Static_Rect_IEnumerable_1_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeLine_Public_Static_BezierSegment_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_QuadraticToCubic_Public_Static_BezierSegment_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakePathLine_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeArcQuarter_Internal_Static_BezierSegment_Vector2_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeArc_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_QuadrantAtAngle_Internal_Static_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ArcSegmentForQuadrant_Internal_Static_BezierSegment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FlipSegment_Public_Static_BezierSegment_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Bounds_Public_Static_Void_BezierSegment_byref_Vector2_byref_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Eval_Public_Static_Vector2_BezierSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalTangent_Public_Static_Vector2_BezierSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalNormal_Public_Static_Vector2_BezierSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_byref_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentsLengths_Public_Static_Il2CppStructArray_1_Single_IList_1_BezierPathSegment_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentsLength_Public_Static_Single_IList_1_BezierPathSegment_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentLength_Public_Static_Single_BezierSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentLengthIterative_Internal_Static_Single_BezierSegment_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HasLargeCoordinates_Internal_Static_Boolean_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdaptiveQuadraticApproxSplitPoint_Private_Static_Single_BezierSegment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MidPointQuadraticApproxLength_Private_Static_Single_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SplitSegment_Public_Static_Void_BezierSegment_Single_byref_BezierSegment_byref_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Vector2_Single_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SceneNodes_Public_Static_IEnumerable_1_SceneNode_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WorldTransformedSceneNodes_Private_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_SceneNodeWorldTransform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WorldTransformedSceneNodes_Public_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RealignVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FlipVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampVerticesInBounds_Internal_Static_Void_IList_1_Vector2_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SegmentsInPath_Public_Static_IEnumerable_1_BezierSegment_IEnumerable_1_BezierPathSegment_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SolveQuadratic_Private_Static_Void_Single_Single_Single_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IntersectLines_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IntersectLineSegments_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PointOnTheLeftOfLine_Private_Static_Boolean_Vector2_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindBezierLineIntersections_Public_Static_Il2CppStructArray_1_Single_BezierSegment_Vector2_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CubicRoots_Private_Static_Il2CppStructArray_1_Single_Double_Double_Double_Double_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BezierCoefficients_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SceneNodeBounds_Public_Static_Rect_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApproximateSceneNodeBounds_Public_Static_Rect_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsEmptySegment_Internal_Static_Boolean_BezierSegment_0;

	public unsafe static Material s_ExpandEdgesMat
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_s_ExpandEdgesMat, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_s_ExpandEdgesMat, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe static float Epsilon
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Epsilon, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Epsilon, (void*)(&num));
		}
	}

	static VectorUtils()
	{
		Il2CppClassPointerStore<VectorUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "VectorUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr);
		NativeFieldInfoPtr_s_ExpandEdgesMat = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "s_ExpandEdgesMat");
		NativeFieldInfoPtr_Epsilon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, "Epsilon");
		NativeMethodInfoPtr_BuildEllipsePath_Internal_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663536);
		NativeMethodInfoPtr_ComputeEllipseParameters_Private_Static_Void_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_byref_Vector2_byref_Single_byref_Single_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663537);
		NativeMethodInfoPtr_EnsureRadiiAreLargeEnough_Private_Static_Void_Vector2_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663538);
		NativeMethodInfoPtr_BuildRectangleContour_Public_Static_BezierContour_Rect_Vector2_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663539);
		NativeMethodInfoPtr_FlipYAxis_Private_Static_Void_IList_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663540);
		NativeMethodInfoPtr_AdjustWinding_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_WindingDir_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663541);
		NativeMethodInfoPtr_FlipRangeIfNecessary_Private_Static_Void_List_1_Vector2_List_1_UInt16_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663542);
		NativeMethodInfoPtr_RenderFromArrays_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Color_Il2CppStructArray_1_Vector2_Texture2D_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663543);
		NativeMethodInfoPtr_RenderSprite_Public_Static_Void_Sprite_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663544);
		NativeMethodInfoPtr_RenderSpriteToTexture2D_Public_Static_Texture2D_Sprite_Int32_Int32_Material_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663545);
		NativeMethodInfoPtr_GetPivot_Internal_Static_Vector2_Alignment_Vector2_Rect_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663546);
		NativeMethodInfoPtr_TessellatePath_Public_Static_Void_BezierContour_PathProperties_TessellationOptions_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663547);
		NativeMethodInfoPtr_TraceShape_Private_Static_Il2CppStructArray_1_Vector2_BezierContour_Stroke_TessellationOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663548);
		NativeMethodInfoPtr_TryGetMoreRemainingUnits_Private_Static_Boolean_byref_Single_PathDistanceForwardIterator_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663549);
		NativeMethodInfoPtr_HandleNewSegmentJoining_Private_Static_Void_PathDistanceForwardIterator_PathPatternIterator_Il2CppReferenceArray_1_JoiningInfo_Single_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663550);
		NativeMethodInfoPtr_SkipRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663551);
		NativeMethodInfoPtr_TessellateRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_TessellationOptions_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_Single_Int32_List_1_Vector2_List_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663552);
		NativeMethodInfoPtr_AddSegment_Private_Static_Void_BezierSegment_Single_Single_Il2CppReferenceArray_1_JoiningInfo_Single_List_1_Vector2_List_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663553);
		NativeMethodInfoPtr_ForeseeJoining_Private_Static_JoiningInfo_BezierSegment_BezierSegment_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663554);
		NativeMethodInfoPtr_LineBezierThicknessIntersect_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_Single_Vector2_Vector2_byref_Single_byref_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663555);
		NativeMethodInfoPtr_PointOnLineIsWithinSegment_Private_Static_Boolean_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663556);
		NativeMethodInfoPtr_GenerateJoining_Private_Static_Void_JoiningInfo_PathCorner_Single_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663557);
		NativeMethodInfoPtr_GenerateTip_Private_Static_Void_BezierSegment_Boolean_Single_PathEnding_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663558);
		NativeMethodInfoPtr_CalculateArcSteps_Private_Static_Int32_Single_Single_Single_TessellationOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663559);
		NativeMethodInfoPtr_TessellateRect_Public_Static_Void_Rect_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663560);
		NativeMethodInfoPtr_TessellateRectBorder_Public_Static_Void_Rect_Single_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663561);
		NativeMethodInfoPtr_BezierSegmentToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663562);
		NativeMethodInfoPtr_BezierSegmentsToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663563);
		NativeMethodInfoPtr_PathSegmentAtIndex_Public_Static_BezierSegment_IList_1_BezierPathSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663564);
		NativeMethodInfoPtr_PathEndsPerfectlyMatch_Public_Static_Boolean_IList_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663565);
		NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663566);
		NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_Vector2_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663567);
		NativeMethodInfoPtr_MakeEllipseShape_Public_Static_Void_Shape_Vector2_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663568);
		NativeMethodInfoPtr_MakeCircleShape_Public_Static_Void_Shape_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663569);
		NativeMethodInfoPtr_Bounds_Public_Static_Rect_Il2CppStructArray_1_BezierPathSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663570);
		NativeMethodInfoPtr_Bounds_Public_Static_Rect_IEnumerable_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663571);
		NativeMethodInfoPtr_MakeLine_Public_Static_BezierSegment_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663572);
		NativeMethodInfoPtr_QuadraticToCubic_Public_Static_BezierSegment_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663573);
		NativeMethodInfoPtr_MakePathLine_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663574);
		NativeMethodInfoPtr_MakeArcQuarter_Internal_Static_BezierSegment_Vector2_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663575);
		NativeMethodInfoPtr_MakeArc_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663576);
		NativeMethodInfoPtr_QuadrantAtAngle_Internal_Static_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663577);
		NativeMethodInfoPtr_ArcSegmentForQuadrant_Internal_Static_BezierSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663578);
		NativeMethodInfoPtr_FlipSegment_Public_Static_BezierSegment_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663579);
		NativeMethodInfoPtr_Bounds_Public_Static_Void_BezierSegment_byref_Vector2_byref_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663580);
		NativeMethodInfoPtr_Eval_Public_Static_Vector2_BezierSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663581);
		NativeMethodInfoPtr_EvalTangent_Public_Static_Vector2_BezierSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663582);
		NativeMethodInfoPtr_EvalNormal_Public_Static_Vector2_BezierSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663583);
		NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663584);
		NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_byref_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663585);
		NativeMethodInfoPtr_SegmentsLengths_Public_Static_Il2CppStructArray_1_Single_IList_1_BezierPathSegment_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663586);
		NativeMethodInfoPtr_SegmentsLength_Public_Static_Single_IList_1_BezierPathSegment_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663587);
		NativeMethodInfoPtr_SegmentLength_Public_Static_Single_BezierSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663588);
		NativeMethodInfoPtr_SegmentLengthIterative_Internal_Static_Single_BezierSegment_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663589);
		NativeMethodInfoPtr_HasLargeCoordinates_Internal_Static_Boolean_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663590);
		NativeMethodInfoPtr_AdaptiveQuadraticApproxSplitPoint_Private_Static_Single_BezierSegment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663591);
		NativeMethodInfoPtr_MidPointQuadraticApproxLength_Private_Static_Single_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663592);
		NativeMethodInfoPtr_SplitSegment_Public_Static_Void_BezierSegment_Single_byref_BezierSegment_byref_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663593);
		NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Vector2_Single_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663594);
		NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663595);
		NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663596);
		NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663597);
		NativeMethodInfoPtr_SceneNodes_Public_Static_IEnumerable_1_SceneNode_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663598);
		NativeMethodInfoPtr_WorldTransformedSceneNodes_Private_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_SceneNodeWorldTransform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663599);
		NativeMethodInfoPtr_WorldTransformedSceneNodes_Public_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663600);
		NativeMethodInfoPtr_RealignVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663601);
		NativeMethodInfoPtr_FlipVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663602);
		NativeMethodInfoPtr_ClampVerticesInBounds_Internal_Static_Void_IList_1_Vector2_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663603);
		NativeMethodInfoPtr_SegmentsInPath_Public_Static_IEnumerable_1_BezierSegment_IEnumerable_1_BezierPathSegment_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663604);
		NativeMethodInfoPtr_SolveQuadratic_Private_Static_Void_Single_Single_Single_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663605);
		NativeMethodInfoPtr_IntersectLines_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663606);
		NativeMethodInfoPtr_IntersectLineSegments_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663607);
		NativeMethodInfoPtr_PointOnTheLeftOfLine_Private_Static_Boolean_Vector2_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663608);
		NativeMethodInfoPtr_FindBezierLineIntersections_Public_Static_Il2CppStructArray_1_Single_BezierSegment_Vector2_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663609);
		NativeMethodInfoPtr_CubicRoots_Private_Static_Il2CppStructArray_1_Single_Double_Double_Double_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663610);
		NativeMethodInfoPtr_BezierCoefficients_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663611);
		NativeMethodInfoPtr_SceneNodeBounds_Public_Static_Rect_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663612);
		NativeMethodInfoPtr_ApproximateSceneNodeBounds_Public_Static_Rect_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663613);
		NativeMethodInfoPtr_IsEmptySegment_Internal_Static_Boolean_BezierSegment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<VectorUtils>.NativeClassPtr, 100663614);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27130, RefRangeEnd = 27131, XrefRangeStart = 27101, XrefRangeEnd = 27130, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> BuildEllipsePath(Vector2 p0, Vector2 p1, float rotation, float rx, float ry, bool largeArc, bool sweep)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&p0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &rx;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &ry;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &largeArc;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &sweep;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildEllipsePath_Internal_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27155, RefRangeEnd = 27156, XrefRangeStart = 27131, XrefRangeEnd = 27155, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ComputeEllipseParameters(Vector2 p0, Vector2 p1, float phi, float rx, float ry, bool fa, bool fs, out Vector2 c, out float theta1, out float sweepTheta, out float adjustedRx, out float adjustedRy)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[12];
		*ptr = (nint)(&p0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &phi;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &rx;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &ry;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &fa;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &fs;
		*(void**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref c);
		*(void**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref theta1);
		*(void**)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref sweepTheta);
		*(void**)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref adjustedRx);
		*(void**)((byte*)ptr + checked((nuint)11u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref adjustedRy);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeEllipseParameters_Private_Static_Void_Vector2_Vector2_Single_Single_Single_Boolean_Boolean_byref_Vector2_byref_Single_byref_Single_byref_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27156, XrefRangeEnd = 27158, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void EnsureRadiiAreLargeEnough(Vector2 p, ref float rx, ref float ry)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&p);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref rx);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnsureRadiiAreLargeEnough_Private_Static_Void_Vector2_byref_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27274, RefRangeEnd = 27275, XrefRangeStart = 27158, XrefRangeEnd = 27274, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierContour BuildRectangleContour(Rect rect, Vector2 radiusTL, Vector2 radiusTR, Vector2 radiusBR, Vector2 radiusBL)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&rect);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusTL;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusTR;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusBR;
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusBL;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildRectangleContour_Public_Static_BezierContour_Rect_Vector2_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new BezierContour(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27275, XrefRangeEnd = 27291, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FlipYAxis(IList<Vector2> vertices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FlipYAxis_Private_Static_Void_IList_1_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27291, XrefRangeEnd = 27297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AdjustWinding(Il2CppStructArray<Vector2> vertices, Il2CppStructArray<ushort> indices, WindingDir dir)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)indices);
		*(WindingDir**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &dir;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustWinding_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_WindingDir_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27297, XrefRangeEnd = 27318, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FlipRangeIfNecessary(List<Vector2> vertices, List<ushort> indices, int indexStart, int indexEnd, bool flipYAxis)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)indices);
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &indexStart;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &indexEnd;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &flipYAxis;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FlipRangeIfNecessary_Private_Static_Void_List_1_Vector2_List_1_UInt16_Int32_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27318, XrefRangeEnd = 27338, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RenderFromArrays(Il2CppStructArray<Vector2> vertices, Il2CppStructArray<ushort> indices, Il2CppStructArray<Vector2> uvs, Il2CppStructArray<Color> colors, Il2CppStructArray<Vector2> settings, Texture2D texture, Material mat, bool clear = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)indices);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)uvs);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)colors);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &clear;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenderFromArrays_Internal_Static_Void_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_UInt16_Il2CppStructArray_1_Vector2_Il2CppStructArray_1_Color_Il2CppStructArray_1_Vector2_Texture2D_Material_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27423, RefRangeEnd = 27425, XrefRangeStart = 27338, XrefRangeEnd = 27423, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RenderSprite(Sprite sprite, Material mat, bool clear = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &clear;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenderSprite_Public_Static_Void_Sprite_Material_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27425, XrefRangeEnd = 27487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Texture2D RenderSpriteToTexture2D(Sprite sprite, int width, int height, Material mat, int antiAliasing = 1, bool expandEdges = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &width;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &height;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &antiAliasing;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &expandEdges;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RenderSpriteToTexture2D_Public_Static_Texture2D_Sprite_Int32_Int32_Material_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe static Vector2 GetPivot(Alignment alignment, Vector2 customPivot, Rect bbox, bool flipYAxis)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&alignment);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &customPivot;
		*(Rect**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &bbox;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &flipYAxis;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPivot_Internal_Static_Vector2_Alignment_Vector2_Rect_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27487, XrefRangeEnd = 27564, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void TessellatePath(BezierContour contour, PathProperties pathProps, TessellationOptions tessellateOptions, out Il2CppStructArray<Vector2> vertices, out Il2CppStructArray<ushort> indices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)contour));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)pathProps));
		*(TessellationOptions**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		byte* num = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellatePath_Public_Static_Void_BezierContour_PathProperties_TessellationOptions_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		vertices = ((num5 == 0) ? null : new Il2CppStructArray<Vector2>((System.IntPtr)num5));
		nint num6 = num4;
		indices = ((num6 == 0) ? null : new Il2CppStructArray<ushort>((System.IntPtr)num6));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27564, XrefRangeEnd = 27629, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<Vector2> TraceShape(BezierContour contour, Stroke stroke, TessellationOptions tessellateOptions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)contour));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stroke);
		*(TessellationOptions**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TraceShape_Private_Static_Il2CppStructArray_1_Vector2_BezierContour_Stroke_TessellationOptions_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27637, RefRangeEnd = 27639, XrefRangeStart = 27629, XrefRangeEnd = 27637, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool TryGetMoreRemainingUnits(ref float unitsRemaining, PathDistanceForwardIterator pathIt, float startingLength, float distance, float stepDistance)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)Unsafe.AsPointer(ref unitsRemaining);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathIt);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &startingLength;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &distance;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &stepDistance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TryGetMoreRemainingUnits_Private_Static_Boolean_byref_Single_PathDistanceForwardIterator_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27649, RefRangeEnd = 27651, XrefRangeStart = 27639, XrefRangeEnd = 27649, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void HandleNewSegmentJoining(PathDistanceForwardIterator pathIt, PathPatternIterator patternIt, Il2CppReferenceArray<JoiningInfo> joiningInfo, float halfThickness, Il2CppStructArray<float> segmentLengths)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)patternIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joiningInfo);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segmentLengths);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleNewSegmentJoining_Private_Static_Void_PathDistanceForwardIterator_PathPatternIterator_Il2CppReferenceArray_1_JoiningInfo_Single_Il2CppStructArray_1_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27656, RefRangeEnd = 27657, XrefRangeStart = 27651, XrefRangeEnd = 27656, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SkipRange(float distance, PathDistanceForwardIterator pathIt, PathPatternIterator patternIt, PathProperties pathProps, Il2CppReferenceArray<JoiningInfo> joiningInfo, Il2CppStructArray<float> segmentLengths)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&distance);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)patternIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)pathProps));
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joiningInfo);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segmentLengths);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkipRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27699, RefRangeEnd = 27700, XrefRangeStart = 27657, XrefRangeEnd = 27699, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void TessellateRange(float distance, PathDistanceForwardIterator pathIt, PathPatternIterator patternIt, PathProperties pathProps, TessellationOptions tessellateOptions, Il2CppReferenceArray<JoiningInfo> joiningInfo, Il2CppStructArray<float> segmentLengths, float totalLength, int rangeIndex, List<Vector2> verts, List<ushort> inds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[11];
		*ptr = (nint)(&distance);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pathIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)patternIt);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)pathProps));
		*(TessellationOptions**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joiningInfo);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segmentLengths);
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &totalLength;
		*(int**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &rangeIndex;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)9u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verts);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)10u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellateRange_Private_Static_Void_Single_PathDistanceForwardIterator_PathPatternIterator_PathProperties_TessellationOptions_Il2CppReferenceArray_1_JoiningInfo_Il2CppStructArray_1_Single_Single_Int32_List_1_Vector2_List_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 27733, RefRangeEnd = 27736, XrefRangeStart = 27700, XrefRangeEnd = 27733, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void AddSegment(BezierSegment segment, float toT, float halfThickness, Il2CppReferenceArray<JoiningInfo> joinInfo, float segmentLengthSoFar, List<Vector2> verts, List<ushort> inds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &toT;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joinInfo);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &segmentLengthSoFar;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verts);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddSegment_Private_Static_Void_BezierSegment_Single_Single_Il2CppReferenceArray_1_JoiningInfo_Single_List_1_Vector2_List_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27736, XrefRangeEnd = 27781, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static JoiningInfo ForeseeJoining(BezierSegment end, BezierSegment start, float halfThickness, float endSegmentLength)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&end);
		*(BezierSegment**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &start;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &endSegmentLength;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForeseeJoining_Private_Static_JoiningInfo_BezierSegment_BezierSegment_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JoiningInfo>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27795, RefRangeEnd = 27797, XrefRangeStart = 27781, XrefRangeEnd = 27795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<Vector2> LineBezierThicknessIntersect(BezierSegment seg, float thickness, Vector2 lineFrom, Vector2 lineTo, out float distanceToIntersection, out Vector2 intersection)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&seg);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &thickness;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineFrom;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineTo;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref distanceToIntersection);
		*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref intersection);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LineBezierThicknessIntersect_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_Single_Vector2_Vector2_byref_Single_byref_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 27805, RefRangeEnd = 27808, XrefRangeStart = 27797, XrefRangeEnd = 27805, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool PointOnLineIsWithinSegment(Vector2 lineFrom, Vector2 lineTo, Vector2 point)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&lineFrom);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineTo;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointOnLineIsWithinSegment_Private_Static_Boolean_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27852, RefRangeEnd = 27854, XrefRangeStart = 27808, XrefRangeEnd = 27852, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GenerateJoining(JoiningInfo joinInfo, PathCorner corner, float halfThickness, float tippedCornerLimit, TessellationOptions tessellateOptions, List<Vector2> verts, List<ushort> inds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)joinInfo);
		*(PathCorner**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &corner;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &tippedCornerLimit;
		*(TessellationOptions**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verts);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateJoining_Private_Static_Void_JoiningInfo_PathCorner_Single_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 27924, RefRangeEnd = 27926, XrefRangeStart = 27854, XrefRangeEnd = 27924, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void GenerateTip(BezierSegment segment, bool atStart, float t, PathEnding ending, float halfThickness, TessellationOptions tessellateOptions, List<Vector2> verts, List<ushort> inds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = (nint)(&segment);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &atStart;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		*(PathEnding**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &ending;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		*(TessellationOptions**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)verts);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)inds);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateTip_Private_Static_Void_BezierSegment_Boolean_Single_PathEnding_Single_TessellationOptions_List_1_Vector2_List_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 27934, RefRangeEnd = 27935, XrefRangeStart = 27926, XrefRangeEnd = 27934, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int CalculateArcSteps(float radius, float fromAngle, float toAngle, TessellationOptions tessellateOptions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&radius);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &fromAngle;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &toAngle;
		*(TessellationOptions**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &tessellateOptions;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CalculateArcSteps_Private_Static_Int32_Single_Single_Single_TessellationOptions_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27935, XrefRangeEnd = 27948, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void TessellateRect(Rect rect, out Il2CppStructArray<Vector2> vertices, out Il2CppStructArray<ushort> indices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&rect);
		byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellateRect_Public_Static_Void_Rect_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		vertices = ((num5 == 0) ? null : new Il2CppStructArray<Vector2>((System.IntPtr)num5));
		nint num6 = num4;
		indices = ((num6 == 0) ? null : new Il2CppStructArray<ushort>((System.IntPtr)num6));
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 27948, XrefRangeEnd = 28076, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void TessellateRectBorder(Rect rect, float halfThickness, out Il2CppStructArray<Vector2> vertices, out Il2CppStructArray<ushort> indices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&rect);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &halfThickness;
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		byte* num3 = (byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)));
		nint num4 = 0;
		*(nint**)num3 = &num4;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TessellateRectBorder_Public_Static_Void_Rect_Single_byref_Il2CppStructArray_1_Vector2_byref_Il2CppStructArray_1_UInt16_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num5 = num2;
		vertices = ((num5 == 0) ? null : new Il2CppStructArray<Vector2>((System.IntPtr)num5));
		nint num6 = num4;
		indices = ((num6 == 0) ? null : new Il2CppStructArray<ushort>((System.IntPtr)num6));
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28079, RefRangeEnd = 28080, XrefRangeStart = 28076, XrefRangeEnd = 28079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> BezierSegmentToPath(BezierSegment segment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BezierSegmentToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28108, RefRangeEnd = 28109, XrefRangeStart = 28080, XrefRangeEnd = 28108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> BezierSegmentsToPath(Il2CppStructArray<BezierSegment> segments)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BezierSegmentsToPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28121, RefRangeEnd = 28126, XrefRangeStart = 28109, XrefRangeEnd = 28121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment PathSegmentAtIndex(IList<BezierPathSegment> path, int index)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PathSegmentAtIndex_Public_Static_BezierSegment_IList_1_BezierPathSegment_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28140, RefRangeEnd = 28141, XrefRangeStart = 28126, XrefRangeEnd = 28140, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool PathEndsPerfectlyMatch(IList<BezierPathSegment> path)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PathEndsPerfectlyMatch_Public_Static_Boolean_IList_1_BezierPathSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 28153, RefRangeEnd = 28156, XrefRangeStart = 28141, XrefRangeEnd = 28153, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeRectangleShape(Shape rectShape, Rect rect)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectShape);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rect;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 28165, RefRangeEnd = 28169, XrefRangeStart = 28156, XrefRangeEnd = 28165, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeRectangleShape(Shape rectShape, Rect rect, Vector2 radiusTL, Vector2 radiusTR, Vector2 radiusBR, Vector2 radiusBL)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectShape);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &rect;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusTL;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusTR;
		*(Vector2**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusBR;
		*(Vector2**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusBL;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeRectangleShape_Public_Static_Void_Shape_Rect_Vector2_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28173, RefRangeEnd = 28174, XrefRangeStart = 28169, XrefRangeEnd = 28173, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeEllipseShape(Shape ellipseShape, Vector2 pos, float radiusX, float radiusY)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)ellipseShape);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusX;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &radiusY;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeEllipseShape_Public_Static_Void_Shape_Vector2_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28181, RefRangeEnd = 28182, XrefRangeStart = 28174, XrefRangeEnd = 28181, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void MakeCircleShape(Shape circleShape, Vector2 pos, float radius)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)circleShape);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &radius;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeCircleShape_Public_Static_Void_Shape_Vector2_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28207, RefRangeEnd = 28209, XrefRangeStart = 28182, XrefRangeEnd = 28207, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect Bounds(Il2CppStructArray<BezierPathSegment> path)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bounds_Public_Static_Rect_Il2CppStructArray_1_BezierPathSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 28227, RefRangeEnd = 28230, XrefRangeStart = 28209, XrefRangeEnd = 28227, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect Bounds(IEnumerable<Vector2> vertices)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bounds_Public_Static_Rect_IEnumerable_1_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 28230, RefRangeEnd = 28240, XrefRangeStart = 28230, XrefRangeEnd = 28230, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment MakeLine(Vector2 from, Vector2 to)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&from);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &to;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeLine_Public_Static_BezierSegment_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static BezierSegment QuadraticToCubic(Vector2 p0, Vector2 p1, Vector2 p2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&p0);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_QuadraticToCubic_Public_Static_BezierSegment_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 28243, RefRangeEnd = 28247, XrefRangeStart = 28240, XrefRangeEnd = 28243, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> MakePathLine(Vector2 from, Vector2 to)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&from);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &to;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakePathLine_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28257, RefRangeEnd = 28259, XrefRangeStart = 28247, XrefRangeEnd = 28257, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment MakeArcQuarter(Vector2 center, float startAngleRads, float sweepAngleRads)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&center);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startAngleRads;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sweepAngleRads;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeArcQuarter_Internal_Static_BezierSegment_Vector2_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28353, RefRangeEnd = 28358, XrefRangeStart = 28259, XrefRangeEnd = 28353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> MakeArc(Vector2 center, float startAngleRads, float sweepAngleRads, float radius)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&center);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &startAngleRads;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sweepAngleRads;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &radius;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeArc_Public_Static_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28358, XrefRangeEnd = 28359, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static int QuadrantAtAngle(float angle)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&angle);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_QuadrantAtAngle_Internal_Static_Int32_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28359, XrefRangeEnd = 28381, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment ArcSegmentForQuadrant(int quadrant)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&quadrant);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ArcSegmentForQuadrant_Internal_Static_BezierSegment_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static BezierSegment FlipSegment(BezierSegment segment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FlipSegment_Public_Static_BezierSegment_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28393, RefRangeEnd = 28394, XrefRangeStart = 28381, XrefRangeEnd = 28393, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Bounds(BezierSegment segment, out Vector2 min, out Vector2 max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&segment);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref min);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref max);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Bounds_Public_Static_Void_BezierSegment_byref_Vector2_byref_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 28394, RefRangeEnd = 28406, XrefRangeStart = 28394, XrefRangeEnd = 28394, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 Eval(BezierSegment segment, float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Eval_Public_Static_Vector2_BezierSegment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28417, RefRangeEnd = 28422, XrefRangeStart = 28406, XrefRangeEnd = 28417, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 EvalTangent(BezierSegment segment, float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvalTangent_Public_Static_Vector2_BezierSegment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28422, XrefRangeEnd = 28426, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 EvalNormal(BezierSegment segment, float t)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvalNormal_Public_Static_Vector2_BezierSegment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28436, RefRangeEnd = 28441, XrefRangeStart = 28426, XrefRangeEnd = 28436, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 EvalFull(BezierSegment segment, float t, out Vector2 tangent)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28441, XrefRangeEnd = 28445, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 EvalFull(BezierSegment segment, float t, out Vector2 tangent, out Vector2 normal)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref tangent);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref normal);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EvalFull_Public_Static_Vector2_BezierSegment_Single_byref_Vector2_byref_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28476, RefRangeEnd = 28478, XrefRangeStart = 28445, XrefRangeEnd = 28476, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<float> SegmentsLengths(IList<BezierPathSegment> segments, bool closed, float precision = 0.001f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &precision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentsLengths_Public_Static_Il2CppStructArray_1_Single_IList_1_BezierPathSegment_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28478, XrefRangeEnd = 28518, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float SegmentsLength(IList<BezierPathSegment> segments, bool closed, float precision = 0.001f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &precision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentsLength_Public_Static_Single_IList_1_BezierPathSegment_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28549, RefRangeEnd = 28551, XrefRangeStart = 28518, XrefRangeEnd = 28549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float SegmentLength(BezierSegment segment, float precision = 0.001f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &precision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentLength_Public_Static_Single_BezierSegment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28560, RefRangeEnd = 28562, XrefRangeStart = 28551, XrefRangeEnd = 28560, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float SegmentLengthIterative(BezierSegment segment, int steps = 10)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &steps;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentLengthIterative_Internal_Static_Single_BezierSegment_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool HasLargeCoordinates(BezierSegment segment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasLargeCoordinates_Internal_Static_Boolean_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28562, XrefRangeEnd = 28570, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float AdaptiveQuadraticApproxSplitPoint(BezierSegment segment, float precision)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &precision;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdaptiveQuadraticApproxSplitPoint_Private_Static_Single_BezierSegment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28597, RefRangeEnd = 28599, XrefRangeStart = 28570, XrefRangeEnd = 28597, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float MidPointQuadraticApproxLength(BezierSegment segment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MidPointQuadraticApproxLength_Private_Static_Single_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 28603, RefRangeEnd = 28606, XrefRangeStart = 28599, XrefRangeEnd = 28603, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SplitSegment(BezierSegment segment, float t, out BezierSegment b1, out BezierSegment b2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&segment);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref b1);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref b2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SplitSegment_Public_Static_Void_BezierSegment_Single_byref_BezierSegment_byref_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28614, RefRangeEnd = 28615, XrefRangeStart = 28606, XrefRangeEnd = 28614, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment TransformSegment(BezierSegment segment, Vector2 translation, float rotation, Vector2 scaling)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&segment);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &translation;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaling;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Vector2_Single_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28615, XrefRangeEnd = 28622, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static BezierSegment TransformSegment(BezierSegment segment, Matrix2D matrix)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&segment);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &matrix;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransformSegment_Public_Static_BezierSegment_BezierSegment_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(BezierSegment*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28635, RefRangeEnd = 28640, XrefRangeStart = 28622, XrefRangeEnd = 28635, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> TransformBezierPath(Il2CppStructArray<BezierPathSegment> path, Vector2 translation, float rotation, Vector2 scaling)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &translation;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rotation;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &scaling;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Vector2_Single_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28650, RefRangeEnd = 28652, XrefRangeStart = 28640, XrefRangeEnd = 28650, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<BezierPathSegment> TransformBezierPath(Il2CppStructArray<BezierPathSegment> path, Matrix2D matrix)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)path);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &matrix;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_TransformBezierPath_Public_Static_Il2CppStructArray_1_BezierPathSegment_Il2CppStructArray_1_BezierPathSegment_Matrix2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<BezierPathSegment>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28658, RefRangeEnd = 28660, XrefRangeStart = 28652, XrefRangeEnd = 28658, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerable<SceneNode> SceneNodes(SceneNode root)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SceneNodes_Public_Static_IEnumerable_1_SceneNode_SceneNode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<SceneNode>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28660, XrefRangeEnd = 28668, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerable<SceneNodeWorldTransform> WorldTransformedSceneNodes(SceneNode child, Dictionary<SceneNode, float> nodeOpacities, SceneNodeWorldTransform parent)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)child);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nodeOpacities);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)parent));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WorldTransformedSceneNodes_Private_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_SceneNodeWorldTransform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<SceneNodeWorldTransform>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 28688, RefRangeEnd = 28691, XrefRangeStart = 28668, XrefRangeEnd = 28688, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerable<SceneNodeWorldTransform> WorldTransformedSceneNodes(SceneNode root, Dictionary<SceneNode, float> nodeOpacities)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nodeOpacities);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WorldTransformedSceneNodes_Public_Static_IEnumerable_1_SceneNodeWorldTransform_SceneNode_Dictionary_2_SceneNode_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<SceneNodeWorldTransform>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28691, XrefRangeEnd = 28703, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void RealignVerticesInBounds(IList<Vector2> vertices, Rect bounds, bool flip)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &bounds;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &flip;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RealignVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28703, XrefRangeEnd = 28715, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void FlipVerticesInBounds(IList<Vector2> vertices, Rect bounds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &bounds;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FlipVerticesInBounds_Public_Static_Void_IList_1_Vector2_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28715, XrefRangeEnd = 28727, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void ClampVerticesInBounds(IList<Vector2> vertices, Rect bounds)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &bounds;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampVerticesInBounds_Internal_Static_Void_IList_1_Vector2_Rect_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 28733, RefRangeEnd = 28737, XrefRangeStart = 28727, XrefRangeEnd = 28733, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerable<BezierSegment> SegmentsInPath(IEnumerable<BezierPathSegment> segments, bool closed = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)segments);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &closed;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SegmentsInPath_Public_Static_IEnumerable_1_BezierSegment_IEnumerable_1_BezierPathSegment_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<BezierSegment>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28739, RefRangeEnd = 28741, XrefRangeStart = 28737, XrefRangeEnd = 28739, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SolveQuadratic(float a, float b, float c, out float s1, out float s2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&a);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &c;
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref s1);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref s2);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SolveQuadratic_Private_Static_Void_Single_Single_Single_byref_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 28746, RefRangeEnd = 28750, XrefRangeStart = 28741, XrefRangeEnd = 28746, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 IntersectLines(Vector2 line1Pt1, Vector2 line1Pt2, Vector2 line2Pt1, Vector2 line2Pt2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&line1Pt1);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line1Pt2;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2Pt1;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2Pt2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IntersectLines_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28750, XrefRangeEnd = 28751, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector2 IntersectLineSegments(Vector2 line1Pt1, Vector2 line1Pt2, Vector2 line2Pt1, Vector2 line2Pt2)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&line1Pt1);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &line1Pt2;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2Pt1;
		*(Vector2**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &line2Pt2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IntersectLineSegments_Public_Static_Vector2_Vector2_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static bool PointOnTheLeftOfLine(Vector2 lineFrom, Vector2 lineTo, Vector2 point)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&lineFrom);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &lineTo;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &point;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PointOnTheLeftOfLine_Private_Static_Boolean_Vector2_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 28778, RefRangeEnd = 28780, XrefRangeStart = 28751, XrefRangeEnd = 28778, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<float> FindBezierLineIntersections(BezierSegment segment, Vector2 p0, Vector2 p1)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&segment);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &p0;
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &p1;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindBezierLineIntersections_Public_Static_Il2CppStructArray_1_Single_BezierSegment_Vector2_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28890, RefRangeEnd = 28891, XrefRangeStart = 28780, XrefRangeEnd = 28890, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<float> CubicRoots(double a, double b, double c, double d)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&a);
		*(double**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &b;
		*(double**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &c;
		*(double**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &d;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CubicRoots_Private_Static_Il2CppStructArray_1_Single_Double_Double_Double_Double_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 28894, RefRangeEnd = 28895, XrefRangeStart = 28891, XrefRangeEnd = 28894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Il2CppStructArray<Vector2> BezierCoefficients(BezierSegment segment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&segment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BezierCoefficients_Private_Static_Il2CppStructArray_1_Vector2_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 28935, RefRangeEnd = 28940, XrefRangeStart = 28895, XrefRangeEnd = 28935, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect SceneNodeBounds(SceneNode root)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SceneNodeBounds_Public_Static_Rect_SceneNode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 28940, XrefRangeEnd = 28994, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Rect ApproximateSceneNodeBounds(SceneNode root)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApproximateSceneNodeBounds_Public_Static_Rect_SceneNode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 29002, RefRangeEnd = 29004, XrefRangeStart = 28994, XrefRangeEnd = 29002, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsEmptySegment(BezierSegment bs)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&bs);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsEmptySegment_Internal_Static_Boolean_BezierSegment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public VectorUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
