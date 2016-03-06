$FuelCapacity = 50;
$FuelUsage = 300; // Liters per hour

$CorpseTimeoutValue = 15000;
$TorqueToFeet = 20 / 9.81;

datablock PlayerData(PlayerSpaceArmor : PlayerStandardArmor)
{
	uiName = "space Player";
	isSpacePlayer = 1;

	mass = 160;
	drag = 0;

	airControl = 0;
	minImpactSpeed = 30;

	runForce = 4320;
	jumpForce = 1728;
	jumpSound = 0;

	canJet = 0;
	maxTools = 10;

	upMaxSpeed = 1000;
	upResistFactor = 0;
	upResistSpeed = 0;
};

datablock PlayerData(PlayerSpaceRunningArmor : PlayerSpaceArmor)
{
	uiName = "";
	isRunning = 1;

	showEnergyBar = 0;
	maxForwardSpeed = 10.5;

	runForce = 6000;
	jumpForce = 864;

	minRunEnergy = 2.5;
	minJumpEnergy = 20;

	runEnergyDrain = 2;
	jumpEnergyDrain = 20;
};

function PlayerSpaceArmor::onTrigger(%this, %obj, %slot, %state)
{
	Parent::onTrigger(%this, %obj, %slot, %state);

	// if (%slot == 0 && %state)
	// 	%obj.triggerZipline();

	if (%obj.spaceZone.gravityMod > 0)
	{
		if (%slot == 4 && %state)
		{
			%obj.changeDataBlock(PlayerSpaceRunningArmor);
			%obj.monitorEnergyLevel();
		}

		%obj.usingJetpack = 0;
	}
	else if (%slot == 4)
		%obj.usingJetpack = %state;
}

function PlayerSpaceRunningArmor::onTrigger(%this, %obj, %slot, %state)
{
	Parent::onTrigger(%this, %obj, %slot, %state);

	if (%slot == 0 && %state)
		%obj.triggerZipline();

	if (%obj.spaceZone.gravityMod > 0)
	{
		if (%slot == 4 && !%state)
		{
			%obj.changeDataBlock(PlayerSpaceArmor);
			%obj.monitorEnergyLevel();
		}

		%obj.usingJetpack = 0;
	}
	else if (%slot == 4)
		%obj.usingJetpack = %state;
}

function Player::monitorEnergyLevel(%this, %last)
{
	cancel(%this.monitorEnergyLevel);

	if (%this.getState() $= "Dead" || !isObject(%this.client))
		return;

	%show = %this.getEnergyLevel() < %this.getDataBlock().maxEnergy;

	if (%show != %last)
		commandToClient(%this.client, 'ShowEnergyBar', %show);

	%this.monitorEnergyLevel = %this.schedule(100, "monitorEnergyLevel", %show);
}

function Player::spaceTick(%this)
{
	cancel(%this.spaceTick);

	if (!%this.getDataBlock().isSpacePlayer)
		return;

	if (%this.fuel $= "")
		%this.fuel = $FuelCapacity;

	// if (!isObject(%this.cube))
	// 	%this.cube = createShape(CubeGlowShapeData, "0 0.5 0 0.5");
	// else
	// 	cancel(%this.cube.delete);

	// %this.cube.delete = %this.cube.schedule(200, "delete");

	if (%this.inertialDampeners $= "")
		%this.inertialDampeners = 0;

	%data = %this.getDataBlock();

	%sx = getWord(%data.boundingBox, 0) / 4;
	%sy = getWord(%data.boundingBox, 1) / 4;
	%sz = getWord(%data.boundingBox, 2) / 4 / 2;

	%position = getWords(%this.getTransform(), 0, 2);
	//%position = vectorAdd(%position, vectorScale(%this.getVelocity(), 32 / 1000 / 2));
	%position = vectorAdd(%position, vectorScale(%this.getVelocity(), 32 / 1000));
	// %this.cube.setTransform(%position);
	// %this.cube.setScale(%sx SPC %sy SPC %sz);
	%position = vectorSub(%position, %sx / 2 SPC %sy / -2 SPC %sz / 2);

	%this.spaceZone.setScale(%sx SPC %sy SPC %sz);
	%this.spaceZone.setTransform(%position);

	%ray = containerRayCast(%this.getPosition(), vectorAdd(%this.getPosition(), "0 0 30"), $TypeMasks::FxBrickObjectType);
	%position = %this.getPosition();

	// %this.spaceZone.gravityMod = %ray != 0;
	%this.spaceZone.gravityMod = 0;
	%this.spaceZone.setAppliedForce("0 0 0");

	initContainerRadiusSearch(%position, 300, $TypeMasks::StaticShapeObjectType);

	while (isObject(%shape = containerSearchNext()))
	{
		if (isObject(%shape.gravityGenerator))
		{
			%this.spaceZone.gravityMod += %shape.gravityGenerator.getGravityMod(%position);
			%this.addAppliedForce(%shape.gravityGenerator.getAppliedForce(%position));
		}
	}

	%usage = $FuelUsage * (32 / 1000) / 60 / 60;

	if (%this.usingJetpack && %this.fuel >= %usage)
	{
		%this.fuel -= %usage;

		//%force = %this.getDataBlock().mass * -20;
		%force = %this.getDataBlock().mass * 20;
		%velocity = vectorScale(%this.getEyeVector(), %force);

		%this.addAppliedForce(%velocity);
	}

	if (%this.inertialDampeners)
	{
		if (vectorLen(%this.getVelocity()) >= 0.25)
			%this.addAppliedForce(vectorScale(%this.getVelocity(), -%this.getDataBlock().mass));
		else
			%this.setVelocity("0 0 0");
	}

	initContainerRadiusSearch(%this.getHackPosition(), 32, $TypeMasks::StaticShapeObjectType);

	while (%obj = containerSearchNext())
	{
		if (isEventPending(%obj.updateSingularity))
		{
			%diff = vectorSub(%obj.getPosition(), %this.getHackPosition());
			%norm = vectorNormalize(%diff);
			%dist = vectorLen(%diff);

			%force = %this.getDataBlock().mass * 50 * (1 - mClampF(%dist / 32, 0, 1));
			%this.addAppliedForce(vectorScale(%norm, %force));
		}
	}

	%force = %this.spaceZone.appliedForce;
	%force = vectorAdd(%force, "0 0" SPC %this.spaceZone.gravityMod * -20 * 160);

	%bottom = "<font:palatino linotype:20>";
	//%bottom = %bottom @ "<color:FFFF77>Velocity\c6: " @ vectorLen(%this.getVelocity()) / 2 @ " m/s\n";
	%bottom = %bottom @ "<color:FFFF77>Velocity\c6: " @ mFloatLength(vectorLen(%this.getVelocity()) / $TorqueToFeet, 1) @ " m/s\n";
	//%bottom = %bottom @ "<just:right><color:" @ (%this.usingJetpack ? "77FF77" : "777777") @ ">Jetpack \n<just:left>";
	//%bottom = %bottom @ "<color:FFFF77>Gravity\c6: " @ mFloatLength(-10 * %this.spaceZone.gravityMod / 2, 1) @ " m/s\n";
	//%bottom = %bottom @ "<color:FFFF77>Gravity\c6: " @ mFloatLength(20 * %this.spaceZone.gravityMod / $TorqueToFeet, 1) @ " m/s�";
	//%bottom = %bottom @ "<color:FFFF77>Acceleration\c6: " @ mFloatLength(vectorLen(%force) / 160 / $TorqueToFeet, 1) @ " m/s�";
	%bottom = %bottom @ "<color:FFFF77>Acceleration\c6: " @ mFloatLength(vectorLen(%force) / 160 / $TorqueToFeet, 1) @ " m/s�";
	%bottom = %bottom @ "<just:right><color:" @ (%this.inertialDampeners ? "77FF77" : "777777") @ ">Inertial dampeners \n<just:left>";
	%bottom = %bottom @ "<color:FFFF77>To Station\c6: " @ mFloatLength(vectorDist(%this.position, $base.position) / 2, 1) @ " m\n";
	%bottom = %bottom @ "<color:FFFF77>Fuel\c6: " @ mFloatLength(%this.fuel, 2) @ " L \n";

	if (%this.getState() !$= "Dead" && isObject(%this.client) && $Sim::Time - %this.lastSpaceUpdate > 0.05)
	{
		//commandToClient(%this.client, 'CenterPrint', %center, 0);
		commandToClient(%this.client, 'BottomPrint', %bottom, 0, 1);

		%this.lastSpaceUpdate = $Sim::Time;
	}

	%this.spaceTick = %this.schedule(32, "spaceTick");
}

function Player::addAppliedForce(%this, %force)
{
	%this.spaceZone.setAppliedForce(vectorAdd(%this.spaceZone.appliedForce, %force));
}

package SpaceStation_Player
{
	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);

		if (%this.defaultMiniGame.owner == 0 && isObject(%this.player))
		{
			%center = "\n\n\n\n\n<font:palatino linotype:20>\n\n";
			%center = %center @ "<color:77FF77>Jet \c6- Sprint / Use Jetpack\n";
			%center = %center @ "<color:77FF77>Plant Brick \c6- Toggle inertial dampeners\n";

			%this.centerPrint(%center, 10);
		}
	}

	function Armor::onNewDataBlock(%this, %obj)
	{
		Parent::onNewDataBlock(%this, %obj);

		if (%this.isSpacePlayer)
		{
			%obj.setShapeNameDistance(15);
			%obj.monitorEnergyLevel();

			if (!isObject(%obj.spaceZone))
			{
				%obj.spaceZone = new PhysicalZone()
				{
					polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
				};

				MissionCleanup.add(%obj.spaceZone);
			}

			if (!isEventPending(%obj.spaceTick))
				%obj.spaceTick = %obj.schedule(0, "spaceTick");
		}
		else
		{
			if (isObject(%obj.spaceZone))
				%obj.spaceZone.delete();

			if (isEventPending(%obj.spaceTick))
				cancel(%obj.spaceTick);
		}
	}

	function Armor::onRemove(%this, %obj)
	{
		if (isObject(%obj.spaceZone))
			%obj.spaceZone.delete();

		Parent::onRemove(%this, %obj);
	}

	function Armor::onImpact(%this, %obj, %col, %pos, %speed)
	{
		if (!%this.isSpacePlayer)
			return Parent::onImpact(%this, %obj, %col, %pos, %speed);

		%speed /= $TorqueToFeet;
		%speed -= 10;
		%speed *= 2;

		if (%speed > 50)
			%speed = 50;

		%obj.damage(%obj, %pos, %speed, $DamageType::Fall);
	}

	function serverCmdPlantBrick(%client, %angle)
	{
		%player = %client.player;

		if (isObject(%player) && %player.getDataBlock().isSpacePlayer)
		{
			%player.inertialDampeners = !%player.inertialDampeners;
			return;
		}

		Parent::serverCmdRotateBrick(%client, %angle);
	}
};

activatePAckage("SpaceStation_Player");
