using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using UnityEngine;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGParser : Il2CppSystem.Object
{
	public sealed class SceneInfo : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr__Scene_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__SceneViewport_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__NodeOpacity_k__BackingField;

		private static readonly System.IntPtr NativeFieldInfoPtr__NodeIDs_k__BackingField;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Internal_Void_Scene_Rect_Dictionary_2_SceneNode_Single_Dictionary_2_String_SceneNode_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Scene_Public_get_Scene_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_SceneViewport_Public_get_Rect_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_NodeOpacity_Public_get_Dictionary_2_SceneNode_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0;

		public unsafe Scene _Scene_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Scene_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Scene>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Scene_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scene));
			}
		}

		public unsafe Rect _SceneViewport_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SceneViewport_k__BackingField);
				return *(Rect*)num;
			}
			set
			{
				*(Rect*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SceneViewport_k__BackingField)) = rect;
			}
		}

		public unsafe Dictionary<SceneNode, float> _NodeOpacity_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NodeOpacity_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NodeOpacity_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe Dictionary<string, SceneNode> _NodeIDs_k__BackingField
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NodeIDs_k__BackingField);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, SceneNode>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NodeIDs_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe Scene Scene
		{
			[CallerCount(155)]
			[CachedScanResults(RefRangeStart = 19464, RefRangeEnd = 19619, XrefRangeStart = 19464, XrefRangeEnd = 19464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Scene_Public_get_Scene_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Scene>(intPtr) : null;
			}
		}

		public unsafe Rect SceneViewport
		{
			[CallerCount(0)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SceneViewport_Public_get_Rect_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(Rect*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		public unsafe Dictionary<SceneNode, float> NodeOpacity
		{
			[CallerCount(24)]
			[CachedScanResults(RefRangeStart = 19619, RefRangeEnd = 19643, XrefRangeStart = 19619, XrefRangeEnd = 19619, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NodeOpacity_Public_get_Dictionary_2_SceneNode_Single_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SceneNode, float>>(intPtr) : null;
			}
		}

		public unsafe Dictionary<string, SceneNode> NodeIDs
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<string, SceneNode>>(intPtr) : null;
			}
		}

		static SceneInfo()
		{
			Il2CppClassPointerStore<SceneInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SVGParser>.NativeClassPtr, "SceneInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr);
			NativeFieldInfoPtr__Scene_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, "<Scene>k__BackingField");
			NativeFieldInfoPtr__SceneViewport_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, "<SceneViewport>k__BackingField");
			NativeFieldInfoPtr__NodeOpacity_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, "<NodeOpacity>k__BackingField");
			NativeFieldInfoPtr__NodeIDs_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, "<NodeIDs>k__BackingField");
			NativeMethodInfoPtr__ctor_Internal_Void_Scene_Rect_Dictionary_2_SceneNode_Single_Dictionary_2_String_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, 100663361);
			NativeMethodInfoPtr_get_Scene_Public_get_Scene_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, 100663362);
			NativeMethodInfoPtr_get_SceneViewport_Public_get_Rect_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, 100663363);
			NativeMethodInfoPtr_get_NodeOpacity_Public_get_Dictionary_2_SceneNode_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, 100663364);
			NativeMethodInfoPtr_get_NodeIDs_Public_get_Dictionary_2_String_SceneNode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr, 100663365);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19461, XrefRangeEnd = 19464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SceneInfo(Scene scene, Rect sceneViewport, Dictionary<SceneNode, float> nodeOpacities, Dictionary<string, SceneNode> nodeIDs)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)scene);
			*(Rect**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &sceneViewport;
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nodeOpacities);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nodeIDs);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Internal_Void_Scene_Rect_Dictionary_2_SceneNode_Single_Dictionary_2_String_SceneNode_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SceneInfo(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public SceneInfo()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SceneInfo>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_Single_Single_Int32_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_ViewportOptions_Single_Single_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	static SVGParser()
	{
		Il2CppClassPointerStore<SVGParser>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGParser");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGParser>.NativeClassPtr);
		NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_Single_Single_Int32_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGParser>.NativeClassPtr, 100663358);
		NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_ViewportOptions_Single_Single_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGParser>.NativeClassPtr, 100663359);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGParser>.NativeClassPtr, 100663360);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 19644, RefRangeEnd = 19645, XrefRangeStart = 19643, XrefRangeEnd = 19644, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SceneInfo ImportSVG(TextReader textReader, float dpi = 0f, float pixelsPerUnit = 1f, int windowWidth = 0, int windowHeight = 0, bool clipViewport = false)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textReader);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dpi;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &pixelsPerUnit;
		*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowWidth;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowHeight;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &clipViewport;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_Single_Single_Int32_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SceneInfo(pointer);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 19724, RefRangeEnd = 19725, XrefRangeStart = 19645, XrefRangeEnd = 19724, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static SceneInfo ImportSVG(TextReader textReader, ViewportOptions viewportOptions, float dpi = 0f, float pixelsPerUnit = 1f, int windowWidth = 0, int windowHeight = 0)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textReader);
		*(ViewportOptions**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &viewportOptions;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &dpi;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &pixelsPerUnit;
		*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowWidth;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &windowHeight;
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ImportSVG_Public_Static_SceneInfo_TextReader_ViewportOptions_Single_Single_Int32_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new SceneInfo(pointer);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGParser()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGParser>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SVGParser(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
