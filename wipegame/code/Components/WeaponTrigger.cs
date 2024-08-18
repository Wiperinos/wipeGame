using Sandbox;

public sealed class WeaponTrigger : Component, Component.ITriggerListener
{
	[Property]
	[Category( "Components" )]
	public EquipmentManager EquipmentManager { get; set; }


	public void OnTriggerEnter( Collider other )
	{
		if ( other.Tags.Has( "player" ) )
		{
			var realGameObject = GameObject.Parent;
			realGameObject.Name = EquipmentManager.Name;
			other.GameObject.Components.TryGet<Player>( out var player );


			var attachment = player.Animator.Target.GetAttachment( EquipmentManager.Attachment ) ?? global::Transform.Zero;
			realGameObject.Transform.World = attachment.ToWorld( EquipmentManager.AttachmentTransform );
			realGameObject.SetParent( player.Animator.Target.GetBoneObject( EquipmentManager.Attachment ) );
			realGameObject.Enabled = false;
			GameObject.Enabled = false;
			player.inventory.AddItem( EquipmentManager );

		}
		else return;
	}
	
}
