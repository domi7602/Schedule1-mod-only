using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Xml;

namespace Il2CppToolBuddy.ThirdParty.VectorGraphics;

public class SVGFormatException : Il2CppSystem.Exception
{
	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_XmlReader_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_StackError_Public_Static_get_SVGFormatException_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComposeMessage_Private_Static_String_XmlReader_String_0;

	public unsafe static SVGFormatException StackError
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 20047, RefRangeEnd = 20050, XrefRangeStart = 20037, XrefRangeEnd = 20047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_StackError_Public_Static_get_SVGFormatException_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SVGFormatException>(intPtr) : null;
		}
	}

	static SVGFormatException()
	{
		Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "ToolBuddy.ThirdParty.VectorGraphics", "SVGFormatException");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr, 100663380);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr, 100663381);
		NativeMethodInfoPtr__ctor_Public_Void_XmlReader_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr, 100663382);
		NativeMethodInfoPtr_get_StackError_Public_Static_get_SVGFormatException_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr, 100663383);
		NativeMethodInfoPtr_ComposeMessage_Private_Static_String_XmlReader_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr, 100663384);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20022, XrefRangeEnd = 20026, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGFormatException()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 20031, RefRangeEnd = 20032, XrefRangeStart = 20026, XrefRangeEnd = 20031, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGFormatException(string message)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 20032, XrefRangeEnd = 20037, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SVGFormatException(XmlReader reader, string message)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SVGFormatException>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_XmlReader_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 20060, RefRangeEnd = 20065, XrefRangeStart = 20050, XrefRangeEnd = 20060, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static string ComposeMessage(XmlReader reader, string message)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComposeMessage_Private_Static_String_XmlReader_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public SVGFormatException(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
