function FxDTSBrickData::onWaterPlant(%this, %obj)
{
	if (%obj.waterLevel $= "")
		%obj.waterLevel = %this.level / 5;

	%obj.setColliding(0);
	%obj.setRayCasting(0);
	%obj.setShapeFX(2);

	%zone = new PhysicalZone()
	{
		polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
		isWater = 1;

		waterViscosity = 40;
		waterDensity = 1;
		waterColor = "0.529412 0.803922 0.976471 0.394118";
	};

	MissionCleanup.add(%zone);
	%obj.waterZone = %zone;

	%sub = "0 0" SPC 0.1 + (2 - %this.height) * 0.5;
	//%sub = "0 -1" SPC (2 - %this.height) * 0.5 + 0.5;
	//%sub = "0 -1 0";

	%zone.setScale("2 2" SPC %this.height);
	%zone.setTransform(vectorSub(%obj.getPosition(), %sub));

	%obj.needsWaterUpdate();
}

function GridWorld::updateWater(%this, %x, %y, %z)
{
	%level = %this.getWaterLevel(%x, %y, %z);

	if (%level < 0.02)
	{
		%this.setWaterLevel(%x, %y, %z, 0);
		return;
	}

	if (%level <= 0)
		return;

	%source = %this.getBrick(%x, %y, %z);

	if (!isObject(%source))
		return;

	if (%z != 0)
	{
		%belowLevel = %this.getWaterLevel(%x, %y, %z - 1);

		if (%belowLevel != -1 && %belowLevel < 1)
		{
			%taken = getMin(%level, 1 - %belowLevel);
			%level -= %taken;

			%this.setWaterLevel(%x, %y, %z - 1, %taken);
		}
	}

	if (%level <= 0)
	{
		%this.setWaterLevel(%x, %y, %z, 0);
		return;
	}

	%dx[0] = -1;
	%dx[1] = 1;
	%dy[2] = -1;
	%dy[3] = 1;

	%count = 0;
	%sum = 0;

	for (%i = 0; %i < 4; %i++)
	{
		%nx = %x + %dx[%i];
		%ny = %y + %dy[%i];

		%neighborLevel = %this.getWaterLevel(%nx, %ny, %z);

		if (%neighborLevel != -1 && %neighborLevel < %level)
		{
			%sum += %neighborLevel;
			%count++;
		}
	}

	if (%count <= 0)
		return;

	%average = %sum / %count;
	echo(%average);

	if (%this.level < %average)
		return;

	%shared = (%level - %average) / 2;
	%level -= %shared;
	%shared /= %count;

	for (%i = 0; %i < 4; %i++)
	{
		%nx = %x + %dx[%i];
		%ny = %y + %dy[%i];

		%neighborLevel = %this.getWaterLevel(%nx, %ny, %z);

		if (%neighborLevel != -1 && %neighborLevel < %level)
			%count++;
	}

	for (%i = 0; %i < 4; %i++)
	{
		%nx = %x + %dx[%i];
		%ny = %y + %dy[%i];

		%neighborLevel = %this.getWaterLevel(%nx, %ny, %z);

		if (%neighborLevel != -1 && %neighborLevel < %level)
			%this.setWaterLevel(%nx, %ny, %z, %neighborLevel + %shared);
	}

	%this.setWaterLevel(%x, %y, %z, %level);
}

function GridWorld::getWaterLevel(%this, %x, %y, %z)
{
	if (%z < 0)
		return -1;

	%brick = %this.getBrick(%x, %y, %z);

	if (!isObject(%brick))
		return 0;

	if (!%brick.getDataBlock().containsWater)
		return -1;

	return %brick.waterLevel;
}

function GridWorld::setWaterLevel(%this, %x, %y, %z, %level)
{
	%brick = %this.getBrick(%x, %y, %z);

	if (isObject(%brick) && !%brick.getDataBlock().containsWater)
		return;

	%level = mClampF(%level, 0, 1);
	%data = nameToID("BrickPlainData" @ mCeil(%level * 5));

	if (isObject(%brick))
	{
		if (%level > 0)
		{
			if (%level != %brick.waterLevel)
			{
				%brick.waterLevel = %level;
				%brick.needsWaterUpdate();
			}

			if (isObject(%data) && %brick.getDataBlock() != %data)
				%brick.setDataBlock(%data);
		}
		else
			%this.set(%x, %y, %z, 0);
	}
	else if (isObject(%data))
	{
		%brick = %this.set(%x, %y, %z, %data);
		%brick.waterLevel = %level;
	}
}

function FxDTSBrick::needsWaterUpdate(%this)
{
	if (!isEventPending(%this.updateWater))
		%this.updateWater = GridWorld.schedule(400, "updateWater",
			%this.gridX, %this.gridY, %this.gridZ);
}