namespace DiceRoller.Domain
{
  public interface IRoller
  {
    RollerDomainResponse Roll(int count, int sides, int constant, int attempts);
  }
}