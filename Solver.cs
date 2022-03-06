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
        public string[] Possibilities { get; set; }

        public delegate void Filter(string[] possibilities);

        public event Filter OnFilter;

        public delegate void Solve(string solution);

        public event Solve OnSolve;

        public Solver(string wordFilePath)
        {
            this.Possibilities = File.ReadAllLines(wordFilePath);
        }

        public void FilterPossibilities(CharacterStatus[] response, string attempt)
        {
            for (int index = 0; index < attempt.Length; index++)
            {
                switch (response[index])
                {
                    case CharacterStatus.Black:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where !possibility.Contains(attempt[index])
                                             select possibility).ToArray();
                        break;
                    case CharacterStatus.Yellow:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where possibility.Contains(attempt[index]) 
                                             && possibility[index] != attempt[index]
                                             select possibility).ToArray();
                        break;
                    case CharacterStatus.Green:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where possibility[index] == attempt[index]
                                             select possibility).ToArray();
                        break;
                    default:
                        break;
                }
            }

            switch (this.Possibilities.Length)
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