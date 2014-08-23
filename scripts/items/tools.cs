datablock itemData(crowbarItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/crowbar.dts";
	image = crowbarImage;
	uiName = "Crowbar";
	doColorShift = true;
	colorShiftColor = "1 0 0 1";
	canDrop = true;
};

datablock ShapeBaseImageData(crowbarImage)
{
	shapeFile = $SS::Path @ "shapes/items/crowbar.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = crowbarItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 5;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = crowbarItem.doColorShift;
	colorShiftColor = crowbarItem.colorShiftColor;

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


datablock itemData(screwdriverItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/screwdriver.dts";
	image = screwdriverImage;
	uiName = "Screwdriver";
	doColorShift = true;
	colorShiftColor = "0 0 0.5 1";
	canDrop = true;
};

datablock ShapeBaseImageData(screwdriverImage)
{
	shapeFile = $SS::Path @ "shapes/items/screwdriver.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = screwdriverItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 6;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = screwdriverItem.doColorShift;
	colorShiftColor = screwdriverItem.colorShiftColor;

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


datablock itemData(wirecuttersItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/wirecutters.dts";
	image = wirecuttersImage;
	uiName = "Wirecutters";
	doColorShift = true;
	colorShiftColor = "0.5 0 0 1";
	canDrop = true;
};

datablock ShapeBaseImageData(wirecuttersImage)
{
	shapeFile = $SS::Path @ "shapes/items/wirecutters.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = wirecuttersItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 5;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = wirecuttersItem.doColorShift;
	colorShiftColor = wirecuttersItem.colorShiftColor;

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


datablock itemData(ssWrenchItem : hammerItem)
{
	shapeFile = "base/data/shapes/wrench.dts";
	image = ssWrenchImage;
	uiName = "Engineering Wrench";
	doColorShift = true;
	colorShiftColor = "0.25 0.25 0.25 1";
	canDrop = true;
};

datablock ShapeBaseImageData(ssWrenchImage)
{
	shapeFile = "base/data/shapes/wrench.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = ssWrenchItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 5;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = ssWrenchItem.doColorShift;
	colorShiftColor = ssWrenchItem.colorShiftColor;

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


datablock itemData(welderItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/welder.dts";
	image = welderImage;
	uiName = "Welding Tool";
	doColorShift = true;
	colorShiftColor = "0.5 0.5 0.5 1";
	canDrop = true;
};

datablock ShapeBaseImageData(welderImage)
{
	shapeFile = $SS::Path @ "shapes/items/welder.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = welderItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	raycastEnabled = 1;
	raycastRange = 5;
	raycastHitExplosion = hammerProjectile;
	// raycastHitPlayerExplosion = "";

	directDamage = 1;
	directDamageType = $DamageType::Direct;

	raycastCount = 1;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = welderItem.doColorShift;
	colorShiftColor = welderItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.01;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[3]					= "Fire";
	stateTransitionOnTimeout[3]		= "Check";
	stateTimeoutValue[3]			= 0.01;
	stateFire[3]					= true;
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Fire";
	stateScript[3]					= "onFire";
	stateWaitForTimeout[3]			= true;

	stateName[5]					= "Check";
	stateTransitionOnTriggerUp[5]	= "Ready";
	stateTransitionOnTimeout[5]		= "Fire";
	stateTimeoutValue[5]			= 0.01;
	stateAllowImageChange[5]		= false;
	stateWaitForTimeout[5]			= true;
	// stateSequence[5]				= "StopFire";
	stateScript[5]					= "onCheck";
};