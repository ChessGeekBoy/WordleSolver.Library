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
        public List<string> Possibilities { get; set; }

        public Solver(string wordFilePath)
        {
            this.Possibilities = File.ReadAllText(wordFilePath).Split("\n").ToList<string>();
        }

        public IEnumerable<string> FilterPossibilities(CharacterStatus[] response, string attempt)
        {
            for (int index = 0; index < attempt.Length; index++)
            {
                switch (response[index])
                {
                    case CharacterStatus.Black:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where !possibility.Contains(attempt[index])
                                             select possibility).ToList();
                        break;
                    case CharacterStatus.Yellow:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where possibility.Contains(attempt[index]) 
                                             && possibility[index] != attempt[index]
                                             select possibility).ToList();
                        break;
                    case CharacterStatus.Green:
                        this.Possibilities = (from possibility in this.Possibilities
                                             where possibility[index] == attempt[index]
                                             select possibility).ToList();
                        break;
                    default:
                        break;
                }
            }
            
            return this.Possibilities;
        }
    }
}