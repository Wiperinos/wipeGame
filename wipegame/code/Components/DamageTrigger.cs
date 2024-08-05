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

	TimeSince _lastDamage;


	public void OnTriggerEnter (Collider other )
	{
		_IsInside = true;
		//Log.Info( _IsInside );
		UInfo = other.Components.Get<UnitInfo>();
	}
	
	public void OnTriggerExit (Collider other) 
	{
		_IsInside = false;
		Log.Info( _IsInside ); 
	}
	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( _IsInside is true && _lastDamage > TimeDamage)
		{

			UInfo.Damage( Amount );
			
			Log.Info( _lastDamage );
			_lastDamage = 0;
		}
		if ( _IsInside is true && _lastDamage < TimeDamage )
		{
			_lastDamage = _lastDamage+1;
			Log.Info( "Reset" );
		}

	}

}
