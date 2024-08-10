using System;
using System.Threading;
using Sandbox;
using Sandbox.Citizen;

public sealed class DamageTrigger : Component, Component.ITriggerListener
{
	[Property] float Amount { get; set; } = 10f;
	public bool _IsInside {  get; set; }
	public UnitInfo UInfo {  get; set; }
	[Property] float TimeDamage { get; set; } = 10f;
	Collider collider { get; set; }

	TimeSince _lastDamage;


	public void OnTriggerEnter (Collider other )
	{

		if ( other.GameObject.Tags.Has( "player" ) )
		{
			_IsInside = true;
			UInfo = other.Components.Get<UnitInfo>();
		}
		else return;

		//Log.Info( UInfo );
		//Log.Info( _IsInside );
	}
	
	public void OnTriggerExit (Collider other) 
	{
		if ( other.GameObject.Tags.Has( "player" ) )
		{
			_IsInside = false;
		}
		else return;
		//Log.Info( _IsInside ); 
	}
	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		damage();
	}
	void damage()
	{
		if ( UInfo != null )
		{
			if ( _IsInside is true && _lastDamage > TimeDamage )
			{
				UInfo.Damage( Amount );
				//Log.Info( _lastDamage );
				_lastDamage = 0;
			}
			if ( _IsInside is true && _lastDamage < TimeDamage )
			{
				_lastDamage = _lastDamage + 1;
				//Log.Info( "Reset" );
			}
		}
		else return;

	}

}




// todo :  GameObject.Tags.Has( "enemy" ) )
