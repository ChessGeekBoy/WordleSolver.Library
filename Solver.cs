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

        public Solver(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            this.Possibilities = JsonSerializer.Deserialize<IEnumerable<string>>(json);
        }

        public IEnumerable<string> FilterPossibilities(Dictionary<int, CharacterStatus> response, string attempt)
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
            
            return this.Possibilities;
        }
    }
}