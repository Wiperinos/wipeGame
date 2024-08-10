using Sandbox;

public sealed class WeaponTrigger : Component, Component.ITriggerListener
{
	[Property]
	[Category( "Components" )]
	public string weaponType { get; set; }
	[Property]
	[Category( "Components" )]
	public WeaponManager Manager { get; set; }


	public void OnTriggerEnter( Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			GameObject.Parent.Transform.Position = new Vector3( 0, 0, 0 );
			GameObject.Parent.Name = weaponType;
			GameObject.Parent.Parent = other.GameObject;
			Manager.addWeapon( weaponType );
			GameObject.Enabled = false;
		}
		else return;
	}

}
