$CorpseTimeoutValue = 15000;
$TorqueToFeet = 20 / 9.81;

datablock PlayerData(PlayerSpaceArmor : PlayerStandardArmor)
{
	uiName = "space Player";
	isSpacePlayer = 1;

	mass = 160;
	drag = 0;

	airControl = 0;

	runForce = 4320;
	jumpForce = 1728;

	canJet = 0;
	maxTools = 10;

	upMaxSpeed = 1000;
	upResistFactor = 0;
	upResistSpeed = 0;
};

function Player::spaceTick(%this)
{
	cancel(%this.spaceTick);

	if (!%this.getDataBlock().isSpacePlayer)
		return;

	if (%this.inertialDampeners $= "")
		%this.inertialDampeners = 1;

	%worldBox = %this.getWorldBox();

	%sx = (getWord(%worldBox, 3) - getWord(%worldBox, 0)) / 4 / 2;
	%sy = (getWord(%worldBox, 4) - getWord(%worldBox, 1)) / 4 / 2;
	%sz = (getWord(%worldBox, 5) - getWord(%worldBox, 2)) / 4 / 2;

	%position = getWords(%this.getTransform(), 0, 2);
	%position = vectorSub(%position, %sx / 2 SPC %sy / -2 SPC %sz / 2);

	%this.spaceZone.setScale(%sx SPC %sy SPC %sz);
	%this.spaceZone.setTransform(%position);

	%ray = containerRayCast(%this.getPosition(), vectorAdd(%this.getPosition(), "0 0 30"), $TypeMasks::FxBrickObjectType);

	%this.spaceZone.gravityMod = %ray != 0;
	%this.spaceZone.setAppliedForce("0 0 0");

	if (%this.usingJetpack)
	{
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

	// %center = "<just:right><font:palatino linotype:20>";
	// %center = %center @ "<color:77FF77>Jet \c6- Fire extinguisher\n";
	// %center = %center @ "<color:77FF77>Numpad 9 \c6- Inertial dampeners\n";

	%bottom = "<font:palatino linotype:20>";
	//%bottom = %bottom @ "<color:FFFF77>Velocity\c6: " @ vectorLen(%this.getVelocity()) / 2 @ " m/s\n";
	%bottom = %bottom @ "<color:FFFF77>Velocity\c6: " @ vectorLen(%this.getVelocity()) / $TorqueToFeet @ " m/s";
	%bottom = %bottom @ "<just:right>\c6Jetpack: <color:" @ (%this.usingJetpack ? "77FF77>Active" : "FF7777>Inactive") @ " \n<just:left>";
	//%bottom = %bottom @ "<color:FFFF77>Gravity\c6: " @ mFloatLength(-10 * %this.spaceZone.gravityMod / 2, 1) @ " m/s\n";
	%bottom = %bottom @ "<color:FFFF77>Gravity\c6: " @ mFloatLength(20 * %this.spaceZone.gravityMod / $TorqueToFeet, 1) @ " m/s²";
	%bottom = %bottom @ "<just:right>\c6Inertial dampeners: <color:" @ (%this.inertialDampeners ? "77FF77>Active" : "FF7777>Inactive") @ " \n<just:left>";
	%bottom = %bottom @ "<color:FFFF77>To Station\c6: " @ mFloatLength(vectorDist(%this.position, $base.position) / 2, 1) @ " m\n";

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

function PlayerSpaceArmor::onTrigger(%this, %obj, %slot, %state)
{
	Parent::onTrigger(%this, %obj, %slot, %state);

	if (%slot == 4)
		%obj.usingJetpack = %state;
}

package SpaceStation_Player
{
	function Armor::onNewDataBlock(%this, %obj)
	{
		Parent::onNewDataBlock(%this, %obj);

		if (%this.isSpacePlayer)
		{
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