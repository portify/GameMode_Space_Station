Brick2x2FPrintData.printID = 38;

datablock FxDTSBrickData(BrickBaseData)
{
	brickFile = $SS::Path @ "bricks/base.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Plating Base";

	colorID = 49;
};

datablock FxDTSBrickData(BrickStairsData)
{
	brickFile = $SS::Path @ "bricks/stairs.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Stairs";

	colorID = 49;
	orientationFix = 2;
};

datablock FxDTSBrickData(BrickWallGirdersData)
{
	brickFile = $SS::Path @ "bricks/wall_girders.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Wall - Girders";

	colorID = 48;
};

datablock FxDTSBrickData(BrickWallNormalData)
{
	brickFile = $SS::Path @ "bricks/wall_normal_simplified.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Wall - Normal";

	colorID = 48;
};

datablock FxDTSBrickData(BrickWallRiggedData)
{
	brickFile = $SS::Path @ "bricks/wall_rigged_simplified.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Wall - Rigged";

	colorID = 48;
};

datablock FxDTSBrickData(BrickGlassData)
{
	brickFile = $SS::Path @ "bricks/glass.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Glass";

	colorID = 53;
};

datablock FxDTSBrickData(BrickWireStraightData)
{
	brickFile = $SS::Path @ "bricks/wire_straight.blb";

	category = "Space";
	subCategory = "Wires";
	uiName = "Wire - Straight";

	colorID = 48;
};

datablock FxDTSBrickData(BrickWireTurnData)
{
	brickFile = $SS::Path @ "bricks/wire_turn.blb";

	category = "Space";
	subCategory = "Wires";
	uiName = "Wire - Turn";

	colorID = 48;
};

datablock FxDTSBrickData(BrickWireCrossData)
{
	brickFile = $SS::Path @ "bricks/wire_cross.blb";

	category = "Space";
	subCategory = "Wires";
	uiName = "Wire - Cross";

	colorID = 48;
};

datablock FxDTSBrickData(BrickGateData)
{
	brickFile = $SS::Path @ "bricks/gate.blb";

	category = "Space";
	subCategory = "Wires";
	uiName = "Gate";

	colorID = 48;
};

datablock FxDTSBrickData(BrickPlainData5)
{
	brickFile = $SS::Path @ "bricks/plain_5.blb";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain - 100%";

	colorID = 14;
	height = 2;
	level = 5;

	containsWater = 1;
	isWaterBrick = 1;
};

datablock FxDTSBrickData(BrickPlainData4)
{
	brickFile = $SS::Path @ "bricks/plain_4.blb";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain - 80%";

	colorID = 14;
	height = 1.6;
	level = 4;

	containsWater = 1;
	isWaterBrick = 1;
};

datablock FxDTSBrickData(BrickPlainData3)
{
	brickFile = $SS::Path @ "bricks/plain_3.blb";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain - 60%";

	colorID = 14;
	height = 1.2;
	level = 3;

	containsWater = 1;
	isWaterBrick = 1;
};

datablock FxDTSBrickData(BrickPlainData2)
{
	brickFile = $SS::Path @ "bricks/plain_2.blb";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain - 40%";

	colorID = 14;
	height = 0.8;
	level = 2;

	containsWater = 1;
	isWaterBrick = 1;
};

datablock FxDTSBrickData(BrickPlainData1)
{
	brickFile = $SS::Path @ "bricks/plain_1.blb";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain - 20%";

	colorID = 14;
	height = 0.4;
	level = 1;

	containsWater = 1;
	isWaterBrick = 1;
};

function FxDTSBrick::getTileCoverage(%this)
{
	if (isObject(%this.fullTile))
		return 1;

	return (
		isObject(%this.tile[0, 0]) +
		isObject(%this.tile[0, 1]) +
		isObject(%this.tile[1, 0]) +
		isObject(%this.tile[1, 1])
	) / 4;
}

// This function is really ugly.
function FxDTSBrick::setTile(%this, %data, %x, %y)
{
	if (%this.getDataBlock() != BrickBaseData.getID())
		return;

	%px = getWord(%this.position, 0);
	%py = getWord(%this.position, 1);
	%pz = getWord(%this.position, 2) + 1;

	%x = mClamp(%x, 0, 1);
	%y = mClamp(%y, 0, 1);

	if (!isObject(%data))
	{
		if (isObject(%this.fullTile))
			%this.clearTiles();
		else
			%this.clearTiles(%x, %y);

		return;
	}

	if (%data.brickSizeZ != 1 || %data.brickSizeX != %data.brickSizeY)
	{
		error("ERROR: '" @ %data.getName() @ "' has invalid tile dimensions");
		return;
	}

	if (%data.brickSizeX == 4)
	{
		if (isObject(%this.fullTile))
		{
			if (%this.fullTile.getDataBlock() != %data)
				%this.fullTile.setDataBlock(%data);
		}
		else
		{
			%this.clearTiles("grid");

			%this.fullTile = new FxDTSBrick()
			{
				datablock = %data;
				position = %px SPC %py SPC %pz;

				isPlanted = 1;
				isGridBrick = 1;
				isTile = 1;

				//colorID = %data.colorID;
				colorID = 41;
				printID = %data.printID;

				gridX = %this.gridX;
				gridY = %this.gridY;
				gridZ = %this.gridZ;

				tileX = %this.tileX;
				tileY = %this.tileY;

				baseBrick = %this;
			};

			%this.fullTile.setTrusted(1);
			%this.fullTile.plant();

			%this.getGroup().add(%this.fullTile);
		}
	}
	else if (%data.brickSizeX == 2)
	{
		%tile = %this.tile[%x, %y];

		if (isObject(%tile))
		{
			if (%tile.getDataBlock() != %data)
				%tile.setDataBlock(%data);
		}
		else
		{
			%px += %x - 0.5;
			%py += %y - 0.5;

			%this.clearTiles("full");

			%this.tile[%x, %y] = new FxDTSBrick()
			{
				datablock = %data;
				position = %px SPC %py SPC %pz;

				isPlanted = 1;
				isGridBrick = 1;
				isTile = 1;

				//colorID = %data.colorID;
				colorID = 41;
				printID = %data.printID;

				gridX = %this.gridX;
				gridY = %this.gridY;
				gridZ = %this.gridZ;

				tileX = %this.tileX;
				tileY = %this.tileY;

				baseBrick = %this;
			};

			%this.tile[%x, %y].setTrusted(1);
			%this.tile[%x, %y].plant();

			%this.getGroup().add(%this.tile[%x, %y]);
		}
	}
}

function FxDTSBrick::clearTiles(%this, %x, %y)
{
	if (%this.getDataBlock() != BrickBaseData.getID())
		return;

	if (%y $= "" && (%x $= "full" || %x $= ""))
	{
		if (isObject(%this.fullTile))
		{
			%this.fullTile.delete();
			%this.fullTile = "";
		}
	}

	if (%y $= "" && (%x $= "grid" || %x $= ""))
	{
		for (%x = 0; %x < 2; %x++)
		{
			for (%y = 0; %y < 2; %y++)
			{
				if (isObject(%this.tile[%x, %y]))
				{
					%this.tile[%x, %y].delete();
					%this.tile[%x, %y] = "";
				}
			}
		}
	}
	else if (isObject(%this.tile[%x, %y]))
	{
		%this.tile[%x, %y].delete();
		%this.tile[%x, %y] = "";
	}
}

function BrickBaseData::onDeath(%this, %obj)
{
	%obj.clearTiles();
	Parent::onDeath(%this, %obj);
}

function BrickBaseData::onRemove(%this, %obj)
{
	%obj.clearTiles();
	Parent::onRemove(%this, %obj);
}

function BrickWallNormalData::onActivate(%this, %obj)
{
	//%obj.setDataBlock(BrickWallGirdersData);
}

function FxDTSBrickData::onActivate(%this, %obj)
{
}

function FxDTSBrickData::onBrickPlant(%this, %obj)
{
	if (%this.containsWater)
		%this.onWaterPlant(%obj);
}

function FxDTSBrickData::onBrickRemove(%this, %obj)
{
	if (isObject(%obj.waterZone))
		%obj.waterZone.delete();
}

package ActivateData
{
	function FxDTSBrickData::onPlant(%this, %obj)
	{
		Parent::onPlant(%this, %obj);
		%this.onBrickPlant(%obj);
	}

	function FxDTSBrickData::onLoadPlant(%this, %obj)
	{
		Parent::onLoadPlant(%this, %obj);
		%this.onBrickPlant(%obj);
	}

	function FxDTSBrickData::onDeath(%this, %obj)
	{
		Parent::onDeath(%this, %obj);
		%this.onBrickRemove(%obj);
	}

	function FxDTSBrickData::onRemove(%this, %obj)
	{
		Parent::onRemove(%this, %obj);
		%this.onBrickRemove(%obj);
	}

	function FxDTSBrick::onActivate(%this, %a, %b, %c, %d)
	{
		Parent::onActivate(%this, %a, %b, %c, %d);
		%this.getDataBlock().onActivate(%this, %a, %b, %c, %d);
	}
};

activatePackage("ActivateData");