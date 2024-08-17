using Sandbox;

public sealed class WeaponTrigger : Component, Component.ITriggerListener
{
	[Property]
	[Category( "Components" )]
	public string weaponType { get; set; }
	[Property]
	[Category( "Components" )]
	public EquipmentManager EquipmentManager { get; set; }

	public GameObject gameObjectParent;

	public void OnTriggerEnter( Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			GameObject.Parent.Transform.Position = new Vector3( 0, 0, 0 );
			GameObject.Parent.Name = weaponType;
			GameObject.Parent.Parent = other.GameObject;
			gameObjectParent = GameObject.Parent;

			other.GameObject.Components.TryGet<Player>( out var player );
			player.inventory.AddItem( EquipmentManager );

			GameObject.Parent.Enabled = false;
			GameObject.Enabled = false;
		}
		else return;
	}
	
}
