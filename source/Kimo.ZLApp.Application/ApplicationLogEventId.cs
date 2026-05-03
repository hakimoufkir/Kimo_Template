namespace Kimo.ZLApp.Application;

public static class ApplicationLogEventIds // 1000
{
    public static class Pipelines // 1100 - 1499
    {
        public static class Behaviors
        {
            public static class AuthorizationBehavior // 1200 - 1299
            {
                // 00 - 49 Info/Trace/Debug
                public const int CheckingPermissionsForRequest = 1300;
                public const int NoAuthGuardsFound = 1301;
                public const int AuthorizationSuccess = 1302;

                // 50 - 74 Warning
                public const int UserIsNotAuthenticated = 1340;
                public const int UserDoesNotHavePermission = 1341;
                public const int UserDoesNotHavePermissions = 1342;

                // 75 - 99 Error/Fatal
            }

            public static class HandleExceptionBehavior // 1300 - 1399
            {
                public const int UnhandledExceptionWhileProcessingRequest = 1400;
            }

            public static class ValidationBehavior // 1400 - 1499
            {
                public const int NoValidatorsFound = 1200;
                public const int ValidationPassed = 1201;
                public const int ValidationFailed = 1202;
            }
        }
    }

    public static class Locations // 1500 - 2000
    {
        public static class Queries
        {
            public static class AllLocations // 1500 - 1599
            {
                public const int SuccessfullyLoaded = 1500;
            }
        }
    }

    public static class WeatherForecasts // 2000 - 2500
    {
        public static class Queries
        {
            public static class AllForecasts // 2000 - 2099
            {
                public const int SuccessfullyLoaded = 2000;
            }

            public static class SingleForecast // 2100 - 2199
            {
                public const int SuccessfullyLoaded = 2100;

                public const int NotFound = 2150;
            }
        }

        public static class Commands // 2500 - 3000
        {
            public static class CreateForecast // 2500 -2599
            {
                public const int CreateNewForecast = 2500;
                public const int AddingPrecipitation = 2501;
                public const int AddingTemperature = 2502;
                public const int SuccessfullyCreated = 2503;

                public const int LocationNotFound = 2550;
            }

            public static class DeleteForecast // 2600 -2699
            {
                public const int ForecastRemoved = 2600;
                public const int DeletedSuccessfully = 2601;

                public const int ForecastNotFound = 2650;
            }

            public static class DeleteForecastBeforeDate // 2700 -2799
            {
                public const int DeletedSuccessfully = 2600;
            }
        }
    }
}