namespace Web.API;

public static class Endpoints
{
    private const string ApiBase = "api";

    public static class Assets
    {
        private const string Base = $"{ApiBase}/assets";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }

    public static class Departments
    {
        private const string Base = $"{ApiBase}/departments";
        public const string Create = Base;
        public const string GetAll = Base;
    }
}
