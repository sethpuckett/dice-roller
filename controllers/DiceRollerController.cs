using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DiceRoller.Controllers
{
  [Route("/dice")]
  public class DiceRollerController : Controller
  {
    [HttpGet]
    public IActionResult Roll(int sides, int count = 1, int constant = 0, int rolls = 1)
    {
      if (!inputValid(sides, count, constant, rolls)) {
        return BadRequest();
      }

      var retArray = new List<int>();
      var rand = new Random();

      for (int i = 0; i < rolls; i++) {
        retArray.Add(calculateRoll(rand, sides, count, constant));
      }

      if (rolls == 1) {
        return Ok(retArray[0]);
      } else {
        return Ok(retArray);
      }
    }

    private int calculateRoll(Random rand, int sides, int count, int constant)
    {
        var total = 0;
        for (var j = 0; j < count; j++)
        {
          total += rand.Next(1, sides + 1);
        }
        total += constant;
        return total;
    }

    private bool inputValid(int sides, int count, int constant, int rolls)
    {
      return sides > 0
        && sides <= 100
        && count > 0
        && count <= 100
        && constant >= 0
        && constant <= 1000000
        && rolls > 0
        && rolls <= 100;
    }
  }
}