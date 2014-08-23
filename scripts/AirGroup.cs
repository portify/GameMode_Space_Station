function GridSpace(%x, %y, %z)
{
	return new ScriptObject()
	{
		x = %x;
		y = %y;
		z = %z;
	};
}

function AirGroup()
{
	return new ScriptObject()
	{
		class = "AirGroup";
	};
}

function AirGroup::onAdd(%this)
{
	%this.members = new ScriptGroup();
	%this.borders = new ScriptGroup();

	if (isObject(GridWorld))
		GridWorld.airGroups.add(%this);
	else
		warn("WARNING: AirGroup created without GridWorld");
}

function AirGroup::onRemove(%this)
{
	for (%i = %this.members.getCount() - 1; %i >= 0; %i--)
	{
		%member = %this.members.getObject(%i);
		%this.removeMember(%member.x, %member.y, %member.z);
	}

	for (%i = %this.borders.getCount() - 1; %i >= 0; %i--)
	{
		%border = %this.borders.getObject(%i);
		%this.removeBorder(%border.x, %border.y, %border.z);
	}

	%this.members.delete();
	%this.borders.delete();
}

function AirGroup::addMember(%this, %x, %y, %z)
{
	if (!isObject(GridWorld))
		return "";

	if (isObject(GridWorld.airGroup[%x, %y, %z]))
		return 0;

	if (isObject(%this.member[%x, %y, %z]))
		return 1;

	%obj = GridSpace(%x, %y, %z);

	%this.members.add(%obj);
	%this.member[%x, %y, %z] = %obj;

	GridWorld.airGroup[%x, %y, %z] = %this;
	return 1;
}

function AirGroup::removeMember(%this, %x, %y, %z)
{
	%member = %this.member[%x, %y, %z];

	if (isObject(%member))
	{
		%member.delete();
		%this.member[%x, %y, %z] = "";

		if (isObject(GridWorld) && GridWorld.airGroup[%x, %y, %z] == %this)
			GridWorld.airGroup[%x, %y, %z] = "";
	}
}

function AirGroup::addBorder(%this, %x, %y, %z)
{
	if (!isObject(GridWorld))
		return "";

	if (isObject(%this.border[%x, %y, %z]))
		return 1;

	if (!isObject(GridWorld.airBorder[%x, %y, %z]))
	{
		%group = new SimSet();

		GridWorld.airBorder[%x, %y, %z] = %group;
		GridWorld.airBorderGroup.add(%group);
	}

	%obj = GridSpace(%x, %y, %z);

	%this.borders.add(%obj);
	%this.border[%x, %y, %z] = %obj;

	GridWorld.airBorder[%x, %y, %z].add(%this);
	return 1;
}

function AirGroup::removeBorder(%this, %x, %y, %z)
{
	%border = %this.border[%x, %y, %z];

	if (isObject(%border))
	{
		%border.delete();
		%this.border[%x, %y, %z] = "";

		if (isObject(GridWorld))
		{
			%group = GridWorld.airBorder[%x, %y, %z];

			if (%group.getCount() < 1)
			{
				%group.delete();
				GridWorld.airBorder[%x, %y, %z] = "";
			}
		}
	}
}

function GridWorld::createAirGroup(%this, %sx, %sy, %sz)
{
	%dx[0] = -1;
	%dx[1] = 1;
	%dy[2] = -1;
	%dy[3] = 1;
	%dz[4] = -1;
	%dz[5] = 1;

	// %count = 1;
	// %space[0] = %start;

	%members = new SimSet();
	%borders = new SimSet();

	%start = GridSpace(%sx, %sy, %sz);
	%start.depth = 0;

	%depth = 0;
	%opened[%sx, %sy, %sz] = 1;
	%members.add(%start);

	//while (%count >= 1 && %sanity++ <= 1000)
	for (%i = 0; %i < %members.getCount() && %i < 1000; %i++)
	{
		// %test = %space[%count - 1];
		// %space[%count--] = "";

		%test = %members.getObject(%i);

		//for (%i = 0; %i < 6; %i++)
		for (%j = 0; %j < 6; %j++)
		{
			%nx = %test.x + %dx[%j];
			%ny = %test.y + %dy[%j];
			%nz = %test.z + %dz[%j];

			if (isObject(%this.airGroup[%nx, %ny, %nz]))
				continue;

			if (%opened[%nx, %ny, %nz])
				continue;

			%opened[%nx, %ny, %nz] = 1;

			%density = %this.getDensity(%nx, %ny, %nz);
			%neighbor = GridSpace(%nx, %ny, %nz);
			%neighbor.depth = %test.depth + 1;
			%neighbor.parent = %test;

			if (%neighbor.depth > %depth)
				%depth = %neighbor.depth;

			if (%density <= 0)
			{
				%members.add(%neighbor);

				// schedule(%members.getCount() * 16, 0, createDebugLine, 
				// 	%this.getGridPosition(%nx, %ny, %nz),
				// 	%this.getGridPosition(%test.x, %test.y, %test.z),
				// 	"0 1 0 1", 10000, 0.01
				// );

				schedule(%i * 16, 0, createDebugCube,
					%this.getGridPosition(%nx, %ny, %nz), "2 2 2",
					"0.6 0.4 0.2 0.1", 20000, 0.01
				);
			}
			else
				%borders.add(%neighbor);
		}
	}

	if (%members.getCount() == 1)
	{
		%members.deleteAll();
		%members.delete();

		%borders.deleteAll();
		%borders.delete();

		return 0;
	}

	%group = AirGroup();

	for (%i = %members.getCount() - 1; %i >= 0; %i--)
	{
		%member = %members.getObject(%i);
		%parent = %member.parent;

		if (!isObject(%parent))
			continue;
		continue;

		%factor = %member.depth / %depth;
		%color = 1 - %factor SPC %factor SPC "0 1";

		// echo("creating line at depth " @ %member.depth);
		// findlocalclient().player.settransform(%this.getGridPosition(%member.x, %member.y, %member.z));

		createDebugLine(
			%this.getGridPosition(%member.x, %member.y, %member.z),
			%this.getGridPosition(%parent.x, %parent.y, %parent.z),
			%color, 10000
		);
	}

	for (%i = %members.getCount() - 1; %i >= 0; %i--)
	{
		%member = %members.getObject(%i);
		%group.addMember(%member.x, %member.y, %member.z);
		%member.delete();
	}

	for (%i = %borders.getCount() - 1; %i >= 0; %i--)
	{
		%border = %borders.getObject(%i);
		%group.addBorder(%border.x, %border.y, %border.z);
		%border.delete();
	}

	return %group;
}