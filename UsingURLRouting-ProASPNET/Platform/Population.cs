namespace UsingURLRouting_ProASPNET.Platform
{
    public class Population
    {

        public static async Task Endpoint(HttpContext context)
        {
            string? city = context.Request.RouteValues["city"] as string ?? "london";

            int? pop = null;

            switch(city.ToLower())
            {
                case "london":
                    pop = 8_136_00;
                    break;

                case "paris":
                    pop = 2_141_00;
                    break;

                case "monaco":
                    pop = 39_000;
                    break;
            }
            if (pop != null)
                await context.Response.WriteAsync($"The population of {city} is: {pop}");
            else
                context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
