datablock FxDTSBrickData(BrickGravityGeneratorTransferData)
{
	brickFile = $SS::Path @ "bricks/gravity_generator_transfer.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Transfer";

	colorID = 2;
	ColorFxID = 4;
	isGenTransfer = 1;
};

datablock FxDTSBrickData(BrickGravityGeneratorControlData)
{
	brickFile = $SS::Path @ "bricks/plain_5.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Control";

	colorID = 48;
	isGenControl = 1;
};

datablock FxDTSBrickData(BrickGravityGeneratorCoreData)
{
	brickFile = $SS::Path @ "bricks/plain_5.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Core";

	colorID = 48;
};

datablock FxDTSBrickData(BrickGravityGeneratorStabilizerData)
{
	brickFile = $SS::Path @ "bricks/plain_5.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Stabilizer";

	colorID = 48;
	isGenStabilizer = 1;
};

function BrickGravityGeneratorControlData::onActivate(%this, %obj)
{
	%obj.updateGeneratorCore();
}

function FxDTSBrick::updateGeneratorCore(%this)
{
	%x = %this.gridX;
	%y = %this.gridY;
	%z = %this.gridZ;

	%t1 = GridWorld.getData(%x, %y + 1, %z).isGenTransfer;
	%t2 = GridWorld.getData(%x, %y - 1, %z).isGenTransfer;
	%t3 = GridWorld.getData(%x + 1, %y, %z).isGenTransfer;
	%t4 = GridWorld.getData(%x - 1, %y, %z).isGenTransfer;

	if (%t1 + %t2 + %t3 + %t4 != 1)
		return;

	if (%t1)
	{
		%transfer1 = GridWorld.getBrick(%x, %y + 1, %z, BrickGravityGeneratorTransferData);
		%transfer2 = GridWorld.getBrick(%x, %y + 2, %z, BrickGravityGeneratorTransferData);

		%stabilizer1 = GridWorld.getBrick(%x - 1, %y + 3, %z, BrickGravityGeneratorStabilizerData);
		%stabilizer2 = GridWorld.getBrick(%x + 1, %y + 3, %z, BrickGravityGeneratorStabilizerData);

		%core = GridWorld.getBrick(%x, %y + 3, %z, BrickGravityGeneratorCoreData);
	}

	if (%t2)
	{
		%transfer1 = GridWorld.getBrick(%x, %y - 1, %z, BrickGravityGeneratorTransferData);
		%transfer2 = GridWorld.getBrick(%x, %y - 2, %z, BrickGravityGeneratorTransferData);

		%stabilizer1 = GridWorld.getBrick(%x + 1, %y - 3, %z, BrickGravityGeneratorStabilizerData);
		%stabilizer2 = GridWorld.getBrick(%x - 1, %y - 3, %z, BrickGravityGeneratorStabilizerData);

		%core = GridWorld.getBrick(%x, %y - 3, %z, BrickGravityGeneratorCoreData);
	}

	if (%t3)
	{
		%transfer1 = GridWorld.getBrick(%x + 1, %y, %z, BrickGravityGeneratorTransferData);
		%transfer2 = GridWorld.getBrick(%x + 2, %y, %z, BrickGravityGeneratorTransferData);

		%stabilizer1 = GridWorld.getBrick(%x + 3, %y - 1, %z, BrickGravityGeneratorStabilizerData);
		%stabilizer2 = GridWorld.getBrick(%x + 3, %y + 1, %z, BrickGravityGeneratorStabilizerData);

		%core = GridWorld.getBrick(%x + 3, %y, %z, BrickGravityGeneratorCoreData);
	}

	if (%t4)
	{
		%transfer1 = GridWorld.getBrick(%x - 1, %y, %z, BrickGravityGeneratorTransferData);
		%transfer2 = GridWorld.getBrick(%x - 2, %y, %z, BrickGravityGeneratorTransferData);

		%stabilizer1 = GridWorld.getBrick(%x - 3, %y + 1, %z, BrickGravityGeneratorStabilizerData);
		%stabilizer2 = GridWorld.getBrick(%x - 3, %y - 1, %z, BrickGravityGeneratorStabilizerData);

		%core = GridWorld.getBrick(%x - 3, %y, %z, BrickGravityGeneratorCoreData);
	}

	if (!isObject(%transfer2) || !isObject(%stabilizer1) || !isObject(%stabilizer2) || !isObject(%core))
	{
		if (isObject(%transfer1))
			%transfer1.setGGStatus(-1);

		if (isObject(%transfer2))
			%transfer2.setGGStatus(-1);

		return;
	}

	// if (isObject(%transfer1.control) && %transfer1.control != %this)
	// 	return;

	// if (isObject(%transfer2.control) && %transfer2.control != %this)
	// 	return;

	// if (isObject(%stabilizer1.control) && %stabilizer1.control != %this)
	// 	return;

	// if (isObject(%stabilizer2.control) && %stabilizer2.control != %this)
	// 	return;

	// if (isObject(%core.control) && %core.control != %this)
	// 	return;

	%transfer1.control = %this;
	%transfer2.control = %this;
	%stabilzer1.control = %this;
	%stabilizer2.control = %this;
	%core.control = %this;

	%this.transfer1 = %transfer1;
	%this.transfer2 = %transfer2;

	%transfer1.setGGStatus(2);
	%transfer2.setGGStatus(2);
}

function FxDTSBrick::setGGStatus(%this, %status)
{
	%this.setColorFX(%status >= 1 ? 4 : 0);

	switch (%status)
	{
		case -1: %this.setColor(52);
		case 0: %this.setColor(0);
		case 1: %this.setColor(2);
		case 2: %this.setColor(6);
	}
}