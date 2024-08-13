using System;
using Sandbox;
using Sandbox.Citizen;
using static Sandbox.VertexLayout;

public sealed class Player : Component
{
	[Property]
	[Category( "Components" )]
	public GameObject Camera { get; set; }

	[Property]
	[Category( "Components" )]
	public GameObject TestModel { get; set; }

	[Property]
	[Category( "Components" )]
	public CharacterController Controller { get; set; }


	[Property]
	[Category( "Components" )]
	public CitizenAnimationHelper Animator { get; set; }

	[Property]
	[Category( "Components" )]
	public GameObject objectGrabPointTransform { get; set; }

	[Property]
	[Category( "Components" )]
	public WeaponManager weaponManager { get; set; }

	[Property]
	[Category( "Components" )]
	public GameObject weaponPosition { get; set; }

	/// <summary>
	/// Velocidad al Caminar (units per second)
	/// </summary>
	[Property]
	[Category( "Stats" )]
	[Range( 0f, 400f, 1f )]
	public float WalkSpeed { get; set; } = 120f;
	/// <summary>
	/// Velocidad al Correr (units per second)
	/// </summary>
	[Property]
	[Category( "Stats" )]
	[Range( 0f, 800f, 1f )]
	public float RunSpeed { get; set; } = 250f;
	/// <summary>
	/// Fuerza del salto (units per second)
	/// </summary>
	[Property]
	[Category( "Stats" )]
	[Range( 0f, 1000f, 10f )]
	public float JumpStrenght { get; set; } = 400f;
	/// <summary>
	/// Dmg por Golpe
	/// </summary>
	[Property]
	[Category( "Stats" )]
	[Range( 0f, 5f, 0.1f )]
	public float PunchSTR { get; set; } = 1f;
	/// <summary>
	/// Cooldown por golpe
	/// </summary>
	[Property]
	[Category( "Stats" )]
	public float PunchCooldown { get; set; } = 0.5f;
	/// <summary>
	/// Rango del golpe
	/// </summary>
	[Property]
	[Category( "Stats" )]
	public float PunchRange { get; set; } = 50f;
	[Property]
	[Category( "Stats" )]
	public float GrabRange { get; set; } = 50f;
	/// <summary>
	/// Where the camera rotates around and the aim originates from
	/// </summary>
	[Property]
	public Vector3 EyePosition { get; set; }
	public Vector3 EyeWorldPosition => Transform.Local.PointToWorld( EyePosition );
	public Angles EyeAngles { get; set; }
	Transform _initialCameraTransform;
	TimeSince _lastPunch;
	public bool IsRunning { get; set; }
	public bool IsCrouched { get; set; }
	public bool IsGrabbing { get; set; }
	public ObjectGrabable GrabbedObject { get; set; }
	public ObjectGrabable LastHighlited { get; set; }
	public Vector3 CameraForward => Camera.Transform.Rotation.Forward;
	public Vector3 CameraPosition => Camera.Transform.Position;
	public float maxDist = 10000;

	protected override void DrawGizmos()
	{
		if ( !Gizmo.IsSelected ) return;

		var draw = Gizmo.Draw;

		draw.LineSphere( EyePosition, 10f );
		// rango del golpe
		// draw.LineCylinder( EyePosition, EyePosition + Transform.Rotation.Forward * PunchRange, 5f, 5f, 10 ); 

		// rango del grab
		draw.LineCylinder( EyeWorldPosition,  CameraPosition + CameraForward * GrabRange, 5f, 5f, 10 );



	}
	protected override void OnUpdate()
	{
		EyeAngles += Input.AnalogLook;
		EyeAngles = EyeAngles.WithPitch( MathX.Clamp( EyeAngles.pitch, -80f, 80f ) );
		Transform.Rotation = Rotation.FromYaw( EyeAngles.yaw );
		if ( Camera != null )
		{
			var cameraTransform = _initialCameraTransform.RotateAround( EyePosition, EyeAngles.WithYaw( 0f ) );
			var cameraPosition = Transform.Local.PointToWorld( cameraTransform.Position );
			var cameraTrace = Scene.Trace.Ray( EyeWorldPosition, cameraPosition )
				.Size( 5f )
				.IgnoreGameObjectHierarchy( GameObject )
				.WithoutTags( "player" )
				.Run();
			Camera.Transform.Position = cameraTrace.EndPosition;
			Camera.Transform.LocalRotation = cameraTransform.Rotation;
		}
		UpdateCrouch();
		UpdateAnimations();
		UpdateGrab();
		HighlightTraceGrab();
		CameraCenterTest();
	}
	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		if ( Controller == null ) return;

		var wishSpeed = Input.Down( "Run" ) ? RunSpeed : WalkSpeed;
		var wishVelocity = Input.AnalogMove.Normal * wishSpeed * Transform.Rotation;




		Controller.Accelerate( wishVelocity );

		if ( Controller.IsOnGround )
		{
			Controller.Acceleration = 10f;
			Controller.ApplyFriction( 5f, 20f );

			if ( Input.Pressed( "Jump" ) )
			{
				Controller.Punch( Vector3.Up * JumpStrenght );

				if ( Animator != null )
					Animator.TriggerJump();
			}
		}
		else
		{
			Controller.Acceleration = 5f;
			Controller.Velocity += Scene.PhysicsWorld.Gravity * Time.Delta;
		}

		Controller.Move();

		if ( Animator != null )
		{
			Animator.WithVelocity( Controller.Velocity );

			if ( _lastPunch >= 2f )
				Animator.HoldType = CitizenAnimationHelper.HoldTypes.None;
		}
		if ( Input.Pressed( "Punch" ) && _lastPunch >= PunchCooldown )
		{
			Punch();

		}
	}
	protected override void OnStart()
	{
		if ( Camera != null )
			_initialCameraTransform = Camera.Transform.Local;
	}
	protected override void OnEnabled()
	{
		base.OnEnabled();
	}
	protected override void OnDisabled()
	{
		base.OnDisabled();
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
	public void Punch()
	{
		if ( Animator != null )
		{
			Animator.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
			Animator.Target.Set( "b_attack", true );
		}
		var punchTrace = Scene.Trace
			.FromTo( EyeWorldPosition, EyeWorldPosition + EyeAngles.Forward * PunchRange )
			.Size( 10f )
			.WithoutTags( "player" )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();
		if ( punchTrace.Hit )
			if ( punchTrace.GameObject.Components.TryGet<UnitInfo>( out var unitInfo ) )
				unitInfo.Damage( PunchSTR );


		_lastPunch = 0f;
	}
	public void Grab()
	{
		var grabTrace = Scene.Trace
			.FromTo( CameraPosition, EyeWorldPosition + (CameraForward) * GrabRange )
			.Size( 10f )
			.WithTag( "grab" )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();
		if ( grabTrace.Hit )
		{
			if ( grabTrace.GameObject.Components.TryGet<ObjectGrabable>( out var objectGrab ) )
			{
				//var objectGrabPosition = EyeWorldPosition + Camera.Transform.Rotation.Forward * 100f;

				objectGrab.GameObject.SetParent( this.objectGrabPointTransform, false );
				objectGrab.Transform.Position = objectGrabPointTransform.Transform.Position;
				//objectGrab.Transform.
				GrabbedObject = objectGrab;
				GrabbedObject.GameObject.Components.TryGet<Rigidbody>( out var ridbody );
				ridbody.Gravity = false;
				GrabbedObject.PaintColor();
				GrabbedObject.isGrabbred = true;
				
			}
		}
	}
	public void UnGrab()
	{
		if ( GrabbedObject != null )
		{
			GrabbedObject.GameObject.Components.TryGet<Rigidbody>( out var ridbody );
			ridbody.Gravity = true;
			GrabbedObject.GameObject.Parent = null;
			GrabbedObject.Transform.Position = this.objectGrabPointTransform.Transform.Position;
			GrabbedObject.isGrabbred = false;
			GrabbedObject = null;

		}
	}
	void UpdateCrouch()
	{
		if ( Controller is null ) return;

		if ( Input.Pressed( "Crouch" ) && !IsCrouched )
		{
			IsCrouched = true;
			Controller.Height /= 2f;

			EyePosition = new Vector3  (EyePosition.x , EyePosition.y , (EyePosition.z/2)) ;
			Log.Info( Camera.Transform.Local.Position );
			//Camera.Transform.Local.Position = new Vector3( Camera.Transform.Local.Position.x, Camera.Transform.Local.Position.y, (Camera.Transform.Local.Position.z / 2) );
			//objectGrabPointTransform.Transform.Position  2f;

		}
		if ( Input.Released( "Crouch" ) && IsCrouched )
		{
			IsCrouched = false;
			Controller.Height *= 2f;
			EyePosition = new Vector3( EyePosition.x, EyePosition.y, (EyePosition.z * 2) );
		}
	}
	void UpdateAnimations()
	{
		
		Animator.DuckLevel = IsCrouched ? 1f : 0f;
		Animator.IsGrounded = Controller.IsOnGround;
	}
	void UpdateGrab()
	{

		if ( Input.Pressed( "Grab" ) && !IsGrabbing )
		{
			Grab();
			IsGrabbing = true;

		}
		if ( Input.Released( "Grab" ) && IsGrabbing )
		{
			UnGrab();
			IsGrabbing = false;
		}
	}
	void HighlightTraceGrab()
	{
		var highlightTrace = Scene.Trace
			.FromTo( CameraPosition, EyeWorldPosition + (CameraForward * GrabRange) )
			.Size( 1f )
			.WithTag( "highlight" )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();
		if ( highlightTrace.Hit )
		{
			Gizmo.Draw.Line( EyeWorldPosition, highlightTrace.HitPosition );
			if ( highlightTrace.GameObject.Components.TryGet<ObjectGrabable>( out var highlightoutLineR ) )
			{
				highlightoutLineR.PaintColor();
			}
		}
	}
	private void CameraCenterTest()
	{
		var tr = Scene.Trace
			.FromTo( CameraPosition, CameraPosition + (CameraForward * maxDist) )
			.WithTag( "" )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();
		if ( tr.Hit )
		{
			//Gizmo.Draw.Line( EyeWorldPosition, tr.HitPosition );
			//TestModel.Transform.Position = tr.HitPosition;
		}
	}

}
