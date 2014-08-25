Brick2x2FPrintData.printID = 38;
Brick4x4FData.material = "tile";

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

	material = "tile";
};

datablock FxDTSBrickData(BrickStairsInvertedData)
{
	brickFile = $SS::Path @ "bricks/stairs_inverted.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Stairs Inv";

	colorID = 49;
	orientationFix = 2;

	material = "tile";
};

datablock FxDTSBrickData(BrickStairsCornerData)
{
	brickFile = $SS::Path @ "bricks/stairs_corner.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Stairs Corner";

	colorID = 49;
	orientationFix = 2;

	material = "tile";
};

datablock FxDTSBrickData(BrickStairsCornerInvertedData)
{
	brickFile = $SS::Path @ "bricks/stairs_corner_inverted.blb";

	category = "Space";
	subCategory = "Basic";
	uiName = "Stairs Corner Inv";

	colorID = 49;
	orientationFix = 2;

	material = "tile";
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
	material = "solidglass";
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

	//colorID = 14;
	colorID = 48;
	// height = 2;
	// level = 5;

	// containsWater = 1;
	// isWaterBrick = 1;
};

datablock FxDTSBrickData(BrickPlainRampData)
{
	brickFile = $SS::Path @ "bricks/plain_ramp.blb";
	collisionShapeName = $SS::Path @ "bricks/plain_ramp.dts";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain Ramp";

	colorID = 48;
};

datablock FxDTSBrickData(BrickPlainCornerData)
{
	brickFile = $SS::Path @ "bricks/plain_corner.blb";
	collisionShapeName = $SS::Path @ "bricks/plain_corner.dts";

	category = "Space";
	subCategory = "Plain";
	uiName = "Plain Corner";

	colorID = 48;
};

datablock FxDTSBrickData(BrickTableSingleData)
{
	brickFile = $SS::Path @ "bricks/table_single.blb";

	category = "Space";
	subCategory = "Props";
	uiName = "Table Single";

	colorID = 58;
};

datablock FxDTSBrickData(BrickTableCornerData)
{
	brickFile = $SS::Path @ "bricks/table_corner.blb";

	category = "Space";
	subCategory = "Props";
	uiName = "Table Corner";

	colorID = 58;
};

datablock FxDTSBrickData(BrickChairData)
{
	brickFile = $SS::Path @ "bricks/chair.blb";

	category = "Space";
	subCategory = "Props";
	uiName = "Chair";

	colorID = 58;
};

datablock FxDTSBrickData(BrickTileFloorData)
{
	brickFile = $SS::Path @ "bricks/tile_floor.blb";

	category = "Space";
	subCategory = "Tiles";
	uiName = "Floor Tile";

	colorID = 49;
	material = "tile";
};

datablock FxDTSBrickData(BrickTileFloor2xData)
{
	brickFile = $SS::Path @ "bricks/tile_floor_2x.blb";

	category = "Space";
	subCategory = "Tiles";
	uiName = "Floor Tile Part";

	colorID = 49;
	material = "tile";
};

datablock FxDTSBrickData(BrickTilePatternEvenData)
{
	brickFile = $SS::Path @ "bricks/tile_pattern_even.blb";

	category = "Space";
	subCategory = "Tiles";
	uiName = "Even Pattern Tile";

	colorID = 40;
	material = "concrete";
};

datablock FxDTSBrickData(BrickTilePatternShuffledData)
{
	brickFile = $SS::Path @ "bricks/tile_pattern_shuffled.blb";

	category = "Space";
	subCategory = "Tiles";
	uiName = "Shuffled Pattern Tile";

	colorID = 39;
	material = "wood";
};

function BrickChairData::onActivate(%this, %obj, %player)
{
	switch (%obj.angleID % 4)
	{
		case 0:
			%rotation = "0 0 1 1.5708";
			%vector = "1 0 0";

		case 1:
			%rotation = "0 0 1 3.1415";
			%vector = "0 -1 0";
		case 2:
			%rotation = "0 0 -1 1.5708";
			%vector = "-1 0 0";

		case 3:
			%rotation = "0 0 1 0";
			%vector = "0 1 0";
	}

	%position = vectorAdd(%obj.position, "0 0 0.15");
	%position = vectorAdd(%position, vectorScale(%vector, 0.3));

	%player.setTransform(%position SPC %rotation);
	%player.setActionThread("sit", 1);
}

// datablock FxDTSBrickData(BrickPlainData4)
// {
// 	brickFile = $SS::Path @ "bricks/plain_4.blb";

// 	category = "Space";
// 	subCategory = "Plain";
// 	uiName = "Plain - 80%";

// 	colorID = 14;
// 	height = 1.6;
// 	level = 4;

// 	containsWater = 1;
// 	isWaterBrick = 1;
// };

// datablock FxDTSBrickData(BrickPlainData3)
// {
// 	brickFile = $SS::Path @ "bricks/plain_3.blb";

// 	category = "Space";
// 	subCategory = "Plain";
// 	uiName = "Plain - 60%";

// 	colorID = 14;
// 	height = 1.2;
// 	level = 3;

// 	containsWater = 1;
// 	isWaterBrick = 1;
// };

// datablock FxDTSBrickData(BrickPlainData2)
// {
// 	brickFile = $SS::Path @ "bricks/plain_2.blb";

// 	category = "Space";
// 	subCategory = "Plain";
// 	uiName = "Plain - 40%";

// 	colorID = 14;
// 	height = 0.8;
// 	level = 2;

// 	containsWater = 1;
// 	isWaterBrick = 1;
// };

// datablock FxDTSBrickData(BrickPlainData1)
// {
// 	brickFile = $SS::Path @ "bricks/plain_1.blb";

// 	category = "Space";
// 	subCategory = "Plain";
// 	uiName = "Plain - 20%";

// 	colorID = 14;
// 	height = 0.4;
// 	level = 1;

// 	containsWater = 1;
// 	isWaterBrick = 1;
// };

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

				colorID = %data.colorID $= "" ? 41 : %data.colorID;
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
			return %this.fullTile;
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

				colorID = %data.colorID $= "" ? 41 : %data.colorID;
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
			return %this.tile[%x, %y];
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

function BrickBaseData::onExport(%this, %obj, %handle)
{
	if (%obj.hasTempCover)
		return;

	if (isObject(%obj.fullTile))
	{
		%name = %obj.fullTile.getDataBlock().getName();
		%handle.writeExportData("FullTile" SPC %name SPC %obj.fullTile.colorID);
	}
	else
	{
		for (%x = 0; %x < 2; %x++)
		{
			for (%y = 0; %y < 2; %y++)
			{
				%tile = %obj.tile[%x, %y];

				if (isObject(%tile))
				{
					%name = %tile.getDataBlock().getName();
					%handle.writeExportData("PartTile" SPC %x SPC %y SPC %name SPC %tile.colorID);
				}
			}
		}
	}
}

function BrickBaseData::onImport(%this, %obj, %line)
{
	%field = firstWord(%line);

	switch$ (%field)
	{
		case "FullTile":
			%brick = %obj.setTile(getWord(%line, 1));

			if (isObject(%brick))
				%brick.setColor(getWord(%line, 2));

		case "PartTile":
			%brick = %obj.setTile(getWord(%line, 3), getWord(%line, 1), getWord(%line, 2));

			if (isObject(%brick))
				%brick.setColor(getWord(%line, 4));
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