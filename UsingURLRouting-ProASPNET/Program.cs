using UsingURLRouting_ProASPNET.Platform;

var builder = WebApplication.CreateBuilder(args);


/* Custom Constraint */
builder.Services.Configure<RouteOptions>(opts =>
{
    opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
});
/* Custom Constraint*/


var app = builder.Build();

var capital = new Capital();
var population = new Population();


/* Custom Constraint */
app.MapGet("capital/{country:countryName}", Capital.Endpoint);
/* Custom Constraint */


#region Routing and Endpoints
//app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("routing", async context =>
//    {
//        await context.Response.WriteAsync("Request was routed");
//    });
//    endpoints.MapGet("capital/uk", capital.Invoke);
//    endpoints.MapGet("population/paris", population.Invoke);
//});
//app.MapGet("routing", async context =>
//{
//    await context.Response.WriteAsync("Request was Routed");
//});
#endregion

#region Using Segment Variables in URL Patterns

app.MapGet("{first}/{second}/{third}", async context =>
{
    await context.Response.WriteAsync("Route was Routed\n");
    foreach (var kvp in context.Request.RouteValues)
    {
            await context.Response.WriteAsync($"{kvp.Key}: {kvp.Value}\n");
    }
});

#endregion

#region Matching Multiple values from a single URL Segment

// A url pode conter várias variáveis, desde que separadas por uma string estática, neste caso, o "."
// essas urls mais complexas são lidas da esquerda para a direita.
// há algumas complicações/problemas em ter urls complexas, por isso, é de boas práticas deixá-las o menos complexo possivel
app.MapGet("/file/{filename}.{ext}", async context =>
{
    await context.Response.WriteAsync("Route was routed\n");

    foreach(var kvp in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"key:{kvp.Key}, value: {kvp.Value} \n");
    }
});

#endregion

//app.MapGet("capital/{country=France}", Capital.Endpoint);
// route with Regex
app.MapGet("capital/{country:regex(^uk|france|canada$)}", Capital.Endpoint);
app.MapGet("size/{city?}", Population.Endpoint)
    .WithMetadata(new RouteNameMetadata("population"));
// nome utilizado para referenciar a rota

// Mesmo eu alterando o nome de "population" para "size" na URL, ao colocar capital/monaco continuará
// sendo redirecionado para a url, pois o RouteNameMetadata continua "population"

#region Catchall Segments (/param/{*catchall})
// Catchall permite ter mais parametors do que o necessário, e todos os próximos parâmetros serão o catchall

app.MapGet("{first}/{second}/{third}/{*catchall}", async context =>
{
    await context.Response.WriteAsync("Route was routed\n");
    foreach(var kvp in context.Request.RouteValues)
    {
        //if(kvp.Key == "catchall")
        //{
        //    await context.Response.WriteAsync($"{kvp.Value?.ToString()?.Split("/")[1]}\n");
        //}
        await context.Response.WriteAsync($"{kvp.Key}, {kvp.Value}\n");
    }
});
#endregion

#region Constraining Segment Matching (/param:string/param1:int)

app.MapGet("{first:int}/{second:bool}", async context =>
{
    await context.Response.WriteAsync("Request was routed\n");

    foreach(var kvp in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"{kvp.Key}, {kvp.Value}\n");
    }
});

// search for: url pattern constraints

// combining constraints

app.MapGet("{first:alpha:length(4)}/{second:bool}", async context =>
{
    await context.Response.WriteAsync("Request was routed\n");

    foreach(var kvp in context.Request.RouteValues)
    {
        await context.Response.WriteAsync($"{kvp.Key}, {kvp.Value}\n");
    }
});

#endregion

#region Fallback route (rota para caso não encontre nenhum endpoint)

app.MapFallback(async context =>
{
    await context.Response.WriteAsync($"{StatusCodes.Status404NotFound}");
});

#endregion





app.Run();