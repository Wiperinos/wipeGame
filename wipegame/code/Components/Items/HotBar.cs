using Sandbox;
using System.Numerics;

public class HotBar
{
	public List<ItemHelper> HotBarList;

	public HotBar()
	{
		HotBarList = new List<ItemHelper>();


	}
	public void AddItem( ItemHelper item )
	{
		HotBarList.Add( item );

	}
	public List<ItemHelper> GetItemList()
	{
		return HotBarList;
	}

}
