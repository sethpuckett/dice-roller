using System.Collections.Generic;
using System.Linq;
using DiceRoller.Domain;
using NUnit.Framework;

namespace DiceRoller.Tests.Domain
{
  [TestFixture]
  public class DiceRollerTests
  {
    public static readonly List<int> ValidSides = new List<int> { 4, 6, 8, 10, 12, 100 };
    [TestCaseSource("ValidSides")]
    public void Roll_GivenValidSides_ExpectValidResponse(int sides)
    {
      var actual = Roller.Roll(1, sides);
      Assert.That(actual.Valid, Is.True, "Roll should be valid");
    }

    public static readonly List<int> InvalidSides = new List<int> { -4, 0, 7, 101 };
    [TestCaseSource("InvalidSides")]
    public void Roll_GivenInvalidSides_ExpectInvalidResponse(int sides)
    {
      var actual = Roller.Roll(1, sides);
      Assert.That(actual.Valid, Is.False, "Roll should be invalid");
    }

    public static readonly List<int> ValidCount = new List<int> { 1, 2, 100 };
    [TestCaseSource("ValidCount")]
    public void Roll_GivenValidCount_ExpectValidResponse(int count)
    {
      var actual = Roller.Roll(count, 6);
      Assert.That(actual.Valid, Is.True, "Roll should be Valid");
    }

    public static readonly List<int> InvalidCount = new List<int> { -1, 0, 101};
    [TestCaseSource("InvalidCount")]
    public void Roll_GivenInvalidCount_ExpectInvalidResponse(int count)
    {
      var actual = Roller.Roll(count, 6);
      Assert.That(actual.Valid, Is.False, "Roll should be invalid");
    }

    [Test]
    public void Roll_GivenConstant_ExpectConstantAddedToValue()
    {
      var actual = Roller.Roll(1, 4, 100);
      Assert.That(actual.Rolls.First().Total, Is.GreaterThan(100), "Roll should be greater than constant value");
      Assert.That(actual.Rolls.First().Total, Is.LessThan(105), "Roll should be less than constant value plus max dice roll");
    }

    [Test]

    public void Roll_GivenNegativeConstant_ExpectConstantSubtractedFromValue()
    {
      var actual = Roller.Roll(1, 4, -1);
      Assert.That(actual.Rolls.First().Total, Is.GreaterThan(-1), "Roll should be lowest possible value");
      Assert.That(actual.Rolls.First().Total, Is.LessThan(4), "Roll should be less than max dice roll minus constant");
    }

    [Test]

    public void Roll_GivenNegativeConstant_RollCannotBeLessThanZero()
    {
      var actual = Roller.Roll(1, 4, -100);
      Assert.That(actual.Rolls.First().Total, Is.EqualTo(0), "Roll should always be 0 with high negative constant");
    }

    [Test]

    public void Roll_GivenMultipleAttempts_ExpectMultipleRollResponses()
    {
      var actual = Roller.Roll(1, 4, 0, 2);
      Assert.That(actual.Rolls.Count, Is.EqualTo(2), "Should contain 2 rolls");
    }

    public static readonly List<int> InvalidAttempts = new List<int> { -1, 0, 101};
    [TestCaseSource("InvalidAttempts")]
    public void Roll_InvalidAttempts_ExpectInvalidResponse(int attempts)
    {
      var actual = Roller.Roll(1, 4, 0, attempts);
      Assert.That(actual.Valid, Is.False, "Roll should be invalid with invalid attempts");
    }
  }
}