namespace SIMSAPI.Models
{
	public class Incident
	{
        public string Bearbeiter { get; set; }
		public string Melder { get; set; }

		public string Schweregrad { get; set; }
		public string Bearbeitungsstatus { get; set; }
		public string Cve { get; set; }
		public string Systembetroffenheit { get; set; }
		public string Beschreibung { get; set; }
		public DateTime Zeitstempel { get; set; }
		public int Eskalationslevel { get; set; }
	}
}
