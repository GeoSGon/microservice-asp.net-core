namespace productService.Utils.Validation;

public static class Validation
{
    public static bool IsInvalidId(int id) => id <= 0;

}