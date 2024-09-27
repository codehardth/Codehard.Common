using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Codehard.Common.AspNetCore.Controllers.Parameters;

/// <summary>
/// Represents an abstract parameter for a controller.
/// </summary>
public abstract record ControllerParameter
{
    private ControllerParameter()
    {
    }

    /// <summary>
    /// Represents a parameter containing JSON data.
    /// </summary>
    public sealed record Json(JsonDocument JsonDocument) : ControllerParameter;

    /// <summary>
    /// Represents a parameter containing form data.
    /// </summary>
    public sealed record Form(IFormCollection FormCollection) : ControllerParameter;

    /// <summary>
    /// Represents an unknown parameter type.
    /// </summary>
    public sealed record Unknown(HttpRequest Request) : ControllerParameter;

    /// <summary>
    /// Match a parameter based on its internal type.
    /// </summary>
    /// <param name="json">The action to perform if the parameter is of type Json.</param>
    /// <param name="form">The action to perform if the parameter is of type Form.</param>
    /// <param name="other">The action to perform if the parameter is of type Unknown (optional).</param>
    /// <typeparam name="TResult">The result type of the actions.</typeparam>
    /// <returns>The result of the matched action.</returns>
    /// <exception cref="NotSupportedException">Thrown when the parameter type is not supported.</exception>
    public TResult Match<TResult>(
        Func<Json, TResult> json,
        Func<Form, TResult> form,
        Func<Unknown, TResult>? other = default)
    {
        return this switch
        {
            Json j => json(j),
            Form f => form(f),
            Unknown u =>
                other is not null
                    ? other.Invoke(u)
                    : default!,
            _ => throw new NotSupportedException($"{this.GetType()} is not supported in this context."),
        };
    }

    /// <summary>
    /// Checks if this parameter is <see cref="Json"/>.
    /// </summary>
    public bool IsJsonParameter => this is Json;

    /// <summary>
    /// Checks if this parameter is <see cref="Form"/>.
    /// </summary>
    public bool IsFormParameter => this is Form;

    /// <summary>
    /// Executes the provided action if this parameter is of type Json.
    /// </summary>
    /// <param name="action">The action to execute if the parameter is of type Json.</param>
    public void IfJson(Action<Json> action)
    {
        if (IsJsonParameter)
        {
            action((Json)this);
        }
    }

    /// <summary>
    /// Executes the provided action if this parameter is of type Form.
    /// </summary>
    /// <param name="action">The action to execute if the parameter is of type Form.</param>
    public void IfForm(Action<Form> action)
    {
        if (IsFormParameter)
        {
            action((Form)this);
        }
    }

    /// <summary>
    /// Executes the provided asynchronous action if this parameter is of type Json.
    /// </summary>
    /// <param name="action">The asynchronous action to execute if the parameter is of type Json.</param>
    public async Task IfJsonAsync(Func<Json, Task> action)
    {
        if (IsJsonParameter)
        {
            await action((Json)this);
        }
    }

    /// <summary>
    /// Executes the provided asynchronous action if this parameter is of type Form.
    /// </summary>
    /// <param name="action">The asynchronous action to execute if the parameter is of type Form.</param>
    public async Task IfFormAsync(Func<Form, Task> action)
    {
        if (IsJsonParameter)
        {
            await action((Form)this);
        }
    }
}