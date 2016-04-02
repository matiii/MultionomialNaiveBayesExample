using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MultionomialNaiveBayesExample.App
{
    class Example
    {
        private readonly NaiveBayesHelper _helper = new NaiveBayesHelper();
        private readonly Probabilities _probabilities = new Probabilities(); 
        private readonly Dictionary<char, double> _priors = new Dictionary<char, double>();

        private Dictionary<string, char> _training = new Dictionary<string, char>
        {
            {"Chinese Beijing Chinese", 'c' },
            {"Chinese Chinese Shangai", 'c' },
            {"Chinese Macao", 'c' },
            {"Tokyo Japan Chinese", 'j' }
        };

        private string[] _test =
        {
            "Chinese Chinese Chinese Tokyo Japan"
        };

        public void Show()
        {
            Separator();
            ShowData();

            Separator();
            Priors();

            Separator();
            ConditionalProbabilities();

            Separator();
            ChoosingClass();
        }

        private void ChoosingClass()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Choosing a class");

            string[] words = _test[0].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var distinctClass in _helper.GetDistinctClasses(_training))
            {
                sb.AppendLine(GetClass(distinctClass, words, _test[0]));
            }

            WriteLine(sb);
        }

        private string GetClass(char distinctClass, string[] words, string s)
        {
            var sb = new StringBuilder();
            sb.Append($"P({distinctClass}|{s}) = ");
            double start = 1.0;

            foreach (var word in words)
            {
                if (start == 1.0)
                {
                    sb.Append($"{_priors[distinctClass].ToString("F")} * ");
                    start *= _priors[distinctClass];
                }
                else
                    sb.Append(" * ");

                double p = _probabilities.GetProbability(distinctClass, word);
                start *= p;

                sb.Append($"{p.ToString("F")}");
            }

            sb.Append($" = {start}");

            return sb.ToString();
        }

        private void ConditionalProbabilities()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Conditional Probabilities");

            foreach (var @class in _helper.GetDistinctClasses(_training))
            {
                foreach (var word in _helper.GetDistinctWords(new Dictionary<string, char> {{_test[0], '?'}}))
                {
                    sb.AppendLine(GetProbability(word, @class));
                }
            }

            WriteLine(sb);
        }

        private string GetProbability(string word, char @class)
        {
            int a1 = _helper.GetWordCountInTheClass(_training, word, @class);
            int a2 = _helper.GetWordCountInTheClass(_training, @class);
            int a3 = _helper.GetDistinctWords(_training).Length;

            _probabilities.Add(new Entries
            {
                Class = @class,
                Word = word,
                Probability = (a1 + 1) / (double)(a2 + a3)
            });

            return $"P({word}|{@class}) = ({a1} + 1) / ({a2} + {a3}) = {a1 + 1} / {a2 + a3} = {_probabilities.Last().Probability.ToString("F")}";
        }

        private void Priors()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Priors");

            foreach (var c in _helper.GetDistinctClasses(_training))
                sb.AppendLine(GetPrior(c));

            WriteLine(sb);
        }

        private string GetPrior(char @class)
        {
            int a1 = _helper.GetNumberOfClass(_training, @class);
            int a2 = _training.Count;

            _priors.Add(@class, a1 / (double) a2);

            return $"P({@class}) = {a1} / {a2}";
        }

        private void ShowData()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Training data");
            sb.AppendLine();

            foreach (var training in _training)
                sb.AppendLine($"Doc: {training.Key} class: {training.Value}");

            sb.AppendLine();
            sb.AppendLine("Test data");
            sb.AppendLine();

            foreach (var test in _test)
                sb.AppendLine($"Doc: {test} class: ?");

            WriteLine(sb);
        }

        private void Separator()
        {
            WriteLine();
            WriteLine("***----------------------------------------------------________");
            WriteLine();
        }


    }
}
