using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class TrialBehaviour : MonoBehaviour
{
    static int trialCount = 0;

    private AgentBehaviour[] Agents;
    private Dictionary<Gene, int> Records = new Dictionary<Gene, int>();
    private void Start()
    {
        this.Agents = GetComponentsInChildren<AgentBehaviour>();
        foreach (var agent in this.Agents) agent.OnFinish += this.OnAgentFinish;
    }

    public void OnAgentFinish(AgentBehaviour agent)
    {
        agent.OnFinish -= this.OnAgentFinish;
        // if (trialCount > 10000) return;
        var result = agent.GetResult();
        this.Records.Add(result.gene, result.score);
        var pos = agent.StartingPosition;
        var next = Instantiate(Resources.Load("Prefabs/Capsule"), this.transform) as GameObject;
        next.transform.position = pos;
        var nextModel = next.GetComponent<AgentBehaviour>();
        nextModel.OnFinish += this.OnAgentFinish;
        if (Records.Count > 2)
        {
            Gene first = null;
            Gene second = null;
            foreach(var v in Records.OrderByDescending(x => x.Value))
            {
                if (first == null)
                {
                    first = v.Key;
                    continue;
                }
                second = v.Key;
            }
            var offspring = Gene.GetOffsprings(first, second);
            nextModel.Model = new NeuralNetwork(offspring.a);
        }
        Destroy(agent.gameObject);

        trialCount++;
    }

    private void Update()
    {
        
    }
}
