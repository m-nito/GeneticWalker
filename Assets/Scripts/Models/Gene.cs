using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Gene
    {
        public int[] Metadata;
        public float[] Chromosomes;

        public Gene(int[] meta, float[] chromosomes)
        {
            this.Metadata = meta;
            this.Chromosomes = chromosomes;
        }

        /// <summary>
        /// Gets an offspring with given single crossover point.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="crossover"></param>
        /// <returns></returns>
        private static Gene GetOffspring(Gene a, Gene b, int crossover) =>
            new Gene(a.Metadata, Enumerable.Range(0, a.Chromosomes.Length).Select(i => i < crossover ? a.Chromosomes[i] : b.Chromosomes[i]).ToArray());

        /// <summary>
        /// Gets offsprings with given single crossover point.
        /// Selects random crossover point when given crossover was zero.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="crossover"></param>
        /// <returns></returns>
        public static (Gene a, Gene b) GetOffsprings(Gene a, Gene b, int crossover = 0)
        {
            if (crossover == 0) crossover = Rand.GetRandom(0, a.Chromosomes.Length);
            return (GetOffspring(a, b, crossover), GetOffspring(b, a, crossover));
        }

        private List<int> GetMutationIndice(int mutationCount = 0)
        {
            var indice = new List<int>();
            while (indice.Count() < mutationCount)
            {
                var index = Rand.GetRandom(0, this.Chromosomes.Length);
                if (indice.Contains(index)) continue;
                indice.Add(index);
            }
            return indice;
        }
    }
}
