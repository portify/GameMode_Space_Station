datablock AudioProfile(GravityGeneratorLoopSound)
{
	fileName = $SS::Path @ "sounds/gravity_generator_loop.wav";
	description = AudioDefault3D;
	preload = 1;
};

datablock StaticShapeData(EmptyShapeData)
{
	shapeFile = "base/data/shapes/empty.dts";
};

datablock FxDTSBrickData(BrickGravityGeneratorTransferData)
{
	brickFile = $SS::Path @ "bricks/gravity_generator_transfer.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Transfer";

	colorID = 52;
	ColorFxID = 0;
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
	brickFile = $SS::Path @ "bricks/gravity_generator_stabilizer.blb";

	category = "Space";
	subCategory = "Gravity Generator";
	uiName = "GG - Stabilizer";

	colorID = 48;
	isGenStabilizer = 1;
};

function BrickGravityGeneratorControlData::onBrickPlant(%this, %obj)
{
	%obj.gravityGenerator = GravityGenerator(%obj);
	%obj.schedule(500, "updateGeneratorCore");
}

function BrickGravityGeneratorControlData::onBrickRemove(%this, %obj)
{
	if (isObject(%obj.gravityGenerator))
		%obj.gravityGenerator.delete();
}

function BrickGravityGeneratorControlData::onExport(%this, %obj, %handle)
{
	%handle.writeExportData("FieldSize" SPC %obj.gravityGenerator.getFieldSize());
	%handle.writeExportData("FieldOffset" SPC %obj.gravityGenerator.getFieldOffset());
	%handle.writeExportData("GravityMod" SPC %obj.gravityGenerator.gravityMod);
	%handle.writeExportData("Visible" SPC %obj.gravityGenerator.visible);
}

function BrickGravityGeneratorControlData::onImport(%this, %obj, %line)
{
	%field = firstWord(%line);
	%value = restWords(%line);

	switch$ (%field)
	{
		case "FieldSize": %obj.gravityGenerator.setFieldSize(%value);
		case "FieldOffset": %obj.gravityGenerator.setFieldOffset(%value);
		case "GravityMod": %obj.gravityGenerator.gravityMod = %value;
		case "Visible": %obj.gravityGenerator.setVisible(%value);
	}
}

function BrickGravityGeneratorControlData::onActivate(%this, %obj, %player, %client)
{
	%obj.updateGeneratorCore(%client);
}

function FxDTSBrick::updateGeneratorCore(%this, %client)
{
	if (!isObject(%this.gravityGenerator))
		return;

	%x = %this.gridX;
	%y = %this.gridY;
	%z = %this.gridZ;

	%t1 = GridWorld.getData(%x, %y + 1, %z).isGenTransfer;
	%t2 = GridWorld.getData(%x, %y - 1, %z).isGenTransfer;
	%t3 = GridWorld.getData(%x + 1, %y, %z).isGenTransfer;
	%t4 = GridWorld.getData(%x - 1, %y, %z).isGenTransfer;

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

	%this.gravityGenerator.setPart("transfer1", %transfer1);
	%this.gravityGenerator.setPart("transfer2", %transfer2);
	%this.gravityGenerator.setPart("stablilizer1", %stabilizer1);
	%this.gravityGenerator.setPart("stablilizer2", %stabilizer2);
	%this.gravityGenerator.setPart("core", %core);

	if (isObject(%client))
	{
		%gen = %this.gravityGenerator;

		switch$ (%gen.status)
		{
			case -1: %status = "<color:888888>Missing Components";
			case 0: %status = "<color:FF7777>Disabled";
			case 1: %status = "<color:FF7700>Low Power";
			case 2: %status = "<color:0077FF>Active";

			default: %status = "\c5Invalid Status";
		}

		%text = "<font:palatino linotype:32>\c3Gravity Generator \c7[" @ %status @ "\c7]\n";
		%text = %text @ "<font:palatino linotype:24>";
		%text = %text @ "\c3Force\c6: " @ %gen.getForce() @ " m/s²\n";
		%text = %text @ "\c3Field Size\c6: " @ %gen.getFieldSize() @ "\n";
		%text = %text @ "\c3Field Offset\c6: " @ %gen.getFieldOffset() @ "\n";

		%client.centerPrint(%text, 4);
	}
}

function GravityGenerator(%control)
{
	return new ScriptGroup()
	{
		class = "GravityGenerator";
		control = %control;
	};
}

function GravityGenerator::onAdd(%this)
{
	%this.visible = 1;
	%this.gravityMod = 1;

	%this.parts = new SimSet();

	%this.marker = new StaticShape()
	{
		datablock = EmptyShapeData;
		gravityGenerator = %this;
	};

	%this.cube = createShape(CubeGlowShapeData, "0.2 0.7 0.9 0.04");
	%this.setStatus(-1);

	%this.setFieldSize("10 10 5");
	%this.setFieldOffset("0 0 0");
}

function GravityGenerator::onRemove(%this)
{
	%this.setStatus(-1);
	%this.marker.delete();

	if (isObject(%this.audio))
		%this.audio.delete();

	%this.parts.delete();
	%this.cube.delete();
}

function GravityGenerator::setPart(%this, %name, %obj)
{
	if (isObject(%obj.gravityGenerator) && %obj.gravityGenerator != %this)
		%obj.gravityGenerator.setPart(%obj.gravityGeneratorPart, "");

	if (isObject(%this.part[%name]) && %this.parts.isMember(%this.part[%name]))
		%this.parts.remove(%this.part[%name]);

	%this.part[%name] = %obj;

	%obj.gravityGenerator = %this;
	%obj.gravityGeneratorPart = %name;

	if (isObject(%obj) && %name $= "core")
	{
		if (isObject(%this.audio))
			%this.audio.setTransform(%obj.getTransform());

		%this.marker.setTransform(%obj.getTransform());
	}

	if (isObject(%obj) && !%this.parts.isMember(%obj))
		%this.parts.add(%obj);

	%this.setStatus(%this.parts.getCount() == 5 ? 2 : -1);
}

function GravityGenerator::setStatus(%this, %status)
{
	%this.status = %status;

	if (%status == 2 && isObject(%this.part["core"]))
	{
		if (%this.visible)
		{
			%core = %this.part["core"];

			%this.cube.setTransform(vectorAdd(%core.getTransform(), %this.offset));
			%this.cube.setScale(vectorScale(%this.size, 2));
		}

		%this.setAudio(GravityGeneratorLoopSound);
	}
	else
	{
		%this.cube.setTransform("0 0 0");
		%this.cube.setScale("0 0 0");

		%this.setAudio(0);
	}

	%colorFX = %status >= 1 ? 4 : 0;

	switch (%status)
	{
		case -1: %color = 52;
		case 0: %color = 0;
		case 1: %color = 2;
		case 2: %color = 6;
	}

	%count = %this.parts.getCount();

	for (%i = 0; %i < %count; %i++)
	{
		%part = %this.parts.getObject(%i);

		if (%part.getDataBlock() == BrickGravityGeneratorTransferData.getID())
		{
			%part.setColor(%color);
			%part.setColorFX(%colorFX);
		}
	}
}

function GravityGenerator::setAudio(%this, %profile)
{
	if (isObject(%this.audio))
	{
		if (%this.audio.profile.getID() == %profile.getID())
			return;

		%this.audio.delete();
	}

	if (!isObject(%profile))
		return;

	%this.audio = new AudioEmitter()
	{
		profile = %profile;

		is3D = 1;
		isLooping = 1;

		maxDistance = 40;
		referenceDistance = 20;

		useProfileDescription = 0;
	};

	if (isObject(%this.part["core"]))
		%this.audio.setTransform(%this.part["core"].getTransform());
}

function GravityGenerator::getGravityMod(%this, %position)
{
	if (!isObject(%this.part["core"]))
		return 0;

	%delta = vectorSub(%position, vectorAdd(%this.part["core"].getPosition(), %this.offset));

	%x = mAbs(getWord(%delta, 0));
	%y = mAbs(getWord(%delta, 1));
	%z = mAbs(getWord(%delta, 2));

	if (%x > getWord(%this.size, 0) || %y > getWord(%this.size, 1) || %z > getWord(%this.size, 2))
		return 0;

	return %this.gravityMod;
}

function GravityGenerator::getAppliedForce(%this, %position)
{
	if (!isObject(%this.part["core"]))
		return 0;

	%delta = vectorSub(%position, vectorAdd(%this.part["core"].getPosition(), %this.offset));

	%x = mAbs(getWord(%delta, 0));
	%y = mAbs(getWord(%delta, 1));
	%z = mAbs(getWord(%delta, 2));

	if (%x > getWord(%this.size, 0) || %y > getWord(%this.size, 1) || %z > getWord(%this.size, 2))
		return 0;

	//return vectorScale("0 -10 0", 160);
	return 0;
	//return vectorScale("0 0 -10", 160);
}

function GravityGenerator::setFieldOffset(%this, %offset)
{
	%this.offset = vectorAdd(vectorScale(%offset, 2), "0 0 -0.1");
	%this.setStatus(%this.status);
}

function GravityGenerator::getFieldOffset(%this)
{
	return vectorScale(vectorSub(%this.offset, "0 0 -0.1"), 0.5);
}

function GravityGenerator::setFieldSize(%this, %size)
{
	%this.size = vectorSub(vectorScale(%size, 2), "1 1 0.9");
	%this.setStatus(%this.status);
}

function GravityGenerator::getFieldSize(%this)
{
	return vectorScale(vectorAdd(%this.size, "1 1 0.9"), 0.5);
}

function GravityGenerator::setVisible(%this, %visible)
{
	%this.visible = !!%visible;
	%this.setStatus(%this.status);
}

function GravityGenerator::getForce(%this)
{
	return %this.gravityMod * -20 / $TorqueToFeet;
}