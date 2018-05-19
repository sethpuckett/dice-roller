using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Domain
{
  public class Roll
  {
    public readonly IEnumerable<int> Values;
    public readonly int Constant;

    public Roll(IEnumerable<int> values, int constant)
    {
      this.Values = values;
      this.Constant = constant;
    }

    public int Total
    {
      get => Math.Max(0, this.Values.Sum() + this.Constant);
    }
  }
}