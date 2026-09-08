using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Xml;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class XmlReaderIterator : Il2CppSystem.Object
{
	public class Node : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_reader;

		private static readonly System.IntPtr NativeFieldInfoPtr_depth;

		private static readonly System.IntPtr NativeFieldInfoPtr_name;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Item_Public_get_String_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAttributes_Public_SVGPropertySheet_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetException_Public_SVGFormatException_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetUnsupportedAttribValException_Public_SVGFormatException_String_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_get_Depth_Public_get_Int32_0;

		public unsafe XmlReader reader
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reader);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<XmlReader>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)xmlReader));
			}
		}

		public unsafe int depth
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depth);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_depth)) = num;
			}
		}

		public unsafe string name
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string Name
		{
			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 2958, RefRangeEnd = 2959, XrefRangeStart = 2958, XrefRangeEnd = 2959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
		}

		public unsafe string this[string attrib]
		{
			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19728, XrefRangeEnd = 19729, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.ManagedStringToIl2Cpp(attrib);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Item_Public_get_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return IL2CPP.Il2CppStringToManaged(intPtr);
			}
		}

		public unsafe int Depth
		{
			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 3891, RefRangeEnd = 3894, XrefRangeStart = 3891, XrefRangeEnd = 3894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Depth_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}
		}

		static Node()
		{
			Il2CppClassPointerStore<Node>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, "Node");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Node>.NativeClassPtr);
			NativeFieldInfoPtr_reader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "reader");
			NativeFieldInfoPtr_depth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "depth");
			NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Node>.NativeClassPtr, "name");
			NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663373);
			NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663374);
			NativeMethodInfoPtr_get_Item_Public_get_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663375);
			NativeMethodInfoPtr_GetAttributes_Public_SVGPropertySheet_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663376);
			NativeMethodInfoPtr_GetException_Public_SVGFormatException_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663377);
			NativeMethodInfoPtr_GetUnsupportedAttribValException_Public_SVGFormatException_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663378);
			NativeMethodInfoPtr_get_Depth_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Node>.NativeClassPtr, 100663379);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19725, XrefRangeEnd = 19728, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe Node(XmlReader reader)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Node>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19729, XrefRangeEnd = 19737, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SVGPropertySheet GetAttributes()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAttributes_Public_SVGPropertySheet_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGPropertySheet>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19737, XrefRangeEnd = 19745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SVGFormatException GetException(string message)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetException_Public_SVGFormatException_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGFormatException>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 19745, XrefRangeEnd = 19768, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SVGFormatException GetUnsupportedAttribValException(string attrib)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(attrib);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetUnsupportedAttribValException_Public_SVGFormatException_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGFormatException>(intPtr) : null;
		}

		public Node(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_reader;

	private static readonly System.IntPtr NativeFieldInfoPtr_currentElementVisited;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GoToRoot_Public_Boolean_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_VisitCurrent_Public_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsEmptyElement_Public_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GoToNextChild_Public_Boolean_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkipCurrentChildTree_Public_Void_Node_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadTextWithinElement_Public_String_0;

	public unsafe XmlReader reader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<XmlReader>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_reader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)xmlReader));
		}
	}

	public unsafe bool currentElementVisited
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentElementVisited);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentElementVisited)) = flag;
		}
	}

	static XmlReaderIterator()
	{
		Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "XmlReaderIterator");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr);
		NativeFieldInfoPtr_reader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, "reader");
		NativeFieldInfoPtr_currentElementVisited = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, "currentElementVisited");
		NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663366);
		NativeMethodInfoPtr_GoToRoot_Public_Boolean_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663367);
		NativeMethodInfoPtr_VisitCurrent_Public_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663368);
		NativeMethodInfoPtr_IsEmptyElement_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663369);
		NativeMethodInfoPtr_GoToNextChild_Public_Boolean_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663370);
		NativeMethodInfoPtr_SkipCurrentChildTree_Public_Void_Node_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663371);
		NativeMethodInfoPtr_ReadTextWithinElement_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr, 100663372);
	}

	[CallerCount(203)]
	[CachedScanResults(RefRangeStart = 19776, RefRangeEnd = 19979, XrefRangeStart = 19768, XrefRangeEnd = 19776, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe XmlReaderIterator(XmlReader reader)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<XmlReaderIterator>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_XmlReader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool GoToRoot(string tagName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(tagName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GoToRoot_Public_Boolean_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(22)]
	[CachedScanResults(RefRangeStart = 19985, RefRangeEnd = 20007, XrefRangeStart = 19979, XrefRangeEnd = 19985, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Node VisitCurrent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_VisitCurrent_Public_Node_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Node>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20007, XrefRangeEnd = 20008, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsEmptyElement()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsEmptyElement_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 20009, RefRangeEnd = 20012, XrefRangeStart = 20008, XrefRangeEnd = 20009, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool GoToNextChild(Node node)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GoToNextChild_Public_Boolean_Node_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 20016, RefRangeEnd = 20018, XrefRangeStart = 20012, XrefRangeEnd = 20016, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SkipCurrentChildTree(Node node)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)node);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkipCurrentChildTree_Public_Void_Node_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20018, XrefRangeEnd = 20022, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string ReadTextWithinElement()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadTextWithinElement_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public XmlReaderIterator(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
