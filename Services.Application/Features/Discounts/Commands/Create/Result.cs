using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Discounts.Commands.Create
{
	public sealed record CreateDiscountResult(Guid id ,
		double Percentage ,
		DateTime Expireon)
	{
		public static implicit operator CreateDiscountResult(Discount d) =>
			new(d.Id, d.Percentage, d.ExpireOn);
			
 	}
}
