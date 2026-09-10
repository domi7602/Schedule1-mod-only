using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppFluffyUnderware.Curvy.ThirdParty.LibTessDotNet;

public sealed class CombineCallback : Il2CppSystem.MulticastDelegate
{
	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Object_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_AsyncCallback_Object_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Object_IAsyncResult_0;

	static CombineCallback()
	{
		Il2CppClassPointerStore<CombineCallback>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp-firstpass.dll", "FluffyUnderware.Curvy.ThirdParty.LibTessDotNet", "CombineCallback");
		NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CombineCallback>.NativeClassPtr, 100667240);
		NativeMethodInfoPtr_Invoke_Public_Virtual_New_Object_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CombineCallback>.NativeClassPtr, 100667241);
		NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CombineCallback>.NativeClassPtr, 100667242);
		NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Object_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CombineCallback>.NativeClassPtr, 100667243);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 60673, RefRangeEnd = 60674, XrefRangeStart = 60670, XrefRangeEnd = 60673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CombineCallback(Il2CppSystem.Object @object, System.IntPtr method)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CombineCallback>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual Il2CppSystem.Object Invoke(Vec3 position, Il2CppReferenceArray<Il2CppSystem.Object> data, Il2CppStructArray<float> weights)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&position);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)weights);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Object_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 60674, XrefRangeEnd = 60678, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(Vec3 position, Il2CppReferenceArray<Il2CppSystem.Object> data, Il2CppStructArray<float> weights, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&position);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)weights);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Vec3_Il2CppReferenceArray_1_Object_Il2CppStructArray_1_Single_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual Il2CppSystem.Object EndInvoke(Il2CppSystem.IAsyncResult result)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Object_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
	}

	public CombineCallback(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public static implicit operator CombineCallback(System.Func<Vec3, Il2CppReferenceArray<Il2CppSystem.Object>, Il2CppStructArray<float>, Il2CppSystem.Object> P_0)
	{
		return DelegateSupport.ConvertDelegate<CombineCallback>((System.Delegate)P_0);
	}

	public static CombineCallback operator +(CombineCallback P_0, CombineCallback P_1)
	{
		return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<CombineCallback>();
	}

	public static CombineCallback operator -(CombineCallback P_0, CombineCallback P_1)
	{
		object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
		if (obj != null)
		{
			obj = ((Il2CppObjectBase)obj).Cast<CombineCallback>();
		}
		return (CombineCallback)obj;
	}
}
