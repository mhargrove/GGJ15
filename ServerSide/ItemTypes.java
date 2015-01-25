public enum ItemTypes{
	BED("BED"), 
	FLOWER("FLOWER"),
	MEDKIT("MEDKIT"), 
	NYQUIL("NYQUIL"),
	ADDERAL("ADDERAL"), 
	FOOD("FOOD"), 
	COIN("COIN"), 
	BOOKS("BOOKS");

	public final String netName;
	ItemTypes(String name){
		this.netName = name;
	}

	//game random, no beds or other static items
	public static ItemTypes getNonStaticRandom(){
		double val = Math.random();
		if(val > 0 && val <= 0.14f){return COIN;}
		else if(val > 0.14f && val <= 0.28f){return FLOWER;}
		else if(val > 0.28f && val <= 0.42f){return MEDKIT;}
		else if(val > 0.42f && val <= 0.56f){return NYQUIL;}
		else if(val > 0.56f && val <= 0.70f){return ADDERAL;}
		else if(val > 0.70f && val <= 0.84f){return FOOD;}
		else if(val > 0.84f){return BOOKS;}
		return BOOKS; //never gonna happen
	}
}