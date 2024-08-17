using Sandbox;
using Sandbox.Citizen;
using System.Xml.Linq;
using static Sandbox.Citizen.CitizenAnimationHelper;
public enum WeaponType :byte
{
	[Icon( "check_box_outline_blank" )] Default,
	[Icon( "boy" )] Fists,
	[Icon( "gavel" )] Melee,
	[Icon( "accessible_forward" )] Pistol,
	[Icon( "accessible_forward" )] Rifle,
	[Icon( "accessible_forward" )] Potion
}
public enum EquipSlot : byte
{
	Head,
	Face,
	Body,
	Legs,
	Feet,
	Hand
}
public enum HoldType : byte
{
	None,
	Rifle,
	Pistol,
	Punch,
	HoldItem
}

public sealed class EquipmentManager : Component
{
	[Property, Category( "Base" )] public string Name { get; set; }
	[Property, Category( "Base" )] public WeaponType WeaponType { get; set; }
	[Property, Category( "Base" )] public bool IsItem { get; set; }
	[Property, Category( "Base" )] public GameObject ThisGameObject { get; set; }
	[Property, Category( "Equipment" )] public EquipSlot Slot { get; set; } = EquipSlot.Hand;
	[Property, Category( "Equipment" ), ShowIf( "Slot", EquipSlot.Hand )] public HoldType HoldType { get; set; } = HoldType.HoldItem;	
	[Property, Category( "Holding" )] public bool UpdatePosition { get; set; }
	[Property, Category( "Holding" ), ShowIf( "UpdatePosition", true )] public string Attachment { get; set; } = "hand_R";
	[Property, Category( "Holding" ), ShowIf( "UpdatePosition", true )] public Transform AttachmentTransform { get; set; } = global::Transform.Zero;
	[Property, Category( "Item" ), ShowIf( "IsItem", true )] public float ItemDamage {get;set;}
	[Property, Category( "Item" ), ShowIf( "IsItem", true )] public float ItemSpeed { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsItem", true )] public float ItemSpread { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsItem", true )] private bool IsMelee { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsMelee", true )] public float MeleeRange  { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsMelee", true )] public float MeleeCooldown { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsItem", true )] private bool IsWeapon { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsWeapon", true )] public GameObject BulletSpawnPoint { get; set; }
	[Property, Category( "Item" ), ShowIf( "IsWeapon", true )] public GameObject BulletPrefab;

	public TimeSince _lastPunch;

	public void WeaponAttack( Player player)
	{
		if ( Name == "Pistol" )
		{
			PistolAttack( player );
		}
		if ( Name == "Jarr" )
		{
			PunchAttack( player );
		}
	}
	public void PistolAttack( Player player)
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
			var objBullet = BulletPrefab.Clone( BulletSpawnPoint.Transform.Position , BulletSpawnPoint.Transform.Rotation );
			var physics = objBullet.Components.Get<Rigidbody>( FindMode.EnabledInSelfAndDescendants );
			if ( physics != null )
			{
				objBullet.Components.TryGet<Bullet>( out var bulletoo );
				bulletoo.bulletDamage = ItemDamage;
				Gizmo.Draw.Line( BulletSpawnPoint.Transform.Position, tr.HitPosition );
				BulletSpawnPoint.Transform.Rotation = Rotation.LookAt( tr.HitPosition - BulletSpawnPoint.Transform.Position );
				physics.Velocity = BulletSpawnPoint.Transform.Rotation.Forward * ItemSpeed;
			}
		}	
	}
	public void PunchAttack(Player player )
	{
		if (_lastPunch > MeleeCooldown )
		{		
			if ( player.Animator != null )
			{
				player.Animator.HoldType = CitizenAnimationHelper.HoldTypes.Punch;
				player.Animator.Target.Set( "b_attack", true );
			}
			var punchTrace = Scene.Trace
				.FromTo( player.EyeWorldPosition, player.EyeWorldPosition + player.EyeAngles.Forward * MeleeRange )
				.Size( 10f )
				.WithoutTags( "player" )
				.IgnoreGameObjectHierarchy( GameObject )
				.Run();
			if ( punchTrace.Hit )
			{
				if ( punchTrace.GameObject.Components.TryGet<UnitInfo>( out var unitInfo ) )
					unitInfo.Damage( ItemDamage );
				
			}
			_lastPunch = 0f;
		}
	}

}
