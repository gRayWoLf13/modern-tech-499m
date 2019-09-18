using modern_tech_499m.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace modern_tech_499m
{
    static class AISolver
    {
        static readonly Random rand = new Random();
        public static Task<int> GetCell()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                return rand.Next(6);
            });
        }
    }
}