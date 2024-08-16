using Sandbox;
[Title( "Item Helper" )]
[Category( "Items" )]
[Icon( "directions_run" )]
[Alias( "ItemHelper" )]
public sealed class ItemHelper : Component
{

	public enum ItemType
	{
		Weapon,
		Potion,
		Armor,
		Default
	}
	[Property]
	public ItemType itemType { get; set; }
	[Property]
	public int amount { get; set; }
	[Property]
	public string name { get; set; }
	[Property]
	public WeaponManager manager { get; set; }


}
