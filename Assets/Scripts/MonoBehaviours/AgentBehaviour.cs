using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    public int Score;
    public NeuralNetwork Model;
    public Action<AgentBehaviour> OnFinish;

    public Vector3 StartingPosition;
    public bool Ready = false;
    private HeadBehaviour Head;
    private LegBehaviour[] Legs;


    /// <summary>
    /// Current stage of motion. 0 - 1f.
    /// </summary>
    private float _MotionStage = 0f;

    private void Start()
    {
        this.StartingPosition = this.transform.position;
        this.Head = this.GetComponentInChildren<HeadBehaviour>();
        this.Legs = this.GetComponentsInChildren<LegBehaviour>();
        

        // initializes own neural network model.
        this.Model = new NeuralNetwork(
            inputCount: 4,
            hiddenLayers: new int[2] { 4, 4 },
            outputCount: 2);
    }

    private void Update()
    {
        var first = this.Legs[0];
        var second = this.Legs[1];

        // set current stage of motion.
        _MotionStage = this._MotionStage > 1f ? 0f : _MotionStage + 0.1f;

        // get current state of agent.
        var x = this.gameObject.transform.rotation.x;
        var y = this.gameObject.transform.rotation.y;
        var z = this.gameObject.transform.rotation.z;

        // fill inputs.
        var inputs = new float[] { _MotionStage, x, y, z };

        // calculate output.
        var outputs = this.Model.GetOutput(inputs);

        // Move agent
        if (_MotionStage > 0.5f)
        {
            first.Forward(outputs[0]);
            second.Backward(outputs[1]);
        }
        else
        {
            first.Backward(outputs[0]);
            second.Forward(outputs[1]);
        }

        // Debug.Log($"{outputs[0]},{outputs[1]}");

        if (this.Head.Touched)
        {
            this.Score = (int)( (this.transform.position.z - this.StartingPosition.z) * 10);
            this.OnFinish?.Invoke(this);
            Debug.Log($"Atempt ended. Score: {this.Score}");
            return;
        }
    }
    public (int score, Gene gene) GetResult() => (score: (int)this.gameObject.transform.position.z, this.Model.ToGene());
}
