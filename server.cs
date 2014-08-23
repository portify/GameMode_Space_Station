$SS::Path = "Add-Ons/GameMode_Space_Station/";

exec("./support/vizard.cs");

exec("./scripts/bricks.cs");
exec("./scripts/player.cs");

exec("./scripts/GridWorld.cs");
exec("./scripts/AirGroup.cs");
exec("./scripts/water.cs");
exec("./scripts/singularity.cs");
exec("./scripts/gravity_generator.cs");

if (!isObject(GridWorld))
{
	new ScriptObject(GridWorld)
	{
		size = "4 4 10";
	};

	$base = GridWorld.set(0, 0, 500, BrickBaseData);

	if (isObject($base))
		$base.setTile(Brick4x4fData);
}

function serverCmdBase(%client)
{
	%player = %client.player;

	if (isObject(%player) && isObject($base))
	{
		%player.setTransform(vectorAdd($base.getPosition(), "0 0 1.4"));
		%player.setVelocity("0 0 0");
	}
}

// if ($GameModeArg !$= "Add-Ons/GameMode_Space_Station/gamemode.txt")
// {
// 	error("ERROR: GameMode_Space_Station cannot be used in custom games");
// 	return;
// }

function SimObject::placeBlockTick(%this)
{
	cancel(%this.placeBlockTick);

	if ((%this.getType() & $TypeMasks::PlayerObjectType) && %this.getState() $= "Dead")
		return;

	%client = %this.getControllingClient();
	%data = %client.inventory[%client.currInv];

	if (!isObject(%data))
		return;

	if (%data.brickSizeX > 4 || %data.brickSizeY > 4 || %data.brickSizeZ > 10)
		return;

	if (%this.getClassName() $= "Camera")
		%range = 100;
	else
		%range = 25;

	%start = %this.getEyePoint();

	%end = vectorAdd(%start, vectorScale(%this.getEyeVector(), %range));
	%ray = containerRayCast(%start, %end, $TypeMasks::FxBrickAlwaysObjectType);

	if (%ray.isGridBrick)
	{
		if (%ray.isTile)
			%ray = setWord(%ray, 0, %ray.baseBrick);

		%delta = vectorSub(getWords(%ray, 1, 3), %ray.getPosition());

		if (%ray.getDataBlock() == BrickBaseData.getID() && %data.brickSizeZ == 1 && %data.brickSizeX == %data.brickSizeY && (%data.brickSizeX % 2 == 0))
		{
			if (%data.brickSizeX == 2)
			{
				%dx = getWord(%delta, 0);
				%dy = getWord(%delta, 1);

				%tileX = %dx >= 0;
				%tileY = %dy >= 0;

				%ray.setTile(%data, %tileX, %tileY);
			}
			else
				%ray.setTile(%data);
		}
		else
		{
			%index = getLargestComponent(%delta);
			%sign = mSign(getWord(%delta, %index));

			%x = %ray.gridX;
			%y = %ray.gridY;
			%z = %ray.gridZ;

			switch (%index)
			{
				case 0: %x += %sign;
				case 1: %y += %sign;
				case 2: %z += %sign;
			}

			if (!isObject(GridWorld.getBrick(%x, %y, %z)))
			{
				%forward = %this.getForwardVector();

				%yaw = mATan(getWord(%forward, 1), getWord(%forward, 0));
				%yaw = mFloatLength((%yaw / $pi) * 2, 0) % 4;

				%brick = GridWorld.set(%x, %y, %z, %data, %yaw);

				if (isObject(%brick))
				{
					if (isObject(%client.brickGroup))
						%client.brickGroup.add(%brick);
				}
			}
		}
	}

	%this.placeBlockTick = %this.schedule(250, "placeBlockTick");
}

function getLargestComponent(%vector)
{
	%x = mAbs(getWord(%vector, 0));
	%y = mAbs(getWord(%vector, 1));
	%z = mAbs(getWord(%vector, 2));

	return %x > %y ? (%x > %z ? 0 : 2) : (%y > %z ? 1 : 2);
}

function mSign(%value)
{
	return %value == 0 ? 0 : (%value > 0 ? 1 : -1);
}

package SpaceStationPackage
{
	function MiniGameSO::pickSpawnPoint(%this)
	{
		if (%this.owner == 0 && isObject($base))
			return vectorAdd($base.position, "0 0 1.4");

		return ParenT::pickSpawnPoint(%this);
	}

	function Observer::onTrigger(%this, %obj, %slot, %state)
	{
		Parent::onTrigger(%this, %obj, %slot, %state);

		if (!%obj.getControllingClient().isAdmin || isObject(%obj.getOrbitObject()))
			return;

		if (%slot == 0)
		{
			if (%state)
			{
				%start = %obj.getEyePoint();

				%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 100));
				%ray = containerRayCast(%start, %end, $TypeMasks::FxBrickAlwaysObjectType);

				if (%ray.isGridBrick && (!isObject($base) || %ray != $base))
				{
					if (%ray.isTile && isObject(%ray.baseBrick))
						//%ray.baseBrick.setTile(0, %ray.tileX, %ray.tileY);
						%ray.delete();
					else
						GridWorld.set(%ray.gridX, %ray.gridY, %ray.gridZ, 0);
				}
			}
		}
		else if (%slot == 4)
		{
			if (%state)
				%obj.placeBlockTick();
			else if (isEventPending(%obj.placeBlockTick))
				cancel(%obj.placeBlockTick);
		}
	}

	function Camera::mountImage(%this, %image, %slot)
	{
		Parent::mountImage(%this, %image, %slot);

		if (%slot == 0 && isObject(%image))
			%this.setShapeName(%this.getControllingClient().name);
	}

	function Camera::unMountImage(%this, %slot)
	{
		Parent::unMountImage(%this, %slot);

		if (%slot == 0)
			%this.setShapeName("");
	}

	function BrickImage::onFire(%this, %obj, %slot)
	{
		// Parent::onFire(%this, %obj, %slot);
	}

	function GameConnection::setControlObject(%this, %object)
	{
		Parent::setControlObject(%this, %object);

		if (isObject(%object) && %object.getClassName() $= "Camera")
		//if (isObject(%object))
		{
			//%text = "<font:palatino linotype:30>\n\n\n\c6<font:palatino linotype:24>+";
			%text = "<font:palatino linotype:28><br><br><br>\c6<font:palatino linotype:36>\c6+";

			commandToClient(%this, 'CenterPrint', %text, 0);
		}
		else
			commandToClient(%this, 'ClearCenterPrint');
	}

	function serverCmdRotateBrick(%client, %delta)
	{
		%control = %client.getControlObject();
	}
};

activatePackage("SpaceStationPackage");