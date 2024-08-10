using Sandbox;

public sealed class Bullet : Component , Component.ICollisionListener
{
	
	public float bulletDamage;
	

	public async void OnCollisionStart( Collision collision )
	{
		if(collision.Other.GameObject.Tags.Has("enemy"))
		{
			if (collision.Other.GameObject.Components.TryGet<UnitInfo> (out var UInfo ) )
			{
				UInfo.Damage( bulletDamage );
				
			}
		}

		GameObject.Destroy();
	}
	void bulletSetDamage(float weaponManagerDamage)
	{
		bulletDamage = weaponManagerDamage; 
	}


}
