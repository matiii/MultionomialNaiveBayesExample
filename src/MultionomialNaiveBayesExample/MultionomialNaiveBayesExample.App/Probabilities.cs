using System.Collections.Generic;
using System.Linq;

namespace MultionomialNaiveBayesExample.App
{
    class Probabilities: List<Entries>
    {
        public double GetProbability(char @class, string word) => this.First(x => x.Class == @class && x.Word == word).Probability;
    }
}
