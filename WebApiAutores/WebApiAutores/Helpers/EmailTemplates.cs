namespace WebApiAutores.Helpers
{
    public static class EmailTemplates
    {
        public static string LoginTemplate(string email)
        {
            return $@"
            <h3>Inicio de sesion realizado correctamente</h3>
            <P>Le informamos que se ha iciado sesion en cu cuenta</p>
            <P>Fecha y Hora de inicio de sesion: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}</p>
            ";
        }
    }
}
