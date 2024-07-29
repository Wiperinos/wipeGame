using Sandbox;

public enum UnitType
{
	/// <summary>
	/// Enviromental units or resources
	/// </summary>
	[Icon( "check_box_outline_blank" )]
	None,
	/// <summary>
	/// Player and turrets
	/// </summary>
	[Icon( "boy" )]
	Player,
	/// <summary>
	/// Enemies
	/// </summary>
	[Icon( "cloud_queue" )]
	Snot
}
[Icon( "psychology" )]

public sealed class UnitInfo : Component
{
	[Property]
	public UnitType Team { get; set; }
	[Property]
	[Range( 1f, 100f, 1f )]
	public float MaxHealth { get; set; } = 5f;
	/// <summary>
	/// HP Regen Fuera de combat
	/// </summary>
	[Property]
	[Range( 0f, 2f, 0.1f )]
	public float HealthRegenAmount { get; set; } = 0.5f;

	/// <summary>
	/// Segundos antes de HP Regen
	/// </summary>
	[Property]
	[Range( 0f, 2f, 1f )]
	public float HealthRegenTimer { get; set; }

	public float Health { get; set; }

	public bool Alive { get; private set; } = true;

	TimeSince _lastDamage;
	TimeUntil _nextHeal;


	protected override void OnUpdate()
	{
		if ( _lastDamage >= HealthRegenTimer && Health != MaxHealth && Alive )
		{
			if ( _nextHeal )
			{
				Damage( -HealthRegenAmount );
				_nextHeal = 1f;
			}
		}
	}
	protected override void OnStart()
	{
		Health = MaxHealth;
	}

	/// <summary>
	/// Damage the Unit, Clamped, Heal if set to negative
	/// </summary>
	/// <param name="damage"></param>
	public void Damage( float damage )
	{
		if ( !Alive ) return;

		Health = MathX.Clamp( Health - damage, 0f, MaxHealth );

		if ( damage > 0 )
			_lastDamage = 0f;

		if ( Health <= 0 )
			Krill();
	}
	public void Krill()
	{
		Health = 0f;
		Alive = false;
		GameObject.Destroy();
	}

}
