using System;
using System.Runtime.CompilerServices;
using Il2CppFishNet.Object;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppScheduleOne.Combat;

[System.Serializable]
public class Impact : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_HitPoint;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactForceDirection;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactDamage;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactType;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactSource;

	private static readonly System.IntPtr NativeFieldInfoPtr_ImpactID;

	private static readonly System.IntPtr NativeFieldInfoPtr_ExplosionType;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsLethal_Public_Static_Boolean_EImpactType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsPlayerImpact_Public_Boolean_byref_Player_0;

	public unsafe Vector3 HitPoint
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HitPoint);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_HitPoint)) = vector;
		}
	}

	public unsafe Vector3 ImpactForceDirection
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactForceDirection);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactForceDirection)) = vector;
		}
	}

	public unsafe float ImpactForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactForce);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactForce)) = num;
		}
	}

	public unsafe float ImpactDamage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactDamage);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactDamage)) = num;
		}
	}

	public unsafe EImpactType ImpactType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactType);
			return *(EImpactType*)num;
		}
		set
		{
			*(EImpactType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactType)) = eImpactType;
		}
	}

	public unsafe NetworkObject ImpactSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactSource);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<NetworkObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactSource)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)networkObject));
		}
	}

	public unsafe int ImpactID
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactID);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ImpactID)) = num;
		}
	}

	public unsafe EExplosionType ExplosionType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionType);
			return *(EExplosionType*)num;
		}
		set
		{
			*(EExplosionType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_ExplosionType)) = eExplosionType;
		}
	}

	static Impact()
	{
		Il2CppClassPointerStore<Impact>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Combat", "Impact");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Impact>.NativeClassPtr);
		NativeFieldInfoPtr_HitPoint = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "HitPoint");
		NativeFieldInfoPtr_ImpactForceDirection = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactForceDirection");
		NativeFieldInfoPtr_ImpactForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactForce");
		NativeFieldInfoPtr_ImpactDamage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactDamage");
		NativeFieldInfoPtr_ImpactType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactType");
		NativeFieldInfoPtr_ImpactSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactSource");
		NativeFieldInfoPtr_ImpactID = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ImpactID");
		NativeFieldInfoPtr_ExplosionType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Impact>.NativeClassPtr, "ExplosionType");
		NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Impact>.NativeClassPtr, 100686034);
		NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Impact>.NativeClassPtr, 100686035);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Impact>.NativeClassPtr, 100686036);
		NativeMethodInfoPtr_IsLethal_Public_Static_Boolean_EImpactType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Impact>.NativeClassPtr, 100686037);
		NativeMethodInfoPtr_IsPlayerImpact_Public_Boolean_byref_Player_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Impact>.NativeClassPtr, 100686038);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 295501, RefRangeEnd = 295504, XrefRangeStart = 295494, XrefRangeEnd = 295501, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Impact(Vector3 hitPoint, Vector3 impactForceDirection, float impactForce, float impactDamage, EImpactType impactType, NetworkObject impactSource, int impactID)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Impact>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = (nint)(&hitPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactForceDirection;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactForce;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactDamage;
		*(EImpactType**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactType;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)impactSource);
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactID;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 295511, RefRangeEnd = 295513, XrefRangeStart = 295504, XrefRangeEnd = 295511, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Impact(Vector3 hitPoint, Vector3 impactForceDirection, float impactForce, float impactDamage, EImpactType impactType, NetworkObject impactSource)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Impact>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = (nint)(&hitPoint);
		*(Vector3**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactForceDirection;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactForce;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactDamage;
		*(EImpactType**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &impactType;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)impactSource);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Vector3_Vector3_Single_Single_EImpactType_NetworkObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2575)]
	[CachedScanResults(RefRangeStart = 370, RefRangeEnd = 2945, XrefRangeStart = 370, XrefRangeEnd = 2945, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Impact()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Impact>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 295513, RefRangeEnd = 295515, XrefRangeStart = 295513, XrefRangeEnd = 295513, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsLethal(EImpactType impactType)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&impactType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsLethal_Public_Static_Boolean_EImpactType_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 295527, RefRangeEnd = 295530, XrefRangeStart = 295515, XrefRangeEnd = 295527, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsPlayerImpact(out Player player)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		nint num = 0;
		*ptr = (nint)(&num);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPlayerImpact_Public_Boolean_byref_Player_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num2 = num;
		player = ((num2 == 0) ? null : new Player(num2));
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public Impact(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
