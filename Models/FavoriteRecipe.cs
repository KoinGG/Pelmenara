using System;
using System.Collections.Generic;

namespace Pelmenara_AUI_RUI;

public partial class FavoriteRecipe
{
    public int FavoriteRecipeId { get; set; }

    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
