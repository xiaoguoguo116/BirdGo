var rainPrefab: GameObject;
var snowPrefab: GameObject;

var rainActive: boolean = true;
var snowActive: boolean = false;

function Update () 
{
	if(Input.GetMouseButtonDown(0))
	{
		if(rainActive)
		{
			rainActive = false;
			rainPrefab.SetActive(false);
			snowActive = true;
			snowPrefab.SetActive(true);
		}
		else
		{
			snowActive = false;
			snowPrefab.SetActive(false);
			rainActive = true;
			rainPrefab.SetActive(true);
		}	
	}	
}