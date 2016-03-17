using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiceProb {
	class Program {
		static void Main(string[] args) {
			List<int[]> dice = new List<int[]>(); //Input dice, transformed to polynomial coefficients (e.g. 1, 1, 1, 1, 1, 1)
			int[] d = null; //Single input die, for parsing
			int dq = 0; //Number of dice
			int[] result = null; //Accumulated result
			int[] lastresult = null; //Previous result, used in repeated multiplication
			int[] current = null; //Current die being multiplied with lastresult
			string line; //User input
			int q = 0; //Quantity (multiplicity) of input die
			List<int> inputs; //Parsed user input, with values of die faces (e.g. 1, 2, 3, 4, 5, 6)

			double p; //Total permutations
			double ft; //Cumulative sum of freq

			//Input parsing loop
			while (true) {
				Console.Write("Die {0}? ", dq + 1);
				line = Console.ReadLine();
				if (String.IsNullOrWhiteSpace(line)) {
					break;
				}
				Console.Write("Die {0} quantity? [1] ", dq + 1);
				if (!Int32.TryParse(Console.ReadLine(), out q)) {
					q = 1;
				}
				inputs = line.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x.Trim())).ToList<int>();
				d = new int[inputs.Max() + 1]; //There's a zero element that remains 0, just to keep the indexing straight-forward. If I do a C version, it'll probably store the array length.
				for (int i = 0; i < inputs.Count; i++) {
					d[inputs[i]]++;
				}
				for (int i = 0; i < q; i++) {
					dq++;
					dice.Add(d);
				}
			};

			//Calculation loop(s)
			lastresult = dice[0];
			for (int i = 1; i < dq; i++) { //Loop through the dice.
				current = dice[i];
				result = new int[lastresult.Length + current.Length - 1];
				for (int x = 0; x < lastresult.Length; x++) {
					if (lastresult[x] == 0) {
						continue;
					}
					for (int y = 0; y < current.Length; y++) {
						result[x + y] += lastresult[x] * current[y];
					}
				}
				lastresult = result;
			}

			//Result display loop
			bool skipzero = true;
			p = 0.0D;
			ft = 0.0D;
			for (int i = 0; i < lastresult.Length; i++) {
				p += (double)lastresult[i];
			}
			Console.WriteLine("Permutations: {0:n0}", p);
			Console.WriteLine("Total\tFreq\tProb\t\tp<=\t\tp>=");
			for (int i = 0; i < lastresult.Length; i++) {
				ft += (double)lastresult[i];
				if (lastresult[i] == 0 && skipzero) {
					continue;
				}
				skipzero = false;
				Console.WriteLine("{0}\t{1}\t{2:p4}\t{3:p4}\t{4:p4}", i, lastresult[i], (double)lastresult[i] / p, ft / p, (p - ft + (double)lastresult[i]) / p);
			}
		}
	}
}
