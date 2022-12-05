using System;
using System.Collections.Generic;

namespace Pelmenara_AUI_RUI;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<FavoriteRecipe> FavoriteRecipes { get; } = new List<FavoriteRecipe>();

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
}
