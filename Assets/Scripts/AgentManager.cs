using System;
using System.Collections.Generic;
using UnityEngine;

    public static class AgentManager
    {
        public static  Agent [] agentList  =new Agent[5];

        public  static void AddAgent(Agent agent, int id )
        {
            agentList[id] =agent;
        }

        public  static Agent GetAgent(int id)
        {
            return agentList[id];
        }
    }
