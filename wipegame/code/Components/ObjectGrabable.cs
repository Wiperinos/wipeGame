using Sandbox;

public sealed class ObjectGrabable : Component
{
	[Property]
	[Category( "Highlight Outline" )]
	public HighlightOutline HLOutline { get; set; }
	TimeSince LastPaint;
	public bool isGrabbred { get; set; }

	protected override async void OnUpdate()
	{
		if (LastPaint > 0.1 && !isGrabbred )
		{
			ResetColor();
		}
	}
	public void ResetColor()
	{
		if ( HLOutline.Color != "#00000000" )
		{

			HLOutline.Color = "#00000000";
		}
	}
	public void PaintColor()
	{
		HLOutline.Color = "#2E00FF";
		LastPaint = 0;
	}

}
