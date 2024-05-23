namespace Web.API;

public static class Endpoints
{
    private const string ApiBase = "api";

    public static class Assets
    {
        private const string Base = $"{ApiBase}/assets";
        public const string CreateAnAsset = Base;
        public const string GetTheAssetById = $"{Base}/{{id:guid}}";
        public const string GetAllTheAssets = Base;
        public const string UpdateTheAssetBasicInfoById = $"{Base}/{{id:guid}}";
        public const string DeleteAnAsset = $"{Base}/{{id:guid}}";
    }

    public static class Departments
    {
        private const string Base = $"{ApiBase}/departments";
        public const string CreateADepartment = Base;
        public const string GetAllDepartments = Base;
    }
}
