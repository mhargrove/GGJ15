public class Item{
	public ItemTypes type;
	public int posX;
	public int posY;
	public Item(ItemTypes name, int x, int y){
		this.type = name;
		this.posX = x;
		this.posY = y;
	}

	public String toString(){
		return type.netName+"@"+"["+posX+","+posY+"]";
	}

	public Stats apply(Stats in){
		Stats result = in;
		switch(type){
			case BED: 
				result.sleepy = 0;
				result.timeDelta += 30;
				break;
			case FLOWER: 
				result.romance += 15;
				result.cash -= 10;
				break;
			case MEDKIT: 
				result.health += 40;
				result.cash -= 25;
				break;
			case NYQUIL: 
				result.health -= 15;
				result.cash -= 5;
				result.sleepy -= 60;
				result.timeDelta += 15;
				break;
			case ADDERAL: 
				result.sleepy += 50;
				result.cash -= 30;
				result.health -= 10;
				result.moveFastFor = 10;
				break;
			case FOOD: 
				result.health += 20;
				result.cash -= 10;
				result.hungry += 80;
				break; 
			case COIN: 
				result.cash += 25;
				break;
			case BOOKS: 
				if(result.hungry > 50){
					if(result.hungry > 75){
						result.study += 15;
					}
					else{
						result.study += 10;
					}
				}
				result.study += 7;
				if(result.sleepy > 50){
					if(result.sleepy > 75){
						result.study -= 6;
					}
					else{
						result.study -= 2;
					}
				}
				break;
		}
		return result;
	}
}