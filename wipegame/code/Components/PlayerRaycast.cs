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
		if ( !Gizmo.IsSelected ) return;

		var draw = Gizmo.Draw;

		draw.LineSphere( Player.EyePosition, 10f );
		draw.LineCylinder( Player.EyePosition, Player.EyePosition + Transform.Rotation.Forward * Rango, 5f, 5f, 10 );

	}


}
