using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class AvatarScript : MonoBehaviour {
	// movement property references
    public float moveSpeed = 6.0F;
	public float rotateSpeed = 100.0f;
    public float jumpSpeed = 8.0F;
	public float gravity = 10.0F;
	public float windSlow = 0.2f;
	
	// two curves for attack/decay
	public AnimationCurve groundAttack;
	public AnimationCurve groundDecay;
	
	// our movement vector
	private Vector3 moveVector = Vector3.zero;
	//private Vector3 moveDirection;
	
	// Animation references:  if you want to control the kids and do something with them
	GameObject head;
	GameObject body;
		
	// initial position
	Vector3 initialPosition;
	Quaternion initialRotation;

	// variables for our state machine
    float inputRotate;
    float inputMove;
    Vector3 horizontalVelocity;
    float horizontalSpeed;  
    float verticalSpeed;
	
	// our character's controller
	CharacterController controller;
	CollisionFlags collisionFlags;
	bool isGrounded;
	int previousX, previousY;

	void Start (){
		// must have both!
		controller = gameObject.GetComponent<CharacterController>(); 
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		
		initialPosition = ConvertLocation.ConvertToReal(GameEngine.player.LocationX, transform.localPosition.y, GameEngine.player.LocationY);
		initialRotation = transform.rotation;
			
     	// Assign body parts to variables;  
		// -> could also have these as properties you set in editor
		// -> could also have used Transform.Find to only search in the children of this object
	 	head = GameObject.Find ("Head");
     	body = GameObject.Find ("Body");
		
		// so we can go outside the bounds
		groundAttack.preWrapMode = WrapMode.ClampForever;
		groundAttack.postWrapMode = WrapMode.ClampForever;
		groundDecay.preWrapMode = WrapMode.ClampForever;
		groundDecay.postWrapMode = WrapMode.ClampForever;
		
		gameObject.SetActive(false);	
	}
	
	void GameStart() {
		gameObject.SetActive(true);	
		previousX = GameEngine.player.LocationX;
		previousY = GameEngine.player.LocationY;
		//Debug.Log (GameMap.GameMapArray(
		//previousX = 50;
		//previousY = 39;
		//GameEngine.setPlayerPosition((GameEngine.guards[0].LocationX + 1), (GameEngine.guards[0].LocationY+1));
		transform.position = ConvertLocation.ConvertToReal(previousX, transform.localPosition.y, previousY);
		transform.rotation = initialRotation;		
		
		collisionFlags = CollisionFlags.None;
		isGrounded = false;
		inputRotate = 0f;
        inputMove = 0f;
       	horizontalVelocity = Vector3.zero;
        horizontalSpeed = 0f;
        verticalSpeed = 0f;

		moveVector = Vector3.zero;
		GameObject.Find("TextureMaking").GetComponent<ControlPlane>().makeRoute(previousX, previousY);
	}
	
	void GameOver() {
		// won't be called
		//gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
	// Main update loop!
	void Update() {

		// change states if need be
		if (!isGrounded) {
			// no rotation while jumping.  transform.Rotate(0f, inputRotate * rotateSpeed / 2f, 0f);
	        Vector3 vec = new Vector3(0f, 0f, inputMove * moveSpeed);
			vec = transform.TransformDirection(vec);
			moveVector +=  vec * Time.deltaTime;
			
			// slow the movement vector down a bit (wind resistance?) in the x/z plane
			vec = new Vector3(moveVector.x, 0f, moveVector.z);
			moveVector -= vec * windSlow * Time.deltaTime;
				
			// if we're on the ground, and are "inside" whatever we are on, move "almost" out.  If we are 
			// in the air, apply some gravity
			moveVector.y -= gravity * Time.deltaTime;	
			collisionFlags = controller.Move(moveVector * Time.deltaTime);	
		}else{
			Vector3 tempC = controller.velocity;
	       	horizontalVelocity = new Vector3(tempC.x, 0, tempC.z);
			horizontalVelocity = transform.InverseTransformDirection(horizontalVelocity);
	        horizontalSpeed = horizontalVelocity.z;  // want the plus or minus on speed
	        verticalSpeed = tempC.y;
			// When we are walking, do something interesting that shows which is the direction of travel				
			if (horizontalSpeed > 0.1 ) { 	
				body.transform.localRotation = Quaternion.Euler (10,0,0);
			} else if (horizontalSpeed < -0.1) {
				body.transform.localRotation = Quaternion.Euler (-10,0,0);
			} else {
				body.transform.localRotation = Quaternion.Euler (0,0,0);
			}	
			inputRotate = Input.GetAxis("Horizontal");
			inputMove = Input.GetAxis ("Vertical");
			moveVector = body.transform.forward*moveSpeed*inputMove*Time.deltaTime;
			this.transform.Rotate(new Vector3(0,inputRotate*rotateSpeed*Time.deltaTime,0));
			collisionFlags = controller.Move(moveVector+new Vector3(0,-gravity*Time.deltaTime,0));
			Vector2 temp = ConvertLocation.ConvertTo2D(transform.localPosition);
			if(previousX != (int)temp.x || previousY != (int)temp.y){
				previousX = (int)temp.x; previousY = (int)temp.y;
				GameEngine.setPlayerPosition(previousX, previousY);
				GameObject.Find("TextureMaking").GetComponent<ControlPlane>().makeRoute(previousX, previousY);
			}
		}
		
		// check for restart
        if (Input.GetKeyDown(KeyCode.R)) 
		{
			GameEventManager.TriggerGameOver();
			GameEventManager.TriggerGameStart();
			return;
		}
		isGrounded = ((collisionFlags & CollisionFlags.CollidedBelow) != 0);
	}
}