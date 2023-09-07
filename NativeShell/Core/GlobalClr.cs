namespace NativeShell.Core
{
    public class GlobalClr


    {
        public GlobalClr()
        {
        }


        public Type? ResolveType(string typeName)
        {
            return Type.GetType(typeName);
        }

        public string Serialize(object obj)
        {
            if (obj is Exception ex)
            {
                return System.Text.Json.JsonSerializer.Serialize(new { error = ex.ToString() });
            }
            return System.Text.Json.JsonSerializer.Serialize(new { value = obj });
        }
    }
}
