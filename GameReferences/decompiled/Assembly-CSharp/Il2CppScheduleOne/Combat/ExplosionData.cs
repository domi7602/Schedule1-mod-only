using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2CppScheduleOne.Combat;

[StructLayout(LayoutKind.Explicit)]
public struct ExplosionData
{
	private static readonly System.IntPtr NativeFieldInfoPtr_DamageRadius;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxDamage;

	private static readonly System.IntPtr NativeFieldInfoPtr_PushForceRadius;

	private static readonly System.IntPtr NativeFieldInfoPtr_MaxPushForce;

	private static readonly System.IntPtr NativeFieldInfoPtr_CheckLoS;

	private static readonly System.IntPtr NativeFieldInfoPtr_ExplosionType;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultSmall;

	private static readonly System.IntPtr NativeFieldInfoPtr_LightningStrike;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Boolean_EExplosionType_0;

	[FieldOffset(0)]
	public float DamageRadius;

	[FieldOffset(4)]
	public float MaxDamage;

	[FieldOffset(8)]
	public float PushForceRadius;

	[FieldOffset(12)]
	public float MaxPushForce;

	[FieldOffset(16)]
	[MarshalAs(UnmanagedType.U1)]
	public bool CheckLoS;

	[FieldOffset(20)]
	public EExplosionType ExplosionType;

	public unsafe static ExplosionData DefaultSmall
	{
		get
		{
			Unsafe.SkipInit(out ExplosionData result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultSmall, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultSmall, (void*)(&explosionData));
		}
	}

	public unsafe static ExplosionData LightningStrike
	{
		get
		{
			Unsafe.SkipInit(out ExplosionData result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_LightningStrike, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_LightningStrike, (void*)(&explosionData));
		}
	}

	static ExplosionData()
	{
		Il2CppClassPointerStore<ExplosionData>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "ScheduleOne.Combat", "ExplosionData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr);
		NativeFieldInfoPtr_DamageRadius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "DamageRadius");
		NativeFieldInfoPtr_MaxDamage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "MaxDamage");
		NativeFieldInfoPtr_PushForceRadius = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "PushForceRadius");
		NativeFieldInfoPtr_MaxPushForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "MaxPushForce");
		NativeFieldInfoPtr_CheckLoS = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "CheckLoS");
		NativeFieldInfoPtr_ExplosionType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "ExplosionType");
		NativeFieldInfoPtr_DefaultSmall = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "DefaultSmall");
		NativeFieldInfoPtr_LightningStrike = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, "LightningStrike");
		NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Boolean_EExplosionType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, 100686017);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 295471, RefRangeEnd = 295474, XrefRangeStart = 295471, XrefRangeEnd = 295471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ExplosionData(float damageRadius, float maxDamage, float maxPushForce, bool checkLoS, EExplosionType explosionType = EExplosionType.Default)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&damageRadius);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxDamage;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &maxPushForce;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &checkLoS;
		*(EExplosionType**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &explosionType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_Single_Single_Boolean_EExplosionType_0, (System.IntPtr)(nint)Unsafe.AsPointer(ref this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public unsafe Il2CppSystem.Object BoxIl2CppObject()
	{
		return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ExplosionData>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
	}
}
