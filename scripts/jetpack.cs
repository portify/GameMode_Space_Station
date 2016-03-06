$JetpackEfficiency = 10000; // grams per hour (measured by reference unit of thrust)
$JetpackThrust = 6 * 160; // thrust per second, 160 = mass of player

function serverCmdJetpack(%client, %field, %value)
{
	%client.player.jetpack[%field] = %value;
}

function Player::updateJetpack(%this, %delta)
{
	%force = "0 0 0";

	if (%this.fuel <= 0)
		return;

	%y = %this.getEyeVector();
	%z = %this.getUpVector(); // ... calculate based on pitch
	%x = vectorCross(%y, %z);

	if (%this.jetpackAntiGravity)
	{
		%gravity = %this.spaceZone.appliedForce;
		%gravity = vectorAdd(%gravity, "0 0" SPC %this.spaceZone.gravityMod * -20 * 160);

		%force = vectorAdd(%force, vectorScale(%gravity, -1));
	}

	if (%this.jetpackInertialDamp)
	{
		if (vectorLen(%this.getVelocity()) >= 0.25)
			%force = vectorAdd(%force, vectorScale(%this.getVelocity(), -160));
		else
			%this.setVelocity("0 0 0");
	}

	%force = vectorAdd(%force, vectorScale(%x, %this.jetpackX * $JetpackThrust));
	%force = vectorAdd(%force, vectorScale(%y, %this.jetpackY * $JetpackThrust));
	%force = vectorAdd(%force, vectorScale(%z, %this.jetpackZ * $JetpackThrust));

	%length = vectorLen(%force);
	%cost = (%length / $JetpackThrust) * $JetpackEfficiency * %delta / 60 / 60;

	if (%this.fuel >= %cost)
	{
		%this.fuel -= %cost;
		%this.addAppliedForce(%force);
	}
}