function GridWorld::onAdd(%this)
{
	%this.airBorderGroup = new ScriptGroup();
	%this.airGroups = new ScriptGroup();
}

function GridWorld::onRemove(%this)
{
	%this.airBorderGroup.delete();
	%this.airGroups.delete();
}

function GridWorld::set(%this, %x, %y, %z, %data, %angleID, %bypass)
{
	if (isEventPending(%this.modifyTick) && !%bypass)
		return;

	if (%x !$= mFloor(%x) || %y !$= mFloor(%y) || %z !$= mFloor(%z))
	{
		error("ERROR: Invalid block coordinates '" @ %x SPC %y SPC %z @ "'");
		return;
	}

	if (%z < 0)
		return;

	%gridSizeX = getWord(%this.size, 0);
	%gridSizeY = getWord(%this.size, 1);
	%gridSizeZ = getWord(%this.size, 2);

	%brick = %this.brick[%x, %y, %z];

	if (isObject(%data))
	{
		if (%data.brickSizeX > %gridSizeX || %data.brickSizeY > %gridSizeY || %data.brickSizeZ > %gridSizeZ)
		{
			error("ERROR: Brick datablock '" @ %data @ "' has invalid dimensions");
			return;
		}

		%data = %data.getID();

		if (%angleID $= "")
			%angleID = 0;

		if (!%bypass)
			%angleID += %data.orientationFix;

		%angleID %= 4;
	}
	else
	{
		if (isObject(%brick))
		{
			%control = %brick.control;
			%brick.delete();

			%this.brick[%x, %y, %z] = "";
			%this.onBlockRemoved(%x, %y, %z, %control);
		}

		return 0;
	}

	%px = (%gridSizeX * 0.5) * %x;
	%py = (%gridSizeY * 0.5) * %y;
	%pz = (%gridSizeZ * 0.2) * (0.5 + %z);

	if (%data.brickSizeZ < %gridSizeZ)
		//%pz -= (%gridSizeZ - %data.brickSizeZ) * 0.2;
		%pz -= (%gridSizeZ - %data.brickSizeZ) * 0.1;

	if (isObject(%brick))
	{
		if (%brick.getDataBlock() == %data && %brick.angleID == %angleID)
			return %brick;

		%control = %brick.control;
		%brick.delete();

		%this.brick[%x, %y, %z] = "";
		%this.onBlockRemoved(%x, %y, %z, %control);
	}

	switch(%angleID % 4)
	{
		case 0: %rotation = "1 0 0 0";
		case 1: %rotation = "0 0 1 90";
		case 2: %rotation = "0 0 1 180";
		case 3: %rotation = "0 0 -1 90";
	}

	%brick = new FxDTSBrick()
	{
		datablock = %data;

		position = %px SPC %py SPC %pz;
		rotation = %rotation;

		isPlanted = 1;
		isGridBrick = 1;

		angleID = %angleID;
		colorID = %data.colorID;

		gridX = %x;
		gridY = %y;
		gridZ = %z;
	};

	if (!isObject(%brick))
		return 0;

	BrickGroup_888888.add(%brick);

	%brick.setTrusted(1);
	%error = %brick.plant();

	if (%error != 0 && %error != 2)
	{
		%brick.delete();
		return 0;
	}

	%this.brick[%x, %y, %z] = %brick;
	%this.onBlockAdded(%x, %y, %z, %data, %brick);

	return %brick;
}

function GridWorld::getGridPosition(%this, %x, %y, %z)
{
	%gridSizeX = getWord(%this.size, 0);
	%gridSizeY = getWord(%this.size, 1);
	%gridSizeZ = getWord(%this.size, 2);

	return
		(%gridSizeX * 0.5) * %x SPC
		(%gridSizeY * 0.5) * %y SPC
		(%gridSizeZ * 0.2) * (0.5 + %z);
}

function GridWorld::getBrick(%this, %x, %y, %z)
{
	if (%x !$= mFloor(%x) || %y !$= mFloor(%y) || %z !$= mFloor(%z))
	{
		error("ERROR: Invalid block coordinates '" @ %x SPC %y SPC %z @ "'");
		return 0;
	}

	%brick = %this.brick[%x, %y, %z];

	if (%z >= 0 && isObject(%brick))
		return %brick;

	return 0;
}

function GridWorld::getData(%this, %x, %y, %z)
{
	if (%x !$= mFloor(%x) || %y !$= mFloor(%y) || %z !$= mFloor(%z))
	{
		error("ERROR: Invalid block coordinates '" @ %x SPC %y SPC %z @ "'");
		return 0;
	}

	%brick = %this.brick[%x, %y, %z];

	if (%z >= 0 && isObject(%brick))
		return %brick.getDataBlock();

	return 0;
}

function GridWorld::getDensity(%this, %x, %y, %z)
{
	if (%x !$= mFloor(%x) || %y !$= mFloor(%y) || %z !$= mFloor(%z))
	{
		error("ERROR: Invalid block coordinates '" @ %x SPC %y SPC %z @ "'");
		return 0;
	}

	%brick = %this.brick[%x, %y, %z];

	if (%z >= 0 && isObject(%brick))
		return 1;

	return 0;
}

function GridWorld::empty(%this, %importFileName)
{
	if (isEventPending(%this.modifyTick))
		return;

	if (%this.minX $= "" || %this.minY $= "" || %this.minZ $= "")
	{
		if (%importFileName !$= "")
			%this.import(%importFileName);

		return;
	}

	if (%this.maxX $= "" || %this.maxY $= "" || %this.maxZ $= "")
	{
		if (%importFileName !$= "")
			%this.import(%importFileName);

		return;
	}

	%this.tickEmpty(%this.minY, %importFileName);
}

function GridWorld::tickEmpty(%this, %y, %importFileName)
{
	cancel(%this.modifyTick);

	if (%y > %this.maxY)
	{
		%this.minX = "";
		%this.minY = "";
		%this.minZ = "";

		%this.maxX = "";
		%this.maxY = "";
		%this.maxZ = "";

		if (%importFileName !$= "")
			%this.import(%importFileName);

		return;
	}

	%this.createSliceCube(%y, "1 0 0 0.5", 50);

	for (%z = %this.minZ; %z <= %this.maxZ; %z++)
	{
		for (%x = %this.minX; %x <= %this.maxX; %x++)
		{
			%brick = %this.brick[%x, %y, %z];

			if (isObject(%brick))
				%brick.delete();
		}
	}

	%range = %this.maxY - %this.minY;
	%moved = %y - %this.minY;

	%progress = (%moved / %range) * 100;
	%text = %text @ "\c6Progress: " @ mFloatLength(%progress, 0) @ "%\n";

	centerPrintAll("\c3Emptying station\n\n" @ %text, 1);
	%this.modifyTick = %this.schedule(50, "tickEmpty", %y + 1, %importFileName);
}

function GridWorld::export(%this, %fileName)
{
	if (isEventPending(%this.modifyTick))
		return;

	%handle = new FileObject();

	if (!%handle.openForWrite(%fileName))
	{
		error("ERROR: Cannot open '" @ %fileName @ "' for writing");
		%handle.delete();
		return;
	}

	if (%this.minX $= "" || %this.minY $= "" || %this.minZ $= "")
	{
		%handle.close();
		%handle.delete();
		return;
	}

	if (%this.maxX $= "" || %this.maxY $= "" || %this.maxZ $= "")
	{
		%handle.close();
		%handle.delete();
		return;
	}

	%this.tickExport(%this.minY, %handle, 0, 0);
}

function GridWorld::tickExport(%this, %y, %handle, %empty, %saved)
{
	cancel(%this.modifyTick);

	if (!isObject(%handle))
		return;

	if (%y > %this.maxY)
	{
		%handle.close();
		%handle.delete();

		return;
	}

	%this.createSliceCube(%y, "0 1 0 0.5", 100);

	for (%z = %this.minZ; %z <= %this.maxZ; %z++)
	{
		for (%x = %this.minX; %x <= %this.maxX; %x++)
		{
			%brick = %this.brick[%x, %y, %z];

			if (!isObject(%brick))
			{
				%empty++;
				continue;
			}

			%handle.writeLine(%x SPC %y SPC %z SPC %brick.getDataBlock().getName() SPC %brick.angleID SPC %brick.colorID);
			%saved++;
		}
	}

	%range = %this.maxY - %this.minY;
	%moved = %y - %this.minY;

	%progress = (%moved / %range) * 100;

	%text = %text @ "\c6Progress: " @ mFloatLength(%progress, 0) @ "%\n";
	%text = %text @ "\c6Empty spaces: " @ %empty @ "\n";
	%text = %text @ "\c6Saved spaces: " @ %saved @ "\n";

	centerPrintAll("\c3Saving station\n\n" @ %text, 1);
	%this.modifyTick = %this.schedule(100, "tickExport", %y + 1, %handle, %empty, %saved);
}

function GridWorld::import(%this, %fileName)
{
	if (isEventPending(%this.modifyTick))
		return;

	if (%this.minX !$= "")
	{
		%this.empty(%fileName);
		return;
	}

	%handle = new FileObject();

	if (!%handle.openForRead(%fileName))
	{
		error("ERROR: Cannot open '" @ %fileName @ "' for reading");
		%handle.delete();
		return;
	}

	%this.tickImport(%handle, 0, 0);
}

function GridWorld::tickImport(%this, %handle, %added, %failed)
{
	cancel(%this.modifyTick);

	if (!isObject(%handle))
		return;

	for (%i = 0; %i < 8; %i++)
	{
		if (%handle.isEOF())
		{
			%handle.close();
			%handle.delete();

			return;
		}

		%line = %handle.readLine();

		%x = getWord(%line, 0);
		%y = getWord(%line, 1);
		%z = getWord(%line, 2);

		%data = getWord(%line, 3);

		%angleID = getWord(%line, 4);
		%colorID = getWord(%line, 5);

		if (isObject(%data) && %data.getClassName() $= "FxDTSBrickData")
		{
			%brick = %this.set(%x, %y, %z, %data, %angleID, 1);
			%brick.setColor(%colorID);

			if (%x == 0 && %y == 0 && %z == 500)
				$base = %brick;

			%added++;
		}
		else
			%failed++;
	}

	%text = %text @ "\c3Added spaces: " @ %added @ "\n";
	%text = %text @ "\c6Failed spaces: " @ %failed @ "\n";

	centerPrintAll("\c3Loading station\n\n" @ %text, 1);
	%this.modifyTick = %this.schedule(16, "tickImport", %handle, %added, %failed);
}

function GridWorld::createSliceCube(%this, %y, %color, %time)
{
	%px = (2 * %this.minX + 2 * %this.maxX) / 2;
	%py = 2 * %y;
	%pz = 1 + (2 * %this.minZ + 2 * %this.maxZ) / 2;

	%sx = 2 * (%this.maxX - %this.minX) + 2;
	%sy = 2;
	%sz = 2 * (%this.maxZ - %this.minZ) + 2;

	createDebugCube(%px SPC %py SPC %pz, %sx SPC %sy SPC %sz, %color, %time);
}

function GridWorld::onBlockAdded(%this, %x, %y, %z, %data, %brick)
{
	if (%this.minX $= "" || %x < %this.minX) %this.minX = %x;
	if (%this.minY $= "" || %y < %this.minY) %this.minY = %y;
	if (%this.minZ $= "" || %z < %this.minZ) %this.minZ = %z;

	if (%this.maxX $= "" || %x > %this.maxX) %this.maxX = %x;
	if (%this.maxY $= "" || %y > %this.maxY) %this.maxY = %y;
	if (%this.maxZ $= "" || %z > %this.maxZ) %this.maxZ = %z;

	cancel(%this.waterSchedule[%x, %y, %z]);
	%base = %this.getBrick(%x, %y, %z - 1);

	if (isObject(%base) && %base.getTileCoverage() == 0)
	{
		%base.setTile(Brick4x4fData);

		if (isObject(%base.fullTile))
			%base.fullTile.setColor(48);

		%base.hasTempCover = 1;
	}
}

function GridWorld::onBlockRemoved(%this, %x, %y, %z, %control)
{
	%base = %this.getBrick(%x, %y, %z - 1);

	if (isObject(%base.fullTile) && %base.hasTempCover)
	{
		%base.setTile(0);
		%base.hasTempCover = 0;
	}

	if (isObject(%control))
		%control.updateGeneratorCore();

	//%water
}