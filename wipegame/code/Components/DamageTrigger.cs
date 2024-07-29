using System.Threading;
using Sandbox;
using Sandbox.Citizen;

public sealed class DamageTrigger : Component, Component.ITriggerListener
{
	[Property] float Amount { get; set; } = 10f;
	public bool _IsInside {  get; set; }
	//	public UnitInfo other2 { get; set; }	


	public void OnTriggerEnter (Collider other )
	{
		_IsInside = true;
		Log.Info( _IsInside );
		//other2.Components.Get<UnitInfo>();

	}
	
	public void OnTriggerExit (Collider other) 
	{
		_IsInside = false;
		Log.Info( _IsInside ); 
	}
	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( _IsInside is true )
		{
			//Collider.
			//Log.Info( _IsInside );
			//other2.Damage( Amount );
		}

	}

}
