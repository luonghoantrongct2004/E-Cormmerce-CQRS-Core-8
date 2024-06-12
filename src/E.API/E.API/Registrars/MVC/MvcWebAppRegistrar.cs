﻿using E.API.Registrars.RegistrarBase;

namespace E.API.Registrars.MVC;

public class MvcWebAppRegistrar : IWebApplicationRegistrar
{
    public void RegisterPipelineComponents(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();
    }
}