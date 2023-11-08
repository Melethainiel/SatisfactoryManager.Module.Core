using System.Globalization;
using GameDataParser.Models;

namespace SatisfactoryManager.Module.Core.Models.Json;

public class Recipe
{
    private Ingredient[]? _ingredients;
    private string[]? _produceIn;
    private Ingredient[]? _product;
    public string ClassName { get; set; } = null!;
    public string MDisplayName { get; set; } = null!;
    public string MProducedIn { get; set; } = null!;
    public string MProduct { get; set; } = null!;
    public string MIngredients { get; set; } = null!;
    public string MRelevantEvents { get; set; } = null!;
    public string MManufactoringDuration { get; set; } = null!;

    public string[] ProducedIn
    {
        get
        {
            if (_produceIn is not null) return _produceIn;
            if (string.IsNullOrEmpty(MProducedIn))
            {
                _produceIn = Array.Empty<string>();
                return _produceIn;
            }

            try
            {
                var produceIn = MProducedIn[1..^1]
                    .Split(',');
                for (var index = 0; index < produceIn.Length; index++)
                    produceIn[index] = produceIn[index]
                        .Split('.')
                        .Last();

                _produceIn = produceIn;
                return _produceIn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public int ManufactoringDuration
    {
        get
        {
            var duration = decimal.Parse(
                MManufactoringDuration,
                NumberStyles.Number,
                new NumberFormatInfo
                {
                    NumberDecimalSeparator = "."
                });

            return decimal.ToInt32(duration);
        }
    }

    public Ingredient[] Ingredients
    {
        get
        {
            if (_ingredients is not null) return _ingredients;

            if (string.IsNullOrEmpty(MIngredients))
            {
                _ingredients = Array.Empty<Ingredient>();
                return _ingredients;
            }

            try
            {
                var ingredients = MIngredients[1..^1]
                    .Split("),(");

                _ingredients = new Ingredient[ingredients.Length];
                for (var index = 0; index < ingredients.Length; index++)
                {
                    var ingredient = ingredients[index]
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(',');

                    var name = ingredient[0]
                        .Split('.')
                        .Last()
                        .Replace("\"", "")
                        .Replace("'", "");

                    var amount = int.Parse(
                        ingredient[1]
                            .Split('=')
                            .Last());


                    _ingredients[index] = new Ingredient
                    {
                        Name = name,
                        Amount = amount
                    };
                }

                return _ingredients;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public Ingredient[] Product
    {
        get
        {
            if (_product is not null) return _product;

            if (string.IsNullOrEmpty(MIngredients))
            {
                _product = Array.Empty<Ingredient>();
                return _product;
            }

            try
            {
                var ingredients = MProduct[1..^1]
                    .Split("),(");

                _product = new Ingredient[ingredients.Length];
                for (var index = 0; index < ingredients.Length; index++)
                {
                    var ingredient = ingredients[index]
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(',');

                    var name = ingredient[0]
                        .Split('.')
                        .Last()
                        .Replace("\"", "")
                        .Replace("'", "");

                    var amount = int.Parse(
                        ingredient[1]
                            .Split('=')
                            .Last());


                    _product[index] = new Ingredient
                    {
                        Name = name,
                        Amount = amount
                    };
                }

                return _product;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}