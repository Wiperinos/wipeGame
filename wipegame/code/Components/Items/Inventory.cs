using Sandbox;

public class Inventory
{
	public List<ItemHelper> itemList;

	public Inventory()
	{
		itemList = new List<ItemHelper>();

		AddItem( new ItemHelper { itemType = ItemHelper.ItemType.Weapon, amount = 1, name = "uno" } );
		AddItem( new ItemHelper { itemType = ItemHelper.ItemType.Weapon, amount = 1, name = "dos" } );
		AddItem( new ItemHelper { itemType = ItemHelper.ItemType.Weapon, amount = 1, name = "tres" } );
		AddItem( new ItemHelper { itemType = ItemHelper.ItemType.Weapon, amount = 1, name = "cuatro" } );
	}
	public void AddItem( ItemHelper item )
	{
		itemList.Add( item );
	}
	public List<ItemHelper> GetItemList()
	{
		return itemList;
	}

}

