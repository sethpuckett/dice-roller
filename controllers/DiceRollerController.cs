using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DiceRoller.Domain;
using System.Linq;

namespace DiceRoller.Controllers
{
  [Route("/dice")]
  public class DiceRollerController : Controller
  {
    private readonly IRoller Roller;

    public DiceRollerController(IRoller roller)
    {
      this.Roller = roller;
    }

    [HttpGet]
    public IActionResult Roll(int sides, int count = 1, int constant = 0, int attempts = 1, bool verbose = false)
    {
      var domainResponse = this.Roller.Roll(count, sides, constant, attempts);

      if (!domainResponse.Valid) {
        return BadRequest(domainResponse.Message);
      }

      if (attempts == 1)
      {
        if (verbose) {
          return Ok(VerboseResponse(domainResponse.Rolls.First()));
        } else {
          return Ok(domainResponse.Rolls.First().Total);
        }
      } else {
        if (verbose) {
          var webResponses = domainResponse.Rolls.Select(roll => VerboseResponse(roll));
          return Ok(webResponses);
        } else {
          return Ok(domainResponse.Rolls.Select(roll => roll.Total));
        }
      }
    }

    private DiceRollerWebResponse VerboseResponse(Roll roll)
    {
      return new DiceRollerWebResponse
      {
        Total = roll.Total,
        Rolls = roll.Values
      };
    }
  }

  public class DiceRollerWebResponse
  {
    public int Total;
    public IEnumerable<int> Rolls;
  }
}