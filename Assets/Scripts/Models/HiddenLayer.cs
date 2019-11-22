using System.Linq;

namespace Models
{
    public class HiddenLayer
    {
        public Perceptron[] Neurons;
        public int Count { get => this.Neurons.Length; }
        public HiddenLayer(string str)
        {
            var values = str.Split(',');
            // fill all layers
            for (int i = 0; i < this.Neurons.Length; i++)
                this.Neurons[i].LoadWeights(values[i]);
        }
        public HiddenLayer(int previousNodeCount, int nodeCount)
        {
            this.Neurons = new Perceptron[nodeCount];
            for(int i = 0; i < this.Neurons.Length; i++)
            {
                this.Neurons[i] = new Perceptron(previousNodeCount);
            }

        }

        public int LoadWeights(int index, float[] weights)
        {
            for (int i = 0; i < this.Neurons.Length; i++)
            {
                if (weights.Length - this.Neurons[i].InputWeights.Length - index < 0)
                    throw new System.Exception($"current: {index}, weights:{weights.Length}");
                for (int j = 0; j < this.Neurons[i].InputWeights.Length; j++)
                {
                    this.Neurons[i].InputWeights[j] = weights[index];
                    index++;
                }
            }
            return index;
        }

        /// <summary>
        /// Get outputs for next layer.
        /// </summary>
        /// <param name="inputs">Output values from previous layer.</param>
        /// <returns></returns>
        public float[] GetOutputs(float[] inputs) => this.Neurons.Select(neuron => neuron.Output(inputs)).ToArray();
        public float[] GetWeights() => this.Neurons.SelectMany(neuron => neuron.InputWeights).ToArray();
    }
}

