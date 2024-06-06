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
        public const string ChangeSerialNumber = $"{Base}/serialNumber/{{id:guid}}";
        public const string UpsertDepartment = $"{Base}/department/{{id:guid}}";
    }

    public static class Departments
    {
        private const string Base = $"{ApiBase}/departments";
        public const string CreateADepartment = Base;
        public const string GetAllDepartments = Base;
        public const string DeleteADepartmentByName = $"{Base}/{{name}}";
        public const string ChangeDepartmentName = $"{Base}/{{name}}";
    }

    public static class Accounts
    {
        private const string Base = $"{ApiBase}/accounts";
        public const string RegisterANewUserAndSendEmail = Base;
        public const string CompleteRegistrationFromEmail = $"{Base}/completeRegistration";
        public const string ResendSendEmailToRegister = $"{Base}/resendRegistration";
        public const string Login = $"{Base}/login";
        public const string ForgotPassword = $"{Base}/forgotPassword";
        public const string ChangePassword = $"{Base}/changePassword";
    }
}
