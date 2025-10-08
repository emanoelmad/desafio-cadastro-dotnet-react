using System.Text.RegularExpressions;

namespace CadastroPessoasApi.Validators
{
    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            var digits = Regex.Replace(cpf, "[^0-9]", string.Empty);

            if (digits.Length != 11) return false;

            
            var invalids = new[]
            {
                "00000000000","11111111111","22222222222","33333333333","44444444444",
                "55555555555","66666666666","77777777777","88888888888","99999999999"
            };
            if (invalids.Contains(digits)) return false;

            
            int[] numbers = digits.Select(c => c - '0').ToArray();

            int sum = 0;
            for (int i = 0; i < 9; i++) sum += numbers[i] * (10 - i);
            int result = sum % 11;
            int firstDigit = (result < 2) ? 0 : 11 - result;
            if (numbers[9] != firstDigit) return false;

            
            sum = 0;
            for (int i = 0; i < 10; i++) sum += numbers[i] * (11 - i);
            result = sum % 11;
            int secondDigit = (result < 2) ? 0 : 11 - result;
            if (numbers[10] != secondDigit) return false;

            return true;
        }
    }
}
