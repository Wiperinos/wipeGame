using Sandbox;

public sealed class HealTrigger : Component, Component.ITriggerListener
{
	[Property] float Amount { get; set; } = 10f;
	[Property] TimeSince Rate { get; set; } = 1f;

	public void OnTriggerEnter (Collider other )
	{
		var player = other.Components.Get<Player>();
		var UnitInfo = other.Components.Get<UnitInfo>();
		if ( player != null ) 
		{
			UnitInfo.Damage( -Amount );
			
		}

		Log.Info( other );
	}
	
	public void OnTriggerExit (Collider other) 
	{ 
		Log.Info( other ); 
	}

}
