using Sandbox;
using System.Numerics;

public sealed class Inventory : Component
{
	public List<EquipmentManager> itemList;
	public Inventory()
	{
		itemList = new List<EquipmentManager>();
	}

	public void AddItem( EquipmentManager item )
	{
		itemList.Add( item );
	}
	public List<EquipmentManager> GetItemList()
	{
		return itemList;
	}
	public void EnableDisableWeapons( string status, int weapon )
	{
		if ( status == "Enable" )
		{
			itemList[weapon].GameObject.Enabled = true;

		}
		else if ( status == "Disable" )
		{
			itemList[weapon].GameObject.Enabled = false;
		}
	}
}

