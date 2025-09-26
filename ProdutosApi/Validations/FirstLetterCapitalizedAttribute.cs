using System.ComponentModel.DataAnnotations;

namespace ProdutosApi.Validations
{
    public class FirstLetterCapitalizedAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            char primeiraLetra = value.ToString()[0];

            if (char.IsLower(primeiraLetra))
            {
                return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula");
            }

            return ValidationResult.Success;
        }
    }
}
