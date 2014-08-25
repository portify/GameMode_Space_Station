function Player::triggerZipline(%this)
{
	if (%this.ziplinePull)
		return;

	if (isObject(%this.ziplineObject))
	{
		%this.ziplinePull = 1;
		return;
	}

	%eye = %this.getEyePoint();
	%vec = vectorScale(%this.getEyeVector(), 50);
	%end = vectorAdd(%eye, %vec);

	%ray = containerRayCast(%eye, %end, $TypeMasks::FxBrickObjectType, %this);

	if (!%ray)
		return;

	%this.ziplineObject = getWord(%ray, 0);
	%this.ziplinePoint = getWords(%ray, 1, 3);
	%this.ziplineLength = vectorDist(%eye, %this.ziplinePoint);
	%this.ziplinePull = 0;

	%this.updateZipline();
}

function Player::updateZipline(%this, %draw)
{
	cancel(%this.updateZipline);

	if (!isObject(%this.ziplineObject))
	{
		if (isObject(%draw))
			%draw.delete();

		return;
	}

	if (%this.ziplineLength < 0.5)
	{
		if (isObject(%draw))
			%draw.delete();

		%this.ziplineObject = "";
		%this.ziplinePoint = "";
		%this.ziplineLength = "";
		%this.ziplinePull = "";

		return;
	}

	if (!isObject(%draw))
		%draw = createShape(CylinderGlowShapeData, "0.65 0.65 0.65 1");

	cancel(%draw.delete);

	%draw.delete = %draw.schedule(128, "delete");
	%draw.transformLine(%this.getMuzzlePoint(0), %this.ziplinePoint, 0.2);

	if (%this.ziplinePull)
		%this.ziplineLength -= (32 / 1000) * 24;

	//GrappleRope(%this, %this.ziplinePoint);

	%eye = %this.getEyePoint();
	%velocity = %this.getVelocity();

	%point = %this.ziplinePoint;
	%length = %this.ziplineLength;

	%distance = vectorDist(%eye, %point);

	if (%distance > %length)
	{
		// %vec = vectorNormalize(vectorSub(%point, %eye));
		// %pos = vectorAdd(%point, vectorScale(%vec, %length - 0.5));
		// %temp = vectorSub(vectorAdd(vectorScale(%delta, %length / vectorLen(%delta)), %point), %eye);
		// %vel = vectorAdd(vectorScale(%temp, vectorLen(%velocity) / vectorLen(%temp)), vectorNormalize(vectorSub(%point, %eye)));
		%vel = vectorScale(vectorNormalize(vectorSub(%point, %eye)), 20);

		// %this.setTransform(%pos);
		// %this.setVelocity("0 0 0");
	}

	%this.addAppliedForce(vectorScale(%vel, 160));
	%this.updateZipline = %this.schedule(32, "updateZipline", %draw);
}

function GrappleRope(%obj, %staticPos)
{
	%pos = %staticPos;
	%ppos = %obj.getPosition();
	%dist = getWord(%pos, 0) - getWord(%ppos, 0) SPC getWord(%pos, 1) - getWord(%ppos, 1) SPC getWord(%pos, 2) - getWord(%ppos, 2);
	%vel = %obj.getVelocity();
	%temp = vectorSub(vectorAdd(%ppos, vectorScale(%vel,1)), %pos);
	if(vectorLen(%temp) > %obj.ziplineLength)
   {
      %temp2 = vectorSub(vectorAdd(vectorScale(%temp, %obj.ziplineLength / vectorLen(%temp)), %pos), %ppos);
      %vel = vectorAdd(vectorScale(%temp2, vectorLen(%vel) / vectorLen(%temp2)), vectorScale(vectorSub(%pos, %ppos), 1 / vectorLen(vectorSub(%pos, %ppos))));
   }
   %obj.setVelocity(%vel);
}