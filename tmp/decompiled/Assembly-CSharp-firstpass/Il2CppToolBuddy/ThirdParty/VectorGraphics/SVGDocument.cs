using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Xml;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGDocument : Il2CppSystem.Object
{
	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ViewBoxAlign")]
	public enum ViewBoxAlign
	{
		Min,
		Mid,
		Max
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "ViewBoxAspectRatio")]
	public enum ViewBoxAspectRatio
	{
		DontPreserve,
		FitLargestDim,
		FitSmallestDim
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct ViewBoxInfo
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ViewBox;

		private static readonly System.IntPtr NativeFieldInfoPtr_AspectRatio;

		private static readonly System.IntPtr NativeFieldInfoPtr_AlignX;

		private static readonly System.IntPtr NativeFieldInfoPtr_AlignY;

		private static readonly System.IntPtr NativeFieldInfoPtr_IsEmpty;

		[FieldOffset(0)]
		public Rect ViewBox;

		[FieldOffset(16)]
		public ViewBoxAspectRatio AspectRatio;

		[FieldOffset(20)]
		public ViewBoxAlign AlignX;

		[FieldOffset(24)]
		public ViewBoxAlign AlignY;

		[FieldOffset(28)]
		[MarshalAs(UnmanagedType.U1)]
		public bool IsEmpty;

		static ViewBoxInfo()
		{
			Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "ViewBoxInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr);
			NativeFieldInfoPtr_ViewBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, "ViewBox");
			NativeFieldInfoPtr_AspectRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, "AspectRatio");
			NativeFieldInfoPtr_AlignX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, "AlignX");
			NativeFieldInfoPtr_AlignY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, "AlignY");
			NativeFieldInfoPtr_IsEmpty = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, "IsEmpty");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ViewBoxInfo>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public sealed class HierarchyUpdate : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Parent;

		private static readonly System.IntPtr NativeFieldInfoPtr_NewNode;

		private static readonly System.IntPtr NativeFieldInfoPtr_ReplaceNode;

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

		public unsafe SceneNode NewNode
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NewNode);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_NewNode)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNode ReplaceNode
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReplaceNode);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ReplaceNode)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		static HierarchyUpdate()
		{
			Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "HierarchyUpdate");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr);
			NativeFieldInfoPtr_Parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr, "Parent");
			NativeFieldInfoPtr_NewNode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr, "NewNode");
			NativeFieldInfoPtr_ReplaceNode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr, "ReplaceNode");
		}

		public HierarchyUpdate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public HierarchyUpdate()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<HierarchyUpdate>.NativeClassPtr))
		{
		}
	}

	public sealed class ElemHandler : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static ElemHandler()
		{
			Il2CppClassPointerStore<ElemHandler>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "ElemHandler");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ElemHandler>.NativeClassPtr, 100663449);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ElemHandler>.NativeClassPtr, 100663450);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ElemHandler>.NativeClassPtr, 100663451);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ElemHandler>.NativeClassPtr, 100663452);
		}

		[CallerCount(1472)]
		[CachedScanResults(RefRangeStart = 20074, RefRangeEnd = 21546, XrefRangeStart = 20071, XrefRangeEnd = 20074, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe ElemHandler(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ElemHandler>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual void Invoke()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 21546, XrefRangeEnd = 21547, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 21547, XrefRangeEnd = 21548, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public ElemHandler(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator ElemHandler(System.Action P_0)
		{
			return DelegateSupport.ConvertDelegate<ElemHandler>((System.Delegate)P_0);
		}

		public static ElemHandler operator +(ElemHandler P_0, ElemHandler P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<ElemHandler>();
		}

		public static ElemHandler operator -(ElemHandler P_0, ElemHandler P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<ElemHandler>();
			}
			return (ElemHandler)obj;
		}
	}

	public class Handlers : Dictionary<string, ElemHandler>
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		static Handlers()
		{
			Il2CppClassPointerStore<Handlers>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "Handlers");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Handlers>.NativeClassPtr);
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Handlers>.NativeClassPtr, 100663453);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 21548, XrefRangeEnd = 21551, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Handlers(int capacity)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Handlers>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&capacity);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public Handlers(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[OriginalName("Assembly-CSharp-firstpass.dll", "", "DimType")]
	public enum DimType
	{
		Width,
		Height,
		Length
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct NodeGlobalSceneState
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_ContainerSize;

		[FieldOffset(0)]
		public Vector2 ContainerSize;

		static NodeGlobalSceneState()
		{
			Il2CppClassPointerStore<NodeGlobalSceneState>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "NodeGlobalSceneState");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NodeGlobalSceneState>.NativeClassPtr);
			NativeFieldInfoPtr_ContainerSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeGlobalSceneState>.NativeClassPtr, "ContainerSize");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<NodeGlobalSceneState>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public class GradientExData : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_WorldRelative;

		private static readonly System.IntPtr NativeFieldInfoPtr_FillTransform;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe bool WorldRelative
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldRelative);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_WorldRelative)) = flag;
			}
		}

		public unsafe Matrix2D FillTransform
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FillTransform);
				return *(Matrix2D*)num;
			}
			set
			{
				*(Matrix2D*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_FillTransform)) = matrix2D;
			}
		}

		static GradientExData()
		{
			Il2CppClassPointerStore<GradientExData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "GradientExData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<GradientExData>.NativeClassPtr);
			NativeFieldInfoPtr_WorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientExData>.NativeClassPtr, "WorldRelative");
			NativeFieldInfoPtr_FillTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<GradientExData>.NativeClassPtr, "FillTransform");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<GradientExData>.NativeClassPtr, 100663454);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe GradientExData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<GradientExData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public GradientExData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class LinearGradientExData : GradientExData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_X1;

		private static readonly System.IntPtr NativeFieldInfoPtr_Y1;

		private static readonly System.IntPtr NativeFieldInfoPtr_X2;

		private static readonly System.IntPtr NativeFieldInfoPtr_Y2;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe string X1
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_X1);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_X1)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Y1
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Y1);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Y1)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string X2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_X2);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_X2)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Y2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Y2);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Y2)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static LinearGradientExData()
		{
			Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "LinearGradientExData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr);
			NativeFieldInfoPtr_X1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr, "X1");
			NativeFieldInfoPtr_Y1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr, "Y1");
			NativeFieldInfoPtr_X2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr, "X2");
			NativeFieldInfoPtr_Y2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr, "Y2");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr, 100663455);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe LinearGradientExData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<LinearGradientExData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public LinearGradientExData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public class RadialGradientExData : GradientExData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Parsed;

		private static readonly System.IntPtr NativeFieldInfoPtr_Cx;

		private static readonly System.IntPtr NativeFieldInfoPtr_Cy;

		private static readonly System.IntPtr NativeFieldInfoPtr_Fx;

		private static readonly System.IntPtr NativeFieldInfoPtr_Fy;

		private static readonly System.IntPtr NativeFieldInfoPtr_R;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe bool Parsed
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parsed);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Parsed)) = flag;
			}
		}

		public unsafe string Cx
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cx);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cx)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Cy
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cy);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Cy)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Fx
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fx);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fx)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Fy
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fy);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Fy)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string R
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_R);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_R)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static RadialGradientExData()
		{
			Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "RadialGradientExData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr);
			NativeFieldInfoPtr_Parsed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "Parsed");
			NativeFieldInfoPtr_Cx = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "Cx");
			NativeFieldInfoPtr_Cy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "Cy");
			NativeFieldInfoPtr_Fx = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "Fx");
			NativeFieldInfoPtr_Fy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "Fy");
			NativeFieldInfoPtr_R = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, "R");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr, 100663456);
		}

		[CallerCount(2575)]
		[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe RadialGradientExData()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RadialGradientExData>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public RadialGradientExData(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct ClipData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_WorldRelative;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public bool WorldRelative;

		static ClipData()
		{
			Il2CppClassPointerStore<ClipData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "ClipData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClipData>.NativeClassPtr);
			NativeFieldInfoPtr_WorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipData>.NativeClassPtr, "WorldRelative");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ClipData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct PatternData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_WorldRelative;

		private static readonly System.IntPtr NativeFieldInfoPtr_ContentWorldRelative;

		private static readonly System.IntPtr NativeFieldInfoPtr_PatternTransform;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public bool WorldRelative;

		[FieldOffset(1)]
		[MarshalAs(UnmanagedType.U1)]
		public bool ContentWorldRelative;

		[FieldOffset(4)]
		public Matrix2D PatternTransform;

		static PatternData()
		{
			Il2CppClassPointerStore<PatternData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "PatternData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PatternData>.NativeClassPtr);
			NativeFieldInfoPtr_WorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternData>.NativeClassPtr, "WorldRelative");
			NativeFieldInfoPtr_ContentWorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternData>.NativeClassPtr, "ContentWorldRelative");
			NativeFieldInfoPtr_PatternTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PatternData>.NativeClassPtr, "PatternTransform");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<PatternData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct MaskData
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_WorldRelative;

		private static readonly System.IntPtr NativeFieldInfoPtr_ContentWorldRelative;

		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.U1)]
		public bool WorldRelative;

		[FieldOffset(1)]
		[MarshalAs(UnmanagedType.U1)]
		public bool ContentWorldRelative;

		static MaskData()
		{
			Il2CppClassPointerStore<MaskData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "MaskData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaskData>.NativeClassPtr);
			NativeFieldInfoPtr_WorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaskData>.NativeClassPtr, "WorldRelative");
			NativeFieldInfoPtr_ContentWorldRelative = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaskData>.NativeClassPtr, "ContentWorldRelative");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<MaskData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	public sealed class NodeWithParent : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_node;

		private static readonly System.IntPtr NativeFieldInfoPtr_parent;

		public unsafe SceneNode node
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe SceneNode parent
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		static NodeWithParent()
		{
			Il2CppClassPointerStore<NodeWithParent>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "NodeWithParent");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NodeWithParent>.NativeClassPtr);
			NativeFieldInfoPtr_node = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeWithParent>.NativeClassPtr, "node");
			NativeFieldInfoPtr_parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeWithParent>.NativeClassPtr, "parent");
		}

		public NodeWithParent(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public NodeWithParent()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NodeWithParent>.NativeClassPtr))
		{
		}
	}

	public sealed class NodeReferenceData : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_node;

		private static readonly System.IntPtr NativeFieldInfoPtr_viewport;

		private static readonly System.IntPtr NativeFieldInfoPtr_id;

		public unsafe SceneNode node
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		public unsafe Rect viewport
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_viewport);
				return *(Rect*)num;
			}
			set
			{
				*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_viewport)) = rect;
			}
		}

		public unsafe string id
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_id)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static NodeReferenceData()
		{
			Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "NodeReferenceData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr);
			NativeFieldInfoPtr_node = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr, "node");
			NativeFieldInfoPtr_viewport = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr, "viewport");
			NativeFieldInfoPtr_id = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr, "id");
		}

		public NodeReferenceData(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public NodeReferenceData()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<NodeReferenceData>.NativeClassPtr))
		{
		}
	}

	public sealed class PostponedStopData : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_fill;

		public unsafe GradientFill fill
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fill);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GradientFill>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fill)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradientFill));
			}
		}

		static PostponedStopData()
		{
			Il2CppClassPointerStore<PostponedStopData>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "PostponedStopData");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PostponedStopData>.NativeClassPtr);
			NativeFieldInfoPtr_fill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PostponedStopData>.NativeClassPtr, "fill");
		}

		public PostponedStopData(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public PostponedStopData()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PostponedStopData>.NativeClassPtr))
		{
		}
	}

	public sealed class PostponedClip : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_node;

		public unsafe SceneNode node
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_node)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode));
			}
		}

		static PostponedClip()
		{
			Il2CppClassPointerStore<PostponedClip>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "PostponedClip");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PostponedClip>.NativeClassPtr);
			NativeFieldInfoPtr_node = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PostponedClip>.NativeClassPtr, "node");
		}

		public PostponedClip(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public PostponedClip()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PostponedClip>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_SVGLengthFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr_whiteSpaceNumberChars;

	private static readonly System.IntPtr NativeFieldInfoPtr_docReader;

	private static readonly System.IntPtr NativeFieldInfoPtr_scene;

	private static readonly System.IntPtr NativeFieldInfoPtr_dpiScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_windowWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr_windowHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr_scenePos;

	private static readonly System.IntPtr NativeFieldInfoPtr_sceneSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_svgObjects;

	private static readonly System.IntPtr NativeFieldInfoPtr_subTags;

	private static readonly System.IntPtr NativeFieldInfoPtr_gradientExInfo;

	private static readonly System.IntPtr NativeFieldInfoPtr_symbolViewBoxes;

	private static readonly System.IntPtr NativeFieldInfoPtr_nodeGlobalSceneState;

	private static readonly System.IntPtr NativeFieldInfoPtr_nodeOpacity;

	private static readonly System.IntPtr NativeFieldInfoPtr_nodeIDs;

	private static readonly System.IntPtr NativeFieldInfoPtr_nodeStyleLayers;

	private static readonly System.IntPtr NativeFieldInfoPtr_clipData;

	private static readonly System.IntPtr NativeFieldInfoPtr_patternData;

	private static readonly System.IntPtr NativeFieldInfoPtr_maskData;

	private static readonly System.IntPtr NativeFieldInfoPtr_postponedSymbolData;

	private static readonly System.IntPtr NativeFieldInfoPtr_postponedStopData;

	private static readonly System.IntPtr NativeFieldInfoPtr_postponedClip;

	private static readonly System.IntPtr NativeFieldInfoPtr_postponedFills;

	private static readonly System.IntPtr NativeFieldInfoPtr_invisibleNodes;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentContainerSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentViewBoxSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentSceneNode;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentGradientFill;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentGradientId;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentGradientLink;

	private static readonly System.IntPtr NativeFieldInfoPtr_allElems;

	private static readonly System.IntPtr NativeFieldInfoPtr_elemsToAddToHierarchy;

	private static readonly System.IntPtr NativeFieldInfoPtr_styles;

	private static readonly System.IntPtr NativeFieldInfoPtr_applyRootViewBox;

	private static readonly System.IntPtr NativeFieldInfoPtr_sceneViewport;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_XmlReader_Single_Scene_Int32_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Import_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NodeOpacities_Public_get_Dictionary_2_SceneNode_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StockBlackNonZeroFillName_Internal_Static_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StockBlackOddEvenFillName_Internal_Static_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseChildren_Private_Void_Node_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_circle_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_defs_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ellipse_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_g_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_image_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_line_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_linearGradient_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_path_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_polygon_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_polyline_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_radialGradient_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_clipPath_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_pattern_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_mask_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_rect_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_stop_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_svg_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_symbol_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_use_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_style_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResolveReferencedNode_Private_Void_SceneNode_NodeReferenceData_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneSceneNode_Private_SceneNode_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneShape_Private_Shape_Shape_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneContour_Private_BezierContour_BezierContour_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneFill_Private_IFill_IFill_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClonePathProps_Private_PathProperties_PathProperties_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CloneGradientFill_Private_GradientFill_GradientFill_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_DimType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_Single_DimType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AttribLengthVal_Private_Single_String_Node_String_Single_DimType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddToSVGDictionaryIfPossible_Private_Void_Node_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseViewport_Private_Rect_Node_SceneNode_Vector2_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseViewBox_Private_ViewBoxInfo_Node_SceneNode_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseViewBoxAspectRatio_Private_Void_Node_byref_ViewBoxInfo_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyViewBox_Private_Void_SceneNode_ViewBoxInfo_Rect_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseStrokeAttributeSet_Private_Stroke_Node_byref_PathCorner_byref_PathEnding_Inheritance_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseID_Private_Void_Node_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseOpacity_Private_Single_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseClipAndMask_Private_Void_Node_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseClip_Private_Void_Node_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyClipper_Private_Void_SceneNode_SceneNode_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseMask_Private_Void_Node_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DecodeTextureData_Private_Texture2D_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_PostProcess_Private_Void_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustFills_Private_Void_SceneNode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustGradientFill_Private_Void_SceneNode_Matrix2D_IFill_Il2CppReferenceArray_1_BezierContour_byref_Matrix2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustPatternFill_Private_SceneNode_SceneNode_Matrix2D_Shape_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveInvisibleNodes_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShouldDeclareSupportedChildren_Private_Boolean_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SupportElems_Private_Void_Node_Il2CppReferenceArray_1_ElemHandler_0;

	public unsafe static float SVGLengthFactor
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_SVGLengthFactor, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_SVGLengthFactor, (void*)(&num));
		}
	}

	public unsafe static Il2CppStructArray<char> whiteSpaceNumberChars
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_whiteSpaceNumberChars, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<char>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_whiteSpaceNumberChars, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe XmlReaderIterator docReader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_docReader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<XmlReaderIterator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_docReader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)xmlReaderIterator));
		}
	}

	public unsafe Scene scene
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scene);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Scene>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scene)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scene));
		}
	}

	public unsafe float dpiScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dpiScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dpiScale)) = num;
		}
	}

	public unsafe int windowWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_windowWidth);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_windowWidth)) = num;
		}
	}

	public unsafe int windowHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_windowHeight);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_windowHeight)) = num;
		}
	}

	public unsafe Vector2 scenePos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scenePos);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scenePos)) = vector;
		}
	}

	public unsafe Vector2 sceneSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sceneSize);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sceneSize)) = vector;
		}
	}

	public unsafe SVGDictionary svgObjects
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_svgObjects);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGDictionary>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_svgObjects)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sVGDictionary));
		}
	}

	public unsafe Dictionary<string, Handlers> subTags
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_subTags);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, Handlers>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_subTags)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<GradientFill, GradientExData> gradientExInfo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gradientExInfo);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<GradientFill, GradientExData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_gradientExInfo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, ViewBoxInfo> symbolViewBoxes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_symbolViewBoxes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, ViewBoxInfo>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_symbolViewBoxes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, NodeGlobalSceneState> nodeGlobalSceneState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeGlobalSceneState);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, NodeGlobalSceneState>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeGlobalSceneState)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, float> nodeOpacity
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeOpacity);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeOpacity)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<string, SceneNode> nodeIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeIDs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, SceneNode>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, SVGStyleResolver.StyleLayer> nodeStyleLayers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeStyleLayers);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, SVGStyleResolver.StyleLayer>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nodeStyleLayers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, ClipData> clipData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, ClipData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, PatternData> patternData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, PatternData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_patternData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SceneNode, MaskData> maskData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, MaskData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_maskData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<string, List<NodeReferenceData>> postponedSymbolData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedSymbolData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, List<NodeReferenceData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedSymbolData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<string, List<PostponedStopData>> postponedStopData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedStopData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, List<PostponedStopData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedStopData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<string, List<PostponedClip>> postponedClip
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedClip);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, List<PostponedClip>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedClip)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe SVGPostponedFills postponedFills
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedFills);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGPostponedFills>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_postponedFills)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sVGPostponedFills));
		}
	}

	public unsafe List<NodeWithParent> invisibleNodes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_invisibleNodes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<NodeWithParent>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_invisibleNodes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Stack<Vector2> currentContainerSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentContainerSize);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentContainerSize)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
		}
	}

	public unsafe Stack<Vector2> currentViewBoxSize
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentViewBoxSize);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<Vector2>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentViewBoxSize)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
		}
	}

	public unsafe Stack<SceneNode> currentSceneNode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSceneNode);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stack<SceneNode>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentSceneNode)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stack));
		}
	}

	public unsafe GradientFill currentGradientFill
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientFill);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GradientFill>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientFill)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gradientFill));
		}
	}

	public unsafe string currentGradientId
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientId);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientId)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string currentGradientLink
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientLink);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentGradientLink)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe Il2CppReferenceArray<ElemHandler> allElems
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allElems);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ElemHandler>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_allElems)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe HashSet<ElemHandler> elemsToAddToHierarchy
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elemsToAddToHierarchy);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<ElemHandler>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_elemsToAddToHierarchy)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe SVGStyleResolver styles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_styles);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGStyleResolver>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_styles)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sVGStyleResolver));
		}
	}

	public unsafe bool applyRootViewBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_applyRootViewBox);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_applyRootViewBox)) = flag;
		}
	}

	public unsafe Rect sceneViewport
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sceneViewport);
			return *(Rect*)num;
		}
		set
		{
			*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_sceneViewport)) = rect;
		}
	}

	public unsafe Dictionary<SceneNode, float> NodeOpacities
	{
		[CallerCount(16)]
		[CachedScanResults(RefRangeStart = 21999, RefRangeEnd = 22015, XrefRangeStart = 21999, XrefRangeEnd = 21999, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NodeOpacities_Public_get_Dictionary_2_SceneNode_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<string, SceneNode> NodeIDs
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 22015, RefRangeEnd = 22016, XrefRangeStart = 22015, XrefRangeEnd = 22015, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, SceneNode>>(intPtr) : null;
		}
	}

	public unsafe static string StockBlackNonZeroFillName
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22016, XrefRangeEnd = 22018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StockBlackNonZeroFillName_Internal_Static_get_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	public unsafe static string StockBlackOddEvenFillName
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22018, XrefRangeEnd = 22020, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StockBlackOddEvenFillName_Internal_Static_get_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static SVGDocument()
	{
		Il2CppClassPointerStore<SVGDocument>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGDocument");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr);
		NativeFieldInfoPtr_SVGLengthFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "SVGLengthFactor");
		NativeFieldInfoPtr_whiteSpaceNumberChars = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "whiteSpaceNumberChars");
		NativeFieldInfoPtr_docReader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "docReader");
		NativeFieldInfoPtr_scene = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "scene");
		NativeFieldInfoPtr_dpiScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "dpiScale");
		NativeFieldInfoPtr_windowWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "windowWidth");
		NativeFieldInfoPtr_windowHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "windowHeight");
		NativeFieldInfoPtr_scenePos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "scenePos");
		NativeFieldInfoPtr_sceneSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "sceneSize");
		NativeFieldInfoPtr_svgObjects = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "svgObjects");
		NativeFieldInfoPtr_subTags = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "subTags");
		NativeFieldInfoPtr_gradientExInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "gradientExInfo");
		NativeFieldInfoPtr_symbolViewBoxes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "symbolViewBoxes");
		NativeFieldInfoPtr_nodeGlobalSceneState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "nodeGlobalSceneState");
		NativeFieldInfoPtr_nodeOpacity = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "nodeOpacity");
		NativeFieldInfoPtr_nodeIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "nodeIDs");
		NativeFieldInfoPtr_nodeStyleLayers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "nodeStyleLayers");
		NativeFieldInfoPtr_clipData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "clipData");
		NativeFieldInfoPtr_patternData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "patternData");
		NativeFieldInfoPtr_maskData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "maskData");
		NativeFieldInfoPtr_postponedSymbolData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "postponedSymbolData");
		NativeFieldInfoPtr_postponedStopData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "postponedStopData");
		NativeFieldInfoPtr_postponedClip = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "postponedClip");
		NativeFieldInfoPtr_postponedFills = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "postponedFills");
		NativeFieldInfoPtr_invisibleNodes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "invisibleNodes");
		NativeFieldInfoPtr_currentContainerSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentContainerSize");
		NativeFieldInfoPtr_currentViewBoxSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentViewBoxSize");
		NativeFieldInfoPtr_currentSceneNode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentSceneNode");
		NativeFieldInfoPtr_currentGradientFill = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentGradientFill");
		NativeFieldInfoPtr_currentGradientId = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentGradientId");
		NativeFieldInfoPtr_currentGradientLink = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "currentGradientLink");
		NativeFieldInfoPtr_allElems = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "allElems");
		NativeFieldInfoPtr_elemsToAddToHierarchy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "elemsToAddToHierarchy");
		NativeFieldInfoPtr_styles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "styles");
		NativeFieldInfoPtr_applyRootViewBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "applyRootViewBox");
		NativeFieldInfoPtr_sceneViewport = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, "sceneViewport");
		NativeMethodInfoPtr__ctor_Public_Void_XmlReader_Single_Scene_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663387);
		NativeMethodInfoPtr_Import_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663388);
		NativeMethodInfoPtr_get_NodeOpacities_Public_get_Dictionary_2_SceneNode_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663389);
		NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663390);
		NativeMethodInfoPtr_get_StockBlackNonZeroFillName_Internal_Static_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663391);
		NativeMethodInfoPtr_get_StockBlackOddEvenFillName_Internal_Static_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663392);
		NativeMethodInfoPtr_ParseChildren_Private_Void_Node_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663393);
		NativeMethodInfoPtr_circle_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663394);
		NativeMethodInfoPtr_defs_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663395);
		NativeMethodInfoPtr_ellipse_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663396);
		NativeMethodInfoPtr_g_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663397);
		NativeMethodInfoPtr_image_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663398);
		NativeMethodInfoPtr_line_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663399);
		NativeMethodInfoPtr_linearGradient_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663400);
		NativeMethodInfoPtr_path_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663401);
		NativeMethodInfoPtr_polygon_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663402);
		NativeMethodInfoPtr_polyline_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663403);
		NativeMethodInfoPtr_radialGradient_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663404);
		NativeMethodInfoPtr_clipPath_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663405);
		NativeMethodInfoPtr_pattern_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663406);
		NativeMethodInfoPtr_mask_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663407);
		NativeMethodInfoPtr_rect_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663408);
		NativeMethodInfoPtr_stop_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663409);
		NativeMethodInfoPtr_svg_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663410);
		NativeMethodInfoPtr_symbol_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663411);
		NativeMethodInfoPtr_use_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663412);
		NativeMethodInfoPtr_style_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663413);
		NativeMethodInfoPtr_ResolveReferencedNode_Private_Void_SceneNode_NodeReferenceData_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663414);
		NativeMethodInfoPtr_CloneSceneNode_Private_SceneNode_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663415);
		NativeMethodInfoPtr_CloneShape_Private_Shape_Shape_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663416);
		NativeMethodInfoPtr_CloneContour_Private_BezierContour_BezierContour_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663417);
		NativeMethodInfoPtr_CloneFill_Private_IFill_IFill_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663418);
		NativeMethodInfoPtr_ClonePathProps_Private_PathProperties_PathProperties_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663419);
		NativeMethodInfoPtr_CloneGradientFill_Private_GradientFill_GradientFill_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663420);
		NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663421);
		NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663422);
		NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663423);
		NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663424);
		NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_DimType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663425);
		NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_Single_DimType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663426);
		NativeMethodInfoPtr_AttribLengthVal_Private_Single_String_Node_String_Single_DimType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663427);
		NativeMethodInfoPtr_AddToSVGDictionaryIfPossible_Private_Void_Node_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663428);
		NativeMethodInfoPtr_ParseViewport_Private_Rect_Node_SceneNode_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663429);
		NativeMethodInfoPtr_ParseViewBox_Private_ViewBoxInfo_Node_SceneNode_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663430);
		NativeMethodInfoPtr_ParseViewBoxAspectRatio_Private_Void_Node_byref_ViewBoxInfo_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663431);
		NativeMethodInfoPtr_ApplyViewBox_Private_Void_SceneNode_ViewBoxInfo_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663432);
		NativeMethodInfoPtr_ParseStrokeAttributeSet_Private_Stroke_Node_byref_PathCorner_byref_PathEnding_Inheritance_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663433);
		NativeMethodInfoPtr_ParseID_Private_Void_Node_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663434);
		NativeMethodInfoPtr_ParseOpacity_Private_Single_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663435);
		NativeMethodInfoPtr_ParseClipAndMask_Private_Void_Node_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663436);
		NativeMethodInfoPtr_ParseClip_Private_Void_Node_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663437);
		NativeMethodInfoPtr_ApplyClipper_Private_Void_SceneNode_SceneNode_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663438);
		NativeMethodInfoPtr_ParseMask_Private_Void_Node_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663439);
		NativeMethodInfoPtr_DecodeTextureData_Private_Texture2D_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663440);
		NativeMethodInfoPtr_PostProcess_Private_Void_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663441);
		NativeMethodInfoPtr_AdjustFills_Private_Void_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663442);
		NativeMethodInfoPtr_AdjustGradientFill_Private_Void_SceneNode_Matrix2D_IFill_Il2CppReferenceArray_1_BezierContour_byref_Matrix2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663443);
		NativeMethodInfoPtr_AdjustPatternFill_Private_SceneNode_SceneNode_Matrix2D_Shape_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663444);
		NativeMethodInfoPtr_RemoveInvisibleNodes_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663445);
		NativeMethodInfoPtr_ShouldDeclareSupportedChildren_Private_Boolean_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663446);
		NativeMethodInfoPtr_SupportElems_Private_Void_Node_Il2CppReferenceArray_1_ElemHandler_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr, 100663447);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 21970, RefRangeEnd = 21971, XrefRangeStart = 21551, XrefRangeEnd = 21970, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGDocument(XmlReader docReader, float dpi, Scene scene, int windowWidth, int windowHeight, bool applyRootViewBox)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGDocument>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)docReader);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dpi;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scene);
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowWidth;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowHeight;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &applyRootViewBox;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_XmlReader_Single_Scene_Int32_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 21998, RefRangeEnd = 21999, XrefRangeStart = 21971, XrefRangeEnd = 21998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Import()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Import_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 22079, RefRangeEnd = 22086, XrefRangeStart = 22020, XrefRangeEnd = 22079, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseChildren(XmlReaderIterator.Node node, string nodeName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(nodeName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseChildren_Private_Void_Node_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22086, XrefRangeEnd = 22152, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void circle()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_circle_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22152, XrefRangeEnd = 22183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void defs()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_defs_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22183, XrefRangeEnd = 22253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ellipse()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ellipse_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22253, XrefRangeEnd = 22279, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void g()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_g_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22279, XrefRangeEnd = 22420, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void image()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_image_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22420, XrefRangeEnd = 22495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void line()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_line_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22495, XrefRangeEnd = 22620, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void linearGradient()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_linearGradient_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22620, XrefRangeEnd = 22669, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void path()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_path_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22669, XrefRangeEnd = 22764, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void polygon()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_polygon_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22764, XrefRangeEnd = 22859, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void polyline()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_polyline_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22859, XrefRangeEnd = 22989, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void radialGradient()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_radialGradient_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 22989, XrefRangeEnd = 23047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void clipPath()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_clipPath_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23047, XrefRangeEnd = 23114, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void pattern()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_pattern_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23114, XrefRangeEnd = 23157, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void mask()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_mask_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23157, XrefRangeEnd = 23237, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void rect()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_rect_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23237, XrefRangeEnd = 23317, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void stop()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_stop_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 23368, RefRangeEnd = 23369, XrefRangeStart = 23317, XrefRangeEnd = 23368, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void svg()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_svg_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23369, XrefRangeEnd = 23443, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void symbol()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_symbol_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23443, XrefRangeEnd = 23509, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void use()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_use_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23509, XrefRangeEnd = 23523, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void style()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_style_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 23624, RefRangeEnd = 23627, XrefRangeStart = 23523, XrefRangeEnd = 23624, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResolveReferencedNode(SceneNode referencedNode, NodeReferenceData refData, bool isDeferred)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)referencedNode);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)refData));
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &isDeferred;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResolveReferencedNode_Private_Void_SceneNode_NodeReferenceData_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 23707, RefRangeEnd = 23710, XrefRangeStart = 23627, XrefRangeEnd = 23707, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SceneNode CloneSceneNode(SceneNode node)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneSceneNode_Private_SceneNode_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 23729, RefRangeEnd = 23730, XrefRangeStart = 23710, XrefRangeEnd = 23729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Shape CloneShape(Shape shape)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shape);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneShape_Private_Shape_Shape_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Shape>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23730, XrefRangeEnd = 23735, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BezierContour CloneContour(BezierContour c)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)c));
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneContour_Private_BezierContour_BezierContour_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new BezierContour(pointer);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 23745, RefRangeEnd = 23747, XrefRangeStart = 23735, XrefRangeEnd = 23745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IFill CloneFill(IFill fill)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fill);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneFill_Private_IFill_IFill_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IFill>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 23759, RefRangeEnd = 23760, XrefRangeStart = 23747, XrefRangeEnd = 23759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PathProperties ClonePathProps(PathProperties props)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)props));
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClonePathProps_Private_PathProperties_PathProperties_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new PathProperties(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23760, XrefRangeEnd = 23765, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GradientFill CloneGradientFill(GradientFill other)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)other);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneGradientFill_Private_GradientFill_GradientFill_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GradientFill>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23765, XrefRangeEnd = 23767, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int AttribIntVal(string attribName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attribName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23767, XrefRangeEnd = 23769, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int AttribIntVal(string attribName, int defaultVal)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultVal;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribIntVal_Private_Int32_String_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23769, XrefRangeEnd = 23770, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float AttribFloatVal(string attribName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attribName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(15)]
	[CachedScanResults(RefRangeStart = 23776, RefRangeEnd = 23791, XrefRangeStart = 23770, XrefRangeEnd = 23776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float AttribFloatVal(string attribName, float defaultVal)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultVal;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribFloatVal_Private_Single_String_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23791, XrefRangeEnd = 23793, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float AttribLengthVal(XmlReaderIterator.Node node, string attribName, DimType dimType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(DimType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &dimType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_DimType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 23793, XrefRangeEnd = 23795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float AttribLengthVal(XmlReaderIterator.Node node, string attribName, float defaultUnitVal, DimType dimType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultUnitVal;
		*(DimType**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &dimType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribLengthVal_Private_Single_Node_String_Single_DimType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(61)]
	[CachedScanResults(RefRangeStart = 23828, RefRangeEnd = 23889, XrefRangeStart = 23795, XrefRangeEnd = 23828, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float AttribLengthVal(string val, XmlReaderIterator.Node node, string attribName, float defaultUnitVal, DimType dimType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(val);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attribName);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultUnitVal;
		*(DimType**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &dimType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AttribLengthVal_Private_Single_String_Node_String_Single_DimType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 23895, RefRangeEnd = 23900, XrefRangeStart = 23889, XrefRangeEnd = 23895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddToSVGDictionaryIfPossible(XmlReaderIterator.Node node, Il2CppSystem.Object vectorElement)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vectorElement);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddToSVGDictionaryIfPossible_Private_Void_Node_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 23916, RefRangeEnd = 23919, XrefRangeStart = 23900, XrefRangeEnd = 23916, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Rect ParseViewport(XmlReaderIterator.Node node, SceneNode sceneNode, Vector2 defaultViewportSize)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		*(Vector2**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &defaultViewportSize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseViewport_Private_Rect_Node_SceneNode_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 23937, RefRangeEnd = 23939, XrefRangeStart = 23919, XrefRangeEnd = 23937, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ViewBoxInfo ParseViewBox(XmlReaderIterator.Node node, SceneNode sceneNode, Rect sceneViewport)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		*(Rect**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sceneViewport;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseViewBox_Private_ViewBoxInfo_Node_SceneNode_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ViewBoxInfo*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 24001, RefRangeEnd = 24003, XrefRangeStart = 23939, XrefRangeEnd = 24001, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseViewBoxAspectRatio(XmlReaderIterator.Node node, ref ViewBoxInfo viewBoxInfo)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref viewBoxInfo);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseViewBoxAspectRatio_Private_Void_Node_byref_ViewBoxInfo_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 24017, RefRangeEnd = 24020, XrefRangeStart = 24003, XrefRangeEnd = 24017, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyViewBox(SceneNode sceneNode, ViewBoxInfo viewBoxInfo, Rect sceneViewport)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		*(ViewBoxInfo**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &viewBoxInfo;
		*(Rect**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &sceneViewport;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyViewBox_Private_Void_SceneNode_ViewBoxInfo_Rect_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 24082, RefRangeEnd = 24090, XrefRangeStart = 24020, XrefRangeEnd = 24082, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Stroke ParseStrokeAttributeSet(XmlReaderIterator.Node node, out PathCorner strokeCorner, out PathEnding strokeEnding, Inheritance inheritance = Inheritance.Inherited)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref strokeCorner);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref strokeEnding);
		*(Inheritance**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &inheritance;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseStrokeAttributeSet_Private_Stroke_Node_byref_PathCorner_byref_PathEnding_Inheritance_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Stroke>(intPtr) : null;
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 24104, RefRangeEnd = 24115, XrefRangeStart = 24090, XrefRangeEnd = 24104, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseID(XmlReaderIterator.Node node, SceneNode sceneNode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseID_Private_Void_Node_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 24121, RefRangeEnd = 24122, XrefRangeStart = 24115, XrefRangeEnd = 24121, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe float ParseOpacity(SceneNode sceneNode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseOpacity_Private_Single_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 24122, XrefRangeEnd = 24124, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseClipAndMask(XmlReaderIterator.Node node, SceneNode sceneNode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseClipAndMask_Private_Void_Node_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 24161, RefRangeEnd = 24173, XrefRangeStart = 24124, XrefRangeEnd = 24161, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseClip(XmlReaderIterator.Node node, SceneNode sceneNode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseClip_Private_Void_Node_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 24197, RefRangeEnd = 24198, XrefRangeStart = 24173, XrefRangeEnd = 24197, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyClipper(SceneNode clipper, SceneNode target, bool worldRelative)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clipper);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)target);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &worldRelative;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyClipper_Private_Void_SceneNode_SceneNode_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 24236, RefRangeEnd = 24248, XrefRangeStart = 24198, XrefRangeEnd = 24236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ParseMask(XmlReaderIterator.Node node, SceneNode sceneNode)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sceneNode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseMask_Private_Void_Node_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 24248, XrefRangeEnd = 24268, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Texture2D DecodeTextureData(string dataURI)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(dataURI);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DecodeTextureData_Private_Texture2D_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 24268, XrefRangeEnd = 24269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void PostProcess(SceneNode root)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_PostProcess_Private_Void_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 24377, RefRangeEnd = 24380, XrefRangeStart = 24269, XrefRangeEnd = 24377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdjustFills(SceneNode root)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)root);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustFills_Private_Void_SceneNode_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 24478, RefRangeEnd = 24480, XrefRangeStart = 24380, XrefRangeEnd = 24478, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdjustGradientFill(SceneNode node, Matrix2D worldTransform, IFill fill, Il2CppReferenceArray<BezierContour> contours, ref Matrix2D computedTransform)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &worldTransform;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)fill);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)contours);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref computedTransform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustGradientFill_Private_Void_SceneNode_Matrix2D_IFill_Il2CppReferenceArray_1_BezierContour_byref_Matrix2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 24607, RefRangeEnd = 24608, XrefRangeStart = 24480, XrefRangeEnd = 24607, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SceneNode AdjustPatternFill(SceneNode node, Matrix2D worldTransform, Shape shape)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(Matrix2D**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &worldTransform;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shape);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustPatternFill_Private_SceneNode_SceneNode_Matrix2D_Shape_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SceneNode>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 24625, RefRangeEnd = 24626, XrefRangeStart = 24608, XrefRangeEnd = 24625, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveInvisibleNodes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveInvisibleNodes_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 24626, XrefRangeEnd = 24629, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool ShouldDeclareSupportedChildren(XmlReaderIterator.Node node)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShouldDeclareSupportedChildren_Private_Boolean_Node_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(20)]
	[CachedScanResults(RefRangeStart = 24653, RefRangeEnd = 24673, XrefRangeStart = 24629, XrefRangeEnd = 24653, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SupportElems(XmlReaderIterator.Node node, [Optional] Il2CppReferenceArray<ElemHandler> handlers)
	{
		if (handlers == null)
		{
			handlers = new Il2CppReferenceArray<ElemHandler>(0L);
		}
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)handlers);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SupportElems_Private_Void_Node_Il2CppReferenceArray_1_ElemHandler_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public void SupportElems(XmlReaderIterator.Node node, params ElemHandler[] handlers)
	{
		SupportElems(node, new Il2CppReferenceArray<ElemHandler>(handlers));
	}

	public SVGDocument(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
