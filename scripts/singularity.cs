// 1x1
// 2x2
// 3x3
// 5x5

function createSingularity(%transform)
{
	%obj = createShape(SphereGlowShapeData);
	%obj.setNodeColor("ALL", "0.4 0 1 1");

	%obj.setScale("12 12 12");
	%obj.setTransform(%transform);

	%obj.schedule(0, "updateSingularity");

	return %obj;
}

function StaticShape::updateSingularity(%this)
{
	cancel(%this.updateSingularity);

	%origin = %this.getPosition();
	%radius = 6;

	initContainerRadiusSearch(%origin, %radius, $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType);

	while (%obj = containerSearchNext())
	{
		if (%obj.isGridBrick || %obj.isTile)
		{
			%obj.delete();
			continue;
		}

		%dist = vectorDist(%origin, %obj.getHackPosition());

		if (%dist > %radius)
			continue;

		%obj.kill();
		%obj.delete();
	}

	initContainerRadiusSearch(%origin, 250, $TypeMasks::FxBrickAlwaysObjectType);

	if (!isObject(%this.target))
	{
		while (%obj = containerSearchNext())
		{
			if (%obj.isGridBrick)
			{
				%this.target = %obj;
				break;
			}
		}
	}

	%speed = 10 * 0.032;
	%position = %this.getPosition();

	if (isObject(%this.target))
	{
		%target = %this.target.getPosition();
		createDebugLine(%this.getPosition(), %target, "1 0 0 1", 100, 0.05);

		%sub = vectorSub(%target, %position);
		%vec = vectorNormalize(%sub);

		if (vectorLen(%sub) < %speed)
			%moved = %sub;
		else
			%moved = vectorScale(%vec, %speed);

		%position = vectorAdd(%position, %moved);
	}

	%this.setTransform(%position);
	%this.updateSingularity = %this.schedule(32, "updateSingularity");
}