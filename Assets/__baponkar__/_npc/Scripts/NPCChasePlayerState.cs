using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace baponkar.npc.zombie
{
    public class NPCChasePlayerState : NPCState
    {
        float timer;
        public NPCStateId GetId()
        {
            return NPCStateId.ChasePlayer;
        }

        void NPCState.Enter(NPCAgent agent)
        {
            agent.playerSeen = true;
            //NavMeshAgent variables controls
            agent.navMeshAgent.isStopped = false;
            agent.navMeshAgent.speed = agent.config.chaseSpeed;
            agent.navMeshAgent.acceleration = agent.config.chaseAcceleration;
            //agent.navMeshAgent.stoppingDistance = agent.config.attackRadius;
            //agent.animator.Play("Zombie Scream");
        }

        void NPCState.Exit(NPCAgent agent)
        {
            agent.navMeshAgent.stoppingDistance = 0.0f;
            agent.navMeshAgent.isStopped = false;
        }

        void NPCState.Update(NPCAgent agent)
        {
           ChasePlayer(agent);
        }

        private static void ChasePlayer(NPCAgent agent)
        {
            if(agent.targetingSystem.HasTarget)
            {
                float distance = Vector3.Distance(agent.targetingSystem.TargetPosition, agent.transform.position);
                if (distance > agent.config.attackRadius)
                {
                    agent.animator.SetBool("isAttacking", false);
                    agent.navMeshAgent.destination = agent.targetingSystem.TargetPosition;
                }
                else
                {
                    agent.stateMachine.ChangeState(NPCStateId.Attack);
                }
            }
            else
            {
                agent.stateMachine.ChangeState(NPCStateId.Patrol);
                agent.playerSeen = false;
            }
            
            
            
        }
    }
}