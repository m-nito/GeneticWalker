using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class NeuralNetwork
    {
        private HiddenLayer InputLayer;
        private HiddenLayer[] HiddenLayers;
        private HiddenLayer OutputLayer;

        public NeuralNetwork(int inputCount, int[] hiddenLayers, int outputCount)
        {
            this.InputLayer = new HiddenLayer(inputCount, inputCount);
            this.HiddenLayers = new HiddenLayer[hiddenLayers.Length];
            int prevCount = inputCount;
            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                this.HiddenLayers[i] = new HiddenLayer(prevCount, hiddenLayers[i]);
                prevCount = this.HiddenLayers[i].Neurons.Length;
            }
            this.OutputLayer = new HiddenLayer(prevCount, outputCount);
        }

        /// <summary>
        /// Create neural network from genetic data.
        /// </summary>
        /// <param name="gene"></param>
        public NeuralNetwork(Gene gene)
        {
            var meta = gene.Metadata;

            // meta[0] is numbers of layers.
            int layerCount = meta.Length;

            // total count - inputlayer - outputlayer = numbers of hidden layer.
            this.HiddenLayers = new HiddenLayer[layerCount - 2];

            // records previous node count;
            int previous = 0;

            // layer data starts from Metadata[1].
            for (int i = 0; i < layerCount; i++)
            {
                // first one is always input layer.
                if (i == 0) this.InputLayer = new HiddenLayer(meta[i], meta[i]);
                // last one is always output layer.
                else if (i == layerCount - 1)
                    this.OutputLayer = new HiddenLayer(previous, meta[i]);
                // otherwise hidden layer.
                else this.HiddenLayers[i - 1] = new HiddenLayer(previous, meta[i]);
                previous = meta[i];
            }
            if (this.HiddenLayers.Any(x => x is null))
            {
                int ind = 0;
                foreach(var l in this.HiddenLayers)
                {
                    if (l == null) throw new Exception(ind.ToString());
                    ind++;

                }
            }

            // load weights from gene.
            int current = 0;
            var chromo = gene.Chromosomes;
            for (int i = 0; i < layerCount; i++)
            {
                if (i == 0) current =  this.InputLayer.LoadWeights(current, chromo);
                else if (i == layerCount - 1) current = this.OutputLayer.LoadWeights(current, chromo);
                else current = this.HiddenLayers[i - 1].LoadWeights(current, chromo);
            }
        }

        public IEnumerable<HiddenLayer> FromString(string multiLineString)
        {
            // Split by new line, while ignoring comment line.
            var layerCodes = multiLineString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => !x.StartsWith("//")).ToArray();
            var last = layerCodes.Length - 1;
            for (int i = 0; i < layerCodes.Length; i++)
            {
                // input layer
                if (i == 0) yield return new HiddenLayer(layerCodes[i]);
                // output layer
                else if (i == last) yield return new HiddenLayer(layerCodes[i]);
                // other
                else yield return new HiddenLayer(layerCodes[i]);
            }
        }

        public float[] GetOutput(float[] inputs)
        {
            // input layer.
            float[] current = this.InputLayer.GetOutputs(inputs);
            // hidden layers.
            for(int i = 0; i < this.HiddenLayers.Length; i++)
            {
                current = this.HiddenLayers[i].GetOutputs(current);
            }
            // output layer
            return this.OutputLayer.GetOutputs(current);
        }
        /// <summary>
        /// Returns genetic informaton.
        /// </summary>
        /// <returns></returns>
        public Gene ToGene()
        {
            var meta = new List<int> { this.InputLayer.Count };
            foreach(var hidden in this.HiddenLayers)
            {
                meta.Add(hidden.Count);
            }
            meta.Add(this.OutputLayer.Count);
            var inputChromo = this.InputLayer.GetWeights();
            var hiddenChromo = this.HiddenLayers.SelectMany(layer => layer.GetWeights());
            var outputChromo = this.OutputLayer.GetWeights();
            var chromosomes = inputChromo.Concat(hiddenChromo).Concat(outputChromo).ToArray();
            return new Gene(meta.ToArray(), chromosomes.ToArray());
        }
    }
}

