using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Models;
using PW_Museo.Repositories;

namespace PW_Museo.Endpoints
{
    public static class ShowsEndpoints
    {
        public static void MapShowsEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/shows");

            group.MapGet("/", async (ExhibitionsRepository repo) => Results.Ok(await repo.GetAllAsync()));
            group.MapGet("/{id:guid}", async (Guid id, ExhibitionsRepository repo) =>
            {
                var item = await repo.GetByIdAsync(id);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            });
            group.MapPost("/", async (Show item, ExhibitionsRepository repo) =>
            {
                item.Id = Guid.NewGuid();
                await repo.CreateAsync(item);
                return Results.Created($"/api/shows/{item.Id}", item);
            });
            group.MapPut("/{id:guid}", async (Guid id, Show item, ExhibitionsRepository repo) =>
            {
                if (id != item.Id) return Results.BadRequest();
                if (await repo.GetByIdAsync(id) is null) return Results.NotFound();
                await repo.UpdateAsync(item);
                return Results.NoContent();
            });
            group.MapDelete("/{id:guid}", async (Guid id, ExhibitionsRepository repo) =>
            {
                if (await repo.GetByIdAsync(id) is null) return Results.NotFound();
                await repo.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
