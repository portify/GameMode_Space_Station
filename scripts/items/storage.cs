datablock itemData(boxItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/box.dts";
	image = boxImage;
	uiName = "Box";
	doColorShift = true;
	colorShiftColor = "0.6 0.6 0.6 1";
	canDrop = true;
};

datablock ShapeBaseImageData(boxImage)
{
	shapeFile = $SS::Path @ "shapes/items/box.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = boxItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 0;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = boxItem.doColorShift;
	colorShiftColor = boxItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.01;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[3]					= "Fire";
	stateTransitionOnTimeout[3]		= "StopFire";
	stateTimeoutValue[3]			= 0.3;
	stateFire[3]					= true;
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Fire";
	stateScript[3]					= "onFire";
	stateWaitForTimeout[3]			= true;

	stateName[5]					= "StopFire";
	stateTransitionOnTriggerUp[5]	= "Ready";
	stateTimeoutValue[5]			= 0.1;
	stateAllowImageChange[5]		= false;
	stateWaitForTimeout[5]			= true;
	stateSequence[5]				= "StopFire";
	stateScript[5]					= "onStopFire";
};

datablock itemData(toolboxItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/toolbox.dts";
	image = toolboxImage;
	uiName = "Toolbox";
	doColorShift = true;
	colorShiftColor = "0 0 1 1";
	canDrop = true;
};

datablock ShapeBaseImageData(toolboxImage)
{
	shapeFile = $SS::Path @ "shapes/items/toolbox.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	rotation = eulerToMatrix("90 0 0");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = toolboxItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 10;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = false;

	doColorShift = toolboxItem.doColorShift;
	colorShiftColor = toolboxItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.01;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "root";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[3]					= "Fire";
	stateTransitionOnTimeout[3]		= "StopFire";
	stateTimeoutValue[3]			= 0.3;
	stateFire[3]					= true;
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Fire";
	stateScript[3]					= "onFire";
	stateWaitForTimeout[3]			= true;

	stateName[5]					= "StopFire";
	stateTransitionOnTriggerUp[5]	= "Ready";
	stateTimeoutValue[5]			= 0.1;
	stateAllowImageChange[5]		= false;
	stateWaitForTimeout[5]			= true;
	stateSequence[5]				= "StopFire";
	stateScript[5]					= "onStopFire";
};