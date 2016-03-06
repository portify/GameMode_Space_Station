datablock AudioProfile(SplashTextSound)
{
	fileName = $SS::Path @ "sounds/splash_text.wav";
	description = AudioDefault3D;
	preload = 1;
};

datablock AudioProfile(ChatSound)
{
	fileName = $SS::Path @ "sounds/chat.wav";
	description = AudioDefault3D;
	preload = 1;
};

datablock ItemData(ChatItem)
{
	shapeFile = "base/data/shapes/empty.dts";
	gravityMod = 0;
};

function ChatItem::onPickup() {}

package SpaceStation_Chat
{
	function serverCmdMessageSent(%client, %text)
	{
		if (%client.isSuperAdmin && getSubStr(%text, 0, 1) $= "$")
		{
			%text = getSubStr(%text, 1, strLen(%text));
			eval(%text);
			messageAll('', "\c3" @ %client.getPlayerName() @ " \c6=> " @ %text);
			return;
		}
		
		if (!isObject(%client.miniGame))
		{
			Parent::serverCmdMessageSent(%client, %text);
			return;
		}

		%text = trim(stripMLControlChars(%text));

		if (%text $= "")
			return;

		if (isObject(%client.player))
		{
			serverPlay3D(ChatSound, %client.player.getHackPosition());

			%item = new Item()
			{
				datablock = ChatItem;
			};

			MissionCleanup.add(%item);

			%item.setTransform(vectorAdd(%client.player.getPosition(), "0 0 2.5"));
			%item.setVelocity("0 0 0.5");

			%item.setShapeName(%text);
			%item.setShapeNameDistance(20);

			%item.schedule(1500, "delete");
		}

		%display = "<color:FFFF77>" @ %client.getPlayerName() @ " <color:777777>says, '<color:FFFFFF>" @ %text @ "<color:777777>'";
		messageAll('', %display);
	}
};

activatePAckage("SpaceStation_Chat");