using System;
using System.Collections.Generic;

namespace DiceRoller.Domain
{
  public class Roller : IRoller
  {

    public RollerDomainResponse Roll(int count, int sides, int constant = 0, int attempts = 1)
    {
      if (!InputValid(count, sides, constant, attempts)) {
        return RollerDomainResponse.Invalid("Invalid dice parameters");
      }

      var rolls = new List<Roll>();
      var rand = new Random();

      for (int i = 0; i < attempts; i++) {
        rolls.Add(CalculateAttempt(rand, count, sides, constant));
      }

      return new RollerDomainResponse(rolls);
    }

    private static Roll CalculateAttempt(Random rand, int count, int sides, int constant)
    {
        var rolls = new List<int>();
        for (var j = 0; j < count; j++)
        {
          rolls.Add(rand.Next(1, sides + 1));
        }
        return new Roll(rolls, constant);
    }

    private static bool InputValid(int count, int sides, int constant, int attempts)
    {
      return
        new List<int> {4, 6, 8, 10, 12, 20, 100}.Contains(sides)
        && count > 0
        && count <= 100
        && constant >= -1000000
        && constant <= 1000000
        && attempts > 0
        && attempts <= 100;
    }
  }
}