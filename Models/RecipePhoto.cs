using System;
using System.Collections.Generic;

namespace Pelmenara_AUI_RUI;

public partial class RecipePhoto
{
    public int RecipePhotoId { get; set; }

    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();
}
