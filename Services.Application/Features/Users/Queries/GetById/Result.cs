using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Users.Queries.GetById
{
	public sealed record GetUserResult(
	Guid Id,
	string Name,
	string Email,
	DateTime CreatedOn,
	IEnumerable<string> roles
);
}
