using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2Cpp;

public class FlockController : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__childPrefab;

	private static readonly IntPtr NativeFieldInfoPtr__childAmount;

	private static readonly IntPtr NativeFieldInfoPtr__slowSpawn;

	private static readonly IntPtr NativeFieldInfoPtr__spawnSphere;

	private static readonly IntPtr NativeFieldInfoPtr__spawnSphereHeight;

	private static readonly IntPtr NativeFieldInfoPtr__spawnSphereDepth;

	private static readonly IntPtr NativeFieldInfoPtr__minSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__maxSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__minScale;

	private static readonly IntPtr NativeFieldInfoPtr__maxScale;

	private static readonly IntPtr NativeFieldInfoPtr__soarFrequency;

	private static readonly IntPtr NativeFieldInfoPtr__soarAnimation;

	private static readonly IntPtr NativeFieldInfoPtr__flapAnimation;

	private static readonly IntPtr NativeFieldInfoPtr__idleAnimation;

	private static readonly IntPtr NativeFieldInfoPtr__diveValue;

	private static readonly IntPtr NativeFieldInfoPtr__diveFrequency;

	private static readonly IntPtr NativeFieldInfoPtr__minDamping;

	private static readonly IntPtr NativeFieldInfoPtr__maxDamping;

	private static readonly IntPtr NativeFieldInfoPtr__waypointDistance;

	private static readonly IntPtr NativeFieldInfoPtr__minAnimationSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__maxAnimationSpeed;

	private static readonly IntPtr NativeFieldInfoPtr__randomPositionTimer;

	private static readonly IntPtr NativeFieldInfoPtr__positionSphere;

	private static readonly IntPtr NativeFieldInfoPtr__positionSphereHeight;

	private static readonly IntPtr NativeFieldInfoPtr__positionSphereDepth;

	private static readonly IntPtr NativeFieldInfoPtr__childTriggerPos;

	private static readonly IntPtr NativeFieldInfoPtr__forceChildWaypoints;

	private static readonly IntPtr NativeFieldInfoPtr__forcedRandomDelay;

	private static readonly IntPtr NativeFieldInfoPtr__flatFly;

	private static readonly IntPtr NativeFieldInfoPtr__flatSoar;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoid;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidHorizontalForce;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidDown;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidUp;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidVerticalForce;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidDistanceMax;

	private static readonly IntPtr NativeFieldInfoPtr__birdAvoidDistanceMin;

	private static readonly IntPtr NativeFieldInfoPtr__soarMaxTime;

	private static readonly IntPtr NativeFieldInfoPtr__avoidanceMask;

	private static readonly IntPtr NativeFieldInfoPtr__roamers;

	private static readonly IntPtr NativeFieldInfoPtr__posBuffer;

	private static readonly IntPtr NativeFieldInfoPtr__updateDivisor;

	private static readonly IntPtr NativeFieldInfoPtr__newDelta;

	private static readonly IntPtr NativeFieldInfoPtr__updateCounter;

	private static readonly IntPtr NativeFieldInfoPtr__activeChildren;

	private static readonly IntPtr NativeFieldInfoPtr__groupChildToNewTransform;

	private static readonly IntPtr NativeFieldInfoPtr__groupTransform;

	private static readonly IntPtr NativeFieldInfoPtr__groupName;

	private static readonly IntPtr NativeFieldInfoPtr__groupChildToFlock;

	private static readonly IntPtr NativeFieldInfoPtr__startPosOffset;

	private static readonly IntPtr NativeFieldInfoPtr__thisT;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddChild_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddChildToParent_Public_Void_Transform_0;

	private static readonly IntPtr NativeMethodInfoPtr_RemoveChild_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_InstantiateGroup_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateChildAmount_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDrawGizmos_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetFlockRandomPosition_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_destroyBirds_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe FlockChild _childPrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childPrefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<FlockChild>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childPrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)flockChild));
		}
	}

	public unsafe int _childAmount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childAmount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childAmount)) = num;
		}
	}

	public unsafe bool _slowSpawn
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__slowSpawn);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__slowSpawn)) = flag;
		}
	}

	public unsafe float _spawnSphere
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphere);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphere)) = num;
		}
	}

	public unsafe float _spawnSphereHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphereHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphereHeight)) = num;
		}
	}

	public unsafe float _spawnSphereDepth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphereDepth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnSphereDepth)) = num;
		}
	}

	public unsafe float _minSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minSpeed)) = num;
		}
	}

	public unsafe float _maxSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxSpeed)) = num;
		}
	}

	public unsafe float _minScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minScale)) = num;
		}
	}

	public unsafe float _maxScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxScale)) = num;
		}
	}

	public unsafe float _soarFrequency
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarFrequency);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarFrequency)) = num;
		}
	}

	public unsafe string _soarAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarAnimation);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarAnimation)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _flapAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flapAnimation);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flapAnimation)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _idleAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleAnimation);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__idleAnimation)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float _diveValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diveValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diveValue)) = num;
		}
	}

	public unsafe float _diveFrequency
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diveFrequency);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diveFrequency)) = num;
		}
	}

	public unsafe float _minDamping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minDamping);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minDamping)) = num;
		}
	}

	public unsafe float _maxDamping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDamping);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDamping)) = num;
		}
	}

	public unsafe float _waypointDistance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waypointDistance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__waypointDistance)) = num;
		}
	}

	public unsafe float _minAnimationSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minAnimationSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minAnimationSpeed)) = num;
		}
	}

	public unsafe float _maxAnimationSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxAnimationSpeed);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxAnimationSpeed)) = num;
		}
	}

	public unsafe float _randomPositionTimer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__randomPositionTimer);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__randomPositionTimer)) = num;
		}
	}

	public unsafe float _positionSphere
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphere);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphere)) = num;
		}
	}

	public unsafe float _positionSphereHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphereHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphereHeight)) = num;
		}
	}

	public unsafe float _positionSphereDepth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphereDepth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionSphereDepth)) = num;
		}
	}

	public unsafe bool _childTriggerPos
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childTriggerPos);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__childTriggerPos)) = flag;
		}
	}

	public unsafe bool _forceChildWaypoints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__forceChildWaypoints);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__forceChildWaypoints)) = flag;
		}
	}

	public unsafe float _forcedRandomDelay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__forcedRandomDelay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__forcedRandomDelay)) = num;
		}
	}

	public unsafe bool _flatFly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flatFly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flatFly)) = flag;
		}
	}

	public unsafe bool _flatSoar
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flatSoar);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__flatSoar)) = flag;
		}
	}

	public unsafe bool _birdAvoid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoid);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoid)) = flag;
		}
	}

	public unsafe int _birdAvoidHorizontalForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidHorizontalForce);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidHorizontalForce)) = num;
		}
	}

	public unsafe bool _birdAvoidDown
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDown);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDown)) = flag;
		}
	}

	public unsafe bool _birdAvoidUp
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidUp);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidUp)) = flag;
		}
	}

	public unsafe int _birdAvoidVerticalForce
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidVerticalForce);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidVerticalForce)) = num;
		}
	}

	public unsafe float _birdAvoidDistanceMax
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDistanceMax);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDistanceMax)) = num;
		}
	}

	public unsafe float _birdAvoidDistanceMin
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDistanceMin);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__birdAvoidDistanceMin)) = num;
		}
	}

	public unsafe float _soarMaxTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarMaxTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__soarMaxTime)) = num;
		}
	}

	public unsafe LayerMask _avoidanceMask
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__avoidanceMask);
			return *(LayerMask*)num;
		}
		set
		{
			*(LayerMask*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__avoidanceMask)) = layerMask;
		}
	}

	public unsafe List<FlockChild> _roamers
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__roamers);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<FlockChild>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__roamers)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector3 _posBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__posBuffer);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__posBuffer)) = vector;
		}
	}

	public unsafe int _updateDivisor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__updateDivisor);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__updateDivisor)) = num;
		}
	}

	public unsafe float _newDelta
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__newDelta);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__newDelta)) = num;
		}
	}

	public unsafe int _updateCounter
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__updateCounter);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__updateCounter)) = num;
		}
	}

	public unsafe float _activeChildren
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeChildren);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__activeChildren)) = num;
		}
	}

	public unsafe bool _groupChildToNewTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupChildToNewTransform);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupChildToNewTransform)) = flag;
		}
	}

	public unsafe Transform _groupTransform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupTransform);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupTransform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe string _groupName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool _groupChildToFlock
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupChildToFlock);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__groupChildToFlock)) = flag;
		}
	}

	public unsafe Vector3 _startPosOffset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startPosOffset);
			return *(Vector3*)num;
		}
		set
		{
			*(Vector3*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startPosOffset)) = vector;
		}
	}

	public unsafe Transform _thisT
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__thisT);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__thisT)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	static FlockController()
	{
		Il2CppClassPointerStore<FlockController>.NativeClassPtr = IL2CPP.GetIl2CppClass("Assembly-CSharp.dll", "", "FlockController");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<FlockController>.NativeClassPtr);
		NativeFieldInfoPtr__childPrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_childPrefab");
		NativeFieldInfoPtr__childAmount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_childAmount");
		NativeFieldInfoPtr__slowSpawn = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_slowSpawn");
		NativeFieldInfoPtr__spawnSphere = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_spawnSphere");
		NativeFieldInfoPtr__spawnSphereHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_spawnSphereHeight");
		NativeFieldInfoPtr__spawnSphereDepth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_spawnSphereDepth");
		NativeFieldInfoPtr__minSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_minSpeed");
		NativeFieldInfoPtr__maxSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_maxSpeed");
		NativeFieldInfoPtr__minScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_minScale");
		NativeFieldInfoPtr__maxScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_maxScale");
		NativeFieldInfoPtr__soarFrequency = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_soarFrequency");
		NativeFieldInfoPtr__soarAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_soarAnimation");
		NativeFieldInfoPtr__flapAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_flapAnimation");
		NativeFieldInfoPtr__idleAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_idleAnimation");
		NativeFieldInfoPtr__diveValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_diveValue");
		NativeFieldInfoPtr__diveFrequency = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_diveFrequency");
		NativeFieldInfoPtr__minDamping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_minDamping");
		NativeFieldInfoPtr__maxDamping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_maxDamping");
		NativeFieldInfoPtr__waypointDistance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_waypointDistance");
		NativeFieldInfoPtr__minAnimationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_minAnimationSpeed");
		NativeFieldInfoPtr__maxAnimationSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_maxAnimationSpeed");
		NativeFieldInfoPtr__randomPositionTimer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_randomPositionTimer");
		NativeFieldInfoPtr__positionSphere = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_positionSphere");
		NativeFieldInfoPtr__positionSphereHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_positionSphereHeight");
		NativeFieldInfoPtr__positionSphereDepth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_positionSphereDepth");
		NativeFieldInfoPtr__childTriggerPos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_childTriggerPos");
		NativeFieldInfoPtr__forceChildWaypoints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_forceChildWaypoints");
		NativeFieldInfoPtr__forcedRandomDelay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_forcedRandomDelay");
		NativeFieldInfoPtr__flatFly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_flatFly");
		NativeFieldInfoPtr__flatSoar = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_flatSoar");
		NativeFieldInfoPtr__birdAvoid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoid");
		NativeFieldInfoPtr__birdAvoidHorizontalForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidHorizontalForce");
		NativeFieldInfoPtr__birdAvoidDown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidDown");
		NativeFieldInfoPtr__birdAvoidUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidUp");
		NativeFieldInfoPtr__birdAvoidVerticalForce = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidVerticalForce");
		NativeFieldInfoPtr__birdAvoidDistanceMax = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidDistanceMax");
		NativeFieldInfoPtr__birdAvoidDistanceMin = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_birdAvoidDistanceMin");
		NativeFieldInfoPtr__soarMaxTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_soarMaxTime");
		NativeFieldInfoPtr__avoidanceMask = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_avoidanceMask");
		NativeFieldInfoPtr__roamers = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_roamers");
		NativeFieldInfoPtr__posBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_posBuffer");
		NativeFieldInfoPtr__updateDivisor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_updateDivisor");
		NativeFieldInfoPtr__newDelta = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_newDelta");
		NativeFieldInfoPtr__updateCounter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_updateCounter");
		NativeFieldInfoPtr__activeChildren = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_activeChildren");
		NativeFieldInfoPtr__groupChildToNewTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_groupChildToNewTransform");
		NativeFieldInfoPtr__groupTransform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_groupTransform");
		NativeFieldInfoPtr__groupName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_groupName");
		NativeFieldInfoPtr__groupChildToFlock = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_groupChildToFlock");
		NativeFieldInfoPtr__startPosOffset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_startPosOffset");
		NativeFieldInfoPtr__thisT = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<FlockController>.NativeClassPtr, "_thisT");
		NativeMethodInfoPtr_Start_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663595);
		NativeMethodInfoPtr_AddChild_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663596);
		NativeMethodInfoPtr_AddChildToParent_Public_Void_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663597);
		NativeMethodInfoPtr_RemoveChild_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663598);
		NativeMethodInfoPtr_Update_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663599);
		NativeMethodInfoPtr_InstantiateGroup_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663600);
		NativeMethodInfoPtr_UpdateChildAmount_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663601);
		NativeMethodInfoPtr_OnDrawGizmos_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663602);
		NativeMethodInfoPtr_SetFlockRandomPosition_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663603);
		NativeMethodInfoPtr_destroyBirds_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663604);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FlockController>.NativeClassPtr, 100663605);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67842, XrefRangeEnd = 67849, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 67886, RefRangeEnd = 67887, XrefRangeStart = 67849, XrefRangeEnd = 67886, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddChild(int amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&amount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddChild_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67887, XrefRangeEnd = 67888, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddChildToParent(Transform obj)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddChildToParent_Public_Void_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67888, XrefRangeEnd = 67900, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RemoveChild(int amount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&amount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveChild_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67900, XrefRangeEnd = 67916, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67916, XrefRangeEnd = 67936, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InstantiateGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InstantiateGroup_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67936, XrefRangeEnd = 67949, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateChildAmount()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateChildAmount_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67949, XrefRangeEnd = 67966, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDrawGizmos()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDrawGizmos_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 67984, RefRangeEnd = 67985, XrefRangeStart = 67966, XrefRangeEnd = 67984, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFlockRandomPosition()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFlockRandomPosition_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67985, XrefRangeEnd = 67997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void destroyBirds()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_destroyBirds_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 67997, XrefRangeEnd = 68015, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe FlockController()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<FlockController>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public FlockController(IntPtr pointer)
		: base(pointer)
	{
	}
}
