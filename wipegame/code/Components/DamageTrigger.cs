using Sandbox;
using Sandbox.Citizen;

public sealed class DamageTrigger : Component, Component.ITriggerListener
{
	[Property] float Amount { get; set; } = 10f;


	public void OnTriggerEnter (Collider other )
	{
		/*var player = other.Components.Get<Player>();
		var UnitInfo = other.Components.Get<UnitInfo>();
		if ( player != null ) 
		{
			UnitInfo.Damage( Amount );
		}
		*/
		Log.Info( other );
	}
	
	public void OnTriggerExit (Collider other) 
	{ 
		Log.Info( other ); 
	}

}
