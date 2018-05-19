using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Domain
{
  public class RollerDomainResponse
  {
    public readonly bool Valid;
    public readonly string Message;
    public readonly IEnumerable<Roll>  Rolls;

    public RollerDomainResponse(IEnumerable<Roll> rolls, bool valid = true, string message = null)
    {
      this.Rolls = rolls;
      this.Valid = valid;
      this.Message = message;
    }

    public static RollerDomainResponse Invalid(string message)
    {
      return new RollerDomainResponse(null, false, message);
    }
  }
}