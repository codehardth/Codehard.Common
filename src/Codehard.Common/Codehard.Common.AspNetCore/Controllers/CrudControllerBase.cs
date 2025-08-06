using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Codehard.Common.AspNetCore.Controllers.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Codehard.Common.AspNetCore.Controllers;

[Route("api/[controller]")]
public abstract class CrudControllerBase<TEntityKey, TEntity> : ControllerBase
    where TEntity : class
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var requestParams =
            this.Request.Query
                .Select(q => new GetRequestParameter(q.Key, q.Value))
                .ToImmutableArray();

        var res = await this.ReadAsync(requestParams, cancellationToken);

        return res;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken = default)
    {
        var parameter = await GetParameter();

        var res = await this.CreateAsync(parameter, cancellationToken);

        return res;
    }

    [HttpPut]
    public async Task<IActionResult> Put(CancellationToken cancellationToken = default)
    {
        var parameter = await GetParameter();

        var res = await this.UpdateAsync(parameter, cancellationToken);

        return res;
    }

    [HttpPatch]
    public async Task<IActionResult> Patch(
        [FromBody] JsonPatchDocument<TEntity>? patch,
        CancellationToken cancellationToken = default)
    {
        var res = await this.PartialUpdateAsync(patch, cancellationToken);

        return res;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        TEntityKey id,
        CancellationToken cancellationToken = default)
    {
        var res = await this.DeleteAsync(id, cancellationToken);

        return res;
    }

    private async Task<ControllerParameter> GetParameter()
    {
        var parameter =
            (this.Request.ContentType switch
            {
                "application/json" => await ReadBodyAsync(this.Request),
                _ when this.Request.HasFormContentType => await ReadFormDataAsync(this.Request),
                _ => new ControllerParameter.Unknown(this.Request),
            })!;

        return parameter;

        static async Task<ControllerParameter> ReadFormDataAsync(HttpRequest request)
        {
            var forms = await request.ReadFormAsync();

            return new ControllerParameter.Form(forms);
        }

        static async Task<ControllerParameter> ReadBodyAsync(HttpRequest request)
        {
            using var sr = new StreamReader(request.Body, Encoding.UTF8);

            var json = await sr.ReadToEndAsync();

            return new ControllerParameter.Json(JsonDocument.Parse(json));
        }
    }

    /// <summary>
    /// Perform a read operation in an asynchronous manner.
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract ValueTask<IActionResult> ReadAsync(
        IReadOnlyCollection<GetRequestParameter> parameters,
        CancellationToken cancellationToken);

    /// <summary>
    /// Perform a create operation in an asynchronous manner.
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract ValueTask<IActionResult> CreateAsync(
        ControllerParameter parameter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Perform an update operation in an asynchronous manner.
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract ValueTask<IActionResult> UpdateAsync(
        ControllerParameter parameter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Perform a partial update operation in an asynchronous manner.
    /// </summary>
    /// <param name="patch"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract ValueTask<IActionResult> PartialUpdateAsync(
        JsonPatchDocument<TEntity>? patch,
        CancellationToken cancellationToken);

    /// <summary>
    /// Perform a delete operation in an asynchronous manner.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract ValueTask<IActionResult> DeleteAsync(
        TEntityKey key,
        CancellationToken cancellationToken);
}