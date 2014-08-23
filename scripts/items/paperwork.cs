datablock itemData(paperItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/paper.dts";
	image = paperImage;
	uiName = "Paper";
	doColorShift = false;
	colorShiftColor = "0.6 0.6 0.6 1";
	canDrop = true;
};

datablock ShapeBaseImageData(paperImage)
{
	shapeFile = $SS::Path @ "shapes/items/paper.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = paperItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = paperItem.doColorShift;
	colorShiftColor = paperItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.01;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "root";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Activate";
	stateAllowImageChange[1]		= true;

	stateName[3]					= "Activate";
	stateTransitionOnTriggerup[3]	= "Ready";
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Activate";
	stateScript[3]					= "onActivate";
	stateWaitForTimeout[3]			= true;
};

datablock itemData(BlackPenItem : hammerItem)
{
	shapeFile = $SS::Path @ "shapes/items/pen.dts";
	image = BlackPenImage;
	uiName = "Black Pen";
	doColorShift = true;
	colorShiftColor = "0 0 0 1";
	canDrop = true;
};

datablock ShapeBaseImageData(BlackPenImage)
{
	shapeFile = $SS::Path @ "shapes/items/pen.dts";
	emap = true;

	mountPoint = 0;
	// offset = "0 0 0.2";
	// rotation = eulerToMatrix("0 0 180");
	eyeOffset = "0 0 0";
	correctMuzzleVector = false;
	className = "WeaponImage";

	item = BlackPenItem;
	ammo = " ";
	projectile = SwordProjectile;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;
	armReady = true;

	doColorShift = BlackPenItem.doColorShift;
	colorShiftColor = BlackPenItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.01;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "root";
	stateSound[0]					= "";

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Activate";
	stateAllowImageChange[1]		= true;

	stateName[3]					= "Activate";
	stateTransitionOnTriggerup[3]	= "Ready";
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Activate";
	stateScript[3]					= "onActivate";
	stateWaitForTimeout[3]			= true;
};

datablock itemData(BluePenItem : BlackPenItem)
{
	uiName = "Blue Pen";
	image = BluePenImage;
	colorShiftColor = "0 0 1 1";
};

datablock ShapeBaseImageData(BluePenImage : BlackPenImage)
{
	item = BluePenItem;
	doColorShift = BluePenItem.doColorShift;
	colorShiftColor = BluePenItem.colorShiftColor;
};

datablock itemData(RedPenItem : BlackPenItem)
{
	uiName = "Red Pen";
	image = RedPenImage;
	colorShiftColor = "1 0 0 1";
};

datablock ShapeBaseImageData(RedPenImage : BlackPenImage)
{
	item = RedPenItem;
	doColorShift = RedPenItem.doColorShift;
	colorShiftColor = RedPenItem.colorShiftColor;
};

datablock itemData(GreenPenItem : BlackPenItem)
{
	uiName = "Green Pen";
	image = GreenPenImage;
	colorShiftColor = "0 1 0 1";
};

datablock ShapeBaseImageData(GreenPenImage : BlackPenImage)
{
	item = GreenPenItem;
	doColorShift = GreenPenItem.doColorShift;
	colorShiftColor = GreenPenItem.colorShiftColor;
};