namespace CheckBoXIndexAPP.Modeli
{
    public class ObradaPdf
    {
        public string StariNaziv { get; set; }
        public string NoviNaziv { get; set; }
        public List<UnosNovaApp> Unosi { get; set; } = new List<UnosNovaApp>();
    }
}
