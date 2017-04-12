using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using System.Linq;

    public class BoardManager : MonoBehaviour
    {
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class Count
        {
            public int minimum;             //Minimum value for our Count class.
            public int maximum;             //Maximum value for our Count class. 
            //Assignment constructor.
            public Count (int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }        
        public const int columns = 8;                                   //Number of columns in our game board.
        public const int rows = 8;                                      //Number of rows in our game board.
       
        public const int plainsMoveCost = 1;
        public const int mountMoveCost = 10; 
        public const int buildMoveCost = 12;

        public const int plainsSightCost = 1;
        public const int mountSightCost = 2;
        public const int buildSightCost = 4;

        public GameObject mine;                                         //Prefab for mine.
        public GameObject bank;                                         //Prefab for bank.
        public GameObject shack;                                        //Prefab for shack.
        public GameObject saloon;                                       //Prefab for saloon.
        public GameObject cemetary;                                     //Prefab for cemetary.
        public GameObject outlawCamp;                                   //Prefab for outlaw camp.
        public GameObject undertakersOffice;                            //Prefab for undertakers office.
        public GameObject sheriffsOffice;                               //Prefab for undertakers office.
        public GameObject mountain;                                     //Prefab for mountain.

        public GameObject bob;
        public GameObject jesse;
        public GameObject elsa;
        public GameObject sheriff;
        public GameObject undertaker;

        public GameObject[] mountains;                                   //Array of mountains
        public GameObject[] plains;                                      //Array of plains prefabs.
        public GameObject[] agentTiles;                                  //Array of agent prefabs.
        
        public Transform boardHolder; //A variable to store a reference to the transform of our Board object.

        public static Vector2 [] locationsPos= new Vector2[8];                          
        private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.
        //public static AgentManager agentManager;
        public static float[,] moveCosts = new float[columns,rows];
        public static float[,] sightPropogations = new float[columns,rows];

        //Clears our list gridPositions and prepares it to generate a new board.
        void InitialiseList ()
        {
            //Clear our list gridPositions.
            gridPositions.Clear ();
            
            //Loop through x axis (columns).
            for(int x = 1; x < columns-1; x++)
            {
                //Within each column, loop through y axis (rows).
                for(int y = 1; y < rows-1; y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add (new Vector3(x, y, 0f));
                }
            }
        }
               
        //Sets up the outer walls and floor (background) of the game board.
        void BoardSetup ()
        {
            //Instantiate Board and set boardHolder to its transform.
            boardHolder = new GameObject ("Board").transform;           
            //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
            for(int x = 0; x < columns ; x++)
            {
                //Loop along y axis, starting from -1 to place floor or outerwall tiles.
                for(int y = 0; y < rows ; y++)
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = plains[Random.Range (0,plains.Length)];                                        
                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance =
                        Instantiate (toInstantiate, new Vector3 (x, y, toInstantiate.transform.position.z), Quaternion.identity) as GameObject;
                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent (boardHolder);
                    if((x>=0) && (y>=0) && (x<columns )&& (y<rows)){
                        moveCosts[x, y] = plainsMoveCost;
                        sightPropogations[x, y] = plainsSightCost; 
                    }   
                }
            }
        }
                
        //RandomPosition returns a random position from our list gridPositions.
        Vector3 RandomPosition ()
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range (0, gridPositions.Count);            
            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];
            //Remove the entry at randomIndex from the list so that it can't be re-used.
            gridPositions.RemoveAt (randomIndex);
            //Return the randomly selected Vector3 position.
            return randomPosition;
        }
        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        List<GameObject> LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
        {
            var result = new List<GameObject> ();
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            int objectCount = Random.Range (minimum, maximum+1);            
            //Instantiate objects until the randomly chosen limit objectCount is reached
            for(int i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                Vector3 randomPosition = RandomPosition();                
                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
                // Preserve original Z
                randomPosition.z = tileChoice.transform.position.z;                   
                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                result.Add((GameObject)Instantiate(tileChoice, randomPosition, Quaternion.identity));
            }
            return result;
        }

        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene()
        {
            //Creates the outer walls and floor.
            BoardSetup();
            //Reset our list of gridpositions.
            InitialiseList();

            //Instantiate the prefabs for the tiles
            mine = (GameObject)Instantiate(mine, RandomPosition(), Quaternion.identity);
            bank = (GameObject)Instantiate(bank, RandomPosition(), Quaternion.identity);
            shack = (GameObject)Instantiate(shack, RandomPosition(), Quaternion.identity);
            saloon = (GameObject)Instantiate(saloon, RandomPosition(), Quaternion.identity);
            cemetary = (GameObject)Instantiate(cemetary, RandomPosition(), Quaternion.identity);
            outlawCamp = (GameObject)Instantiate(outlawCamp, RandomPosition(), Quaternion.identity);
            sheriffsOffice = (GameObject)Instantiate(sheriffsOffice, RandomPosition(), Quaternion.identity);            
            undertakersOffice = (GameObject)Instantiate(undertakersOffice, RandomPosition(), Quaternion.identity);

            bob = (GameObject)Instantiate(bob, shack.transform.position, Quaternion.identity);
            elsa = (GameObject)Instantiate(elsa, shack.transform.position, Quaternion.identity);
            jesse = (GameObject)Instantiate(jesse, outlawCamp.transform.position, Quaternion.identity);
            undertaker = (GameObject)Instantiate(undertaker, undertakersOffice.transform.position, Quaternion.identity);
            sheriff = (GameObject)Instantiate(sheriff, sheriffsOffice.transform.position, Quaternion.identity);

            mountains = LayoutObjectAtRandom (new GameObject[] { mountain }, 9, 10).ToArray();
            mountains.ToList().ForEach((mountainTile) => {
                var pos = mountainTile.transform.position;
                moveCosts[(int)pos.x, (int)pos.y] = mountMoveCost;
                sightPropogations[(int)pos.x, (int)pos.y] = mountSightCost;
            });

            new GameObject[] { mine, bank, shack, saloon, cemetary, outlawCamp, sheriffsOffice ,undertakersOffice }.ToList ().ForEach ((building) => {
                var pos = building.transform.position;
                moveCosts[(int)pos.x, (int)pos.y] = buildMoveCost;
                sightPropogations[(int)pos.x, (int)pos.y] = buildSightCost;
            });
            //load in prefab positions into array at indexes given by the location from Locations enum.
            locationsPos[(int)Locations.Bank]=bank.transform.position;
            locationsPos[(int)Locations.Shack]=shack.transform.position;
            locationsPos[(int)Locations.Saloon]=saloon.transform.position;
            locationsPos[(int)Locations.Cemetary]=cemetary.transform.position;
            locationsPos[(int)Locations.OutlawCamp]=outlawCamp.transform.position;
            locationsPos[(int)Locations.SheriffsOffice]=sheriffsOffice.transform.position;
            locationsPos[(int)Locations.UndertakersOffice]=undertakersOffice.transform.position;
            locationsPos[(int)Locations.Mine]=mine.transform.position;

            Debug.Log("<color=red> </color>" + moveCosts);
           
        }


        public static Grid getGrid ()
        {
            Grid grid = new Grid(rows, columns,  moveCosts);
            return grid;
        }

        public static Grid getSenseGrid ()
        {
            Grid grid = new Grid(rows, columns,  sightPropogations);
            return grid;
        }
    }