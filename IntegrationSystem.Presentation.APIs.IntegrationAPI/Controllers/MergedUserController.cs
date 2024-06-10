using IntegrationSystem.Application.Interfaces.UseCases;
using IntegrationSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationSystem.Presentation.APIs.IntegrationAPI.Controllers;

/// <summary>
/// Controller for <see cref="MergedUser"/> entity.
/// </summary>
/// <param name="mergedUserUseCase">The <see cref="IMergedUserUseCase"/> to use.</param>
[Route("api/users")]
[ApiController]
public class MergedUserController(IMergedUserUseCase mergedUserUseCase)
	: ControllerBase
{
	/// <summary>
	/// The <see cref="IMergedUserUseCase"/>.
	/// </summary>
	private readonly IMergedUserUseCase _mergedUserUseCase = mergedUserUseCase;

	/// <summary>
	/// Gets all <see cref="MergedUser"/>s.
	/// </summary>
	/// <returns>ActionResult of <see cref="MergedUser"/>s</returns>
	[HttpGet("all")]
	public async Task<ActionResult<IEnumerable<MergedUser>>> GetAllMergedUsers()
	{
		// Get all merged users
		var users = await _mergedUserUseCase.GetMergedUsersAsync();

		// Return the users
		return Ok(users ?? []);
	}
}
