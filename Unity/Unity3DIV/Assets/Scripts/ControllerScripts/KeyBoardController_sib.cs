using UnityEngine;
using System.Collections;

public class KeyBoardController_sib : MonoBehaviour {
	public GameObject playerCam; 
	public float moveStep = 0.5f;
	public float rotateStep = 2.0f;	
	
	//Similar to wiimote texture
	public Texture2D cursorImage;
	
	//Gui elementen op muis/wiimote
	public GUITexture baseGuiTexture;
	private GUITexture screenpointer;
	
	//scripts
	private RayCastScript raycastscript;
	private RotateScript rotateScript;
	private ScaleScript scaleScript;
	private StackScript stackScript;
	private MoveScript moveScript;
	
	//last object selected
	private GameObject lastGameObjectHit;
	
	// Use this for initialization
	void Start () {
		//Set ref script
		raycastscript = gameObject.GetComponent("RayCastScript") as RayCastScript;
		rotateScript = gameObject.GetComponent("RotateScript") as RotateScript;
		scaleScript = gameObject.GetComponent("ScaleScript") as ScaleScript;
		stackScript = gameObject.GetComponent("StackScript") as StackScript;		
		moveScript = gameObject.GetComponent("MoveScript") as MoveScript;
		
		//Turn off mouse pointer and set the cursorImage
		screenpointer = (GUITexture)Instantiate(baseGuiTexture);
		Screen.showCursor = false; 
		screenpointer.texture = cursorImage;
		screenpointer.color = Color.red;
		screenpointer.pixelInset = new Rect(10,10,10,10);
		screenpointer.transform.localScale -= new Vector3(1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {		
		updateNavigation();
		updateStackingManipulation();
		updateMovingManipulation();
				
		if (Input.GetButton("Fire1")){
			lastGameObjectHit = raycastscript.getTargetObjects(Input.mousePosition, playerCam.camera);
			if (lastGameObjectHit != rotateScript.clone){
				rotateScript.selectedObject = lastGameObjectHit;
				rotateScript.SetDrawFeedback(true);
			}
			//if (lastGameObjectHit != scaleScript.clone){
			//	scaleScript.selectedObject = lastGameObjectHit;
			//	scaleScript.SetDrawFeedback(true, "x");	
			//}
		}
		//rotate
		if (Input.GetKey("-"))
			rotateScript.RotateLeft();
		if (Input.GetKey("="))
			rotateScript.RotateRight();
		//scale
		/*if (Input.GetKey("["))
			scaleScript.ScaleXSmaller();
		if (Input.GetKey("]"))
			scaleScript.ScaleXBigger();
		if (Input.GetKey(";"))
			scaleScript.ScaleYSmaller();
		if (Input.GetKey("'"))
			scaleScript.ScaleYBigger();
			if (Input.GetKey("."))
			scaleScript.ScaleZSmaller();
		if (Input.GetKey("/"))
			scaleScript.ScaleZBigger();*/
				
		
		//Set the gui shizzle
		Vector3 mousePos= Input.mousePosition;
		float mouseX = mousePos.x/Screen.width;
		float mouseY = mousePos.y/Screen.height;
		//Debug.Log(mouseX + "\t" + mouseY);
		//screenpointer.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
		//screenpointer.transform.position = new Vector3(mouseX, mouseY, 0);
		//screenpointer.transform.position = mouseY;
		//Rect cursloc = new Rect(mousePos.x, Screen.height - mousePos.y, cursorImage.width, cursorImage.height);
		//GUI.Label(cursloc, cursorImage);
	}
	
	private void updateNavigation(){
		if (Input.GetKey("left") && !stackScript.isActive && !moveScript.isActive)
			moveCameraLeft();
		if (Input.GetKey("right") && !stackScript.isActive && !moveScript.isActive)
			moveCameraRight();
		if (Input.GetKey("up") && !stackScript.isActive && !moveScript.isActive)
			moveCameraForward();
		if (Input.GetKey("down") && !stackScript.isActive && !moveScript.isActive)
			moveCameraBackward();
		if (Input.GetKey("g") && !stackScript.isActive && !moveScript.isActive)
			rotateCameraLeft();
		if (Input.GetKey("h") && !stackScript.isActive && !moveScript.isActive)
			rotateCameraRight();
	}
	
	private void updateStackingManipulation(){
		if (Input.GetKeyUp ("t")){
				TestScript script = (TestScript) GameObject.Find("InputController").GetComponent("TestScript");
				script.testStack(); // place an object -> skip selection step
		}
		if (Input.GetKeyUp ("p")){
				TestScript script = (TestScript) GameObject.Find("InputController").GetComponent("TestScript");
				script.testParentMove();
		}	
		if (Input.GetKeyUp ("m")){
				TestScript script = (TestScript) GameObject.Find("InputController").GetComponent("TestScript");
				script.testParentRotate();
		}	
		if (Input.GetKeyUp ("l")){
				ObjectScript script = (ObjectScript) GameObject.Find("Tafel").GetComponent("ObjectScript");
				script.detachChild(GameObject.Find("schaal1"));
		}	
			
		if(stackScript.isActive){
			if(stackScript.gridModus){
				if (Input.GetKeyUp("left"))
					stackScript.goToNextAvailablePositionLeft();	
				if (Input.GetKeyUp("right"))
					stackScript.goToNextAvailablePositionRight();
				if (Input.GetKeyUp("up"))
					stackScript.goToNextAvailablePositionTop();
				if (Input.GetKeyUp("down"))
					stackScript.goToNextAvailablePositionDown();
			}else{
				if (Input.GetKey("left"))
					stackScript.goToLeft();	
				if (Input.GetKey("right"))
					stackScript.goToRight();
				if (Input.GetKey("up"))
					stackScript.goToTop();
				if (Input.GetKey("down"))
					stackScript.goToBottom();
			}
			
				
			if (Input.GetKeyUp ("y") && stackScript.gridModus)
				stackScript.goToNextAvailablePosition(); // scrolling  left-right, bottom-up
			if (Input.GetKeyUp ("u"))
				stackScript.goToNextPossibleStackedObject(); // scrolling between possible object
			if (Input.GetKeyUp ("i"))
				stackScript.Abort(); // abort this manipulation
			if (Input.GetKeyUp ("o"))
				stackScript.End(); // end this manipulation
		}		
	}
	
	private void updateMovingManipulation(){
		if (Input.GetKeyUp ("k")){
			TestScript script = (TestScript) GameObject.Find("InputController").GetComponent("TestScript");
			script.testMove();
		}		
		if (Input.GetKeyUp ("j")){
				TestScript script = (TestScript) GameObject.Find("InputController").GetComponent("TestScript");
				script.testChangeParent(); 
		}
			
		if(moveScript.isActive){
			if(moveScript.gridModus){
				if (Input.GetKeyUp("left"))
					moveScript.goToNextAvailablePositionLeft();	
				if (Input.GetKeyUp("right"))
					moveScript.goToNextAvailablePositionRight();
				if (Input.GetKeyUp("up"))
					moveScript.goToNextAvailablePositionTop();
				if (Input.GetKeyUp("down"))
					moveScript.goToNextAvailablePositionDown();
			}else{
				if (Input.GetKey("left"))
					moveScript.goToLeft();	
				if (Input.GetKey("right"))
					moveScript.goToRight();
				if (Input.GetKey("up"))
					moveScript.goToTop();
				if (Input.GetKey("down"))
					moveScript.goToBottom();
			}
			
				
			if (Input.GetKeyUp ("y") && moveScript.gridModus)
				moveScript.goToNextAvailablePosition(); // scrolling  left-right, bottom-up
			if (Input.GetKeyUp ("o"))
				moveScript.End(); // end this manipulation
		}		
	}
	
	//COPYPASTA
	private void moveCameraLeft(){
		Debug.Log("Move camera left");	
		
		Vector3 cameraRelative = playerCam.transform.TransformDirection (-moveStep,0,0);		
		cameraRelative.y = 0;
		playerCam.transform.localPosition += cameraRelative;
	}	
	private void moveCameraRight(){
		Debug.Log("Move camera right");
		
		Vector3 cameraRelative = playerCam.transform.TransformDirection (moveStep,0,0);
		cameraRelative.y = 0;		
		playerCam.transform.localPosition += cameraRelative;
	}	
	private void moveCameraForward(){
		Debug.Log("Move camera forward");
		
		Vector3 cameraRelative = playerCam.transform.TransformDirection (0,0,moveStep);	
		cameraRelative.y = 0;
		playerCam.transform.localPosition += cameraRelative;
	}	
	private void moveCameraBackward(){
		Debug.Log("Move camera backward");
		
		Vector3 cameraRelative = playerCam.transform.TransformDirection (0,0,-moveStep);	
		cameraRelative.y = 0;		
		playerCam.transform.localPosition += cameraRelative;
	}
	
	private void rotateCameraLeft()	{
		Debug.Log("Rotate camera left");
		
		playerCam.transform.RotateAround(playerCam.transform.position, Vector3.up, -rotateStep);
	}	
	private void rotateCameraRight(){
		Debug.Log("Rotate camera right");
		
		playerCam.transform.RotateAround(playerCam.transform.position, Vector3.up, rotateStep);
	}	
	private void rotateCameraUp(){
		Debug.Log("Rotate camera up");
		
		playerCam.transform.RotateAround(playerCam.transform.position, Vector3.right, rotateStep);
	}	
	private void rotateCameraDown()	{
		Debug.Log("Rotate camera down");
		
		playerCam.transform.RotateAround(playerCam.transform.position, Vector3.right, -rotateStep);
	}		
}
