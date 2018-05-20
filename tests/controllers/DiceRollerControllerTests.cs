
using System.Collections.Generic;
using DiceRoller.Controllers;
using DiceRoller.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DiceRoller.Tests.Controllers
{
  [TestFixture]
  public class DiceRollerControllerTests
  {
    private DiceRollerController sut;
    private Mock<IRoller> roller;

    private readonly IEnumerable<int> diceValues;
    private readonly IEnumerable<Roll> singleRollAttemptList;
    private readonly IEnumerable<Roll> multiRollAttemptList;

    public DiceRollerControllerTests()
    {
      diceValues = new List<int> { 1, 2, 3 };
      singleRollAttemptList = new List<Roll> { new Roll(diceValues, 0) };
      multiRollAttemptList = new List<Roll> { new Roll(diceValues, 0), new Roll(diceValues, 0) };
    }

    [SetUp]
    public void Setup()
    {
      roller = new Mock<IRoller>();
      sut = new DiceRollerController(roller.Object);
    }

    [Test]
    public void Roll_GivenValidSingleAttempt_ExpectNumber()
    {
      var singleRollResponse = new RollerDomainResponse(singleRollAttemptList, true, string.Empty);
      roller
        .Setup(r => r.Roll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns(singleRollResponse);

      var result = sut.Roll(6);

      Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
      var okResult = result as OkObjectResult;
      Assert.That(okResult.Value, Is.TypeOf(typeof(int)), "single, non-verbose rolls should return a numeric value");
    }

    [Test]
    public void Roll_GivenValidMultipleAttempt_ExpectEnumerableOfInts()
    {
      var multiRollResponse = new RollerDomainResponse(multiRollAttemptList, true, string.Empty);
      roller
        .Setup(r => r.Roll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns(multiRollResponse);

      var result = sut.Roll(6, 1, 0, 2);

      Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
      var okResult = result as OkObjectResult;
      Assert.That(okResult.Value, Is.TypeOf(typeof(List<int>)), "multi-attempt, non-verbose rolls should return a list of numbers");
    }

    [Test]
    public void Roll_GivenSingleVerboseRoll_ExpectWebResponse()
    {
      var singleRollResponse = new RollerDomainResponse(singleRollAttemptList, true, string.Empty);
      roller
        .Setup(r => r.Roll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns(singleRollResponse);

      var result = sut.Roll(6, 1, 0, 1, true);

      Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
      var okResult = result as OkObjectResult;
      Assert.That(okResult.Value, Is.TypeOf(typeof(DiceRollerWebResponse)), "verbose rolls should return an DiceRollerWebResponse");
    }

    [Test]
    public void Roll_GivenMultiVerboseRoll_ExpectWebResponseEnumerable()
    {
      var multiRollResponse = new RollerDomainResponse(multiRollAttemptList, true, string.Empty);
      roller
        .Setup(r => r.Roll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns(multiRollResponse);

      var result = sut.Roll(6, 1, 0, 2, true);

      Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
      var okResult = result as OkObjectResult;
      Assert.That(okResult.Value, Is.TypeOf(typeof(List<DiceRollerWebResponse>)), "multi-attempt verbose rolls should return a list of DiceRollerWebResponses");
    }

    [Test]
    public void Roll_GivenInvalidParameters_ExpectBadRequest()
    {
      var errorResponse = new RollerDomainResponse(null, false, "Error!");
      roller
        .Setup(r => r.Roll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
        .Returns(errorResponse);

      var result = sut.Roll(5);

      Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
    }
  }
}