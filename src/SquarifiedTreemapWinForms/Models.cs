namespace SquarifiedTreemapWinForms;

public record AppSettings
{
    public bool IsAutoLoad { get; set; } = false;
    public string AutoLoadFilePath { get; init; } = "";
}
