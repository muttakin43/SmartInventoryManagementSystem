namespace SmartInventory.API
{
    public static class ApiEndpoint
    {

        private const string ApiBase = "api";


        public static class Product
        {
            private const string Base = $"{ApiBase}/product";



            public const string GetAll = Base;
            public const string Get = $"{Base}/{{productId}}";
            public const string Create = Base;
            public const string Update = $"{Base}/{{productId}}";
            public const string Delete = $"{Base}/{{productId}}";
        }

    }
}
