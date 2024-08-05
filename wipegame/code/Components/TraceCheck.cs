using Sandbox;

public sealed class TraceCheck : Component
{
	[Property]
	public Vector3 EyePosition { get; set; }
	public Vector3 EyeWorldPosition => Transform.Local.PointToWorld( EyePosition );

	public Angles EyeAngles { get; set; }
	/*

	protected override void OnUpdate()
	{
		var highlightTrace = Scene.Trace
			.FromTo( EyePosition, EyeWorldPosition + EyeAngles.Forward * 100f )
			.Size( 10f )
			.WithTag( "grab" )
			.IgnoreGameObjectHierarchy( GameObject )
			.Run();
		if ( highlightTrace.Hit)
		{
			if (highlightTrace.GameObject.Components.TryGet<HighlightOutline>( out var highlightoutLineR ))
			{
				//highlightoutLineR.Enabled = true;
				Log.Info( highlightoutLineR );
			}			
		}
	}
	*/
}
