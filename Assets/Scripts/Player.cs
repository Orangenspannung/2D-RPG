using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Allows us to use SceneManager

    //Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MovingObject
    {
        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level. 
        private Animator animator;                  //Used to store a reference to the Player's animator component.
        private int food;                           //Used to store player food points total during level.
        
        
        //Start overrides the Start function of MovingObject
        protected override void Start ()
        {
            //Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();
            
            //Get the current food point total stored in GameManager.instance between levels.
            food = 1;
            
            //Call the Start function of the MovingObject base class.
            base.Start ();
        }
        
        private void Update ()
        {
            //If it's not the player's turn, exit the function.
            if(!GameManager.instance.playersTurn) return;
            
            int horizontal = 0;     //Used to store the horizontal move direction.
            int vertical = 0;       //Used to store the vertical move direction.
            
            
            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
            
            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int) (Input.GetAxisRaw ("Vertical"));
            
            //Check if moving horizontally, if so set vertical to zero.
            if(horizontal != 0)
            {
                vertical = 0;
            }
            
            //Check if we have a non-zero value for horizontal or vertical
            if(horizontal != 0 || vertical != 0)
            {
                //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                AttemptMove(horizontal, vertical);
            }
        }
        
        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove (int xDir, int yDir)
        {          
            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove (xDir, yDir);
            
            //Hit allows us to reference the result of the Linecast done in Move.
            RaycastHit2D hit;
            
            //If Move returns true, meaning Player was able to move into an empty space.
            if (Move (xDir, yDir, out hit)) 
            {
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
            }
            
            //Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver ();
        }
        
        
        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D (Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if(other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke ("Restart", restartLevelDelay);
                
                //Disable the player object since level is over.
                enabled = false;
            }
        }
        
        
        //Restart reloads the scene when called.
        private void Restart ()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game.
            SceneManager.LoadScene (0);
        }
        
        
        //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
        private void CheckIfGameOver ()
        {
            //Check if food point total is less than or equal to zero.
            if (food <= 0) 
            {
                
                //Call the GameOver function of GameManager.
                GameManager.instance.GameOver ();
            }
        }
    }