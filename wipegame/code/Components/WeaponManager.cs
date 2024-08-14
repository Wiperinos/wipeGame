using System.Net.Sockets;
using Sandbox;

public sealed class WeaponManager : Component
{
	[Property]
	[Category( "Gun Components" )]
	public float weaponSpeed { get; set; }
	[Property]
	[Category( "Gun Components" )]
	public float weaponDamage { get; set; }
	[Property]
	[Category( "Gun Components" )]
	public GameObject bulletPrefab;
	[Property]
	[Category( "Gun Components" )]
	public GameObject bulletSpawnPoint;

	public Player player { get; set; }



	public string _weaponName;


	protected override void OnUpdate()
	{
		Gizmo.Draw.LineSphere( bulletSpawnPoint.Transform.Position, 2f );
		WeaponAttack();
		drawGizmo();
	}
	public void addWeapon( string weapontype )
	{
		_weaponName = weapontype;
		GameObject.Parent.Components.TryGet<Player>( out var player1 );
		player = player1;
		GameObject.Transform.Position = player.weaponPosition.Transform.Position;
		

	}
	public void testweapon()
	{
		Log.Info( _weaponName );
	}
	public void WeaponAttack()
	{
		if ( player == null )
		{
			return;
		}
		if ( player != null )
		{
			if ( Input.Down( "Fire2" ) )
			{
				var dir = player.CameraForward;
				var pos = player.CameraPosition;
				var tr = Scene.Trace
				.FromTo( pos, pos + (dir * 10000) )
				.WithTag( "" )
				.IgnoreGameObjectHierarchy( GameObject )
				.Run();
					if ( tr.Hit )
					{
					var objBullet = bulletPrefab.Clone( bulletSpawnPoint.Transform.Position, bulletSpawnPoint.Transform.Rotation );
					var physics = objBullet.Components.Get<Rigidbody>( FindMode.EnabledInSelfAndDescendants );
					if ( physics != null )
						{
						objBullet.Components.TryGet<Bullet>( out var bulletoo );
						bulletoo.bulletDamage = weaponDamage;
						Gizmo.Draw.Line( bulletSpawnPoint.Transform.Position, tr.HitPosition );
						bulletSpawnPoint.Transform.Rotation =  Rotation.LookAt( tr.HitPosition - bulletSpawnPoint.Transform.Position ); 
						physics.Velocity = bulletSpawnPoint.Transform.Rotation.Forward * 250;
						//Log.Info( tr.Direction );
						}
					}
			}
		}
	}

	public void drawGizmo()
	{
		if ( player == null ) { return; }
		if ( player != null )
		{
			var dir = player.Camera.Transform.Rotation.Forward;
			var draw = Gizmo.Draw;
			draw.LineCylinder( bulletSpawnPoint.Transform.Position, bulletSpawnPoint.Transform.Position + ( dir * 50), 5f, 5f, 10 );		
		}
		//player.EyeAngles.Forward * 10, 5f, 5f, 10 );
		//Log.Info( player.EyeAngles.Forward.ToString() );

		//bulletSpawnPoint.Transform.Position
	}

}
