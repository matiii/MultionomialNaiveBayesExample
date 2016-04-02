using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrainingSet = System.Collections.Generic.IDictionary<string, char>;

namespace MultionomialNaiveBayesExample.App
{
    class NaiveBayesHelper
    {
        public char[] GetDistinctClasses(TrainingSet training) => training.Select(x => x.Value).Distinct().ToArray();

        public int GetNumberOfClass(TrainingSet training, char @class)
            => training.Count(x => x.Value == @class);

        public string[] GetDistinctWords(TrainingSet training)
        {
            var dictionary = new Dictionary<string, int>();

            foreach (var t in training)
            {
                string[] words = t.Key.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                    dictionary[word] = 0;
            }

            return dictionary.Keys.ToArray();
        }

        public int GetWordCountInTheClass(TrainingSet training, char @class)
        {
            return training
                .Where(x => x.Value == @class)
                .SelectMany(x => x.Key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Count();
        }

        public int GetWordCountInTheClass(TrainingSet training, string word, char @class)
        {
            return training
                .Where(x => x.Value == @class)
                .SelectMany(x => x.Key.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Count(x => x == word);
        }
    }
}
