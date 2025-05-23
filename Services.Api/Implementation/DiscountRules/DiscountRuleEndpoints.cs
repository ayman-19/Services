using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.DiscountRules.Command.Create;
using Services.Application.Features.DiscountRules.Command.Delete;
using Services.Application.Features.DiscountRules.Command.Update;
using Services.Application.Features.DiscountRules.Queries.GetById;
using Services.Application.Features.DiscountRules.Queries.Paginate;

namespace Services.Api.Implementation.DiscountRules
{
    public class Endpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints
                .MapGroup("/DiscountRules")
                .WithTags("DiscountRules");

            group.MapPost(
                "CreateAsync/",
                async (
                    CreateDiscountRulesCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );
            //.RequireAuthorization(nameof(Permissions.CreateBooking));
            group.MapPut(
                "UpdateAsync/",
                async (
                    UpdateDiscountRulesCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );
            //.RequireAuthorization(nameof(Permissions.UpdateBranch));

            group.MapDelete(
                "DeleteAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await sender.Send(new DeleteDiscountRulesCommand(id), cancellationToken)
                    )
            );
            //.RequireAuthorization(nameof(Permissions.DeleteBranch));

            group.MapGet(
                "GetByIdAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetDiscountRuleQuery(id), cancellationToken))
            );
            //.RequireAuthorization(nameof(Permissions.GetBranch));

            group.MapPost(
                "PaginateAsync",
                async (
                    PaginateDiscountRuleQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );
            //.RequireAuthorization(nameof(Permissions.PaginateBranch));
        }
    }
}
