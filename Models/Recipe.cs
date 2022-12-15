using System;
using System.Collections.Generic;

namespace Pelmenara_AUI_RUI;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string Title { get; set; } = null!;

    //public int PhotoId { get; set; }

    public string Description { get; set; } = null!;

    public string Ingredients { get; set; } = null!;

    public string CookingTime { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public int OwnerId { get; set; }

    public virtual ICollection<FavoriteRecipe> FavoriteRecipes { get; } = new List<FavoriteRecipe>();

    public virtual User Owner { get; set; } = null!;

    //public virtual RecipePhoto Photo { get; set; } = null!;
}
