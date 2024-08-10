using Sandbox;
using System.Threading.Tasks;

public sealed class ObjectGrabable : Component
{
	[Property]
	[Category( "Highlight Outline" )]
	public HighlightOutline HLOutline { get; set; }
	TimeSince LastPaint;
	public bool isGrabbred { get; set; }

	protected override async void OnUpdate()
	{
		await ColorCheck();
		
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
	public void PaintColorWeapon()
	{
		HLOutline.Color = "#00FF0B";
		LastPaint = 0;
	}
	public async Task ColorCheck()
	{
		if ( (LastPaint > 0.1) && !isGrabbred )
		{
			await Task.Frame();
			ResetColor();
		}
	}
}
