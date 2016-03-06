datablock StaticShapeData(AsteroidShape1)
{
	shapeFile = $SS::Path @ "shapes/asteroids/1.dts";
	scale = "7.312 7.312 7.312";
};

datablock StaticShapeData(AsteroidShape2)
{
	shapeFile = $SS::Path @ "shapes/asteroids/2.dts";
	scale = "0.08 0.08 0.08";
};

datablock StaticShapeData(AsteroidShape3)
{
	shapeFile = $SS::Path @ "shapes/asteroids/3.dts";
	scale = "0.065 0.065 0.065";
};

function clearAsteroids()
{
	for (%i = MissionCleanup.getCount() - 1; %i >= 0; %i--)
	{
		%obj = MissionCleanup.getObject(%i);

		if (%obj.isAsteroid)
			%obj.delete();
	}
}

function asteroidShower()
{
	%yaw = getScalar($pi);
	//%pitch = getScalar($pi / 2);
	%z = getRandom();

	//%vector = mCos(%yaw) SPC mSin(%yaw) SPC mCos(-%pitch);
	%vector = mSqrt(1 - mPow(%z, 2) * mCos(%yaw)) SPC mSqrt(1 - mPow(%z, 2) * mSin(%yaw)) SPC %z;
	%count = getRandom(6, 12);

	announce("picked direction = " @ %vector);
	announce("  (yaw = " @ %yaw @ " pitch = " @ %pitch @ ")");
	announce("picked count = " @ %count);

	for (%i = 0; %i < %count; %i++)
	{
		%x = -34;
		%y = 0;
		%z = 500;

		%transform = vectorSub(GridWorld.getGridPosition(%x, %y, %z), vectorScale(%vector, 300 + getScalar(50)));
		%transform = vectorAdd(%transform, getScalar(8) SPC getScalar(8) SPC getScalar(8));
		//%transform = vectorSub("0 0 1000", vectorScale(%vector, 20));
		%velocity = vectorScale(%vector, 15 + getScalar(5));

		%obj = createAsteroid(%transform, 1 + getScalar(0.5));
		//announce("spawning asteroid at " @ %transform);
		//%obj.velocity = %velocity;
		%obj.velocity = %velocity;
		//findClientByName("Port").camera.setTransform(%transform);
	}
}

function createAsteroid(%transform, %size)
{
	if (%size $= "")
		%size = 1;

	%data = nameToID("AsteroidShape" @ getRandom(1, 3));

	if (!isObject(%data))
		return 0;

	%obj = new StaticShape()
	{
		datablock = %data;
		size = %size;

		velocity = getScalar(8) SPC getScalar(8) SPC getScalar(8);
		angularVelocity = 0 SPC getScalar(36) SPC getScalar(36);

		isAsteroid = 1;
	};

	%obj.setScale(vectorScale(%data.scale, %size));
	%obj.setTransform(%transform);

	%obj.updateAsteroid();
	return %obj;
}

function StaticShape::onAsteroidCollide(%this, %pos, %normal)
{
	%friction = 0.1;
	%elasticity = 0.6;

	initContainerRadiusSearch(%pos, %this.size * 2.5, $TypeMasks::FxBrickAlwaysObjectType);

	while (%obj = containerSearchNext())
	{
		if (%obj.isGridBrick || %obj.isTile)
			%obj.delete();
	}

	%u = vectorScale(%normal, vectorDot(%this.velocity, %normal) / vectorDot(%normal, %normal));
	%w = vectorSub(%this.velocity, %u);

	%position = vectorAdd(%rayPoint, vectorScale(%normal, 0.1));
	%this.velocity = vectorSub(vectorScale(%w, 1 - %friction), vectorScale(%u, %elasticity));
}

function StaticShape::updateAsteroid(%this)
{
	cancel(%this.updateAsteroid);

	if (vectorDist("0 0 1000", %this.getPosition()) > 600)
	{
		%this.delete();
		return;
	}

	%origin = %this.getPosition();
	%radius = %this.size * 2.5;

	initContainerRadiusSearch(%origin, %radius, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType);

	while (%obj = containerSearchNext())
	{
		if (%obj.isGridBrick || %obj.isTile)
		{
			%ray = containerRayCast(%this.getPosition(), %obj.getPosition(), $TypeMasks::FxBrickAlwaysObjectType);
			%this.onAsteroidCollide(%obj.getPosition(), getWords(%ray, 1, 3));

			//%this.velocity = vectorScale(%this.velocity, -0.25);
			break;
		}

		%dist = vectorDist(%origin, %obj.getHackPosition());

		if (%dist > %radius)
			continue;

		%obj.kill();
	}

	// if (!isObject(%this.col))
	// 	%this.col = createShape(SphereGlowShapeData, "0.5 0 0 0.5");
	// else
	// 	cancel(%this.col.delete);

	// %this.col.delete = %this.col.schedule(64, "delete");
	// %this.col.setScale(%radius*2 SPC %radius*2 SPC %radius*2);

	%euler = axisToEuler(getWords(%this.getTransform(), 3, 6));
	%euler = vectorAdd(%euler, vectorScale(%this.angularVelocity, 32 / 1000));

	%axis = eulerToAxis(%euler);

	%position = %this.getPosition();
	%position = vectorAdd(%position, vectorScale(%this.velocity, 32 / 1000));

	%this.setTransform(%position SPC %axis);
	//%this.col.setTransform(%this.getTransform());

	%this.updateAsteroid = %this.schedule(32, "updateAsteroid");
}

function mMod(%num, %a)
{
	return ((%num / %a) - mFloor(%num / %a)) * %a;
}

function getScalar(%scale)
{
	if (%scale $= "")
		%scale = 1;

	return (getRandom() * 2 - 1) * %scale;
}

function eulerToAxis(%euler)
{
	%euler = VectorScale(%euler,$pi / 180);
	%matrix = MatrixCreateFromEuler(%euler);
	return getWords(%matrix,3,6);
}

function axisToEuler(%axis)
{
	%angleOver2 = getWord(%axis,3) * 0.5;
	%angleOver2 = -%angleOver2;
	%sinThetaOver2 = mSin(%angleOver2);
	%cosThetaOver2 = mCos(%angleOver2);
	%q0 = %cosThetaOver2;
	%q1 = getWord(%axis,0) * %sinThetaOver2;
	%q2 = getWord(%axis,1) * %sinThetaOver2;
	%q3 = getWord(%axis,2) * %sinThetaOver2;
	%q0q0 = %q0 * %q0;
	%q1q2 = %q1 * %q2;
	%q0q3 = %q0 * %q3;
	%q1q3 = %q1 * %q3;
	%q0q2 = %q0 * %q2;
	%q2q2 = %q2 * %q2;
	%q2q3 = %q2 * %q3;
	%q0q1 = %q0 * %q1;
	%q3q3 = %q3 * %q3;
	%m13 = 2.0 * (%q1q3 - %q0q2);
	%m21 = 2.0 * (%q1q2 - %q0q3);
	%m22 = 2.0 * %q0q0 - 1.0 + 2.0 * %q2q2;
	%m23 = 2.0 * (%q2q3 + %q0q1);
	%m33 = 2.0 * %q0q0 - 1.0 + 2.0 * %q3q3;
	return mRadToDeg(mAsin(%m23)) SPC mRadToDeg(mAtan(-%m13, %m33)) SPC mRadToDeg(mAtan(-%m21, %m22));
}