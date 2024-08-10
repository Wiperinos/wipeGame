using Sandbox;

public sealed class PlayerRaycast : Component
{
	[Property]
	[Category("Components")]
	public Player Player { get; set; }

	[Property]
	public float Rango { get; set; } = 10f;
	protected override void OnUpdate()
	{

	}

	protected override void DrawGizmos()
	{

	}


}
