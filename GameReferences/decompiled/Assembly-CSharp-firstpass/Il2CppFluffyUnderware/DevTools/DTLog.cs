using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppFluffyUnderware.DevTools;

public static class DTLog : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_Log_Public_Static_Void_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Log_Public_Static_Void_Object_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogError_Public_Static_Void_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogError_Public_Static_Void_Object_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0;

	static DTLog()
	{
		Il2CppClassPointerStore<DTLog>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.DevTools", "DTLog");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DTLog>.NativeClassPtr);
		NativeMethodInfoPtr_Log_Public_Static_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664023);
		NativeMethodInfoPtr_Log_Public_Static_Void_Object_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664024);
		NativeMethodInfoPtr_LogError_Public_Static_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664025);
		NativeMethodInfoPtr_LogError_Public_Static_Void_Object_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664026);
		NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664027);
		NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664028);
		NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664029);
		NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664030);
		NativeMethodInfoPtr_LogFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664031);
		NativeMethodInfoPtr_LogFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664032);
		NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664033);
		NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664034);
		NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664035);
		NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DTLog>.NativeClassPtr, 100664036);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31832, XrefRangeEnd = 31836, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Log(Il2CppSystem.Object message)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Log_Public_Static_Void_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 31840, RefRangeEnd = 31843, XrefRangeStart = 31836, XrefRangeEnd = 31840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void Log(Il2CppSystem.Object message, UnityEngine.Object context)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Log_Public_Static_Void_Object_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 31847, RefRangeEnd = 31857, XrefRangeStart = 31843, XrefRangeEnd = 31847, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogError(Il2CppSystem.Object message)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogError_Public_Static_Void_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 31861, RefRangeEnd = 31877, XrefRangeStart = 31857, XrefRangeEnd = 31861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogError(Il2CppSystem.Object message, UnityEngine.Object context)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogError_Public_Static_Void_Object_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31877, XrefRangeEnd = 31881, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogErrorFormat(string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 31885, RefRangeEnd = 31887, XrefRangeStart = 31881, XrefRangeEnd = 31885, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogErrorFormat(UnityEngine.Object context, string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogErrorFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31887, XrefRangeEnd = 31891, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogException(Il2CppSystem.Exception exception)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exception);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31891, XrefRangeEnd = 31895, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogException(Il2CppSystem.Exception exception, UnityEngine.Object context)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exception);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogException_Public_Static_Void_Exception_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31895, XrefRangeEnd = 31899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogFormat(string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31899, XrefRangeEnd = 31903, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogFormat(UnityEngine.Object context, string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 31907, RefRangeEnd = 31913, XrefRangeStart = 31903, XrefRangeEnd = 31907, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogWarning(Il2CppSystem.Object message)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 31917, RefRangeEnd = 31924, XrefRangeStart = 31913, XrefRangeEnd = 31917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogWarning(Il2CppSystem.Object message, UnityEngine.Object context)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)message);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogWarning_Public_Static_Void_Object_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31924, XrefRangeEnd = 31928, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogWarningFormat(string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 31928, XrefRangeEnd = 31932, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void LogWarningFormat(UnityEngine.Object context, string format, [Optional] Il2CppReferenceArray<Il2CppSystem.Object> args)
	{
		if (args == null)
		{
			args = new Il2CppReferenceArray<Il2CppSystem.Object>(0L);
		}
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)context);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(format);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)args);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LogWarningFormat_Public_Static_Void_Object_String_Il2CppReferenceArray_1_Object_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public static void LogErrorFormat(string format, params Il2CppSystem.Object[] args)
	{
		LogErrorFormat(format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public static void LogErrorFormat(UnityEngine.Object context, string format, params Il2CppSystem.Object[] args)
	{
		LogErrorFormat(context, format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public static void LogFormat(string format, params Il2CppSystem.Object[] args)
	{
		LogFormat(format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public static void LogFormat(UnityEngine.Object context, string format, params Il2CppSystem.Object[] args)
	{
		LogFormat(context, format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public static void LogWarningFormat(string format, params Il2CppSystem.Object[] args)
	{
		LogWarningFormat(format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public static void LogWarningFormat(UnityEngine.Object context, string format, params Il2CppSystem.Object[] args)
	{
		LogWarningFormat(context, format, new Il2CppReferenceArray<Il2CppSystem.Object>(args));
	}

	public DTLog(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
