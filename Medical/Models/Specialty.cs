namespace Medical.Models;

public class Specialty
{
    public int Id { get; set; }
    public string Slug { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string NameBn { get; set; } = "";
    public string Icon { get; set; } = "";

    public string DisplayName(bool bangla) => bangla ? NameBn : NameEn;
}