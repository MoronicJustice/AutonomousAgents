using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Based on code from this tutorial: https://www.youtube.com/watch?v=mZfyt03LDH4
*/
    public class Node
    {
        // node starting params
        public int gridX;
        public int gridY;
        public float penalty;

        // calculated values while finding path
        public int gCost;
        public int hCost;
        public Node parent;

        // create the node
        public Node(float tileCost, int theGridX, int theGridY)
        {
            penalty = tileCost;
            gridX = theGridX;
            gridY = theGridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
}
