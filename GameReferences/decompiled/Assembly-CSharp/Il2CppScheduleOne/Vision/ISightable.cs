using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Object;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppScheduleOne.Vision;

public class ISightable : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_get_NetworkObject_Public_Abstract_Virtual_New_get_NetworkObject_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_HighestProgressionEvent_Public_Abstract_Virtual_New_get_VisionEvent_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_HighestProgressionEvent_Public_Abstract_Virtual_New_set_Void_VisionEvent_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_VisibilityComponent_Public_Abstract_Virtual_New_get_EntityVisibility_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsCurrentlySightable_Public_Abstract_Virtual_New_Boolean_0;

	public unsafe virtual NetworkObject NetworkObject
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_NetworkObject_Public_Abstract_Virtual_New_get_NetworkObject_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<NetworkObject>(intPtr) : null;
		}
	}

	public unsafe virtual VisionEvent HighestProgressionEvent
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_HighestProgressionEvent_Public_Abstract_Virtual_New_get_VisionEvent_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VisionEvent>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_set_HighestProgressionEvent_Public_Abstract_Virtual_New_set_Void_VisionEvent_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe virtual EntityVisibility VisibilityComponent
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_VisibilityComponent_Public_Abstract_Virtual_New_get_EntityVisibility_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EntityVisibility>(intPtr) : null;
		}
	}

	static ISightable()
	{
		Il2CppClassPointerStore<ISightable>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Vision", "ISightable");
		NativeMethodInfoPtr_get_NetworkObject_Public_Abstract_Virtual_New_get_NetworkObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISightable>.NativeClassPtr, 100668582);
		NativeMethodInfoPtr_get_HighestProgressionEvent_Public_Abstract_Virtual_New_get_VisionEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISightable>.NativeClassPtr, 100668583);
		NativeMethodInfoPtr_set_HighestProgressionEvent_Public_Abstract_Virtual_New_set_Void_VisionEvent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISightable>.NativeClassPtr, 100668584);
		NativeMethodInfoPtr_get_VisibilityComponent_Public_Abstract_Virtual_New_get_EntityVisibility_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISightable>.NativeClassPtr, 100668585);
		NativeMethodInfoPtr_IsCurrentlySightable_Public_Abstract_Virtual_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ISightable>.NativeClassPtr, 100668586);
	}

	[CallerCount(0)]
	public unsafe virtual bool IsCurrentlySightable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_IsCurrentlySightable_Public_Abstract_Virtual_New_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public ISightable(IntPtr pointer)
		: base(pointer)
	{
	}
}
