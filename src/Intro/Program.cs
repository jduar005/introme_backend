using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNewLib;

namespace Intro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Calculator calc = new Calculator();
            var answer = calc.Add(18,24);

            Console.WriteLine($"The answer is {answer}")
        }
    }
}
