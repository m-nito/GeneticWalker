using System.Linq;

namespace Models
{
    public class Perceptron
    {
        public float[] InputWeights;
        public float Bias;

        public Perceptron(int nodeCount)
        {
            this.InputWeights = new float[nodeCount];
            this.SetRandomWeights();
        }
        /// <summary>
        /// Loads weights for this neuron from genetic code.
        /// </summary>
        /// <param name="input"></param>
        public void LoadWeights(string input)
        {
            var inputs = input.Split(',').Select(x => float.Parse(x)).ToArray();
            for (int i = 0; i < this.InputWeights.Length; i++)
            {
                this.InputWeights[i] = inputs[i];
            }
        }
        public void SetRandomWeights()
        {
            for (int i = 0; i < this.InputWeights.Length; i++)
            {
                this.InputWeights[i] = Rand.GetRandomFloat();
            }
        }

        public float Output(float[] inputs) => Activation.ReLU(this.WeightedInput(inputs) + this.Bias);

        /// <summary>
        /// Returns sigma(w * x)
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private float WeightedInput(float[] inputs) => Enumerable.Range(0, inputs.Length).Select(i => inputs[i] * this.InputWeights[i]).Sum();


    }
}

