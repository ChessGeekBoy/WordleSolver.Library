using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace WordleSolver.Library
{
    public class Solver
    {
        public IEnumerable<string> Possibilities { get; set; }

        public delegate void Filter(IEnumerable<string> possibilities);

        public event Filter OnFilter;

        public delegate void Solve(string solution);

        public event Solve OnSolve;

        public Solver(string wordFilePath)
        {
            this.Possibilities = File.ReadAllLines(wordFilePath) as IEnumerable<string>;
        }

        public void FilterPossibilities(CharacterStatus[] response, string attempt)
        {
            for (int index = 0; index < attempt.Length; index++)
            {
                switch (response[index])
                {
                    case CharacterStatus.Black:
                        this.Possibilities = from possibility in this.Possibilities
                                             where !possibility.Contains(attempt[index])
                                             select possibility;
                        break;
                    case CharacterStatus.Yellow:
                        this.Possibilities = from possibility in this.Possibilities
                                             where possibility.Contains(attempt[index]) 
                                             && possibility[index] != attempt[index]
                                             select possibility;
                        break;
                    case CharacterStatus.Green:
                        this.Possibilities = from possibility in this.Possibilities
                                             where possibility[index] == attempt[index]
                                             select possibility;
                        break;
                    default:
                        break;
                }
            }

            switch (this.Possibilities.Count())
            {
                case 1:
                    this.OnSolve(this.Possibilities.FirstOrDefault());
                    break;
                default:
                    this.OnFilter(this.Possibilities);
                    break;
            }
        }
    }
}