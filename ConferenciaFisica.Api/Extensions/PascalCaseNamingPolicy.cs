using System.Globalization;
using System.Text.Json;

namespace ConferenciaFisica.Api.Extensions
{
    public class PascalCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
        }
    }

}
