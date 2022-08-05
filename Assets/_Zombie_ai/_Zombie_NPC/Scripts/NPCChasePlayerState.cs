using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Baponkar.FPS.Simple;

namespace baponkar.npc.zombie
{
    public class NPCChasePlayerState : NPCState
    {
        
        float timer = 0.0f;

        public NPCStateId GetId()
        {
            return NPCStateId.ChasePlayer;
        }

        void NPCState.Enter(NPCAgent agent)
        {
            agent.playerSeen = true;
            agent.isChaseing = true;
            agent.navMeshAgent.stoppingDistance = agent.config.attackRadius;
        }

        void NPCState.Exit(NPCAgent agent)
        {
            agent.isChaseing = false;
            agent.navMeshAgent.stoppingDistance = 0.0f;
        }

        void NPCState.Update(NPCAgent agent)
        {
            timer -= Time.deltaTime;
            if(agent.aiHealth.isDead)
            {
                agent.stateMachine.ChangeState(NPCStateId.Death);
            }
            else
            {
                if(!agent.navMeshAgent.hasPath){
                    //agent.navMeshAgent.speed = agent.config.chaseWalkingSpeed + agent.config.offsetChaseSpeed;
                    if(timer <= 0.0f){
                        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
                        timer = agent.config.waitTime;
                    }
                }
                else
                {
                    if(timer <= 0.0f)
                    {
                        ChasePlayer(agent);
                        timer = agent.config.waitTime;
                    }
                }
            }
        }

        private static void ChasePlayer(NPCAgent agent)
        {
            PlayerHealth playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
            float distance = Vector3.Distance(playerHealth.transform.position, agent.transform.position);

            if (distance > agent.config.attackRadius)
            {
                agent.animator.SetBool("isAttacking", false);
                agent.navMeshAgent.isStopped = false;
                //agent.navMeshAgent.speed = agent.config.chaseWalkingSpeed + agent.config.offsetChaseSpeed;
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }
            else
            {
                agent.navMeshAgent.isStopped = true;
                agent.stateMachine.ChangeState(NPCStateId.Attack);
            }
            
            if(playerHealth.isDead)
            {
                agent.stateMachine.ChangeState(NPCStateId.Patrol);
            }
        }
    }
}