using Sandbox;

public sealed class Clothing : Component
{

	protected override void OnStart()
	{
		if ( Components.TryGet<SkinnedModelRenderer>( out var model ) )
		{
			var clothing = ClothingContainer.CreateFromLocalUser();
			clothing.Apply( model );
		}
	}

	protected override void OnUpdate()
	{

	}
}
