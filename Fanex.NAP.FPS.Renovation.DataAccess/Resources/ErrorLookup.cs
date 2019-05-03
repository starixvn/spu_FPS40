namespace Fanex.NAP.FPS.Renovation.DataAccess.Resources
{
    using System.Resources;

    public static class ErrorLookup
    {
        private static readonly ResourceManager resource = new ResourceManager(typeof(Error));

        public static string GetValue(string key)
        {
            return resource.GetString(key);
        }
    }
}
